using System;

namespace Physics.Shared.UI.Services.ValuesTable
{
	[AttributeUsage(AttributeTargets.Property)]
	public class ValuesTableHeaderAttribute : Attribute
	{
		public ValuesTableHeaderAttribute(string name) => Header = name;

		public string Header { get; set; }
	}
}
