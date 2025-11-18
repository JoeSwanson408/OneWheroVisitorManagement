using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
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
using OnewheroVisitorManagement.Model;
using OnewheroVisitorManagement.ViewModel;

namespace OnewheroVisitorManagement.View
{
    /// <summary>
    /// Interaction logic for Events.xaml
    /// </summary>
    public partial class VisitorEvents : UserControl
    {
        string connectionString = "Data Source=users.db;Version=3;";
        
        public VisitorEvents()
        {
            InitializeComponent();            
        }

        private void BookEvent_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                string argument = clickedButton.Tag.ToString();
                MessageBox.Show($"Argument received: {argument}");
                Booking bookingPage = new Booking(argument);
                VisitorEventsContent.Content = bookingPage;
            }

        }
    }
}
