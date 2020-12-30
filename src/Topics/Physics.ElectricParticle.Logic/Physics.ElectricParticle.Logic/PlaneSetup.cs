namespace Physics.ElectricParticle.Logic
{
	public class PlaneSetup
    {
		public PlaneSetup(Polarity polarity, float voltage, float distance)
		{
			Polarity = polarity;
			Voltage = voltage;
			Distance = distance;		
		}

		public Polarity Polarity { get; set; }

		public float Voltage { get; set; }

		public float Distance { get; set; }
	}
}
