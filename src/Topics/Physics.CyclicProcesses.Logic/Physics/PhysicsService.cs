using Physics.CyclicProcesses.Logic.Input;

namespace Physics.CyclicProcesses.Logic.Physics;

public abstract class PhysicsService : IPhysicsService
{
	protected const float CycleLengthInSeconds = 10f;

	/// <summary>
	/// Poisson constant.
	/// </summary>
	protected const float K = 5 / 3.0f;

	/// <summary>
	/// Gas constant.
	/// </summary>
	protected const float R = 8.3145f;

	/// <summary>
	/// Molar temperature capacity with constant pressure.
	/// </summary>
	protected const float Cp = 5 * R / 2;

	/// <summary>
	/// Molar temperature capacity with constant volume.
	/// </summary>
	protected const float Cv = 3 * R / 2;

	private IInputConfiguration _input;

	protected PhysicsService(IInputConfiguration input)
	{
		_input = input ?? throw new ArgumentNullException(nameof(input));
	}

	public abstract ProcessType Process { get; }

	public abstract float CalculateP(float time);

	public abstract float CalculateV(float time);

	protected float CalculateCycleValue(float minValue, float maxValue, float currentTime, float cycleLengthInSeconds = CycleLengthInSeconds)
	{
		var valueDelta = maxValue - minValue;
		var halfCycle = cycleLengthInSeconds / 2;
		var cycleNumber = (int)(currentTime / halfCycle);
		var cyclePosition = currentTime % halfCycle;
		var relativeCyclePosition = cyclePosition / halfCycle;
		if (cycleNumber % 2 == 0)
		{
			// From 0 up
			return minValue + relativeCyclePosition * valueDelta;
		}
		else
		{
			// From 1 down
			return maxValue - relativeCyclePosition * valueDelta;
		}
	}
}
