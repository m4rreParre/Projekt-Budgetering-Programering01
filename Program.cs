using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static List<Transaction> transactions = new List<Transaction>();
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
        }
        else if (commandParts.Length > 3 && commandParts[0] == "expense" && commandParts[1] == "add")
        {
            commandParts[4] = commandParts[4];
            AddExpense(decimal.Parse(commandParts[2]), commandParts[3], commandParts[4]);
        }
        else if (commandParts.Length > 3 && commandParts[0] == "income" && commandParts[1] == "add")
        {
            AddIncome(decimal.Parse(commandParts[2]), commandParts[3], commandParts[4]);
        }
        else if (commandParts.Length == 1 && commandParts[0] == "balance")
        {
            ShowBalance();
        }
        else
        {
            Console.WriteLine("Okänt kommando");
        }
    }

    static void AddExpense(decimal amount, string category, string description)
    {
        Transaction transaction = new Transaction(-amount, category, description);
        transactions.Add(transaction);
        Console.WriteLine($"Utgiften har lagts till: {amount}kr för {category} ({description})");
    }
    static void AddIncome(decimal amount, string category, string description)
    {
        Transaction transaction = new Transaction(amount, category, description);
        transactions.Add(transaction);
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
    static void Main(string[] args)
    {
        Console.WriteLine("Välkommen till Budgeteringprogrammet! Skriv 'exit' för att avsluta.");

        while (true)
        {
            Console.Write(">");
            string command = Console.ReadLine();
            HandleCommand(command);
        }
    }
}