using System;
using System.Collections.Generic;
using System.Drawing;

namespace Physics.HuygensPrinciple.Logic
{
	public static class HuygensShapeDrawer
    {
		public static IList<CellStateChange> DrawRectangle(HuygensField field, Point leftUpper, Point rightLower, CellState fill = CellState.Source, bool avoidObjects = true)
		{
			var results = new List<CellStateChange>();

			for (int x = leftUpper.X; x <= rightLower.X; x++)
			{
				for (int y = leftUpper.Y; y <= rightLower.Y; y++)
				{
					if (avoidObjects && (field[x, y] == CellState.Source || field[x, y] == CellState.Wall))
					{
						continue;
					}

					if (field[x, y] != fill)
					{
						field[x, y] = fill;
						results.Add(new CellStateChange(x, y, fill));
					}
				}
			}

			return results;
		}

		public static IList<CellStateChange> DrawCircle(HuygensField field, Point center, float radius, CellState fill = CellState.Source, bool avoidObjects = true)
		{
			var results = new List<CellStateChange>();

			var (cX, cY) = (center.X, center.Y);
			var xStart = Math.Max((int)(cX - radius - 1), 0);
			var xEnd = Math.Min((int)(cX + radius + 1), field.Width - 1);

			var yStart = Math.Max((int)(cY - radius - 1), 0);
			var yEnd = Math.Min((int)(cY + radius + 1), field.Height - 1);

			var radius2 = radius * radius;

			for (int x = xStart; x <= xEnd; x++)
			{
				for (int y = yStart; y <= yEnd; y++)
				{
					if ((x - cX) * (x - cX) + (y - cY) * (y - cY) <= radius2)
					{
						if (avoidObjects && (field[x, y] == CellState.Source || field[x, y] == CellState.Wall))
						{
							continue;
						}

						if (field[x, y] != fill)
						{
							field[x, y] = fill;
							results.Add(new CellStateChange(x, y, fill));
						}
					}
				}
			}

			return results;
		}
	}
}
