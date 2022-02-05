using System;

namespace Physics.GravitationalFieldMovement.Logic
{
	internal class PhysicsService
	{
		public PhysicsService(InputConfiguration input)
		{

		}

		public double Phi0 { get; }

		public double R0 { get; }

		public double X0 { get; }

		public double Y0 { get; }

		public double Beta { get; }

		public double SigBeta { get; }

		public double Vx0 { get; }

		public double Vy0 { get; }

		public double Alpha { get; }

		public double L { get; }

		public double SigL { get; }

		public double En { get; }

		public double P { get; }

		public double Eps { get; }

		public double ConicSec { get; }

		public double A { get; }

		public double N { get; }

		public double T { get; }

		public double PeriodInHours { get; }

		public double PeriodInDays { get; }

		public double Chi { get; }

		public double Theta0 { get; }

		public double? E0 { get; }

		public double M0 { get; }

		public double? H0 { get; }

		public bool IsEllipse { get; }

		public double Tau { get; }

		public double Omega { get; }

		public double OmegaDeg { get; }
	}
}
