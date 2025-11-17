using OnewheroVisitorManagement.View;
using OnewheroVisitorManagement.ViewModel;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OnewheroVisitorManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ObservableCollection<Event> EventsList = new ObservableCollection<Event>();
        public MainWindow()
        {
            InitializeComponent();
            EnsureDatabaseAndTable();
            LoadEvents();
        }

        public string connectionString = "Data Source=users.db;Version=3;";
        

        private void EnsureDatabaseAndTable()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string adminTable = @"CREATE TABLE IF NOT EXISTS Admins(
                    AdminID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL UNIQUE,
                    Password TEXT NOT NULL);";

                string visitorTable = @"CREATE TABLE IF NOT EXISTS Visitors(
                    VisitorID INTEGER PRIMARY KEY AUTOINCREMENT, 
                    FullName TEXT, 
                    ContactEmail TEXT NOT NULL,
                    Interests TEXT);";

                string bookingTable = @"CREATE TABLE IF NOT EXISTS Bookings(
                    BookingID INTEGER PRIMARY KEY AUTOINCREMENT,
                    VisitorID INTEGER,
                    EventID INTEGER,
                    BookingDate DATETIME,
                    NumTickets INTEGER,
                    TotalCost DOUBLE,
                    FOREIGN KEY(EventID) REFERENCES Events(EventID));";

                string eventTable = @"CREATE TABLE IF NOT EXISTS Events(
                    EventID INTEGER PRIMARY KEY AUTOINCREMENT,
                    EventName TEXT,
                    EventDate TEXT,
                    EventDesc TEXT,
                    EventCost DOUBLE,
                    EventImgSrc TEXT);";

                new SQLiteCommand(adminTable, connection).ExecuteNonQuery(); //create table if it doesnt exist
                new SQLiteCommand(visitorTable, connection).ExecuteNonQuery(); //create table if it doesnt exist
                new SQLiteCommand(bookingTable, connection).ExecuteNonQuery(); //create table if it doesnt exist
                new SQLiteCommand(eventTable, connection).ExecuteNonQuery(); //create table if it doesnt exist

                //if there is no admin then create a default admin
                string checkAdmin = "SELECT COUNT(*) FROM Admins WHERE Username='admin';";
                using (var cmd = new SQLiteCommand(checkAdmin, connection))
                {
                    int Count = Convert.ToInt32(cmd.ExecuteScalar()); //returns first column of first row
                    if (Count == 0)
                    {
                        string insertAdmin = "INSERT INTO Admins (Username, Password) VALUES ('admin', 'admin123');";
                        new SQLiteCommand(insertAdmin, connection).ExecuteNonQuery();
                    }
                }

                string checkEvent = "SELECT COUNT(*) FROM Events WHERE EventName='Piranha Petting Zoo';";
                using (var cmd = new SQLiteCommand(checkEvent, connection))
                {
                    int Count = Convert.ToInt32(cmd.ExecuteScalar()); //returns first column of first row
                    if (Count == 0)
                    {
                        string insertEvent = "INSERT INTO Events (EventName, EventDate, EventDesc, EventCost, EventImgSrc) VALUES (" +
                            "'Piranha Petting Zoo', " +
                            "'12-10-2025', " +
                            "'In this event, you can bring your children to pet some piranhas :D'," +
                            " 15.00," +
                            "'/Images/piranhas.jpg');";
                        new SQLiteCommand(insertEvent, connection).ExecuteNonQuery();
                    }
                }

                connection.Close();
            }
        }

        private void LoadEvents()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT * FROM Events;";
                using (var cmd = new SQLiteCommand(selectQuery, connection))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Event ev = new Event
                            {
                                EventID = reader.GetInt32(0),
                                EventName = reader.GetString(1),
                                EventDates = reader.GetString(2),
                                EventDesc = reader.GetString(3),
                                EventCost = reader.GetDouble(4),
                                EventImgSrc = reader.GetString(5)
                            };
                            EventsList.Add(ev);
                        }
                    }
                }
            }
        }

        private void Visitor_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new VisitorNav());
        }

        private void AdminLogin_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AdminLogin());
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new RegisterPage());
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}