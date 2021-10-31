namespace Physics.LawOfConservationOfMomentum.Logic
{
	public class MotionSetup
	{
		public MotionSetup(CollisionType type, CollisionSubtype subtype, float v1, float m1, float v2, float m2, float coefficientOfRestitution)
		{
			Type = type;
			Subtype = subtype;
			V1 = v1;
			M1 = m1;
			V2 = v2;
			M2 = m2;
			CoefficientOfRestitution = coefficientOfRestitution;
		}

		public CollisionType Type { get; set; }

		public CollisionSubtype Subtype { get; set; }

		public float V1 { get; set; }

		public float V2 { get; set; }

		public float M1 { get; set; }

		public float M2 { get; set; }

		public float CoefficientOfRestitution { get; set; }
	}
}
