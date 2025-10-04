namespace publicLibrary.Models;

public interface ICrud
{
    void Create();
    
    void Update();

    void Read();

    void Delete();
}