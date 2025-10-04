namespace publicLibrary.Models;

public class Loan
{
    public int Id { get; set; }
    
    // FK a client
    public int Client_ID { get; set; }
    public Client Client;               // To stablish relationship between Loan - Client 
    
    // FK loanDetails
    public int LoanDetails_ID { get; set; }
    public LoanDetails LoanDetails;     // To stablish relationship between Loan -LoanDetails 

}