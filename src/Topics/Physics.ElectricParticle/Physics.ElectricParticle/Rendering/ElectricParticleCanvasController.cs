﻿using System;
using Microsoft.Toolkit.Uwp.Helpers;
using Physics.ElectricParticle.Logic;
using Physics.Shared.Mathematics;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.ElectricParticle.Rendering
{
	public class ElectricParticleCanvasController : SkiaCanvasController
	{
		private ElectricParticleSimulationSetup _setup;
		private PhysicsService _service;

		private float _unitDimension;

		private float _unitToPixel;
		private float _pixelToUnit;

		private int _simulationFrame;
		private BigNumber _time;
		private BigNumber _deltaT;

		private readonly SKPaint _planePaint = new SKPaint()
		{
			Color = SKColors.Black,
			StrokeWidth = 5,
			IsStroke = true
		};

		private readonly SKPaint _particlePaint = new SKPaint()
		{
			Color = SKColors.Black,
			IsStroke = false,
			IsAntialias = true
		};

		public ElectricParticleCanvasController(ISkiaCanvas canvasAnimatedControl) : base(canvasAnimatedControl)
		{
		}

		public void StartSimulation()
		{
			_simulationFrame = 0;
			_time = 0;
			SimulationTime.Restart();
			Play();
		}

		public void SetMotion(ElectricParticleSimulationSetup setup)
		{
			_setup = setup;

			var unitXSize = -1f;
			var unitYSize = -1f;

			if (_setup.VerticalPlane != null)
			{
				unitXSize = _setup.VerticalPlane.Distance * 1.1f;
			}

			if (_setup.HorizontalPlane != null)
			{
				unitYSize = _setup.HorizontalPlane.Distance * 1.1f;
			}

			_unitDimension = Math.Max(unitXSize, unitYSize);

			var color = ColorHelper.ToColor(setup.Color);
			_particlePaint.Color = new SKColor(color.R, color.G, color.B);
			_service = new PhysicsService(_setup);
			_deltaT = _service.ComputeDeltaT(600);
		}

		public override void Update(ISkiaCanvas sender)
		{
			var minDimension = Math.Min(sender.ScaledSize.Width, sender.ScaledSize.Height);
			_pixelToUnit = _unitDimension / minDimension;
			_unitToPixel = minDimension / _unitDimension;			
		}

		public override void Draw(ISkiaCanvas sender, SKSurface args)
		{
			if (_setup == null)
			{
				return;
			}

			DrawPlanes(sender, args);

			DrawParticle(sender, args);

			_time += _deltaT;
		}

		private void DrawPlanes(ISkiaCanvas sender, SKSurface args)
		{
			if (_setup.HorizontalPlane != null)
			{
				DrawHorizontalPlanes(sender, args);
			}

			if (_setup.VerticalPlane != null)
			{
				DrawVerticalPlanes(sender, args);
			}
		}

		private void DrawParticle(ISkiaCanvas sender, SKSurface args)
		{
			var x = (float)(double)_service.ComputeX(_time);
			var y = (float)(double)_service.ComputeY(_time);

			var zeroX = _canvas.ScaledSize.Width / 2;
			var zeroY = _canvas.ScaledSize.Height / 2;

			args.Canvas.DrawCircle(new SKPoint(zeroX + x * _unitToPixel, zeroY - y * _unitToPixel), 5, _particlePaint);
		}

		private void DrawVerticalPlanes(ISkiaCanvas sender, SKSurface args)
		{
			var verticalPlaneDistance = _setup.VerticalPlane.Distance;
			var horizontalPlaneDistance = _setup.HorizontalPlane?.Distance ?? sender.ScaledSize.Height * 0.8f * _pixelToUnit;
			var zeroX = _canvas.ScaledSize.Width / 2;
			var zeroY = _canvas.ScaledSize.Height / 2;

			var planeTop = zeroY - horizontalPlaneDistance * _unitToPixel / 2;
			var planeBottom = zeroY + horizontalPlaneDistance * _unitToPixel / 2;

			var leftPlaneX = zeroX - verticalPlaneDistance * _unitToPixel / 2;
			var rightPlaneX = zeroX + verticalPlaneDistance * _unitToPixel / 2;

			args.Canvas.DrawLine(
				new SKPoint(leftPlaneX, planeTop),
				new SKPoint(leftPlaneX, planeBottom),
				_planePaint);

			args.Canvas.DrawLine(
				new SKPoint(rightPlaneX, planeTop),
				new SKPoint(rightPlaneX, planeBottom),
				_planePaint);
		}

		private void DrawHorizontalPlanes(ISkiaCanvas sender, SKSurface args)
		{
			var verticalPlaneDistance = _setup.VerticalPlane?.Distance ?? sender.ScaledSize.Width * 0.8f * _pixelToUnit;
			var horizontalPlaneDistance = _setup.HorizontalPlane.Distance;
			var zeroX = _canvas.ScaledSize.Width / 2;
			var zeroY = _canvas.ScaledSize.Height / 2;

			var planeLeft = zeroX - verticalPlaneDistance * _unitToPixel / 2;
			var planeRight = zeroX + verticalPlaneDistance * _unitToPixel / 2;

			var topPlaneY = zeroY - horizontalPlaneDistance * _unitToPixel / 2;
			var bottomPlaneY = zeroY + horizontalPlaneDistance * _unitToPixel / 2;

			args.Canvas.DrawLine(
				new SKPoint(planeLeft, topPlaneY),
				new SKPoint(planeRight, topPlaneY),
				_planePaint);

			args.Canvas.DrawLine(
				new SKPoint(planeLeft, bottomPlaneY),
				new SKPoint(planeRight, bottomPlaneY),
				_planePaint);
		}		
	}
}
