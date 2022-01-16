using System;
using System.Linq;
using MvvmCross;
using MvvmCross.ViewModels;
using Physics.FluidFlow.Logic;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Localization;
using Physics.Shared.UI.Services.Dialogs;
using Physics.Shared.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Physics.FluidFlow.ViewModels
{
	public class SceneConfigurationDialogViewModel : ViewModelBase
	{
		public SceneConfigurationDialogViewModel()
		{
			Fluids = FluidDefinitions.Definitions.Select(f => new FluidDefinitionViewModel(f)).ToArray();
			SelectedFluid = Fluids.First();
				//Label = oscillationInfo.Label;
				//Color = ColorHelper.ToColor(oscillationInfo.Color);
				//Frequency = oscillationInfo.Frequency;
				//Amplitude = oscillationInfo.Amplitude;
				//PhaseInDeg = oscillationInfo.PhaseInDeg;
		}

		public FluidDefinitionViewModel[] Fluids { get; }

		public FluidDefinitionViewModel SelectedFluid { get; set; }

		public async void Save(ContentDialog dialog, ContentDialogButtonClickEventArgs args)
		{
			var deferral = args.GetDeferral();
			try
			{
				//Result = PrepareMotion();
			}
			catch (ArgumentException)
			{
				string errorMessage = Localizer.Instance.GetString("ArgumentExceptionErrorMessage");
				var contentDialogHelper = Mvx.IoCProvider.Resolve<IContentDialogHelper>();
				await contentDialogHelper.ShowAsync(Localizer.Instance.GetString("InvalidInput"), errorMessage);
				args.Cancel = true;
			}
			finally
			{
				deferral.Complete();
			}
		}
	}
}
