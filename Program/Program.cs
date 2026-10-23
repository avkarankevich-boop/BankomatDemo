namespace AtmPatterns.Program;

public static class Program
{
    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("Банкомат: демонстрация паттернов State и Chain of Responsibility");
        Demo.Run();
    }
}
