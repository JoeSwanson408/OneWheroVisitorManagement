using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for RegisterNewVisitor.xaml
    /// </summary>
    public partial class RegisterNewVisitor : UserControl
    {
       string connectionString = "Data Source=users.db;Version=3;";
        string visitorName;
        string visitorEmail;
        public RegisterNewVisitor(string fullName, string email)
        {
            InitializeComponent();
            visitorName = fullName;
            visitorEmail = email;
        }

        public void Submit_Click(object sender, RoutedEventArgs e)
        {
            var list = this.Checklist.Children.OfType<CheckBox>().Where(x => x.IsChecked == true);
            list.ToList().ForEach(x => Debug.WriteLine(x.Content.ToString()));
            string interests = string.Join(", ", list.Select(x => x.Content.ToString()));
            Debug.Write(interests);
            using (var connection = new System.Data.SQLite.SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Visitors (FullName, ContactEmail, Interests) VALUES (@FullName, @ContactEmail, @Interests);";
                using (var cmd = new System.Data.SQLite.SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@FullName", visitorName);
                    cmd.Parameters.AddWithValue("@ContactEmail", visitorEmail);
                    cmd.Parameters.AddWithValue("@Interests", interests);
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
            MessageBox.Show("Thank you for registering, " + visitorName + "!", "Registration Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            Application.Current.MainWindow.Content = new VisitorNav();
        }
    }
}
