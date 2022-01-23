namespace Physics.CyclicProcesses.Logic.Input.Dialog;

public static class InputDialogConfigurations
{
	public static Dictionary<ProcessType, InputDialogConfiguration> Configurations { get; } = new Dictionary<ProcessType, InputDialogConfiguration>()
	{
		{
			ProcessType.Isotermic,
			new()
			{
				IsTVisible = true,
				IsV1Visible = true,
				IsV2Visible = true,
			}
		},
		{
			ProcessType.Isochoric,
			new()
			{
				IsVVisible = true,
				IsT1Visible = true,
				IsT2Visible = true,
			}
		},
		{
			ProcessType.Isobaric,
			new()
			{
				IsPVisible = true,
				IsV1Visible = true,
				IsV2Visible = true,
			}
		},
		{
			ProcessType.Adiabatic,
			new()
			{
				IsP1Visible = true,
				IsV1Visible = true,
				IsV2Visible = true,
			}
		},
		{
			ProcessType.StirlingEngine,
			new()
			{
				IsV1Visible = true,
				IsV2Visible = true,
				IsT12Visible = true,
				IsT34Visible = true,
			}
		},
	};
}
