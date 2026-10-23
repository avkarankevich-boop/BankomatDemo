using AtmPatterns.Models;

namespace AtmPatterns.Patterns.State;

/// <summary>Ожидание карты.</summary>
public class IdleState : AtmState
{
    public IdleState(Atm atm) : base(atm) { }
    public override string Name => "ожидание карты";

    public override void InsertCard(Card card)
    {
        Atm.Card = card;
        Console.WriteLine($"  Карта {card.Number} принята.");
        Atm.SetState(new CardInsertedState(Atm));
    }
}

/// <summary>Карта вставлена: ждём подтверждения операции «Снять наличные» (кнопка Ввод).</summary>
public class CardInsertedState : AtmState
{
    public CardInsertedState(Atm atm) : base(atm) { }
    public override string Name => "карта вставлена";

    public override void PressEnter()
    {
        Console.WriteLine("  Выбрана операция «Снять наличные». Введите PIN.");
        Atm.SetState(new PinEntryState(Atm));
    }

    public override void PressCancel()
    {
        Console.WriteLine("  Операция отменена.");
        Atm.EjectCard();
    }
}

/// <summary>Ввод PIN: цифры накапливаются, Ввод проверяет, три неверные попытки — блокировка.</summary>
public class PinEntryState : AtmState
{
    private string _entered = "";
    public PinEntryState(Atm atm) : base(atm) { }
    public override string Name => "ввод PIN";

    public override void PressDigit(char digit)
    {
        if (!char.IsDigit(digit) || _entered.Length >= 4) return;
        _entered += digit;
        Console.WriteLine($"  PIN: {new string('*', _entered.Length)}");
    }

    public override void PressEnter()
    {
        if (_entered == Atm.Card!.Pin)
        {
            Console.WriteLine("  PIN верный.");
            Atm.FailedAttempts = 0;
            Atm.SetState(new DispensingState(Atm));
            return;
        }

        Atm.FailedAttempts++;
        _entered = "";
        Console.WriteLine($"  Неверный PIN. Попытка {Atm.FailedAttempts} из {Atm.MaxAttempts}.");
        if (Atm.FailedAttempts >= Atm.MaxAttempts)
        {
            Console.WriteLine("  Карта заблокирована и изъята.");
            Atm.SetState(new BlockedState(Atm));
        }
    }

    public override void PressCancel()
    {
        Console.WriteLine("  Ввод PIN отменён.");
        Atm.EjectCard();
    }
}

/// <summary>Выдача наличных: вводится сумма, Ввод запускает цепочку выдачи купюр.</summary>
public class DispensingState : AtmState
{
    private string _amount = "";
    public DispensingState(Atm atm) : base(atm) { }
    public override string Name => "выдача наличных";

    public override void PressDigit(char digit)
    {
        if (!char.IsDigit(digit) || _amount.Length >= 6) return;
        _amount += digit;
        Console.WriteLine($"  Сумма: {_amount}");
    }

    public override void PressEnter()
    {
        if (_amount.Length == 0)
        {
            Console.WriteLine("  Введите сумму.");
            return;
        }

        int amount = int.Parse(_amount);
        _amount = "";
        Console.WriteLine($"  Запрошена сумма {amount}. Запрос идёт по цепочке номиналов:");

        if (Atm.Bank.GetBalance(Atm.Card!) < amount)
        {
            Console.WriteLine("  Недостаточно средств на счёте.");
            return;
        }

        var result = Atm.Dispenser.TryDispense(amount);
        if (result == null)
        {
            Console.WriteLine("  Банкомат не может выдать эту сумму имеющимися купюрами. Введите другую.");
            return;
        }

        Atm.Bank.TryWithdraw(Atm.Card!, amount);
        Console.WriteLine($"  Выдано: {result} (итого {amount}). Заберите деньги.");
        Atm.EjectCard();
    }

    public override void PressCancel()
    {
        Console.WriteLine("  Операция отменена.");
        Atm.EjectCard();
    }
}

/// <summary>Блокировка: все действия игнорируются, пока не вмешается обслуживание.</summary>
public class BlockedState : AtmState
{
    public BlockedState(Atm atm) : base(atm) { }
    public override string Name => "блокировка";

    public override void InsertCard(Card card) =>
        Console.WriteLine("  Банкомат заблокирован, карты не принимаются. Обратитесь в банк.");
    public override void PressDigit(char digit) => Console.WriteLine("  Банкомат заблокирован.");
    public override void PressEnter() => Console.WriteLine("  Банкомат заблокирован.");
    public override void PressCancel() => Console.WriteLine("  Банкомат заблокирован.");
}
