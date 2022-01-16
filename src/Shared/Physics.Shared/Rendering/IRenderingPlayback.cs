using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.Shared.UI.Rendering
{
    public interface IRenderingPlayback
    {
        SimulationTime SimulationTime { get; }
        bool IsPaused { get; }

        void FastForward(float jumpSize);
        void Rewind(float jumpSize);
        void Pause();
        void Play();

		event EventHandler PlayStateChanged;
    }
}
