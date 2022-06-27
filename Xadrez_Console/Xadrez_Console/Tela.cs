using System;
using System.Collections.Generic;
using tabuleiro;
using Xadrez;

namespace Xadrez_Console
{
    class Tela
    {
        public static void ImprimirPartida(PartidaXadrez partida)
        {
            PrintTabuleiro(partida.Tab);
            Console.WriteLine();
            ImprimirPecasCapturadas(partida);
            Console.WriteLine();
            Console.WriteLine("Turno: " + partida.Turno);

            if (!partida.Terminada) 
            {
                Console.WriteLine("Aguardando jogada: " + partida.JogadorAtual);
                Console.WriteLine();
                if (partida.Xeque)
                {
                    Console.WriteLine(" -----> XEQUE!! <------ ");
                }  
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine(" >>>>> XEQUEMATE! <<<<< ");
                Console.WriteLine();
                Console.WriteLine("   --> VENCEDOR: " + partida.JogadorAtual + " <--");
            }
        }

        public static void ImprimirPecasCapturadas(PartidaXadrez partida)
        {
            Console.WriteLine("Peças capturadas:");
            Console.Write("Brancas: ");

            ImprimirConjunto(partida.PecasCapturadas(Color.Branca));
            Console.WriteLine();

            Console.Write("Pretas: ");
            ConsoleColor aux = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;

            ImprimirConjunto(partida.PecasCapturadas(Color.Preta));
            Console.ForegroundColor = aux;
            Console.WriteLine();
        }

        public static void ImprimirConjunto(HashSet<Peca> conjunto)
        {
            Console.Write("[");
            foreach (Peca x in conjunto)
            {
                Console.Write(x + " ");
            }
            Console.Write("]");
        }

        public static void PrintTabuleiro(Tabuleiro tab)
        {
            for (int i = 0; i < tab.Linhas; i++)
            {
                Console.Write(8 - i + " ");
                for (int j = 0; j < tab.Colunas; j++)
                {
                    PrintPeca(tab.peca(i, j));
                }
                Console.WriteLine();
            }

            //Console.WriteLine("  a b c d e f g h");

            Console.WriteLine();
            for (char c = 'a'; c <= 'h'; c++)
            {
                Console.Write("  " + c);
            }
            Console.WriteLine();
        } 
        
        public static void PrintTabuleiro(Tabuleiro tab, bool[,] lancesPermitidos)
        {
            ConsoleColor original = Console.BackgroundColor;
            ConsoleColor mod = ConsoleColor.Magenta;

            for (int i = 0; i < tab.Linhas; i++)
            {
                Console.Write(8 - i + " ");
                for (int j = 0; j < tab.Colunas; j++)
                {
                    if(lancesPermitidos[i, j])
                    {
                        Console.BackgroundColor = mod;
                    }
                    else
                    {
                        Console.BackgroundColor = original;
                    }
                    PrintPeca(tab.peca(i, j));
                    Console.BackgroundColor = original;
                }
                Console.WriteLine();
            }

            //Console.WriteLine("  a b c d e f g h");

            Console.WriteLine();
            for (char c = 'a'; c <= 'h'; c++)
            {
                Console.Write("  " + c);
            }
            Console.WriteLine();

            Console.BackgroundColor = original;
        }

        public static NotacaoXadrez LerPosicaoXadrez()
        {
            string s = Console.ReadLine();
            char coluna = s[0];
            int linha = int.Parse(s[1] + "");
            return new NotacaoXadrez(coluna, linha);
        }

        public static void PrintPeca(Peca peca)
        {
            if (peca == null)
            {
                Console.Write("-  ");
            }
            else
            {
                if (peca.Cor == Color.Branca)
                {
                    Console.Write(peca);
                }
                else
                {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(peca);
                    Console.ForegroundColor = aux;
                }

                Console.Write("  ");
            }
        }
    }
}
