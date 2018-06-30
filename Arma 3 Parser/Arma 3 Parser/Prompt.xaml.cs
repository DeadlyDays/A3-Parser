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
using System.Windows.Shapes;

namespace Arma_3_Parser
{
    /// <summary>
    /// Interaction logic for Prompt.xaml
    /// </summary>
    public partial class Prompt : Window
    {
        public Prompt(String promptText)
        {
            InitializeComponent();
            lblPrompt.Content = promptText;
        }
        public Prompt()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
