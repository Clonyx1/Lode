using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip_hra_v_konzoli
{
    public enum StavHry
    {
        VyhraHrac,
        VyhraAI,
        HraProbiha
    }
    class HerniLogika
    {
        NahodnaHra novaNahodnaHra = new NahodnaHra();
        CustomHra novaCustomHra = new();
        public bool NahodnaHra;
        public Hrac hrac;
        public AI ai;
        public StavHry StavHry
        {
            get
            {
                if (ai.PocetZnicenychLodi == 5)
                {
                    return BattleShip_hra_v_konzoli.StavHry.VyhraAI;
                }
                if (hrac.PocetZnicenychLodi == 5)
                {
                    return BattleShip_hra_v_konzoli.StavHry.VyhraHrac;
                }
                return BattleShip_hra_v_konzoli.StavHry.HraProbiha;
            }
        }
        //Konstruktor
        public HerniLogika(bool nahodnaHra)
        {
            NahodnaHra = nahodnaHra;
            ai = new AI(novaNahodnaHra.NovaHra());
            if (NahodnaHra)
            {
                hrac = new Hrac(novaNahodnaHra.NovaHra());

                hrac.zbyvajiciLode = novaNahodnaHra.Lode;
            }
            else
            {
                hrac = new(novaCustomHra.NovaHra());
                hrac.zbyvajiciLode = novaCustomHra.Lode;
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

            if (X <= 9 && X >= 0 && Y <= 9 && Y >= 0 && !ai.Pole[X, Y].Pouzito)
            {
                hrac.Utok(ai, X, Y);
            }
            else if (ai.Pole[X, Y].Pouzito)
            {
                Console.WriteLine("Tyto souřadnice již byly použity, zadejte jiné!");
                Thread.Sleep(2000);
                return;
            }
            else
            {
                Console.WriteLine("Zadané souřadnice jsou mimo rozsah hracího pole, zadejte jiné");
                Thread.Sleep(2000);
                return;
            }
            ai.Utok(hrac);
        }
    }
}
