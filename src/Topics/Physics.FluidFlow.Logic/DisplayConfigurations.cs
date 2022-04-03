namespace Physics.FluidFlow.Logic;

public static class DisplayConfigurations
{
	public static DisplayConfiguration[] Configurations { get; } = new[]
	{
		new DisplayConfiguration(InputVariant.ContinuityEquation, DiameterRelationType.Equal)
		{
			V = true,
			D = true,
		},
		new DisplayConfiguration(InputVariant.ContinuityEquation, DiameterRelationType.S1Larger)
		{
			V1 = true,
			V2 = true,
			D1 = true,
			D2 = true,
		},
		new DisplayConfiguration(InputVariant.ContinuityEquation, DiameterRelationType.S2Larger)
		{
			V1 = true,
			V2 = true,
			D1 = true,
			D2 = true,
		},
		new DisplayConfiguration(InputVariant.BernoulliEquationWithoutHeightDecrease, DiameterRelationType.S1Larger)
		{
			V1 = true,
			V2 = true,
			D1 = true,
			D2 = true,
			P1 = true,
			P2 = true,
			H1 = true,
			H2 = true,
		},
		new DisplayConfiguration(InputVariant.BernoulliEquationWithoutHeightDecrease, DiameterRelationType.S2Larger)
		{
			V1 = true,
			V2 = true,
			D1 = true,
			D2 = true,
			P1 = true,
			P2 = true,
			H1 = true,
			H2 = true,
		},
		new DisplayConfiguration(InputVariant.BernoulliEquationWithHeightDecrease, DiameterRelationType.S1Larger)
		{
			V1 = true,
			V2 = true,
			D1 = true,
			D2 = true,
			P1 = true,
			P2 = true,
			H1 = true,
			H2 = true,
		},
		new DisplayConfiguration(InputVariant.BernoulliEquationWithHeightDecrease, DiameterRelationType.S2Larger)
		{
			V1 = true,
			V2 = true,
			D1 = true,
			D2 = true,
			P1 = true,
			P2 = true,
			H1 = true,
			H2 = true,
		},
		new DisplayConfiguration(InputVariant.RealFluidMovement, DiameterRelationType.Equal)
		{
			D = true,
			L = true,
			V = true,
			Re = true,
			DeltaP = true,
		},
	};
}
