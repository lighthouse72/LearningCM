namespace QuickRenameTool.Shell
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for ShelView.xaml
    /// </summary>
    public partial class ShellView : Window
    {
        public ShellView()
        {
            InitializeComponent();
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            // This can be done different but for now its ok.
            this.Close();
        }

    }
}
