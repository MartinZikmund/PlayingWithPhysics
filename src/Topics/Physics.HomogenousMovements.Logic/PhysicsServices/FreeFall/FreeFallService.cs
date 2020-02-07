namespace Physics.HomogenousMovement.PhysicsServices.FreeFall
{
    class ValueRow
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Time { get; set; }

        public ValueRow(double x, double y, double time)
        {
            X = x;
            Y = y;
            Time = time;
        }
    }
    /*
    class FreeFallService : IPhysicsService
    {
        public const double G = 9.8;
        public double YStart { get; set; }
        public List<ValueRow> ValueTable = new List<ValueRow>();
        public double ComputeY(double time)
        {
            //y = y0 - ((0.5 * g) * (t^2))
            return (YStart - ((0.5 * G) * (Math.Pow(time, 2))));
        }

        public void PopulateValuesTable(double x, double y, double mass, double timeInterval)
        {
            ValueTable.Add(new ValueRow(x, y, 0));
            double yChangeInInterval = ComputeY(timeInterval);
            int iterations = 0;
            while (y > 0)
            {
                ValueTable.Add(new ValueRow(x, y, iterations*timeInterval));
                iterations++;
                y -= yChangeInInterval;
            }
        }
        */
}
