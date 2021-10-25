Task:

-   The primary goal of this exercise is not to check if you can code (we are sure that you can), 
    but to see how exactly you craft the code, to see your approach, to learn about your style of work
-   Please provide a good object-oriented solution in C# or any oo-language. 
    A Visual Studio Solution would be perfect.
-   The solution should include a brief demonstration for all three cases. 
    The form of this demonstration does not matter. For instance, no UI is required, 
    a console application or unit test is absolutely enough. 
    Please don't waste time on UI, the focus here is on clean object oriented code.
-   You'll score better with unit tests, please include them into your solution if you write any.
-   Please don't write any code which is not directly required by the given task, 
    but at the same time keep in mind that the code must be easily extendible if requirements change.
 

Sales tax

Basic sales tax is applicable at a rate of 10% on all goods – except books, food and medical products, which are exempt.
Import duty is an additional sales tax applicable on all imported goods at a rate of 5%, with no exceptions.
The rounding rules for sales tax are that for a tax rate of n%, a shelf price of p contains (n*p/100 rounded up to the next $0.05) amount of sales tax.
Please implement the sales tax to satisfy the following requirement:
When I purchase items I receive a receipt which lists the names of all the items and their price (including tax), 
finishing with the total cost of the items, and the total amounts of sales taxes paid.

Aufgabe:

-   Das primäre Ziel dieser Übung besteht nicht darin, zu überprüfen, ob Sie programmieren können (wir sind sicher, dass Sie das können),
    Aber um zu sehen, wie genau Sie den Code erstellen, um Ihren Ansatz zu sehen, um Ihren Arbeitsstil kennenzulernen
-   Bitte stellen Sie eine gute objektorientierte Lösung in C# oder einer anderen oo-Sprache bereit.
    Eine Visual Studio-Lösung wäre perfekt.
-   Die Lösung sollte eine kurze Demonstration für alle drei Fälle beinhalten.
    Die Form dieser Demonstration spielt keine Rolle. Beispielsweise ist keine Benutzeroberfläche erforderlich,
    ein Konsolenanwendungs- oder Komponententest ist absolut ausreichend.
    Bitte verschwenden Sie keine Zeit mit der Benutzeroberfläche, der Fokus liegt hier auf sauberem objektorientiertem Code.
-   Sie werden mit Unit-Tests besser abschneiden, bitte nehmen Sie sie in Ihre Lösung auf, wenn Sie welche schreiben.
-   Bitte schreiben Sie keinen Code, der nicht direkt von der gegebenen Aufgabe benötigt wird,
    denken Sie aber auch daran, dass der Code leicht erweiterbar sein muss, wenn sich die Anforderungen ändern.
 

Mehrwertsteuer

Auf alle Waren – mit Ausnahme von Büchern, Lebensmitteln und Medizinprodukten, die ausgenommen sind – wird die Grundumsatzsteuer in Höhe von 10 % erhoben.
Der Einfuhrzoll ist eine zusätzliche Umsatzsteuer, die auf alle importierten Waren in Höhe von 5% ohne Ausnahmen erhoben wird.

Die Rundungsregeln für die Umsatzsteuer lauten, dass bei einem Steuersatz von n% ein Regalpreis von p (n*p/100 aufgerundet auf die nächsten 0,05 US-Dollar) Umsatzsteuer enthält.

Bitte implementieren Sie die Umsatzsteuer, um die folgende Anforderung zu erfüllen:
Beim Kauf von Artikeln erhalte ich eine Quittung, in der die Namen aller Artikel und deren Preis (einschließlich Steuern) aufgeführt sind.
mit den Gesamtkosten der Artikel und den Gesamtbeträgen der gezahlten Mehrwertsteuer.

Please demonstrate the 3 cases:

_____________________________________________________

Case 1:

Input:
    1 book at 12.49
    1 music CD at 14.99
    1 chocolate bar at 0.85


Output:
    1 book: $12.49
    1 music CD: $16.49
    1 chocolate bar: $0.85

Sales Taxes: $1.50
Total: $29.83

_____________________________________________________

Case 2:

Input:
    1 imported box of chocolates at 10.00
    1 imported bottle of perfume at 47.50
 
Output:
    1 imported box of chocolates: 10.50
    1 imported bottle of perfume: 54.65

Sales Taxes: 7.65
Total: 65.15


_____________________________________________________

Case 3:

Input:
    5 imported medical plaster at 0.10

Output:
    5 imported medical plaster: $0.75

Sales Taxes: $0.25
Total: $0.75
