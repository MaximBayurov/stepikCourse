using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GettingStarted
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this.textBox.Text, this.textBox_Copy.Text);
        }
    }
}
