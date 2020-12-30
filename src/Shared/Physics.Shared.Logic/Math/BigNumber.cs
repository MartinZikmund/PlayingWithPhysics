namespace Physics.Shared.Math
{
	public struct BigNumber
    {
		public BigNumber(double mantisa, double exponent)
		{
			Mantisa = mantisa;
			Exponent = exponent;
		}

        public double Mantisa { get; }

		public double Exponent { get; }
    }
}
