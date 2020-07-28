﻿using Physics.DragMovement.Logic.PhysicsServices;
using Physics.Shared.Logic.Constants;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Localization;
using Physics.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ColorHelper = Microsoft.Toolkit.Uwp.Helpers.ColorHelper;

namespace Physics.DragMovement.ViewModels
{
    public class AddOrUpdateMotionViewModel : ViewModelBase
    {
        private readonly string[] _existingNames;
        private float _x0;
        private float _y0;
        private float _v0;
        private float _mass = 1;
        private float _angle;
        private float _area;
        private float _gravity = GravityConstants.Earth;
        private Color _color = ColorHelper.ToColor("#0063B1");
        private bool _autogenerateLabel = true;

        public AddOrUpdateMotionViewModel(MotionInfo motionInfo, DifficultyOption difficulty, params string[] existingNames)
        {
            _existingNames = existingNames;
            _autogenerateLabel = false; // Do not autogenerate for edit mode.
            Label = motionInfo.Label;
            Gravity = motionInfo.G;
            Color = ColorHelper.ToColor(motionInfo.Color);
            V0 = motionInfo.V0;
            Angle = motionInfo.Angle;
            X0 = motionInfo.Origin.X;
            Y0 = motionInfo.Origin.Y;
            Mass = motionInfo.Mass;
            SelectedMotionIndex = (int)motionInfo.Type;
            Difficulty = difficulty;
            SelectedResistanceCoefficient = ResistanceCoefficients[0];
            DisableUnusedInputs();
        }

        public AddOrUpdateMotionViewModel(DifficultyOption difficulty, params string[] existingNames)
        {
            _existingNames = existingNames;
            SelectedMotionIndex = (int)MovementType.FreeFall;
            SetLocalizedAndNumberedLabelName();
            Difficulty = difficulty;
            SelectedResistanceCoefficient = ResistanceCoefficients[0];
            //Set localized and numbered label text
            var resLoader = ResourceLoader.GetForCurrentView();
            var movementType = (MovementType)SelectedMotionIndex;
            SetLocalizedAndNumberedLabelName();
            DisableUnusedInputs();
        }

        public ICommand SaveCommand => GetOrCreateAsyncCommand<ContentDialogButtonClickEventArgs>(SaveAsync);

        private async Task SaveAsync(ContentDialogButtonClickEventArgs args)
        {
            var deferral = args.GetDeferral();
            try
            {
                ResultMotionInfo = PrepareMotion();
                ResultMotionInfo.Label = Label;
            }
            catch (ArgumentException)
            {
                var resourceManager = ResourceLoader.GetForCurrentView();
                string errorMessage = resourceManager.GetString("ArgumentExceptionErrorMessage");
                var messageDialog = new MessageDialog(resourceManager.GetString("InvalidInput"), errorMessage);
                await messageDialog.ShowAsync();
                args.Cancel = true;
            }
            finally
            {
                deferral.Complete();
            }
        }

        public MotionInfo ResultMotionInfo { get; set; }

        public DifficultyOption Difficulty { get; set; }

