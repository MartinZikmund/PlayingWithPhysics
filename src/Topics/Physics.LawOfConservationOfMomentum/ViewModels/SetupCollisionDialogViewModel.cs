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
			Configuration = VariantConfigurations.All[0];
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

		public VariantConfiguration Configuration { get; set; }
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
		public float MassOne { get; set; }
		public float MassTwo { get; set; }
		public float VelocityOne { get; set; }
		public float VelocityTwo { get; set; }
		public float CoefficientOfRestitution { get; set; }

		public List<CollisionSubtype> Subtypes { get; } = new List<CollisionSubtype>
		{
			CollisionSubtype.V2ZeroM2BiggerThanM1,
			CollisionSubtype.V2Zero,
			CollisionSubtype.SpeedsSameDirection,
			CollisionSubtype.SpeedsOppositeDirection
		};

		public int SelectedSubtypeIndex { get; set; } = 0;

		public void OnSelectedSubtypeIndexChanged()
		{
			if (SelectedSubtypeIndex <= 0)
			{
				return;
			}
			Configuration = VariantConfigurations.All.FirstOrDefault(c => c.Subtype == (CollisionSubtype)SelectedSubtypeIndex);
		}

		public bool CoefficientOfRestitutionVisibility => _variant == CollisionType.ImperfectlyElastic;

		public MotionSetup Result;
		public string Label { get; set; }
	}
}
