using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using T9try.ViewModels;

namespace T9try
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var a = (DataContext as MainWindowViewModel);
            Text.TextChanged += a.TextBox_TextChanged;
            this.Closing += a.Window_Closing;
        }

        private void TextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {

            if (!(Text.Text == "" || Text.Text.Last() == ' '))
            {
                var a = Text.Text.Split('.');
                var b = a.Last().Split(' ');
                var c = b.Last();
                Text.Text = Text.Text.Remove(Text.Text.Length - c.Length);

            }
            Text.Text += (sender as TextBlock).Text+" ";
            Text.CaretIndex = Text.Text.Length;
        }

        
    }
}
