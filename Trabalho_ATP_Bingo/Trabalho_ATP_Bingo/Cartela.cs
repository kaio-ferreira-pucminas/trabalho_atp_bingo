using System;

namespace Trabalho_ATP_Bingo
{
    class Cartela
    {
        private int num;
        private int[,] numeros = new int[5, 5];
        private bool[,] marcados = new bool[5, 5];
        public bool VerificarMatriz(int num)
        {

            for (int i = 0; i < numeros.GetLength(0); i++)
            {
                for (int j = 0; j < numeros.GetLength(1); j++)
                {
                    if (numeros[i, j] == num) return true;

                }
            }
            return false;
        }

        public void GerarNumero(Random r)
        {
            bool ok = true;
            for (int i = 0; i < numeros.GetLength(0); i++)
            {

                for (int j = 0; j < numeros.GetLength(1); j++)
                {
                    // ----- marcar o meio ----

                    if (i == 2 && j == 2)
                    {
                        numeros[i, j] = 0;
                        marcados[i, j] = true;
                    }
                    // -----primeira coluna----

                    else if (j == 0)
                    {
                        num = r.Next(1, 16);
                        if (VerificarMatriz(num))
                        {
                            do
                            {
                                num = r.Next(1, 16);
                                if (VerificarMatriz(num)) ok = true;
                                else { numeros[i, j] = num; ok = false; }
                            } while (ok == true);

                        }
                        else { numeros[i, j] = num; }
                        //------ segunda coluna-------

                    }
                    else if (j == 1)
                    {
                        num = r.Next(16, 31);
                        if (VerificarMatriz(num))
                        {
                            do
                            {
                                num = r.Next(16, 31);
                                if (VerificarMatriz(num)) ok = true;
                                else { numeros[i, j] = num; ok = false; }
                            } while (ok == true);
                        }
                        else { numeros[i, j] = num; }

                    }

                    //------ terceira coluna-------

                    else if (j == 2)
                    {
                        num = r.Next(31, 46);
                        if (VerificarMatriz(num))
                        {
                            do
                            {
                                num = r.Next(31, 46);
                                if (VerificarMatriz(num)) ok = true;

                                else { numeros[i, j] = num; ok = false; }
                            } while (ok == true);
                        }
                        else { numeros[i, j] = num; }
                    }

                    //------ quarta coluna-------

                    else if (j == 3)
                    {
                        num = r.Next(46, 61);
                        if (VerificarMatriz(num))
                        {
                            do
                            {
                                num = r.Next(46, 61);
                                if (VerificarMatriz(num)) ok = true;

                                else { numeros[i, j] = num; ok = false; }
                            } while (ok == true);

                        }
                        else { numeros[i, j] = num; }
                    }
                    //------ quinta coluna-------
                    else if (j == 4)
                    {
                        num = r.Next(61, 76);
                        if (VerificarMatriz(num))
                        {
                            do
                            {
                                num = r.Next(61, 76);
                                if (VerificarMatriz(num)) ok = true;

                                else { numeros[i, j] = num; ok = false; }
                            } while (ok == true);

                        }
                        else { numeros[i, j] = num; }
                    }
                }
            }
        }

        // metodos da cartela

        public void Exibir()
        {
            Console.WriteLine(" B\t I\t N\t G\t O");
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (i == 2 && j == 2)
                        Console.Write("\t");
                    else if (marcados[i, j])
                        Console.Write("[" + numeros[i, j] + "]\t");
                    else
                        Console.Write(numeros[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }

        public bool Marcar(int numero)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (numeros[i, j] == numero)
                    {
                        marcados[i, j] = true;
                        i = 5;
                        return true;
                    }
                }
            }
            return false;
        }

        public bool VerificarLinha(int linha)
        {
            for (int j = 0; j < marcados.GetLength(1); j++)
            {
                if (marcados[linha, j] != true) return false;

            }
            return true;
        }
        public bool VerificarColuna(int coluna)
        {
            for (int i = 0; i < marcados.GetLength(0); i++)
            {
                if (marcados[i, coluna] != true) return false;
            }
            return true;

        }

        public bool FoiBingo()
        {
            for (int i = 0; i < marcados.GetLength(0); i++)
            {
                if (VerificarColuna(i)) return true;
                if (VerificarLinha(i)) return true;
            }
            return false;
        }
    }
}