        public Visibility IsProjectileMotionCheckBoxEnabled
        {
            get
            {
                bool isAdvanced = (Difficulty == DifficultyOption.Advanced);
                return isAdvanced ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public Visibility IsFreeFallMotionCheckBoxEnabled
        {
            get
            {
                bool isAdvanced = (Difficulty == DifficultyOption.Advanced);
                return isAdvanced ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public Visibility IsSpeedEnabled
        {
            get
            {
                bool isAdvanced = (Difficulty == DifficultyOption.Advanced);
                return isAdvanced ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public Visibility IsElevationAngleEnabled
        {
            get
            {
                bool isAdvanced = (Difficulty == DifficultyOption.Advanced);
                return isAdvanced ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public int SelectedMotionIndex { get; set; }

        public void OnSelectedMotionIndexChanged()
        {
            MovementType = (MovementType)SelectedMotionIndex;
            if (_autogenerateLabel)
            {
                SetLocalizedAndNumberedLabelName();
            }
        }

        private void SetLocalizedAndNumberedLabelName()
        {
            var movementTypeName = ResourceLoader.GetForCurrentView().GetString(MovementType.ToString());
            var currentId = 0;

            string generatedName;
            do
            {
                generatedName = $"{movementTypeName} #{++currentId}";
            } while (_existingNames.Contains(generatedName));

            Label = generatedName;
        }

        public MovementType MovementType { get; set; }

        public void OnMovementTypeChanged()
        {
            if (Enum.IsDefined(typeof(MovementType), MovementType))
            {
                TryUpdateLabel();
                DisableUnusedInputs();
            }
        }

        private void TryUpdateLabel()
        {
            var resourceLoader = ResourceLoader.GetForCurrentView();
            var movementTypes = Enum.GetValues(typeof(MovementType)).OfType<MovementType>();
            foreach (var movementType in movementTypes)
            {
                if (Label.StartsWith(resourceLoader.GetString(movementType.ToString())))
                {
                    // user has probably not modified the name, we can update according to movement type
                    SetLocalizedAndNumberedLabelName();
                }
            }

        }

        public bool IsV0Enabled { get; set; }
        public bool IsMassEnabled { get; set; }
        public bool IsAngleEnabled { get; set; }
        public bool IsX0Enabled { get; set; }
        public bool IsY0Enabled { get; set; }

        public string Label { get; set; }

        public Color[] AvailableColors { get; } = new Color[]
        {
            ColorHelper.ToColor("#0063B1"),
            ColorHelper.ToColor("#2D7D9A"),
            ColorHelper.ToColor("#E81123"),
            ColorHelper.ToColor("#881798"),
            ColorHelper.ToColor("#498205"),
            ColorHelper.ToColor("#515C6B"),
        };

        public void OnResistanceCoefficientChanged()
        {
            var selectedValue = SelectedResistanceCoefficient?.Value ?? -1;
            if (Math.Abs(ResistanceCoefficient - selectedValue) > 0.01)
            {
                SelectedResistanceCoefficient = ResistanceCoefficients[0];
            }
        }

        public ObservableCollection<ResistanceCoefficient> ResistanceCoefficients { get; } = new ObservableCollection<ResistanceCoefficient>()
        {
            new ResistanceCoefficient(Localizer.Instance["NoResistance"], 0f),
            new ResistanceCoefficient(Localizer.Instance["AerodynamicShape"], 0.037f),
            new ResistanceCoefficient(Localizer.Instance["Ball"], 0.5f),
            new ResistanceCoefficient(Localizer.Instance["ThinLayer"], 1.2f),
            new ResistanceCoefficient(Localizer.Instance["Parachute"], 1.3f)
        };

        public ResistanceCoefficient SelectedResistanceCoefficient { get; set; }

        public void OnSelectedResistanceCoefficientChanged()
        {
            //Ball selected, enable density and diameter, disable weight, area
            if (SelectedResistanceCoefficient == ResistanceCoefficients[2])
            {
                SetInputsForBall();
            } else
            {
                SetInputsForOtherShapes();
            }
            ResistanceCoefficient = SelectedResistanceCoefficient?.Value ?? 0f;
        }

        public void SetInputsForBall()
        {
            IsDensityInputEnabled = Visibility.Visible;
            IsDiameterInputEnabled = Visibility.Visible;
            IsAreaInputEnabled = Visibility.Collapsed;
            IsMassInputEnabled = Visibility.Collapsed;
        }
        
        public void SetInputsForOtherShapes()
        {
            IsDensityInputEnabled = Visibility.Collapsed;
            IsDiameterInputEnabled = Visibility.Collapsed;
            IsAreaInputEnabled = Visibility.Visible;
            IsMassInputEnabled = Visibility.Visible;
        }

        public float Diameter { get; set; }
        public float Density { get; set; }

        public Visibility IsDensityInputEnabled { get; set;  }
        public Visibility IsDiameterInputEnabled { get; set;  }
        public Visibility IsAreaInputEnabled { get; set;  }
        public Visibility IsMassInputEnabled { get; set;  }

        public float ResistanceCoefficient { get; set; } = 0f;

        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                RaisePropertyChanged();
            }
        }

        public float X0
        {
            get => _x0;
            set
            {
                if (!float.IsNaN(value) && value != _x0)
                {
                    _x0 = value;
                    RaisePropertyChanged();
                }

            }
        }

        public float Y0
        {
            get => _y0;
            set
            {
                if (!float.IsNaN(value) && value != _y0)
                {
                    _y0 = value;
                    RaisePropertyChanged();
                }

            }
        }

        public float Mass
        {
            get => _mass;
            set
            {
                if (!float.IsNaN(value) && value != _mass)
                {
                    _mass = value;
                    RaisePropertyChanged();
                }
            }
        }

        public float V0
        {
            get => _v0;
            set
            {
                if (!float.IsNaN(value) && value != _v0)
                {
                    _v0 = value;
                    RaisePropertyChanged();
                }
            }
        }

        public float Angle
        {
            get => _angle;
            set
            {
                if (!float.IsNaN(value) && value != _angle)
                {
                    _angle = value;
                    RaisePropertyChanged();
                }
            }
        }

        public float Gravity
        {
            get => _gravity;
            set
            {
                if (!float.IsNaN(value) && value != _gravity)
                {
                    _gravity = value;
                    RaisePropertyChanged();
                }
            }
        }

        public float Area
        {
            get => _area;
            set
            {
                if (!float.IsNaN(value) && value != _area)
                {
                    _area = value;
                    RaisePropertyChanged();
                }
            }
        }

        private void DisableUnusedInputs()
        {
            switch (MovementType)
            {
                case MovementType.VerticalMotion:
                    IsY0Enabled = true;
                    IsX0Enabled = true;
                    IsV0Enabled = true;
                    IsMassEnabled = true;
                    IsAngleEnabled = false;
                    break;
                case MovementType.HorizontalMotion:
                    IsY0Enabled = true;
                    IsV0Enabled = true;
                    IsX0Enabled = true;
                    IsMassEnabled = true;
                    IsAngleEnabled = false;
                    break;
                case MovementType.ProjectileMotion:
                    IsY0Enabled = true;
                    IsX0Enabled = true;

                    IsV0Enabled = true;
                    IsMassEnabled = true;
                    IsAngleEnabled = true;
                    break;
                default:
                    IsY0Enabled = true;
                    IsX0Enabled = true;
                    IsV0Enabled = false;
                    IsMassEnabled = true;
                    IsAngleEnabled = false;
                    break;
            }
        }

        private MotionInfo PrepareMotion()
            => MovementType switch
            {
                MovementType.FreeFall => MotionFactory.CreateFreeFall(
                    new Vector2(X0, Y0),
                    Mass,
                    0,
                    ColorHelper.ToHex(Color), Gravity),
                MovementType.VerticalMotion =>
                MotionFactory.CreateUpwardMotion(
                    new Vector2(X0, Y0),
                    Mass,
                    0,
                    V0,
                    ColorHelper.ToHex(Color), Gravity),
                MovementType.HorizontalMotion => MotionFactory.CreateHorizontalMotion(
                    new Vector2(X0, Y0),
                    Mass,
                    0,
                    V0,
                    ColorHelper.ToHex(Color), Gravity),
                MovementType.ProjectileMotion => MotionFactory.CreateProjectileMotion(
                    new Vector2(X0, Y0),
                    Mass,
                    0,
                    V0,
                    ColorHelper.ToHex(Color),
                    Angle, Gravity),
                _ => throw new ArgumentNullException()
            };
    }
}