using System;

class Program
{
    static void Main()
    {
        const int Max = int.MaxValue;

        Console.WriteLine("• SumFor:");
        Ascendente(SumFor, "SumFor", Max);
        Descendente(SumFor, "SumFor", Max);

        Console.WriteLine("\n• SumIte:");
        Ascendente(SumIte, "SumIte", Max);
        Descendente(SumIte, "SumIte", Max);
    }

    static int SumFor(int n)
    {
        return n * (n + 1) / 2;
    }

    static int SumIte(int n)
    {
        int sum = 0;
        for (int i = 1; i <= n; i++)
        {
            sum += i;
        }
        return sum;
    }

    static void Ascendente(Func<int, int> sumMethod, string methodName, int max)
{
    int lastValidN = 0;
    int lastValidSum = 0;
    int prevSum = 0;

    for (int n = 1; n <= max; n++)
    {
        int sum = sumMethod(n);
        if (n > 1 && sum < prevSum)
        {
            // Detecta overflow cuando el resultado disminuye inesperadamente
            break;
        }
        lastValidN = n;
        lastValidSum = sum;
        prevSum = sum;
    }

    Console.WriteLine($"\t◦ From 1 to Max → n: {lastValidN} → sum: {lastValidSum}");
}

    static void Descendente(Func<int, int> sumMethod, string methodName, int max)
{
    int step = 10000; // Puedes ajustar este valor para acelerar la búsqueda
    for (int n = max; n >= 1; n -= step)
    {
        int sum = sumMethod(n);
        if (sum > 0)
        {
            Console.WriteLine($"\t◦ From Max to 1 → n: {n} → sum: {sum}");
            return;
        }
    }

    Console.WriteLine($"\t◦ From Max to 1 → No se encontró un valor válido.");
}

}


