﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;



class Program
{

    //TODO filter by date - last month, last week, last year
    //TODO add a way to edit transactions
    //TODO add a monthly budget calculator that shows how much you can spend each day to stay within budget (watches how many days left in the month)

    static CultureInfo culture = CultureInfo.CurrentCulture;
    static string usedDecimalType = culture.NumberFormat.NumberDecimalSeparator;
    static List<Transaction> transactions = new List<Transaction>();
    static List<Transaction> incomes = new List<Transaction>();
    static List<Transaction> expenses = new List<Transaction>();
    static Regex letterFilter = new Regex(@"[a-zA-Z]");


    static void Introduction()
    {
        Console.WriteLine("lista över kommandon: ");
        Console.WriteLine("==========================");
        Console.WriteLine("clear - för att ränsa skärmen");
        Console.WriteLine("expense add <belopp> <kategori> <beskrivning>");
        Console.WriteLine("income add <belopp> <kategori> <beskrivning>");
        Console.WriteLine("-------------------------------------------");
        Console.WriteLine("list transactions - för att visa alla transaktioner");
        Console.WriteLine("list incomes - för att visa alla inkomster");
        Console.WriteLine("list expenses - för att visa alla utgifter");
        Console.WriteLine("-------------------------------------------");
        Console.WriteLine("balance - för att visa ditt saldo");
        Console.WriteLine("remove <id> - för att ta bort en transaktion");
        Console.WriteLine("-------------------------------------------");
        Console.WriteLine("list transactions sortby <value|category|date> <highest/lowest |kategorinamn|newest/oldest> - för att sortera transaktioner");
        Console.WriteLine("list expenses sortby <value|category> <highest/lowest |kategorinamn> - för att sortera utgifter");
        Console.WriteLine("list incomes sortby <value|category> <highest/lowest |kategorinamn> - för att sortera inkomster");
        Console.WriteLine("-------------------------------------------");
        Console.WriteLine("savetransactions - för att spara transaktioner till en fil");
        Console.WriteLine("loadtransactions - för att ladda transaktioner från en fil");
        Console.WriteLine("==========================");
    }
    static void HandleCommand(string command)
    {
        command = command.Trim().ToLower();
        string[] commandParts = command.Split(' ');
        if (commandParts[0] == "exit")
        {
            Environment.Exit(0);
        }
        else if (commandParts[0] == "help")
        {
            Introduction();
        }
        else if (commandParts[0] == "clear")
        {
            Console.Clear();
            Console.WriteLine("Välkommen till Budgeteringprogrammet! Skriv 'exit' för att avsluta.");
            Console.WriteLine("använder valutan: kr");
            Console.WriteLine("skriv 'help' för att se en lista över kommandon");
        }
        else if (commandParts.Length > 3 && commandParts[0] == "expense" && commandParts[1] == "add")
        {
            string InputAmount = commandParts[2];
            bool containsLetter = letterFilter.IsMatch(InputAmount);
            if (usedDecimalType == "," && InputAmount.Contains("."))
            {
                InputAmount = InputAmount.Replace(".", ",");
            }
            else if (usedDecimalType == "." && InputAmount.Contains(","))
            {
                InputAmount = InputAmount.Replace(",", ".");
            }
            if (containsLetter)
            {
                Console.WriteLine("Felaktig inmatning, skriv in beloppet utan valutan eller bokstäver");
                return;
            }
            else if (commandParts.Length == 4)
            {
                AddExpense(decimal.Parse(InputAmount), commandParts[3]);
            }
            else if (commandParts.Length > 4)
            {
                string description = JoinFromIndex(4, commandParts);
                AddExpense(decimal.Parse(InputAmount), commandParts[3], description);
            }

        }

        else if (commandParts.Length > 3 && commandParts[0] == "income" && commandParts[1] == "add")
        {
            string InputAmount = commandParts[2];
            bool containsLetter = letterFilter.IsMatch(InputAmount);
            if (usedDecimalType == "," && InputAmount.Contains("."))
            {
                InputAmount = InputAmount.Replace(".", ",");
            }
            else if (usedDecimalType == "." && InputAmount.Contains(","))
            {
                InputAmount = InputAmount.Replace(",", ".");
            }
            if (containsLetter)
            {
                Console.WriteLine("Felaktig inmatning, skriv in beloppet utan bokstäver eller tecken");
                return;
            }
            else if (commandParts.Length == 4)
            {
                AddIncome(decimal.Parse(InputAmount), commandParts[3]);
            }
            else if (commandParts.Length > 4)
            {
                string description = JoinFromIndex(4, commandParts);
                AddIncome(decimal.Parse(InputAmount), commandParts[3], description);
            }

        }
        else if (commandParts.Length == 3 && commandParts[0] == "income" && commandParts[1] == "add" || commandParts.Length == 3 && commandParts[0] == "expense" && commandParts[1] == "add")
        {
            Console.WriteLine("du måste ange en kategori");
        }
        else if (commandParts.Length == 1 && commandParts[0] == "balance")
        {
            ShowBalance();
        }
        else if (commandParts.Length <= 3 && commandParts[0] == "list" && commandParts[1] == "transactions")
        {

            if (commandParts.Length == 2)
            {
                ListTransactions(transactions);
            }
            else if (commandParts.Length == 3)
            {

                bool containsLetter = letterFilter.IsMatch(commandParts[2]);
                if (containsLetter)
                {
                    Console.WriteLine("Felaktig inmatning, skriv in mängden med bara siffror");
                }
                else
                {
                    ListTransactions(transactions, int.Parse(commandParts[2]));
                }
            }
        }
        else if (commandParts.Length == 2 && commandParts[0] == "remove")
        {
            bool usesLetters = letterFilter.IsMatch(commandParts[1].ToString());
            if (usesLetters)
            {
                Console.WriteLine("Felaktig inmatning, skriv in id:t med bara siffror");
                return;
            }
            DeleteTransaction(int.Parse(commandParts[1]));
        }
        else if (commandParts.Length >= 5 && commandParts[0] == "list" && commandParts[1] == "transactions" && commandParts[2] == "sortby")
        {
            if (commandParts.Length == 6)
            {
                SortingHandler(commandParts[3], commandParts[4], commandParts[5]);
            }
            else
            {
                SortingHandler(commandParts[3], commandParts[4]);
            }

        }
        else if (commandParts.Length == 2 && commandParts[0] == "list" && commandParts[1] == "expenses")
        {
            //implementera senare siffra efter expenses för att lista visst antal exoenses
            ListExpense();
        }
        else if (commandParts.Length == 2 && commandParts[0] == "list" && commandParts[1] == "incomes")
        {
            //implementera senare siffra efter incomes för att lista visst antal inkomster
            ListIncome();
        }
        else if (commandParts.Length >= 2 && commandParts[0] == "list" && commandParts[1] == "expenses" && commandParts[2] == "sortby")
        {
            IncomeAndOutcomeSeperator();
            if (commandParts.Length == 6)
            {
                SortingHandler(commandParts[3], commandParts[1], commandParts[4], expenses);
            }
            else
            {
                if (commandParts[3] == "category")
                {
                    SortingHandler(commandParts[3], commandParts[4], null, expenses);
                }
                else
                {
                    SortingHandler(commandParts[1], commandParts[4], null, expenses);
                }
            }
        }
        else if (commandParts.Length >= 2 && commandParts[0] == "list" && commandParts[1] == "incomes" && commandParts[2] == "sortby")
        {
            IncomeAndOutcomeSeperator();
            if (commandParts.Length == 6)
            {
                SortingHandler(commandParts[3], commandParts[1], commandParts[4], incomes);
            }
            else
            {
                if (commandParts[3] == "category")
                {
                    SortingHandler(commandParts[3], commandParts[4], null, incomes);
                }
                else
                {
                    SortingHandler(commandParts[1], commandParts[4], null, incomes);
                }
            }
        }
        else if (commandParts[0] == "savetransactions")
        {
            SaveTransactions();
        }
        else if (commandParts[0] == "loadtransactions")
        {
            LoadTransactions();

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
    static void ListTransactions(List<Transaction> usedList, int amountShowed = 0)
    {
        if (usedList.Count == 0)
        {
            Console.WriteLine("Inga transaktioner att visa");
            return;
        }
        else if (amountShowed == 0)
        {
            for (int i = 0; i < usedList.Count; i++)
            {
                if (usedList[i].Description == null)
                {
                    Console.WriteLine($"id:{usedList[i].Id} {usedList[i].Date} {usedList[i].Amount}kr {usedList[i].Category}");
                    continue;
                }
                Console.WriteLine($"id:{usedList[i].Id} {usedList[i].Date} {usedList[i].Amount}kr {usedList[i].Category} ({usedList[i].Description})");
            }
        }
        else if (amountShowed > usedList.Count)
        {
            Console.WriteLine("det finns bara " + usedList.Count + " transaktioner");
        }
        else if (amountShowed != 0)
        {
            for (int i = 0; i < amountShowed; i++)
            {
                if (usedList[i].Description == null)
                {
                    Console.WriteLine($"id:{usedList[i].Id} {usedList[i].Date} {usedList[i].Amount}kr {usedList[i].Category}");
                    continue;
                }
                Console.WriteLine($"id:{usedList[i].Id} {usedList[i].Date} {usedList[i].Amount}kr {usedList[i].Category} ({usedList[i].Description})");
            }
        }

    }
    static void DeleteTransaction(int id)
    {
        bool idFound = false;

        for (int i = 0; i < transactions.Count; i++)
        {

            if (transactions[i].Id == id)
            {
                idFound = true;
                transactions.RemoveAt(i);
                Console.WriteLine("Transaktionen har tagits bort");
                return;
            }

        }
        if (idFound == false)
        {
            Console.WriteLine("Transaktionen med det id:t finns inte");
        }
    }
    static void SortingHandler(string sortby, string sort, string sort2 = null, List<Transaction> usedList = null)
    {
        if (usedList == null)
        {
            usedList = transactions;
        }
        if (sortby == "value" && sort == "highest")
        {
            ValueSorter("highest", transactions);
        }
        else if (sortby == "value" && sort == "lowest")
        {
            ValueSorter("lowest", transactions);
        }
        else if (sortby == "date" && sort == "newest")
        {
            DateSorter(transactions, false);
        }
        else if (sortby == "date" && sort == "oldest")
        {
            DateSorter(transactions, true);
        }
        else if (sortby == "category")
        {
            CategorySorter(sort, usedList);
        }
        else if (sortby == "expenses" && sort == "highest")
        {
            ValueSorter("highest", expenses);
        }
        else if (sortby == "expenses" && sort == "lowest")
        {
            ValueSorter("lowest", expenses);
        }
        else if (sortby == "expenses" && sort == "category")
        {
            CategorySorter(sort2, expenses);
        }
        else if (sortby == "incomes" && sort == "highest")
        {
            ValueSorter("highest", incomes);
        }
        else if (sortby == "incomes" && sort == "lowest")
        {
            ValueSorter("lowest", incomes);
        }
        else if (sortby == "incomes" && sort == "category")
        {
            CategorySorter(sort2, incomes);
        }


    }
    static void ValueSorter(string sort, List<Transaction> usedList)
    {
        
        if (usedList.Count == 0)
        {
            Console.WriteLine("Inga transaktioner att visa");
            return;
        }
        QuickSort(usedList, 0, usedList.Count - 1);
        if (sort == "highest")
        {
            usedList.Reverse();
        }
        ListTransactions(usedList);
    }
    static void DateSorter(List<Transaction> usedList, bool newestFirst = true)
    {
        List<Transaction> sortedList = QuickSortDates(usedList, 0, usedList.Count - 1);
        if (sortedList.Count == 0)
        {
            Console.WriteLine("Inga transaktioner att visa");
            return;
        }

        if (!newestFirst)
        {
            sortedList.Reverse();
        }
        ListTransactions(sortedList);


    }
    static void IncomeAndOutcomeSeperator()
    {
        for (int i = 0; i < transactions.Count; i++)
        {
            if (transactions[i].Amount > 0)
            {
                incomes.Add(transactions[i]);
            }
            else if (transactions[i].Amount < 0)
            {
                expenses.Add(transactions[i]);
            }
        }
    }
    static void ListIncome()
    {
        incomes.Clear();

        IncomeAndOutcomeSeperator();
        ListTransactions(incomes);
        Console.WriteLine("totala inkomster: " + incomes.Count);
    }
    static void ListExpense()
    {
        expenses.Clear();
        IncomeAndOutcomeSeperator();
        ListTransactions(expenses);
        Console.WriteLine("totala utgifter: " + expenses.Count);
    }
    static bool CategoryIsInList(string category, List<Transaction> usedList)
    {
        for (int i = 0; i < usedList.Count; i++)
        {
            if (usedList[i].Category == category)
            {
                return false;
            }
        }
        return true;
    }
    static void CategorySorter(string category, List<Transaction> usedList)
    {
        bool CategoryNotFound = CategoryIsInList(category, usedList);
        for (int i = 0; i < transactions.Count; i++)
        {

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
    static void QuickSort(List<Transaction> data, int left, int right)
    {
        if (left < right)
        {
            int pivotIndex = Partition(data, left, right);
            QuickSort(data, left, pivotIndex - 1); // Sortera vänstra delen
            QuickSort(data, pivotIndex + 1, right); // Sortera högra delen
        }
    }
    static int Partition(List<Transaction> data, int left, int right)
    {
        Decimal pivot = data[(left + right) / 2].Amount; // Pivot från mitten
        int leftHold = left;
        int rightHold = right;

        while (leftHold <= rightHold)
        {
            while (data[leftHold].Amount < pivot)
            {
                leftHold++;
            }
            while (data[rightHold].Amount > pivot)
            {
                rightHold--;
            }
            if (leftHold <= rightHold)
            {
                Transaction temp = data[leftHold];
                data[leftHold] = data[rightHold];
                data[rightHold] = temp;
                leftHold++;
                rightHold--;
            }
        }
        return leftHold;
    }
    static List<Transaction> QuickSortDates(List<Transaction> usedList, int left, int right)
    {
        List<Transaction> data = new List<Transaction>(usedList);

        if (left < right)
        {
            // Hittar pivot-index genom att partitionera listan
            int pivotIndex = PartitionDates(data, left, right);

            // Sorterar delarna före och efter pivot
            QuickSortDates(data, left, pivotIndex - 1);
            QuickSortDates(data, pivotIndex + 1, right);
        }

        return data;
    }
    static int PartitionDates(List<Transaction> data, int left, int right)
    {
        // Väljer pivot-värde från mitten av listan
        DateTime pivot = data[(left + right) / 2].Date;

        int leftHold = left;
        int rightHold = right;

        while (leftHold <= rightHold)
        {
            // Hittar första elementet från vänster som är >= pivot
            while (data[leftHold].Date < pivot)
            {
                leftHold++;
            }

            // Hittar första elementet från höger som är <= pivot
            while (data[rightHold].Date > pivot)
            {
                rightHold--;
            }

            // Byter plats på elementen om de är på fel sida av pivot
            if (leftHold <= rightHold)
            {
                Transaction temp = data[leftHold];
                data[leftHold] = data[rightHold];
                data[rightHold] = temp;
                leftHold++;
                rightHold--;
            }
        }

        // Returnerar ny position där partitioneringen kört klart
        return leftHold;
    }
    static void SaveTransactions()
    {
        string json = JsonConvert.SerializeObject(transactions, Formatting.Indented);
        string filePath = "transactions.json";
        File.WriteAllText(filePath, json);
        Console.WriteLine("Transaktioner har sparats till " + filePath);
    }
    static void LoadTransactions()
    {
        string filePath = "transactions.json";
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            transactions = JsonConvert.DeserializeObject<List<Transaction>>(json);
            Console.WriteLine("Transpersoner har laddats från " + filePath);
            int id = LoadId();
            Transaction.idChange(id);
        }
        else
        {
            Console.WriteLine("ingen transaktionsfil att ladda");
        }
    }

    static int LoadId()
    {

        int id = 0;
        for (int i = 0; i < transactions.Count; i++)
        {
            if (transactions[i].Id > id)
            {
                id = transactions[i].Id;
            }
        }
        return id + 1;
    }
    static void Main(string[] args)
    {
        Console.WriteLine("Välkommen till Budgeteringprogrammet! Skriv 'exit' för att avsluta.");
        Console.WriteLine("använder valutan: kr");
        Console.WriteLine("skriv 'help' för att se en lista över kommandon");

        while (true)
        {
            Console.Write(">");
            string command = Console.ReadLine();
            HandleCommand(command);

        }
    }
}