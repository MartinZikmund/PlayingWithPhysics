using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.LissajousCurves.Logic
{
	public interface IOscillationPhysicsService
	{
		float CalculateY(float timeInSeconds);

		float CalculateA(float timeInSeconds);

		float CalculateV(float timeInSeconds);
	}
}
