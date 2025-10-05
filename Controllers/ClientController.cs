using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using publicLibrary.Data; // Asegúrate de que este sea el namespace de tu DbContext
using publicLibrary.Models; // Asegúrate de que este sea el namespace de tu modelo Client

namespace publicLibrary.Controllers;

public class ClientController : Controller
{
    private readonly AppDbContext _context;

    // Inyección de Dependencias del AppDbContext
    public ClientController(AppDbContext context)
    {
        _context = context;
    }

    
    
    
    // -----------------------------------------------------------------
    // READ ALL: GET /Client
    public async Task<IActionResult> Index()
    {
        // Trae todos los clientes de la base de datos
        var clients = await _context.clients.ToListAsync();
        return View(clients);
    }

    
    
    
    // -----------------------------------------------------------------
    // READ ONE: GET /Client/Details/5
    public async Task<IActionResult> Details(int id)
    {
        // Busca un cliente por su Id
        var client = await _context.clients
            .FirstOrDefaultAsync(c => c.Id == id);

        if (client == null)
            return NotFound();
            
        return View(client);
    }

    
    
    
    // -----------------------------------------------------------------
    // CREATE (GET): GET /Client/Create
    // It shows the form for a new register
    public IActionResult Create()
    {
        return View();
    }
    
    
    
    
    
    // -----------------------------------------------------------------
    // CREATE (POST): POST /Client/Create
    // Save the new client on the DB
    
    [HttpPost]
    [ValidateAntiForgeryToken]      // Práctica de seguridad recomendada
    public async Task<IActionResult> Create([Bind("Name,DocumentNumb,Age,Email,Status,Phone")]Client client)
    {
        if (ModelState.IsValid)
        {
            // Agrega el cliente al contexto
            _context.clients.Add(client);
            
            // Guarda los cambios en la DB (INSERT)
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }
        return View(client);
    }

    
    
    // -----------------------------------------------------------------
    // UPDATE (GET): GET /Client/Edit/5
    // Shows the form with the data to edit
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var client = await _context.clients.FindAsync(id);
        
        if (client == null)
            return NotFound();
            
        return View(client);
    }

    
    
    
    
    // -----------------------------------------------------------------
    // UPDATE (POST): POST /Client/Edit/5
    // Update the client on the DB
    [HttpPost]
    [ActionName("Edit")] 
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPost(int id)
    {
        // 1. Busca el objeto existente en la DB que será rastreado por EF Core
        var clientToUpdate = await _context.clients.FindAsync(id);

        if (clientToUpdate == null) return NotFound();

        // 2. Aplica los valores del formulario al modelo rastreado
        // Se recomienda usar el atributo [Bind] en el método GET para listar solo las propiedades que se desean editar
        if (await TryUpdateModelAsync<Client>(
                clientToUpdate,
                "", // Prefijo (vacío si no hay prefijo en el formulario)
                c => c.Name, c => c.DocumentNumb, c => c.Age, c => c.Email, c => c.Status, c => c.Phone))
        {
            try
            {
                // Solo guardará los campos que fueron modificados
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                // Manejo de errores de concurrencia
                if (!_context.clients.Any(c => c.Id == id))
                {
                    return NotFound();
                }
                throw;
            }
        }
        // Si el modelo no es válido o TryUpdateModelAsync falla
        return View(clientToUpdate);
    }

    
    
    // -----------------------------------------------------------------
    // DELETE (POST): POST /Client/DeleteConfirmed/5
    // Elimina el cliente de la DB
    // -----------------------------------------------------------------
    // Por convención, es mejor usar dos acciones: Delete (GET para confirmar) y DeleteConfirmed (POST para ejecutar)
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        // Busca el cliente
        var client = await _context.clients.FindAsync(id);
        
        if (client != null)
        {
            // Marca para eliminación y guarda cambios
            _context.clients.Remove(client);
            await _context.SaveChangesAsync(); // (DELETE)
        }
        
        return RedirectToAction(nameof(Index));
    }

}