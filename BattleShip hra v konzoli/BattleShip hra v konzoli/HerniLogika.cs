using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip_hra_v_konzoli
{
    class HerniLogika
    {
        HraciPole[,] poleNPC = new HraciPole[10,10];
        int pocetLodiNPC = 0;

        HraciPole[,] poleHrac = new HraciPole[10, 10];
        int pocetLodiHrac = 0;

        //Konstruktor
        public HerniLogika(HraciPole[,] poleNPC, HraciPole[,] poleHrac)
        {
            this.poleNPC = poleNPC;
            this.poleHrac = poleHrac;
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
        //Přijme souřadnice zadané uživatelem a automaticky zaútočí
        public void Zasah((int X, int Y) souradnice)
        {
            UtokHrac(souradnice);
        }
        private void UtokHrac((int X, int Y) souradnice)
        {
            int X = souradnice.X;
            int Y = souradnice.Y;

            if (X <= 9 && X >= 0 && Y <= 9 && X >= 0)
            {
                if (poleHrac[X, Y].Pouzito == false)
                {
                    poleHrac[X, Y].Pouzito = true;
                    if (poleNPC[X, Y].JeLod == true)
                    {
                        poleNPC[X, Y].Znak = "X";
                        poleNPC[X, Y].Lod.Zasahy += 1;

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
