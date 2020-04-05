using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Physics.Shared.SelfStudyEditor.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly Dictionary<string, ICommand> _commands = new Dictionary<string, ICommand>();

        protected ICommand GetOrCreateCommand(Action execute, [CallerMemberName]string propertyName = null)
        {
            if (!_commands.TryGetValue(propertyName, out var command))
            {
                command = new RelayCommand(execute);
                _commands.Add(propertyName, command);
            }
            return command;
        }

        protected ICommand GetOrCreateCommand<T>(Action<T> execute, [CallerMemberName]string propertyName = null)
        {
            if (!_commands.TryGetValue(propertyName, out var command))
            {
                command = new RelayCommand<T>(execute);
                _commands.Add(propertyName, command);
            }
            return command;
        }

        private void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
