using BattleShip_hra_v_konzoli;

Lode novaHra = new Lode();

string[,] hraciPole = novaHra.NovaHra();

for(int i = 0; i < hraciPole.GetLength(0); i++)
{
    for(int j = 0; j< hraciPole.GetLength(1); j++)
    {
        if (string.IsNullOrEmpty(hraciPole[i,j]))hraciPole[i, j] = "O";
        Console.Write(hraciPole[i, j] + " ");
    }
    Console.WriteLine();
}