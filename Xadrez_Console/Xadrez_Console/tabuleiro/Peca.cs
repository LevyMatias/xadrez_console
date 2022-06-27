namespace tabuleiro
{
    abstract class Peca
    {
        public Posicao Posicao { get; set; }
        public Color Cor { get; protected set; }
        public int QntdMovimentos { get; protected set; }
        public Tabuleiro Tab { get; protected set; }

        public Peca(Color cor, Tabuleiro tab)
        {
            Posicao = null;
            Cor = cor;
            Tab = tab;
            QntdMovimentos = 0;
        }

        public void IncrementarQntdMovimentos()
        {
            QntdMovimentos++;
        }
        public void DecrementarQntdMovimentos()
        {
            QntdMovimentos--;
        }

        public bool ExisteMovimentosPossiveis()
        {
            bool[,] mat = MovimentosPossiveis();
            for (int i = 0; i < Tab.Linhas; i++)
            {
                for (int j = 0; j < Tab.Colunas; j++)
                {
                    if (mat[i, j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool MovimentoPossivel(Posicao pos)
        {
            return MovimentosPossiveis()[pos.Linha, pos.Coluna];
        }

        public abstract bool[,] MovimentosPossiveis();
    }
}
