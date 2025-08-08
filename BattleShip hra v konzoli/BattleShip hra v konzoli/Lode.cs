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
        Lod letadlova;
        Lod bitevni;
        Lod kriznik;
        Lod ponorka;
        Lod clun;
        public Lode()
        {
        }
        //Metody
        public string[,] NovaHra()
        {
            //Nastavení hracího pole a vytvoření lodí
            string[,] hraciPole = new string[10, 10];
            NastaveniLodi(out letadlova, out bitevni, out kriznik, out ponorka, out clun);

            IEnumerable<(int X, int Y)> souradniceLodi = letadlova.SouradniceLode();

            foreach (var souradnice in souradniceLodi) hraciPole[souradnice.X, souradnice.Y] = "X";
            
            souradniceLodi = bitevni.SouradniceLode();
            foreach(var souradnice in souradniceLodi) hraciPole[souradnice.X, souradnice.Y] = "X";
            
            souradniceLodi = kriznik.SouradniceLode();
            foreach (var souradnice in souradniceLodi) hraciPole[souradnice.X, souradnice.Y] = "X";

            souradniceLodi = ponorka.SouradniceLode();
            foreach (var souradnice in souradniceLodi) hraciPole[souradnice.X, souradnice.Y] = "X";

            souradniceLodi = clun.SouradniceLode();
            foreach (var souradnice in souradniceLodi) hraciPole[souradnice.X, souradnice.Y] = "X";

            return hraciPole;
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
        //Vygenerovat lodě
        private void NastaveniLodi(out Lod letadlova, out Lod bitevni, out Lod kriznik, out Lod ponorka, out Lod clun)
        {
            int delka = 5;

            Orientace o = NahodnaOrientace(5);
            letadlova = new Lod("Letadlová loď", delka, o, StartovaciPozice(delka, o));
            ZabratSouradnice(letadlova);

            delka = 4;

            o = NahodnaOrientace(4);
            bitevni = new Lod("Bitevní loď", delka, o, StartovaciPozice(delka, o));
            ZabratSouradnice(bitevni);

            delka = 3;

            o = NahodnaOrientace(3);
            kriznik = new Lod("Křižník", delka, o, StartovaciPozice(delka, o));
            ZabratSouradnice(kriznik);
            
            o = NahodnaOrientace(3);
            ponorka = new Lod("Ponorka", delka, o, StartovaciPozice(delka, o));
            ZabratSouradnice(ponorka);

            delka = 2;
            o = NahodnaOrientace(2);
            clun = new Lod("Člun", delka, o, StartovaciPozice(delka, o));

        }
    }
}
