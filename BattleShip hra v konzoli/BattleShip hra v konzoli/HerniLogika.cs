using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip_hra_v_konzoli
{
    class HerniLogika
    {
        HraciPole[,] poleNPC = new HraciPole[10,10];
        private int pocetLodiNPC = 0;
        private List<(int X, int Y)> Zasahy = new List<(int X, int Y)>();
        private List<(int X, int Y)> pouzitelnaPole = new List<(int X, int Y)>();

        HraciPole[,] poleHrac = new HraciPole[10, 10];
        private int pocetLodiHrac = 0;

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

            if (X <= 9 && X >= 0 && Y <= 9 && X >= 0)
            {
                if (poleHrac[X, Y].Pouzito == false)
                {
                    poleHrac[X, Y].Pouzito = true;
                    if (poleNPC[X, Y].JeLod)
                    {
                        poleNPC[X, Y].Znak = "X";
                        poleNPC[X, Y].Lod.Zasahy++;

                        if (poleNPC[X, Y].Lod.Zasahy >= poleNPC[X, Y].Lod.Delka)
                        {
                            Console.WriteLine("Zničili jste " + poleNPC[X, Y].Lod.Nazev);
                            pocetLodiHrac++;
                            Console.WriteLine($"Počet zničených lodí: {pocetLodiHrac}/5");
                            Thread.Sleep(4000);
                        }
                    }
                    else
                    {
                        poleNPC[X, Y].Znak = ".";
                    }
                }
                else
                {
                    Console.WriteLine("Zadejte jiné souřadnice, tyto už byly použity");
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
                case 2:

                    break;
                default: Console.WriteLine("Něco se nepovedlo - Utok NPC");
                    break;
            }

        }
        private void UtokNpcRandom()
        {
            Random r = new Random();
            (int X, int Y) souradnice = pouzitelnaPole[r.Next(pouzitelnaPole.Count)];

            int X = souradnice.X;
            int Y = souradnice.Y;

            KontrolaZasahu(X, Y);
        }
        private void UtokNpcSousedniSouradniceRandom()
        {
            Random r = new Random();

            (int X, int Y) souradnice = Zasahy[0];

            int X = souradnice.X;
            int Y = souradnice.Y;

            List<(int X, int Y)> utokNaSouradnice = new List<(int X, int Y)>();

            List<(int X, int Y)> mozneSouradnice = new List<(int X, int Y)>();

            mozneSouradnice.Add((X - 1, Y));
            mozneSouradnice.Add((X + 1, Y));
            mozneSouradnice.Add((X, Y - 1));
            mozneSouradnice.Add((X, Y + 1));

            foreach(var s in mozneSouradnice)
            {
                if(pouzitelnaPole.Contains(s) && (s.X >= 0 && s.X <= 9) && (s.Y >= 0 && s.Y <= 9))
                {
                    utokNaSouradnice.Add(s);
                }
            }

            souradnice = utokNaSouradnice[r.Next(utokNaSouradnice.Count)];

            X = souradnice.X;
            Y = souradnice.Y;

            KontrolaZasahu(X, Y);
        }
        private void KontrolaZasahu(int X, int Y)
        {
            (int X, int Y) souradnice = (X, Y);
            if (poleHrac[X, Y].JeLod)
            {
                poleHrac[X, Y].Znak = "X";
                poleHrac[X, Y].Lod.Zasahy++;

                Zasahy.Add(souradnice);

                if (poleHrac[X, Y].Lod.Zasahy >= poleHrac[X, Y].Lod.Delka)
                {
                    Console.WriteLine("Vaše loď " + poleHrac[X, Y].Lod.Nazev + "byla zničena");
                    pocetLodiNPC++;
                    Console.WriteLine($"Počet zničených lodí: {pocetLodiNPC}/5");
                    Thread.Sleep(4000);
                    Zasahy.Clear();
                }
            }
            else
            {
                poleHrac[X, Y].Znak = ".";
            }
            poleHrac[X, Y].Pouzito = true;
            pouzitelnaPole.Remove(souradnice);
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
