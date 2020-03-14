using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.Platforms.Uap.Core;
using MvvmCross.ViewModels;
using Physics.Shared.Infrastructure.Topics;
using Physics.Shared.Services.Preferences;
using Physics.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Platforms.Uap.Presenters;
using MvvmCross.Platforms.Uap.Views;

namespace Physics.Shared.Infrastructure.Setup
{
    public abstract class DefaultAppSetup<T> : MvxWindowsSetup<T>
        where T : class, IMvxApplication, new()
    {
        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IPreferences, Preferences>();
        }

        public override IEnumerable<Assembly> GetViewAssemblies()
        {
            var list = new List<Assembly>();
            list.AddRange(base.GetViewAssemblies());
            list.Add(typeof(DefaultAppSetup<>).Assembly);
            return list.ToArray();
        }

        public override IEnumerable<Assembly> GetViewModelAssemblies()
        {
            var list = new List<Assembly>();
            list.AddRange(base.GetViewModelAssemblies());
            list.Add(typeof(MainMenuViewModel).Assembly);
            return list.ToArray();
        }

        protected override IMvxWindowsViewPresenter CreateViewPresenter(IMvxWindowsFrame rootFrame)
        {
            return new DefaultViewPresenter(rootFrame);
        }
    }
}
