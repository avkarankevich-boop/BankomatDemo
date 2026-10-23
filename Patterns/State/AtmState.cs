namespace AtmPatterns.Patterns.State;

/// <summary>Базовый класс: по умолчанию любое действие недопустимо в данном состоянии.</summary>
public abstract class AtmState : IAtmState
{
    protected readonly Atm Atm;
    protected AtmState(Atm atm) => Atm = atm;

    public abstract string Name { get; }

    public virtual void InsertCard(Models.Card card) =>
        Console.WriteLine($"  ! В состоянии «{Name}» вставка карты невозможна");

    public virtual void PressDigit(char digit) =>
        Console.WriteLine($"  ! В состоянии «{Name}» цифры не принимаются");

    public virtual void PressEnter() =>
        Console.WriteLine($"  ! В состоянии «{Name}» кнопка «Ввод» ничего не делает");

    public virtual void PressCancel() =>
        Console.WriteLine($"  ! В состоянии «{Name}» кнопка «Отмена» ничего не делает");
}
