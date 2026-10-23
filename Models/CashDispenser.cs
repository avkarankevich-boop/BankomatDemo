using AtmPatterns.Patterns.Chain;

namespace AtmPatterns.Models;

/// <summary>Кассетный модуль: собирает цепочку обработчиков от крупного номинала к мелкому.</summary>
public class CashDispenser
{
    private readonly BanknoteHandler _first;

    public CashDispenser(int n100, int n50, int n20, int n10, int n5)
    {
        _first = new Note100Handler(n100);
        _first.SetNext(new Note50Handler(n50))
              .SetNext(new Note20Handler(n20))
              .SetNext(new Note10Handler(n10))
              .SetNext(new Note5Handler(n5));
    }

    /// <summary>Пробует выдать сумму. При успехе списывает купюры и возвращает результат, иначе null.</summary>
    public DispenseResult? TryDispense(int amount)
    {
        var result = new DispenseResult(amount);
        _first.Handle(result);
        if (!result.IsComplete)
            return null; // запасов не хватает или сумма не набирается — ничего не списано
        _first.Commit(result);
        return result;
    }

    public void PrintStock()
    {
        Console.WriteLine("  Остаток купюр в банкомате:");
        _first.PrintStock();
    }
}
