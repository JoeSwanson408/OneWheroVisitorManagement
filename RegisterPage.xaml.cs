using OnewheroVisitorManagement.View;
using OnewheroVisitorManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Navigation;


namespace OnewheroVisitorManagement
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : UserControl
    {
        string connectionString = "Data Source=users.db;Version=3;";
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (txtFullName == null)
            {
                MessageBox.Show("Please enter your full name", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (txtEmail == null)
            {
                MessageBox.Show("Please enter your email", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            } else
            {
                string fullName = txtFullName.Text.Trim();
                string email = txtEmail.Text.Trim();

                try
                {
                    using (var connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();
                        string query = "SELECT COUNT(*) FROM Visitors WHERE ContactEmail=@ContactEmail;";
                        using (var cmd = new SQLiteCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@ContactEmail", email);

                            int Count = Convert.ToInt32(cmd.ExecuteScalar());
                            if (Count > 0)
                            {
                                MessageBox.Show("Welcome back " + fullName + "! We found your previous registration and filled in your details for you.", "Welcome Back", MessageBoxButton.OK, MessageBoxImage.Information);
                                RegisterContent.Content = new VisitorNav();

                            }
                            else
                            {
                                RegisterContent.Content = new RegisterNewVisitor(fullName, email);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Registration failed. Please try again", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Console.WriteLine(ex.Message);
                    txtFullName.Clear();
                    txtEmail.Clear();
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
