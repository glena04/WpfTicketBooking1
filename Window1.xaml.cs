using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfTicketBooking1
{
    public partial class Window1 : Window
    {
        private List<EventDetails> eventsData;
        private string loggedInUsername;
        private List<BookingDetails> bookingData;
        public Window1(string username)
        {
            InitializeComponent();
            Username = username;
            loggedInUsername = username; // Set the loggedInUsername
            DataContext = this; // Set DataContext to enable data binding
            // Attach the SelectionChanged event handler to the TabControl
            TabControl.SelectionChanged += TabControl_SelectionChanged;
            // Load event data when the Window1 is initialized
            LoadEventDataFromExcel();
        }

        public string Username { get; set; }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Add your logout logic here, if any
            // Create a new instance of MainWindow
            MainWindow mainWindow = new MainWindow();
            // Show the MainWindow
            mainWindow.Show();
            // Close the current Window1
            this.Close();
        }

        private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Get the selected tab item
            TabItem selectedTab = TabControl.SelectedItem as TabItem;

            if (selectedTab != null)
            {
                string tabHeaderText = selectedTab.Header as string;

                // Perform actions based on the selected tab
                if (tabHeaderText == "Boka Biljett")
                {
                    // Code to handle the "Boka Biljett" tab
                    LoadEventDataFromExcel();
                    EventListView.ItemsSource = eventsData;
                }
                else if (tabHeaderText == "View Bookings")
                {
                    // Code to handle the "View Bookings" tab
                    LoadBookingDataFromExcel(); // Load booking data
                    ViewBookingsListView.ItemsSource = bookingData.Where(e => e.Username == loggedInUsername);
                }
                // You can add additional conditions and logic for other tabs as needed
            }
        }

        private void LoadEventDataFromExcel()
        {
            // Specify the path to your Excel file
            string excelFilePath = @"F:\WpfTicketBooking1 - kopia/TicketBookingDataK.xlsx";

            try
            {
                // Set the LicenseContext for EPPlus
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                FileInfo fileInfo = new FileInfo(excelFilePath);

                using (ExcelPackage package = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1]; // Assuming data is in the "Events" worksheet
                    // Define a class or data structure to store event data (EventDetails class in this example)
                    eventsData = new List<EventDetails>();

                    // Loop through the rows in the worksheet and populate the events list
                    for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                    {
                        EventDetails eventDetail = new EventDetails
                        {
                            Username = worksheet.Cells[row, 4].Text,
                            Evenemangsnamn = worksheet.Cells[row, 1].Text,
                            Datum = worksheet.Cells[row, 2].Text,
                            Tid = TimeSpan.Parse(worksheet.Cells[row, 3].Text),
                            // Add more properties as needed
                        };
                        eventsData.Add(eventDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading data from Excel: " + ex.Message);
            }
        }

        public class EventDetails
        {
            public string Username { get; set; }
            public string Evenemangsnamn { get; set; }
            public string Datum { get; set; }
            public TimeSpan Tid { get; set; }

            // Add more properties as needed
        }
        private void LoadBookingDataFromExcel()
        {
            // Specify the path to your Excel file
            string excelFilePath = @"F:\WpfTicketBooking1 - kopia/TicketBookingDataK.xlsx";

            try
            {
                // Set the LicenseContext for EPPlus
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                FileInfo fileInfo = new FileInfo(excelFilePath);

                using (ExcelPackage package = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets["Bookings"]; // Use the correct worksheet name
                                                                                        // Define a class or data structure to store booking data (BookingDetails class in this example)
                    bookingData = new List<BookingDetails>();

                    // Loop through the rows in the worksheet and populate the bookings list
                    for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                    {
                        BookingDetails bookingDetail = new BookingDetails
                        {
                            Username = worksheet.Cells[row, 1].Text,
                            Evenemangsnamn = worksheet.Cells[row, 2].Text,
                            Datum = worksheet.Cells[row, 3].Text,
                            Tid = TimeSpan.Parse(worksheet.Cells[row, 4].Text),
                            // Add more properties as needed
                        };
                        bookingData.Add(bookingDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading booking data from Excel: " + ex.Message);
            }
        }

        public class BookingDetails
        {
            public string Username { get; set; }
            public string Evenemangsnamn { get; set; }
            public string Datum { get; set; }
            public TimeSpan Tid { get; set; }
            // Add more properties as needed
        }





        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            //string username = getloggedInUsername (); // Replace with your method to get the currently logged-in user's username
            // Use the loggedInUsername field directly
            string username = loggedInUsername;
            // Load the Excel file that stores booking information
            string excelFilePath = @"F:\WpfTicketBooking1 - kopia/TicketBookingDataK.xlsx";// Path to your Excel file
            
            FileInfo file = new FileInfo(excelFilePath);

            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[2];
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string storedUsername = worksheet.Cells[row, 1].Text;

                    // Check if the username in the Excel row matches the logged-in user's username
                    if (username == storedUsername)
                    {
                        // Cancel the ticket by deleting the row (or marking it as canceled)
                        worksheet.DeleteRow(row);
                        package.Save();
                        MessageBox.Show("Ticket cancellation successful.");
                        return;
                    }
                }
            }

            MessageBox.Show("No tickets found for cancellation.");

        }



    }

}


