using AtmPatterns.Models;

namespace AtmPatterns.Patterns.State;

/// <summary>Контекст паттерна State: делегирует все действия текущему состоянию.</summary>
public class Atm
{
    private IAtmState _state;

    public Bank Bank { get; }
    public CashDispenser Dispenser { get; }
    public Card? Card { get; set; }
    public int FailedAttempts { get; set; }
    public int MaxAttempts { get; }

    public Atm(Bank bank, CashDispenser dispenser, int maxAttempts = 3)
    {
        Bank = bank;
        Dispenser = dispenser;
        MaxAttempts = maxAttempts;
        _state = new IdleState(this);
    }

    public string StateName => _state.Name;

    public void SetState(IAtmState state)
    {
        Console.WriteLine($"  >> Состояние: «{_state.Name}» -> «{state.Name}»");
        _state = state;
    }

    public void EjectCard()
    {
        Console.WriteLine("  Карта возвращена.");
        Card = null;
        FailedAttempts = 0;
        SetState(new IdleState(this));
    }

    // Действия внешнего мира (карта и клавиатура) просто передаются состоянию.
    public void InsertCard(Card card) { Console.WriteLine($"[Действие] вставить карту {card.Number}"); _state.InsertCard(card); }
    public void PressDigit(char digit) { Console.WriteLine($"[Действие] клавиша «{digit}»"); _state.PressDigit(digit); }
    public void PressEnter() { Console.WriteLine("[Действие] клавиша «Ввод»"); _state.PressEnter(); }
    public void PressCancel() { Console.WriteLine("[Действие] клавиша «Отмена»"); _state.PressCancel(); }

    public void TypeDigits(string digits) { foreach (var d in digits) PressDigit(d); }
}
