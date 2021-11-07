using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Physics.HuygensPrinciple.Logic.Benchmarks
{
	public class HuygensStepperBenchmarks
	{
		private HuygensFieldBuilder _builder;

		public HuygensStepperBenchmarks()
		{
			var data = new HuygensFieldBuilder(1920, 1080);
			data.DrawScene(ScenePresets.Presets[0]);
			_builder = data;
		}

		[Benchmark]
		public Task PrecalculateSimple() => new HuygensStepper(_builder.Build(), 5).PrecalculateStepsAsync();
	}

	public class Program
	{
		public static void Main(string[] args)
		{
			var summary = BenchmarkRunner.Run<HuygensStepperBenchmarks>();
		}
	}
}
