using System;

namespace Trabalho_ATP_Bingo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("====================================");
            Console.WriteLine("            JOGO DE BINGO");
            Console.WriteLine("====================================");
            Console.WriteLine();

            Jogo jogo = new Jogo();

            jogo.CadastrarJogadores();
            jogo.DistribuirCartelas();
            jogo.ExibirCartelasIniciais();
            jogo.IniciarPartida();
            jogo.ExibirRanking();
            jogo.ExibirLog();

            Console.WriteLine();
            Console.WriteLine("Fim do jogo. Pressione qualquer tecla para sair...");
            Console.ReadKey();
        }
    }
}
