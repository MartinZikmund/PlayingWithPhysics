using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.Shared.Services.Sounds
{
    public interface ISoundPlayer
    {
        Task PreloadSoundAsync(Uri sound, string name);

        void PlaySound(string name, double volume = 1.0);
    }
}
