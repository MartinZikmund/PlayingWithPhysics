using System.Diagnostics;
using MvvmCross.ViewModels;
using Physics.Shared.Helpers;

namespace Physics.ElectricParticle.Game
{
	public class GameInfo : MvxNotifyPropertyChanged
	{
		private const int LevelCount = 11;
		private const float AccelerationScale = 0.00005f;

		private readonly Stopwatch _drawingStopwatch = new Stopwatch();

		private float _ux = 0;
		private float _uy = 0;

		public int Level { get; set; }

		public string LevelText => (Level + 1).ToString();

		public bool UseGravity { get; set; }

		public bool IsPenDown { get; set; }

		public GameState State { get; private set; }

		public PenState PenState { get; private set; } = new PenState();

		public Drawing Drawing { get; private set; } = new Drawing();

		public string DrawingTimeString => $"{_drawingStopwatch.Elapsed.Minutes:00}:{_drawingStopwatch.Elapsed.Seconds:00}";

		public float Ux
		{
			get => _ux;
			set => SetProperty(ref _ux, MathHelpers.Clamp(value, -100, 100));
		}

		public float Uy
		{
			get => _uy;
			set => SetProperty(ref _uy, MathHelpers.Clamp(value, -100, 100));
		}

		public void GoToNextLevel()
		{
			Level = (Level + 1) % LevelCount;
			ResetLevel();
		}

		public void ResetLevel()
		{
			Drawing = new Drawing();
			State = GameState.Idle;
			IsPenDown = false;
			UseGravity = false;
			PenState = new PenState();
			_drawingStopwatch.Stop();
			_drawingStopwatch.Reset();
			Ux = 0;
			Uy = 0;
		}

		public void Start()
		{
			State = GameState.Drawing;
			_drawingStopwatch.Start();
		}

		public void End()
		{
			State = GameState.Ended;
			_drawingStopwatch.Stop();
			//TODO: Evaluate drawing
		}

		public void UpdatePenPosition(float elapsedTime)
		{
			var ax = Ux * AccelerationScale;
			var ay = -Uy * AccelerationScale + (UseGravity ? 0.3f : 0);

			var vx = PenState.Speed.X + ax * elapsedTime;
			var vy = PenState.Speed.Y + ay * elapsedTime;

			var x = PenState.Position.X + PenState.Speed.X * elapsedTime + 0.5f * PenState.Acceleration.X * elapsedTime * elapsedTime;
			var y = PenState.Position.Y + PenState.Speed.Y * elapsedTime + 0.5f * PenState.Acceleration.Y * elapsedTime * elapsedTime;

			if (x <= 0 || x >= 1)
			{
				x = MathHelpers.Clamp(x, 0, 1);
				vx = 0;
			}

			if (y <= 0 || y >= 1)
			{
				y = MathHelpers.Clamp(y, 0, 1);
				vy = 0;
			}

			PenState.Position = new System.Numerics.Vector2(x, y);
			PenState.Acceleration = new System.Numerics.Vector2(ax, ay);
			PenState.Speed = new System.Numerics.Vector2(vx, vy);

			if (IsPenDown)
			{
				Drawing.AddPoint(PenState.Position);
			}
			else
			{
				Drawing.PenUp();
			}

			RaisePropertyChanged(nameof(DrawingTimeString));
		}
	}
}
