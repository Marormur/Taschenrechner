using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Converters;
using Microsoft.Xaml.Behaviors.Core;
using Taschenrechner.Annotations;

namespace Taschenrechner
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly char[] operatoren = {'+', '-', '*', '/', '='};

        private string eingabe = "0";

        /// <summary>
        ///     Komplette Rechenformel
        /// </summary>
        private string kompletteRechnung = string.Empty;

        /// <summary>
        ///     Die Eingabe, welche dem Nutzer angezeigt wird.
        /// </summary>
        public string Eingabe
        {
            get => this.eingabe;
            set
            {
                if (value == this.eingabe)
                {
                    return;
                }

                this.eingabe = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        ///     Wenn einer der Hauptbuttons betätigt wurde.
        /// </summary>
        public ICommand GenericButtonClickCommand => new ActionCommand(delegate(object o)
        {
            if (o is not RoutedEventArgs {Source: Button knopf})
            {
                return;
            }

            this.AddInput(knopf.Content.ToString() ?? string.Empty);
        });

        /// <summary>
        ///     Wenn das Fenster Tastatureingaben empfängt.
        /// </summary>
        public ICommand HardwareKeyUpCommand => new ActionCommand(delegate(object o)
        {
            if (o is not KeyEventArgs args)
            {
                return;
            }

            string keyString = new KeyConverter().ConvertToString(args.Key) ?? string.Empty;
            if (string.IsNullOrEmpty(keyString) || keyString.Length > 1 || !char.IsDigit(keyString[0]))
            {
                return;
            }

            this.AddInput(keyString);
        });

        public event PropertyChangedEventHandler? PropertyChanged;

        private void AddInput(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            switch (input[0])
            {
                case '=':
                    this.Eingabe = new Rechner().EvaluateExpression(this.kompletteRechnung).ToString(CultureInfo.InvariantCulture);
                    this.kompletteRechnung = this.Eingabe;
                    break;
                case '*' or '+' or '-' or '/':
                    this.kompletteRechnung += input;
                    break;
                default:
                    if (this.operatoren.Any(c => c.Equals(this.kompletteRechnung.Length > 1 ? this.kompletteRechnung[^1] : this.kompletteRechnung[0])))
                    {
                        this.Eingabe = input;
                    }
                    else
                    {
                        this.Eingabe += input;
                        this.kompletteRechnung += input;
                    }

                    break;
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null!) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}