using System;
using System.Collections.Generic;
using tabuleiro;

namespace Xadrez
{
    class PartidaXadrez
    {
        public Tabuleiro Tab { get; private set; }

        public int Turno { get; private set; }
        public Color JogadorAtual { get; private set; }
        public bool Terminada { get; private set; }

        private HashSet<Peca> Pecas;
        private HashSet<Peca> Capturadas;
        public bool Xeque { get; private set; }
        public Peca VulneravelEnPassant { get; private set; }

        public PartidaXadrez()
        {
            Tab = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Color.Branca;
            Terminada = false;
            Xeque = false;
            VulneravelEnPassant = null;
            Pecas = new HashSet<Peca>();
            Capturadas = new HashSet<Peca>();
            ColocarPecas();
        }

        public Peca ExecutarMovimento(Posicao origem, Posicao destino)
        {
            Peca p = Tab.RetirarPeca(origem);
            p.IncrementarQntdMovimentos();
            Peca pecaCapturada = Tab.RetirarPeca(destino);
            Terminada = false;
            Tab.ColocarPeca(p,destino);

            if(pecaCapturada != null)
            {
                Capturadas.Add(pecaCapturada);
            }

            // jogada especial roque pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = Tab.RetirarPeca(origemT);
                T.IncrementarQntdMovimentos();
                Tab.ColocarPeca(T, destinoT);
            }

            // jogada especial roque grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = Tab.RetirarPeca(origemT);
                T.IncrementarQntdMovimentos();
                Tab.ColocarPeca(T, destinoT);
            }

            // jogada especial en passant
            if (p is Peao)
            {
                if (origem.Coluna != destino.Coluna && pecaCapturada == null)
                {
                    Posicao posP;
                    if (p.Cor == Color.Branca)
                    {
                        posP = new Posicao(destino.Linha + 1, destino.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(destino.Linha - 1, destino.Coluna);
                    }
                    pecaCapturada = Tab.RetirarPeca(posP);
                    Capturadas.Add(pecaCapturada);
                }
            }



            return pecaCapturada;
        }

        public void DesfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = Tab.RetirarPeca(destino);
            p.DecrementarQntdMovimentos();
            if (pecaCapturada != null)
            {
                Tab.ColocarPeca(pecaCapturada, destino);
                Capturadas.Remove(pecaCapturada);
            }

            Tab.ColocarPeca(p, origem);

