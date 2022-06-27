using tabuleiro;

namespace Xadrez
{
    class NotacaoXadrez
    {
        public char Coluna { get; set; }
        public int Linha { get; set; }

        public NotacaoXadrez(char coluna, int linha)
        {
            Coluna = coluna;
            Linha = linha;
        }

        public Posicao ToPosicao() // converte para notacao do xadrez
        {
            return new Posicao(8 - Linha, Coluna - 'a');
        }

        public override string ToString()
        {
            return "" + Coluna + Linha;
        }
    }
}
