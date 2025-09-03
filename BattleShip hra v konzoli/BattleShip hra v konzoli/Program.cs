using BattleShip_hra_v_konzoli;

HerniLogika momentalniHra = new HerniLogika();

string[,] viditelnaPoleAI = new string[10, 10];

bool vyhra = false;
while (!vyhra)
{
    Console.Clear();
    momentalniHra.ZobrazitPole("Pole nepřítele", momentalniHra.hrac.Pole);
    momentalniHra.ZobrazitPole("Vaše pole", momentalniHra.ai.Pole);

    Console.WriteLine("Napište souřadnice, na které chcete útočit ve formátu: X, Y nebo X Y");
    string souradnice = Console.ReadLine();
    souradnice = souradnice.Trim();

    string[] cislice = new string[2];
    try
    {
        if (souradnice.Contains(','))
        {
            cislice = souradnice.Split(",");
        }
        else
        {
            cislice = souradnice.Split(" ");
        }
        (int X, int Y) bodZasahu = (Convert.ToInt32(cislice[0]), Convert.ToInt32(cislice[1]));
        momentalniHra.Utok(bodZasahu);
    }
    catch
    {
        Console.Clear();
        Console.WriteLine("Zadejte validní souřadnice!");
        Thread.Sleep(4000);
    }

    vyhra = momentalniHra.Vyhra();
}