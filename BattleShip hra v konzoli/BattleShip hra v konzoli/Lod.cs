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
        public HashSet<(int, int)> Zasahy { get; set; } = new();
        //Konstruktor
        public Lod(string n, int d, Orientace o, (int X, int Y) pozice) 
        {
            Nazev = n;
            Delka = d;
            Orientace = o;
            PocatecniPozice = pozice;
        }
        //Metody
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
