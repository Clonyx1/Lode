using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip_hra_v_konzoli
{
    class HerniLogika
    {
        bool[,] hraciPoleNPC = new bool[10, 10];
        bool[,] pouzitePoleNPC = new bool[10, 10];
        string[,] viditelnePoleNPC = new string[10, 10];

        bool[,] hraciPoleHrac = new bool[10, 10];
        bool[,] pouzitePoleHrac = new bool[10, 10];
        string[,] viditelnePoleHrac = new string[10, 10];

        //Konstruktor
        public HerniLogika(bool[,] poleNPC, bool[,] poleHrac)
        {
            hraciPoleNPC = poleNPC;
            hraciPoleHrac = poleHrac;

            for (int y = 0; y < hraciPoleNPC.GetLength(1); y++)
            {
                for (int x = 0; x < hraciPoleNPC.GetLength(0); x++)
                {
                    viditelnePoleNPC[x, y] = "O";
                    viditelnePoleHrac[x, y] = "O";
                }
            }
        }
        //Metody
        public void ZobrazitPole()
        {
            Console.WriteLine("Herní pole NPC");
            bool prvniIterace = true;
            for (int y = 0; y < hraciPoleNPC.GetLength(1); y++)
            {
                if(!prvniIterace) Console.Write(y + " ");
                for (int x = 0; x < hraciPoleNPC.GetLength(0); x++)
                {
                    if(prvniIterace)
                    {
                        Console.WriteLine("  0 1 2 3 4 5 6 7 8 9");
                        Console.Write("0 ");
                        prvniIterace = false;
                    }
                    Console.Write(viditelnePoleNPC[x, y] + " ");
                }
                Console.WriteLine();
            }

            Console.WriteLine("Herní pole hráč");
            for (int y = 0; y < hraciPoleHrac.GetLength(1); y++)
            {
                for (int x = 0; x < hraciPoleHrac.GetLength(0); x++)
                {
                    Console.Write(viditelnePoleHrac[x, y] + " ");
                }
                Console.WriteLine();
            }
        }
        //Přijme souřadnice zadané uživatelem a automaticky zaútočí
        public void Zasah((int X, int Y) souradnice)
        {
            int X = souradnice.X;
            int Y = souradnice.Y;

            if (X <= 9 && X >= 0 && Y <= 9 && X >= 0)
            {
                if (pouzitePoleHrac[X, Y] == false)
                {
                    pouzitePoleHrac[X, Y] = true;
                    if (hraciPoleNPC[X, Y] == true)
                    {
                        viditelnePoleNPC[X, Y] = "X";
                    }
                    else
                    {
                        viditelnePoleNPC[X, Y] = ".";
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
            Console.Clear();
        }
    }
}
