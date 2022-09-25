using System;
using System.Collections.Generic;
using MvvmCross.ViewModels;
using Physics.OpticalInstruments.Rendering;
using Physics.Shared.Helpers;
using Physics.Shared.Logic.Geometry;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;
using static Microsoft.Toolkit.Uwp.UI.Animations.Expressions.ExpressionValues;

namespace Physics.OpticalInstruments.Game
{
	public class GameInfo : MvxNotifyPropertyChanged
	{
		private const float ActualPlanetSize = 100;

		internal int ObjectX => 1300;		

		private Random _randomizer = new Random();

		public int TargetAngle { get; private set; }

		public event EventHandler GameStateChanged;

		public void OnStateChanged() => GameStateChanged?.Invoke(this, EventArgs.Empty);

		public GameState State { get; private set; }

		public DateTimeOffset LastFireTime { get; set; }

		public bool LaserHitPlanet { get; set; } = true;

		public bool PerfectHit => BestScore == 1000;

		public int CurrentShot { get; private set; } = 1;

		public int TotalShots => 3;

		public int CurrentAngle { get; set; } = 20;

		public int? BestScore { get; private set; }

		public int FinishedAttempts { get; private set; }

		public List<int> Attempts { get; } = new List<int>();

		public string Attempt1Text => Attempts.Count > 0 ? Attempts[0].ToString() : "";

		public string Attempt2Text => Attempts.Count > 1 ? Attempts[1].ToString() : "";

		public string Attempt3Text => Attempts.Count > 2 ? Attempts[2].ToString() : "";

		public void Shoot()
		{
			LastFireTime = DateTimeOffset.UtcNow;
			FinishedAttempts++;
			int score = CalculateScore();
			LaserHitPlanet = score > 0;

			Attempts.Add(score);
			BestScore = BestScore is null ? score : Math.Max(score, BestScore.Value);
			
			if (FinishedAttempts == 3 || CurrentAngle == TargetAngle)
			{
				State = GameState.GameEnded;
			}
			else
			{
				State = GameState.Fired;
			}
						
			RaiseAllPropertiesChanged();
		}

		public void StartNewGame()
		{
			GenerateRandomTarget();
			State = GameState.SetAngle;
			LaserHitPlanet = false;
			CurrentShot = 1;
			CurrentAngle = 78;
			Attempts.Clear();
			BestScore = null;
			FinishedAttempts = 0;
			RaiseAllPropertiesChanged();
		}

		public void NextShot()
		{
			State = GameState.SetAngle;
			LaserHitPlanet = false;
			CurrentShot++;
			RaiseAllPropertiesChanged();
		}

		private void GenerateRandomTarget()
		{
			TargetAngle = _randomizer.Next(37, 63);
		}

		internal void MockAngle()
		{
			TargetAngle = CurrentAngle;
		}

		private int CalculateScore()
		{
			var maxAngle = CalculatePlanetEdgeAngle();
			var diff = Math.Abs(CurrentAngle - TargetAngle);

			if (diff > maxAngle)
			{
				return 0;
			}
			else
			{
				return 1000 - diff * 50;
			}
		}

		private double CalculatePlanetEdgeAngle()
		{
			var planetRadius = ActualPlanetSize / 2;
			var planetVerticalX = ObjectX;

			var verticalPoint = new SKPoint(GameCanvasController.MirrorHitPoint.X, 0);

			var targetPointDirection = SkiaHelpers.RotatePoint(verticalPoint, GameCanvasController.MirrorHitPoint, MathHelpers.DegreesToRadians((90 - (float)TargetAngle) * 2));

			var mirrorHitPoint = new Point2d(GameCanvasController.MirrorHitPoint.X, GameCanvasController.MirrorHitPoint.Y);

			var lineToTarget = new Line2d(mirrorHitPoint, new Point2d(targetPointDirection.X, targetPointDirection.Y));
			var lineX = new Line2d(new Point2d(planetVerticalX, 0), new Point2d(planetVerticalX, 1000));
			var planetPoint = lineToTarget.IntersectWith(lineX).Value;

			return MathHelpers.RadiansToDegrees((float)Math.Atan(planetRadius / planetPoint.DistanceTo(mirrorHitPoint)));
		}
	}
}