            // jogada especial roque pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = Tab.RetirarPeca(destinoT);
                T.DecrementarQntdMovimentos();
                Tab.ColocarPeca(T, origemT);
            }

            // jogada especial roque grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = Tab.RetirarPeca(destinoT);
                T.DecrementarQntdMovimentos();
                Tab.ColocarPeca(T, origemT);
            }

            // jogada especial en passant
            if (p is Peao)
            {
                if (origem.Coluna != destino.Coluna && pecaCapturada == VulneravelEnPassant)
                {
                    Peca peao = Tab.RetirarPeca(destino);
                    Posicao posP;
                    if (p.Cor == Color.Branca)
                    {
                        posP = new Posicao(3, destino.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(4, destino.Coluna);
                    }
                    Tab.ColocarPeca(peao, posP);
                }
            }
        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = ExecutarMovimento(origem, destino);

            if (EstaEmXeque(JogadorAtual))
            {
                DesfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }

            Peca p = Tab.peca(destino);

            // #jogadaespecial promocao
            if (p is Peao)
            {
                if ((p.Cor == Color.Branca && destino.Linha == 0) || (p.Cor == Color.Preta && destino.Linha == 7))
                {
                    p = Tab.RetirarPeca(destino);
                    Pecas.Remove(p);
                    Peca dama = new Dama(Tab, p.Cor);
                    Tab.ColocarPeca(dama, destino);
                    Pecas.Add(dama);
                }
            }

            if (EstaEmXeque(Adversaria(JogadorAtual)))
            {
                Xeque = true;
            }
            else
            {
                Xeque = false;
            }

            if (TesteXequemate(Adversaria(JogadorAtual)))
            {
                Terminada = true;
            }
            else
            {
                Turno++;
                MudarJogador();
            }

            // jogada especial en passant
            if (p is Peao && (destino.Linha == origem.Linha - 2 || destino.Linha == origem.Linha + 2))
            {
                VulneravelEnPassant = p;
            }
            else
            {
                VulneravelEnPassant = null;
            }
        }

        public void ValidarPosicaoDeOrigem(Posicao pos)
        {
            if (Tab.peca(pos) == null)
            {
                throw new TabuleiroException("Não existe peça nessa posição de origem escolhida!");
            }
            if (JogadorAtual != Tab.peca(pos).Cor)
            {
                throw new TabuleiroException("A peça de origem escolhida não é sua!");
            }
            if (!Tab.peca(pos).ExisteMovimentosPossiveis())
            {
                throw new TabuleiroException("Não há lances permitidos para a peça de origem escolhida!");
            }
        }

        public void ValidarPosicaoDeDestino(Posicao origem, Posicao destino)
        {
            if (!Tab.peca(origem).MovimentoPossivel(destino))
            {
                throw new TabuleiroException("Posição de destino inválida!");
            }
        }

        private void MudarJogador()
        {
            if(JogadorAtual == Color.Branca)
            {
                JogadorAtual = Color.Preta;
            }
            else
            {
                JogadorAtual = Color.Branca;
            }
        }

        public HashSet<Peca> PecasCapturadas(Color Color)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in Capturadas)
            {
                if (x.Cor == Color)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<Peca> PecasEmJogo(Color Color)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in Pecas)
            {
                if (x.Cor == Color)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(PecasCapturadas(Color));
            return aux;
        }

        private Color Adversaria(Color Color)
        {
            if (Color == Color.Branca)
            {
                return Color.Preta;
            }
            else
            {
                return Color.Branca;
            }
        }

        private Peca rei(Color Color)
        {
            foreach (Peca x in PecasEmJogo(Color))
            {
                if (x is Rei)
                {
                    return x;
                }
            }
            return null;
        }

        public bool EstaEmXeque(Color Color)
        {
            Peca R = rei(Color);
            if (R == null)
            {
                throw new TabuleiroException("Não tem rei da Color " + Color + " no Tabuleiro!");
            }
            foreach (Peca x in PecasEmJogo(Adversaria(Color)))
            {
                bool[,] mat = x.MovimentosPossiveis();
                if (mat[R.Posicao.Linha, R.Posicao.Coluna])
                {
                    return true;
                }
            }
            return false;
        }

        public bool TesteXequemate(Color Color)
        {
            if (!EstaEmXeque(Color))
            {
                return false;
            }
            foreach (Peca x in PecasEmJogo(Color))
            {
                bool[,] mat = x.MovimentosPossiveis();
                for (int i = 0; i < Tab.Linhas; i++)
                {
                    for (int j = 0; j < Tab.Colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = x.Posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = ExecutarMovimento(origem, destino);
                            bool testeXeque = EstaEmXeque(Color);
                            DesfazMovimento(origem, destino, pecaCapturada);

                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public void ColocarNovaPeca(char coluna, int linha, Peca peca)
        {
            Tab.ColocarPeca(peca, new NotacaoXadrez(coluna, linha).ToPosicao());
            Pecas.Add(peca);
        }
        private void ColocarPecas()
        {
            // white pieces
            ColocarNovaPeca('a', 1, new Torre(Tab, Color.Branca));
            ColocarNovaPeca('b', 1, new Cavalo(Tab, Color.Branca));
            ColocarNovaPeca('c', 1, new Bispo(Tab, Color.Branca));
            ColocarNovaPeca('d', 1, new Dama(Tab, Color.Branca));
            ColocarNovaPeca('e', 1, new Rei(Tab, Color.Branca, this));
            ColocarNovaPeca('f', 1, new Bispo(Tab, Color.Branca));
            ColocarNovaPeca('g', 1, new Cavalo(Tab, Color.Branca));
            ColocarNovaPeca('h', 1, new Torre(Tab, Color.Branca));
            ColocarNovaPeca('a', 2, new Peao(Tab, Color.Branca, this));
            ColocarNovaPeca('b', 2, new Peao(Tab, Color.Branca, this));
            ColocarNovaPeca('c', 2, new Peao(Tab, Color.Branca, this));
            ColocarNovaPeca('d', 2, new Peao(Tab, Color.Branca, this));
            ColocarNovaPeca('e', 2, new Peao(Tab, Color.Branca, this));
            ColocarNovaPeca('f', 2, new Peao(Tab, Color.Branca, this));
            ColocarNovaPeca('g', 2, new Peao(Tab, Color.Branca, this));
            ColocarNovaPeca('h', 2, new Peao(Tab, Color.Branca, this));

            // black pieces
            ColocarNovaPeca('a', 8, new Torre(Tab, Color.Preta));
            ColocarNovaPeca('b', 8, new Cavalo(Tab, Color.Preta));
            ColocarNovaPeca('c', 8, new Bispo(Tab, Color.Preta));
            ColocarNovaPeca('d', 8, new Dama(Tab, Color.Preta));
            ColocarNovaPeca('e', 8, new Rei(Tab, Color.Preta, this));
            ColocarNovaPeca('f', 8, new Bispo(Tab, Color.Preta));
            ColocarNovaPeca('g', 8, new Cavalo(Tab, Color.Preta));
            ColocarNovaPeca('h', 8, new Torre(Tab, Color.Preta));
            ColocarNovaPeca('a', 7, new Peao(Tab, Color.Preta, this));
            ColocarNovaPeca('b', 7, new Peao(Tab, Color.Preta, this));
            ColocarNovaPeca('c', 7, new Peao(Tab, Color.Preta, this));
            ColocarNovaPeca('d', 7, new Peao(Tab, Color.Preta, this));
            ColocarNovaPeca('e', 7, new Peao(Tab, Color.Preta, this));
            ColocarNovaPeca('f', 7, new Peao(Tab, Color.Preta, this));
            ColocarNovaPeca('g', 7, new Peao(Tab, Color.Preta, this));
            ColocarNovaPeca('h', 7, new Peao(Tab, Color.Preta, this));
        }
    }
}
