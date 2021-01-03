using System;

namespace Physics.Shared.Mathematics
{
	public struct BigNumber
    {
		public BigNumber(double mantisa, int exponent)
		{
			Mantisa = mantisa;
			Exponent = exponent;
		}

        public double Mantisa { get; }

		public int Exponent { get; }

		public static explicit operator double(BigNumber number) =>
			number.Mantisa * System.Math.Pow(10, number.Exponent);

		public static implicit operator BigNumber(double number) =>
			new BigNumber(number, 0);

		public static implicit operator BigNumber(float number) =>
			new BigNumber(number, 0);

		public static BigNumber operator *(BigNumber a, BigNumber b) =>
			new BigNumber(a.Mantisa * b.Mantisa, a.Exponent + b.Exponent);

		public static BigNumber operator /(BigNumber a, BigNumber b) =>
			new BigNumber(a.Mantisa / b.Mantisa, a.Exponent - b.Exponent);

		public static BigNumber operator +(BigNumber a, BigNumber b)
		{
			if (a.Exponent < b.Exponent)
			{
				b = b.WithExponent(a.Exponent);
			}
			else
			{
				a = a.WithExponent(b.Exponent);
			}
			return new BigNumber(a.Mantisa + b.Mantisa, a.Exponent);
		}

		public static BigNumber operator -(BigNumber a, BigNumber b)
		{
			if (a.Exponent < b.Exponent)
			{
				b = b.WithExponent(a.Exponent);
			}
			else
			{
				a = a.WithExponent(b.Exponent);
			}
			return new BigNumber(a.Mantisa - b.Mantisa, a.Exponent);
		}

		public BigNumber WithExponent(int newExponent)
		{
			var newMantisa = Mantisa;
			if (Exponent > newExponent)
			{
				var diff = Exponent - newExponent;
				for (int i = 0; i < diff; i++)
				{
					newMantisa /= 10;
				}
			}
			else if (Exponent < newExponent)
			{
				var diff = newExponent - Exponent;
				for (int i = 0; i < diff; i++)
				{
					newMantisa *= 10;
				}
			}
			return new BigNumber(newMantisa, newExponent);
		}
	}
}
