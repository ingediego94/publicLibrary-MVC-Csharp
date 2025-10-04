namespace publicLibrary.Models;

public class LoanDetails
{
    public int Id { get; set; }
    
    //FK a Book
    public int Book_ID { get; set; }
    public Book Book;               // To stablish relationship between LoanDetails - Book 

    // Relation 1:N with the other table, (Loan)
    public ICollection<Loan> Loans_d { get; set; } = new List<Loan>(); 
    
    
    public DateOnly DevolutionDate { get; set; }
    public int Amount { get; set; }
    
}