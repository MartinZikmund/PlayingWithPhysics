﻿using MvvmCross.Binding.Combiners;
using MvvmCross.ViewModels;
using Physics.HomogenousMovement.Logic.PhysicsServices;
using Physics.HomogenousMovement.PhysicsServices;
using System;

namespace Physics.HomogenousMovement.ViewModels
{
    public class MotionInfoViewModel : MvxNotifyPropertyChanged
    {
        private IPhysicsService _physicsService = null;

        public MotionInfoViewModel(MotionInfo throwInfo)
        {
            MotionInfo = throwInfo ?? throw new ArgumentNullException(nameof(throwInfo));
        }

        private MotionInfo _motionInfo;

        public MotionInfo MotionInfo
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

        public string Label
        {
            get => MotionInfo.Label;
            set
            {
                MotionInfo.Label = value;
                RaisePropertyChanged();
            }
        }

        public void UpdateCurrentValues(float timeElapsed)
        {
            if (timeElapsed > _physicsService.MaxT)
            {
                timeElapsed = _physicsService.MaxT;
            }

            TimeElapsed = timeElapsed.ToString("0.##") + " s";
            CurrentSpeed = _physicsService.ComputeV(timeElapsed).ToString("0.##") + " m/s";
            CurrentX = _physicsService.ComputeX(timeElapsed).ToString("0.##") + " m";
            CurrentY = _physicsService.ComputeY(timeElapsed).ToString("0.##") + " m";
        }

        public string TimeElapsed { get; private set; }
        
        public string CurrentSpeed { get; private set; }

        public string CurrentX { get; private set; }

        public string CurrentY { get; private set; }
    }
}
