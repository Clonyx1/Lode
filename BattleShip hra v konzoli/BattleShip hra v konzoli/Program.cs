using BattleShip_hra_v_konzoli;

Lode novaHra = new Lode();

//TOHLE BUDE CHTÍT PŘEDĚLAT TAK, ABY TO MĚLO (BOOL JeTadyLod, BOOL zautocilJsemSem)
bool[,] hraciPoleNPC = novaHra.NovaHra();
bool[,] hraciPoleHrac = novaHra.NovaHra();

HashSet<(int X, int Y)> lode = new HashSet<(int X, int Y)>();
lode.Add(novaHra.letadlova.PocatecniPozice);
lode.Add(novaHra.kriznik.PocatecniPozice);
lode.Add(novaHra.ponorka.PocatecniPozice);
lode.Add(novaHra.bitevni.PocatecniPozice);
lode.Add(novaHra.clun.PocatecniPozice);
HerniLogika mometalniHra = new HerniLogika(hraciPoleNPC, hraciPoleHrac);

string[,] viditelnePoleNPC = new string[10, 10];

//Console.WriteLine(string.Join(",", lode));
while (true)
{
    mometalniHra.ZobrazitPole();
    Console.WriteLine("Napište souřadnice, na které chcete útočit ve formátu: X, Y");
    string souradnice = Console.ReadLine();
    string[] cislice = souradnice.Split(",");
    try
    {
        (int X, int Y) bodZasahu = (Convert.ToInt32(cislice[0]), Convert.ToInt32(cislice[1]));
        mometalniHra.Zasah(bodZasahu);
    }
    catch
    {
        Console.Clear();
        Console.WriteLine("Zadejte validní souřadnice");
    }
}