using AtmPatterns.Models;
using AtmPatterns.Patterns.State;

namespace AtmPatterns.Program;

/// <summary>Демо-стенд: прогоняет все сценарии работы паттернов State и Chain of Responsibility.</summary>
public static class Demo
{
    public static void Run()
    {
        Scenario1_SuccessfulWithdrawal();
        Scenario2_NotWithdrawableAmount();
        Scenario3_WrongPinBlocks();
    }

    private static Atm CreateAtm(Bank bank) => new(bank, new CashDispenser(n100: 5, n50: 5, n20: 5, n10: 5, n5: 0));

    private static void Title(string text)
    {
        Console.WriteLine();
        Console.WriteLine($"=== {text} ===");
    }

    private static void Scenario1_SuccessfulWithdrawal()
    {
        Title("Сценарий 1: карта -> PIN -> выдача 380");
        var bank = new Bank();
        var card = new Card("4000-0001", "1234");
        bank.OpenAccount(card, 1000);
        var atm = CreateAtm(bank);

        atm.InsertCard(card);
        atm.PressEnter();
        atm.TypeDigits("1234");
        atm.PressEnter();
        atm.TypeDigits("380");
        atm.PressEnter();
        Console.WriteLine($"  Остаток на счёте: {bank.GetBalance(card)}");
        atm.Dispenser.PrintStock();
    }

    private static void Scenario2_NotWithdrawableAmount()
    {
        Title("Сценарий 2: сумма, которую нельзя набрать, затем корректная");
        var bank = new Bank();
        var card = new Card("4000-0002", "4321");
        bank.OpenAccount(card, 500);
        var atm = CreateAtm(bank);

        atm.InsertCard(card);
        atm.PressEnter();
        atm.TypeDigits("4321");
        atm.PressEnter();
        atm.TypeDigits("33");   // 5-рублёвых купюр нет, кратно 10 — не выйдет
        atm.PressEnter();
        atm.TypeDigits("70");
        atm.PressEnter();
    }

    private static void Scenario3_WrongPinBlocks()
    {
        Title("Сценарий 3: неверный PIN три раза -> блокировка");
        var bank = new Bank();
        var card = new Card("4000-0003", "1111");
        bank.OpenAccount(card, 500);
        var atm = CreateAtm(bank);

        atm.InsertCard(card);
        atm.PressEnter();
        for (int i = 0; i < 3; i++)
        {
            atm.TypeDigits("9999");
            atm.PressEnter();
        }
        atm.PressDigit('1');                 // в блокировке клавиатура не работает
        atm.InsertCard(new Card("4000-0004", "0000"));
        Console.WriteLine($"  Итоговое состояние: «{atm.StateName}»");
    }
}
