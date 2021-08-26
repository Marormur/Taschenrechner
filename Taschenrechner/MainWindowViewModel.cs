using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using Microsoft.Xaml.Behaviors.Core;
using Taschenrechner.Annotations;

namespace Taschenrechner
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Komplette Rechenformel
        /// </summary>
        private string kompletteRechnung;

        private string eingabe = "0";

        /// <summary>
        /// Die Eingabe, welche dem Nutzer angezeigt wird.
        /// </summary>
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

        private readonly char[] operatoren = new char[] { '+', '-', '*', '/','=' };

        /// <summary>
        /// Wenn einer der Hauptbuttons betätigt wurde.
        /// </summary>
        public ICommand GenericButtonClickCommand => new ActionCommand(delegate(object o)
        {
            if (o is not RoutedEventArgs {Source: Button knopf })
            {
                return;
            }

            this.AddInput(knopf.Content.ToString() ?? string.Empty);
        });

        private void AddInput(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            if (this.operatoren.Any(op => op.Equals(input[0])))
            {
                this.Eingabe = string.Empty;
            } 
            else
            {
                this.Eingabe += input;
            }

            this.kompletteRechnung += input;
        }

        /// <summary>
        /// Wenn das Fenster Tastatureingaben empfängt.
        /// </summary>
        public ICommand HardwareKeyUpCommand => new ActionCommand(delegate (object o)
        {
            if (o is not KeyEventArgs args)
            {
                return;
            }

            string keyString = new KeyConverter().ConvertToString(args.Key);
            if (keyString.Length > 1 || !char.IsDigit(keyString[0]) || !this.operatoren.Any(op => op.Equals(keyString[0])))
            {
                return;
            }

            this.AddInput(keyString);
        });

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
