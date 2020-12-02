using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross;
using MvvmCross.Navigation;

namespace Physics.Shared.ViewModels
{
    public abstract class ViewModelBase : MvxViewModel
    {
        private readonly Dictionary<string, ICommand> _commands = new Dictionary<string, ICommand>();

        protected ICommand GetOrCreateCommand(Action execute, [CallerMemberName]string propertyName = null)
        {
            if (!_commands.TryGetValue(propertyName, out var command))
            {
                command = new MvxCommand(execute);
                _commands.Add(propertyName, command);
            }
            return command;
        }

        protected ICommand GetOrCreateCommand<T>(Action<T> execute, [CallerMemberName]string propertyName = null)
        {
            if (!_commands.TryGetValue(propertyName, out var command))
            {
                command = new MvxCommand<T>(execute);
                _commands.Add(propertyName, command);
            }
            return command;
        }

        protected ICommand GetOrCreateAsyncCommand(Func<Task> execute, [CallerMemberName]string propertyName = null)
        {
            if (!_commands.TryGetValue(propertyName, out var command))
            {
                command = new MvxAsyncCommand(execute);
                _commands.Add(propertyName, command);
            }
            return command;
        }

        protected ICommand GetOrCreateAsyncCommand<T>(Func<T,Task> execute, [CallerMemberName]string propertyName = null)
        {
            if (!_commands.TryGetValue(propertyName, out var command))
            {
                command = new MvxAsyncCommand<T>(execute);
                _commands.Add(propertyName, command);
            }
            return command;
        }

        public ICommand GoBackCommand => GetOrCreateAsyncCommand(GoBackAsync);

        private async Task GoBackAsync()
        {
            await Mvx.IoCProvider.Resolve<IMvxNavigationService>().Close(this);
        }
    }

    public abstract class ViewModelBase<TParameter>
        : MvxViewModel<TParameter>
        where TParameter : class
    {
        private readonly Dictionary<string, ICommand> _commands = new Dictionary<string, ICommand>();

        protected ICommand GetOrCreateCommand(Action execute, [CallerMemberName]string propertyName = null)
        {
            if (!_commands.TryGetValue(propertyName, out var command))
            {
                command = new MvxCommand(execute);
                _commands.Add(propertyName, command);
            }
            return command;
        }

        protected ICommand GetOrCreateAsyncCommand(Func<Task> execute, [CallerMemberName]string propertyName = null)
        {
            if (!_commands.TryGetValue(propertyName, out var command))
            {
                command = new MvxAsyncCommand(execute);
                _commands.Add(propertyName, command);
            }
            return command;
        }

        protected ICommand GetOrCreateAsyncCommand<TInput>(Func<TInput, Task> execute, [CallerMemberName]string propertyName = null)
        {
            if (!_commands.TryGetValue(propertyName, out var command))
            {
                command = new MvxAsyncCommand<TInput>(execute);
                _commands.Add(propertyName, command);
            }
            return command;
        }

        public ICommand GoBackCommand => GetOrCreateAsyncCommand(GoBackAsync);

        private async Task GoBackAsync()
        {
            await Mvx.IoCProvider.Resolve<IMvxNavigationService>().Close(this);
        }
    }
}
