using BattleShip_hra_v_konzoli;
using System.Runtime.CompilerServices;


class Program
{
    static void Main(string[] args)
    {
        HerniLogika momentalniHra;

        string[,] viditelnaPoleAI = new string[10, 10];

        bool vyhra = false;

        while (true)
        {
            Console.WriteLine("Přeje si dostat náhodně rozmístěné lodě nebo si je chcete umístit sami? Zadejte 1 nebo 2");
            string vstup = Console.ReadLine();

            bool randomHra = (Convert.ToInt32(vstup) == 1) ? true : false;

            if (randomHra)
            {
                momentalniHra = new(randomHra);
                while (!vyhra)
                {
                    Console.Clear();
                    momentalniHra.ZobrazitPole("Pole nepřítele", momentalniHra.ai.Pole);
                    momentalniHra.ZobrazitPole("Vaše pole", momentalniHra.hrac.Pole);

                    Console.WriteLine("Napište souřadnice, na které chcete útočit ve formátu: X, Y nebo X Y");
                    string souradnice = Console.ReadLine();
                    souradnice = souradnice.Trim();

                    string[] cislice = new string[2];
                    try
                    {
                        if (souradnice.Contains(','))
                        {
                            cislice = souradnice.Split(",");
                        }
                        else
                        {
                            cislice = souradnice.Split(" ");
                        }
                        (int X, int Y) bodZasahu = (Convert.ToInt32(cislice[0]), Convert.ToInt32(cislice[1]));
                        momentalniHra.Utok(bodZasahu);
                    }
                    catch
                    {
                        Console.Clear();
                        Console.WriteLine("Zadejte validní souřadnice!");
                        Thread.Sleep(4000);
                    }

                    vyhra = momentalniHra.Vyhra();
                }
            }
            else
            {
                List<Lod> lode = new();

                Console.Clear();

                momentalniHra = new(randomHra);
                while (!vyhra)
                {
                    Console.Clear();
                    momentalniHra.ZobrazitPole("Pole nepřítele", momentalniHra.ai.Pole);
                    momentalniHra.ZobrazitPole("Vaše pole", momentalniHra.hrac.Pole);

                    Console.WriteLine("Napište souřadnice, na které chcete útočit ve formátu: X, Y nebo X Y");
                    string souradnice = Console.ReadLine();
                    souradnice = souradnice.Trim();

                    string[] cislice = new string[2];
                    try
                    {
                        if (souradnice.Contains(','))
                        {
                            cislice = souradnice.Split(",");
                        }
                        else
                        {
                            cislice = souradnice.Split(" ");
                        }
                        (int X, int Y) bodZasahu = (Convert.ToInt32(cislice[0]), Convert.ToInt32(cislice[1]));
                        momentalniHra.Utok(bodZasahu);
                    }
                    catch
                    {
                        Console.Clear();
                        Console.WriteLine("Zadejte validní souřadnice!");
                        Thread.Sleep(4000);
                    }

                    vyhra = momentalniHra.Vyhra();
                }
            }
        }
    }
}

