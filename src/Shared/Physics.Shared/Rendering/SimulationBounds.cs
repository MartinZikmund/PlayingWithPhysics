namespace Physics.Shared.UI.Rendering
{
    public struct SimulationBounds
    {
        /// <summary>
        /// Top - Y = "0"
        /// Left - X = "0"
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        public SimulationBounds(float left, float top, float right, float bottom)
        {
            Left = left;
            Bottom = bottom;
            Right = right;
            Top = top;
        }

        public float Left { get; }

        public float Bottom { get; }

        public float Right { get; }

        public float Top { get; }
    }
}
