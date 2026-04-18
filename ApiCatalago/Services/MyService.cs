namespace ApiCatalago.Services;

public class MyService : IMyService
{
    public string Talk(string name)
    {
        return $"Bem vindo, {name} \n\n {DateTime.UtcNow}";
    }
}