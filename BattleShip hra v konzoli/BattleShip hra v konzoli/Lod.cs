using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip_hra_v_konzoli
{
    public enum Orientace
    {
        Horizontalni = 0,
        Vertikalni = 1
    }
    public class Lod
    {
        public string Nazev {  get; set; }
        public int Delka { get; set; }
        public (int X, int Y) PocatecniPozice { get; set; }
        public Orientace Orientace { get; set; }
        public int Zasahy { get; set; } = new();
        public HashSet<(int X, int Y)> SousedniSouradnice { get; set; } = new();
        //Konstruktor
        public Lod(string n, int d, Orientace o, (int X, int Y) pozice) 
        {
            Nazev = n;
            Delka = d;
            Orientace = o;
            PocatecniPozice = pozice;

            SousedniSouradnice = NajitSousedniSouradnice();
        }
        //Metody
        private HashSet<(int X, int Y)> NajitSousedniSouradnice()
        {
            List<(int X, int Y)> souradniceLodi = SouradniceLode().ToList();

            HashSet<(int X, int Y)> sousedniSouradnice = new();

            int pocatecniX = PocatecniPozice.X;
            int pocatecniY = PocatecniPozice.Y;

            if (pocatecniX - 1 >= 0 && Orientace == Orientace.Horizontalni) sousedniSouradnice.Add((pocatecniX - 1, pocatecniY));
            if (pocatecniX + Delka <= 9 && Orientace == Orientace.Horizontalni) sousedniSouradnice.Add((pocatecniX + Delka, pocatecniY));

            if (pocatecniY - 1 >= 0 && Orientace == Orientace.Vertikalni) sousedniSouradnice.Add((pocatecniX, pocatecniY - 1));
            if (pocatecniY + Delka <= 9 && Orientace == Orientace.Vertikalni) sousedniSouradnice.Add((pocatecniX, pocatecniY + Delka));

            foreach(var s in souradniceLodi)
            {
                int x = s.X;
                int y = s.Y;

                if(Orientace == Orientace.Horizontalni)
                {
                    if (y - 1 >= 0) sousedniSouradnice.Add((x, y - 1));
                    if (y + 1 <= 9) sousedniSouradnice.Add((x, y + 1));
                }
                else
                {
                    if (x - 1 >= 0) sousedniSouradnice.Add((x - 1, y));
                    if (x + 1 <= 9) sousedniSouradnice.Add((x + 1, y));
                }
            }
            return sousedniSouradnice;
        }
        public IEnumerable<(int X, int Y)> SouradniceLode()
        {
            for(int i =0; i < Delka; i++)
            {
                yield return Orientace == Orientace.Horizontalni ? 
                    (PocatecniPozice.X+i, PocatecniPozice.Y) : (PocatecniPozice.X, PocatecniPozice.Y+i);
            }
        }
    }
}
