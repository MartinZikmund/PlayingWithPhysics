using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using MvvmCross.ViewModels;
using Physics.HomogenousMovement.Logic.PhysicsServices;

namespace Physics.HomogenousMovement.ViewModels
{
    public class ThrowTrajectory : MvxNotifyPropertyChanged
    {
        public ThrowTrajectory(string label, Color color, TrajectoryData data) : this(data)
        {
            Label = label;
            Color = color;
        }

        public ThrowTrajectory(TrajectoryData data)
        {
            Data = data;
        }

        public string Label { get; set; } = "";

        public Color Color { get; set; } = Colors.Black;

        public TrajectoryData Data { get; }
    }
}
