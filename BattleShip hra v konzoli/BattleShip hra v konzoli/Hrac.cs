using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    public class Hrac
    {
        public HraciPole[,] Pole = new HraciPole[10, 10];
        public int PocetZnicenychLodi {  get; set; }
        public Random r = new Random();

        //Konstruktor
        public Hrac(HraciPole[,] pole)
        {
            Pole = pole;
        }


        //Metody
        public void Utok(int X, int Y)
        {
            if (X <= 9 && X >= 0 && Y <= 9 && Y >= 0)
            {
                VysledekZasahu vysledek = KontrolaZasahu(X, Y);
                if (vysledek == VysledekZasahu.Potopena)
                {
                    Console.WriteLine("Zničili jste loď " + Pole[X, Y].Lod.Nazev);
                    Console.WriteLine($"Počet zničených lodí: {PocetZnicenychLodi}/5");
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
        }
        public VysledekZasahu KontrolaZasahu(int X, int Y)
        {
            VysledekZasahu vysledek = VysledekZasahu.Vedle;

            (int X, int Y) souradnice = (X, Y);
            if (!Pole[X, Y].Pouzito)
            {
                if (Pole[X, Y].JeLod)
                {
                    Pole[X, Y].Znak = "X";
                    Pole[X, Y].Lod.Zasahy++;

                    vysledek = VysledekZasahu.Zasah;

                    if (Pole[X, Y].Lod.Zasahy >= Pole[X, Y].Lod.Delka)
                    {
                        PocetZnicenychLodi++;

                        foreach (var s in Pole[X, Y].Lod.SousedniSouradnice)
                        {
                            int x = s.X;
                            int y = s.Y;

                            Pole[x, y].Znak = ".";
                            Pole[x, y].Pouzito = true;
                        }
                        vysledek = VysledekZasahu.Potopena;
                    }
                }
                else
                {
                    Pole[X, Y].Znak = ".";
                }
                Pole[X, Y].Pouzito = true;
            }
            else
            {
                vysledek = VysledekZasahu.Neplatne;
            }
            return vysledek;
        }
    }
}
