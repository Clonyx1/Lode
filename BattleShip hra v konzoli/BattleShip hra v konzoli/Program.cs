using BattleShip_hra_v_konzoli;

Lode novaHra = new Lode();

string[,] hraciPole = novaHra.NovaHra();

for(int i = 0; i < hraciPole.GetLength(0); i++)
{
    for(int j = 0; j< hraciPole.GetLength(1); j++)
    {
        if (string.IsNullOrEmpty(hraciPole[j,i]))hraciPole[j, i] = "O";
        Console.Write(hraciPole[j, i] + " ");
    }
    Console.WriteLine();
}