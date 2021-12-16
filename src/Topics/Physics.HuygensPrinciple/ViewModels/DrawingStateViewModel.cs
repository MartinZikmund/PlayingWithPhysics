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
					_isDrawing = value;
					IsDrawingChanged?.Invoke(this, EventArgs.Empty);
				}
			}
		}
		public ShapeType Shape { get; set; } = ShapeType.Circle;

		public float Size { get; set; } = 10;

		public CellState SurfaceType { get; set; } = CellState.Source;

		public event EventHandler IsDrawingChanged;
	}
}
