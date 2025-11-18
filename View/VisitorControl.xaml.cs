using OnewheroVisitorManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;


namespace OnewheroVisitorManagement.View
{
    public partial class VisitorControl : UserControl
    {
        string connectionString = "Data Source=users.db;Version=3;";
        public VisitorControl()
        {
            InitializeComponent();
        }

        private void ViewTable_Click(object sender, RoutedEventArgs e)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT * FROM Visitors;";
                using (var adapter = new SQLiteDataAdapter(selectQuery, connection))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridVisitors.ItemsSource = dt.DefaultView;
                }
            }
        }

        private void AddVisitor_Click(object sender, RoutedEventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string contact = txtContact.Text.Trim();
            string interests = txtInterests.Text.Trim();

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(contact) || string.IsNullOrEmpty(interests))
            {
                MessageBox.Show("Please fill the information again", "Input Error",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string checkVisitor = "SELECT COUNT(*) FROM Visitors WHERE FullName=@FullName AND ContactEmail=@ContactEmail AND Interests=@Interests;";
                    using (var cmd = new SQLiteCommand(checkVisitor, connection))
                    {
                        AddVisitorParameters(cmd, fullName, contact, interests);
                        int Count = Convert.ToInt32(cmd.ExecuteScalar());
                        if (Count == 0)
                        {
                            string insertVisitor = "INSERT INTO Visitors(FullName, ContactEmail, Interests) VALUES (@FullName, @ContactEmail, @Interests);";
                            using (var insertCmd = new SQLiteCommand(insertVisitor, connection))
                            {
                                AddVisitorParameters(insertCmd, fullName, contact, interests);
                                insertCmd.ExecuteNonQuery();
                            }
                            MessageBox.Show("Visitor Added Successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                            txtFullName.Clear();
                            txtContact.Clear();
                            txtInterests.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Visitor already exists!", "Add Visitor failed", MessageBoxButton.OK, MessageBoxImage.Error);
                            txtFullName.Clear();
                            txtContact.Clear();
                            txtInterests.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding visitor: " + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Helper to avoid repeating AddWithValue calls
        private static void AddVisitorParameters(SQLiteCommand cmd, string fullName, string contact, string interests)
        {
            cmd.Parameters.AddWithValue("@FullName", fullName);
            cmd.Parameters.AddWithValue("@ContactEmail", contact);
            cmd.Parameters.AddWithValue("@Interests", interests);
        }

        private void DeleteVisitor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dataGridVisitors.SelectedItem == null)
                {
                    MessageBox.Show("Please select a visitor record to delete", "No selection is made", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;

                }

                //1: get the selected row

                //You are selecting the row in the DataGrid and coverting it to DataTable

                DataRowView row = (DataRowView)dataGridVisitors.SelectedItem;
                int visitorID = Convert.ToInt32((row["VisitorID"]));


                //2: confirm deletion
                MessageBoxResult result = MessageBox.Show(
                    "Are you sure you want to delete this row/record", "Confirm Deletion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                    );

                if (result == MessageBoxResult.No)
                {
                    return;
                }

                //3: delete from database

                using (var connection = new SQLiteConnection(connectionString)) //connection to database
                {
                    connection.Open();
                    string deleteQuery = "DELETE FROM Visitors WHERE VisitorID = @VisitorID";
                    using (var cmd = new SQLiteCommand(deleteQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@VisitorID", visitorID);
                        cmd.ExecuteNonQuery();

                        ViewTable_Click(null, null);
                        MessageBox.Show("Visitor deleted successfully", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error deleting visitor" + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void UpdateVisitor_Click(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string contact = txtContact.Text.Trim();
            string interests = txtInterests.Text.Trim();
            try
            {
                if (dataGridVisitors.SelectedItem == null)
                {
                    MessageBox.Show("Please select a visitor record to update", "No selection is made", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;

                }

                DataRowView row = (DataRowView)dataGridVisitors.SelectedItem;
                int visitorID = Convert.ToInt32((row["VisitorID"]));

                //2: confirm deletion
                MessageBoxResult result = MessageBox.Show(
                    "Are you sure you want to update this row/record", "Confirm update",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                    );

                if (result == MessageBoxResult.No)
                {
                    return;
                }


                string query = @"
                    UPDATE Visitors
                     SET FullName = @FullName AND
                     ContactEmail = @ContactEmail AND
                     Interests = @Interests
                     WHERE VisitorID = @VisitorID";

                using (var conn = new SQLiteConnection(connectionString))
                using (var cmd = new SQLiteCommand(query, conn))

                {
                    AddVisitorParameters(cmd, fullName, contact, interests);
                    cmd.Parameters.AddWithValue("@VisitorID", visitorID);
                    //cmd.Parameters.AddWithValue("@FirstName", txtFullName.Text);
                    //cmd.Parameters.AddWithValue("@Contact", txtContact.Text);
                    //cmd.Parameters.AddWithValue("@Interests", txtInterests.Text);
                    cmd.Parameters.AddWithValue("@FullName", fullName);
                    cmd.Parameters.AddWithValue("@ContactEmail", contact);
                    cmd.Parameters.AddWithValue("@Interests", interests);
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error  visitor: " + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

