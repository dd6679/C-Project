namespace DotPixelEditor
{
    class Point
    {
        public int X;
        public int Y;
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Point Parse(string strPt)
        {
            string[] values = strPt.Split(',');
            if (values == null || values.Length != 2)
                return null;

            return new Point(int.Parse(values[0]), int.Parse(values[1]));
        }
    }
}
