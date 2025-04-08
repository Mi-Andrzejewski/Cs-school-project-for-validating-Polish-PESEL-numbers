namespace Latwy_projekt
{
    public class Dane
    {
        private string pesel;
        private string nazwaMiasta;
        private string imie;
        private string nazwisko;

        // konstruktor domyślny ustawia wartości początkowe
        public Dane()
        {
            pesel = "Nieznany";
            nazwaMiasta = "Nieznany";
            imie = "Nieznany";
            nazwisko = "Nieznany";
        }

        // Konstruktor parametryczny inicjalizuje obiekt danymi użytkownika
        public Dane(string _pesel, string _nazwaMiasta, string _imie, string _nazwisko)
        {
            pesel = _pesel;
            nazwaMiasta = _nazwaMiasta;
            imie = _imie;
            nazwisko = _nazwisko;
        }


        /*
         Ta funkcja pozwala na ustawienie wartości pól | get = zwraca aktualną wartość | set = ustawania nową wartość
        funkcja: IsNullOrWhiteSpace = sprawdza czy wpis nie jest pusty lub spacją
        funkcja: throw new ArgumentException = ukazuje wyjątek jeśli jest coś nie tak jak puste pole
         */
        public string Pesel
        {
            get { return pesel; }
            set { pesel = value; }
        }

        public string NazwaMiasta
        {
            get { return nazwaMiasta; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Nazwa Miasta nie może być pusta.");
                nazwaMiasta = value;
            }
        }

        public string Imie
        {
            get { return imie; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Imię nie może być puste.");
                imie = value;
            }
        }
        public string Nazwisko
        {
            get { return nazwisko; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Nazwisko nie może być puste.");
                nazwisko = value;
            }
        }

        /* Metoda pozwalająca na wprowadzenie danych osobowych przez konsolę które są w pętli aby nie pozwolić
         * na wpisanie niewłaściwej wartości (puste pole lub spacja), jeśli wszystko jest dobrze to "break" kończy pętle */

        public void iiinformacje()
        {
            Console.WriteLine("Proszę uzupełnić dane");
            while (true)
            {
                Console.Write("Nazwa Miasta: ");
                var input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    NazwaMiasta = input;
                    break;
                }
                Console.WriteLine("Należy uzupełnić podać nazwę miasta.");
            }

            while (true)
            {
                Console.Write("Imię: ");
                var input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    Imie = input;
                    break;
                }
                Console.WriteLine("Obywatel musi posiadać imię.");
            }

            while (true)
            {
                Console.Write("Nazwisko: ");
                var input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    Nazwisko = input;
                    break;
                }
                Console.WriteLine("Nazwisko nie może być puste.");
            }

            Console.Write("Pesel: ");
            Pesel = Console.ReadLine();

            Console.WriteLine();

        }

        /* Ta metoda zapisuje dane do pliku Dane.txt | Jeśli plik nie istnieje zostanie utworzony 
         * a konsola przez chwilę pokarze komunikat o utworzeniu obywatela, po czym konsola zostaje oczyszczona
         */

        public void ZapisDoPliku()
        {
            string path = "Dane.txt";
            string content = $"Pesel: {Pesel}\nNazwa Miasta: {NazwaMiasta}\nImię: {Imie}\nNazwisko: {Nazwisko}\n";
            File.AppendAllText(path, content + "\n");
            Console.WriteLine("Dane obywatela zapisane do pliku.");
            Console.Clear();
        }
    }

    /* 
     * Ta funkcja sprawdza poprawność peselu, numer musi posiadać 11 cyfr. Kod Pesel składa się z:
     * YY MM DD XXXX K  | Y = Rok | M = Miesiąc | D = dzień |
     * X = Liczba porządkowa oznaczająca płeć | K = Cyfra kontrolna | 
     * Piersze 10 liczb jest mnożone przez wagi (wagi znalezione w Internecie (https://mat.ug.edu.pl/~akarpowi/pesel.pdf), spawdzone poprzez wprowadzanie 
     * numerów Pesel z generatorów numeru Pesel i poprzez własnoręcznie wpisywanie numerów niezgodnych aby sprawdzić poprawność działania.
     * Ostatnia jedenasta cyfra kontrolna oblicza sumę modulo i sprawdza czy wynik jest zgodny z ostatnią cyfrą PESELU.
     */
    public class CzyPoprawnyPesel
    {
        public static bool PoprawnoscPesel(string pesel)
        {
            if (pesel.Length != 11 || !pesel.All(char.IsDigit))
                return false;

            int[] wagi = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
            int sumaKontrolna = 0;

            for (int i = 0; i < wagi.Length; i++)
                sumaKontrolna += wagi[i] * (pesel[i] - '0');

            int ostatniaCyfra = (10 - (sumaKontrolna % 10)) % 10;
            return ostatniaCyfra == (pesel[10] - '0');
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            /* Dictionary = słownik przechowujący jedyną unikalną wartość jaką jest PESEL
             * char wybór [Y/n] wzorowane na Linuxowym:"Do you want to continue? [Y/n] "
             * W pętli jest proces tworzenia nowego obiektu obywatel następnie zostaje wywołana metoda
             * do wprowadzania danych po czym przebiega sprawdzenie poprawności numeru PESEL 
             * i sprawdzenie czy numer PESEL nie został wcześniej użyty, jeśli tak to dane obywatela są aktualizowane
             * ostatnią czynnością jest zapisanie danych do pliku Dane.txt
             */

            Dictionary<string, Dane> obywatele = new Dictionary<string, Dane>();
            char wybor = 'Y';

            do
            {
                Dane Obywatel = new Dane();
                Obywatel.iiinformacje();

                if (!CzyPoprawnyPesel.PoprawnoscPesel(Obywatel.Pesel))
                {
                    Console.WriteLine("Błąd: Niepoprawny numer PESEL. Obywatel nie zostanie zapisany.\n");
                }
                else
                {
                    if (obywatele.ContainsKey(Obywatel.Pesel))
                    {
                        obywatele[Obywatel.Pesel] = Obywatel;
                        Console.WriteLine("Zaktualizowano dane istniejącego obywatela.\n");
                    }
                    else
                    {
                        obywatele.Add(Obywatel.Pesel, Obywatel);
                        Console.WriteLine("Dodano nowego obywatela.\n");
                    }
                }

                Console.WriteLine("Czy stworzyć następnego obywatela? [Y/n]");
                 var input = Console.ReadLine();
                 wybor = string.IsNullOrEmpty(input) ? 'Y' : char.ToUpper(input[0]);
                Console.WriteLine();

            } while (wybor == 'Y');

            foreach (var obywatel in obywatele.Values)
            {
                obywatel.ZapisDoPliku();
            }
        }
    }
}
