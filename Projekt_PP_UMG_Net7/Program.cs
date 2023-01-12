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
    using System.Diagnostics.Metrics;
    using System.Collections.Immutable;

#pragma warning disable C8604

    internal class Program
    {
        private static readonly string KodAdminów = "123";

        #region Proste Funckcje Ogólne
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
        public static double ZróbDouble(string wprowadzenie)
        {
            double liczba = 0.0;

            try
            {
                wprowadzenie = wprowadzenie.Replace(" ", "").Replace(".", ",");
                liczba = double.Parse(wprowadzenie);
            }
            catch (Exception)
            {
                liczba = -2.0;
            }

            return liczba;
        }
        public static int ZróbInt(string wprowadzenie)
        {
            int liczba = 0;
            double tempLiczba = 0.0;

            try
            {
                wprowadzenie = wprowadzenie.Replace(" ", "").Replace(".", ",");
                tempLiczba = double.Parse(wprowadzenie);
                liczba = (int)tempLiczba;
                if (tempLiczba - liczba >= 0.5) liczba++;
                if (tempLiczba - liczba <= -0.5) liczba--;
            }
            catch (Exception)
            {
                liczba = -2;
            }

            return liczba;
        }
        public static bool CzyWyjść(string wprowadzenie)
        {
            wprowadzenie = wprowadzenie.ToLower();
            wprowadzenie = wprowadzenie.Replace(" ", "").Replace("ź", "z");
            return (wprowadzenie == "wyjdz");
        }
        #endregion

        #region Funckcje Logowania i Rejestracji
        public static void TworzenieUrzytkownika(DaneLogowania dane, int ID, bool tworzyćKlienta)
        {
            Funkcje.ZapiszPlik(dane, ID.ToString());
            string[] zawarteFoldery = { "UrządzeniaKlienta", "AbonamentyKlienta", "PakietyKlienta"/*, "Faktury" */};

            if (tworzyćKlienta)
            {
                foreach (string folder in zawarteFoldery)
                {
                    Directory.CreateDirectory(Funkcje.ŚcieżkaFolderu(folder, ID));
                }
            }
        }
        public static DaneLogowania? DodajUżytkownika()
        {
            Console.Title = "Rejestracja";

            DaneLogowania nowyU = new();
            DaneLogowania[] listaDL = new DaneLogowania[1];
            DateOnly tempData = new DateOnly();
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

                Console.Write("\n\n   Podaj login: ");
                nowyU.Login = Console.ReadLine();

                foreach (string login in listaLoginów)
                {
                    if (login == nowyU.Login)
                    {
                        powtórka = true;
                        Console.Write("\n\n    Login zajęty. ");

                        if ((i++) % 3 == 2)
                        {
                            Console.Write("\n       Jeśli chcesz wyjsc wcisnij Escape (esc) ");
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

            Console.Write("\n   Podaj hasło: ");
            nowyU.Hasło = Console.ReadLine();

            i = 0;

            Console.Write("\n   Podaj imię: ");
            nowyU.Imię = Console.ReadLine();

            Console.Write("\n   Podaj nazwisko: ");
            nowyU.Nazwisko = Console.ReadLine();

            do
            {
                powtórka = false;

                Console.Write("\n   Podaj email: ");
                nowyU.Email = Console.ReadLine();

                foreach (string mail in listaEmaili)
                {
                    if (mail == nowyU.Email)
                    {
                        powtórka = true;
                        Console.WriteLine("\n      Email zajęty");

                        if ((i++) % 3 == 2)
                        {
                            Console.Write("         Jeśli chcesz wyjsc wcisnij Escape (esc) ");
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
                    Console.Write("\n   Podaj rok urodzenia: ");
                    tempData = tempData.AddYears(int.Parse(Console.ReadLine()) - 1);
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
                    Console.Write("\n   Podaj mieisąc urodzenia: ");
                    tempData = tempData.AddMonths(int.Parse(Console.ReadLine()) - 1);
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
                    Console.Write("\n   Podaj dzień urodzenia: ");
                    tempData = tempData.AddDays(int.Parse(Console.ReadLine()) - 1);
                }
                catch (Exception)
                {
                    powtórka = true;
                }

            }
            while (powtórka);

            nowyU.DataUrodzenia = tempData.ToString();
            i = 0;

            Console.Write("\n   Zarejestrować jako obsługa? Pamiętaj, że obsługa nie ma folderu Klienta(Y/N): ");
            string admin = Console.ReadLine();

            if (admin.ToLower() == "y")
            {

                while (true)
                {
                    if (i >= 5)
                    {
                        Console.WriteLine("      Przekroczono liczbę prób, rejestrowanie jako klient");
                        break;
                    }

                    Console.Write("\n   Podaj kod: ");
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
                        Console.Write("         Jeśli chcesz wyjsc wcisnij Escape (esc) ");
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
            Console.Title = "Logownie";

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
                        Console.Write("\n\n       Zrezygnowano z tworzenia użytkownika, kontynuować logowanie? (Y/N): ");
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
                        Console.Write("\n\n       Zrezygnowano z tworzenia użytkownika, kontynuować logowanie? (Y/N): ");
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
                    Console.WriteLine("       Nie istnieją żadni urzytkownicy, utwórz nowego urzytkownika podając \"utwórz\" w miejsce loginu lub hasła ");
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
        #endregion

        #region Funckcje Użytkowników
        public static void TwojeInformacje(ref DaneLogowania daneUrzytkownika, bool admin = false)                   // 1
        {

            Console.Title = "Informacje o urztykowniku";

            CzyszczenieEkranu();

            DateOnly tempDate = DateOnly.Parse(daneUrzytkownika.DataUrodzenia);

            if (!admin)
                Console.WriteLine(" Twoje dane:\n\n");
            else
                Console.WriteLine($" Dane urzytkownaika {daneUrzytkownika.Imię} {daneUrzytkownika.Nazwisko}");

            Console.WriteLine($"   Imie: {daneUrzytkownika.Imię}" +
                  $"\n   Nazwisko: {daneUrzytkownika.Nazwisko}" +
                  $"\n   Data urodzenia: {tempDate.Day}.{tempDate.Month}.{tempDate.Year}" +
                  $"\n   Email: {daneUrzytkownika.Email}");

            string wybór;
            string komunikat = "\n\n      Czy chcesz edytowac dane? (Y/N): ";

            do
            {
                Console.Write(komunikat);
                wybór = Console.ReadLine();


                if (wybór.ToLower().Trim() == "y")
                {
                    bool powtórka = false;
                    Console.Write("\n\n   Które dane chcesz edytowac?\n" +
                          "\n      1. Imię" +
                          "\n      2. Nazwisko" +
                          "\n      3. Data urodzenia" +
                          "\n      4. Email" +
                          "\n\n   Wybierz numer z listy: ");

                    int coEdytować = ZróbInt(Console.ReadLine());
                    CzyszczenieEkranu();

                    Console.WriteLine("\n\n");

                    switch (coEdytować)
                    {
                        case 1:

                            Console.Write("      Wprowadz imie: ");
                            daneUrzytkownika.Imię = Console.ReadLine();
                            Console.WriteLine();
                            break;

                        case 2:

                            Console.Write("      Wprowadz nazwisko: ");
                            daneUrzytkownika.Nazwisko = Console.ReadLine();
                            Console.WriteLine();
                            break;

                        case 3:

                            DateOnly pustyCzas = new DateOnly();

                            do
                            {
                                try
                                {
                                    powtórka = false;
                                    Console.Write("      Wprowadz datę: \n\n   rok urodzenia: ");
                                    pustyCzas = pustyCzas.AddYears(ZróbInt(Console.ReadLine()) - 1);
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
                                    Console.Write("\n   Miesiac urodzenia: ");
                                    pustyCzas = pustyCzas.AddMonths(ZróbInt(Console.ReadLine()) - 1);
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
                                    Console.Write("\n   Dzien urodzenia: ");
                                    pustyCzas = pustyCzas.AddDays(ZróbInt(Console.ReadLine()) - 1);
                                    Console.WriteLine();
                                }
                                catch (Exception)
                                {
                                    powtórka = true;
                                }

                            }
                            while (powtórka);

                            daneUrzytkownika.DataUrodzenia = pustyCzas.ToString();
                            break;

                        case 4:

                            Console.Write("      Wprowadz email: ");
                            daneUrzytkownika.Email = Console.ReadLine();
                            Console.WriteLine();
                            break;
                    }

                    komunikat = "\n Czy chcesz edytować inne dane? (Y/N): ";
                }
            }
            while (wybór.ToLower().Trim() != "n");

            Funkcje.ZapiszPlik(daneUrzytkownika, daneUrzytkownika.ID.ToString());

        }
        public static void InfoOTwoichUrz(DaneLogowania daneUrzytkownika, bool admin = false)                           // 2
        {

            Console.Title = "Informacje o urządzeniach urztykownika";

            while (true)
            {
                CzyszczenieEkranu();
                Console.WriteLine();

                if (!admin)
                    Console.WriteLine(" Wyświetlanie informacji o twoich urządzeniach.\n");
                else
                    Console.WriteLine($" Wyświetlanie informacji o urządzeniach klienta {daneUrzytkownika.Imię} {daneUrzytkownika.Nazwisko}");

                UrządzenieKlienta[] urzKlienta = new UrządzenieKlienta[1];
                urzKlienta = Funkcje.WczytajWszystkiePliki(urzKlienta, daneUrzytkownika.ID);

                string[] Nazwy = new string[urzKlienta.Length];
                if (Nazwy.Length < 1)
                {
                    Console.WriteLine(" Brak urządzeń.");
                    Thread.Sleep(1500);
                    return;
                }

                UrządzeniaInfo wybraneUrzInfo = (UrządzeniaInfo)Funkcje.WczytajPlik("UrządzeniaInfo", 1.ToString());
                PakietKlienta dołączonyPak = null;
                PakietyInfo dołączonyPakInfo = null;

                Console.WriteLine("   Lista urządzeń (od najwczeniej dodanych):\n");

                for (int i = 1; i <= Nazwy.Length; i++)
                {
                    wybraneUrzInfo = (UrządzeniaInfo)Funkcje.WczytajPlik("UrządzeniaInfo", urzKlienta[i - 1].IDOferty.ToString());
                    Console.WriteLine($"         {i} - {wybraneUrzInfo.Nazwa}");
                }

                NaCzerwono("\n   Dowolnym momencie wpisz \"wyjdz\" by wyjść");
                bool powtórka = true;
                int wybraneID = 0;

                do
                {
                    try
                    {
                        powtórka = false;
                        Console.Write("\n   Podaj numer pozycji którą checsz zobaczyć: ");
                        string? wprowadzono = Console.ReadLine();
                        Console.WriteLine();

                        if (CzyWyjść(wprowadzono))
                            return;

                        if (ZróbInt(wprowadzono) < 1 || ZróbInt(wprowadzono) > Nazwy.Length)
                        {
                            Console.WriteLine("      Błędne wprowadzenie");
                            powtórka = true;
                            continue;
                        }

                        wybraneID = ZróbInt(wprowadzono) - 1;
                        wybraneUrzInfo = (UrządzeniaInfo)Funkcje.WczytajPlik("UrządzeniaInfo", urzKlienta[wybraneID].IDOferty.ToString());

                        if (urzKlienta[wybraneID].IDPakietu > 0)
                        {
                            dołączonyPak = (PakietKlienta)Funkcje.WczytajPlik("PakietyKlienta", urzKlienta[wybraneID].IDPakietu.ToString(), daneUrzytkownika.ID);
                            dołączonyPakInfo = (PakietyInfo)Funkcje.WczytajPlik("PakietyInfo", dołączonyPak.IDOferty.ToString(), daneUrzytkownika.ID);
                        }

                    }
                    catch (Exception)
                    {
                        powtórka = true;
                    }

                }
                while (powtórka);

                string pakiet = "Nie jest częścią pakietu";
                if (dołączonyPakInfo != null)
                    pakiet = dołączonyPakInfo.Nazwa;

                Console.WriteLine($"\n\n   Nazwa: {wybraneUrzInfo.Nazwa} {urzKlienta[wybraneID].Wariant} \n" +
                      $"   Kolor: {urzKlienta[wybraneID].Kolor}\n" +
                      $"   Pakiet: {pakiet}\n" +
                      $"   Data dodania: {urzKlienta[wybraneID].DataDodania}\n\n\n");

                if (!admin)
                {
                    Console.WriteLine("  Wciśnij enter, by kontynuować");
                    Console.ReadLine();
                }
                else
                {
                    NaCzerwono("\n\n\n   Dowolnym momencie wpisz \"wyjdz\" by wyjść");
                    Console.Write("\n  Usunąć pozycję? (Y/N)");
                    string wybór = Console.ReadLine().ToLower();
                    Console.WriteLine();

                    if (CzyWyjść(wybór))
                        return;

                    if (wybór.Trim() == "y")
                        Funkcje.UsuńPlik("UrządzeniaKlienta", urzKlienta[wybraneID].ID.ToString(), daneUrzytkownika.ID);
                }
            }
        }
        public static void InfoOTwoichAbo(DaneLogowania daneUrzytkownika, bool admin = false)                           // 3
        {
            Console.Title = "Informacje o abonamentach urzytkownika";

            while (true)
            {
                CzyszczenieEkranu();

                if (!admin)
                    Console.WriteLine(" Wyświetlanie informacji o twoich Abonamentach.\n");
                else
                    Console.WriteLine($" Wyświetlanie informacji o abonamentch klienta {daneUrzytkownika.Imię} {daneUrzytkownika.Nazwisko}");

                AbonamentKlienta[] aboKlienta = new AbonamentKlienta[1];
                aboKlienta = Funkcje.WczytajWszystkiePliki(aboKlienta, daneUrzytkownika.ID);

                string[] Nazwy = new string[aboKlienta.Length];
                if (Nazwy.Length < 1)
                {
                    Console.WriteLine(" Brak abonamentów.");
                    Thread.Sleep(1500);
                    return;
                }

                AbonamentyInfo wybranyAboInfo = (AbonamentyInfo)Funkcje.WczytajPlik("AbonamentyInfo", "1");
                PakietKlienta dołączonyPak = null;
                PakietyInfo dołączonyPakInfo = null;

                Console.WriteLine("   Lista abonamentów (od najwczeniej dodanych):\n");

                for (int i = 1; i <= Nazwy.Length; i++)
                {
                    wybranyAboInfo = (AbonamentyInfo)Funkcje.WczytajPlik("AbonamentyInfo", aboKlienta[i - 1].IDOferty.ToString());
                    Console.WriteLine($"         {i} - {wybranyAboInfo.Nazwa}");
                }

                NaCzerwono("\n   Dowolnym momencie wpisz \"wyjdz\" by wyjść");
                bool powtórka = true;
                int wybraneID = 0;

                do
                {
                    try
                    {
                        powtórka = false;
                        Console.Write("\n   Podaj numer pozycji którą checsz zobaczyć: ");
                        string? wprowadzono = Console.ReadLine();
                        Console.WriteLine();

                        if (CzyWyjść(wprowadzono))
                            return;

                        if (ZróbInt(wprowadzono) < 1 || ZróbInt(wprowadzono) > Nazwy.Length)
                        {
                            Console.WriteLine("      Błędne wprowadzenie");
                            powtórka = true;
                            continue;
                        }

                        wybraneID = ZróbInt(wprowadzono) - 1;
                        wybranyAboInfo = (AbonamentyInfo)Funkcje.WczytajPlik("AbonamentyInfo", aboKlienta[wybraneID].IDOferty.ToString());

                        if (aboKlienta[wybraneID].IDPakietu > 0)
                        {
                            dołączonyPak = (PakietKlienta)Funkcje.WczytajPlik("PakietyKlienta", aboKlienta[wybraneID].IDPakietu.ToString(), daneUrzytkownika.ID);
                            dołączonyPakInfo = (PakietyInfo)Funkcje.WczytajPlik("PakietyInfo", dołączonyPak.IDOferty.ToString(), daneUrzytkownika.ID);
                        }

                    }
                    catch (Exception)
                    {
                        powtórka = true;
                    }

                }
                while (powtórka);

                string pakiet = "Nie jest częścią pakietu";
                if (dołączonyPakInfo != null)
                    pakiet = $"Pakiet: {dołączonyPakInfo.Nazwa}\n";

                string opłacone = "Opłaty zapłacone do przodu";
                if (aboKlienta[wybraneID].NaIleOpłaconoDoPrzodu < 0)
                    opłacone = "Ilość zaległych opłat";

                string przecena = $"Przecena na opłąty: {(aboKlienta[wybraneID].Przecena) * 100}%\n";
                if (aboKlienta[wybraneID].Przecena == 0)
                    przecena = "";

                Console.WriteLine($"\n\n   Nazwa: {wybranyAboInfo.Nazwa}\n" +
                      $"   Przypisany numer telefonu: {aboKlienta[wybraneID].NumerTelefonu}\n" +
                      $"   Pakiet opłacany raz na: {wybranyAboInfo.CzęstotliwośćRozliczania}\n" +
                      $"   {opłacone}: {Math.Abs(aboKlienta[wybraneID].NaIleOpłaconoDoPrzodu)}\n" +
                      $"   Cena za jedna opłatę to: {wybranyAboInfo.Cena}\n" +
                      $"   {przecena}" +
                      $"   {pakiet}\n" +
                      $"   Data dodania: {aboKlienta[wybraneID].DataDodania}\n");
                if (!admin)
                {
                    Console.WriteLine("  Wciśnij enter, by kontynuować");
                    Console.ReadLine();
                }
                else
                {
                    AbonamentKlienta zmienionyAbo = aboKlienta[wybraneID];

                    NaCzerwono("\n\n\n   Dowolnym momencie wpisz \"wyjdz\" by wyjść");

                    Console.Write("\n  Co chcesz zrobić? " +
                        "\n    1 - Opłać" +
                        "\n    2 - Nadaj zniżkę" +
                        "\n    3 - Usuń" +
                        "\n      >> ");

                    string wybór = Console.ReadLine().Trim();
                    string wprowadzono;

                    if (CzyWyjść(wybór))
                        return;

                    switch (wybór)
                    {
                        case "1":
                            {
                                Console.Write($"    Na ile opłacić (rozliczenie co {wybranyAboInfo.CzęstotliwośćRozliczania}): ");
                                wprowadzono = Console.ReadLine();
                                int ilośćI = Math.Abs(ZróbInt(wprowadzono));

                                if (CzyWyjść(wprowadzono))
                                    return;

                                zmienionyAbo.NaIleOpłaconoDoPrzodu += ilośćI;
                                Funkcje.ZapiszPlik(zmienionyAbo, zmienionyAbo.ID.ToString(), daneUrzytkownika.ID);
                                break;
                            }
                        case "2":
                            {
                                Console.Write("    Na ile ustawić zniżkę (w procentach): ");
                                wprowadzono = Console.ReadLine().Replace("%", "");
                                double ilośćD = Math.Abs(ZróbInt(wprowadzono));

                                if (CzyWyjść(wprowadzono))
                                    return;

                                zmienionyAbo.Przecena = ilośćD;
                                Funkcje.ZapiszPlik(zmienionyAbo, zmienionyAbo.ID.ToString(), daneUrzytkownika.ID);
                                break;
                            }
                        case "3":
                            {
                                Funkcje.UsuńPlik("AbonamentyKlienta", zmienionyAbo.ID.ToString(), daneUrzytkownika.ID);
                                zmienionyAbo = new AbonamentKlienta();
                                break;
                            }
                    }
                    return;
                }
            }
        }

        /// <summary>
        /// Podanie admin jako true umożliwia edycję ofert
        /// </summary>
        /// <param name="admin"></param>
        public static void InfoOOfertachUrz(DaneLogowania daneKlienta, bool edytuj = false, bool dajKlientowi = false)                                                          // 5
        {
            Console.Title = "Informacje o ofertach urządzeń";

            if (edytuj)
                Console.Title = "Wyświetlanie i modyfikacja ofert urządzeń";

            while (true)
            {
                UrządzeniaInfo[] dostępne = new UrządzeniaInfo[1];
                dostępne = Funkcje.WczytajWszystkiePliki(dostępne);

                Console.WriteLine($" Wyświetlanie dostępnych ofert Urządzeń ({dostępne.Length})");

                int i = 1;

                foreach (UrządzeniaInfo oferta in dostępne)
                {
                    Console.WriteLine($"    {i++} - {oferta.Nazwa}");
                }

                NaCzerwono("\n   Dowolnym momencie wpisz \"wyjdz\" by wyjść");
                bool powtórka = true;
                int wybraneID = 0;

                do
                {
                    powtórka = false;
                    try
                    {
                        Console.Write("\n   Podaj numer pozycji którą checsz zobaczyć: ");
                        string? wprowadzono = Console.ReadLine();
                        int pozycja = ZróbInt(wprowadzono);
                        Console.WriteLine();

                        if (CzyWyjść(wprowadzono))
                            return;

                        if (pozycja < 1 || pozycja > i)
                        {
                            Console.WriteLine("          Błędne wprowadzenie");
                            powtórka = true;
                            continue;
                        }

                        wybraneID = pozycja - 1;

                    }
                    catch (Exception)
                    {
                        powtórka = true;
                        Console.WriteLine("          Błędne wprowadzenie");
                    }

                }
                while (powtórka);

                CzyszczenieEkranu();
                Console.WriteLine($"\n Nazwa: {dostępne[wybraneID].Nazwa}");
                Console.WriteLine($"\n Wytwóra: {dostępne[wybraneID].Wytwórca}");
                Console.WriteLine($"\n Cena: {dostępne[wybraneID].Cena}");

                Console.WriteLine("\n Dostępne warianty: ");
                foreach (string wariant in dostępne[wybraneID].Warianty)
                {
                    Console.WriteLine($"    - {wariant}");
                }

                Console.WriteLine("\n Dostępne kolory: ");
                foreach (string kolor in dostępne[wybraneID].Kolory)
                {
                    Console.WriteLine($"    - {kolor}");
                }

                string wiadomość = "\n\n    Wpisz \"kup\" aby przejść do kupna";
                if (edytuj)
                    wiadomość = "\n\n    Wpisz \"edytuj\" aby edytowa";
                else if (dajKlientowi)
                    wiadomość = "\n\n    Wpisz \"dodaj\" by dodać wybraną pozyjcę klientowi";

                Console.Write(wiadomość + ", wciśnij enter by kontynuować: ");

                string? wybór = Console.ReadLine().ToLower().Replace('ó', 'u');

                if (CzyWyjść(wybór))
                    return;
                else if (!edytuj && !dajKlientowi && wybór.Contains("kup"))
                    KupUrz(daneKlienta, dostępne[wybraneID]);
                else if (!edytuj && dajKlientowi && wybór.Contains("dodaj"))
                    KupUrz(daneKlienta, dostępne[wybraneID], true);
                else if (edytuj && !dajKlientowi && wybór.Contains("edytuj"))
                {
                    Console.WriteLine("\n");
                    DodajOfertęUrz(dostępne[wybraneID].ID);
                }

                CzyszczenieEkranu();
            }

        }
        /// <summary>
        /// Podanie admin jako true umożliwia edycję ofert
        /// </summary>
        /// <param name="admin"></param>
        public static void InfoOOfertachAbo(DaneLogowania daneKlienta, bool edytuj = false, bool dajKlientowi = false)                                                          // 6
        {
            Console.Title = "Informacje o ofertach abonamentów";

            if (edytuj)
                Console.Title = "Wyświetlanie i modyfikacja ofert abonamentów";

            while (true)
            {
                CzyszczenieEkranu();

                AbonamentyInfo[] dostępne = new AbonamentyInfo[1];
                dostępne = Funkcje.WczytajWszystkiePliki(dostępne);

                Console.WriteLine($" Wyświetlanie dostępnych ofert Abonamntów ({dostępne.Length})");

                int i = 1;

                foreach (AbonamentyInfo oferta in dostępne)
                {
                    Console.WriteLine($"    {i++} - {oferta.Nazwa}");
                }

                NaCzerwono("\n   Dowolnym momencie wpisz \"wyjdz\" by wyjść");
                bool powtórka = true;
                int wybraneID = 0;

                do
                {
                    try
                    {
                        powtórka = false;
                        Console.Write("\n   Podaj numer pozycji którą checsz zobaczyć: ");
                        string? wprowadzono = Console.ReadLine();
                        int pozycja = ZróbInt(wprowadzono);
                        Console.WriteLine();

                        if (CzyWyjść(wprowadzono))
                            return;

                        if (pozycja < 1 || pozycja > dostępne.Length)
                        {
                            Console.WriteLine("          Błędne wprowadzenie");
                            powtórka = true;
                            continue;
                        }

                        wybraneID = pozycja - 1;

                    }
                    catch (Exception)
                    {
                        powtórka = true;
                        Console.WriteLine("          Błędne wprowadzenie");
                    }

                }
                while (powtórka);    //dostępne[wybraneID].

                CzyszczenieEkranu();
                Console.WriteLine($"\n Nazwa: {dostępne[wybraneID].Nazwa}");

                if (dostępne[wybraneID].LimitInternetu == 0)
                    Console.WriteLine($"\n Ten abonament nie zapewnia dostępu do internetu.");
                else if (dostępne[wybraneID].LimitInternetu == -1)
                    Console.WriteLine($"\n Ten abonament zapewnia nielimitowany dostęp do internetu.");
                else
                {
                    double prędkośćMin = dostępne[wybraneID].LimityPrędkości[1];
                    string prędkośćMinTekst = $"{prędkośćMin} Mb/s";
                    if (dostępne[wybraneID].LimityPrędkości[1] < 1)
                    {
                        prędkośćMin *= 1000;
                        prędkośćMinTekst = $"{prędkośćMin} kb/s";
                    }

                    Console.WriteLine($"\n Ten abonament zapewnia dostęp do {dostępne[wybraneID].LimitInternetu} GB internetu z prędkością do" +
                          $" {dostępne[wybraneID].LimityPrędkości[0]} Mb/s. \n      ({prędkośćMinTekst} po wyczerpaniu limitu).");

                    //Console.WriteLine($"\n Cena doładowania internetu to {dostępne[wybraneID].CenaDoładowaniaInternetu} zł za GB");
                }

                Console.WriteLine($"\n Opłaty w wysokoći {dostępne[wybraneID].Cena} zł, naliczane co {dostępne[wybraneID].CzęstotliwośćRozliczania}");

                string wiadomość = "\n\n    Wpisz \"kup\" aby przejść do kupna";

                if (edytuj)
                    wiadomość = "\n\n    Wpisz \"edytuj\" aby edytowa";
                else if (dajKlientowi)
                    wiadomość = "\n\n    Wpisz \"dodaj\" by dodać wybraną pozyjcę klientowi";

                Console.Write(wiadomość + ", wciśnij enter by kontynuować: ");

                string? wybór = Console.ReadLine().ToLower().Replace('ó', 'u');

                if (CzyWyjść(wybór))
                    return;
                else if (!edytuj && !dajKlientowi && wybór.Contains("kup"))
                    KupAbo(daneKlienta, dostępne[wybraneID]);
                else if (!edytuj && dajKlientowi && wybór.Contains("dodaj"))
                    KupAbo(daneKlienta, dostępne[wybraneID], true);
                else if (edytuj && !dajKlientowi && wybór.Contains("edytuj"))
                {
                    Console.WriteLine("\n");
                    DodajOfertęAbo(dostępne[wybraneID].ID);
                }
            }

        }
        /// <summary>
        /// Podanie admin jako true umożliwia edycję ofert
        /// </summary>
        /// <param name="admin"></param>
        public static void InfoOOfertachPak(DaneLogowania daneKlienta, bool edytuj = false, bool dajKlientowi = false)                                                          // 7
        {
            Console.Title = "Informacje o ofertach pakietów";

            if (edytuj)
                Console.Title = "Wyświetlanie i modyfikacja ofert pakietów";

            while (true)
            {
                CzyszczenieEkranu();

                PakietyInfo[] dostępne = new PakietyInfo[1];
                dostępne = Funkcje.WczytajWszystkiePliki(dostępne);

                Console.WriteLine($" Wyświetlanie dostępnych ofert Pakietów ({dostępne.Length})");

                int i = 1;

                foreach (PakietyInfo oferta in dostępne)
                {
                    Console.WriteLine($"    {i++} - {oferta.Nazwa}");
                }

                NaCzerwono("\n   W dowolnym momencie wpisz \"wyjdz\" by wyjść");
                bool powtórka = true;
                int wybraneID = 0;

                do
                {
                    try
                    {
                        powtórka = false;
                        Console.Write("\n   Podaj numer pozycji którą checsz zobaczyć: ");
                        string? wprowadzono = Console.ReadLine();
                        int pozycja = ZróbInt(wprowadzono);
                        Console.WriteLine();

                        if (CzyWyjść(wprowadzono))
                            return;

                        if (pozycja < 1 || pozycja > i)
                        {
                            Console.WriteLine("      Błędne wprowadzenie");
                            powtórka = true;
                            continue;
                        }

                        wybraneID = pozycja - 1;

                    }
                    catch (Exception)
                    {
                        powtórka = true;
                        Console.WriteLine("      Błędne wprowadzenie");
                    }

                }
                while (powtórka);

                CzyszczenieEkranu();

                UrządzeniaInfo[] urządzenia = new UrządzeniaInfo[dostępne[wybraneID].TelefonyID.Length];
                for (i = 0; i < urządzenia.Length; i++)
                {
                    urządzenia[i] = (UrządzeniaInfo)Funkcje.WczytajPlik("UrządzeniaInfo", (string)(dostępne[wybraneID].TelefonyID[i]).ToString());
                }

                AbonamentyInfo abonament = null;
                if (dostępne[wybraneID].MaAbonament)
                    abonament = (AbonamentyInfo)Funkcje.WczytajPlik("AbonamentyInfo", (string)(dostępne[wybraneID].AbonamentID).ToString());

                Console.WriteLine($"\n Nazwa: {dostępne[wybraneID].Nazwa}");
                /*                           dokończyć, 
                Console.WriteLine($"\n Wytwóra: {dostępne[wybraneID].}");
                Console.WriteLine($"\n Wytwóra: {dostępne[wybraneID].}");
                Console.WriteLine($"\n Wytwóra: {dostępne[wybraneID].}");
                */
                Console.WriteLine($"\n Cena: {dostępne[wybraneID].Cena}");

                Console.WriteLine($"\n Zawarty abonament: {abonament.Nazwa}");

                if (dostępne[wybraneID].MaAbonament)
                    Console.WriteLine($"\n Przecena na opłatę abonametu: {dostępne[wybraneID].PrzecenaAbonament*100}%");

                Console.WriteLine("\n Zawarte telefony: ");
                for (i = 0; i < urządzenia.Length; i++)
                {
                    Console.WriteLine($"    - {urządzenia[i].Nazwa} {dostępne[wybraneID].WariantyTelefonów[i]}");
                }


                string wiadomość = "\n\n   Wpisz \"kup\" aby przejść do kupna";
                if (edytuj)
                    wiadomość = "\n\n   Wpisz \"edytuj\" aby edytowa";
                else if (dajKlientowi)
                    wiadomość = "\n\n   Wpisz \"dodaj\" by dodać wybraną pozyjcę klientowi";

                Console.Write(wiadomość + ", wciśnij enter by kontynuować: ");

                string? wybór = Console.ReadLine().ToLower().Replace('ó', 'u');

                if (CzyWyjść(wybór))
                    return;
                else if (!edytuj && !dajKlientowi && wybór.Contains("kup"))
                    KupPak(daneKlienta, dostępne[wybraneID], abonament, urządzenia);
                else if (!edytuj && dajKlientowi && wybór.Contains("dodaj"))
                    KupPak(daneKlienta, dostępne[wybraneID], abonament, urządzenia, true);
                else if (edytuj && !dajKlientowi && wybór.Contains("edytuj"))
                {
                    Console.WriteLine("\n");
                    DodajOfertęPak(dostępne[wybraneID].ID);
                }

                CzyszczenieEkranu();
            }

        }
        #endregion

        #region Funkcje Admina
        public static void InfoOKlientach()                                                             // 1
        {
            Console.Title = "Informacje o klientach";

            Console.WriteLine(" Wyświetlanie informacji o klientach");

            bool poprawnie = false;
            DaneLogowania[] listaTemp = new DaneLogowania[1];         // pełna list danych klientów (nie wszystkich urzytkowników, bo admini nie mają urządzeń, abonamentów ani pakietów)
            DaneLogowania[] daneKlientów = new DaneLogowania[Funkcje.LiczbaKlientów()];
            int i = 0;

            foreach(DaneLogowania urzytkownik in Funkcje.WczytajWszystkiePliki(listaTemp))
            {
                if (urzytkownik.Admin == false)
                    daneKlientów[i++] = urzytkownik;
            }

            if (daneKlientów.Length == 0)
            {
                Console.WriteLine("\n   Nie istnieją żadni Klienci");
                Thread.Sleep(2000);
                return;
            }

            DaneLogowania wybranyKlient = daneKlientów[0];

            Console.WriteLine("\n   Lista klientów: ");
            for (i = 0; i < daneKlientów.Length; i++)
            {
                Console.WriteLine($"      {i + 1} - {daneKlientów[i].Imię} {daneKlientów[i].Nazwisko}");
            }

            while (true)
            {
                Console.Write("\n\n   Wybierz numer pozycji klienta do wyświetlenia: ");
                int pozycjaKlienta = ZróbInt(Console.ReadLine());
                poprawnie = false;

                if (pozycjaKlienta > 0 && pozycjaKlienta <= daneKlientów.Length)
                {
                    wybranyKlient = daneKlientów[pozycjaKlienta - 1]; 
                    break;
                }

                Console.WriteLine("      Niepoprawne wprowadzenie");
            }

            while (true)
            {

                Console.Write("\n\n   Co chcesz zobaczyć?" +
                      "\n      1 - Dane osobiste klienta" +
                      $"\n      2 - Urządzenia ({Funkcje.ListaID("UrządzeniaKlienta", wybranyKlient.ID).Length}) klienta" +
                      $"\n      3 - Abonamenty ({Funkcje.ListaID("AbonamentyKlienta", wybranyKlient.ID).Length}) klienta" +
                      "\n         >> ");

                string wprowadzono = Console.ReadLine().ToLower().Replace(",", "").Trim();

                Console.WriteLine("\n\n");

                switch (wprowadzono)
                {
                    case "1":
                        TwojeInformacje(ref wybranyKlient, true);
                        break;

                    case "2":
                        InfoOTwoichUrz(wybranyKlient, true);
                        break;

                    case "3":
                        InfoOTwoichAbo(wybranyKlient, true);
                        break;
                }
            }

            /*
             * Dodać:
             * 1) Znajdujące się wewnątrz pentli (z której wychodzi się wpisyjąć "wyjdź" (lub wyjdz)) menu wypisujące listę urzytkowników: numer opcji) imię i nazwisko, id
             * 2) Po wybraniu klienta przechodzi do kolejnej pentli (z której wychodzi się wpisyjąć "wyjdź") i wypisuje listę w formacie:
             * 
             *   Imię i Nazwisko, ID
             *      Dane Osobiste
             *         ...(lista danych)
             *      
             *      Urządzenia
             *         ...(ponumerowana lista urządzeń (licznik od 1))
             *      
             *      Abonamenty
             *         ...(ponumerowana lista abonametów (licznik kontynuowany od ostatniej liczby w urządzeniach))
             *      
             *      Pakiety
             *         ...(ponumerowana lista pakietów (licznik kontynuowany od ostatniej liczby w abonamentach))
             *         
             * 3) Po wybraniu numeru należy wyświetlić listę wartości danej pozycji. Zmienianie jak w zmienianiu właściwości urzytkownika (z menu urzytkownika)
             */

        }
        public static void ModyfikacjaOfertUrz()                                                      // 2
        {
            Console.WriteLine(" Modyfikujacja dostepnych ofert Urządzeń");

            InfoOOfertachUrz(null, true);
        }
        public static void ModyfikacjaOfertAbo()                                                      // 3
        {
            Console.WriteLine(" Modyfikujacja dostepnych ofert Abonamentów");

            InfoOOfertachAbo(null, true);
        }
        public static void ModyfikacjaOfertPak()                                                      // 4
        {
            Console.WriteLine(" Modyfikujacja dostepnych ofert Pakietów");

            InfoOOfertachPak(null, true);
        }
        /// <summary>
        /// Zostawić ID   jako -1, by utworzyć nową ofertę.
        /// </summary>
        /// <param name="ID"></param>
        public static void DodajOfertęUrz(int ID = -1)                                                             // 5
        {
            if (ID != -1)
            {
                Console.Title = "Modyfikowanie oferty urządzenia";
                Console.WriteLine(" Modyfikowanie oferty Urządzenia");
            }
            else
            {
                Console.Title = "Dodawanie nowej oferty Urządzenia";
                Console.WriteLine(" Dodawanie nowej oferty Urządzenia");
            }

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
            Console.WriteLine("\n Tworzenie nowego urzadzenia: ");

            do
            {
                powtórka = false;

                Console.Write("\n   Podaj nazwe: ");
                noweUrz.Nazwa = Console.ReadLine();

                if (CzyWyjść(noweUrz.Nazwa))
                {
                    return;
                }

                foreach (string nazwa in listaNazw)
                {
                    if (nazwa == noweUrz.Nazwa)
                    {
                        powtórka = true;
                        Console.Write("\n      Nazwa juz istnieje");

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
                    Console.Write("\n   Podaj cene: ");
                    string wprowadzono = Console.ReadLine();
                    double ilośćD = Funkcje.Zaokrągl(ZróbDouble(wprowadzono));

                    if (CzyWyjść(wprowadzono))
                        return;

                    if (ilośćD < 0)
                    {
                        powtórka = true;
                        continue;
                    }

                    noweUrz.Cena = ilośćD;
                }
                catch (Exception)
                {
                    powtórka = true;
                }

            }
            while (powtórka);

            Console.Write("\n   Podaj wytwórce: ");
            noweUrz.Wytwórca = Console.ReadLine();

            if (CzyWyjść(noweUrz.Wytwórca))
            {
                return;
            }

            Console.Write("\n   Podaj liczbę wariantów: ");
            int ilośćWariantów = ZróbInt(Console.ReadLine());
            noweUrz.Warianty = new string[ilośćWariantów];

            for (int j = 0; j < ilośćWariantów; j++)
            {
                Console.Write($"      Podaj {j + 1} wariant: ");
                noweUrz.Warianty[j] = Console.ReadLine();

                if (CzyWyjść(noweUrz.Warianty[j]))
                    return;
            }

            Console.Write("\n   Podaj liczbę kolorów: ");
            int ilośćKolorów = ZróbInt(Console.ReadLine());
            noweUrz.Kolory = new string[ilośćKolorów];

            for (int j = 0; j < ilośćKolorów; j++)
            {
                Console.Write($"      Podaj {j + 1} kolor: ");
                noweUrz.Kolory[j] = Console.ReadLine();

                if (CzyWyjść(noweUrz.Kolory[j]))
                    return;
            }

            if (ID < 0)
                ID = noweUrz.ID;

            noweUrz.ID = ID;

            Funkcje.ZapiszPlik(noweUrz, ID.ToString());

            Console.WriteLine("   Zakończono dodawanie");
            Thread.Sleep(1000);
        }
        /// <summary>
        /// Zostawić ID   jako -1, by utworzyć nową ofertę
        /// </summary>
        /// <param name="ID"></param>
        public static void DodajOfertęAbo(int ID = -1)                                                             // 6
        {
            if (ID != -1)
            {
                Console.Title = "Modyfikowanie oferty abonamentu";
                Console.WriteLine(" Modyfikowanie oferty Abonamentu");
            }
            else
            {
                Console.Title = "Dodawanie nowej oferty abonamentu";
                Console.WriteLine(" Dodawanie nowej oferty Abonamentu");
            }

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
            Console.WriteLine("\n Tworzenie nowego abonamentu: ");

            do
            {
                powtórka = false;

                Console.Write("\n   Podaj nazwe: ");
                nowyAbo.Nazwa = Console.ReadLine();

                if (CzyWyjść(nowyAbo.Nazwa))
                    return;

                foreach (string nazwa in listaNazw)
                {
                    if (nazwa == nowyAbo.Nazwa)
                    {
                        powtórka = true;
                        Console.Write("\n      Nazwa zajęta");
                        break;
                    }
                }
            }
            while (powtórka);

            Console.Write("\n   Podaj częstotliwośc rozliczenia ( tydzień / miesiąc / rok): ");
            nowyAbo.CzęstotliwośćRozliczania = Console.ReadLine();

            if (CzyWyjść(nowyAbo.CzęstotliwośćRozliczania))
                return;

            do
            {
                try
                {
                    powtórka = false;
                    Console.Write("\n   Podaj cene jendej opłaty: ");
                    string wprowadzono = Console.ReadLine();
                    double ilośćD = Funkcje.Zaokrągl(ZróbDouble(wprowadzono));

                    if (CzyWyjść(wprowadzono))
                        return;

                    if (ilośćD < 0)
                    {
                        powtórka = true;
                        continue;
                    }

                    nowyAbo.Cena = ilośćD;
                }
                catch (Exception)
                {
                    powtórka = true;
                }

            }
            while (powtórka);

            try
            {
                Console.Write("\n   Podaj limit internetu (w GB), -1 dla nielimitowanego, 0 dla braku internetu: ");
                string wprowadzono = Console.ReadLine();
                double ilośćD = Funkcje.Zaokrągl(ZróbDouble(wprowadzono));

                if (CzyWyjść(wprowadzono))
                    return;

                nowyAbo.LimitInternetu = ilośćD;
            }
            catch (Exception) { }

            if (nowyAbo.LimitInternetu > 0)
            {
                powtórka = true;
                do
                {
                    try
                    {
                        Console.Write("\n   Podaj limit szybkości internetu przed wyczerpaniem limitu (w Mb/s): ");
                        string wprowadzono = Console.ReadLine();
                        double ilośćD = Funkcje.Zaokrągl(ZróbDouble(wprowadzono));

                        if (CzyWyjść(wprowadzono))
                            return;
                        if (ilośćD < 0)
                        {
                            continue;
                        }

                        nowyAbo.LimityPrędkości[0] = ilośćD;
                        powtórka = false;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("      Błąd wprowadzania wartości PRZED WYCZERPANIEM LIMITU");
                    }
                }
                while (powtórka);

                powtórka = true;
                do
                {
                    try
                    {
                        Console.Write("\n   Podaj limit szybkości internetu po wyczerpanieu limitu (w Mb/s): ");
                        string wprowadzono = Console.ReadLine();
                        double ilośćD = Funkcje.Zaokrągl(ZróbDouble(wprowadzono));

                        if (CzyWyjść(wprowadzono))
                            return;

                        if (ilośćD < 0)
                        {
                            continue;
                        }

                        powtórka = false;
                        nowyAbo.LimityPrędkości[1] = ilośćD;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("         Błąd wprowadzania wartości PO WYCZERPANIU LIMITU");
                    }
                }
                while (powtórka);
            }

            if (ID == -1)
            {
                ID = nowyAbo.ID;
            }
            Funkcje.ZapiszPlik(nowyAbo, ID.ToString());

            Console.WriteLine("    Zakończono dodawanie");
            Thread.Sleep(1000);
        }
        /// <summary>
        /// Zostawić ID   jako -1, by utworzyć nową ofertę
        /// </summary>
        /// <param name="ID"></param>
        public static void DodajOfertęPak(int ID = -1)                                                             // 7
        {
            if (ID != -1)
            {
                Console.Title = "Modyfikowanie oferty pakietu";
                Console.WriteLine(" Modyfikowanie oferty Pakietu");
            }
            else
            {
                Console.Title = "Dodawanie nowej oferty pakietu";
                Console.WriteLine(" Dodawanie nowej oferty Pakietu");
            }

            PakietyInfo nowyPak = new();
            PakietyInfo[] listaPak = new PakietyInfo[1];
            listaPak = Funkcje.WczytajWszystkiePliki(listaPak);

            UrządzeniaInfo[] listaUrz = new UrządzeniaInfo[1];
            listaUrz = Funkcje.WczytajWszystkiePliki(listaUrz);

            AbonamentyInfo[] listaAbo = new AbonamentyInfo[1];
            listaAbo = Funkcje.WczytajWszystkiePliki(listaAbo);

            int i = 0;
            bool powtórka;
            double cenaNormalna = 0;
            double cenaOpłaty = 0;
            string częstośćRozliczania = "miesiac";

            NaCzerwono("\n\n W dowolnym momencie wpisz \"wyjdz\" aby wyjść do menu");
            Console.WriteLine("\n Tworzenie nowego pakietu: ");

            do
            {
                powtórka = false;

                Console.Write("\n   Podaj nazwe: ");
                nowyPak.Nazwa = Console.ReadLine();

                if (CzyWyjść(nowyPak.Nazwa))
                {
                    return;
                }

                foreach (PakietyInfo pakiet in listaPak)
                {
                    if (pakiet.Nazwa == nowyPak.Nazwa)
                    {
                        powtórka = true;
                        Console.Write("\n   Nazwa zajęta");

                        break;
                    }
                }
            }
            while (powtórka);

            Console.Write("\n   Podaj liczbę dołączonych do pakietu urządzeń: ");
            int ilośćTelefonów = ZróbInt(Console.ReadLine());
            if (ilośćTelefonów > listaUrz.Length)
            {
                Console.WriteLine($"      Nie na tylu urządzeń, zaniżono liczbę do {listaUrz.Length}.");
                ilośćTelefonów = listaUrz.Length;
            }
            nowyPak.TelefonyID = new int[ilośćTelefonów];
            nowyPak.WariantyTelefonów = new string[ilośćTelefonów];
            bool znaleziono = false;

            if (ilośćTelefonów > 0)
            {
                Console.WriteLine("\n   Lista dostępnych urządzeń:");
                foreach (UrządzeniaInfo urządzenie in listaUrz)
                {
                    Console.WriteLine("      - {0,-15},   (id - {1})", urządzenie.Nazwa, urządzenie.ID);
                }
            }

            for (int j = 0; j < ilośćTelefonów; j++)
            {
                do
                {
                    Console.Write($"\n      Podaj nazwę lub ID dla {j + 1} modelu:");
                    string? podanyModel = Console.ReadLine();
                    znaleziono = false;

                    try
                    {
                        nowyPak.TelefonyID[j] = int.Parse(podanyModel.Replace(" ", ""));                   // Sprawdza czy int. Jeśli nie, patrzy w catchu czy istnieje taki model
                                                                                                           //Console.WriteLine("Jest ID");

                        foreach (UrządzeniaInfo urządzenie in listaUrz)
                        {
                            if (nowyPak.TelefonyID[j] == urządzenie.ID)
                            {
                                znaleziono = true;
                                cenaNormalna += urządzenie.Cena;
                                break;
                            }
                        }

                        if (!znaleziono)
                        {
                            throw new Exception();
                        }
                    }
                    catch (Exception)
                    {
                        //Console.WriteLine("Szukanie po nazwie");

                        foreach (UrządzeniaInfo urządzenie in listaUrz)
                        {
                            znaleziono = false;

                            if (podanyModel.Trim() == urządzenie.Nazwa.Trim())
                            {
                                znaleziono = true;
                                cenaNormalna += urządzenie.Cena;
                                nowyPak.TelefonyID[j] = urządzenie.ID;
                                break;
                            }
                        }
                    }
                    if (!znaleziono)
                        Console.WriteLine("         Nie istnieje takie urządzenie");
                    else // daj model
                    {
                        UrządzeniaInfo urządzenie = (UrządzeniaInfo)Funkcje.WczytajPlik("UrządzeniaInfo", nowyPak.TelefonyID[j].ToString());
                        Console.WriteLine("         Wybierz model urządzenia: ");
                        foreach (string warinat in urządzenie.Warianty)
                        {
                            Console.WriteLine("            - " + warinat);
                        }
                        Console.Write("               >> ");
                        string wybranyWar = Console.ReadLine();

                        if (CzyWyjść(wybranyWar))
                            return;

                        nowyPak.WariantyTelefonów[j] = wybranyWar;
                    }
                }
                while (!znaleziono);
            }

            Console.WriteLine("\n   Lista dostępnych abonamentów:");
            foreach (AbonamentyInfo abonament in listaAbo)
            {
                Console.WriteLine("      - {0,15}    (id - {1})", abonament.Nazwa, abonament.ID);
            }

            do
            {
                Console.Write("\n   Podaj ID lub nazwę zawartego abonamentu (\"-1\" by nie dodawać): ");
                string podane = Console.ReadLine();
                int ilośćI = ZróbInt(podane);

                if (CzyWyjść(podane))
                    return;

                if (ilośćI == -1 || listaAbo.Length < 1)
                {
                    nowyPak.MaAbonament = false;
                    if (listaAbo.Length < 1)
                        Console.WriteLine("      Nie ma żadnych abonamentów.");
                    else
                        Console.WriteLine("      Zrezygnowano z dodawania abonamentu.");

                    Thread.Sleep(1000);
                    break;
                }

                znaleziono = false;

                try
                {
                    nowyPak.AbonamentID = int.Parse(podane.Replace(" ", ""));                   // Sprawdza czy int. Jeśli nie, patrzy w catchu czy istnieje taki model
                                                                                                //Console.WriteLine("Jest ID");

                    foreach (AbonamentyInfo abonament in listaAbo)
                    {
                        if (ZróbInt(podane) == abonament.ID)
                        {
                            znaleziono = true;
                            nowyPak.AbonamentID = abonament.ID;
                            cenaOpłaty = abonament.Cena;
                            częstośćRozliczania = abonament.CzęstotliwośćRozliczania;
                            break;
                        }
                    }

                    if (!znaleziono)
                        throw new Exception();
                }
                catch (Exception)
                {
                    //Console.WriteLine("Szukanie po nazwie");

                    foreach (AbonamentyInfo abonament in listaAbo)
                    {
                        znaleziono = false;

                        if (podane.Trim() == abonament.Nazwa.Trim())
                        {
                            znaleziono = true;
                            nowyPak.AbonamentID = abonament.ID;
                            cenaOpłaty = abonament.Cena;
                            częstośćRozliczania = abonament.CzęstotliwośćRozliczania;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("      Nie istnieje taki abonament");
                        }
                    }
                }

                if (znaleziono)
                    nowyPak.MaAbonament = true;
            }
            while (!znaleziono);


            if (nowyPak.AbonamentID >= 0)
            {
                do
                {
                    try
                    {
                        powtórka = false;
                        Console.Write("\n   Podaj przecenę na okresową opłatę abonamentu (w procentach): ");
                        string wprowadzono = Console.ReadLine().Replace("%", "");
                        double ilośćD = Funkcje.Zaokrągl(ZróbDouble(wprowadzono)/100, 2);

                        if (CzyWyjść(wprowadzono))
                            return;

                        if (ilośćD < 0)
                        {
                            powtórka = true;
                            continue;
                        }

                        nowyPak.PrzecenaAbonament = (ilośćD);
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
                        Console.Write($"\n   Na ile do przodu opłacić abonament (rozlicza co {częstośćRozliczania}): ");
                        string wprowadzono = Console.ReadLine().Replace(" ", "").Replace("-", "");
                        int ilośćI = ZróbInt(wprowadzono);

                        if (CzyWyjść(wprowadzono))
                            return;

                        if (ilośćI < 0)
                            ilośćI = -ilośćI;

                        nowyPak.CzasTrwania = ilośćI;
                        cenaNormalna += cenaOpłaty * ilośćI;
                    }
                    catch (Exception)
                    {
                        powtórka = true;
                    }
                }
                while (powtórka);
            }

            do
            {
                try
                {
                    powtórka = false;
                    Console.Write($"\n   Podaj cenę pakietu (cena normalna urządzeń i abonamentu to {cenaNormalna} zł): ");
                    string wprowadzono = Console.ReadLine().Replace("%", "");
                    double ilośćD = Funkcje.Zaokrągl(ZróbDouble(wprowadzono));

                    if (CzyWyjść(wprowadzono))
                        return;

                    if (ilośćD < 0)
                    {
                        powtórka = true;
                        continue;
                    }

                    nowyPak.Cena = ilośćD;
                }
                catch (Exception)
                {
                    powtórka = true;
                }

            }
            while (powtórka);

            if (ID == -1)
                ID = nowyPak.ID;

            Funkcje.ZapiszPlik(nowyPak, ID.ToString());

            Console.WriteLine("   Zakończono dodawanie");
            Thread.Sleep(1000);
        }
        #endregion

        #region Dodawanie Klientowi Oferty
        public static void KupUrz(DaneLogowania daneKlienta, UrządzeniaInfo urządzenie, bool admin = false, int IDPak = -1, string wariant = "")
        {
            if (IDPak == -1)
                Console.Title = "Kupowanie urządzenia";

            CzyszczenieEkranu();

            string kolor = "";
            UrządzenieKlienta dodaneUrz = new UrządzenieKlienta(daneKlienta.ID, true, urządzenie.ID);

            if (IDPak < 1)
            {
                Console.WriteLine(" Kupowanie urządzenia " + urządzenie.Nazwa);

                Console.WriteLine("\n   Dostępne warianty: ");
                for (int i = 0; i < urządzenie.Warianty.Length; i++)
                {
                    Console.WriteLine($"      {i + 1} - {urządzenie.Warianty[i]}");
                }

                Console.Write("\n   Wybierz wariant: ");
                try
                {
                    wariant = urządzenie.Warianty[ZróbInt(Console.ReadLine()) - 1];
                }
                catch (Exception e)
                {
                    wariant = urządzenie.Warianty[0];
                }
            }

            Console.WriteLine("\n   Dostępne kolory: ");
            for (int i = 0; i < urządzenie.Kolory.Length; i++)
            {
                Console.WriteLine($"      {i + 1} - {urządzenie.Kolory[i]}");
            }

            Console.Write($"\n   Wybierz kolor urządzenia {urządzenie.Nazwa}: ");
            try
            {
                kolor = urządzenie.Kolory[ZróbInt(Console.ReadLine()) - 1];
            }
            catch (Exception e)
            {
                kolor = urządzenie.Kolory[0];
            }

            dodaneUrz.Kolor = kolor;
            dodaneUrz.Wariant = wariant;
            dodaneUrz.IDPakietu = IDPak;
            dodaneUrz.DataDodania = DateOnly.FromDateTime(DateTime.Now).ToString();

            Funkcje.ZapiszPlik(dodaneUrz, dodaneUrz.ID.ToString(), daneKlienta.ID);

            if (IDPak == -1)
            {
                Console.WriteLine("\n\n         Zakup Udany!");
                Thread.Sleep(2000);
                CzyszczenieEkranu();
            }
        }
        public static int KupAbo(DaneLogowania daneKlienta, AbonamentyInfo oferta, bool admin = false, int IDPak = -1, int opłacono = 0, double przecena = 0)
        {
            if (IDPak == -1)
                Console.Title = "Kupowanie urządzenia";

            CzyszczenieEkranu();

            AbonamentKlienta dodanyAbo = new AbonamentKlienta(daneKlienta.ID, true, oferta.ID);

            dodanyAbo.IDPakietu = IDPak;
            dodanyAbo.IDOferty = oferta.ID;
            dodanyAbo.DataDodania = DateOnly.FromDateTime(DateTime.Now).ToString();
            dodanyAbo.Przecena = przecena;

            if (IDPak == -1)
            {
                Console.Write("   Ile do przodu chcesz opłacić abonament?: ");
                opłacono = ZróbInt(Console.ReadLine());
                Console.WriteLine();
            }

            Console.Write("   Podaj numer telefonu który przypisać do abonamentu: ");
            char[] podane = ZróbInt(Console.ReadLine()).ToString().ToCharArray();
            for (int i = 0; i < 9 && i < podane.Length; i++)
            {
                dodanyAbo.NumerTelefonu[8 - i] = Convert.ToInt32(podane[podane.Length - 1 - i]);
            }
            Console.WriteLine();

            dodanyAbo.NaIleOpłaconoDoPrzodu = opłacono + 1;

            dodanyAbo.OstatniaOpłata = DateOnly.FromDateTime(DateTime.Now).ToString();

            Funkcje.ZapiszPlik(dodanyAbo, dodanyAbo.ID.ToString(), daneKlienta.ID);

            if (IDPak == -1)
            {
                Console.WriteLine("\n\n         Zakup Udany!");
                Thread.Sleep(2000);
                CzyszczenieEkranu();
            }

            return dodanyAbo.ID;
        }
        public static void KupPak(DaneLogowania daneKlienta, PakietyInfo oferta, AbonamentyInfo? załączonyAbo = null, UrządzeniaInfo[]? dodaneUrz = null, bool admin = false)
        {
            Console.Title = "Kupowanie Pakietu";

            CzyszczenieEkranu();

            PakietKlienta dodanyPak = new PakietKlienta(daneKlienta.ID, true, oferta.ID);

            dodanyPak.DataDodania = DateOnly.FromDateTime(DateTime.Now).ToString();

            int IDAbo = KupAbo(daneKlienta, załączonyAbo, admin, dodanyPak.ID, oferta.CzasTrwania, oferta.PrzecenaAbonament);

            for (int i = 0; i < oferta.TelefonyID.Length; i++)
            {
                KupUrz(daneKlienta, (UrządzeniaInfo)Funkcje.WczytajPlik("UrządzeniaInfo", oferta.TelefonyID[i].ToString()), admin, dodanyPak.ID, oferta.WariantyTelefonów[i]);
            }

            Funkcje.ZapiszPlik(dodanyPak, dodanyPak.ID.ToString(), daneKlienta.ID);

            Console.WriteLine("\n\n         Zakup Udany!");
            Thread.Sleep(2000);
            CzyszczenieEkranu();
        }
        #endregion

        static void Main(string[] args)
        {
            CzyszczenieEkranu();
            CultureInfo.CurrentCulture = new CultureInfo("pl-PL");
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Funkcje.CzyIstniejąWszystkieFoldery();
            Console.Title = "Telefonia komorkowa";

            //Thread.Sleep(3000);

            bool zostań = true;

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
                        Console.WriteLine("\n\n Dostępne opcje: " +
                              "\n\n      1 - Wyświetl lub modyfikuj informacje o sobie " +

                              "\n\n      2 - Wyświetl informacje o swoich urządzeniach" +
                              "\n      3 - Wyświetl informacje o swoich abonamentach" +
                              //"\n      4 - Wyświetl informacje o swoich pakietach " +

                              "\n\n      4 - Pokaż dostepne oferty Urządzeń" +
                              "\n      5 - Pokaż dostepne oferty Abonamentów" +
                              "\n      6 - Pokaż dostepne oferty Pakietów " +

                              "\n\n    Wyloguj - Wyloguj się ze swojego konta " +
                              "\n    Wyjdz - Wyjdź z programu");
                        Console.Write("\n\n   ");

                        string? opcja = Console.ReadLine();
                        if (opcja == null)
                        {
                            opcja = "";
                        }

                        opcja = opcja.Replace(" ", "");
                        opcja = opcja.ToLower();
                        opcja = opcja.Replace('ź', 'z');

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
                            case ("-9999"):
                                {
                                    CzyszczenieEkranu();
                                    //InfoOTwoichPak(zalogowanyUrzytkownik);
                                    break;
                                }
                            case ("4"):
                                {
                                    CzyszczenieEkranu();
                                    InfoOOfertachUrz(zalogowanyUrzytkownik);
                                    break;
                                }
                            case ("5"):
                                {
                                    CzyszczenieEkranu();
                                    InfoOOfertachAbo(zalogowanyUrzytkownik);
                                    break;
                                }
                            case ("6"):
                                {
                                    CzyszczenieEkranu();
                                    InfoOOfertachPak(zalogowanyUrzytkownik);
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
                        Console.WriteLine("\n\n\n Dostępne opcje: " +
                              "\n      1 - Wyświetl informacje o klientach \n" +

                              "\n      2 - Pokaż i modyfikuj dostepne oferty Urządzeń" +
                              "\n      3 - Pokaż i modyfikuj dostepne oferty Abonamentów" +
                              "\n      4 - Pokaż i modyfikuj dostepne oferty Pakietów \n" +

                              "\n      5 - Dodaj nową ofertę Urządzenia" +
                              "\n      6 - Dodaj nową ofertę Abonamentu" +
                              "\n      7 - Dodaj nową ofertę Pakietu \n" +

                              "\n    Wyloguj - Wyloguj się ze swojego konta " +
                              "\n    Wyjdź - Wyjdź z programu");
                        Console.Write("\n\n         >> ");

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
                                    Console.WriteLine("            Błędna komenda");
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