using System;
using System.Collections.Generic;

public struct Transaction
{
    public decimal Amount; //belopp (positivt för inkomst, negativ för utgift)
    public string Category; //kategori (t.ex. "Mat", "Fritid", "Bostad")
    public string Description; //beskrivning (t.ex. "Köpte mjölk", "Betalade hyra")
    public DateTime Date; //Datum när transaktionen gjordes

    public Transaction(decimal amount, string category, string description = "no description")
    {
        Amount = amount;
        Category = category;
        Description = description;
        Date = DateTime.Now;
    }
}