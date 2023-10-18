using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfTicketBooking1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            DateTime currentDate = DateTime.Now.Date;
            CurrentDateTextBlock.Text = "Current Date: " + currentDate.ToString("yyyy-MM-dd");
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (IsUserValid(username, password))
            {
                Window1 window1 = new Window1(username);
                // Set the WindowState property to Maximized to make it full-screen
                window1.WindowState = WindowState.Maximized;
                window1.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Login failed. Please check your credentials.");
            }
        }

        private bool IsUserValid(string username, string password)
        {
            string excelFilePath = "F:\\WpfTicketBooking1 - kopia/TicketBookingDataK.xlsx"; // Path to your Excel file
            FileInfo file = new FileInfo(excelFilePath);

            using (ExcelPackage package = new ExcelPackage(file))
            {
                // Set the LicenseContext for EPPlus
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string storedUsername = worksheet.Cells[row, 1].Text;
                    string storedPassword = worksheet.Cells[row, 2].Text;

                    if (username == storedUsername && password == storedPassword)
                    {
                        return true; // Authentication successful
                    }
                }
            }

            return false; // Authentication failed
        }






        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string username = UsernameTextBox.Text;
                string password = PasswordBox.Password;

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Please fill in both fields.");
                }
                else if (IsUserRegistered(username))
                {
                    MessageBox.Show("Username is already registered.");
                }
                else
                {
                    RegisterUser(username, password);
                    MessageBox.Show("Registration successful!");
                    ClearInputFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private bool IsUserRegistered(string username)
        {
            string excelFilePath = "F:/WpfTicketBooking1 - kopia/TicketBookingDataK.xlsx"; // Path to your Excel file
            FileInfo file = new FileInfo(excelFilePath);
            // Set the LicenseContext for EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string storedUsername = worksheet.Cells[row, 1].Text;

                    if (username == storedUsername)
                    {
                        return true; // User is already registered
                    }
                }
            }

            return false; // User is not registered
        }

        private void RegisterUser(string username, string password)
        {
            string excelFilePath = "F:\\WpfTicketBooking1 - kopia/TicketBookingDataK.xlsx"; // Path to your Excel file
            FileInfo file = new FileInfo(excelFilePath);

            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows + 1;

                worksheet.Cells[rowCount, 1].Value = username;
                worksheet.Cells[rowCount, 2].Value = password;

                package.Save();
            }
        }

        private void ClearInputFields()
        {
            UsernameTextBox.Clear();
            PasswordBox.Clear();
        }
    }
}