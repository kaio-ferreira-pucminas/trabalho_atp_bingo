using System;

namespace Trabalho_ATP_Bingo
{
    class Jogador
    {
        private string nome;
        private int idade;
        private string sexo;
        private Cartela[] cartelas = new Cartela[4];
        private int quantCartelas;
        private bool estaAtivo;
        private int posicaoRanking;

        public Jogador(string nome, int idade, string sexo, int quantCartelas)
        {
            this.Nome = nome;
            this.Idade = idade;
            this.Sexo = sexo;
            this.cartelas = new Cartela[this.QuantCartelas];
            this.EstaAtivo = true;
        }



        // metodo adicionar cartela
        public void AdicionarCartela(Cartela c)
        {
            cartelas[this.quantCartelas] = c;
            quantCartelas++;
        }
        // metodo get e set

        public Cartela[] Cartelas
        {
            get { return this.cartelas; }
            set { this.cartelas = value; }
        }
        public int QuantCartelas
        {
            get { return this.quantCartelas; }
            set { this.quantCartelas = value; }

        }
        public bool EstaAtivo
        {
            get { return this.estaAtivo; }
            set { this.estaAtivo = value; }
        }
        public int PosicaoRanking
        {
            get { return this.posicaoRanking; }
            set { this.posicaoRanking = value; }
        }

        public string Nome
        {
            get { return this.nome; }
            set { this.nome = value; }
        }
        public int Idade
        {
            get { return this.idade; }
            set { this.idade = value; }
        }
        public string Sexo
        {
            get { return this.sexo; }
            set { this.sexo = value; }
        }




    }
}
