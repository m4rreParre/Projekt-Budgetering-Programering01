﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    //TODO Add a sortby parameter to valuesorter so you can just sort and show the incomes or expenses
    //TODO change sortingalgorithm to a faster one then bubble sort. 
    //TODO be able to list just transactions of a sertain number like only 5 latest


    //TODO filters in listbalance syntax list balance sort_by="value" sort="highest", list balance sort_by="value" sort="lowest"
    //TODO filter by date - last month, last week, last year


    //TODO add a way to add many transactions at once
    //TODO add a way to remove transactions
    //TODO add a way to edit transactions
    //TODO add a way to save transactions to a file
    //TODO add a way to load transactions from a file
    //TODO add a monthly budget calculator that shows how much you can spend each day to stay within budget (watches how many days left in the month)
    static List<Transaction> transactions = new List<Transaction>();
    static List<Transaction> Incomes = new List<Transaction>();
    static List<Transaction> Expenses = new List<Transaction>();
    static Regex LetterFilter = new Regex(@"[a-zA-Z]");

    static void introduction()
    {
        Console.WriteLine("Välkommen till Budgeteringprogrammet! Skriv 'exit' för att avsluta.");
        Console.WriteLine("lista över kommandon: ");
        Console.WriteLine("expense add <belopp> <kategori> <beskrivning>");
        Console.WriteLine("income add <belopp> <kategori> <beskrivning>");
        Console.WriteLine("list transactions - för att visa alla transaktioner");
        Console.WriteLine("balance - för att visa ditt saldo");
        Console.WriteLine("remove <id> - för att ta bort en transaktion");
        Console.WriteLine("list transactions sortby <value|category> <highest/lowest |kategorinamn> - för att sortera transaktioner");
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
            bool containsLetter = LetterFilter.IsMatch(commandParts[2]);
            if (containsLetter)
            {
                Console.WriteLine("Felaktig inmatning, skriv in beloppet utan valutan eller bokstäver");
                return;
            }
            else if (commandParts.Length == 4)
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
            bool containsLetter = LetterFilter.IsMatch(commandParts[2]);
            if (containsLetter)
            {
                Console.WriteLine("Felaktig inmatning, skriv in beloppet utan valutan eller bokstäver");
                return;
            }
            else if (commandParts.Length == 4)
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
        else if (commandParts.Length < 3 && commandParts[0] == "list" && commandParts[1] == "transactions")
        {
            ListTransactions();
        }
        else if (commandParts.Length == 2 && commandParts[0] == "remove")
        {
            DeleteTransaction(int.Parse(commandParts[1]));
        }
        else if (commandParts.Length == 5 && commandParts[0] == "list" && commandParts[1] == "transactions" && commandParts[2] == "sortby")
        {
            SortingHandler(commandParts[3], commandParts[4]);
        }
        else if (commandParts.Length == 2 && commandParts[0] == "list" && commandParts[1] == "expenses")
        {
            ListExpense();
        }
        else if (commandParts.Length == 2 && commandParts[0] == "list" && commandParts[1] == "incomes")
        {
            ListIncome();
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
    static void ListTransactions()
    {
        for (int i = 0; i < transactions.Count; i++)
        {
            if (transactions[i].Description == null)
            {
                Console.WriteLine($"id:{transactions[i].Id} {transactions[i].Date} {transactions[i].Amount}kr {transactions[i].Category}");
                continue;
            }
            Console.WriteLine($"id:{transactions[i].Id} {transactions[i].Date} {transactions[i].Amount}kr {transactions[i].Category} ({transactions[i].Description})");
        }
    }
    static void DeleteTransaction(int id)
    {
        for (int i = 0; i < transactions.Count; i++)
        {
            if (transactions[i].Id == id)
            {
                transactions.RemoveAt(i);
                Console.WriteLine("Transaktionen har tagits bort");
                return;
            }
        }
    }
    static void SortingHandler(string sort_by, string sort)
    {
        if (sort_by == "value" && sort == "highest")
        {
            ValueSorter("highest");
        }
        else if (sort_by == "value" && sort == "lowest")
        {
            ValueSorter("lowest");
        }
        else if (sort_by == "date" && sort == "newest")
        {

        }
        else if (sort_by == "date" && sort == "oldest")
        {

        }
        else if (sort_by == "category")
        {
            CategorySorter(sort);
        }
    }
    static void ValueSorter(string sort)
    {
        List<Transaction> ValueSortedTransactions = new List<Transaction>(transactions);
        if (sort == "highest")
        {
            for(int i = 0; i < ValueSortedTransactions.Count; i++)
            {
                for(int j = i + 1; j < ValueSortedTransactions.Count; j++)
                {
                    if(ValueSortedTransactions[i].Amount < ValueSortedTransactions[j].Amount)
                    {
                        Transaction temp = ValueSortedTransactions[i];
                        ValueSortedTransactions[i] = ValueSortedTransactions[j];
                        ValueSortedTransactions[j] = temp;
                    }
                }
            }
            for(int i = 0; i < ValueSortedTransactions.Count; i++)
            {
                if (ValueSortedTransactions[i].Description == null)
                {
                    Console.WriteLine($"id:{ValueSortedTransactions[i].Id} {ValueSortedTransactions[i].Date} {ValueSortedTransactions[i].Amount}kr {ValueSortedTransactions[i].Category}");
                    continue;
                }
                Console.WriteLine($"id:{ValueSortedTransactions[i].Id} {ValueSortedTransactions[i].Date} {ValueSortedTransactions[i].Amount}kr {ValueSortedTransactions[i].Category} ({ValueSortedTransactions[i].Description})");
            }
        }
        else if (sort == "lowest")
        {
            for(int i = 0; i < ValueSortedTransactions.Count; i++)
            {
                for(int j = i + 1; j < ValueSortedTransactions.Count; j++)
                {
                    if(ValueSortedTransactions[i].Amount > ValueSortedTransactions[j].Amount)
                    {
                        Transaction temp = ValueSortedTransactions[i];
                        ValueSortedTransactions[i] = ValueSortedTransactions[j];
                        ValueSortedTransactions[j] = temp;
                    }
                }
            }
            for(int i = 0; i < ValueSortedTransactions.Count; i++)
            {
                if (ValueSortedTransactions[i].Description == null)
                {
                    Console.WriteLine($"id:{ValueSortedTransactions[i].Id} {ValueSortedTransactions[i].Date} {ValueSortedTransactions[i].Amount}kr {ValueSortedTransactions[i].Category}");
                    continue;
                }
                Console.WriteLine($"id:{ValueSortedTransactions[i].Id} {ValueSortedTransactions[i].Date} {ValueSortedTransactions[i].Amount}kr {ValueSortedTransactions[i].Category} ({ValueSortedTransactions[i].Description})");
            }
        }
    }
    static void IncomeAndOutcomeSeperator()
    {
        for (int i = 0; i < transactions.Count; i++)
        {
            if (transactions[i].Amount > 0)
            {
                Incomes.Add(transactions[i]);
            }
            else if (transactions[i].Amount < 0)
            {
                Expenses.Add(transactions[i]);
            }
        }
    }
    static void ListIncome()
    {
        Incomes.Clear();

        IncomeAndOutcomeSeperator();
        for (int i = 0; i < Incomes.Count; i++)
        {
            if (Incomes[i].Description == null)
            {
                Console.WriteLine($"id:{Incomes[i].Id} {Incomes[i].Date} {Incomes[i].Amount}kr {Incomes[i].Category}");
                continue;
            }
            Console.WriteLine($"id:{Incomes[i].Id} {Incomes[i].Date} {Incomes[i].Amount}kr {Incomes[i].Category} ({Incomes[i].Description})");
        }
    }
    static void ListExpense()
    {
        Expenses.Clear();

        IncomeAndOutcomeSeperator();
        for (int i = 0; i < Expenses.Count; i++)
        {
            if (Expenses[i].Description == null)
            {
                Console.WriteLine($"id:{Expenses[i].Id} {Expenses[i].Date} {Expenses[i].Amount}kr {Expenses[i].Category}");
                continue;
            }
            Console.WriteLine($"id:{Expenses[i].Id} {Expenses[i].Date} {Expenses[i].Amount}kr {Expenses[i].Category} ({Expenses[i].Description})");
        }
    }
    static bool CategoryIsInList(string category)
    {
        for (int i = 0; i < transactions.Count; i++)
        {
            if (transactions[i].Category == category)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        return true;
    }
    static void CategorySorter(string category)
    {
        bool CategoryNotFound = CategoryIsInList(category);
        for (int i = 0; i < transactions.Count; i++)
        {
            int j = i + 1;
            if (transactions[i].Category == category)
            {
                if (transactions[i].Description == null)
                {
                    Console.WriteLine($"id:{transactions[i].Id} {transactions[i].Date} {transactions[i].Amount}kr {transactions[i].Category}");
                    continue;
                }
                Console.WriteLine($"id:{transactions[i].Id} {transactions[i].Date} {transactions[i].Amount}kr {transactions[i].Category} ({transactions[i].Description})");
            }
            else if (CategoryNotFound)
            {
                Console.WriteLine("skriv in en kategori som finns");
                return;
            }
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