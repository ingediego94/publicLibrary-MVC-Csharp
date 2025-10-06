namespace publicLibrary.Models;

public class Client : Person
{
    // Specific properties to client:
    public int Id { get; set; }
    public string Email { get; set; }
    public bool Status { get; set; }
    public string Phone { get; set; }
    public string Name { get; set; }
    // Relation 1:N with the other table (Loans):
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}