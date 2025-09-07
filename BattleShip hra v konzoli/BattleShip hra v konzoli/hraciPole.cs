using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip_hra_v_konzoli
{
    public class HraciPole
    {
        public string Znak {  get; set; }
        public bool JeLod => Lod != null;
        public bool Pouzito { get; set; }
        

        public Lod? Lod { get; set; }
        //Konstruktor
        public HraciPole(string znak, Lod? lod, bool pouzito)
        {
            Znak = znak;
            Lod = lod;
            Pouzito = pouzito;
        }
    }
}
