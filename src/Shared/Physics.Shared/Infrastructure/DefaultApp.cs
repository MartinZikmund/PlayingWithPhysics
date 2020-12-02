using MvvmCross.IoC;
using MvvmCross.ViewModels;

namespace Physics.Shared.UI.Infrastructure
{
	public class DefaultApp<TMvxAppStart> : MvxApplication
		where TMvxAppStart : class, IMvxAppStart
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterCustomAppStart<TMvxAppStart>();
        }
    }

	public class DefaultApp : DefaultApp<DefaultAppStart>
	{
	}
}
