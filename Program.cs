using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    //TODO error handling for input of string instead of int in belopp (example: "200kr" - ERROR)
     
    //TODO filters in listbalance syntax list balance sort_by="value" sort="highest", list balance sort_by="value" sort="lowest"
    //TODO filter by category
    //TODO filter by date - last month, last week, last year
    //TODO filter by amount less / most

    //TODO add a way to add many transactions at once
    //TODO add a way to remove transactions
    //TODO add a way to edit transactions
    //TODO add a way to save transactions to a file
    //TODO add a way to load transactions from a file
    static List<Transaction> transactions = new List<Transaction>();
    static void introduction()
    {
        Console.WriteLine("Välkommen till Budgeteringprogrammet! Skriv 'exit' för att avsluta.");
        Console.WriteLine("lista över kommandon: ");
        Console.WriteLine("expense add <belopp> <kategori> <beskrivning>");
        Console.WriteLine("income add <belopp> <kategori> <beskrivning>");
        Console.WriteLine("list balance - för att visa alla transaktioner");
        Console.WriteLine("balance - för att visa ditt saldo");
        Console.WriteLine("clear - för att ränsa skärmen");
    }
    static void HandleCommand(string command)
    {
        command = command.Trim().ToLower();
        string[] commandParts = command.Split(' ');
        if (commandParts[0] == "exit")
        {
            Environment.Exit(0);
        }
        else if (commandParts[0] == "clear")
        {
            Console.Clear();
            introduction();
        }
        else if (commandParts.Length > 3 && commandParts[0] == "expense" && commandParts[1] == "add")
        {
            if (commandParts.Length == 4)
            {
                AddExpense(decimal.Parse(commandParts[2]), commandParts[3]);
            }
            else if (commandParts.Length > 4)
            {
                string description = JoinFromIndex(4, commandParts);
                AddExpense(decimal.Parse(commandParts[2]), commandParts[3], description);
            }

        }
        else if (commandParts.Length > 3 && commandParts[0] == "income" && commandParts[1] == "add")
        {
            if (commandParts.Length == 4)
            {
                AddIncome(decimal.Parse(commandParts[2]), commandParts[3]);
            }
            else if (commandParts.Length > 4)
            {
                string description = JoinFromIndex(4, commandParts);
                AddIncome(decimal.Parse(commandParts[2]), commandParts[3], description);
            }

        }
        else if (commandParts.Length == 1 && commandParts[0] == "balance")
        {
            ShowBalance();
        }
        else if (commandParts.Length < 3 && commandParts[0] == "list" && commandParts[1] == "balance")
        {
            ListBalance();
        }
        else
        {
            Console.WriteLine("Okänt kommando");
        }
    }
    static string JoinFromIndex(int index, string[] strings)
    {
        string result = "";
        for (int i = index; i < strings.Length; i++)
        {
            result += strings[i];
            if (i < strings.Length - 1)
            {
                result += " ";
            }
        }
        return result;
    }

    static void AddExpense(decimal amount, string category, string description = null)
    {
        Transaction transaction = new Transaction(-amount, category, description);
        transactions.Add(transaction);
        if (description == null)
        {
            Console.WriteLine($"Utgiften har lagts till: {amount}kr för {category}");
            return;
        }
        Console.WriteLine($"Utgiften har lagts till: {amount}kr för {category} ({description})");
    }
    static void AddIncome(decimal amount, string category, string description = null)
    {
        Transaction transaction = new Transaction(amount, category, description);
        transactions.Add(transaction);
        if (description == null)
        {
            Console.WriteLine($"Inkomsten har lagts till: {amount}kr för {category}");
            return;
        }
        Console.WriteLine($"Inkomsten har lagts till: {amount}kr för {category} ({description})");
    }
    static void ShowBalance()
    {
        decimal totalIncome = 0;
        decimal totalExpense = 0;

        for (int i = 0; i < transactions.Count; i++)
        {
            if (transactions[i].Amount > 0)
            {
                totalIncome += transactions[i].Amount;
            }
            else
            {
                totalExpense += transactions[i].Amount;
            }
        }

        decimal balance = totalIncome + totalExpense;
        Console.WriteLine("totala inkomster: " + totalIncome + "kr");
        Console.WriteLine("totala utgifter: " + totalExpense + "kr");
        Console.WriteLine("Ditt saldo är: " + balance + "kr");
    }
    static void ListBalance()
    {
        for (int i = 0; i < transactions.Count; i++)
        {
            if (transactions[i].Description == null)
            {
                Console.WriteLine($"{transactions[i].Date} {transactions[i].Amount}kr {transactions[i].Category}");
                continue;
            }
            Console.WriteLine($"{transactions[i].Date} {transactions[i].Amount}kr {transactions[i].Category} ({transactions[i].Description})");
        }
    }
    static void Main(string[] args)
    {
        introduction();

        while (true)
        {
            Console.Write(">");
            string command = Console.ReadLine();
            HandleCommand(command);

        }
    }
}