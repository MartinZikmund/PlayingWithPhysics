using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.ViewModels;

namespace Physics.LissajousCurves.ViewModels
{
	public class DemoItemViewModel : MvxNotifyPropertyChanged
	{
		public string Label { get; set; }
		public OscillationInfoViewModel X { get; set; }
		public OscillationInfoViewModel Y { get; set; }
		public DemoItemViewModel(string label, OscillationInfoViewModel x, OscillationInfoViewModel y)
		{
			Label = label;
			X = x;
			Y = y;
		}
	}
}
