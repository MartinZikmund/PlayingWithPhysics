using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

        protected ICommand GetOrCreateAsyncCommand(Func<Task> execute, [CallerMemberName]string propertyName = null)
        {
            if (!_commands.TryGetValue(propertyName, out var command))
            {
                command = new MvxAsyncCommand(execute);
                _commands.Add(propertyName, command);
            }
            return command;
        }
    }

    public abstract class ViewModelBase<TParameter>
        : MvxViewModel<TParameter>
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
    }
}
