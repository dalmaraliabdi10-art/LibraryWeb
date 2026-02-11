Bibliotekssystem (LibraryWeb)
Detta är en internetbaserad programvara för att hantera bibliotek, skapad med c#, Blazor Server och Entity Framework Core (SQLite). Projektet uppfyller kriterierna för objektorienterad programmering, databasadministration och enhetstester.

Funktioner i lösningen
Sökning: Man kan leta efter böcker utifrån titel, författare eller ISBN.

Lånesystem: Medlemmar har möjlighet att låna böcker och se vilka lån de har aktivt.

Återlämning: Medlemmar kan lämna tillbaka böcker (böckerna blir då omedelbart tillgängliga för andra).

Admin: Speciell inloggning för att kunna lägga till nya böcker i systemet.

Autentisering: System för inloggning och registrering av nya medlemmar.

Enhetstester: Kodbasen testas med xUnit.

Hur man kör programmet
För att starta applikationen, följ dessa steg i terminalen:

1. cd LibraryWeb

2. dotnet ef database update

3. dotnet watch

4. Logga in:
Gå till "Bibliotek" i menyn

Admin-konto:

Email: admin@admin.se

Lösenord: admin

Vanlig användare:

Email: anna@mail.com

Lösenord: 123

Man kan även registera ny användare

Hur man kör testerna
För att verifiera att logiken fungerar
* dotnet test LibraryWeb.Tests


Missförstånd av uppgiften:
I början av projektet hade jag en felaktig förståelse av vad som behövdes och började skapa appen som en vanlig konsolapplikation (Del 1) utan att involvera Blazor eller Entity Framework Core. När jag fick hjälp att förstå att webbgränssnittet och databasen var viktiga delar var jag tvungen att helt ändra struktur för att anpassa logiken till en modern webbdesign.

Problem:
Ett problem hände efter att jag hade jobbat länge. På grund av ett tekniskt problem förlorade jag hela min lokala version av programmet, inklusive Program.cs och mina modeller. Det tekniska problemet var att jag försökte koppla github till mappen men de viktigaste delarna försvann.

Lösning:
Jag fick börja om och bygga applikationen från grunden. Tack vare att jag hade sparat vissa kodfiler separat och kom ihåg logiken, då kunde jag lägga upp projektet snabbare andra gången.

Problem:
Jag insåg sent under arbetet att jag hade missat att inkludera en funktion för att returnera böcker.

Lösning:
Jag implementerade en metod som heter ReturnBookAsync i tjänsten som ställer in ReturnDate och gör boken tillgänglig (IsAvailable = true) och en knapp i användargränssnittet som endast syns för aktiva lån.