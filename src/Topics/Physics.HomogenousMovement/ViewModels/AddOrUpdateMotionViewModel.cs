using System;
using System.Linq;
using Physics.Shared.ViewModels;
using Physics.HomogenousMovement.Logic.PhysicsServices;
using Physics.Shared.Logic.Constants;
using Physics.HomogenousMovement.PhysicsServices;
using System.Numerics;
using Windows.UI;
using ColorHelper = Microsoft.Toolkit.Uwp.Helpers.ColorHelper;
using Windows.UI.Xaml;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Helpers;

namespace Physics.HomogenousMovement.ViewModels
{
    public class AddOrUpdateMotionViewModel : ViewModelBase
    {
        private readonly string[] _existingNames;
        private float _x0;
        private float _y0;
        private float _v0;
        private float _mass = 1;
        private float _angle;
        private float _gravity = PhysicsConstants.EarthGravity;
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
            DisableUnusedInputs();
        }

        public AddOrUpdateMotionViewModel(DifficultyOption difficulty, params string[] existingNames)
        {
            _existingNames = existingNames;
            SelectedMotionIndex = (int)MovementType.FreeFall;
            SetLocalizedAndNumberedLabelName();
            Difficulty = difficulty;
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
            catch (ArgumentException e)
            {
                var resourceManager = ResourceLoader.GetForCurrentView();
				var errorMessage = resourceManager.GetString(e.Message);
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
                bool isEasy = (Difficulty == DifficultyOption.Easy);
                return isEasy ? Visibility.Collapsed : Visibility.Visible;
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
			var generatedName = UniqueNameGenerator.Generate(movementTypeName, _existingNames);
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
