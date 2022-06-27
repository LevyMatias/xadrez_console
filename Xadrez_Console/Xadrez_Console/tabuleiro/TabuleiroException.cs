using System;

namespace tabuleiro
{
    class TabuleiroException : Exception
    {
        public TabuleiroException(string message) : base(message)
        {
        }
    }
}
