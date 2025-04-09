# Budgetplanerare

## Introduktion

Välkommen till Budgetplanerare, ett kraftfullt kommandoradsverktyg för personlig ekonomihantering. Detta verktyg hjälper dig att enkelt hålla koll på dina inkomster och utgifter för att få bättre kontroll över din ekonomiska situation. Med Budgetplanerare kan du effektivt spåra dina transaktioner, kategorisera dem och få en tydlig överblick över ditt finansiella läge.

## Tillgängliga kommandon

### Grundläggande kommandon
* `clear` - Rensar skärmen och visar introduktionsmeddelandet
* `balance` - Visar ditt aktuella saldo (inkomster minus utgifter)
* `help` - Visar en lista över alla tillgängliga kommandon

### Transaktionshantering
* `expense add <belopp> <kategori> <beskrivning>` - Lägger till en ny utgift
* `income add <belopp> <kategori> <beskrivning>` - Lägger till en ny inkomst
* `remove <id>` - Tar bort en transaktion baserat på dess ID

### Visningskommandon
* `list transactions` - Visar alla transaktioner
* `list incomes` - Visar alla inkomsttransaktioner
* `list expenses` - Visar alla utgiftstransaktioner

### Sorteringskommandon
* `list transactions sortby <value|category|date> <highest/lowest|category|newest/oldest>` - Sorterar transaktioner efter värde, kategori eller datum
* `list expenses sortby <value|category> <highest/lowest|category>` - Sorterar utgifter efter värde eller kategori
* `list incomes sortby <value|category> <highest/lowest|category>` - Sorterar inkomster efter värde eller kategori

### Hantering av filer
* `savetransactions` - Sparar alla transaktioner till en fil
* `loadtransactions` - Läser in transaktioner från en sparad fil

## Exempel på användning

### Lägga till transaktioner
```
income add 28500 Lön Januari månadslön
expense add 7500 Hyra Månadshyra för lägenheten
expense add 1235.50 Mat Veckohandling på ICA
expense add 349 Nöje Biobiljetter och popcorn
income add 5000 Extraarbete Hundvakt åt grannen
```

### Visa transaktioner och saldo
```
list transactions
list expenses
list incomes
balance
```

### Sortera transaktioner
```
list expenses sortby value highest
list transactions sortby date newest
list incomes sortby category
```

### Spara och läsa in data
```
savetransactions
loadtransactions
```

## Tips för effektiv ekonomihantering

* Använd konsekventa kategorinamn för att bättre organisera dina finanser (t.ex. "Mat", "Boende", "Transport", "Nöje")
* Kontrollera ditt saldo regelbundet med `balance`-kommandot för att hålla koll på din ekonomiska hälsa
* Lägg till detaljerade beskrivningar som hjälper dig att minnas vad transaktionen gällde
* Dela upp dina utgifter i "nödvändiga" (t.ex. hyra, mat) och "icke-nödvändiga" (t.ex. nöje, shopping) för att lättare hitta möjligheter att spara
* Exportera regelbundet din transaktionsdata för säkerhetskopiering och vidare analys
* Sätt upp månatliga budgetar för olika kategorier och följ upp hur väl du håller dig till dem
* Använd `list expenses sortby value highest` för att identifiera dina största utgifter och var du potentiellt kan spara

## Installation av Newtonsoft.Json

Budgetplanerare använder paketet Newtonsoft.Json för att hantera sparande och inläsning av data. Om detta paket inte redan finns i ditt projekt, följ dessa steg för att installera det:

1. Öppna en terminal eller kommandoprompt
2. Navigera till din projektmapp
3. Kör kommandot: `dotnet add package Newtonsoft.Json`

## Kom igång

För att komma igång med Budgetplanerare, starta programmet och använd `help`-kommandot för att se en lista över alla tillgängliga funktioner. Börja med att registrera dina inkomster och utgifter, och använd sedan verktygets olika funktioner för att analysera din ekonomiska situation.

Lycka till med din ekonomiplanering!
