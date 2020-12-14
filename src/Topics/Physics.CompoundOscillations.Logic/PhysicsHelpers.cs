using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.CompoundOscillations.Logic
{
    public static class PhysicsHelpers
    {
		public static float FrequencyToPeriod(float frequency) => 1 / frequency;

		public static float PhaseInRadToDeg(float phaseInRad) => (180 * phaseInRad) / (float)Math.PI;

		public static float FrequencyToAngularSpeedInRad(float frequency) => frequency * 2 * (float)Math.PI;

		public static float FrequencyToAngularSpeedInDeg(float frequency) => frequency * 360;

		public static float AngularSpeedInRadToFrequency(float angularSpeedInRad) => angularSpeedInRad / (2 * (float)Math.PI);

		public static float AngularSpeedInDegToFrequency(float angularSpeedInDeg) => angularSpeedInDeg / 360;
	}
}
