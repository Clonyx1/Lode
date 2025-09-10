using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip_hra_v_konzoli
{
    public class CustomHra : Hra
    {
        HraciPole[,] pole = new HraciPole[10, 10];

        //Konstruktor
        public CustomHra()
        {
            VyplneniPolePraznymiHodnotami();
        }
        //Metody
        public HraciPole[,] NovaHra()
        {
            Lode[4] = PridaniLodi("Člun", 2);
            Lode[3] = PridaniLodi("Ponorka", 3);
            Lode[2] = PridaniLodi("Křižník", 3);
            Lode[1] = PridaniLodi("Bitevní loď", 4);
            Lode[0] = PridaniLodi("Letadlová loď", 5);

            HraciPole[,] hraciPole = InicializaceHracihoPole(Lode.ToArray());

            return hraciPole;
        }
        public Lod PridaniLodi(string nazev, int delka)
        {
            while (true)
            {
                Console.WriteLine($"Zadejte souřadnice {nazev}(délka {delka}) ve tvaru: x, y, orientace(v/h), tudíž např. 1, 2, v (= počáteční souřadnice 1x, 2y, orientace vertikální)");
                string odpoved = Console.ReadLine();

                string[] parametryLodi = odpoved.Split(',');

                Orientace o = (parametryLodi[2].Trim() == "v") ? Orientace.Vertikalni : Orientace.Horizontalni;
                int x = Convert.ToInt32(parametryLodi[0]);
                int y = Convert.ToInt32(parametryLodi[1]);

                Lod lod = new Lod(nazev, delka, o, (x, y));
                if (JeValidni(lod))
                {
                    ZabratSouradnice(lod);
                    ZobrazitRozlozeniLodi(lod);
                    return lod;
                }
                else
                {
                    Console.WriteLine("Zadali jste neplatný vstup, zkuste to znovu!");
                }
            }
        }
        private void VyplneniPolePraznymiHodnotami()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    pole[i, j] = new HraciPole("~", null, false);
                }
            }
        }
        private void ZobrazitRozlozeniLodi(Lod lod)
        {
            foreach(var souradnice in lod.SouradniceLode())
            {
                int X = souradnice.X;
                int Y = souradnice.Y;

                pole[X, Y].Znak = "X";
                pole[X, Y].Pouzito = false;
                pole[X, Y].Lod = lod;
            }
            for (int y = 0; y < pole.GetLength(1); y++)
            {
                for (int x = 0; x < pole.GetLength(0); x++)
                {
                    Console.Write(pole[x, y].Znak + " ");
                }
                Console.WriteLine();
            }
        }
        private bool JeValidni(Lod lod)
        {
            foreach(var souradnice in lod.SouradniceLode())
            {
                if (zabraneSouradnice.Contains(souradnice)) return false;
            }
            return true;
        }
    }
}
