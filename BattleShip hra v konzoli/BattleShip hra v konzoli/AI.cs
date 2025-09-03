using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip_hra_v_konzoli
{
    public class AI : Hrac
    {
        private List<(int X, int Y)> Zasahy = new List<(int X, int Y)>();
        public List<(int X, int Y)> pouzitelnaPole = new List<(int X, int Y)>();
        //Konstruktor
        public AI(HraciPole[,] pole) : base(pole) 
        {
            
        }
        //Metody
        public void Utok()
        {

            switch (Zasahy.Count)
            {
                case 0:
                    UtokRandom();
                    break;
                case 1:
                    UtokSousedniSouradniceRandom();
                    break;
                case >= 2:
                    UtokNajitaLod();
                    break;
                default:
                    Console.WriteLine("Něco se nepovedlo - Utok NPC");
                    Thread.Sleep(4000);
                    break;
            }

        }
        private void UtokRandom()
        {
            (int X, int Y) souradnice = pouzitelnaPole[r.Next(pouzitelnaPole.Count)];

            int X = souradnice.X;
            int Y = souradnice.Y;

            VysledekZasahu vysledek = KontrolaZasahu(X, Y);
            if (vysledek == VysledekZasahu.Zasah) Zasahy.Add(souradnice);

            pouzitelnaPole.Remove(souradnice);
        }
        private void UtokSousedniSouradniceRandom()
        {
            (int X, int Y) souradnice = Zasahy[0];

            int X = souradnice.X;
            int Y = souradnice.Y;

            List<(int X, int Y)> mozneSouradnice = new List<(int X, int Y)>();

            mozneSouradnice.Add((X - 1, Y));
            mozneSouradnice.Add((X + 1, Y));
            mozneSouradnice.Add((X, Y - 1));
            mozneSouradnice.Add((X, Y + 1));

            mozneSouradnice.RemoveAll(s => !(pouzitelnaPole.Contains(s) && (s.X >= 0 && s.X <= 9) && (s.Y >= 0 && s.Y <= 9)));

            souradnice = mozneSouradnice[r.Next(mozneSouradnice.Count)];

            X = souradnice.X;
            Y = souradnice.Y;

            VysledekZasahu vysledek = KontrolaZasahu(X, Y);
            if (vysledek == VysledekZasahu.Zasah) Zasahy.Add(souradnice);
            if (vysledek == VysledekZasahu.Potopena)
            {
                Zasahy.Clear();
                Console.WriteLine("Vaše loď " + Pole[X, Y].Lod.Nazev + " byla zničena");
                Console.WriteLine($"Počet zničených lodí: {PocetZnicenychLodi}/5");
                Thread.Sleep(4000);
            }
            pouzitelnaPole.Remove(souradnice);
        }
        private void UtokNajitaLod()
        {
            //Od nejmenších po největší
            Zasahy.Sort();

            (int X, int Y) souradnice1 = Zasahy[0];
            (int X, int Y) souradnice2 = Zasahy[Zasahy.Count - 1];

            List<(int X, int Y)> mozneUtoky = new List<(int X, int Y)>();
            //Horizontální orientace
            if (souradnice1.Y - souradnice2.Y == 0)
            {
                int X1 = souradnice1.X;
                int X2 = souradnice2.X;

                int Y1 = souradnice1.Y;
                if (X1 - 1 >= 0 && pouzitelnaPole.Contains((X1 - 1, Y1)))
                {
                    mozneUtoky.Add((X1 - 1, Y1));
                }
                if (X2 + 1 <= 9 && pouzitelnaPole.Contains((X2 + 1, Y1)))
                {
                    mozneUtoky.Add((X2 + 1, Y1));
                }
            }
            //Vertikální orientace
            else
            {
                int Y1 = souradnice1.Y;
                int Y2 = souradnice2.Y;

                int X1 = souradnice1.X;
                if (Y1 - 1 >= 0 && pouzitelnaPole.Contains((X1, Y1 - 1)))
                {
                    mozneUtoky.Add((X1, Y1 - 1));
                }
                if (Y2 + 1 <= 9 && pouzitelnaPole.Contains((X1, Y2 + 1)))
                {
                    mozneUtoky.Add((X1, Y2 + 1));
                }
            }
            var souradnice = mozneUtoky[r.Next(mozneUtoky.Count)];
            int X = souradnice.X;
            int Y = souradnice.Y;

            VysledekZasahu vysledek = KontrolaZasahu(X, Y);
            if (vysledek == VysledekZasahu.Zasah) Zasahy.Add(souradnice);
            if (vysledek == VysledekZasahu.Potopena)
            {
                Zasahy.Clear();
                Console.WriteLine("Vaše loď " + Pole[X, Y].Lod.Nazev + " byla zničena");
                Console.WriteLine($"Počet zničených lodí: {PocetZnicenychLodi}/5");
                Thread.Sleep(4000);
            }
            pouzitelnaPole.Remove(souradnice);
        }
    }
}
