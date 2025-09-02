using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip_hra_v_konzoli
{
    public enum VysledekZasahu
    {
        Vedle, 
        Zasah,
        Potopena,
        Neplatne
    }
    class HerniLogika
    {
        HraciPole[,] poleNPC = new HraciPole[10,10];
        private int pocetLodiNPC = 0;
        private List<(int X, int Y)> Zasahy = new List<(int X, int Y)>();
        private List<(int X, int Y)> pouzitelnaPole = new List<(int X, int Y)>();

        HraciPole[,] poleHrac = new HraciPole[10, 10];
        private int pocetLodiHrac = 0;

        Random r = new Random();
        //Konstruktor
        public HerniLogika(HraciPole[,] poleNPC, HraciPole[,] poleHrac)
        {
            this.poleNPC = poleNPC;
            this.poleHrac = poleHrac;

            for (int y = 0; y < poleHrac.GetLength(1); y++)
            {
                for (int x = 0; x < poleHrac.GetLength(0); x++)
                {
                    if (poleHrac[x, y].Pouzito == false) pouzitelnaPole.Add((x, y));
                }
            }
        }
        //Metody
        public void ZobrazitPole(string nadpis, HraciPole[,] pole)
        {
            Console.WriteLine(nadpis);
            bool prvniIterace = true;
            for (int y = 0; y < pole.GetLength(1); y++)
            {
                if (!prvniIterace) Console.Write(y + " ");
                for (int x = 0; x < pole.GetLength(0); x++)
                {
                    if (prvniIterace)
                    {
                        Console.WriteLine("  0 1 2 3 4 5 6 7 8 9");
                        Console.Write("0 ");
                        prvniIterace = false;
                    }
                    Console.Write(pole[x, y].Znak + " ");
                }
                Console.WriteLine();
            }
        }
        //Umožňuje hráči útočit a potom zaútočí i NPC
        public void Utok((int X, int Y) souradnice)
        {
            int X = souradnice.X;
            int Y = souradnice.Y;

            if (X <= 9 && X >= 0 && Y <= 9 && Y >= 0)
            {
                VysledekZasahu vysledek = KontrolaZasahu(X, Y, poleNPC,ref pocetLodiHrac);
                if (vysledek == VysledekZasahu.Potopena)
                {
                    Console.WriteLine("Zničili jste loď " + poleNPC[X, Y].Lod.Nazev);
                    Console.WriteLine($"Počet zničených lodí: {pocetLodiHrac}/5");
                    Thread.Sleep(4000);
                }
                if (vysledek == VysledekZasahu.Neplatne)
                {
                    Console.WriteLine("Tyto souřadnice již byly použity, zadejte jiné!");
                    Thread.Sleep(3000);
                    return;
                }
            }
            else
            {
                Console.WriteLine("Zadané souřadnice jsou mimo rozsah hracího pole, zadejte jiné");
                return;
            }

            UtokNPC();
        }
        private void UtokNPC()
        {

            switch (Zasahy.Count)
            {
                case 0: 
                    UtokNpcRandom();
                    break;
                case 1:
                    UtokNpcSousedniSouradniceRandom();
                    break;
                case >= 2:
                    UtokNpcNajitaLod();
                    break;
                default:
                    Console.WriteLine("Něco se nepovedlo - Utok NPC");
                    Thread.Sleep(4000);
                    break;
            }

        }
        private void UtokNpcRandom()
        {
            (int X, int Y) souradnice = pouzitelnaPole[r.Next(pouzitelnaPole.Count)];

            int X = souradnice.X;
            int Y = souradnice.Y;

            VysledekZasahu vysledek = KontrolaZasahu(X, Y, poleHrac,ref pocetLodiNPC);
            if (vysledek == VysledekZasahu.Zasah) Zasahy.Add(souradnice);

            pouzitelnaPole.Remove(souradnice);
        }
        private void UtokNpcSousedniSouradniceRandom()
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

            VysledekZasahu vysledek = KontrolaZasahu(X, Y, poleHrac,ref pocetLodiNPC);
            if (vysledek == VysledekZasahu.Zasah) Zasahy.Add(souradnice);
            if (vysledek == VysledekZasahu.Potopena)
            {
                Zasahy.Clear();
                Console.WriteLine("Vaše loď " + poleHrac[X, Y].Lod.Nazev + " byla zničena");
                Console.WriteLine($"Počet zničených lodí: {pocetLodiNPC}/5");
                Thread.Sleep(4000);
            }
            pouzitelnaPole.Remove(souradnice);
        }
        private void UtokNpcNajitaLod()
        {
            //Od nejmenších po největší
            Zasahy.Sort();

            (int X, int Y) souradnice1 = Zasahy[0];
            (int X, int Y) souradnice2 = Zasahy[Zasahy.Count-1];

            List<(int X, int Y)> mozneUtoky = new List<(int X, int Y)>();
            //Horizontální orientace
            if(souradnice1.Y - souradnice2.Y == 0)
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

            VysledekZasahu vysledek = KontrolaZasahu(X, Y, poleHrac,ref pocetLodiNPC);
            if (vysledek == VysledekZasahu.Zasah) Zasahy.Add(souradnice);
            if (vysledek == VysledekZasahu.Potopena)
            {
                Zasahy.Clear();
                Console.WriteLine("Vaše loď " + poleHrac[X, Y].Lod.Nazev + " byla zničena");
                Console.WriteLine($"Počet zničených lodí: {pocetLodiNPC}/5");
                Thread.Sleep(4000);
            }
            pouzitelnaPole.Remove(souradnice);
        }
        private VysledekZasahu KontrolaZasahu(int X, int Y, HraciPole[,] pole,ref int pocetLodi)
        {
            VysledekZasahu vysledek = VysledekZasahu.Vedle;

            (int X, int Y) souradnice = (X, Y);
            if (!pole[X, Y].Pouzito)
            {
                if (pole[X, Y].JeLod)
                {
                    pole[X, Y].Znak = "X";
                    pole[X, Y].Lod.Zasahy++;

                    vysledek = VysledekZasahu.Zasah;

                    if (pole[X, Y].Lod.Zasahy >= pole[X, Y].Lod.Delka)
                    {
                        pocetLodi++;

                        foreach (var s in pole[X, Y].Lod.SousedniSouradnice)
                        {
                            int x = s.X;
                            int y = s.Y;

                            pole[x, y].Znak = ".";
                            pole[x, y].Pouzito = true;
                        }
                        vysledek = VysledekZasahu.Potopena;
                    }
                }
                else
                {
                    pole[X, Y].Znak = ".";
                }
                pole[X, Y].Pouzito = true;
            }
            else
            {
                vysledek = VysledekZasahu.Neplatne;
            }
            return vysledek;
        }
        public bool Vyhra()
        {
            if(pocetLodiNPC == 5)
            {
                Console.WriteLine("Prohráli jste");
                return true;
            }
            if(pocetLodiHrac == 5)
            {
                Console.WriteLine("Vyhráli jste");
                return true;
            }
            return false;
        }
    }
}
