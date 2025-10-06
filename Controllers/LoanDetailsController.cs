using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using publicLibrary.Data;
using publicLibrary.Models;

namespace publicLibrary.Controllers;

public class LoanDetailsController : Controller
{
    private readonly AppDbContext _context;

    // Injection of Dependencies from AppDbContext
    public LoanDetailsController(AppDbContext context)
    {
        _context = context;
    }
    
    
    
    // -------------------------------------------------
    // Helper method to populate the Book dropdown list
    private async Task PopulateBooksDropDownList(object selectedBook = null)
    {
        var booksQuery = _context.books

        .Select(b => new 
            { 
                Id = b.Id, // Usamos el Id para el valor de la opción
                Title = b.Title // Usamos Title para el texto visible
            })
            .OrderBy(b => b.Title);
        
        ViewData["BookId"] = new SelectList(await booksQuery.ToListAsync(), "Id", "Title", selectedBook);
    }
    
    
    
    // -----------------------------------------------------------------
    // READ ALL: GET /LoanDetails
    public async Task<IActionResult> Index()
    {
        
        // AÑADIR: Cargar la lista de libros antes de retornar la vista
        await PopulateBooksDropDownList(); 
        
        // Get all LoanDetails from the DB
        var loanDetails_x = await _context.loansDetails.ToListAsync();
        return View(loanDetails_x);
    }
    
    
    
    
    
    // -----------------------------------------------------------------
    // READ ONE: GET /LoanDetails/Details/5
    public async Task<IActionResult> Details(int id)
    {
        // Search the loanDetails by its Id
        var loanDetails_x = await _context.loansDetails
            .FirstOrDefaultAsync(ld => ld.Id == id);

        if (loanDetails_x == null)
            return NotFound();
            
        return View(loanDetails_x);
    }
    
    
    
    
    
        
    // -----------------------------------------------------------------
    // CREATE (GET): GET /LoanDetails/Create
    // It shows the form for a new register
    public async Task<IActionResult> Create()
    {
        await PopulateBooksDropDownList();
        return View();
    }

    
    
    
    
    // -----------------------------------------------------------------
    // CREATE (POST): POST /LoanDetails/Create
    // Save the new LoanDetail on the DB
    
    [HttpPost]
    [ValidateAntiForgeryToken]      // It is a good safety practice. It's like a double validation.
    public async Task<IActionResult> Create([Bind("Book_Id,DevolutionDate,Amount")]LoanDetails loansDetails_y)
    {
        if (ModelState.IsValid)
        {
            // Add the new loandDetails to the context
            _context.loansDetails.Add(loansDetails_y);
            
            // Saves the changes on the DB (INSERT)
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }
        
        await PopulateBooksDropDownList(loansDetails_y.Book_Id);
        return View(loansDetails_y);
    }
    
    
    
    
    
    
    // -----------------------------------------------------------------
    // UPDATE (GET): GET /LoanDetails/Edit/5
    // Shows the form with the data to edit
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var loansDetails_y = await _context.loansDetails.FindAsync(id);
        
        if (loansDetails_y == null)
            return NotFound();
        
        await PopulateBooksDropDownList(loansDetails_y.Book_Id);
        return View(loansDetails_y);
    }
    
    
    
    
    
    
    // -----------------------------------------------------------------
    // UPDATE (POST): POST /LoanDetail/Edit/5
    // Update the client on the DB
    [HttpPost]
    [ActionName("Edit")] 
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPost(int id)
    {
        // 1. It searches the object on the DB that it will be tracked by EF Core
        var loanDetailToUpdate = await _context.loansDetails.FindAsync(id);

        if (loanDetailToUpdate == null) 
            return NotFound();

        // 2. It applies the form values to the tracked model
        //  It is recomended to use the attribute [Bind] on the GET method to list  only the properties that you want edit.
        if (await TryUpdateModelAsync<LoanDetails>(
                loanDetailToUpdate,
                "", // Prefix (empty if there isn't prefix on the form)
                lds => lds.Book_Id, 
                lds => lds.DevolutionDate, 
                lds => lds.Amount
            )
           )
        {
            try
            {
                // Only saves fields that were modified
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handled of errors of concurrency 
                if (!_context.loansDetails.Any(lds => lds.Id == id))
                {
                    return NotFound();
                }
                throw;
            }
        }
        // If the model it not valid or TryUpdateModelAsync fails.
        await PopulateBooksDropDownList(loanDetailToUpdate.Book_Id); 
        return View(loanDetailToUpdate);
    }
    
    
    
    
    
    // -----------------------------------------------------------------
    // DELETE (POST): POST /LoanDetails/DeleteConfirmed/5
    // It deletes the LoanDetail of DB
    // By convention, it's better using two actions: Delete (GET to confirm) and DeleteConfirmed (POST to execute)
    
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        // It searches the LoanDetail by ID
        var loanDetails_y = await _context.loansDetails.FindAsync(id);
        
        if (loanDetails_y != null)
        {
            // Mark for deletion and save changes. 
            _context.loansDetails.Remove(loanDetails_y);
            await _context.SaveChangesAsync(); // (DELETE)
        }
        
        return RedirectToAction(nameof(Index));
    }
    
}