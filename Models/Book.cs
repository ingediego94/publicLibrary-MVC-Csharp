namespace publicLibrary.Models;

public class Book : Content
{
    // Specific properties to book:
    public int Id { get; set; }
    public string Author { get; set; }
    public string Code { get; set; }
    public int Stock { get; set; }
    public DateTime Released { get; set; }
    public string Title { get; set; }
    // Relation 1:N with the other table (Loan):
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}