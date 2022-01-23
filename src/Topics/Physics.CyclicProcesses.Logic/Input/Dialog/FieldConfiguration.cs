namespace Physics.CyclicProcesses.Logic.Input.Dialog;

public class FieldConfiguration
{
	private FieldConfiguration(bool isVisible) => IsVisible = isVisible;

	private FieldConfiguration(double minimum, double maximum, float? defaultValue, float? step = null)
	{
		Minimum = minimum;
		Maximum = maximum;
		DefaultValue = defaultValue;
		Step = step;
	}

	public static FieldConfiguration CreateInvisible() =>
		new FieldConfiguration(false);

	public static FieldConfiguration CreateRestricted(double minimum, double maximum, float? defaultValue = null, float? step = null) =>
		new FieldConfiguration(minimum, maximum, defaultValue, step);

	public static FieldConfiguration CreateUnrestricted(float? defaultValue = null) =>
		new FieldConfiguration(double.MinValue, double.MaxValue, defaultValue);

	public bool IsVisible { get; set; } = true;

	public double Minimum { get; set; }

	public double Maximum { get; set; }

	public float? DefaultValue { get; set; }

	public float? Step { get; set; }
}
