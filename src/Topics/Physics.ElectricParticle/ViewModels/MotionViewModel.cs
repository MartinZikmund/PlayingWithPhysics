using Physics.ElectricParticle.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.ElectricParticle.ViewModels
{
	public class MotionViewModel : Physics.Shared.ViewModels.ViewModelBase
	{
		private PhysicsService _physicsService;
		private TrajectoryPoint[] _trajectory;

		public MotionViewModel(ElectricParticleSimulationSetup motion)
		{
			MotionInfo = motion ?? throw new ArgumentNullException(nameof(motion));
			UpdateCurrentValues(0);
		}

		private ElectricParticleSimulationSetup _motionInfo;

		public ElectricParticleSimulationSetup MotionInfo
		{
			get
			{
				return _motionInfo;
			}
			set
			{
				if (value is null)
				{
					throw new ArgumentNullException(nameof(value));
				}
				_physicsService = new PhysicsService(value);
				_trajectory = _physicsService.ComputeTrajectory(600);
				SetProperty(ref _motionInfo, value);
			}
		}

		public void UpdateCurrentValues(long frame)
		{
			frame = Math.Min(_trajectory.Length - 1, frame);
			var deltaT = _physicsService.ComputeDeltaT(600);

			var time = deltaT * frame;

			TimeElapsed = time.ToString();
			CurrentSpeed = _physicsService.ComputeV(time).ToString();
			CurrentX = _physicsService.ComputeX(time).ToString();
			CurrentY = _physicsService.ComputeY(time).ToString();
			Ek = _physicsService.ComputeEk(time).ToString();
			Ep = _physicsService.ComputeEp(time).ToString();
		}

		public string TimeElapsed { get; private set; } = "";

		public string CurrentSpeed { get; private set; } = "";

		public string DistanceTraveled { get; private set; } = "";

		public string CurrentX { get; private set; } = "";

		public string CurrentY { get; private set; } = "";

		public string Ek { get; private set; } = "";

		public string Ep { get; private set; } = "";
	}
}
