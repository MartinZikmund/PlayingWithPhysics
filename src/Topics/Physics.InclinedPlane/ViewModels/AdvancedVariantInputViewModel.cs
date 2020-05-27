﻿using Physics.InclinedPlane.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.InclinedPlane.ViewModels
{
    public class AdvancedVariantInputViewModel : VariantInputViewModelBase
    {
        public override async Task<IMotionSetup> CreateMotionSetupAsync()
        {
            return new AdvancedMotionSetup(Elevation, Angle, Mass, SelectedDriftCoefficient.Value, Length, Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToHex(Color), SelectedDriftCoefficient.Value);
        }

        private GravityPreset _selectedGravityPreset;

        public GravityPreset SelectedGravityPreset
        {
            get
            {
                return _selectedGravityPreset;
            }
            set
            {
                _selectedGravityPreset = value;
                Gravity = _selectedGravityPreset.Value;
            }
        }
        public ObservableCollection<GravityPreset> GravityPresetDefaults { get; } = new ObservableCollection<GravityPreset>()
        {
            new GravityPreset("Země na rovníku v úrovni mořské hladiny", 9.78f),
            new GravityPreset("Země 45° zeměpisné šířky", 9.80665f),
            new GravityPreset("Země (Zemský pól)", 9.832f),
            new GravityPreset("Země (Praha)", 9.81373f),
            new GravityPreset("Slunce (rovníkové tíhové zrychlení g)", 274f),
            new GravityPreset("Merkur (rovníkové tíhové zrychlení g)", 3.7f),
            new GravityPreset("Venuše (rovníkové tíhové zrychlení g)", 8.87f),
            new GravityPreset("Mars (rovníkové tíhové zrychlení g)", 3.71f),
            new GravityPreset("Jupiter (rovníkové tíhové zrychlení g)", 23.12f),
            new GravityPreset("Saturn (rovníkové tíhové zrychlení g)", 8.96f),
            new GravityPreset("Uran (rovníkové tíhové zrychlení g)", 8.69f),
            new GravityPreset("Neptun (rovníkové tíhové zrychlení g)", 11f)
        };

        public AdvancedVariantInputViewModel()
        {
            SelectedGravityPreset = GravityPresetDefaults[0];
        }

        public float Gravity { get; set; }
        public override string Label { get; set; }
    }
}
