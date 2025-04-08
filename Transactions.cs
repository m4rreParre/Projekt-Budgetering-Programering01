using System;
using System.Collections.Generic;

public struct Transaction
{
    
    private static int nextId = 1;
    public int Id;
    public decimal Amount; //belopp (positivt för inkomst, negativ för utgift)
    public string Category; //kategori (t.ex. "Mat", "Fritid", "Bostad")
    public string Description; //beskrivning (t.ex. "Köpte mjölk", "Betalade hyra")
    public DateTime Date; //Datum när transaktionen gjordes

    public Transaction(decimal amount, string category, string description = null)
    {   
        Id = nextId++;
        Amount = amount;
        Category = category;
        Description = description;
        Date = DateTime.Now;
    }
}