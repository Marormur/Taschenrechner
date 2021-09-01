using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors.Core;
using Taschenrechner.Annotations;

namespace Taschenrechner
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly char[] operatoren = {'+', '-', '*', '/', '='};

        private string eingabe = "0";

        private string? kompletteRechnung;

        /// <summary>
        ///     Komplette Rechenformel
        /// </summary>
        public string KompletteRechnung
        {
            get => this.kompletteRechnung ??= string.Empty;
            set
            {
                if (value == this.kompletteRechnung)
                {
                    return;
                }

                this.kompletteRechnung = value;
                this.NotifyPropertyChanged();
            }
        }

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

        private WindowState fensterStatus = WindowState.Normal;

        public WindowState FensterStatus
        {
            get => this.fensterStatus;
            set
            {
                if (value == this.fensterStatus)
                {
                    return;
                }

                this.fensterStatus = value;
                this.NotifyPropertyChanged();
            }
        }

        public ICommand TitleBarMouseLeftButtonDownCommand =>
            new ActionCommand(o => MainWindowView.Instance.DragMove());

        public ICommand TitleBarCloseButtonCommand => new ActionCommand(o => Environment.Exit(0));

        /// <summary>
        ///     Wenn einer der Hauptbuttons betätigt wurde.
        /// </summary>
        public ICommand GenericButtonClickCommand => new ActionCommand(delegate(object o)
        {
            if (o is not RoutedEventArgs { Source: Button knopf})
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

            if (this.Eingabe == "0")
            {
                this.Eingabe = string.Empty;
            }

            switch (input[0])
            {
                case '=':
                    this.Eingabe = new Rechner().EvaluateExpression(this.KompletteRechnung)
                        .ToString(CultureInfo.InvariantCulture);
                    this.KompletteRechnung += "=";
                    break;
                case '*' or '+' or '-' or '/':
                    if (this.KompletteRechnung.EndsWith('='))
                    {
                        this.KompletteRechnung = this.Eingabe + input;
                    }
                    else
                    {
                        this.KompletteRechnung += input;
                    }
                    break;
                default:
                    if (this.KompletteRechnung.EndsWith('='))
                    {
                        this.KompletteRechnung = string.Empty;
                        this.Eingabe = string.Empty;
                    }

                    if (this.operatoren.Any(c =>
                        c.Equals(this.KompletteRechnung.Length < 1 ? string.Empty :
                            this.KompletteRechnung.Length > 1 ? this.KompletteRechnung[^1] :
                            this.KompletteRechnung[0])))
                    {
                        this.Eingabe = input;
                    }
                    else
                    {
                        this.Eingabe += input;
                    }

                    this.KompletteRechnung += input;
                    break;
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null!) =>
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}