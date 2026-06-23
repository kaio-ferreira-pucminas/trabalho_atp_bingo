using System;
using System.IO;

namespace Trabalho_ATP_Bingo
{
    class Jogo
    {
        private Jogador[] jogadores;
        private int quantidadeJogadores;
        private int[] cartelasDesejadas;
        private int[] numerosJaSorteados = new int[75];
        private int quantidadeSorteados;
        private int posicaoAtualRanking;
        private int posicaoFundoRanking;
        private bool partidaEncerrada;
        private Random random;
        private string caminhoLog;

        public Jogo()
        {
            jogadores = new Jogador[5];
            cartelasDesejadas = new int[5];
            quantidadeJogadores = 0;
            quantidadeSorteados = 0;
            posicaoAtualRanking = 1;
            partidaEncerrada = false;
            random = new Random();
            caminhoLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");

            try
            {
                File.WriteAllText(caminhoLog, "=== LOG DA PARTIDA - " + DateTime.Now + " ===" + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao iniciar o log: " + ex.Message);
            }
        }

        public void CadastrarJogadores()
        {
            Console.WriteLine("=== CADASTRO DE JOGADORES ===");
            Console.WriteLine("Quantos jogadores vao jogar? (minimo 2, maximo 5)");
            quantidadeJogadores = LerInteiro(2, 5);
            posicaoFundoRanking = quantidadeJogadores;
            SalvarLog("Quantidade de jogadores: " + quantidadeJogadores);

            for (int i = 0; i < quantidadeJogadores; i++)
            {
                Console.WriteLine();
                Console.WriteLine("--- Jogador " + (i + 1) + " ---");
                string nome = LerTexto("Nome: ");
                int idade = LerInteiroComMensagem("Idade: ", 1, 120);
                string sexo = LerSexo();
                Console.WriteLine("Com quantas cartelas vai jogar? (1 a 4)");
                int qtd = LerInteiro(1, 4);

                jogadores[i] = new Jogador(nome, idade, sexo, qtd);
                cartelasDesejadas[i] = qtd;
                SalvarLog("Jogador cadastrado: " + nome + ", " + idade + " anos, sexo " + sexo + ", " + qtd + " cartela(s).");
            }
        }

        public void DistribuirCartelas()
        {
            Console.WriteLine();
            Console.WriteLine("=== DISTRIBUINDO CARTELAS ===");
            SalvarLog("Iniciando distribuicao de cartelas.");

            for (int i = 0; i < quantidadeJogadores; i++)
            {
                int qtd = cartelasDesejadas[i];
                jogadores[i].Cartelas = new Cartela[qtd];

                for (int c = 0; c < qtd; c++)
                {
                    Cartela cartela;
                    int tentativas = 0;
                    do
                    {
                        cartela = new Cartela();
                        cartela.GerarNumero(random);
                        tentativas++;
                    } while (JaExisteCartelaIgual(cartela) && tentativas < 100);

                    jogadores[i].AdicionarCartela(cartela);
                    SalvarLog("Cartela " + (c + 1) + " distribuida para " + jogadores[i].Nome + ".");
                }
            }

            Console.WriteLine("Cartelas distribuidas e unicidade validada.");
            SalvarLog("Distribuicao concluida. Unicidade validada.");
        }

        private bool JaExisteCartelaIgual(Cartela nova)
        {
            for (int i = 0; i < quantidadeJogadores; i++)
            {
                for (int c = 0; c < jogadores[i].QuantCartelas; c++)
                {
                    if (jogadores[i].Cartelas[c] != null && MesmosNumeros(nova, jogadores[i].Cartelas[c]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool MesmosNumeros(Cartela a, Cartela b)
        {
            for (int n = 1; n <= 75; n++)
            {
                if (a.VerificarMatriz(n) != b.VerificarMatriz(n)) return false;
            }
            return true;
        }

        public void ExibirCartelasIniciais()
        {
            Console.WriteLine();
            Console.WriteLine("=== CARTELAS DOS JOGADORES ===");
            for (int i = 0; i < quantidadeJogadores; i++)
            {
                Console.WriteLine();
                Console.WriteLine("Jogador " + (i + 1) + " - " + jogadores[i].Nome + ":");
                for (int c = 0; c < jogadores[i].QuantCartelas; c++)
                {
                    Console.WriteLine("Cartela " + (c + 1) + ":");
                    jogadores[i].Cartelas[c].Exibir();
                }
            }
        }

        public void IniciarPartida()
        {
            Console.WriteLine();
            Console.WriteLine("=== A PARTIDA VAI COMECAR ===");
            SalvarLog("=== INICIO DA PARTIDA ===");

            while (!VerificarFimDeJogo())
            {
                int numero = SortearNumero();
                if (numero == -1)
                {
                    Console.WriteLine("Todos os numeros ja foram sorteados. Encerrando a partida.");
                    SalvarLog("Numeros esgotados. Partida encerrada.");
                    break;
                }

                Console.WriteLine();
                Console.WriteLine("Numero sorteado: " + numero);
                Console.WriteLine("Numeros ja sorteados: " + NumerosSorteadosTexto());
                SalvarLog("Numero sorteado: " + numero);

                MarcarCartelas(numero);
                ProcessarCantos();

                if (VerificarFimDeJogo()) break;
            }

            ClassificarAtivosRestantes();
            SalvarLog("=== FIM DA PARTIDA ===");
        }

        public int SortearNumero()
        {
            if (quantidadeSorteados >= 75) return -1;

            int numero;
            bool jaSorteado;
            do
            {
                numero = random.Next(1, 76);
                jaSorteado = false;
                for (int i = 0; i < quantidadeSorteados; i++)
                {
                    if (numerosJaSorteados[i] == numero)
                    {
                        jaSorteado = true;
                        break;
                    }
                }
            } while (jaSorteado);

            numerosJaSorteados[quantidadeSorteados] = numero;
            quantidadeSorteados++;
            return numero;
        }

        public void MarcarCartelas(int n)
        {
            for (int i = 0; i < quantidadeJogadores; i++)
            {
                Jogador j = jogadores[i];
                if (!j.EstaAtivo) continue;

                for (int c = 0; c < j.QuantCartelas; c++)
                {
                    Cartela cartela = j.Cartelas[c];
                    if (cartela != null && cartela.Marcar(n))
                    {
                        SalvarLog(j.Nome + ": numero " + n + " marcado na cartela " + (c + 1) + ".");
                    }
                }
            }
        }

        private void ProcessarCantos()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Algum jogador quer cantar BINGO?");
                Console.WriteLine("Digite o numero do jogador (0 = ninguem / continuar):");
                ExibirJogadoresAtivos();

                int escolha = LerInteiro(0, quantidadeJogadores);
                if (escolha == 0) break;

                Jogador j = jogadores[escolha - 1];
                if (!j.EstaAtivo)
                {
                    Console.WriteLine(j.Nome + " nao esta mais na disputa.");
                    continue;
                }

                SalvarLog(j.Nome + " cantou BINGO.");
                VerificarBingoCantado(j);

                if (VerificarFimDeJogo()) break;
            }
        }

        public void VerificarBingoCantado(Jogador j)
        {
            bool bingoValido = false;
            for (int c = 0; c < j.QuantCartelas; c++)
            {
                Cartela cartela = j.Cartelas[c];
                if (cartela != null && cartela.FoiBingo())
                {
                    bingoValido = true;
                    break;
                }
            }

            if (bingoValido)
            {
                j.EstaAtivo = false;
                j.PosicaoRanking = posicaoAtualRanking;
                posicaoAtualRanking++;
                Console.WriteLine("BINGO de " + j.Nome + " VALIDADO! Posicao no ranking: " + j.PosicaoRanking + ".");
                SalvarLog("BINGO VALIDO: " + j.Nome + " encerrou na posicao " + j.PosicaoRanking + ".");

                if (ContarAtivos() <= 1)
                {
                    partidaEncerrada = true;
                }
                return;
            }

            int validas = ContarCartelasValidas(j);
            if (validas > 1)
            {
                int indice = PrimeiraCartelaValida(j);
                j.Cartelas[indice] = null;
                Console.WriteLine("BINGO de " + j.Nome + " INVALIDO. Uma cartela foi anulada (restam " + (validas - 1) + ").");
                SalvarLog("BINGO INVALIDO: " + j.Nome + " cantou errado. Cartela " + (indice + 1) +
                          " anulada. Restam " + (validas - 1) + " cartela(s).");
            }
            else
            {
                j.EstaAtivo = false;
                j.PosicaoRanking = posicaoFundoRanking;
                posicaoFundoRanking--;
                Console.WriteLine("BINGO de " + j.Nome + " INVALIDO com 1 cartela. " + j.Nome +
                                  " foi ELIMINADO (posicao " + j.PosicaoRanking + ").");
                SalvarLog("BINGO INVALIDO: " + j.Nome + " cantou errado com 1 cartela. ELIMINADO na posicao " +
                          j.PosicaoRanking + ".");
            }
        }

        private int ContarCartelasValidas(Jogador j)
        {
            int total = 0;
            for (int c = 0; c < j.QuantCartelas; c++)
            {
                if (j.Cartelas[c] != null) total++;
            }
            return total;
        }

        private int PrimeiraCartelaValida(Jogador j)
        {
            for (int c = 0; c < j.QuantCartelas; c++)
            {
                if (j.Cartelas[c] != null) return c;
            }
            return -1;
        }

        public bool VerificarFimDeJogo()
        {
            return partidaEncerrada || quantidadeSorteados >= 75 || ContarAtivos() == 0;
        }

        private int ContarAtivos()
        {
            int ativos = 0;
            for (int i = 0; i < quantidadeJogadores; i++)
            {
                if (jogadores[i].EstaAtivo) ativos++;
            }
            return ativos;
        }

        private void ClassificarAtivosRestantes()
        {
            bool porEsgotamento = quantidadeSorteados >= 75 && ContarAtivos() > 1;
            for (int i = 0; i < quantidadeJogadores; i++)
            {
                if (jogadores[i].EstaAtivo)
                {
                    jogadores[i].EstaAtivo = false;
                    jogadores[i].PosicaoRanking = posicaoAtualRanking;
                    posicaoAtualRanking++;
                    string motivo = porEsgotamento
                        ? " (numeros esgotados; desempate pela ordem de cadastro)"
                        : " (ultimo jogador na disputa)";
                    SalvarLog(jogadores[i].Nome + " classificado na posicao " +
                              jogadores[i].PosicaoRanking + motivo + ".");
                }
            }
        }

        public void ExibirRanking()
        {
            Console.WriteLine();
            Console.WriteLine("=== RANKING FINAL ===");
            SalvarLog("=== RANKING FINAL ===");

            for (int pos = 1; pos <= quantidadeJogadores; pos++)
            {
                for (int i = 0; i < quantidadeJogadores; i++)
                {
                    if (jogadores[i].PosicaoRanking == pos)
                    {
                        Jogador j = jogadores[i];
                        string linha = pos + "o lugar - " + j.Nome + ", " + j.Idade + " anos, sexo " + j.Sexo;
                        Console.WriteLine(linha);
                        SalvarLog(linha);
                    }
                }
            }
        }

        public void SalvarLog(string msg)
        {
            try
            {
                string linha = "[" + DateTime.Now.ToString("HH:mm:ss") + "] " + msg + Environment.NewLine;
                File.AppendAllText(caminhoLog, linha);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao gravar o log: " + ex.Message);
            }
        }

        public void ExibirLog()
        {
            Console.WriteLine();
            Console.WriteLine("=== LOG DA PARTIDA (log.txt) ===");
            try
            {
                string conteudo = File.ReadAllText(caminhoLog);
                Console.WriteLine(conteudo);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao ler o log: " + ex.Message);
            }
        }

        private void ExibirJogadoresAtivos()
        {
            for (int i = 0; i < quantidadeJogadores; i++)
            {
                if (jogadores[i].EstaAtivo)
                {
                    Console.WriteLine("  " + (i + 1) + " - " + jogadores[i].Nome);
                }
            }
        }

        private string NumerosSorteadosTexto()
        {
            string texto = "";
            for (int i = 0; i < quantidadeSorteados; i++)
            {
                texto += numerosJaSorteados[i];
                if (i < quantidadeSorteados - 1) texto += ", ";
            }
            return texto;
        }

        private int LerInteiro(int min, int max)
        {
            int valor;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out valor) && valor >= min && valor <= max)
                {
                    return valor;
                }
                Console.WriteLine("Valor invalido. Digite um numero entre " + min + " e " + max + ":");
            }
        }

        private int LerInteiroComMensagem(string mensagem, int min, int max)
        {
            Console.Write(mensagem);
            return LerInteiro(min, max);
        }

        private string LerTexto(string mensagem)
        {
            string texto;
            do
            {
                Console.Write(mensagem);
                texto = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(texto));
            return texto;
        }

        private string LerSexo()
        {
            while (true)
            {
                Console.Write("Sexo (M/F/Outro): ");
                string entrada = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(entrada))
                {
                    string s = entrada.Trim().ToUpper();
                    if (s == "M") return "Masculino";
                    if (s == "F") return "Feminino";
                    if (s == "O" || s == "OUTRO") return "Outro";
                }
                Console.WriteLine("Valor invalido. Informe M, F ou Outro.");
            }
        }
    }
}
