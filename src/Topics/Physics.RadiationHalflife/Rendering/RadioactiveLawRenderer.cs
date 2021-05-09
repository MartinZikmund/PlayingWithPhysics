using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Physics.RadiationHalflife.Logic;
using Physics.Shared.UI.Localization;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.RadiationHalflife.Rendering
{
	public class RadioactiveLawRenderer : IVariantRenderer
	{
		public RadioactiveLawPhysicsService PhysicsService { get; set; }
		Random _randomizer = new Random();
		private float _displayScale = 1f;
		private float _smallerDimension = 0f;
		private float _padding = 1f;
		private List<(float X, float Y)> _values = new List<(float x, float y)>();
		private SKPoint _lastPoint = new SKPoint();
		private int[] _randomizedArray = null;
		//Bool = dead
		private bool[] _aliveList = null;

		private int _centerY = -1;

		private SKPaint _ballFillPaint = new SKPaint()
		{
			IsStroke = false,
			IsAntialias = true,
			Color = SKColors.Gray
		};

		private SKPaint _activeNucleoidFillPaint = new SKPaint()
		{
			IsStroke = false,
			IsAntialias = true,
			Color = SKColors.Orange
		};

		private SKPaint _inactiveNucleoidFillPaint = new SKPaint()
		{
			IsStroke = false,
			IsAntialias = true,
			Color = SKColors.Gray
		};


		private SKPaint _graphStrokePaint = new SKPaint()
		{
			IsStroke = true,
			IsAntialias = true,
			StrokeWidth = 3,
			FilterQuality = SKFilterQuality.High,
			Color = SKColors.Orange
		};

		private SKPaint _axisStrokePaint = new SKPaint()
		{
			IsStroke = true,
			IsAntialias = true,
			FilterQuality = SKFilterQuality.High,
			Color = SKColors.Gray
		};

		private SKPaint _axisLabelPaint = new SKPaint()
		{
			TextSize = 16f,
			IsAntialias = true,
			Color = SKColors.Gray,
			IsStroke = false,
			TextAlign = SKTextAlign.Center
		};

		int _lastTotalParticles;

		public void SetAnimation(PhysicsService physicsService)
		{
			PhysicsService = (RadioactiveLawPhysicsService)physicsService;
			_lastTotalParticles = PhysicsService.Animation.ParticleCount;
			_values = PhysicsService.FillTablePrecise();

			var tempRandomized = Enumerable.Range(0, _lastTotalParticles).ToArray();
			_aliveList = new bool[_lastTotalParticles];

			for (int i = 0; i < _lastTotalParticles; i++)
			{
				_aliveList[i] = true;
			}

			_randomizedArray = tempRandomized.OrderBy(x => _randomizer.Next()).ToArray();
		}

		public void StartSimulation()
		{
			_controller.SimulationTime.Restart();
			_controller.Play();
		}

		public void Draw(ISkiaCanvas sender, SKSurface args)
		{
			DrawField(sender, args);
			DrawAces(sender, args);
			PlotGraph(sender, args);
			DrawLabels(sender, args);
			//var elapsedTime = SimulationTime.TotalTime.TotalSeconds;
			//double endTime = elapsedTime / 30.0 * (6 * PhysicsService.Animation.Halflife);
			//var n = _values.FirstOrDefault(i => i.Y > (float)endTime);
			args.Canvas.DrawCircle(_lastPoint, 5, _ballFillPaint);

		}
		public void Update(ISkiaCanvas sender)
		{
			_centerY = (int)(sender.ScaledSize.Height / 2);
			//smallerDimension so that resizing works
			_smallerDimension = Math.Min(sender.ScaledSize.Width, sender.ScaledSize.Height);
			_padding = _smallerDimension * 0.1f;
			_displayScale = (float)_smallerDimension - 2 * _padding;
		}


		public void DrawField(ISkiaCanvas sender, SKSurface args)
		{
			//Check what time we are in terms of values
			var elapsedTime = _controller.SimulationTime.TotalTime.TotalSeconds;
			double endTime = elapsedTime / 30.0 * (6 * PhysicsService.Animation.Halflife);
			float currentParticlesRemaining = _values.Last().Y;
			for (int i = 0; i < _values.Count; i++)
			{
				float time = _values[i].X;
				if (time > endTime)
				{
					currentParticlesRemaining = _values[i].Y;
					break;
				}
			}

			//Compute how many particles to kill
			//var toKill = PhysicsService.Animation.ParticleCount - (_lastItem + _item);
			var toKill = _lastTotalParticles - currentParticlesRemaining;
			//double endTime = elapsedTime / 30.0 * (6 * PhysicsService.Animation.Halflife);
			//float remainingParticles = _values[elapsedTime];

			//Mark as killed
			for (int i = 0; i < toKill; i++)
			{
				var aliveListIndex = _randomizedArray[i];
				//Mark as dead
				_aliveList[aliveListIndex] = false;
			}

			//Get appropriate row count
			int rows = 10;
			var paddingAroundCenter = _centerY / rows;
			int columns = PhysicsService.Animation.ParticleCount / rows;
			int remainderColumn = PhysicsService.Animation.ParticleCount % rows;
			if (remainderColumn > 0)
			{
				columns++;
			}
			float particleWidth = (sender.ScaledSize.Width - 2 * _padding) / columns;
			particleWidth = Math.Min(particleWidth, 80);
			//Caculate horizontal centering for dynamic particle count
			float fieldLeftX = (sender.ScaledSize.Width / 2) - (particleWidth * columns / 2);

			float particleHeight = (sender.ScaledSize.Height / 2 - _padding - 20) / rows;
			particleHeight = Math.Min(particleHeight, 80);
			float fieldTopY = (sender.ScaledSize.Height * 0.75f) - (particleHeight * rows / 2);
			int drawnParticles = 0;

			for (int y = 0; y < rows; y++)
			{
				for (int x = 0; x < columns; x++)
				{
					if (drawnParticles >= PhysicsService.Animation.ParticleCount)
					{
						break;
					}
					float cx = fieldLeftX + (x * particleWidth) + particleWidth / 2;
					float cy = fieldTopY + (y * particleHeight) + particleHeight / 2;
					if (x >= (sender.ScaledSize.Width - _padding - particleWidth / 2))
					{
						continue;
					}

					var indexInAliveList = (y * columns) + x;
					if (_aliveList[indexInAliveList])
					{
						args.Canvas.DrawCircle(cx, cy, 5, _activeNucleoidFillPaint);
					}
					else
					{
						args.Canvas.DrawCircle(cx, cy, 5, _inactiveNucleoidFillPaint);
					}
					drawnParticles++;
				}
			}
		}

		private void DrawLabels(ISkiaCanvas sender, SKSurface args)
		{
			var paddingAroundCenter = _centerY * 0.1f;
			//Y-axis
			var particleCount = PhysicsService.Animation.ParticleCount;
			args.Canvas.DrawText($"{particleCount}", new SKPoint(_padding - _axisLabelPaint.MeasureText(particleCount.ToString()), _padding + (_padding / 6)), _axisLabelPaint);
			args.Canvas.DrawText($"{particleCount / 2}", new SKPoint(_padding - _axisLabelPaint.MeasureText((particleCount / 2).ToString()), _centerY / 2 + _axisLabelPaint.MeasureText(particleCount.ToString())), _axisLabelPaint);
			args.Canvas.DrawText("0", new SKPoint(_padding - _axisLabelPaint.MeasureText("0"), _centerY - paddingAroundCenter), _axisLabelPaint);
			//X-axis (0 already drawn above)
			for (int i = 1; i <= 6; i++)
			{
				string formattedHalflifeNumber = (i * PhysicsService.Animation.Halflife).ToString("0.00");
				string formattedHalflifeUnit = "";
				if (PhysicsService.Animation.ChemicalElement == "Custom")
				{
					CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
					if (cultureInfo.Name == "en-US")
					{
						formattedHalflifeUnit = PhysicsService.Animation.CustomHalflifeUnit + "s";
					}
					else
					{
						formattedHalflifeUnit = PhysicsService.Animation.CustomHalflifeUnit;
					}
				}
				else
				{
					formattedHalflifeUnit = Localizer.Instance[PhysicsService.Animation.ChemicalElement + "_Halflife"];
				}
				formattedHalflifeUnit = formattedHalflifeUnit.ToLowerInvariant();
				var formattedHalflifeForAxis = $"{formattedHalflifeNumber} {formattedHalflifeUnit}";
				args.Canvas.DrawText($"{formattedHalflifeForAxis}", new SKPoint(_padding + (i * ((sender.ScaledSize.Width - _padding - _padding) / 6)), _centerY - paddingAroundCenter + _axisLabelPaint.MeasureText($"{i}T")), _axisLabelPaint);
			}
			//args.Canvas.DrawText(_padding - (_padding/2), _padding, _padding, _centerY - paddingAroundCenter, _fillPaint);
		}

		private void DrawAces(ISkiaCanvas sender, SKSurface args)
		{
			//args.Canvas.DrawCircle();
			var paddingAroundCenter = _centerY * 0.1f;
			//Vertical
			args.Canvas.DrawLine(_padding, _padding, _padding, _centerY - paddingAroundCenter, _axisStrokePaint);
			//Horizontal
			args.Canvas.DrawLine(_padding, _centerY - paddingAroundCenter, sender.ScaledSize.Width - _padding, _centerY - paddingAroundCenter, _axisStrokePaint);
		}

		private void PlotGraph(ISkiaCanvas sender, SKSurface args)
		{
			var deltaX = (PhysicsService.Animation.Delta / (6 * PhysicsService.Animation.Halflife)) * (sender.ScaledSize.Width - _padding - _padding);
			var path = new SKPath();
			var firstPoint = _values[0];
			var elapsedTime = _controller.SimulationTime.TotalTime.TotalSeconds;
			double endTime = elapsedTime / 30.0 * (6 * PhysicsService.Animation.Halflife);
			path.MoveTo(new SKPoint(_padding, GetGraphAbsoluteY(sender, GetGraphRelativeY(sender, firstPoint.Y))));
			for (int i = 0; (i + 1) < _values.Count && _values[i].X < endTime; i++)
			{
				var relativeX = deltaX * i + _padding;
				var nextPoint = _values[i + 1];
				path.LineTo(relativeX, GetGraphAbsoluteY(sender, GetGraphRelativeY(sender, nextPoint.Y)));
				_lastPoint.X = relativeX;
				_lastPoint.Y = GetGraphAbsoluteY(sender, GetGraphRelativeY(sender, nextPoint.Y));
			}

			args.Canvas.DrawPath(path, _graphStrokePaint);
		}

		private float GetGraphRelativeY(ISkiaCanvas sender, float n)
		{
			var paddingAroundCenter = _centerY * 0.1f;
			return (_centerY - paddingAroundCenter - _padding) * (n / (PhysicsService.Animation.ParticleCount * 1.0f));
		}

		private float GetGraphAbsoluteY(ISkiaCanvas sender, float relativeY)
		{
			var centerY = sender.ScaledSize.Height / 2;
			var paddingAroundCenter = centerY * 0.1f;
			return (float)(_padding + (centerY - paddingAroundCenter - _padding) - relativeY);
		}

		public void Dispose() { }

		private RadiationHalflifeController _controller;
		public RadioactiveLawRenderer(RadiationHalflifeController controller)
		{
			_controller = controller;
		}
	}
}
