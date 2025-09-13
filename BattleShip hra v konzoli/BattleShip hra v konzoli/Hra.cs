using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip_hra_v_konzoli
{
    public abstract class Hra
    {
        protected HashSet<(int, int)> zabraneSouradnice = new HashSet<(int, int)>();
        public Lod[] Lode = new Lod[5];

        //Metody
        protected HraciPole[,] InicializaceHracihoPole(Lod[] lode)
        {
            HraciPole[,] hraciPole = new HraciPole[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    hraciPole[i, j] = new HraciPole("~", null, false);
                }
            }

            foreach (var lod in lode) NaplneniHracihoPole(hraciPole, lod);

            return hraciPole;
        }
        protected void NaplneniHracihoPole(HraciPole[,] hraciPole, Lod lod)
        {
            IEnumerable<(int X, int Y)> poziceLodi = lod.SouradniceLode();
            foreach (var pozice in poziceLodi)
            {
                int X = pozice.X;
                int Y = pozice.Y;

                hraciPole[X, Y] = new HraciPole("~", lod, false);
            }
        }
        protected void ZabratSouradnice(Lod lod)
        {
            int pocatecniX = lod.PocatecniPozice.X;
            int pocatecniY = lod.PocatecniPozice.Y;
            if (lod.Orientace == Orientace.Vertikalni)
            {
                if (pocatecniY != 0) zabraneSouradnice.Add((pocatecniX, pocatecniY - 1));
                if (pocatecniY + lod.Delka - 1 < 9) zabraneSouradnice.Add((pocatecniX, pocatecniY + lod.Delka));

                foreach (var souradnice in lod.SouradniceLode())
                {
                    int X = souradnice.X;
                    int Y = souradnice.Y;

                    zabraneSouradnice.Add(souradnice);
                    zabraneSouradnice.Add((X + 1, Y));
                    zabraneSouradnice.Add((X - 1, Y));
                }
            }
            else
            {
                if (pocatecniX != 0) zabraneSouradnice.Add((pocatecniX - 1, pocatecniY));
                if (pocatecniX + lod.Delka - 1 < 9) zabraneSouradnice.Add((pocatecniX + lod.Delka, pocatecniY));

                foreach (var souradnice in lod.SouradniceLode())
                {
                    int X = souradnice.X;
                    int Y = souradnice.Y;

                    zabraneSouradnice.Add(souradnice);
                    zabraneSouradnice.Add((X, Y + 1));
                    zabraneSouradnice.Add((X, Y - 1));
                }
            }
        }
    }
}
