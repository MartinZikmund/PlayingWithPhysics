using System;
using Microsoft.Graphics.Canvas.UI;

namespace Physics.Shared.UI.Rendering
{
    public class SimulationTime
    {
        public TimeSpan TotalTime { get; private set; } = TimeSpan.Zero;

        public TimeSpan ElapsedTime { get; private set; } = TimeSpan.Zero;

        public long UpdateCount { get; private set; }

        public float SimulationSpeed { get; set; } = 1.0f;

        public void Restart()
        {            
            TotalTime = TimeSpan.Zero;
        }

        public void Rewind(float time)
        {
            TotalTime = TimeSpan.FromSeconds(Math.Max(0.0, TotalTime.TotalSeconds - time));
        }

        public void FastForward(float time)
        {
            TotalTime = TimeSpan.FromSeconds(Math.Max(0.0, TotalTime.TotalSeconds + time));
        }

        internal void UpdateFromCanvas(CanvasTimingInformation updateTimingInformation)
        {
            ElapsedTime = updateTimingInformation.ElapsedTime * SimulationSpeed;
            TotalTime += ElapsedTime;
            UpdateCount = updateTimingInformation.UpdateCount;
        }
    }
}
