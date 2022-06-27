using tabuleiro;

namespace Xadrez
{
    class Rei : Peca
    {
        private PartidaXadrez Partida;
        public Rei(Tabuleiro tab, Color cor, PartidaXadrez partida) : base(cor, tab) 
        {
            Partida = partida;
        }

        public override string ToString()
        {
            return "R";
        }

        private bool Movimento(Posicao pos)
        {
            Peca p = Tab.peca(pos);
            return p == null || p.Cor != Cor;
        }

        private bool TesteTorreParaRoque(Posicao pos)
        {
            Peca p = Tab.peca(pos);
            return p != null && p is Torre && p.Cor == Cor && p.QntdMovimentos == 0;
        }

        public override bool[,] MovimentosPossiveis()
        {
            bool[,] mat = new bool[Tab.Linhas, Tab.Colunas];

            Posicao pos = new Posicao(0,0);

            // movimento norte
            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna);
            if (Tab.PosicaoValida(pos) && Movimento(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }
            
            // movimento nordeste
            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna + 1);
            if (Tab.PosicaoValida(pos) && Movimento(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }
            
            // movimento leste
            pos.DefinirValores(Posicao.Linha, Posicao.Coluna + 1);
            if (Tab.PosicaoValida(pos) && Movimento(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }
            
            // movimento sudeste
            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna + 1);
            if (Tab.PosicaoValida(pos) && Movimento(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }
            
            // movimento sul
            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna);
            if (Tab.PosicaoValida(pos) && Movimento(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }
            
            // movimento sudoeste
            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna - 1);
            if (Tab.PosicaoValida(pos) && Movimento(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }
            
            // movimento oeste
            pos.DefinirValores(Posicao.Linha, Posicao.Coluna - 1);
            if (Tab.PosicaoValida(pos) && Movimento(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }
            
            // movimento noroeste
            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna - 1);
            if (Tab.PosicaoValida(pos) && Movimento(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }

            if (QntdMovimentos == 0 && !Partida.Xeque)
            {
                // jogada especial (roque pequeno)
                Posicao posT1 = new Posicao(Posicao.Linha, Posicao.Coluna + 3);

                if (TesteTorreParaRoque(posT1))
                {
                    Posicao p1 = new Posicao(Posicao.Linha, Posicao.Coluna + 1);
                    Posicao p2 = new Posicao(Posicao.Linha, Posicao.Coluna + 2);
                    if (Tab.peca(p1) == null && Tab.peca(p2) == null)
                    {
                        mat[Posicao.Linha, Posicao.Coluna + 2] = true;
                    }
                }

                // jogada especial (roque grande)
                Posicao posT2 = new Posicao(Posicao.Linha, Posicao.Coluna - 4);
                if (TesteTorreParaRoque(posT2))
                {
                    Posicao p1 = new Posicao(Posicao.Linha, Posicao.Coluna - 1);
                    Posicao p2 = new Posicao(Posicao.Linha, Posicao.Coluna - 2);
                    Posicao p3 = new Posicao(Posicao.Linha, Posicao.Coluna - 3);
                    if (Tab.peca(p1) == null && Tab.peca(p2) == null && Tab.peca(p3) == null)
                    {
                        mat[Posicao.Linha, Posicao.Coluna - 2] = true;
                    }
                }
            }

                return mat;
        }

    }
}
