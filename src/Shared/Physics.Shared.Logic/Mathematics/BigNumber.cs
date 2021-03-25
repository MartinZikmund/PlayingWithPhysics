﻿using System;

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
					newMantisa *= 10;
				}
			}
			else if (Exponent < newExponent)
			{
				var diff = newExponent - Exponent;
				for (int i = 0; i < diff; i++)
				{
					newMantisa /= 10;
				}
			}
			return new BigNumber(newMantisa, newExponent);
		}

		public BigNumber Normalize()
		{
			var mantisa = Mantisa;
			var exponent = Exponent;
			while (mantisa > 10)
			{
				mantisa /= 10;
				exponent++;
			}
			while (mantisa < 1 && mantisa > 0.0001)
			{
				mantisa *= 10;
				exponent--;
			}
			while (mantisa < 0 && mantisa > -1)
			{
				mantisa *= 10;
				exponent--;
			}
			while (mantisa < -10)
			{
				mantisa /= 10;
				exponent++;
			}

			return new BigNumber(mantisa, exponent);
		}

		public BigNumber Sqrt()
		{
			var mantisa = Mantisa;
			var exponent = Exponent;
			if (Exponent % 2 == 1)
			{
				mantisa /= 10;
				exponent++;
			}
			return new BigNumber(Math.Sqrt(mantisa), exponent / 2);
		}

		public override string ToString()
		{
			var normalized = Normalize();
			if (normalized.Mantisa == 0)
			{
				return "0";
			}
			return $"{normalized.Mantisa.ToString("0.000")}.10^{normalized.Exponent}";
		}

		public string ToString(string formatString)
		{
			var normalized = Normalize();
			if (normalized.Mantisa == 0)
			{
				return 0.ToString(formatString);
			}
			return $"{normalized.Mantisa.ToString(formatString)}.10^{normalized.Exponent}";
		}
	}
}
