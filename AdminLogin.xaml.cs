using OnewheroVisitorManagement.ViewModel;
using OnewheroVisitorManagement.View;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Navigation;

namespace OnewheroVisitorManagement
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class AdminLogin : Page
    {
        string connectionString = "Data Source=users.db;Version=3;";
        public AdminLogin()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Admins WHERE Username=@Username AND Password=@Password;";
                using (var cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    int Count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (Count > 0)
                    {
                        MessageBox.Show("Login Successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.NavigationService.Navigate(new AdminNav());
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
