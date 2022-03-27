using System.Numerics;

namespace Physics.ElectricParticle.Game
{
	public class PenState
    {
		public Vector2 Position { get; set; } = new Vector2(0.5f, 0.5f);

		public Vector2 Acceleration { get; set; } = new Vector2(0, 0);

		public Vector2 Speed { get; set; } = new Vector2(0, 0);
	}
}
