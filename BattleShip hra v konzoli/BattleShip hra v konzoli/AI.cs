using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;
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
        //Vybere nejpravděpodobnější souřadnice a zaútočí
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
                var top5 = heatMap.OrderByDescending(x => x.Value).Take(5).ToList();
                souradnice = VazenyVyberSouradnice(top5);
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

            List<(int X, int Y)> mozneSouradnice = NajitMozneSouradnice(X, Y, hrac);

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
        private List<(int X, int Y)> NajitMozneSouradnice(int X, int Y, Hrac hrac)
        {
            int minDelka = hrac.zbyvajiciLode.Min(lod => lod.Delka);
            //Najde souřadnice v okolí zásahu
            Dictionary<(int X, int Y), Orientace> okolniSouradnice = ZiskejOkolniPole(X, Y, minDelka, hrac);
            //Vybere ty souřadnice, na kterých by se mohla nacházet loď
            List<(int X, int Y)> mozneSouradnice = VyberSmysluplnychSouradnic(X, Y, minDelka, okolniSouradnice, hrac);

            return mozneSouradnice;
        }
        //Najde souřadnice sousedící s Zasahy[0]
        private Dictionary<(int X, int Y), Orientace> ZiskejOkolniPole(int X, int Y, int minDelka, Hrac hrac)
        {
            Dictionary<(int X, int Y), Orientace> okolniSouradnice = new(); 

            (int posunX, int posunY, Orientace orientace)[] smery =
{
                (-1, 0, Orientace.Horizontalni), //doleva
                (1, 0, Orientace.Horizontalni), //doprava
                (0, 1, Orientace.Vertikalni), //dolu
                (0, -1, Orientace.Vertikalni) //nahoru
            };

            foreach (var (posunX, posunY, orientace) in smery)
            {
                for (int i = 1; i < minDelka; i++)
                {
                    var souradniceX = X + posunX * i;
                    var souradniceY = Y + posunY * i;

                    if (souradniceX < 0 || souradniceX > 9 || souradniceY < 0 || souradniceY > 9) break;

                    if (hrac.Pole[souradniceX, souradniceY].Pouzito) break;

                    okolniSouradnice.Add((souradniceX, souradniceY), orientace);
                }
            }
            return okolniSouradnice;
        }
        //Vyhodnotí souřadnice, na kterých by se mohla nacházet loď
        private List<(int X, int Y)> VyberSmysluplnychSouradnic(int X, int Y, int minDelka, Dictionary<(int X, int Y), Orientace> okolniSouradnice, Hrac hrac)
        {
            List<(int X, int Y)> mozneSouradnice = new();

            int pocetHorizontalnich = okolniSouradnice.Count(s => s.Value == Orientace.Horizontalni);
            int pocetVertikalnich = okolniSouradnice.Count(s => s.Value == Orientace.Vertikalni);

            if (pocetHorizontalnich >= minDelka - 1)
            {
                if (X + 1 <= 9 && !hrac.Pole[X + 1, Y].Pouzito) mozneSouradnice.Add((X + 1, Y));
                if (X - 1 >= 0 && !hrac.Pole[X - 1, Y].Pouzito) mozneSouradnice.Add((X - 1, Y));
            }
            if (pocetVertikalnich >= minDelka - 1)
            {
                if (Y + 1 <= 9 && !hrac.Pole[X, Y + 1].Pouzito) mozneSouradnice.Add((X, Y + 1));
                if (Y - 1 >= 0 && !hrac.Pole[X, Y - 1].Pouzito) mozneSouradnice.Add((X, Y - 1));
            }

            return mozneSouradnice;
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
