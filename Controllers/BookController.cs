using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using publicLibrary.Data;
using publicLibrary.Models;

namespace publicLibrary.Controllers;

public class BookController : Controller
{
    private readonly AppDbContext _context;
    
    // Injection of Dependecies from AppDbContext
    public BookController(AppDbContext context)
    {
        _context = context;
    }
    
    
    // -----------------------------------------------------------------
    // READ ALL: GET /Book
    public async Task<IActionResult> Index()
    {
        // Get all books from DB
        var books = await _context.books.ToListAsync();
        return View(books);
    }
    
    
    
    
    // -----------------------------------------------------------------
    // READ ONE: GET /Book/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var book = await _context.books
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book == null)
            return NotFound();

        return View(book);
    }
    
    
    
    
    
    // -----------------------------------------------------------------
    // CREATE (GET): GET /Book/Create
    // It shows the form for a new register
    public IActionResult Create()
    {
        return View();
    }

    
    
    
    
    // -----------------------------------------------------------------
    // CREATE (POST): POST /Book/Create
    // Save the new book on the DB

    [HttpPost]
    [ValidateAntiForgeryToken] // It is a good safety practice. It's like a double validation.
    public async Task<IActionResult> Create([Bind("Title,Author,Genre,Released,Stock,Code,PublishingHouse")] Book book)
    {
        if (ModelState.IsValid)
        {
            // Add the new book to the context
            _context.books.Add(book);
            
            // It saves the changes on the DB (INSERT)
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(book);
    }
    
    
    
    
    
    // -----------------------------------------------------------------
    // UPDATE (GET): GET /Book/Edit/5
    // Shows the form with the data to edit
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var book = await _context.books.FindAsync(id);
        
        if (book == null)
            return NotFound();
            
        return View(book);
    }

    
    
    
    
    
    // -----------------------------------------------------------------
    // UPDATE (POST): POST /Book/Edit/5
    // Update the book on the DB
    [HttpPost]
    [ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPost(int id)
    {
        // 1. It searches the object on the DB that it will be tracked by EF Core
        var bookToUpdate = await _context.books.FindAsync(id);

        if (bookToUpdate == null)
            return NotFound();
        
        // 2. It applies the form values to the tracked model
        //  It is recomended to use the attribute [Bind] on the GET method to list  only the properties that you want edit.
        if (await TryUpdateModelAsync<Book>(
                bookToUpdate,
                "", // Prefix (empty if there isn't prefix on the form)
                b => b.Title,
                b => b.Author,
                b => b.Genre,
                b => b.Released,
                b => b.Stock,
                b => b.Code,
                b => b.PublishingHouse
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
                if (!_context.books.Any(b => b.Id == id))
                {
                    return NotFound();
                }
                throw;
            }
        }
        // If the model it not valid or TryUpdateModelAsync fails.
        return View(bookToUpdate);
    }
    
    
    
    
    
    
    // -----------------------------------------------------------------
    // DELETE (POST): POST /Client/DeleteConfirmed/5
    // It deletes the client of DB
    // By convention, it's better using two actions: Delete (GET to confirm) and DeleteConfirmed (POST to execute)
    
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        // It searches the book by ID
        var book = await _context.books.FindAsync(id);
        
        if (book != null)
        {
            // Mark for deletion and save changes. 
            _context.books.Remove(book);
            await _context.SaveChangesAsync(); // (DELETE)
        }
        
        return RedirectToAction(nameof(Index));
    }

    
    
    
}