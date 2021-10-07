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

		private string[] _existingNames;
		public SetupCollisionDialogViewModel(CollisionType variant)
		{
			_variant = variant;
			Configuration = VariantConfigurations.All[0];
		}

		public SetupCollisionDialogViewModel(CollisionType variant, MotionSetup setup) : this(variant)
		{
			SelectedSubtypeIndex = (int)setup.Subtype;
			MassOne = setup.M1;
			MassTwo = setup.M2;
			VelocityOne = setup.V1;
			VelocityTwo = setup.V2;
			CoefficientOfRestitution = setup.CoefficientOfRestitution;
		}

		public async void Save(ContentDialog dialog, ContentDialogButtonClickEventArgs args)
		{
			PrepareMotion(); //Fills in Result
		}


		public void PrepareMotion()
		{
			//Fill in Result
			Result = new MotionSetup(
				_variant,
				(CollisionSubtype)SelectedSubtypeIndex,
				VelocityOne,
				MassOne,
				VelocityTwo,
				MassTwo,
				CoefficientOfRestitution);
		}

		public VariantConfiguration Configuration { get; set; }
		public Visibility IsEasyVariant { get; set; }
		public float MassOne { get; set; } = 1;
		public float MassTwo { get; set; } = 1;
		public float VelocityOne { get; set; } = 1;
		public float VelocityTwo { get; set; } = 1;
		public float CoefficientOfRestitution { get; set; } = 1;

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
	}
}
