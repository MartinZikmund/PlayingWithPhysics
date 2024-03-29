﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.DragMovement.Logic.PhysicsServices
{
    public interface IPhysicsService
    {
        float ComputeX(float timeMoment);
        float ComputeY(float timeMoment);
        float ComputeV(float timeMoment);
        float ComputeF(float timeMoment);
        float ComputeAcceleration(float timeMoment);
        float MaxX { get; }
        float MaxT { get; }
        float MaxY { get; }
        float Resistance { get; }
        MotionInfo MotionInfo { get; }

        //TrajectoryData CreateTrajectoryData();
    }
}
