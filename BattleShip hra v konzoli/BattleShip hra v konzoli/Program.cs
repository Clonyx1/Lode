using BattleShip_hra_v_konzoli;
using System.Runtime.CompilerServices;


class Program
{
    static void Main(string[] args)
    {
        HerniLogika momentalniHra;

        string[,] viditelnaPoleAI = new string[10, 10];

        bool vyhra = false;
        var stavHry = StavHry.HraProbiha;

         while (true)
         {
             if (stavHry == StavHry.VyhraHrac)
             {
                 Console.WriteLine("Vyhráli jste");
                 Thread.Sleep(4000);
             }
             if (stavHry == StavHry.VyhraAI)
             {
                 Console.WriteLine("AI vyhrává");
                 Thread.Sleep(4000);
             }
             stavHry = StavHry.HraProbiha;

             Console.Clear();
             Console.WriteLine("Přeje si hrát s: ");
             Console.WriteLine("1) Hru s náhodným rozmístěním lodí");
             Console.WriteLine("2) Hru, kde si umístíte lodě sami");
             Console.WriteLine("\nZadejte 1 nebo 2 dle Vašeho výběru");

             try
             {
                 string vstup = Console.ReadLine();

                 bool randomHra = (Convert.ToInt32(vstup) == 1) ? true : false;

                 if (randomHra)
                 {
                     momentalniHra = new(randomHra);

                     while (stavHry == StavHry.HraProbiha)
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
                             Thread.Sleep(2000);
                         }

                         stavHry = momentalniHra.StavHry;
                     }
                 }
                 else
                 {
                     List<Lod> lode = new();

                     Console.Clear();

                     momentalniHra = new(randomHra);
                     while (stavHry == StavHry.HraProbiha)
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
                             Thread.Sleep(2000);
                         }

                         stavHry = momentalniHra.StavHry;
                     }
                 }
             }
             catch
             {
                 Console.WriteLine("Zadejte validní odpověď");
                 Thread.Sleep(2000);
             }
         }

        //PRO TESTOVACÍ ÚČELY
       /* bool randomHra = true;
        momentalniHra = new(randomHra);

        while (stavHry == StavHry.HraProbiha)
        {
            Console.Clear();
            momentalniHra.ZobrazitPole("Pole nepřítele", momentalniHra.ai.Pole);
            momentalniHra.ZobrazitPole("Vaše pole", momentalniHra.hrac.Pole);

            Console.WriteLine("Napište souřadnice, na které chcete útočit ve formátu: X, Y nebo X Y");
            string souradnice = Console.ReadLine();
            souradnice = souradnice.Trim();

            string[] cislice = new string[2];

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

            stavHry = momentalniHra.StavHry;
        }*/
    }
}

