
namespace SoulLang
{
    public struct Position
    {
        public int Line { get; }
        public int Column { get; }

        public Position(int line, int column)
        {
            Line = line;
            Column = column;
        }
    }
}
