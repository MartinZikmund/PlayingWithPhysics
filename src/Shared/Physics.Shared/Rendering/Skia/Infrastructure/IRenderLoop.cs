using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.Shared.UI.Rendering.Skia.Infrastructure
{
    public interface IRenderLoop
    {
        void Start(Action action);
        void Stop();
    }
}
