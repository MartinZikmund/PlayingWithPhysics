namespace Physics.ElectricParticle.Logic
{
	public class ParticleSetup
    {
		public ParticleSetup(
			ParticleType type,
			Polarity polarity,
			float chargeMultiplier,
			float massMultiplier,
			float startVelocity,
			float startVelocityDeviation)
		{
			Type = type;
			Polarity = polarity;
			ChargeMultiplier = chargeMultiplier;
			MassMultiplier = massMultiplier;
			StartVelocity = startVelocity;
			StartVelocityDeviation = startVelocityDeviation;
		}

		public ParticleType Type { get; set; }

		public Polarity Polarity { get; set; }

		public float ChargeMultiplier { get; set; }

		public float MassMultiplier { get; set; }

		public float StartVelocity { get; set; }

		public float StartVelocityDeviation { get; set; }
	}
}
