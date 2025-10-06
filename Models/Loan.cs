using System.ComponentModel.DataAnnotations.Schema;

namespace publicLibrary.Models;

public class Loan
{
    public int Id { get; set; }
    
    // FK a client
    // [ForeignKey("Client")]
    public int Client_Id { get; set; }
    public Client Client { get; set; }              // To stablish relationship between Loan - Client 
    
    //FK a Book
    // [ForeignKey("Book")]
    public int Book_Id { get; set; }
    public Book Book { get; set; }                // To stablish relationship between Loan - Book 
    
    public DateOnly DevolutionDate { get; set; }
    
    public int Amount { get; set; }

}