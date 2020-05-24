using Physics.Shared.SelfStudy.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Physics.SelfStudy.Editor.ViewModels
{
    public abstract class ContentBase : INotifyPropertyChanged
    {
        public abstract ContentType Type { get; }

        public void ForceUpdate()
        {
            OnPropertyChanged(null);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
