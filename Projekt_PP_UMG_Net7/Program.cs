namespace ProjetkPP_UMG_Net7
{
    using System.Reflection.Metadata.Ecma335;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;
    using System;
    using System.Diagnostics;
    using System.Xml.Serialization;
    using System.Globalization;
    using Funkcje;

#pragma warning disable C8604

    internal class Program
    {

        private static readonly string KodAdminów = "123";

        public static void CzyszczenieEkranu()
        {
            Console.Clear();
            Console.WriteLine();
        }
        public static string ZobaczKod()
        {
            return KodAdminów;
        }
        public static void NaCzerwono(string wiadomość)
        {
            ConsoleColor foregroundColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{wiadomość}");
            Console.ForegroundColor = foregroundColor;
        }

        public static void TworzenieUrzytkownika(DaneLogowania dane, int ID, bool tworzyćKlienta)
        {
            Funkcje.ZapiszPlik(dane, ID);

            if (tworzyćKlienta)
            {
                Directory.CreateDirectory(Funkcje.ŚcieżkaFolderu("UrządzeniaKlienta", ID));
                Directory.CreateDirectory(Funkcje.ŚcieżkaFolderu("AbonamentyKlienta", ID));
                Directory.CreateDirectory(Funkcje.ŚcieżkaFolderu("PakietyKlienta", ID));
                Directory.CreateDirectory(Funkcje.ŚcieżkaFolderu("Faktury", ID));
            }
        }
        public static DaneLogowania? DodajUżytkownika()
        {
            DaneLogowania nowyU = new();
            DaneLogowania[] listaDL = new DaneLogowania[1];
            listaDL = Funkcje.WczytajWszystkiePliki(listaDL);
            string[] listaLoginów = new string[listaDL.Length];
            string[] listaEmaili = new string[listaDL.Length];
            int i = 0;
            bool powtórka;

            foreach (DaneLogowania dana in listaDL)
            {
                listaLoginów[i] = dana.Login;
                listaEmaili[i] = dana.Email;
                i++;
            }

            i = 0;


            Console.WriteLine("\n Tworzenie nowego użytkownika: ");

            do
            {
                powtórka = false;

                Console.Write("\n\n  Podaj login: ");
                nowyU.Login = Console.ReadLine();

                foreach (string login in listaLoginów)
                {
                    if (login == nowyU.Login)
                    {
                        powtórka = true;
                        Console.Write("\n\n   Login zajęty. ");

                        if ((i++) % 3 == 2)
                        {
                            Console.Write("\n     Jeśli chcesz wyjsc wcisnij Escape (esc) ");
                            ConsoleKeyInfo kij = Console.ReadKey(true);

                            if (kij.Key == ConsoleKey.Escape)
                            {
                                return null;
                            }
                        }
                        break;
                    }
                }
            }
            while (powtórka);

            Console.Write("\n  Podaj hasło : ");
            nowyU.Hasło = Console.ReadLine();

            i = 0;

            Console.Write("\n  Podaj imię : ");
            nowyU.Imię = Console.ReadLine();

            Console.Write("\n  Podaj nazwisko : ");
            nowyU.Nazwisko = Console.ReadLine();

            do
            {
                powtórka = false;

                Console.Write("\n  Podaj email : ");
                nowyU.Email = Console.ReadLine();

                foreach (string mail in listaEmaili)
                {
                    if (mail == nowyU.Email)
                    {
                        powtórka = true;
                        Console.WriteLine("\n    Email zajęty");

                        if ((i++) % 3 == 2)
                        {
                            Console.Write("      Jeśli chcesz wyjsc wcisnij Escape (esc) ");
                            ConsoleKeyInfo kij = Console.ReadKey(true);

                            if (kij.Key == ConsoleKey.Escape)
                            {
                                return null;
                            }
                        }
                        break;
                    }
                }
            }
            while (powtórka);

            do
            {
                try
                {
                    powtórka = false;
                    Console.Write("\n  Podaj rok urodzenia : ");
                    nowyU.DataUrodzenia = nowyU.DataUrodzenia.AddYears(int.Parse(Console.ReadLine()) - 1);
                }
                catch (Exception)
                {
                    powtórka = true;
                }

            }
            while (powtórka);

            do
            {
                try
                {
                    powtórka = false;
                    Console.Write("\n  Podaj mieisąc urodzenia : ");
                    nowyU.DataUrodzenia = nowyU.DataUrodzenia.AddMonths(int.Parse(Console.ReadLine()) - 1);
                }
                catch (Exception)
                {
                    powtórka = true;
                }

            }
            while (powtórka);

            do
            {
                try
                {
                    powtórka = false;
                    Console.Write("\n  Podaj dzień urodzenia : ");
                    nowyU.DataUrodzenia = nowyU.DataUrodzenia.AddDays(int.Parse(Console.ReadLine()) - 1);
                }
                catch (Exception)
                {
                    powtórka = true;
                }

            }
            while (powtórka);

            i = 0;

            Console.Write("\n  Zarejestrować jako obsługa? Pamiętaj, że obsługa nie ma folderu Klienta(Y/N) : ");
            string admin = Console.ReadLine();

            if (admin.ToLower() == "y")
            {

                while (true)
                {
                    if (i >= 5)
                    {
                        Console.WriteLine("    Przekroczono liczbę prób, rejestrowanie jako klient");
                        break;
                    }

                    Console.Write("\n  Podaj kod : ");
                    string? kod = Console.ReadLine();
                    i++;

                    if (kod == ZobaczKod())
                    {
                        nowyU.Admin = true;
                        break;
                    }
                    else if (kod == "exit")
                    {
                        break;
                    }
                    else if (i % 3 == 2)
                    {
                        Console.Write("      Jeśli chcesz wyjsc wcisnij Escape (esc) ");
                        ConsoleKeyInfo kij = Console.ReadKey(true);

                        if (kij.Key == ConsoleKey.Escape)
                        {
                            break;
                        }
                    }
                }
            }

            TworzenieUrzytkownika(nowyU, nowyU.ID, !nowyU.Admin);

            return nowyU;
        }
        public static DaneLogowania? Login()
        {
            DaneLogowania[] listaDL = null;
            listaDL = Funkcje.WczytajWszystkiePliki(listaDL);
            DaneLogowania? danaZalogowania = new();
            bool powtórka;

            CzyszczenieEkranu();

            Console.WriteLine(" Wpisz \"utwórz\" w miejsce loginu lub hasła, aby stworzyć nowy profil ");
            Console.WriteLine();

            do
            {
                powtórka = false;

                Console.Write("\n   Podaj login: ");
                string? login = Console.ReadLine();

                if (login == "utwórz")
                {
                    CzyszczenieEkranu();

                    danaZalogowania = DodajUżytkownika();

                    if (danaZalogowania == null)
                    {
                        Console.Write("\n\n     Zrezygnowano z tworzenia użytkownika, kontynuować logowanie? (Y/N): ");
                        string kij = Console.ReadLine();
                        Console.WriteLine("\n");

                        if (kij.ToLower() != "n")
                        {
                            powtórka = true;
                            continue;
                        }
                    }
                    break;
                }

                Console.Write("\n   Podaj hasło: ");
                string? hasło = Console.ReadLine();

                if (hasło == "utwórz")
                {
                    danaZalogowania = DodajUżytkownika();

                    if (danaZalogowania == null)
                    {
                        Console.Write("\n\n     Zrezygnowano z tworzenia użytkownika, kontynuować logowanie? (Y/N): ");
                        ConsoleKeyInfo kij = Console.ReadKey();
                        Console.WriteLine();

                        if (kij.Key != ConsoleKey.N)
                        {
                            powtórka = true;
                            continue;
                        }
                    }
                    break;
                }

                Console.WriteLine();

                if (listaDL.Length == 0)
                {
                    Console.WriteLine("     Nie istnieją żadni urzytkownicy, utwórz nowego urzytkownika podając \"utwórz\" w miejsce loginu lub hasła ");
                    powtórka = true;
                }
                else if (!powtórka)
                {
                    foreach (DaneLogowania dana in listaDL)
                    {
                        if (dana.Login == login && dana.Hasło == hasło)
                        {
                            danaZalogowania = dana;
                            powtórka = false;
                            return danaZalogowania;
                        }
                        else
                        {
                            powtórka = true;
                        }
                    }

                    Console.WriteLine("\n Błedny login lub hasło\n");
                }
            }
            while (powtórka);

            return danaZalogowania;
        }


        #region Funckcje Użytkowników
        public static void TwojeInformacje(ref DaneLogowania twojeDane, bool admin = false)             // 1
        {
            CzyszczenieEkranu();


            Console.Write(" Start -> Menu -> Wyświetl lub modyfikuj informacje o sobie\n\n\n");

            Console.Write(" Twoje dane:\n\n");

            Console.WriteLine("  Imie: " + twojeDane.Imię);
            Console.WriteLine("\n  Nazwisko: " + twojeDane.Nazwisko);
            Console.WriteLine($"\n  Data urodzenia: {twojeDane.DataUrodzenia.Day}-{twojeDane.DataUrodzenia.Month}-{twojeDane.DataUrodzenia.Year}");
            Console.WriteLine($"\n  Email: {twojeDane.Email}");

            string wybor;
            string komunikat = "\n\n    Czy chcesz edytowac dane? (Y/N): ";

            do
            {

                Console.Write(komunikat);
                wybor = Console.ReadLine();
                CzyszczenieEkranu();


                if (wybor.ToLower() == "y")
                {
                    bool powtórka = false;
                    Console.Write(" Start -> Menu -> Wyświetl informacje o sobie\n\n\n");
                    Console.Write(" Które dane chcesz edytowac?\n\n\t1. Imie\n\t2. Nazwisko\n\t3. Data urodzenia\n\t4. Email\n\n Wybierz numer z listy: ");
                    int coEdytowac = int.Parse(Console.ReadLine());
                    CzyszczenieEkranu();


                    Console.Write(" Start -> Menu -> Wyświetl informacje o sobie -> Edycja danych\n\n\n");

                    switch (coEdytowac)
                    {
                        case 1:

                            Console.Write("    Wprowadz imie: ");
                            twojeDane.Imię = Console.ReadLine();
                            Console.WriteLine();
                            break;

                        case 2:

                            Console.Write("    Wprowadz nazwisko: ");
                            twojeDane.Nazwisko = Console.ReadLine();
                            Console.WriteLine();
                            break;

                        case 3:

                            DateOnly czas = new DateOnly();
                            twojeDane.DataUrodzenia = czas;


                            do
                            {
                                try
                                {
                                    powtórka = false;
                                    Console.Write("    Wprowadz datę: \n\n\trok urodzenia: ");
                                    twojeDane.DataUrodzenia = twojeDane.DataUrodzenia.AddYears(int.Parse(Console.ReadLine()) - 1);
                                }
                                catch (Exception)
                                {
                                    powtórka = true;
                                }

                            }
                            while (powtórka);

                            do
                            {
                                try
                                {
                                    powtórka = false;
                                    Console.Write("\n\tmiesiac urodzenia: ");
                                    twojeDane.DataUrodzenia = twojeDane.DataUrodzenia.AddMonths(int.Parse(Console.ReadLine()) - 1);
                                }
                                catch (Exception)
                                {
                                    powtórka = true;
                                }

                            }
                            while (powtórka);

                            do
                            {
                                try
                                {
                                    powtórka = false;
                                    Console.Write("\n\tdzien urodzenia: ");
                                    twojeDane.DataUrodzenia = twojeDane.DataUrodzenia.AddDays(int.Parse(Console.ReadLine()) - 1);
                                    Console.WriteLine();
                                }
                                catch (Exception)
                                {
                                    powtórka = true;
                                }

                            }
                            while (powtórka);
                            break;

                        case 4:

                            Console.Write("    Wprowadz email: ");
                            twojeDane.Email = Console.ReadLine();
                            Console.WriteLine();
                            break;



                    }
                    komunikat = "\n Czy chcesz edytować inne dane? (Y/N): ";
                }
            } while (wybor.ToLower() != "n");

            DaneLogowania doZapisania = twojeDane;
            Funkcje.ZapiszPlik(doZapisania, doZapisania.ID);

        }
        public static void InfoOTwoichUrz(DaneLogowania twojeDane, bool admin = false)                  // 2
        {
            Console.WriteLine(" Wyświetlanie informacji o twoich urządzeniach.\n");

            UrządzenieKlienta[] urzKlTemp = new UrządzenieKlienta[1];
            urzKlTemp = Funkcje.WczytajWszystkiePliki(urzKlTemp, twojeDane.ID);

            UrządzeniaInfo[] urzInf = new UrządzeniaInfo[1];
            urzInf = Funkcje.WczytajWszystkiePliki(urzInf);

            string[] Nazwy = new string[urzKlTemp.Length];

            if (Nazwy.Length < 1)
            {
                Console.WriteLine(" Brak urządzeń.");
                Thread.Sleep(1500);
                return;
            }

            for (int i = 0; i < Nazwy.Length; i++)
            {
                foreach (UrządzeniaInfo UI in urzInf)
                {
                    if (UI.ID == urzKlTemp[i].IDOferty)
                    {
                        Nazwy[i] = UI.Nazwa;
                        break;
                    }
                }
            }

            Console.WriteLine("   Lista urządzeń (od najwczeniej dodanych) :\n");

            for (int i = 1; i <= Nazwy.Length; i++)
            {
                Console.WriteLine($"      {i} - {Nazwy[i - 1]}");
            }



            while (true)
            {

                NaCzerwono("\n   Dowolnym momencie wpisz \"wyjdz\" by wyjść");
                bool powtórka = true;
                int wybraneID = 0;

                do
                {
                    try
                    {
                        powtórka = false;
                        Console.Write("\n   Podaj numer pozycji której informacje checsz wyświetlić : ");
                        string? wprowadzono = Console.ReadLine().ToLower().Replace("ź", "z").Replace(",", ".").Replace(" ", "");
                        Console.WriteLine();

                        if (wprowadzono == "wyjdz")
                        {
                            return;
                        }

                        if (int.Parse(wprowadzono) < 1 || int.Parse(wprowadzono) > Nazwy.Length)
                        {
                            Console.WriteLine("       Błędne wprowadzenie");
                            powtórka = true;
                            continue;
                        }

                        wybraneID = int.Parse(wprowadzono) - 1;

                    }
                    catch (Exception)
                    {
                        powtórka = true;
                    }

                }
                while (powtórka);

                Console.WriteLine($"\n\n   Nazwa : {urzInf[wybraneID].Nazwa} {urzKlTemp[wybraneID].Wariant} \n" +
                    $"   Kolor : {urzKlTemp[wybraneID].Kolor}\n" +
                    $"   Data dodania : {urzKlTemp[wybraneID].DataDodania}\n");
            }

        }
        public static void InfoOTwoichAbo(DaneLogowania twojeDane, bool admin = false)                  // 3
        {
            Console.WriteLine(" Wyświetlanie informacji o twoich abonamentach.\n");

            AbonamentKlienta[] aboKl = new AbonamentKlienta[1];
            aboKl = Funkcje.WczytajWszystkiePliki(aboKl, twojeDane.ID);

            // tylko wyświetlanie, bez modyfikacji
        }
        public static void InfoOTwoichPak(DaneLogowania twojeDane, bool admin = false)                  // 4
        {
            Console.WriteLine(" Wyświetlanie informacji o twoich pakietach.\n");

            PakietKlienta[] pakKl = new PakietKlienta[1];
            pakKl = Funkcje.WczytajWszystkiePliki(pakKl, twojeDane.ID);

            // tylko wyświetlanie, bez modyfikacji
        }
        public static void InfoOOfertachUrz()                                       // 5
        {
            UrządzeniaInfo[] dostępne = new UrządzeniaInfo[3];
            dostępne = Funkcje.WczytajWszystkiePliki(dostępne);

            Console.WriteLine(" Wyświetlanie dostępnych ofert Urządzeń ({0})", dostępne.Length);

            int i = 1;

            foreach (UrządzeniaInfo oferta in dostępne)
            {
                Console.WriteLine($"   {i++} - {oferta.Nazwa}");
            }

            while (true)
            {

                NaCzerwono("\n   Dowolnym momencie wpisz \"wyjdz\" by wyjść");
                bool powtórka = true;
                int wybraneID = 0;

                do
                {
                    powtórka = false;
                    try
                    {
                        Console.Write("\n   Podaj numer pozycji której informacje checsz wyświetlić : ");
                        string? wprowadzono = Console.ReadLine().ToLower().Replace("ź", "z").Replace(",", ".").Replace(" ", "");
                        Console.WriteLine();

                        if (wprowadzono == "wyjdz")
                        {
                            return;
                        }

                        if (int.Parse(wprowadzono) < 1 || int.Parse(wprowadzono) > i)
                        {
                            Console.WriteLine("       Błędne wprowadzenie");
                            powtórka = true;
                            continue;
                        }

                        wybraneID = int.Parse(wprowadzono) - 1;

                    }
                    catch (Exception)
                    {
                        powtórka = true;
                        Console.WriteLine("       Błędne wprowadzenie");
                    }

                }
                while (powtórka);

                CzyszczenieEkranu();
                Console.WriteLine($" Nazwa : {dostępne[wybraneID].Nazwa}");
                Console.WriteLine($" Wytwóra : {dostępne[wybraneID].Wytwórca}");
                Console.WriteLine($" Cena : {dostępne[wybraneID].Cena}");

                Console.WriteLine("\n Dostępne warianty : ");
                foreach(string wariant in dostępne[wybraneID].Warianty)
                {
                    Console.WriteLine($"   - {wariant}");
                }

                Console.WriteLine("\n Dostępne kolory : ");
                foreach (string kolor in dostępne[wybraneID].Kolory)
                {
                    Console.WriteLine($"   - {kolor}");
                }

                Console.Write("\n\n   Wpisz \"kup\" aby przejść do kupna, wciśnij enter by wrócić");
                string? wybór = Console.ReadLine().ToLower().Replace('ó', 'u').Trim();
                
                if(wybór == "kup")
                {
                    // funkcja zakupu,
                    // Karze płacić (podać informacje do faktury).
                    // Dodaje pozycję do klienta i tworzy jej fakturę
                }
            }

        }
        public static void InfoOOfertachAbo()                                       // 6
        {
            AbonamentyInfo[] dostępne = new AbonamentyInfo[3];
            dostępne = Funkcje.WczytajWszystkiePliki(dostępne);

            Console.WriteLine(" Wyświetlanie dostępnych ofert Abonamntów");

            int i = 1;

            foreach (AbonamentyInfo oferta in dostępne)
            {
                Console.WriteLine($"   {i++} - {oferta.Nazwa}");
            }

            while (true)
            {

                NaCzerwono("\n   Dowolnym momencie wpisz \"wyjdz\" by wyjść");
                bool powtórka = true;
                int wybraneID = 0;

                do
                {
                    try
                    {
                        powtórka = false;
                        Console.Write("\n   Podaj numer pozycji której informacje checsz wyświetlić : ");
                        string? wprowadzono = Console.ReadLine().ToLower().Replace("ź", "z").Replace(",", ".").Replace(" ", "");
                        Console.WriteLine();

                        if (wprowadzono == "wyjdz")
                        {
                            return;
                        }

                        if (int.Parse(wprowadzono) < 1 || int.Parse(wprowadzono) > i)
                        {
                            Console.WriteLine("       Błędne wprowadzenie");
                            powtórka = true;
                            continue;
                        }

                        wybraneID = int.Parse(wprowadzono) - 1;

                    }
                    catch (Exception)
                    {
                        powtórka = true;
                        Console.WriteLine("       Błędne wprowadzenie");
                    }

                }
                while (powtórka);

                CzyszczenieEkranu();
                Console.WriteLine($"  : {dostępne[wybraneID].Nazwa}");
            }

        }
        public static void InfoOOfertachPak()                                       // 7
        {
            PakietyInfo[] dostępne= new PakietyInfo[3];
            dostępne= Funkcje.WczytajWszystkiePliki(dostępne);

            Console.WriteLine(" Wyświetlanie dostępnych ofert Pakietów");

            int i = 1;

            foreach (PakietyInfo oferta in dostępne)
            {
                Console.WriteLine($"   {i++} - {oferta.Nazwa}");
            }

            while (true)
            {

                NaCzerwono("\n   Dowolnym momencie wpisz \"wyjdz\" by wyjść");
                bool powtórka = true;
                int wybraneID = 0;

                do
                {
                    try
                    {
                        powtórka = false;
                        Console.Write("\n   Podaj numer pozycji której informacje checsz wyświetlić : ");
                        string? wprowadzono = Console.ReadLine().ToLower().Replace("ź", "z").Replace(",", ".").Replace(" ", "");
                        Console.WriteLine();

                        if (wprowadzono == "wyjdz")
                        {
                            return;
                        }

                        if (int.Parse(wprowadzono) < 1 || int.Parse(wprowadzono) > i)
                        {
                            Console.WriteLine("       Błędne wprowadzenie");
                            powtórka = true;
                            continue;
                        }

                        wybraneID = int.Parse(wprowadzono) - 1;

                    }
                    catch (Exception)
                    {
                        powtórka = true;
                        Console.WriteLine("       Błędne wprowadzenie");
                    }

                }
                while (powtórka);

                CzyszczenieEkranu();
                Console.WriteLine(" ");
            }

        }
        #endregion

        #region Funkcje Admina
        public static void InfoOKlientach()                                         // 1
        {
            Console.WriteLine(" Wyświetlanie informacji o klientach");

            WszystkieDaneKlienta[] daneKlientów = Funkcje.WczytajWszystkieDaneKlientów();       // pełna list danych klientów (nie wszystkich urzytkowników, bo admini nie mają urządzeń, abonamentów ani pliktów)

            if (daneKlientów.Length == 0)
            {
                Console.WriteLine("\n  Nie istnieją żadni Klienci");
                Thread.Sleep(2000);
                return;
            }

            /*
             * Dodać :
             * 1) Znajdujące się wewnątrz pentli (z której wychodzi się wpisyjąć "wyjdź" (lub wyjdz)) menu wypisujące listę urzytkowników : numer opcji) imię i nazwisko, id
             * 2) Po wybraniu klienta przechodzi do kolejnej pentli (z której wychodzi się wpisyjąć "wyjdź") i wypisuje listę w formacie :
             * 
             *  Imię i Nazwisko, ID
             *    Dane Osobiste
             *      ...(lista danych)
             *    
             *    Urządzenia
             *      ...(ponumerowana lista urządzeń (licznik od 1))
             *    
             *    Abonamenty
             *      ...(ponumerowana lista abonametów (licznik kontynuowany od ostatniej liczby w urządzeniach))
             *    
             *    Pakiety
             *      ...(ponumerowana lista pakietów (licznik kontynuowany od ostatniej liczby w abonamentach))
             *      
             * 3) Po wybraniu numeru należy wyświetlić listę wartości danej pozycji. Zmienianie jak w zmienianiu właściwości urzytkownika (z menu urzytkownika)
             */

        }
        public static void ModyfikacjaOfertUrz()                                    // 2
        {
            Console.WriteLine(" Modyfikujacja dostepnych ofert Urządzeń");

            UrządzeniaInfo[] urzLi = new UrządzeniaInfo[1];
            urzLi = Funkcje.WczytajWszystkiePliki(urzLi);

            /*
             * Dodać :
             * 1) Znajdujące się wewnątrz pentli (z której wychodzi się wpisyjąć "wyjdź" (lub wyjdz)) menu wypisujące listę urządzeń : numer opcji) Nazwa, id
             *      
             * 2) Po wybraniu numeru należy wyświetlić listę wartości danej pozycji. Zmienianie jak w zmienianiu właściwości urzytkownika (z menu urzytkownika)
             */
        }
        public static void ModyfikacjaOfertAbo()                                    // 3
        {
            Console.WriteLine(" Modyfikujacja dostepnych ofert Abonamentów");

            UrządzeniaInfo[] aboLi = new UrządzeniaInfo[1];
            aboLi = Funkcje.WczytajWszystkiePliki(aboLi);

            /*
             * Dodać :
             * 1) Znajdujące się wewnątrz pentli (z której wychodzi się wpisyjąć "wyjdź" (lub wyjdz)) menu wypisujące listę urządzeń : numer opcji) Nazwa, id
             *      
             * 2) Po wybraniu numeru należy wyświetlić listę wartości danej pozycji. Zmienianie jak w zmienianiu właściwości urzytkownika (z menu urzytkownika)
             */
        }
        public static void ModyfikacjaOfertPak()                                    // 4
        {
            Console.WriteLine(" Modyfikujacja dostepnych ofert Pakietów");

            UrządzeniaInfo[] pakLi = new UrządzeniaInfo[1];
            pakLi = Funkcje.WczytajWszystkiePliki(pakLi);

            /*
             * Dodać :
             * 1) Znajdujące się wewnątrz pentli (z której wychodzi się wpisyjąć "wyjdź" (lub wyjdz)) menu wypisujące listę urządzeń : numer opcji) Nazwa, id
             *      
             * 2) Po wybraniu numeru należy wyświetlić listę wartości danej pozycji. Zmienianie jak w zmienianiu właściwości urzytkownika (z menu urzytkownika)
             */
        }
        public static void DodajOfertęUrz()                                         // 5
        {
            Console.WriteLine(" Dodawanie nowej oferty Urzadzenia");

            UrządzeniaInfo noweUrz = new();
            UrządzeniaInfo[] listaUrz = new UrządzeniaInfo[1];
            listaUrz = Funkcje.WczytajWszystkiePliki(listaUrz);
            string[] listaNazw = new string[listaUrz.Length];
            int i = 0;
            bool powtórka;

            foreach (UrządzeniaInfo dana in listaUrz)
            {
                listaNazw[i] = dana.Nazwa;
                i++;
            }

            NaCzerwono("\n\n W dowolnym momencie wpisz \"wyjdz\" aby wyjść do menu");
            Console.WriteLine("\n Tworzenie nowego urzadzenia : ");

            do
            {
                powtórka = false;

                Console.Write("\n  Podaj nazwe: ");
                noweUrz.Nazwa = Console.ReadLine();

                if (noweUrz.Nazwa.ToLower() == "wyjdz")
                {
                    return;
                }

                foreach (string nazwa in listaNazw)
                {
                    if (nazwa == noweUrz.Nazwa)
                    {
                        powtórka = true;
                        Console.Write("\n    Nazwa juz istnieje");

                        break;
                    }
                }
            }
            while (powtórka);

            do
            {
                try
                {
                    powtórka = false;
                    Console.Write("\n  Podaj cene: ");
                    string wprowadzono = Console.ReadLine().ToLower().Replace("ź", "z").Replace(",", ".").Replace(" ", "");

                    if (wprowadzono == "wyjdz")
                    {
                        return;
                    }

                    if (noweUrz.Cena < 0)
                    {
                        powtórka = true;
                        continue;
                    }

                    noweUrz.Cena = double.Parse(wprowadzono);
                }
                catch (Exception)
                {
                    powtórka = true;
                }

            }
            while (powtórka);

            Console.Write("\n  Podaj wytwórce: ");
            noweUrz.Wytwórca = Console.ReadLine();

            if (noweUrz.Wytwórca.ToLower() == "wyjdz")
            {
                return;
            }

            Console.Write("\n  Podaj liczbę wariantów: ");
            int ilośćWariantów = int.Parse(Console.ReadLine());
            noweUrz.Warianty = new string[ilośćWariantów];

            for (int j = 0; j < ilośćWariantów; j++)
            {
                Console.Write($"Podaj {j + 1} wariant:");
                noweUrz.Warianty[j] = Console.ReadLine();

                if (noweUrz.Warianty[j].ToLower() == "wyjdz")
                {
                    return;
                }
            }

            Console.Write("\n  Podaj liczbę kolorów: ");
            int ilośćKolorów = int.Parse(Console.ReadLine());
            noweUrz.Kolory = new string[ilośćKolorów];

            for (int j = 0; j < ilośćKolorów; j++)
            {
                Console.Write($"Podaj {j + 1} wariant: ");
                noweUrz.Kolory[j] = Console.ReadLine();

                if (noweUrz.Kolory[j].ToLower() == "wyjdz")
                {
                    return;
                }
            }

            Funkcje.ZapiszPlik(noweUrz, noweUrz.ID);

        }
        public static void DodajOfertęAbo()                                         // 6
        {
            Console.WriteLine(" Dodawanie nowej oferty Abonamentu");

            AbonamentyInfo nowyAbo = new();
            AbonamentyInfo[] listaAbo = new AbonamentyInfo[1];
            listaAbo = Funkcje.WczytajWszystkiePliki(listaAbo);
            string[] listaNazw = new string[listaAbo.Length];
            int i = 0;
            bool powtórka;

            foreach (AbonamentyInfo dana in listaAbo)
            {
                listaNazw[i] = dana.Nazwa;
                i++;
            }

            NaCzerwono("\n\n W dowolnym momencie wpisz \"wyjdz\" aby wyjść do menu");
            Console.WriteLine("\n Tworzenie nowego urzadzenia : ");

            do
            {
                powtórka = false;

                Console.Write("\n  Podaj nazwe: ");
                nowyAbo.Nazwa = Console.ReadLine();

                if (nowyAbo.Nazwa.ToLower() == "wyjdz")
                {
                    return;
                }

                foreach (string nazwa in listaNazw)
                {
                    if (nazwa == nowyAbo.Nazwa)
                    {
                        powtórka = true;
                        Console.Write("\n    Nazwa zajęta");

                        break;
                    }
                }
            }
            while (powtórka);

            do
            {
                try
                {
                    powtórka = false;
                    Console.Write("\n  Podaj cene: ");
                    string wprowadzono = Console.ReadLine().ToLower().Replace("ź", "z").Replace(" ", "").Replace(",", ".");

                    if (wprowadzono == "wyjdz")
                    {
                        return;
                    }

                    if (nowyAbo.Cena < 0)
                    {
                        powtórka = true;
                        continue;
                    }

                    nowyAbo.Cena = double.Parse(wprowadzono);
                }
                catch (Exception)
                {
                    powtórka = true;
                }

            }
            while (powtórka);

            Console.Write("\n  Podaj częstotliwośc rozliczenia ( dzień / tydzień / miesiąc ): ");
            nowyAbo.CzęstotliwośćRozliczania = Console.ReadLine();

            if (nowyAbo.CzęstotliwośćRozliczania.ToLower().Replace("ź", "z") == "wyjdz")
            {
                return;
            }

            try
            {
                Console.Write("\n  Podaj limit internetu (w GB), -1 dla nielimitowanego, 0 dla braku internetu: ");
                string wprowadzone = Console.ReadLine().ToLower().Replace("ź", "z").Replace(",", ".").Replace(" ", "");

                if (wprowadzone == "wyjdz")
                {
                    return;
                }

                nowyAbo.LimitInternetu = double.Parse(wprowadzone);
            }
            catch (Exception) { }

            try
            {
                Console.Write("\n  Podaj dolny limit szybkości internetu (w kb/s), 0 dla braku internetu: ");
                string wprowadzone = Console.ReadLine().ToLower().Replace("ź", "z").Replace(" ", "").Replace(",", ".");

                if (wprowadzone == "wyjdz")
                {
                    return;
                }

                nowyAbo.LimityPrędkości[0] = double.Parse(wprowadzone);
            }
            catch (Exception) { }

            try
            {
                Console.Write("\n  Podaj górny limit szybkości internetu (w kb/s), 0 dla braku internetu: ");
                string wprowadzone = Console.ReadLine().ToLower().Replace("ź", "z").Replace(" ", "").Replace(",", ".");

                if (wprowadzone == "wyjdz")
                {
                    return;
                }

                nowyAbo.LimityPrędkości[1] = double.Parse(wprowadzone);
            }
            catch (Exception) { }


            Funkcje.ZapiszPlik(nowyAbo, nowyAbo.ID);

        }
        public static void DodajOfertęPak()                                         // 7
        {
            Console.WriteLine(" Dodawanie nowej oferty Pakietu");

        }
        #endregion

        static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = new CultureInfo("pl-PL");

            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            bool zostań = true;

            Funkcje.CzyIstniejąWszystkieFoldery();

            do
            {
                DaneLogowania? zalogowanyUrzytkownik = Login();

                if (zalogowanyUrzytkownik == null)
                {
                    Console.WriteLine(" Anulowano logowanie");
                    return;
                }

                do
                {
                    zostań = true;
                    CzyszczenieEkranu();

                    if (!zalogowanyUrzytkownik.Admin)
                    {
                        Console.Write(" Start -> Menu \t\t |Zalogowano jako: " + zalogowanyUrzytkownik.Imię + "|");
                        Console.WriteLine("\n\n Dostępne opcje: " +
                            "\n\n\t 1 - Wyświetl lub modyfikuj informacje o sobie \n" +

                            "\n\t 2 - Wyświetl informacje o swoich urządzeniach" +
                            "\n\t 3 - Wyświetl informacje o swoich abonamntach" +
                            "\n\t 4 - Wyświetl informacje o swoich pakietach \n" +

                            "\n\t 5 - Pokaż dostepne oferty Urządzeń" +
                            "\n\t 6 - Pokaż dostepne oferty Abonamentów" +
                            "\n\t 7 - Pokaż dostepne oferty Pakietów \n" +

                            "\n\t Wyloguj - Wyloguj się ze swojego konta " +
                            "\n\t Wyjdz - Wyjdź z programu");
                        Console.Write("\n\n\t");

                        string? opcja = Console.ReadLine();
                        if (opcja == null)
                        {
                            opcja = "";
                        }

                        opcja = opcja.Replace(" ", "");
                        opcja = opcja.ToLower();

                        switch (opcja)
                        {
                            case ("1"):
                                {
                                    CzyszczenieEkranu();
                                    TwojeInformacje(ref zalogowanyUrzytkownik);
                                    break;
                                }
                            case ("2"):
                                {
                                    CzyszczenieEkranu();
                                    InfoOTwoichUrz(zalogowanyUrzytkownik);
                                    break;
                                }
                            case ("3"):
                                {
                                    CzyszczenieEkranu();
                                    InfoOTwoichAbo(zalogowanyUrzytkownik);
                                    break;
                                }
                            case ("4"):
                                {
                                    CzyszczenieEkranu();
                                    InfoOTwoichPak(zalogowanyUrzytkownik);
                                    break;
                                }
                            case ("5"):
                                {
                                    CzyszczenieEkranu();
                                    InfoOOfertachUrz();
                                    break;
                                }
                            case ("6"):
                                {
                                    CzyszczenieEkranu();
                                    InfoOOfertachAbo();
                                    break;
                                }
                            case ("7"):
                                {
                                    CzyszczenieEkranu();
                                    InfoOOfertachPak();
                                    break;
                                }
                            case ("wyloguj"):
                                {
                                    zostań = false;
                                    break;
                                }
                            case ("wyjdz"):
                                {
                                    zostań = false;
                                    return;
                                }
                            default:
                                {
                                    Console.WriteLine("Błędna komenda");
                                    break;
                                }
                        }
                    }
                    else
                    {
                        Console.Write(" Start -> Menu \t\t |Zalogowano jako Admin: " + zalogowanyUrzytkownik.Imię + "|");
                        Console.WriteLine("\n\n\n Dostępne opcje: " +
                            "\n\t 1 - Wyświetl informacje o klientach \n" +

                            "\n\t 2 - Pokaż i modyfikuj dostepne oferty Urządzeń" +
                            "\n\t 3 - Pokaż i modyfikuj dostepne oferty Abonamentów" +
                            "\n\t 4 - Pokaż i modyfikuj dostepne oferty Pakietów \n" +

                            "\n\t 5 - Dodaj nową ofertę Urządzenia" +
                            "\n\t 6 - Dodaj nową ofertę Abonamentu" +
                            "\n\t 7 - Dodaj nową ofertę Pakietu \n" +

                            "\n\t Wyloguj - Wyloguj się ze swojego konta " +
                            "\n\t Wyjdź - Wyjdź z programu");
                        Console.Write("\n\n\t");

                        string? opcja = Console.ReadLine();
                        if (opcja == null)
                        {
                            opcja = "";
                        }

                        opcja = opcja.Replace("ź", "z");
                        opcja = opcja.Replace(" ", "");
                        opcja = opcja.ToLower();

                        switch (opcja)
                        {
                            case ("1"):
                                {
                                    CzyszczenieEkranu();
                                    InfoOKlientach();
                                    break;
                                }
                            case ("2"):
                                {
                                    CzyszczenieEkranu();
                                    ModyfikacjaOfertUrz();
                                    break;
                                }
                            case ("3"):
                                {
                                    CzyszczenieEkranu();
                                    ModyfikacjaOfertAbo();
                                    break;
                                }
                            case ("4"):
                                {
                                    CzyszczenieEkranu();
                                    ModyfikacjaOfertPak();
                                    break;
                                }
                            case ("5"):
                                {
                                    CzyszczenieEkranu();
                                    DodajOfertęUrz();
                                    break;
                                }
                            case ("6"):
                                {
                                    CzyszczenieEkranu();
                                    DodajOfertęAbo();
                                    break;
                                }
                            case ("7"):
                                {
                                    CzyszczenieEkranu();
                                    DodajOfertęPak();
                                    break;
                                }
                            case ("wyloguj"):
                                {
                                    zostań = false;
                                    zalogowanyUrzytkownik = null;
                                    break;
                                }
                            case ("wyjdz"):
                                {
                                    zostań = false;
                                    return;
                                }
                            default:
                                {
                                    Console.WriteLine("Błędna komenda");
                                    break;
                                }
                        }
                    }
                }
                while (zostań);
            }
            while (true);

        }
    }
}