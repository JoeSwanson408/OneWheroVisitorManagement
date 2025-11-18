using System;
using System.Collections.Generic;
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

namespace OnewheroVisitorManagement.View
{
    /// <summary>
    /// Interaction logic for Analytics.xaml
    /// </summary>
    public partial class Analytics : UserControl
    {
        public Analytics()
        {
            InitializeComponent();
        }
        string connectionString = "Data Source=users.db;Version=3;";
        private void ViewTable_Click(object sender, RoutedEventArgs e)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT * FROM Bookings;";
                using (var adapter = new SQLiteDataAdapter(selectQuery, connection))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridBookings.ItemsSource = dt.DefaultView;
                }
            }
        }
    }
}
