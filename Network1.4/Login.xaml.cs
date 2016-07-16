using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Network1._4
{
    public partial class Page1 : Page
    {
        string username;
        public Page1()
        {
            InitializeComponent();
            
        }
        private void btnLogin(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Main(username));
        }

        private void textBoxLogin_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            textBoxLogin.Text = null;
        }

        private void textBoxLogin_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.NavigationService.Navigate(new Main(username));
            }
            else {
                username = textBoxLogin.Text;
            }
        }
    }
}
