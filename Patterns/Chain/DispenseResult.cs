namespace AtmPatterns.Patterns.Chain;

/// <summary>Результат прохода запроса по цепочке: сколько купюр каждого номинала выдано и что осталось.</summary>
public class DispenseResult
{
    public int Remaining { get; set; }
    public Dictionary<int, int> Notes { get; } = new();
    public bool IsComplete => Remaining == 0;

    public DispenseResult(int amount) => Remaining = amount;

    public override string ToString() =>
        string.Join(", ", Notes.Where(n => n.Value > 0).Select(n => $"{n.Value} x {n.Key}"));
}
