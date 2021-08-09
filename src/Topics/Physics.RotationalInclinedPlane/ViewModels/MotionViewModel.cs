using System;
using Physics.RotationalInclinedPlane.Logic;

namespace Physics.RotationalInclinedPlane.ViewModels
{
	public class MotionViewModel : Physics.Shared.ViewModels.ViewModelBase
	{
		private PhysicsService _physicsService;

		public MotionViewModel(MotionSetup motion)
		{
			MotionInfo = motion ?? throw new ArgumentNullException(nameof(motion));
			UpdateCurrentValues(0);
		}

		private MotionSetup _motionInfo;

		public MotionSetup MotionInfo
		{
			get
			{
				return _motionInfo;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value));
				}
				_physicsService = new PhysicsService(value);
				SetProperty(ref _motionInfo, value);
			}
		}

		public void UpdateCurrentValues(float timeElapsed)
		{
			if (timeElapsed > _physicsService.CalculateMaxT())
			{
				timeElapsed = _physicsService.CalculateMaxT();
			}

			TimeElapsed = timeElapsed.ToString("0.##");
			CurrentSpeed = _physicsService.CalculateVelocity(timeElapsed).ToString("0.##");
			DistanceTraveled = _physicsService.CalculateDistance(timeElapsed).ToString("0.##");
			CurrentX = _physicsService.CalculateX(timeElapsed).ToString("0.##");
			CurrentY = _physicsService.CalculateY(timeElapsed).ToString("0.##");
			Ek = _physicsService.CalculateEk(timeElapsed).ToString("0.##");
			Ep = _physicsService.CalculateEp(timeElapsed).ToString("0.##");
			Er = _physicsService.CalculateEr(timeElapsed).ToString("0.##");
		}

		public string Label => MotionInfo.Label;

		public string TimeElapsed { get; private set; } = "";

		public string CurrentSpeed { get; private set; } = "";

		public string DistanceTraveled { get; private set; } = "";

		public string CurrentX { get; private set; } = "";

		public string CurrentY { get; private set; } = "";

		public string Ek { get; private set; } = "";

		public string Ep { get; private set; } = "";

		public string Er { get; private set; } = "";
	}
}
