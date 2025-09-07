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
        Lode novaHra = new Lode();
        CustomHra novaCustomHra = new();
        public bool NahodnaHra;
        public Hrac hrac;
        public AI ai;
        //Konstruktor
        public HerniLogika(bool nahodnaHra)
        {
            NahodnaHra = nahodnaHra;
            ai = new AI(novaHra.NovaHra());
            if (NahodnaHra)
            {
                hrac = new Hrac(novaHra.NovaHra());
            }
            else
            {
                hrac = new(novaCustomHra.NovaHra());
            }
            for (int y = 0; y < hrac.Pole.GetLength(1); y++)
            {
                for (int x = 0; x < hrac.Pole.GetLength(0); x++)
                {
                    if (hrac.Pole[x, y].Pouzito == false) ai.pouzitelnaPole.Add((x, y));
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
                hrac.Utok(ai, X, Y);
            }
            else
            {
                Console.WriteLine("Zadané souřadnice jsou mimo rozsah hracího pole, zadejte jiné");
                return;
            }

            ai.Utok(hrac);
        }
        public bool Vyhra()
        {
            if (ai.PocetZnicenychLodi == 5)
            {
                Console.WriteLine("Prohráli jste");
                return true;
            }
            if (hrac.PocetZnicenychLodi == 5)
            {
                Console.WriteLine("Vyhráli jste");
                return true;
            }
            return false;
        }
    }
}
