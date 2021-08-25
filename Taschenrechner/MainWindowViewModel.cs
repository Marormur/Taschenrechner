using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors.Core;
using Taschenrechner.Annotations;

namespace Taschenrechner
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private string eingabe = "0";

        public string Eingabe
        {
            get => eingabe;
            set
            {
                if (value == eingabe)
                {
                    return;
                }

                eingabe = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Wenn einer der Hauptbuttons betätigt wurde.
        /// </summary>
        public ICommand GenericButtonClickCommand => new ActionCommand(delegate(object o)
        {
            if (o is not RoutedEventArgs {Source: Button knopf})
            {
                return;
            }

            eingabe += knopf.Content.ToString();
        });

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
