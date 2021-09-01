using System.Windows;

namespace Taschenrechner
{
    /// <summary>
    ///     Interaction logic for MainWindowView.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        internal static MainWindowView Instance;

        public MainWindowView()
        {
            Instance = this;
            this.InitializeComponent();
        }
    }
}