namespace AtmPatterns.Patterns.State;

/// <summary>State: общий интерфейс состояний банкомата. Каждое действие клавиатуры/приёмника карты
/// обрабатывается по-своему в зависимости от текущего состояния.</summary>
public interface IAtmState
{
    string Name { get; }
    void InsertCard(Models.Card card);
    void PressDigit(char digit);
    void PressEnter();
    void PressCancel();
}
