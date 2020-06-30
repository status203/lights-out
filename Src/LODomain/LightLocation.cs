namespace LODomain
{
    public class LightLocation
    {
        public int Row { get; private set; }
        public int Column { get; private set; }

        public LightLocation(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}