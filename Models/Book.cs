namespace publicLibrary.Models;

public class Book : Content
{
    // Specific properties to book:
    public int Id { get; set; }
    public string Author { get; set; }
    public string Code { get; set; }
    public int Stock { get; set; }
    public DateOnly Released { get; set; }
    
    // Relation 1:N with the other table (LoanDetails):
    public ICollection<LoanDetails> LoansDetails { get; set; } = new List<LoanDetails>();
}