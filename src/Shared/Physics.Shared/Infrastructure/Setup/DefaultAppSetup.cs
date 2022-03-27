using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.Platforms.Uap.Core;
using MvvmCross.Platforms.Uap.Presenters;
using MvvmCross.Platforms.Uap.Views;
using MvvmCross.ViewModels;
using Physics.Shared.Services.Preferences;
using Physics.Shared.Services.Sounds;
using Physics.Shared.UI.Services;
using Physics.Shared.UI.Services.AppList;
using Physics.Shared.UI.Services.Dialogs;
using Physics.Shared.ViewModels;

namespace Physics.Shared.UI.Infrastructure.Setup
{
    public abstract class DefaultAppSetup<T> : MvxWindowsSetup<T>
        where T : class, IMvxApplication, new()
    {
		protected override void InitializeFirstChance(IMvxIoCProvider iocProvider)
		{
			base.InitializeFirstChance(iocProvider);
			iocProvider.LazyConstructAndRegisterSingleton<IPreferences, Preferences>();
			iocProvider.LazyConstructAndRegisterSingleton<ISoundPlayer, SoundPlayer>();
			iocProvider.LazyConstructAndRegisterSingleton<IContentDialogHelper, ContentDialogHelper>();
			iocProvider.LazyConstructAndRegisterSingleton<IAppList, AppList>();
		}

        public override IEnumerable<Assembly> GetViewAssemblies()
        {
            var list = new List<Assembly>();
            list.AddRange(base.GetViewAssemblies());
			list.Add(typeof(DefaultAppSetup<>).Assembly);
			list.Add(GetType().Assembly);
            return list.Distinct().ToArray();
        }

        public override IEnumerable<Assembly> GetViewModelAssemblies()
        {
            var list = new List<Assembly>();
            list.AddRange(base.GetViewModelAssemblies());
            list.Add(typeof(MainMenuViewModel).Assembly);
			list.Add(GetType().Assembly);
            return list.Distinct().ToArray();
        }

        protected override IMvxWindowsViewPresenter CreateViewPresenter(IMvxWindowsFrame rootFrame)
        {
            return new DefaultViewPresenter(rootFrame);
        }

		protected override ILoggerProvider CreateLogProvider()
		{
			return NullLoggerProvider.Instance;
		}

		protected override ILoggerFactory CreateLogFactory()
		{
			return NullLoggerFactory.Instance;
		}
	}
}
