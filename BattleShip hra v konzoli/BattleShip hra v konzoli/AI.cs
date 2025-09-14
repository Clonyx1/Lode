using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip_hra_v_konzoli
{
    public class AI : Hrac
    {
        private List<(int X, int Y)> Zasahy = new List<(int X, int Y)>();
        public List<(int X, int Y)> pouzitelnaPole = new List<(int X, int Y)>();
        private Dictionary<(int X, int Y), int> heatMap = new();
        //Konstruktor
        public AI(HraciPole[,] pole) : base(pole) 
        {
        
        }
        //Metody
        public void Utok(Hrac hrac)
        {

            switch (Zasahy.Count)
            {
                case 0:
                    UtokNenajitaLod(hrac);
                    break;
                case 1:
                    UtokSousedniSouradniceRandom(hrac);
                    break;
                case >= 2:
                    UtokNajitaLod(hrac);
                    break;
                default:
                    Console.WriteLine("Něco se nepovedlo - Utok NPC");
                    Thread.Sleep(4000);
                    break;
            }

        }
        private void VyplneniHeatMapy(Hrac hrac)
        {
            for(int y = 0; y < hrac.Pole.GetLength(1); y++)
            {
                for(int x = 0; x < hrac.Pole.GetLength(0); x++)
                {
                    if (!hrac.Pole[x, y].Pouzito) ZkouskaLodi((x, y), hrac, hrac.zbyvajiciLode.Max(lod => lod.Delka));
                }
            }
        }
        private void ZkouskaLodi((int X, int Y) souradnice, Hrac hrac, int maxDelka)
        { 
            int X = souradnice.X;
            int Y = souradnice.Y;

            List<(int X, int Y)> poleZaSebouX = new();
            List<(int X, int Y)> poleZaSebouY = new();
            //Horizontální orientace
            for(int i = 0; i < maxDelka; i++)
            {
                if (X + i >= hrac.Pole.GetLength(0)) break;
                if (!hrac.Pole[X + i, Y].Pouzito)
                {
                    poleZaSebouX.Add((X + i, Y));
                }
                else if (hrac.Pole[X + i, Y].Pouzito) break;
            }
            //Vertikální orientace
            for(int j = 0; j < maxDelka; j++)
            {
                if (Y + j >= hrac.Pole.GetLength(1)) break;
                if (!hrac.Pole[X, Y + j].Pouzito)
                {
                    poleZaSebouY.Add((X, Y+j));
                }
                else if (hrac.Pole[X, Y + j].Pouzito) break;
            }

            //Doplnění hodnot do heatMapy
            PridaniVahyPoli(poleZaSebouX, hrac);
            PridaniVahyPoli(poleZaSebouY, hrac);
        }
        private void PridaniVahyPoli(List<(int X, int Y)> poleZaSebou, Hrac hrac)
        {
            poleZaSebou.Sort();
            foreach (var lod in hrac.zbyvajiciLode)
            {
                if(lod.Delka <= poleZaSebou.Count)
                {
                    for (int i = 0; i < lod.Delka; i++)
                    {
                        (int X, int Y) souradnice = poleZaSebou[i];
                        int X = souradnice.X;
                        int Y = souradnice.Y;

                        if (heatMap.ContainsKey((X, Y))) heatMap[(X, Y)]++;
                        else heatMap.Add((X, Y), 1);
                    }
                } 
            }
        }
        private void UtokNenajitaLod(Hrac hrac)
        {
            VyplneniHeatMapy(hrac);

            (int X, int Y) souradnice = SouradnicePodleFazeHry(PocetZnicenychLodi);

            int X = souradnice.X;
            int Y = souradnice.Y;

            VysledekZasahu vysledek = KontrolaZasahu(hrac, X, Y);
            if (vysledek == VysledekZasahu.Zasah) Zasahy.Add(souradnice);

            heatMap.Remove(souradnice);
            pouzitelnaPole.Remove(souradnice);
        }
        private (int X, int Y) SouradnicePodleFazeHry(int fazeHry)
        {
            (int X, int Y) souradnice = heatMap.OrderByDescending(x => x.Value).First().Key;
            //Začátek hry
            if (fazeHry < 1)
            {
                var top10 = heatMap.OrderByDescending(x => x.Value).Take(10).ToList();
                souradnice = VazenyVyberSouradnice(top10);
            }
            //Přechod do středu hry
            if (fazeHry == 2)
            {
                var top3 = heatMap.OrderByDescending(x => x.Value).Take(3).ToList();
                souradnice = VazenyVyberSouradnice(top3);
            }

            return souradnice;
        }
        private (int X, int Y) VazenyVyberSouradnice(List<KeyValuePair<(int X, int Y), int>> topN)
        {
            (int X, int Y) souradnice = (0, 0);

            int celkovaVaha = 0;
            foreach(var kvp in topN)
            {
                celkovaVaha += kvp.Value;
            }

            double nahodne = r.NextDouble() * celkovaVaha;

            foreach(var kvp in topN)
            {
                nahodne -= kvp.Value;

                if(nahodne < 0)
                {
                    souradnice = kvp.Key;
                    break;
                }
            }

            return souradnice;
        }
        private void UtokSousedniSouradniceRandom(Hrac hrac)
        {
            //Souřadnice, na které AI našlo loď
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

            VysledekZasahu vysledek = KontrolaZasahu(hrac, X, Y);
            if (vysledek == VysledekZasahu.Zasah) Zasahy.Add(souradnice);
            if (vysledek == VysledekZasahu.Potopena)
            {
                Zasahy.Clear();
                Console.WriteLine("Vaše loď " + hrac.Pole[X, Y].Lod.Nazev + " byla zničena");
                Console.WriteLine($"Počet zničených lodí: {PocetZnicenychLodi}/5");
                Thread.Sleep(4000);
            }
            pouzitelnaPole.Remove(souradnice);
        }
        private void UtokNajitaLod(Hrac hrac)
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

            VysledekZasahu vysledek = KontrolaZasahu(hrac, X, Y);
            if (vysledek == VysledekZasahu.Zasah) Zasahy.Add(souradnice);
            if (vysledek == VysledekZasahu.Potopena)
            {
                Zasahy.Clear();
                Console.WriteLine("Vaše loď " + hrac.Pole[X, Y].Lod.Nazev + " byla zničena");
                Console.WriteLine($"Počet zničených lodí: {PocetZnicenychLodi}/5");
                Thread.Sleep(4000);
            }
            pouzitelnaPole.Remove(souradnice);
        }
    }
}
