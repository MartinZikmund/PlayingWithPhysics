﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace Physics.GravitationalFieldMovement.Rendering.Extensions
{
	internal static class SKBitmapExtensions
	{

		public static void Rotate(this SKBitmap bitmap, double angle)
		{
			double radians = Math.PI * angle / 180;
			float sine = (float)Math.Abs(Math.Sin(radians));
			float cosine = (float)Math.Abs(Math.Cos(radians));
			int originalWidth = bitmap.Width;
			int originalHeight = bitmap.Height;
			int rotatedWidth = (int)(cosine * originalWidth + sine * originalHeight);
			int rotatedHeight = (int)(cosine * originalHeight + sine * originalWidth);

			var rotatedBitmap = new SKBitmap(rotatedWidth, rotatedHeight);

			using (var surface = new SKCanvas(rotatedBitmap))
			{
				surface.Translate(rotatedWidth / 2, rotatedHeight / 2);
				surface.RotateDegrees((float)angle);
				surface.Translate(-originalWidth / 2, -originalHeight / 2);
				surface.DrawBitmap(bitmap, new SKPoint());
			}
			bitmap = rotatedBitmap;
		}

	}
}
