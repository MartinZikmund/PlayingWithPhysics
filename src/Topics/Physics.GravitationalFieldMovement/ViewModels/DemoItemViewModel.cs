using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Converters;
using Physics.GravitationalFieldMovement.Logic;
using SkiaSharp;
using Windows.Storage;

namespace Physics.GravitationalFieldMovement.ViewModels
{
	public class DemoItemViewModel
	{
		public string FriendlyName { get; }
		public InputConfiguration Input { get; }
		public SKColor Color { get; } = SKColors.LightGray;
		public DemoItemViewModel(string name, InputConfiguration input, SKColor? color)
		{
			FriendlyName = name;
			Input =	input;
			if (color != null)
			{
				Color = color.Value;
			}
		}
	}
}
