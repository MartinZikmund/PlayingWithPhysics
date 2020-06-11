using Physics.InclinedPlane.Services;
using Physics.InclinedPlane.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics.InclinedPlane.Logic.PhysicsServices;

namespace Physics.InclinedPlane.ViewModels
{
    public class MotionViewModel : Physics.Shared.ViewModels.ViewModelBase
    {
        private IPhysicsService _physicsService;
        public MotionViewModel(IInclinedPlaneMotionSetup motion)
        {
            MotionInfo = motion ?? throw new ArgumentNullException(nameof(motion));
            UpdateCurrentValues(0);
        }

        private IInclinedPlaneMotionSetup _motionInfo;

        public IInclinedPlaneMotionSetup MotionInfo
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

            TimeElapsed = timeElapsed;
            CurrentSpeed = _physicsService.CalculateV(timeElapsed);
            DistanceTraveled = _physicsService.CalculateS(timeElapsed);
            CurrentX = _physicsService.CalculateX(timeElapsed);
            CurrentY = _physicsService.CalculateY(timeElapsed);
        }

        public float TimeElapsed { get; private set; } = 0;

        public float CurrentSpeed { get; private set; } = 0;
        public float DistanceTraveled { get; private set; } = 0;

        public float CurrentX { get; private set; } = 0;

        public float CurrentY { get; private set; } = 0;
    }
}
