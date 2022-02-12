using System;
using MvvmCross.ViewModels;
using Physics.HuygensPrinciple.Logic;

namespace Physics.HuygensPrinciple.ViewModels
{
	public class RenderSettingsViewModel : MvxNotifyPropertyChanged
	{
		public RenderSettingsViewModel()
		{
			FieldSize = RenderSettingsDefaults.DefaultFieldSize;
			StepRadius = RenderSettingsDefaults.DefaultStepRadius;
		}

		public float StepRadius { get; set; }

		public int FieldSize { get; set; }
	}
}
