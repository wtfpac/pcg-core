using System;
using System.Collections.Generic;
using PCG.Generation;

class Program
{
    static void Main()
    {
        int largura = 40;
        int altura = 24;
        int seed = 42;

        //minroomsize = 6, maxdepth=4 (ate 16 comodos)
        BspGenerator gerador = new BspGenerator(6, 4, seed);
        BspNode raiz = gerador.Generate(new Area(0, 0, largura, altura));

        List<BspNode> comodos = BspGenerator.CollectLeaves(raiz);

        Console.WriteLine($"Seed: {seed} | Comodos gerados: {comodos.Count}");
        Console.WriteLine();

        DesenharPlanta(comodos, largura, altura);
    }

    // monta uma grade de caracteres e imprime a planta baixa
    static void DesenharPlanta(List<BspNode> comodos, int largura, int altura)
    {
        char[,] grade = new char[altura, largura];

        for (int y = 0; y < altura; y++)
            for (int x = 0; x < largura; x++)
                grade[y, x] = ' ';

        // Cada cômodo ganha um caractere próprio pra ficar identificável.
        const string simbolos = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        for (int i = 0; i < comodos.Count; i++)
        {
            Area a = comodos[i].Area;
            char id = simbolos[i % simbolos.Length];

            for (int y = a.Y; y < a.Bottom; y++)
            {
                for (int x = a.X; x < a.Right; x++)
                {
                    bool naBorda = (x == a.X || x == a.Right - 1 ||
                                    y == a.Y || y == a.Bottom - 1);

                    grade[y, x] = naBorda ? '#' : id;
                }
            }
        }

        // Imprime cada célula 2x na horizontal pra compensar o formato do caractere.
        for (int y = 0; y < altura; y++)
        {
            for (int x = 0; x < largura; x++)
            {
                Console.Write(grade[y, x]);
                Console.Write(grade[y, x]);
            }
            Console.WriteLine();
        }
    }
}