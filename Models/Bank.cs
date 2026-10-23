namespace AtmPatterns.Models;

/// <summary>Учебная модель банка: хранит остатки на счетах по номеру карты.</summary>
public class Bank
{
    private readonly Dictionary<string, decimal> _balances = new();

    public void OpenAccount(Card card, decimal balance) => _balances[card.Number] = balance;

    public decimal GetBalance(Card card) => _balances[card.Number];

    public bool TryWithdraw(Card card, decimal amount)
    {
        if (!_balances.TryGetValue(card.Number, out var balance) || balance < amount)
            return false;
        _balances[card.Number] = balance - amount;
        return true;
    }
}
