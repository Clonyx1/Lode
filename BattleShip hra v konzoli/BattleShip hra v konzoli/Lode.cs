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
            int poziceX = 0;
            int poziceY = 0;
            if (orientace == Orientace.Horizontalni)
            {
                poziceX = (int)r.Next(0, 7);
                if (poziceX + delka > 9) poziceX -= delka;
                poziceY = (int)r.Next(0, 9);
            }
            else
            {
                poziceX = (int)r.Next(0, 9);
                poziceY = (int)r.Next(0, 9);
                if(poziceY + delka > 9) poziceY -= delka;
            }

            return (poziceX, poziceY);
        }
        //Metoda pro zajištění, že startovní pozice je validní
        private (int X, int Y) KontrolaStartovniPozice(int delka, Orientace o)
        {
            HashSet<(int x, int y)> vsechnySouraniceLodi = new HashSet<(int x, int y)>();
            (int X, int Y) pocatecniSouradnice;
            bool pokracovat;
            do
            {
                pokracovat = false;
                pocatecniSouradnice = StartovaciPozice(delka, o);
                if(o == Orientace.Horizontalni)
                {
                    for(int i = 0; i < delka; i++)
                    {
                        (int X, int Y) souradnice = (pocatecniSouradnice.X + i, pocatecniSouradnice.Y);
                        vsechnySouraniceLodi.Add(souradnice);
                    }
                }
                else
                {
                    for (int i = 0; i < delka; i++)
                    {
                        (int X, int Y) souradnice = (pocatecniSouradnice.X, pocatecniSouradnice.Y+i);
                        vsechnySouraniceLodi.Add(souradnice);
                    }
                }
                foreach(var souradnice in vsechnySouraniceLodi)
                {
                    if (zabraneSouradnice.Contains(souradnice)) pokracovat = true;
                }
            }
            while (pokracovat);

            return pocatecniSouradnice;
        }
        //Vygenerovat lodě
        private void NastaveniLodi(out Lod letadlova, out Lod bitevni, out Lod kriznik, out Lod ponorka, out Lod clun)
        {
            //Nastavení letadlové lodě
            Orientace o = NahodnaOrientace();
            letadlova = new Lod("Letadlová loď", 5, o, KontrolaStartovniPozice(5, o));
            foreach(var souradnice in letadlova.SouradniceLode()) zabraneSouradnice.Add(souradnice);
            //Nastavení bitevní lodě
            o = NahodnaOrientace();
            bitevni = new Lod("Bitevní loď", 4, o, KontrolaStartovniPozice(5, o));
            foreach (var souradnice in bitevni.SouradniceLode()) zabraneSouradnice.Add(souradnice);
            //Nastavení křižníku
            o = NahodnaOrientace();
            kriznik = new Lod("Křižník", 3, o, KontrolaStartovniPozice(3, o));
            foreach (var souradnice in kriznik.SouradniceLode()) zabraneSouradnice.Add(souradnice);
            //Nastavení ponorky
            o = NahodnaOrientace();
            ponorka = new Lod("Ponorka", 3, o, KontrolaStartovniPozice(3, o));
            foreach (var souradnice in ponorka.SouradniceLode()) zabraneSouradnice.Add(souradnice);
            //Nastavení člunu
            o = NahodnaOrientace();
            clun = new Lod("Člun", 2, o, KontrolaStartovniPozice(2, o));
        }
    }
}
