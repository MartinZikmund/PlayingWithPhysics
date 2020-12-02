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

        public MotionViewModel(IMotionSetup motion)
        {
            MotionInfo = motion ?? throw new ArgumentNullException(nameof(motion));
            UpdateCurrentValues(0);
        }

        private IMotionSetup _motionInfo;

        public IMotionSetup MotionInfo
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
                _physicsService = new PhysicsService(value as MotionSetup);
                SetProperty(ref _motionInfo, value);
            }
        }

        public void UpdateCurrentValues(float timeElapsed)
        {
        //    if (timeElapsed > _physicsService.CalculateMaxT())
        //    {
        //        timeElapsed = _physicsService.CalculateMaxT();
        //    }

        //    TimeElapsed = timeElapsed.ToString("0.##");
        //    CurrentSpeed = _physicsService.CalculateV(timeElapsed).ToString("0.##");
        //    DistanceTraveled = _physicsService.CalculateS(timeElapsed).ToString("0.##");
        //    CurrentX = _physicsService.CalculateX(timeElapsed).ToString("0.##");
        //    CurrentY = _physicsService.CalculateY(timeElapsed).ToString("0.##");
        //    Ft = _physicsService.CalculateFt(timeElapsed).ToString("0.##");
        //    Fp = _physicsService.CalculateFp(timeElapsed).ToString("0.##");
        }

        public string TimeElapsed { get; private set; } = "";

        public string CurrentSpeed { get; private set; } = "";

        public string DistanceTraveled { get; private set; } = "";

        public string CurrentX { get; private set; } = "";

        public string CurrentY { get; private set; } = "";

        public string Ft { get; private set; } = "";

        public string Fp { get; private set; } = "";
    }
}
