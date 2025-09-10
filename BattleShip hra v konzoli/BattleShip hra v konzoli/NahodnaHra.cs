using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip_hra_v_konzoli
{
    public class NahodnaHra : Hra
    {
        private static readonly Random r = new Random();

        //Konstruktor
        public NahodnaHra()
        {

        }
        //Metody
        public HraciPole[,] NovaHra()
        {
            Lode[0] = InicializaceLodi("Letadlová loď", 5);
            Lode[1] = InicializaceLodi("Bitevní loď", 4);
            Lode[2] = InicializaceLodi("Křižník", 3);
            Lode[3] = InicializaceLodi("Ponorka", 3);
            Lode[4] = InicializaceLodi("Člun", 2);

            HraciPole[,] hraciPole = InicializaceHracihoPole(Lode);

            return hraciPole;
        }
        //Vygeneruje lodi náhodnou orientaci
        public Orientace NahodnaOrientace(int delka)
        {
            //Zjednodušený zápis bez ternárního operátoru díky deklaraci enumu
            Orientace orientace = (Orientace)r.Next(0, 2);
            List<(int X, int Y)> moznosti = VolnePozice(delka, orientace);

            if (moznosti.Count == 0 && orientace == Orientace.Horizontalni)
            {
                orientace = Orientace.Vertikalni;
            }
            else if (moznosti.Count == 0 && orientace == Orientace.Vertikalni)
            {
                orientace = Orientace.Horizontalni;
            }
            return orientace;
        }
        //Vygeneruje náhodnou startovací pozici
        public (int X, int Y) StartovaciPozice(int delka, Orientace orientace)
        {
            Random r = new Random();
            List<(int X, int Y)> moznosti = VolnePozice(delka, orientace);

            (int X, int Y) vybranaPozice = moznosti[r.Next(moznosti.Count)];

            return vybranaPozice;
        }
        public List<(int X, int Y)> VolnePozice(int delka, Orientace orientace)
        {
            List<(int X, int Y)> moznosti = new List<(int X, int Y)>();

            int maximalniX = orientace == Orientace.Horizontalni ? 10 - delka : 10;
            int maximalniY = orientace == Orientace.Vertikalni ? 10 - delka : 10;

            for (int y = 0; y < maximalniY; y++)
            {
                for (int x = 0; x < maximalniX; x++)
                {
                    bool koliduje = false;
                    for (int i = 0; i < delka; i++)
                    {
                        int delkaX = x + (orientace == Orientace.Horizontalni ? i : 0);
                        int delkaY = y + (orientace == Orientace.Vertikalni ? i : 0);

                        if (zabraneSouradnice.Contains((delkaX, delkaY)))
                        {
                            koliduje = true;
                            break;
                        }
                    }
                    if (!koliduje) moznosti.Add((x, y));
                }
            }
            return moznosti;
        }
        private Lod InicializaceLodi(string nazev, int delka)
        {
            Orientace o = NahodnaOrientace(delka);

            var lod = new Lod(nazev, delka, o, StartovaciPozice(delka, o));
            ZabratSouradnice(lod);
            return lod;
        }
    }
}
