using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip_hra_v_konzoli
{
    public class Lode
    {
        private List<(int, int)> zabraneSouradnice = new List<(int, int)>();
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
        public Orientace NahodnaOrientace()
        {
            Random r = new Random();
            int nahoda = (int)r.Next(0,2);

            return nahoda == 0 ? Orientace.Horizontalni : Orientace.Vertikalni;
        }
        //Vygeneruje náhodnou startovací pozici
        public (int X, int Y) StartovaciPozice(int delka, Orientace orientace)
        {
            Random r = new Random();
            List<(int X, int Y)> moznosti = new List<(int X, int Y)>();

            int maximalniX = orientace == Orientace.Horizontalni ? 9 - delka : 9;
            int maximalniY = orientace == Orientace.Vertikalni ? 9 - delka : 9;

            for(int y = 0; y < maximalniY; y++)
            {
                for(int x = 0; x < maximalniX; x++)
                {
                    bool koliduje = false;
                    for (int i = 0; i < delka; i++)
                    {
                        int delkaX = x + (orientace == Orientace.Horizontalni ? i : 0);
                        int delkaY = y + (orientace == Orientace.Vertikalni ? 0 : i);

                        if (zabraneSouradnice.Contains((delkaX, delkaY)))
                        {
                            koliduje = true;
                            break;
                        }
                    }
                    if (!koliduje) moznosti.Add((x, y));
                }
            }

            return moznosti[r.Next(moznosti.Count)];
        }
        //Vygenerovat lodě
        private void NastaveniLodi(out Lod letadlova, out Lod bitevni, out Lod kriznik, out Lod ponorka, out Lod clun)
        {
            int delka = 5;

            Orientace o = NahodnaOrientace();
            letadlova = new Lod("Letadlová loď", delka, o, StartovaciPozice(delka, o));
            Console.WriteLine("Počáteční pozice letadlové lodi: " + letadlova.PocatecniPozice + ", orientace: " + o);
            foreach(var souradnice in letadlova.SouradniceLode()) zabraneSouradnice.Add(souradnice);

            delka = 4;
            o = NahodnaOrientace();
            bitevni = new Lod("Bitevní loď", delka, o, StartovaciPozice(delka, o));
            Console.WriteLine("Počáteční pozice bitevní lodi: " + bitevni.PocatecniPozice + ", orientace: " + o);
            foreach (var souradnice in bitevni.SouradniceLode()) zabraneSouradnice.Add(souradnice);

            delka = 3;
            o = NahodnaOrientace();
            kriznik = new Lod("Křižník", delka, o, StartovaciPozice(delka, o));
            Console.WriteLine("Počáteční pozice křižníku: " + kriznik.PocatecniPozice + ", orientace: " + o);
            foreach (var souradnice in kriznik.SouradniceLode()) zabraneSouradnice.Add(souradnice);
            
            o = NahodnaOrientace();
            ponorka = new Lod("Ponorka", delka, o, StartovaciPozice(delka, o));
            Console.WriteLine("Počáteční pozice ponorky: " + ponorka.PocatecniPozice + ", orientace: " + o);
            foreach (var souradnice in ponorka.SouradniceLode()) zabraneSouradnice.Add(souradnice);

            delka = 2;
            o = NahodnaOrientace();
            clun = new Lod("Člun", delka, o, StartovaciPozice(delka, o));
            Console.WriteLine("Počáteční pozice člunu: " + clun.PocatecniPozice + ", orientace: " + o);
        }
    }
}
