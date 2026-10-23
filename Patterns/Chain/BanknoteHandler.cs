namespace AtmPatterns.Patterns.Chain;

/// <summary>
/// Chain of Responsibility: звено цепочки, отвечающее за один номинал.
/// Берёт сколько может (с учётом остатка купюр в кассете) и передаёт остаток дальше.
/// Расчёт не меняет запасы — запасы списываются отдельно (Commit), когда вся сумма набрана.
/// </summary>
public abstract class BanknoteHandler
{
    private BanknoteHandler? _next;

    public int Denomination { get; }
    public int Stock { get; private set; }

    protected BanknoteHandler(int denomination, int stock)
    {
        Denomination = denomination;
        Stock = stock;
    }

    public BanknoteHandler SetNext(BanknoteHandler next)
    {
        _next = next;
        return next;
    }

    public void Handle(DispenseResult result)
    {
        int count = Math.Min(result.Remaining / Denomination, Stock);
        if (count > 0)
        {
            result.Notes[Denomination] = count;
            result.Remaining -= count * Denomination;
            Console.WriteLine($"    [{Denomination}] беру {count} шт., осталось выдать {result.Remaining}");
        }
        else
        {
            Console.WriteLine($"    [{Denomination}] не могу выдать часть суммы {result.Remaining}, передаю дальше");
        }

        if (result.Remaining > 0)
            _next?.Handle(result);
    }

    /// <summary>Списывает выданные купюры из кассеты по всей цепочке.</summary>
    public void Commit(DispenseResult result)
    {
        if (result.Notes.TryGetValue(Denomination, out var count))
            Stock -= count;
        _next?.Commit(result);
    }

    public void PrintStock()
    {
        Console.WriteLine($"    номинал {Denomination}: {Stock} шт.");
        _next?.PrintStock();
    }
}

public class Note100Handler : BanknoteHandler { public Note100Handler(int stock) : base(100, stock) { } }
public class Note50Handler : BanknoteHandler { public Note50Handler(int stock) : base(50, stock) { } }
public class Note20Handler : BanknoteHandler { public Note20Handler(int stock) : base(20, stock) { } }
public class Note10Handler : BanknoteHandler { public Note10Handler(int stock) : base(10, stock) { } }
public class Note5Handler : BanknoteHandler { public Note5Handler(int stock) : base(5, stock) { } }
