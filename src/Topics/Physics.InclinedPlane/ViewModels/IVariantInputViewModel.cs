﻿using Physics.InclinedPlane.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.InclinedPlane.ViewModels
{
    public interface IVariantInputViewModel
    {
        Task<IInclinedPlaneMotionSetup> CreateMotionSetupAsync();
    }
}
