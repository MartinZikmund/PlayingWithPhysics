using System;
using System.IO;
using System.Threading.Tasks;
using Physics.HuygensPrinciple.Logic;
using Physics.Shared.UI.Localization;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI;

namespace Physics.HuygensPrinciple.ViewModels
{
	public class DemoScenarioViewModel
	{
		private BitmapScenePreset _scenePreset;

		public DemoScenarioViewModel(DemoScenario scenario)
		{
			Scenario = scenario;
		}

		public DemoScenario Scenario { get; }

		public string Name => Localizer.Instance.GetString($"Demo_{Scenario.Key}_Name");

		public string Description => Localizer.Instance.GetString($"Demo_{Scenario.Key}_Description");

		public async Task<ScenePreset> ToPresetAsync()
		{
			if (_scenePreset is null)
			{
				// Load bitmap
				var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///Assets/Demo/{Scenario.Key}.png", UriKind.Absolute));
				Stream imagestream = await file.OpenStreamForReadAsync();

				var dec = await BitmapDecoder.CreateAsync(imagestream.AsRandomAccessStream());

				var data = await dec.GetPixelDataAsync();

				var bytes = data.DetachPixelData();

				var huygensField = new HuygensField((int)dec.PixelWidth, (int)dec.PixelHeight);
				for (int y = 0; y < dec.PixelHeight; y++)
				{
					for (int x = 0; x < dec.PixelWidth; x++)
					{
						var pixel = GetPixel(bytes, x, y, dec.PixelWidth, dec.PixelHeight);
						if (pixel.R < 150)
						{
							huygensField[x, y] = CellState.Wall;
						}
					}
				}

				_scenePreset = new BitmapScenePreset(huygensField, Scenario.Key);
			}
			
			return _scenePreset;
		}

		public Color GetPixel(byte[] pixels, int x, int y, uint width, uint height)
		{
			int i = y;
			int j = x;
			int k = (i * (int)width + j) * 4;
			var b = pixels[k + 0];
			var g = pixels[k + 1];
			var r = pixels[k + 2];
			var a = pixels[k + 3];
			return Color.FromArgb(a, r, g, b);
		}
	}
}
