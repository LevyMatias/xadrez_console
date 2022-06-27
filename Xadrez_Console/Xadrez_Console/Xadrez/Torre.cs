using tabuleiro;

namespace Xadrez
{
    class Torre : Peca
    {
        public Torre(Tabuleiro tab, Color cor) : base(cor, tab)
        {
        }

        public override string ToString()
        {
            return "T";
        }

        private bool Movimento(Posicao pos)
        {
            Peca p = Tab.peca(pos);
            return p == null || p.Cor != Cor;
        }

        public override bool[,] MovimentosPossiveis()
        {
            bool[,] mat = new bool[Tab.Linhas, Tab.Colunas];

            Posicao pos = new Posicao(0, 0);

            // norte
            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna);

            while(Tab.PosicaoValida(pos) && Movimento(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;

                if(Tab.peca(pos) != null && Tab.peca(pos).Cor != Cor)
                {
                    break;
                }
                pos.Linha = pos.Linha - 1;
            }
            
            // sul
            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna);

            while(Tab.PosicaoValida(pos) && Movimento(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;

                if(Tab.peca(pos) != null && Tab.peca(pos).Cor != Cor)
                {
                    break;
                }
                pos.Linha = pos.Linha + 1;
            }
            
            // leste
            pos.DefinirValores(Posicao.Linha, Posicao.Coluna +1);

            while(Tab.PosicaoValida(pos) && Movimento(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;

                if(Tab.peca(pos) != null && Tab.peca(pos).Cor != Cor)
                {
                    break;
                }
                pos.Coluna = pos.Coluna + 1;
            }
            
            // oeste
            pos.DefinirValores(Posicao.Linha, Posicao.Coluna -1);

            while(Tab.PosicaoValida(pos) && Movimento(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;

                if(Tab.peca(pos) != null && Tab.peca(pos).Cor != Cor)
                {
                    break;
                }
                pos.Coluna = pos.Coluna - 1;
            }
            return mat;
        }
    }
}
