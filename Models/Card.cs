namespace AtmPatterns.Models;

/// <summary>Банковская карта: номер и PIN (в реальности PIN проверяет банк, здесь — упрощённо).</summary>
public class Card
{
    public string Number { get; }
    public string Pin { get; }

    public Card(string number, string pin)
    {
        Number = number;
        Pin = pin;
    }
}
