using Xunit;

namespace Physics.GravitationalFieldMovement.Logic.Tests;

public class InputConfigurationTests
{
	[Fact]
	public void InputConfiguration_TestData1()
	{
		var input = new InputConfiguration(
			6378000,
			5.97E+24,
			900000,
			8000,
			0,
			90);

		Assert.Equal(1.571, input.Phi0, 3);
		Assert.Equal(7278000.000, input.R0, 3);
		Assert.Equal(0.000, input.X0, 3);
		Assert.Equal(7278000.000, input.Y0, 3);
		Assert.Equal(0.000, input.Beta, 3);
		Assert.Equal(0, input.SigBeta);
		Assert.Equal(-8000.000, input.Vx0, 3);
		Assert.Equal(0.000, input.Vy0, 3);
		Assert.Equal(398199000000000.000, input.Alpha, 3);
		Assert.Equal(58224000000.000, input.L, 3);
		Assert.Equal(1, input.SigL, 3);
		Assert.Equal(-22712695.796, input.En, 3);
		Assert.Equal(8513417.100, input.P, 3);
		Assert.Equal(0.16975, input.Eps, 5);
		Assert.Equal(MovementType.Ellipse, input.ConicSec);
		Assert.Equal(8766000.381, input.A, 3);
		Assert.Equal(0.001, input.N, 3);
		Assert.Equal(8172.069, input.T, 3);
		Assert.Equal(2.270019, input.PeriodInHours, 3);
		Assert.Equal(0.094584, input.PeriodInDays, 3);
		Assert.Equal(1.0000, input.Chi, 3);
		Assert.Equal(0.000, input.Theta0, 3);
		Assert.NotNull(input.E0);
		Assert.Equal(0.000, input.E0!.Value, 3);
		Assert.Equal(0.000, input.M0, 3);
		Assert.Null(input.H0);
		Assert.Equal(0.000, input.Tau, 3);
		Assert.Equal(1.571, input.Omega, 3);
		Assert.Equal(90.0, input.OmegaDeg, 3);
	}

	[Fact]
	public void InputConfiguration_TestData2()
	{
		var input = new InputConfiguration(
			696000000,
			1.99E+30,
			248530000000,
			21972,
			0,
			90);

		Assert.Equal(1.571, input.Phi0, 3);
		Assert.Equal(249226000000.000, input.R0, 3);
		Assert.Equal(0.000, input.X0, 3);
		Assert.Equal(249226000000.000, input.Y0, 3);
		Assert.Equal(0.000, input.Beta, 3);
		Assert.Equal(0, input.SigBeta);
		Assert.Equal(-21972.000, input.Vx0, 3);
		Assert.Equal(0.000, input.Vy0, 3);
		Assert.Equal(1.32733E+20, input.Alpha, 3);
		Assert.Equal(5475993672000000.000, input.L, 3);
		Assert.Equal(1, input.SigL, 3);
		Assert.Equal(-291196478.375, input.En, 3);
		Assert.Equal(225915986949.621, input.P, 3);
		Assert.Equal(0.09353, input.Eps, 5);
		Assert.Equal(MovementType.Ellipse, input.ConicSec);
		Assert.Equal(227909693037.588, input.A, 3);
		Assert.Equal(0.000, input.N, 3);
		Assert.Equal(59338232.716, input.T, 3);
		Assert.Equal(16482.842, input.PeriodInHours, 3);
		Assert.Equal(686.785, input.PeriodInDays, 3);
		Assert.Equal(-1.0000, input.Chi, 3);
		Assert.Equal(3.142, input.Theta0, 3);
		Assert.NotNull(input.E0);
		Assert.Equal(3.1416, input.E0!.Value, 3);
		Assert.Equal(3.142, input.M0, 3);
		Assert.Null(input.H0);
		Assert.Equal(-29669116.358, input.Tau, 3);
		Assert.Equal(-1.571, input.Omega, 3);
		Assert.Equal(-90.0, input.OmegaDeg, 3);
	}
}
