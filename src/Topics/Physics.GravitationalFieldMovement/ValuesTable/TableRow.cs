using System.Collections.Generic;
using Physics.GravitationalFieldMovement.Services;
using Physics.Shared.Mathematics;
using Physics.Shared.UI.Services.ValuesTable;
using ReactiveUI;

namespace Physics.GravitationalFieldMovement.ValuesTable;

public class TableRow : ValuesTableRowBase
{
	private const int TableSignificantDigits = 5;

	public string T { get; set; }

	public string X { get; set; }

	public string Y { get; set; }

	public string Phi { get; set; }

	public string R { get; set; }

	public string H { get; set; }

	public string V { get; set; }

	private readonly IAppPreferences _appPreferences;

	public TableRow(double t, double x, double y, double phi, double r, double h, double v, IAppPreferences appPreferences)
	{
		if (appPreferences.LengthUnit == Logic.LengthUnit.Metric)
		{
			H = h.ToSignificantDigitsString(1);
		} else
		{
			H = h.ToSignificantDigitsString(TableSignificantDigits);
		}
		T = t.ToSignificantDigitsString(TableSignificantDigits);
		X = x.ToSignificantDigitsString(TableSignificantDigits);
		Y = y.ToSignificantDigitsString(TableSignificantDigits);
		V = v.ToSignificantDigitsString(TableSignificantDigits);
		Phi = phi.ToSignificantDigitsString(TableSignificantDigits);
		R = r.ToSignificantDigitsString(TableSignificantDigits);
	}

	protected override IEnumerable<string> GetCellValuesInOrder()
	{
		yield return T;
		yield return X;
		yield return Y;
		yield return Phi;
		yield return R;
		yield return H;
		yield return V;
	}
}
