using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.ViewModels;
using Physics.LawOfConservationOfMomentum.Logic;
using Physics.Shared.UI.Helpers;
using Physics.Shared.UI.Localization;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ColorHelper = Microsoft.Toolkit.Uwp.Helpers.ColorHelper;

namespace Physics.LawOfConservationOfMomentum.ViewModels
{
	public class SetupCollisionDialogViewModel : MvxNotifyPropertyChanged
	{

		private CollisionType _variant;
		private MotionSetup _setup;

		private string[] _existingNames;
		public SetupCollisionDialogViewModel(CollisionType variant)
		{
			_variant = variant;
		}
		public SetupCollisionDialogViewModel(CollisionType variant, MotionSetup setup) : this(variant)
		{
			_setup = setup;
		}
		public async void Save(ContentDialog dialog, ContentDialogButtonClickEventArgs args)
		{
			PrepareMotion(); //Fills in Result
		}


		public void PrepareMotion()
		{
			//Fill in Result
		}

		private void SetLocalizedAndNumberedLabelName()
		{
			var movementTypeName = Localizer.Instance.GetString("Wave");
			var generatedName = UniqueNameGenerator.Generate(movementTypeName, _existingNames);
			Label = generatedName;
		}

		public float Amplitude { get; set; }
		public float Frequency { get; set; }
		public float Period => 1 / Frequency;
		public string AngularSpeedInDeg => PhysicsHelpers.FrequencyToAngularSpeedInDeg(Frequency).ToString("0.0");
		public string AngularSpeedInRad => PhysicsHelpers.FrequencyToAngularSpeedInRad(Frequency).ToString("0.0");
		public float PhaseInPiRad { get; set; }
		public float PhaseInDeg { get; set; }

		public Color Color { get; set; }
		public Color[] AvailableColors { get; } = new Color[]
{
			ColorHelper.ToColor("#0063B1"),
			ColorHelper.ToColor("#2D7D9A"),
			ColorHelper.ToColor("#E81123"),
			ColorHelper.ToColor("#881798"),
			ColorHelper.ToColor("#498205"),
			ColorHelper.ToColor("#515C6B"),
};
		public Visibility IsEasyVariant { get; set; }

		public MotionSetup Result;
		public string Label { get; set; }
	}
}
