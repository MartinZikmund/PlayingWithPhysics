using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics.HuygensPrinciple.Logic;
using Windows.UI.Xaml.Shapes;

namespace Physics.HuygensPrinciple.ViewModels
{
	public class DrawingStateViewModel
	{
		private bool _isDrawing = false;

		public bool IsDrawing
		{
			get => _isDrawing;
			set
			{
				if (_isDrawing != value)
				{
					IsDrawingChanged?.Invoke(this, EventArgs.Empty);
					_isDrawing = value;
				}
			}
		}
		public ShapeType Shape { get; set; } = ShapeType.Circle;

		public float Size { get; set; } = 10;

		public bool IsSource { get; set; } = true;

		public event EventHandler IsDrawingChanged;
	}
}
