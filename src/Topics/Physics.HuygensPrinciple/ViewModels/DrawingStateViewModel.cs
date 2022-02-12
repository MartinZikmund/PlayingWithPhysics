using System;
using System.Linq;
using System.Windows.Input;
using MvvmCross.ViewModels;
using Physics.HuygensPrinciple.Logic;
using Physics.Shared.ViewModels;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Physics.HuygensPrinciple.ViewModels
{
	public class DrawingStateViewModel : ViewModelBase
	{
		public static readonly SolidColorBrush BrushBorder = new SolidColorBrush(Colors.Gray);
		public static readonly SolidColorBrush EmptyBrush = new SolidColorBrush(Colors.White);
		public static readonly SolidColorBrush SourceBrush = new SolidColorBrush(Colors.DarkOrange);
		public static readonly SolidColorBrush WallBrush = new SolidColorBrush(Colors.Brown);

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

		public DrawingTool ActiveTool { get; set; }

		public bool IsBrush => ActiveTool == DrawingTool.Brush;

		public bool IsEraser => ActiveTool == DrawingTool.Eraser;

		public DrawingTool[] DrawingTools { get; } = Enum.GetValues(typeof(DrawingTool)).OfType<DrawingTool>().ToArray();

		public ShapeType Shape { get; set; } = ShapeType.Circle;

		public float Size { get; set; } = RenderSettingsDefaults.DefaultBrushSize;

		public CellState SurfaceType { get; set; } = CellState.Source;

		public event EventHandler IsDrawingChanged;

		public bool CanStart => !IsDrawing;

		public ICommand StartDrawingCommand => GetOrCreateCommand(() => IsDrawing = true);

		public Brush SampleBrush =>
			SurfaceType switch
			{
				CellState.Empty => EmptyBrush,
				CellState.Source => SourceBrush,
				CellState.Wall => WallBrush,
				_ => throw new InvalidOperationException()
			};

		public bool IsSquare => Shape == ShapeType.Square;

		public bool IsCircle => Shape == ShapeType.Circle;
	}
}
