﻿using System;
using Physics.Shared.Helpers;
using Physics.Shared.Logic.Constants;
using Physics.Shared.Mathematics;

namespace Physics.GravitationalFieldMovement.Logic;

public class InputConfiguration
{
	public InputConfiguration(BigNumber planetRadius, BigNumber planetMass, BigNumber height, BigNumber startVelocity, double elevationAngle, double coordinateAngle)
	{
		RzBigNumber = planetRadius;
		MzBigNumber = planetMass;
		HBigNumber = height;
		V0BigNumber = startVelocity;
		BetaDeg = elevationAngle;
		Phi0Deg = coordinateAngle;

		// Calculate extended parameters
		Phi0 = 2 * Math.PI * Phi0Deg / 360;
		R0 = Rz + H;
		X0 = R0 * Math.Cos(Phi0);
		Y0 = R0 * Math.Sin(Phi0);
		Beta = ((BetaDeg + 180) % 360 - 180) * Math.PI / 180;
		SigBeta = Math.Sign(Beta);
		Vx0 = V0 * Math.Cos(Phi0 - Beta + Math.PI / 2);
		Vy0 = V0 * Math.Sin(Phi0 - Beta + Math.PI / 2);
		Alpha = PhysicsConstants.GravitationalConstant * Mz;
		L = R0 * V0 * Math.Cos(Beta);
		SigL = Math.Sign(L);
		En = V0 * V0 / 2 - Alpha / R0;
		P = L * L / Alpha;
		Eps = Math.Sqrt(1 + 2 * L * L * En / (Alpha * Alpha));

		ConicSec = En < 0 ? MovementType.Ellipse : MovementType.Hyperbola;

		A = -Alpha / (2 * En);
		var absA = Math.Abs(A);
		N = Math.Sqrt(Alpha / (absA * absA * absA));
		T = 2 * Math.PI / N;
		PeriodInHours = T / 3600;
		PeriodInDays = T / (3600 * 24);

		if ((Math.Abs(P / R0 - 1) / Eps) - 1 > 0)
		{
			Chi = Math.Sign((P / R0 - 1) / Eps);
		}
		else
		{
			Chi = (P / R0 - 1) / Eps;
		}

		if (SigBeta == 0)
		{
			if (Math.Sign(Chi) == -1)
			{
				Theta0 = Math.PI;
			}
			else
			{
				Theta0 = 0;
			}
		}
		else
		{
			Theta0 = SigBeta * Math.Acos(Chi);
		}

		if (ConicSec == MovementType.Ellipse)
		{
			Psi = MathHelpers.Clamp(-1, (1 - R0 / A) / Eps, 1);
			Psi = Math.Min(1.0d, Math.Abs(Psi.Value)) * Math.Sign(Psi.Value);
			if (SigBeta == 0)
			{
				E0 = Math.Sign(Chi) == -1 ? Math.PI : 0;
			}
			else
			{
				E0 = SigBeta * Math.Acos(Psi.Value);
			}

			M0 = E0.Value - Eps * Math.Sin(E0.Value);
		}

		if (ConicSec == MovementType.Hyperbola)
		{
			Psi = (1 - R0 / A) / Eps;
			Psi = Math.Max(1.0d, Psi.Value);
			H0 = MathHelpers.Acosh(Psi.Value) * SigBeta;
			M0 = Eps * Math.Sinh(H0.Value) - H0.Value;
		}

		Tau = -M0 / N;
		Omega = Phi0 - SigL * Theta0;
		OmegaDeg = 360 * Omega / (2 * Math.PI);


		// Time defaults
		if ( ConicSec == MovementType.Ellipse)
		{
			TMax = T * 1.15;
		}
		else
		{
			var thetaInf = Math.Acos(-1 / Eps);
			var theta1 = (thetaInf + Math.Abs(Theta0)) / 2;
			var theta2 = 0.9 * thetaInf;
			var thetaMax = Math.Max(theta1, theta2);
			var hMax =
				2 * MathHelpers.Atanh(
					Math.Sqrt((Eps - 1) / (Eps + 1)) *
					Math.Tan(thetaMax / 2));
			var mMax = Eps * Math.Sinh(hMax) - hMax;
			TMax = mMax / N + Tau;
		}
		var nSteps = 3600;
		Dt = TMax / nSteps;
	}

	public BigNumber RzBigNumber { get; }

	public double Rz => (double)RzBigNumber;

	public BigNumber MzBigNumber { get; }

	public double Mz => (double)MzBigNumber;

	public BigNumber HBigNumber { get; }

	public double H => (double)HBigNumber;

	public BigNumber V0BigNumber { get; }

	public double V0 => (double)V0BigNumber;

	public double BetaDeg { get; } = 0;

	public double Phi0Deg { get; }

	// Extended parameters

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

	public MovementType ConicSec { get; }

	public double A { get; }

	public double N { get; }

	public double T { get; }

	public double PeriodInHours { get; }

	public double PeriodInDays { get; }

	public double Chi { get; }

	public double Theta0 { get; }

	public double? Psi { get; }

	public double? E0 { get; }

	public double M0 { get; }

	public double? H0 { get; }

	public bool IsEllipse { get; }

	public double Tau { get; }

	public double Omega { get; }

	public double OmegaDeg { get; }

	public double TMax { get; }

	public double Dt { get; }
	
	public static InputConfiguration Default => new InputConfiguration(new BigNumber(6.38, 6), new BigNumber(5.97, 24), new BigNumber(9.0, 5), new BigNumber(7.0, 3), 0, 90);
}
