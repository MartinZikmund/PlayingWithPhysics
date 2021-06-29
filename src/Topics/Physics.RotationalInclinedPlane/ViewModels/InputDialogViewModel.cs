using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Physics.RotationalInclinedPlane.Logic;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Localization;
using Physics.Shared.UI.Models.Input;
using Physics.Shared.ViewModels;
using Windows.UI;
using Windows.UI.Popups;
using ColorHelper = Microsoft.Toolkit.Uwp.Helpers.ColorHelper;

namespace Physics.RotationalInclinedPlane.ViewModels
{
	public class InputDialogViewModel : ViewModelBase
    {
		public InputDialogViewModel(DifficultyOption difficulty)
		{
			Difficulty = difficulty;
			OnGravityChanged();
		}

		public InputDialogViewModel(MotionSetup setup, DifficultyOption difficulty) : this(difficulty)
		{
			if (setup != null)
			{
				BodyType = setup.BodyType;
				SelectedBodyTypeIndex = (int)setup.BodyType;
				Radius = setup.Radius;
				InclinedAngle = setup.InclinedAngle;
				InclinedLength = setup.InclinedLength;
				Gravity = setup.Gravity;		
				Color = ColorHelper.ToColor(setup.Color);
				Mass = setup.Mass;
			}
		}

		internal async Task<bool> ValidateAsync()
		{
			if (Radius * 10 > InclinedLength)
			{
				var dialog = new MessageDialog(Localizer.Instance.GetString("PlaneLengthValidationMessage"), Localizer.Instance.GetString("PlaneLengthValidationTitle"));
				await dialog.ShowAsync();
				return false;
			}
			return true;
		}

		public DifficultyOption Difficulty { get; }

		public bool IsAdvanced => Difficulty == DifficultyOption.Advanced;

		public Color[] AvailableColors { get; } = new Color[]
		{
			ColorHelper.ToColor("#0063B1"),
			ColorHelper.ToColor("#2D7D9A"),
			ColorHelper.ToColor("#E81123"),
			ColorHelper.ToColor("#881798"),
			ColorHelper.ToColor("#498205"),
			ColorHelper.ToColor("#515C6B"),
		};

		public ObservableCollection<GravityDefault> GravityDefaults { get; } = new ObservableCollection<GravityDefault>()
		{
			new GravityDefault(Localizer.Instance["ManualInput"], null),
			new GravityDefault(Localizer.Instance["Gravity_EarthEquatorSeaLevel"], 9.78f),
			new GravityDefault(Localizer.Instance["Gravity_Earth45"], 9.80665f),
			new GravityDefault(Localizer.Instance["Gravity_EarthPole"], 9.832f),
			new GravityDefault(Localizer.Instance["Gravity_EarthPrague"], 9.81373f),
			new GravityDefault(Localizer.Instance["Gravity_Sun"], 274f),
			new GravityDefault(Localizer.Instance["Gravity_Mercury"], 3.7f),
			new GravityDefault(Localizer.Instance["Gravity_Venus"], 8.87f),
			new GravityDefault(Localizer.Instance["Gravity_Mars"], 3.71f),
			new GravityDefault(Localizer.Instance["Gravity_Jupiter"], 23.12f),
			new GravityDefault(Localizer.Instance["Gravity_Saturn"], 8.96f),
			new GravityDefault(Localizer.Instance["Gravity_Uranus"], 8.69f),
			new GravityDefault(Localizer.Instance["Gravity_Neptune"], 11f),
			new GravityDefault(Localizer.Instance["Gravity_Moon"], 1.62f)
		};

		public float Mass { get; set; } = 1;

		public float InclinedLength { get; set; } = 5;

		public float InclinedAngle { get; set; } = 45;

		public int MaximumAngle => 45;

		public float Gravity { get; set; } = 9.806f;

		public void OnGravityChanged()
		{
			if (GravityDefaults.FirstOrDefault(d => d.HasValue && Math.Abs(d.Value.Value - Gravity) < 0.001f) is { } gravity)
			{
				SelectedGravity = gravity;
			}
			else
			{
				SelectedGravity = GravityDefaults[0];
			}
		}

		public int SelectedBodyTypeIndex { get; set; }

		public void OnSelectedBodyTypeIndexChanged()
		{
			BodyType = (BodyType)SelectedBodyTypeIndex;
		}

		public BodyType[] BodyTypes { get; } = Enum.GetValues(typeof(BodyType)).OfType<BodyType>().ToArray();

		public BodyType BodyType { get; private set; }

		public float Radius { get; set; } = 0.1f;

		public GravityDefault SelectedGravity { get; set; }

		public void OnSelectedGravityChanged()
		{
			if (SelectedGravity != GravityDefaults[0])
			{
				Gravity = SelectedGravity?.Value ?? 9.81f;
			}
		}

		public Color Color { get; set; } = ColorHelper.ToColor("#0063B1");

		public MotionSetup CreateMotionSetup()
		{
			return new MotionSetup(
				BodyType,
				Mass,
				Gravity,
				Radius,
				InclinedLength,
				InclinedAngle,				
				0,
				ColorHelper.ToHex(Color));
		}
	}
}
