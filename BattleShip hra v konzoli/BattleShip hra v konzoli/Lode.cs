using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip_hra_v_konzoli
{
    public class Lode
    {
        private HashSet<(int, int)> zabraneSouradnice = new HashSet<(int, int)>();
        private static readonly Random r = new Random();
        public Lod letadlova = null!;
        public Lod bitevni = null!;
        public Lod kriznik = null!;
        public Lod ponorka = null!;
        public Lod clun = null!;
        public Lode()
        {
        }
        //Metody
        public HraciPole[,] NovaHra()
        {
            letadlova = InicializaceLodi("Letadlová loď", 5);
            bitevni = InicializaceLodi("Bitevní loď", 4);
            kriznik = InicializaceLodi("Křižník", 3);
            ponorka = InicializaceLodi("Ponorka", 3);
            clun = InicializaceLodi("Člun", 2);

            HraciPole[,] hraciPole = InicializaceHracihoPole();

            return hraciPole;
        }
        private HraciPole[,] InicializaceHracihoPole()
        {
            HraciPole[,] hraciPole = new HraciPole[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    hraciPole[i, j] = new HraciPole("~", null, false);
                }
            }
            Lod[] lode = { letadlova, bitevni, kriznik, ponorka, clun };

            foreach(var lod in lode) NaplneniHracihoPole(hraciPole, lod);

            return hraciPole;
        }
        private void NaplneniHracihoPole(HraciPole[,] hraciPole, Lod lod)
        {
            IEnumerable<(int X, int Y)> poziceLodi = lod.SouradniceLode();
            foreach (var pozice in poziceLodi)
            {
                int X = pozice.X;
                int Y = pozice.Y;

                hraciPole[X, Y] = new HraciPole("~", lod, false);
            }
        }
        //Vygeneruje lodi náhodnou orientaci
        public Orientace NahodnaOrientace(int delka)
        {
            //Zjednodušený zápis bez ternárního operátoru díky deklaraci enumu
            Orientace orientace = (Orientace) r.Next(0, 2);
            List<(int X, int Y)> moznosti = VolnePozice(delka, orientace);

            if(moznosti.Count == 0 && orientace == Orientace.Horizontalni)
            {
                orientace = Orientace.Vertikalni;
            }
            else if(moznosti.Count == 0 && orientace == Orientace.Vertikalni)
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
        private void ZabratSouradnice(Lod lod)
        {
            int pocatecniX = lod.PocatecniPozice.X;
            int pocatecniY = lod.PocatecniPozice.Y;
            if (lod.Orientace == Orientace.Vertikalni)
            {
                if (pocatecniY != 0) zabraneSouradnice.Add((pocatecniX, pocatecniY - 1));
                if (pocatecniY + lod.Delka-1 < 9) zabraneSouradnice.Add((pocatecniX, pocatecniY + lod.Delka));

                foreach (var souradnice in lod.SouradniceLode())
                {
                    int X = souradnice.X;
                    int Y = souradnice.Y;

                    zabraneSouradnice.Add(souradnice);
                    zabraneSouradnice.Add((X + 1, Y));
                    zabraneSouradnice.Add((X - 1, Y));
                }
            }
            else
            {
                if (pocatecniX != 0) zabraneSouradnice.Add((pocatecniX-1, pocatecniY));
                if (pocatecniX + lod.Delka-1 < 9) zabraneSouradnice.Add((pocatecniX+lod.Delka, pocatecniY));

                foreach (var souradnice in lod.SouradniceLode())
                {
                    int X = souradnice.X;
                    int Y = souradnice.Y;

                    zabraneSouradnice.Add(souradnice);
                    zabraneSouradnice.Add((X, Y + 1));
                    zabraneSouradnice.Add((X, Y - 1));
                }
            }
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
