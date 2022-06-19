using Physics.Shared.UI.Infrastructure.Topics;
using Physics.GravitationalFieldMovement.Rendering;
using Physics.GravitationalFieldMovement.Logic;
using System.Collections.Generic;
using Physics.Shared.Mathematics;
using SkiaSharp;
using ColorHelper = Microsoft.Toolkit.Uwp.Helpers.ColorHelper;
using SkiaSharp.Views.UWP;
using Physics.GravitationalFieldMovement.Services;
using System;
using Windows.UI.Xaml;
using Physics.Shared.UI.Localization;
using Physics.Shared.Helpers;

namespace Physics.GravitationalFieldMovement.ViewModels;

public class DemoViewModel : MainViewModel
{
	public DemoViewModel(IAppPreferences appPreferences) : base(appPreferences)
	{
		SelectedDemo = DemoList[0];
	}

	public List<DemoItemViewModel> DemoList { get; } = new List<DemoItemViewModel>()
	{
		new DemoItemViewModel(Localizer.Instance.GetString("FirstCosmicVelocity"), new InputConfiguration(new BigNumber(6.38, 6), new BigNumber(5.97, 24), 2, 7900.229, 0, 90), PlanetPresets.Earth),
		new DemoItemViewModel(Localizer.Instance.GetString("EllipticTrajectoryOfHalleysComet"), new InputConfiguration(new BigNumber(6.96, 8), new BigNumber(1.99, 30), new BigNumber(5.25, 12), 912, 0, 90), PlanetPresets.Sun),
		new DemoItemViewModel(Localizer.Instance.GetString("HyperbolicTrajectory"), new InputConfiguration(new BigNumber(6.96, 8), new BigNumber(1.99, 30), new BigNumber(5, 11), new BigNumber(4, 4), -75, 90), PlanetPresets.Sun)
	};

	public DemoItemViewModel SelectedDemo { get; set; }

	public void OnSelectedDemoChanged()
	{
		if (SelectedDemo?.Input == null)
		{
			return;
		}
		StartSimulation();
	}

	protected override void StartSimulation()
	{
		if (_controller == null || SelectedDemo?.Input == null)
		{
			return;
		}

		//_controller.Planet = SelectedPreset;
		_controller.SimulationTime.Reset();
		Input = null;
		MaxT = SelectedDemo.Input.Dt * 3600;
		Input = SelectedDemo.Input;
		SelectedPreset = SelectedDemo.Planet;
		_controller.Planet = SelectedDemo.Planet;
		_controller.SetInputConfiguration(Input, Dt);
		_controller.Play();
	}

	protected override void LoadDefaultSimulation()
	{
		MaxT = SelectedDemo.Input.Dt * 3600;
		Input = SelectedDemo.Input;
		SelectedPreset = SelectedDemo.Planet;
		_controller.Planet = SelectedDemo.Planet;		
	}

	public new string HeightText
	{
		get
		{
			if (SelectedDemo == DemoList[1] || SelectedDemo == DemoList[2])
			{
				var heightInAu = MathHelpers.MetersToAstronomicalUnits(Height + Input.Rz);
				return $"{new BigNumber(heightInAu)} au";
			}
			return $"{Math.Round(Height + Input.Rz, 0)} m";
		}
	}
}
