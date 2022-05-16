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

namespace Physics.GravitationalFieldMovement.ViewModels;

public class DemoViewModel : MainViewModel
{
	public DemoViewModel(IAppPreferences appPreferences) : base(appPreferences)
	{
		SelectedDemo = DemoList[0];
	}

	public List<DemoItemViewModel> DemoList { get; } = new List<DemoItemViewModel>()
	{
		new DemoItemViewModel("1. kosmická rychlost", new InputConfiguration(new BigNumber(6.38, 6), new BigNumber(5.97, 24), 2, 7900.229, 0, 90), ColorHelper.ToColor(PlanetPresets.Presets[0].ColorHex).ToSKColor()),
		new DemoItemViewModel("Eliptická dráha Halleovy komety", new InputConfiguration(new BigNumber(6.96, 8), new BigNumber(1.99, 30), new BigNumber(5.25, 12), 912, 0, 90), SKColors.Red),
		new DemoItemViewModel("Hyperbolická trajektorie", new InputConfiguration(new BigNumber(6.96, 8), new BigNumber(1.99, 30), new BigNumber(5, 11), new BigNumber(4, 4), -75, 90), SKColors.Red)
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

	private void StartSimulation()
	{
		if (_controller == null || SelectedDemo?.Input == null)
		{
			return;
		}

		//_controller.Planet = SelectedPreset;
		_controller.SimulationTime.Reset();
		Input = null;
		Dt = SelectedDemo.Input.Dt;
		Input = SelectedDemo.Input;		
		_controller.PlanetPaint.Color = SelectedDemo.Color;
		_controller.SetInputConfiguration(Input, Dt);
		_controller.Play();
	}

	protected override void LoadDefaultSimulation()
	{
		Dt = SelectedDemo.Input.Dt;
		Input = SelectedDemo.Input;
		_controller.PlanetPaint.Color = SelectedDemo.Color;
	}
}
