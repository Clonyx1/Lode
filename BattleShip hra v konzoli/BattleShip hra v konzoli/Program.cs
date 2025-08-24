using BattleShip_hra_v_konzoli;

Lode novaHra = new Lode();

HraciPole[,] hraciPoleNPC = novaHra.NovaHra();
HraciPole[,] hraciPoleHrac = novaHra.NovaHra();

HerniLogika mometalniHra = new HerniLogika(hraciPoleNPC, hraciPoleHrac);

string[,] viditelnePoleNPC = new string[10, 10];

bool vyhra = false;
while (!vyhra)
{
    Console.Clear();
    mometalniHra.ZobrazitPole("Pole nepřítele", hraciPoleNPC);
    mometalniHra.ZobrazitPole("Vaše pole", hraciPoleHrac);

    Console.WriteLine("Napište souřadnice, na které chcete útočit ve formátu: X, Y");
    string souradnice = Console.ReadLine();
    string[] cislice = souradnice.Split(",");
    try
    {
        (int X, int Y) bodZasahu = (Convert.ToInt32(cislice[0]), Convert.ToInt32(cislice[1]));
        mometalniHra.Utok(bodZasahu);
    }
    catch
    {
        Console.Clear();
        Console.WriteLine("Zadejte validní souřadnice");
    }
    vyhra = mometalniHra.Vyhra();
}