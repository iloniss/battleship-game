# Battleship Game

Battleship Game to aplikacja symulująca losową grę w statki. Program umożliwia wygenerowanie losowych plansz do gry w statki dla dwóch graczy i przeprowadzenia losowej symulacji rozgrywki. Za pomocą przycisku Graj istnieje też możliwość losowej rozgrywki między dwiema osobami. Symulacja gry jest wizualizowana z wykorzystaniem technologii webowych.

# Tech stack

Logika aplikacji napisana w języku C#, do wymiany danych ze stroną stworzono API w środowisku .NET 5. Backend aplikacji składa się z dwóch projektów, jednym z projektów jest projekt typu Class Library, w którym przechowywana jest logika, a drugim wcześniej wspomniane API. Uproszczony interfejs aplikacji stworzono wykorzystując React oraz język Typescript.
Implementacja logiki została rozpoczęta z wykorzystaniem tablic, ze względu na problem serializacji/deserializacji w API, utworzono metody, które umożliwiają mapowanie tych obiektów na listy.

# Jak uruchomić aplikację?

Należy uruchomić projekt API o nazwie Battleship na porcie 443, gdyby API nie uruchomiło się automatycznie na porcie 443 należy dokonać zmiany w pliku launchSettings.json w folderze Properties. Po uruchomieniu API należy przejść do folderu battelship-front/battleship i wywołać komendę npm install, a następnie npm start (wymagane node.js).
