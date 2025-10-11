using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace publicLibrary.Models;

public class Loan
{
    public int Id { get; set; }
    
    // FK a client
    [ForeignKey("Client")]
    public int ClientId { get; set; }
    [ValidateNever]
    public Client Client { get; set; }              // To stablish relationship between Loan - Client 
    
    //FK a Book
    [ForeignKey("Book")]
    public int BookId { get; set; }
    [ValidateNever]
    public Book Book { get; set; }                // To stablish relationship between Loan - Book 
    
    public DateTime DevolutionDate { get; set; }
    
    public int Amount { get; set; }

}