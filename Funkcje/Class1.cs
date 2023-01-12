using System.Xml.Serialization;

namespace Funkcje
#pragma warning disable CS8618
#pragma warning disable CS8601
#pragma warning disable CS8600
{
    #region Struktury danych
    [Serializable]
    public class UrządzeniaInfo
    {
        public UrządzeniaInfo()
        {
            this.IDvalue = Funkcje.NajwiększeIDOferty("Urządzenia") + 1;
        }
        public UrządzeniaInfo(int tempID)
        {
            this.IDvalue = tempID;
        }

        private int IDvalue = 1;

        public int ID
        {
            get { return IDvalue; }
            set { IDvalue = value; }
        }

        public double Cena;
        public string Nazwa;
        public string Wytwórca;
        public string[]? Kolory;                                        // niewymagane
        public string[]? Warianty;                                      // niewymagane
    }
    [Serializable]
    public class AbonamentyInfo
    {
        public AbonamentyInfo()
        {
            this.IDvalue = Funkcje.NajwiększeIDOferty("Abonamenty") + 1;
        }
        public AbonamentyInfo(int tempID)
        {
            this.IDvalue = tempID;
        }

        private int IDvalue = 1;

        public int ID
        {
            get { return IDvalue; }
            set { IDvalue = value; }
        }

        public string Nazwa;
        public string CzęstotliwośćRozliczania;                         // zakładam możliwości: tydzień, miesiąc, rok
        public double Cena;
        public double LimitInternetu = 0;                               // 0 dla braku, -1 dla nielimitowanego, >0 dla normalneog limitu , liczone w GB
        //public double CenaDoładowaniaInternetu = 0.5;                  // ile kosztuje doładowanie internetu (zł za 1 GB)
        public double[] LimityPrędkości = { 0.0, 0.0 };                     // prędkość przed i po wyczerpaniu limitu, liczone w mb/s
    }
    [Serializable]
    public class PakietyInfo
    {
        public PakietyInfo()
        {
            this.IDvalue = Funkcje.NajwiększeIDOferty("Pakiety") + 1;
        }
        public PakietyInfo(int tempID)
        {
            this.IDvalue = tempID;
        }

        private int IDvalue = 1;

        public int ID
        {
            get { return IDvalue; }
            set { IDvalue = value; }
        }

        public double Cena;
        public string Nazwa;
        public bool MaAbonament = false;
        public int[]? TelefonyID = null;                                        // ID brane z oferty urządzeń
        public string[] WariantyTelefonów = { };                        // nie podawać nic dla normalnej wersjii telefonu             
        public int AbonamentID = -1;                                     // ID oferyty przypisanego przy zakupie do telefonów abonamentu
        public int CzasTrwania = 0;                                  // Na ile opłaca abonament, wyrażane w ilości "cykli" abonamentu (np. tygodni jeśli opłacany tygodniowo)
        public double PrzecenaAbonament = 0;                            // przecena na abonament po upływie opłacenia, w ułamku diesiętnym, 0 dla braku
    }
    [Serializable]
    public class DaneLogowania
    {
        public DaneLogowania()
        {
            this.IDvalue = Funkcje.NajwiększeIDUrzytkowników() + 1;
        }
        public DaneLogowania(int tempID)
        {
            this.IDvalue = tempID;
        }

        private int IDvalue = 1;

        public int ID
        {
            get { return IDvalue; }
            set { IDvalue = value; }
        }

        public string Login { get; set; }
        public string Hasło { get; set; }
        public string Imię;
        public string Nazwisko;
        public string Email;
        public string DataUrodzenia;
        public bool Admin = false;
    }
    [Serializable]
    public class UrządzenieKlienta
    {
        public UrządzenieKlienta() { }
        public UrządzenieKlienta(int IDKlientaLubPrzedmiotu, bool podanoIDKlienta, int IDOferty)
        {
            if (podanoIDKlienta)
            {
                IDKlientaLubPrzedmiotu = Funkcje.NajwiększeIDPrzedmiotuKlienta("Urządzenia", IDKlientaLubPrzedmiotu) + 1;
            }
            this.IDvalue = IDKlientaLubPrzedmiotu;
            this.IDOferty = IDOferty;
        }

        private int IDvalue = 1;

        public int ID
        {
            get { return IDvalue; }
            set { IDvalue = value; }
        }

        public int IDOferty;                                            // ID oferty(info) z której pochodzi ten telefon
        public string Kolor = "czarny";                                 // przykładowo
        public string Wariant = "normalny";                             // przykładowo
        public int IDPakietu = -1;                                       // jaki ma przypisany pakiet (jesli ma)
        public string DataDodania;                                    // data dodania
    }
    [Serializable]
    public class AbonamentKlienta
    {
        public AbonamentKlienta() { }
        public AbonamentKlienta(int IDKlientaLubPrzedmiotu, bool podanoIDKlienta, int IDOferty)
        {
            if (podanoIDKlienta)
            {
                IDKlientaLubPrzedmiotu = Funkcje.NajwiększeIDPrzedmiotuKlienta("Abonamenty", IDKlientaLubPrzedmiotu) + 1;
            }
            this.IDvalue = IDKlientaLubPrzedmiotu;
            this.IDOferty = IDOferty;
            this.NumerTelefonu[0] = 1;
            for (int i = 8; i < this.NumerTelefonu.Length - 1; i++)
            {
                this.NumerTelefonu[i + 1] = 0;
            }
        }

        private int IDvalue = 1;

        public int ID
        {
            get { return IDvalue; }
            set { IDvalue = value; }
        }

        public int IDOferty;                                            // ID oferty(info) z której pochodzi ten abonament
        public int[] NumerTelefonu = { 0, 0, 0, 0, 0, 0, 0, 0, 0};
        public int NaIleOpłaconoDoPrzodu = 0;                           // czy i na ile opłacone do przodu - ( <0 = zaległość z zapłatą, 0 = za bierząco z opłatą, >0 = opłacone do przodu, np. przez pakiet)
        public string DataNastępnejOpłaty;                            // jeśli opłacono wynosi 0 lub <0, to normalna data zwględem zakupu; jeśli >0, to o ileś okresów płacenia do przodu
        public int IDPakietu = -1;                                       // jaki ma przypisany pakiet (jesli ma)
        public string DataDodania;                                    // data dodania
        public string OstatniaOpłata;                                    // ostatnia opłąta abonamentu
        public double Przecena = 0;
    }
    [Serializable]
    public class PakietKlienta
    {
        public PakietKlienta() { }
        public PakietKlienta(int IDKlientaLubPrzedmiotu, bool podanoIDKlienta, int IDOferty)
        {
            if (podanoIDKlienta)
            {
                IDKlientaLubPrzedmiotu = Funkcje.NajwiększeIDPrzedmiotuKlienta("Pakiety", IDKlientaLubPrzedmiotu) + 1;
            }
            this.IDvalue = IDKlientaLubPrzedmiotu;
            this.IDOferty = IDOferty;
        }

        private int IDvalue = 1;

        public int ID
        {
            get { return IDvalue; }
            set { IDvalue = value; }
        }

        public int IDOferty;                                            // ID oferty(info) z której pochodzi ten abonament
        public string DataDodania;                                    // data dodania
    }
    //[Serializable]
    //public class FakturaKlienta
    //{
    //    public FakturaKlienta() { }
    //    public FakturaKlienta(int IDKlientaLubPrzedmiotu, bool podanoIDKlienta, int IDOferty)
    //    {
    //        if (podanoIDKlienta)
    //        {
    //            IDKlientaLubPrzedmiotu = Funkcje.NajwiększeIDPrzedmiotuKlienta("Pakiety", IDKlientaLubPrzedmiotu) + 1;
    //        }
    //        this.IDvalue = IDKlientaLubPrzedmiotu;
    //        this.IDPozycji = IDOferty;
    //    }

    //    private int IDvalue = 1;

    //    public int ID
    //    {
    //        get { return IDvalue; }
    //        set { IDvalue = value; }
    //    }
    //    public string DataTranzakcji;
    //    public string Kwota;
    //    public string NumberKonta;
    //    public string NIP;
    //    public string Opis;
    //    public int IDPozycji;                                            // ID urządzenia, abonamentu lub pakietu klienta, którego ta FakturaKlienta dotyczy
    //}

    public struct WszystkieDaneKlienta
    {
        public DaneLogowania daneLog;
        public UrządzenieKlienta[] urządzenia;
        public AbonamentKlienta[] abonamenty;
        public PakietKlienta[] pakiety;
        //public FakturaKlienta[] faktury;
    }
    #endregion

    #region KlasyŁadowaniaIZapisywania
    public class ZapisywaniePlików
    {
        public ZapisywaniePlików(object Iteracja, string nazwa, int IDKlienta = -1)
        {
            Type typ = Iteracja.GetType();
            XmlSerializer serializer = new(typ);
            string folder = "";

            if (typ.Equals(typeof(DaneLogowania)))
            {
                folder = "DaneLogowania";
            }
            else if (typ.Equals(typeof(UrządzeniaInfo)))
            {
                folder = "UrządzeniaInfo";
            }
            else if (typ.Equals(typeof(AbonamentyInfo)))
            {
                folder = "AbonamentyInfo";
            }
            else if (typ.Equals(typeof(PakietyInfo)))
            {
                folder = "PakietyInfo";
            }
            else if (typ.Equals(typeof(UrządzenieKlienta)))
            {
                folder = "UrządzeniaKlienta";
            }
            else if (typ.Equals(typeof(AbonamentKlienta)))
            {
                folder = "AbonamentyKlienta";
            }
            else if (typ.Equals(typeof(PakietKlienta)))
            {
                folder = "PakietyKlienta";
            }
            //else if (typ.Equals(typeof(FakturaKlienta)))
            //{
            //    folder = "FakturyKlienta";
            //}

            string ścieżka = Funkcje.ŚcieżkaFolderu(folder, IDKlienta);
            Stream stream = new FileStream($@"{ścieżka}\{nazwa}.xml", FileMode.Create, FileAccess.Write, FileShare.None);

            serializer.Serialize(stream, Iteracja);

            stream.Close();
        }
    }
    public class ŁadowaniePlików
    {
        public object? WczytanyPlik = null;

        public ŁadowaniePlików(string folder, string nazwa, int IDKlienta = -1)
        {
            string ścieżka = Funkcje.ŚcieżkaFolderu(folder, IDKlienta);
            XmlSerializer serializer;
            try
            {
                Stream stream = new FileStream($@"{ścieżka}\{nazwa}.xml", FileMode.Open, FileAccess.Read, FileShare.Read);
                switch (folder)
                {
                    case "DaneLogowania":
                        serializer = new XmlSerializer(typeof(DaneLogowania));
                        this.WczytanyPlik = serializer.Deserialize(stream);
                        break;

                    case "UrządzeniaInfo":
                        serializer = new XmlSerializer(typeof(UrządzeniaInfo));
                        this.WczytanyPlik = serializer.Deserialize(stream);
                        break;

                    case "AbonamentyInfo":
                        serializer = new XmlSerializer(typeof(AbonamentyInfo));
                        this.WczytanyPlik = serializer.Deserialize(stream);
                        break;

                    case "PakietyInfo":
                        serializer = new XmlSerializer(typeof(PakietyInfo));
                        this.WczytanyPlik = serializer.Deserialize(stream);
                        break;

                    case "UrządzeniaKlienta":
                        serializer = new XmlSerializer(typeof(UrządzenieKlienta));
                        this.WczytanyPlik = serializer.Deserialize(stream);
                        break;

                    case "AbonamentyKlienta":
                        serializer = new XmlSerializer(typeof(AbonamentKlienta));
                        this.WczytanyPlik = serializer.Deserialize(stream);
                        break;

                    case "PakietyKlienta":
                        serializer = new XmlSerializer(typeof(PakietKlienta));
                        this.WczytanyPlik = serializer.Deserialize(stream);
                        break;

                    //case "Faktury":
                    //    serializer = new XmlSerializer(typeof(FakturaKlienta));
                    //    this.WczytanyPlik = serializer.Deserialize(stream);
                    //    break;
                }

                stream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Błąd wczytywania pliku, \n" + e.Message);
            }
        }
    }
    public class ŁadowanieWszystkichPlików
    {
        public object[]? ListaDanych = null;
        public ŁadowanieWszystkichPlików(string typPliku, int IDKlienta = -1)
        {
            ŁadowaniePlików łp;
            int[] listaID = Funkcje.ListaID(typPliku, IDKlienta);
            this.ListaDanych = new object[listaID.Length];
            int i = 0;

            switch (typPliku)
            {
                case "DaneLogowania":
                case "UrządzeniaInfo":
                case "AbonamentyInfo":
                case "PakietyInfo":
                case "UrządzeniaKlienta":
                case "AbonamentyKlienta":
                case "PakietyKlienta":
                //case "Faktury":
                    {
                        while (i < listaID.Length)
                        {
                            łp = new ŁadowaniePlików(typPliku, listaID[i].ToString(), IDKlienta);
                            this.ListaDanych[i] = łp.WczytanyPlik;
                            i++;
                        }
                        break;
                    }
                default:
                    {
                        Console.WriteLine("  Nie istnieje taki typ pliku");
                        break;
                    }
            }

        }
    }
    #endregion

    public static class Funkcje
    {
        public static double Zaokrągl(double liczba, int miejscaPoPrzecinku = 2)
        {
            double mnożnik = Math.Pow(10, miejscaPoPrzecinku);
            double temp = liczba * mnożnik;

            if (Math.Abs(temp - Convert.ToInt64(temp)) >= 0.5)
            {
                temp = temp+(temp/Math.Abs(temp));
            }
            double wynik = (double)(Convert.ToInt32(temp) / mnożnik);
            return wynik;
        }
        /// <summary>
        /// Rozszerza tabelę do pewnego rozmiaru lub o pewną ilość (niechcianą ocje pozostawić na -1)
        /// </summary>
        /// <param name="tabela"></param>
        /// <param name="nowyRozmiar"></param>
        /// <param name="zwiększOIleś"></param>
        public static void RozszerzTabelę(ref object[] tabela, int nowyRozmiar = -1, int zwiększOIleś = -1)
        {
            if (nowyRozmiar != -1 && nowyRozmiar > tabela.Length) { tabela = new object[nowyRozmiar]; }
            else if (zwiększOIleś > 0) { tabela = new object[tabela.Length + zwiększOIleś]; }
            else { Console.WriteLine("Błędnie wprowadzone powiększenie, zwiększono o 1"); tabela = new object[tabela.Length + 1]; }
        }

        /// <summary>
        /// Rozszerza tabelę do pewnego rozmiaru lub o pewną ilość (niechcianą ocje pozostawić na -1)
        /// </summary>
        /// <param name="tabela"></param>
        /// <param name="nowyRozmiar"></param>
        /// <param name="zwiększOIleś"></param>
        public static void RozszerzTabelę(ref object[,] tabela, int nowyRozmiar1 = -1, int nowyRozmiar2 = -1, int zwiększOIleś1 = -1, int zwiększOIleś2 = -1)
        {
            if (nowyRozmiar1 > tabela.GetLength(0) && nowyRozmiar2 > tabela.GetLength(1))
            {
                tabela = new object[nowyRozmiar1, nowyRozmiar2];
            }
            else if (zwiększOIleś1 > 0 && zwiększOIleś2 > 0)
            {
                tabela = new object[tabela.GetLength(0) + zwiększOIleś1, tabela.GetLength(1) + zwiększOIleś2];
            }
            else
            {
                Console.WriteLine("Błędnie wprowadzone powiększenie, zwiększono o 1");
                tabela = new object[tabela.GetLength(0) + 1, tabela.GetLength(1) + 1];
            }
        }

        /// <summary>
        /// <para>Dostępne foldery :</para>
        /// <para>DaneLogowania,  UrządzeniaInfo,  AbonamentyInfo,  PakietyInfo,  UrządzeniaKlienta,  AbonamentyKlienta,  PakietyKlienta, Klienci</para>
        /// </summary>
        /// <param name="nazwaFolderu"></param>
        /// <param name="IDklienta"></param>
        /// <param name="rozszerzenie"></param>
        /// <returns></returns>
        public static string ŚcieżkaFolderu(string nazwaFolderu, int IDklienta = 1)
        {
            string ścieżka = Directory.GetCurrentDirectory();

            switch (nazwaFolderu)
            {
                case "DaneLogowania":
                case "UrządzeniaInfo":
                case "AbonamentyInfo":
                case "PakietyInfo":
                case "Klienci":
                    ścieżka = Path.Combine(ścieżka, nazwaFolderu);
                    break;

                case "UrządzeniaKlienta":
                case "AbonamentyKlienta":
                case "PakietyKlienta":
                //case "FakturyKlienta":
                    ścieżka = Path.Combine(ścieżka, "Klienci", (IDklienta + "\\" + nazwaFolderu));
                    break;
            }

            return ścieżka;
        }

        /// <summary>
        /// <para>Dostępne foldery :</para>
        /// <para>DaneLogowania,  UrządzeniaInfo,  AbonamentyInfo,  PakietyInfo,  UrządzeniaKlienta,  AbonamentyKlienta,  PakietyKlienta, Klienci</para>
        /// </summary>
        /// <param name="nazwaFolderu"></param>
        /// <param name="IDklienta"></param>
        /// <param name="rozszerzenie"></param>
        /// <returns></returns>
        public static int LiczbaPlików(string nazwaFolderu, int IDklienta = 1, string rozszerzenie = "*.xml")
        {
            string ścieżka = ŚcieżkaFolderu(nazwaFolderu, IDklienta);
            return Directory.GetFiles(ścieżka, rozszerzenie).Length;
        }
        public static int LiczbaKlientów()
        {
            string ścieżka = ŚcieżkaFolderu("Klienci");

            return Directory.GetDirectories(ścieżka).Length;
        }
        public static int LiczbaUrzytkowników()
        {
            return LiczbaPlików("DaneLogowania");
        }

        /// <summary>
        /// <para>Lista ID plików (lub folderów w przypadku opcji "Klienci") w wybranym folderze</para>
        /// <para>Dostępne foldery :</para>
        /// <para>DaneLogowania,  UrządzeniaInfo,  AbonamentyInfo,  PakietyInfo,  UrządzeniaKlienta,  AbonamentyKlienta,  PakietyKlienta, Klienci</para>
        /// </summary>
        /// <returns></returns>
        public static int[] ListaID(string folder, int IDKlienta = -1)
        {
            int[] lista = new int[0];
            string ścieżka = ŚcieżkaFolderu(folder, IDKlienta);
            string[] nazwy = new string[0];
            string[] zakazaneNazwy = { "rabat" };

            if (!Directory.Exists(ścieżka))
            {
                return lista;
            }
            else if (folder != "Klienci")
            {
                nazwy = Directory.GetFiles(ścieżka, "*.xml");
            }
            else
            {
                nazwy = Directory.GetDirectories(ścieżka);
            }

            int wykryteZakazane = 0;
            foreach (string nazwa in nazwy)
            {
                foreach (string zakazana in zakazaneNazwy)
                {
                    if (nazwa.Replace(@".xml", "").Replace(ścieżka + @"\", "").Contains(zakazana))
                    {
                        wykryteZakazane++;
                        break;
                    }
                }
            }

            lista = new int[nazwy.Length - wykryteZakazane];
            int i = 0;

            foreach (string nazwa in nazwy)
            {
                bool pomiń = false;

                string nazwa2 = nazwa.Replace(@".xml", "");
                string nazwa3 = nazwa2.Replace(ścieżka + @"\", "");

                foreach (string zakazana in zakazaneNazwy)
                {
                    if (nazwa3.Contains(zakazana))
                    {
                        pomiń = true;
                        break;
                    }
                }
                if (!pomiń)
                {
                    lista[i++] = int.Parse(nazwa3);
                }
            }


            return lista;
        }

        public static int NajwiększeID(string nazwaPliku, string ścieżka, int ostatnieID)
        {
            string FN = nazwaPliku.Replace(".xml", "");
            FN = FN.Replace(ścieżka + "\\", "");

            try
            {
                int id1 = int.Parse(FN);
                if (ostatnieID < id1)
                {
                    return id1;
                }
            }
            catch (Exception) { }

            return ostatnieID;
        }
        public static int NajwiększeIDUrzytkowników()                   // ID z danych logowania; pracownicy i admini nie mają własnego folderu klient
        {
            int ID = 0;       // minimalne ID - 1
            string ścieżka = ŚcieżkaFolderu("DaneLogowania");

            if (Directory.Exists(ścieżka))
            {
                foreach (string iu in Directory.GetFiles(ścieżka, "*.xml"))
                {
                    ID = NajwiększeID(iu, ścieżka, ID);
                }
            }

            return ID;
        }
        public static int NajwiększeIDKlientów()
        {
            int ID = 0;       // minimalne ID - 1
            string ścieżka = ŚcieżkaFolderu("Klienci");

            foreach (string kl in Directory.GetDirectories(ścieżka))
            {
                ID = NajwiększeID(kl, ścieżka, ID);
            }
            return ID;
        }

        /// <summary>
        /// Rodzaje przedmiotów : Urządzenia, Abonamenty, Pakiety
        /// </summary>
        /// <param name="IDklienta"></param>
        /// <param name="rodzajPrzedmiodu"></param>
        /// <returns></returns>
        public static int NajwiększeIDPrzedmiotuKlienta(string rodzajPrzedmiotu, int IDklienta)
        {
            int ID = 0;       // minimalne ID - 1
            string ścieżka = ŚcieżkaFolderu((rodzajPrzedmiotu + "Klienta"), IDklienta);

            if (Directory.Exists(ścieżka))
            {
                foreach (string pk in Directory.GetFiles(ścieżka, "*.xml"))
                {
                    ID = NajwiększeID(pk, ścieżka, ID);
                }
            }

            return ID;
        }

        /// <summary>
        /// Rodzaje przedmiotów : Urządzenia, Abonamenty, Pakiety
        /// </summary>
        /// <param name="IDklienta"></param>
        /// <param name="rodzajPrzedmiodu"></param>
        /// <returns></returns>
        public static int NajwiększeIDOferty(string rodzajPrzedmiotu)
        {
            int ID = 0;       // minimalne ID - 1
            string ścieżka = ŚcieżkaFolderu(rodzajPrzedmiotu + "Info");

            if (Directory.Exists(ścieżka))
            {
                foreach (string pk in Directory.GetFiles(ścieżka, "*.xml"))
                {
                    ID = NajwiększeID(pk, ścieżka, ID);
                }
            }

            return ID;
        }

        public static void CzyIstniejąWszystkieFoldery()
        {
            string aktualnaŚcieżka = Directory.GetCurrentDirectory();
            string[] nazwyFolderów = { "DaneLogowania", "UrządzeniaInfo", "AbonamentyInfo", "PakietyInfo", "Klienci"/*, "Rabaty" */};

            foreach (string nazwaF in nazwyFolderów)
            {
                if (!Directory.Exists(aktualnaŚcieżka + @"\" + nazwaF))
                {
                    Directory.CreateDirectory(aktualnaŚcieżka + @"\" + nazwaF);
                }
            }


        }

        /// <summary>
        /// <para>Tworzy plik z załączonej struktury danych, podać kolejno : Strukturę do zapisania, nazwę pliku, ID klienta jeśli jest potrzebne.   </para>
        /// </summary>
        /// <param name="Iteracja"></param>
        /// <param name="ID"></param>
        /// <param name="IDKlienta"></param>
        public static void ZapiszPlik(object Iteracja, string nazwa, int IDKlienta = -1)
        {
            ZapisywaniePlików zp = new(Iteracja, nazwa, IDKlienta);
        }

        /// <summary>
        /// <para>Ładuje plik, podać kolejno : nazwę folderu, nazwę folderu, nazwę pliku, ID klienta jeśli jest potrzebnea(dla 3 ostatnich folderów).   </para>
        /// <para>Dostępne foldery :</para>
        /// <para>DaneLogowania,  UrządzeniaInfo,  AbonamentyInfo,  PakietyInfo,  UrządzeniaKlienta,  AbonamentyKlienta,  PakietyKlienta</para>
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="ID"></param>
        /// <param name="IDKlienta"></param>
        /// 
        public static object WczytajPlik(string folder, string nazwa, int IDKlienta = -1)
        {
            ŁadowaniePlików łp = new(folder, nazwa, IDKlienta);
            return łp.WczytanyPlik;
        }

        #region WczytywanieWszystkiego
        /// <summary>
        /// <para>Ładuje wszystkie pliki, wystarczy podać pusta tabelą odpowiedniego typu</para>
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="ID"></param>
        /// <param name="IDKlienta"></param>
        public static DaneLogowania[] WczytajWszystkiePliki(DaneLogowania[] pustaInstancja)
        {
            string typPliku = "DaneLogowania";
            ŁadowanieWszystkichPlików łwp = new(typPliku);
            pustaInstancja = new DaneLogowania[łwp.ListaDanych.Length];
            int i = 0;

            foreach (object o in łwp.ListaDanych)
            {
                pustaInstancja[i] = (DaneLogowania)o;
                i++;
            }
            return pustaInstancja;
        }

        /// <summary>
        /// <para>Ładuje wszystkie pliki, wystarczy podać pusta tabelą odpowiedniego typu</para>
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="ID"></param>
        /// <param name="IDKlienta"></param>
        public static UrządzeniaInfo[] WczytajWszystkiePliki(UrządzeniaInfo[] pustaInstancja)
        {
            string typPliku = "UrządzeniaInfo";
            ŁadowanieWszystkichPlików łwp = new(typPliku);
            pustaInstancja = new UrządzeniaInfo[łwp.ListaDanych.Length];
            int i = 0;

            foreach (object o in łwp.ListaDanych)
            {
                pustaInstancja[i] = (UrządzeniaInfo)o;
                i++;
            }
            return pustaInstancja;
        }

        /// <summary>
        /// <para>Ładuje wszystkie pliki, wystarczy podać pusta tabelą odpowiedniego typu</para>
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="ID"></param>
        /// <param name="IDKlienta"></param>
        public static AbonamentyInfo[] WczytajWszystkiePliki(AbonamentyInfo[] pustaInstancja)
        {
            string typPliku = "AbonamentyInfo";
            ŁadowanieWszystkichPlików łwp = new(typPliku);
            pustaInstancja = new AbonamentyInfo[łwp.ListaDanych.Length];
            int i = 0;

            foreach (object o in łwp.ListaDanych)
            {
                pustaInstancja[i] = (AbonamentyInfo)o;
                i++;
            }
            return pustaInstancja;
        }

        /// <summary>
        /// <para>Ładuje wszystkie pliki, wystarczy podać pusta tabelą odpowiedniego typu</para>
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="ID"></param>
        /// <param name="IDKlienta"></param>
        public static PakietyInfo[] WczytajWszystkiePliki(PakietyInfo[] pustaInstancja)
        {
            string typPliku = "PakietyInfo";
            ŁadowanieWszystkichPlików łwp = new(typPliku);
            pustaInstancja = new PakietyInfo[łwp.ListaDanych.Length];
            int i = 0;

            foreach (object o in łwp.ListaDanych)
            {
                pustaInstancja[i] = (PakietyInfo)o;
                i++;
            }
            return pustaInstancja;
        }

        /// <summary>
        /// <para>Ładuje wszystkie pliki, wystarczy podać pusta tabelą odpowiedniego typu</para>
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="ID"></param>
        /// <param name="IDKlienta"></param>
        public static UrządzenieKlienta[] WczytajWszystkiePliki(UrządzenieKlienta[] pustaInstancja, int IDKlienta)
        {
            string typPliku = "UrządzeniaKlienta";
            ŁadowanieWszystkichPlików łwp = new(typPliku, IDKlienta);
            pustaInstancja = new UrządzenieKlienta[łwp.ListaDanych.Length];
            int i = 0;

            foreach (object o in łwp.ListaDanych)
            {
                pustaInstancja[i] = (UrządzenieKlienta)o;
                i++;
            }
            return pustaInstancja;
        }

        /// <summary>
        /// <para>Ładuje wszystkie pliki, wystarczy podać pusta tabelą odpowiedniego typu</para>
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="ID"></param>
        /// <param name="IDKlienta"></param>
        public static AbonamentKlienta[] WczytajWszystkiePliki(AbonamentKlienta[] pustaInstancja, int IDKlienta)
        {
            string typPliku = "AbonamentyKlienta";
            ŁadowanieWszystkichPlików łwp = new(typPliku, IDKlienta);
            pustaInstancja = new AbonamentKlienta[łwp.ListaDanych.Length];
            int i = 0;

            foreach (object o in łwp.ListaDanych)
            {
                pustaInstancja[i] = (AbonamentKlienta)o;
                i++;
            }
            return pustaInstancja;
        }

        /// <summary>
        /// <para>Ładuje wszystkie pliki, wystarczy podać pusta tabelą odpowiedniego typu</para>
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="ID"></param>
        /// <param name="IDKlienta"></param>
        public static PakietKlienta[] WczytajWszystkiePliki(PakietKlienta[] pustaInstancja, int IDKlienta)
        {
            string typPliku = "PakietyKlienta";
            ŁadowanieWszystkichPlików łwp = new(typPliku, IDKlienta);
            pustaInstancja = new PakietKlienta[łwp.ListaDanych.Length];
            int i = 0;

            foreach (object o in łwp.ListaDanych)
            {
                pustaInstancja[i] = (PakietKlienta)o;
                i++;
            }
            return pustaInstancja;
        }

        /// <summary>
        /// <para>Ładuje wszystkie pliki, wystarczy podać pusta tabelą odpowiedniego typu</para>
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="ID"></param>
        /// <param name="IDKlienta"></param>
        //public static FakturaKlienta[] WczytajWszystkiePliki(FakturaKlienta[] pustaInstancja, int IDKlienta)
        //{
        //    string typPliku = "Faktury";
        //    ŁadowanieWszystkichPlików łwp = new(typPliku, IDKlienta);
        //    pustaInstancja = new FakturaKlienta[łwp.ListaDanych.Length];
        //    int i = 0;

        //    foreach (object o in łwp.ListaDanych)
        //    {
        //        pustaInstancja[i] = (FakturaKlienta)o;
        //        i++;
        //    }
        //    return pustaInstancja;
        //}

        /// <summary>
        /// <para>Ładuje wszystkie dane danego klienta. Wchodzą w nie : </para>
        /// <para>Dane osobowe, Urządzenia, Abonamenty, Pakiety, Zniżki (jeśli zostaną dodane)</para>
        /// </summary>
        /// <param name="IDKlienta"></param>
        /// <returns></returns>
        public static WszystkieDaneKlienta WczytajDaneKlienta(int IDKlienta)
        {
            UrządzenieKlienta[] urzKl = new UrządzenieKlienta[1];
            AbonamentKlienta[] aboKl = new AbonamentKlienta[1];
            PakietKlienta[] pakKl = new PakietKlienta[1];
            //FakturaKlienta[] fakKl = new FakturaKlienta[1];

            DaneLogowania daneLog = (DaneLogowania)Funkcje.WczytajPlik("DaneLogowania", IDKlienta.ToString());

            WszystkieDaneKlienta dane = new WszystkieDaneKlienta();

            dane.daneLog = daneLog;
            dane.urządzenia = Funkcje.WczytajWszystkiePliki(urzKl, dane.daneLog.ID);
            dane.abonamenty = Funkcje.WczytajWszystkiePliki(aboKl, dane.daneLog.ID);
            dane.pakiety = Funkcje.WczytajWszystkiePliki(pakKl, dane.daneLog.ID);
            //dane.faktury = Funkcje.WczytajWszystkiePliki(fakKl, dane.daneLog.ID);

            return dane;
        }
        public static WszystkieDaneKlienta[] WczytajWszystkieDaneKlientów()
        {
            DaneLogowania[] daneLog = new DaneLogowania[LiczbaKlientów()];
            daneLog = Funkcje.WczytajWszystkiePliki(daneLog);

            List<int> tempListaID = new List<int>();

            foreach (DaneLogowania dl in daneLog)
            {
                if (!dl.Admin)
                {
                    tempListaID.Append(dl.ID);
                }
            }

            int[] listaIDKlientów = tempListaID.ToArray();

            WszystkieDaneKlienta[] dane = new WszystkieDaneKlienta[listaIDKlientów.Length];

            for (int i = 0; i < dane.Length; i++)
            {
                dane[i] = WczytajDaneKlienta(listaIDKlientów[i]);
            }

            return dane;
        }
        #endregion

        /// <summary>
        /// <para>Usuwa plik, podać kolejno : nazwę folderu, nazwę folderu, nazwę pliku, ID klienta jeśli jest potrzebnea(dla 3 ostatnich folderów).   </para>
        /// <para>Dostępne foldery :</para>
        /// <para>DaneLogowania,  UrządzeniaInfo,  AbonamentyInfo,  PakietyInfo,  UrządzeniaKlienta,  AbonamentyKlienta,  PakietyKlienta</para>
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="ID"></param>
        /// <param name="IDKlienta"></param>
        /// 
        public static void UsuńPlik(string folder, string nazwa, int IDKlienta = -1)
        {
            string ścieżka = ŚcieżkaFolderu(folder, IDKlienta) + @$"\{nazwa}.xml";

            File.Delete(ścieżka);
        }

        /// <summary>
        /// <para>Usuwa klienta, podać kolejno : ID klienta.   </para>
        /// </summary>
        /// <param name="IDKlienta"></param>
        /// 
        public static void UsuńKlienta(int IDKlienta)
        {
            UsuńPlik("DaneLogowania", IDKlienta.ToString());

            Directory.Delete(ŚcieżkaFolderu("Klienci"), true);
        }
    }
}