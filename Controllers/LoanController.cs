
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using publicLibrary.Data;
using publicLibrary.Models;

namespace publicLibrary.Controllers;

public class LoanController : Controller
{
    private readonly AppDbContext _context;

    // Injection of Dependencies from AppDbContext
    public LoanController(AppDbContext context)
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
                Id = b.Id,              // We use Id to the option value 
                Title = b.Title         // We use Title for the visible text
            })
            .OrderBy(b => b.Title);
        
        ViewData["BookId"] = new SelectList(await booksQuery.ToListAsync(), "Id", "Title", selectedBook);
    }
    
    
    
    
    // -------------------------------------------------
    // Helper method to populate the Clients dropdown list
    private async Task ListOfClientsDropdown(object selectedClient = null)
    {
        var clientsQuery = _context.clients

            .Select(cl => new 
            { 
                Id = cl.Id,              // We use Id to the option value 
                Name = cl.Name         // We use Title for the visible text
            })
            .OrderBy(cl => cl.Name);
        
        ViewData["ClientId"] = new SelectList(await clientsQuery.ToListAsync(), "Id", "Name", selectedClient);
    }
    
    
    
    // -----------------------------------------------------------------
    // READ ALL: GET /Loan
    public async Task<IActionResult> Index()
    {
        
        // ADD: Loading books list before to return view
        await PopulateBooksDropDownList();
        await ListOfClientsDropdown();
        
        // Get all LoanDetails from the DB
        var loans_x = await _context.loans
            .Include(l => l.Book)
            .Include(l => l.Client)
            .ToListAsync();
        
        return View(loans_x);
    }
    
    
    
    
    
    // -----------------------------------------------------------------
    // READ ONE: GET /Loan/Details/5
    public async Task<IActionResult> Details(int id)
    {
        // Search the loanDetails by its Id
        var loans_x = await _context.loans
            .FirstOrDefaultAsync(l => l.Id == id);

        if (loans_x == null)
            return NotFound();
            
        return View(loans_x);
    }
    
    
    
    
    
        
    // -----------------------------------------------------------------
    // CREATE (GET): GET /Loan/Create
    // It shows the form for a new register
    // public async Task<IActionResult> Create()
    // {
    //     await PopulateBooksDropDownList();
    //     await ListOfClientsDropdown();
    //     
    //     return View();
    // }

    
    
    
    
    // -----------------------------------------------------------------
    // CREATE (POST): POST /Loan/Create
    // Save the new Loan on the DB

    [HttpPost]
    [ValidateAntiForgeryToken] // It is a good safety practice. It's like a double validation.
    public async Task<IActionResult> Create([Bind("ClientId,BookId,DevolutionDate,Amount")] Loan loan)
    {
        if (ModelState.IsValid)
        {
            // Add the new loandDetails to the context
            _context.loans.Add(loan);

            // Saves the changes on the DB (INSERT)
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        await PopulateBooksDropDownList(loan.BookId);
        await ListOfClientsDropdown(loan.ClientId);

        // 2. Obtener la lista de préstamos existentes para renderizar la tabla
        var loans_x = await _context.loans
            .Include(l => l.Book)
            .Include(l => l.Client)
            .ToListAsync();
        
    return View("Index", loans_x);
    }
    
    
    
    
    
    
    // -----------------------------------------------------------------
    // UPDATE (GET): GET /Loan/Edit/5
    // Shows the form with the data to edit
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var loans_x = await _context.loans.FindAsync(id);
        
        if (loans_x == null)
            return NotFound();
        
        await PopulateBooksDropDownList(loans_x.BookId);
        
        return View(loans_x);
    }
    
    
    
    
    
    
    // -----------------------------------------------------------------
    // UPDATE (POST): POST /Loan/Edit/5
    // Update the client on the DB
    
    [HttpPost]
    [ActionName("Edit")] 
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPost(int id)
    {
        // 1. It searches the object on the DB that it will be tracked by EF Core
        var loanToUpdate = await _context.loans.FindAsync(id);

        if (loanToUpdate == null) 
            return NotFound();

        // 2. It applies the form values to the tracked model
        //  It is recomended to use the attribute [Bind] on the GET method to list  only the properties that you want edit.
        if (await TryUpdateModelAsync<Loan>(
                loanToUpdate,
                "",         // Prefix (empty if there isn't prefix on the form)
                l => l.BookId, 
                l => l.DevolutionDate, 
                l => l.Amount
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
                if (!_context.loans.Any(lds => lds.Id == id))
                {
                    return NotFound();
                }
                throw;
            }
        }
        // If the model it not valid or TryUpdateModelAsync fails.
        await PopulateBooksDropDownList(loanToUpdate.BookId); 
        return View(loanToUpdate);
    }
    
    
    
    
    
    // -----------------------------------------------------------------
    // DELETE (POST): POST /Loan/DeleteConfirmed/5
    // It deletes the LoanDetail of DB
    // By convention, it's better using two actions: Delete (GET to confirm) and DeleteConfirmed (POST to execute)
    
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        // It searches the LoanDetail by ID
        var loan_x = await _context.loans.FindAsync(id);
        
        if (loan_x != null)
        {
            // Mark for deletion and save changes. 
            _context.loans.Remove(loan_x);
            await _context.SaveChangesAsync();          // (DELETE)
        }
        
        return RedirectToAction(nameof(Index));
    }
    
}
