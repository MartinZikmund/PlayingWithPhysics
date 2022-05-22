namespace Physics.HuygensPrinciple.Logic;

public class DemoScenario
{
	public DemoScenario(string key, IShape defaultSource)
	{
		Key = key;
		DefaultSource = defaultSource;
	}
	
	public string Key { get; }

	public IShape DefaultSource { get; set; }
}
