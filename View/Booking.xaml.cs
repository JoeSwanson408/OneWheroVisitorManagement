using OnewheroVisitorManagement.Model;
using OnewheroVisitorManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
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
    /// Interaction logic for Booking.xaml
    /// </summary>
    public partial class Booking : UserControl
    {
        public int CurrentNum
        {
            get { return Convert.ToInt32(txtNumTickets.Text); }
            set { txtNumTickets.Text = value.ToString(); }
        }
        public int ThisEventID;
        public double cost;
        public string bookingDate, eventName, visitorID, fullName, contactEmail;
        string connectionString = "Data Source=users.db;Version=3;";

        public double TotalCost
        {
            get { return CurrentNum * cost; }
            set { txtTotalCost.Text = "Total Cost: " + value.ToString(); }
        }

        public Booking(string eventID)
        {

            InitializeComponent();
            DataContext = this;
            ThisEventID = Convert.ToInt32(eventID);
            Debug.WriteLine("ThisEventID: " + ThisEventID);
            CurrentNum = 0;
            TotalCost = 0;
            FindEvent();
        }

        private void FindEvent()
        {
            foreach (Event ev in MainWindow.EventsList)
            {
                Debug.WriteLine(ev.EventName);
                Debug.WriteLine(ev.EventDates);
                Debug.WriteLine(ev.EventDesc);
                Debug.WriteLine(ev.EventCost);
                if (ev.EventID == ThisEventID)
                {
                    Debug.WriteLine(ThisEventID);
                    Debug.WriteLine(ev.EventID);
                    cost = Convert.ToInt32(ev.EventCost);
                    txtBlockCost.Text = "Cost per Ticket: $" + cost;
                    bookingDate = ev.EventDates;
                    eventName = ev.EventName;
                    txtEventName.Text = ev.EventName;

                }

            }
            Debug.Write(bookingDate);
            Debug.Write("$" + cost);
            Debug.Write(eventName);
        }
        public void Decr_Click(object sender, RoutedEventArgs e)
        {
            CurrentNum--;
            TotalCost = CurrentNum * cost;

            if (CurrentNum <= 0)
            {
                CurrentNum = 0;
                TotalCost = CurrentNum * cost;
            }
        }

        public void Incr_Click(object sender, RoutedEventArgs e)
        {
            CurrentNum++;
            Debug.Write(CurrentNum);
            TotalCost = CurrentNum * cost;
        }

        public void ConfirmBook_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentNum == 0)
            {
                MessageBox.Show("Please fill in all fields.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (txtEmail == null)
            {
                MessageBox.Show("Please enter your email", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (txtFullName.Text == null)
            {
                MessageBox.Show("Please enter your email", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {

                fullName = txtFullName.Text.Trim();
                contactEmail = txtEmail.Text.Trim();

                FindUser(contactEmail);
            }
        }

        public DataTable GetDbVisitorID(string email)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                string query = $"SELECT * FROM Visitors WHERE ContactEmail='{email}'";

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    visitorID = dataTable.Rows[0]["VisitorID"].ToString();
                    Debug.Write("Visitor Id: " + visitorID);
                }
                return dataTable;
            }
        }

        public void FindUser(string email)
        {
            try
            {
                GetDbVisitorID(email);
                if (visitorID != null)
                {
                    MessageBox.Show("Welcome back! We found your previous registration and filled in your details for you.", "Welcome Back", MessageBoxButton.OK, MessageBoxImage.Information);
                    InsertBooking();
                    
                }
                else
                {
                    if (CollapseInterests.Visibility == Visibility.Collapsed)
                    {
                        CollapseInterests.Visibility = Visibility.Visible;
                    }
                    ConfirmBookBtn.Visibility = Visibility.Collapsed;
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Registration failed. Please try again", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Debug.WriteLine(ex.Message);
                txtFullName.Clear();
                txtEmail.Clear();
            }
        }

        public void Submit_Click(object sender, RoutedEventArgs e)
        {
            RegisterNewUser(contactEmail, fullName);
        }

        public void RegisterNewUser(string visitorEmail, string visitorName)
        {
            var list = this.Checklist.Children.OfType<CheckBox>().Where(x => x.IsChecked == true);
            list.ToList().ForEach(x => Debug.WriteLine(x.Content.ToString()));
            string interests = string.Join(", ", list.Select(x => x.Content.ToString()));
            Debug.Write(interests);
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Visitors (FullName, ContactEmail, Interests) VALUES (@FullName, @ContactEmail, @Interests);";
                using (var cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@FullName", visitorName);
                    cmd.Parameters.AddWithValue("@ContactEmail", visitorEmail);
                    cmd.Parameters.AddWithValue("@Interests", interests);
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
            MessageBox.Show("Thank you for registering, " + visitorName + "!", "Registration Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            GetDbVisitorID(visitorEmail);
            InsertBooking();
        }

        public void InsertBooking()
        {
            using (var connect = new SQLiteConnection(connectionString))
            {
                connect.Open();
                string insertBookingQuery = "INSERT INTO Bookings (VisitorID, EventID, BookingDate, NumTickets, TotalCost) VALUES (@VisitorID, @EventID, @BookingDate, @NumTickets, @TotalCost);";
                using (var insertCmd = new SQLiteCommand(insertBookingQuery, connect))
                {
                    insertCmd.Parameters.AddWithValue("@VisitorID", visitorID);
                    insertCmd.Parameters.AddWithValue("@EventID", ThisEventID);
                    insertCmd.Parameters.AddWithValue("@BookingDate", bookingDate);
                    insertCmd.Parameters.AddWithValue("@NumTickets", CurrentNum);
                    insertCmd.Parameters.AddWithValue("@TotalCost", TotalCost);
                    insertCmd.ExecuteNonQuery();
                }
                MessageBox.Show($"Booking successful! Total cost: ${TotalCost}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Application.Current.MainWindow.Content = new VisitorNav();
            }
        }
    }
}
