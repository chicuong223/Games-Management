using DataAccess.Configuration;
using DataAccess.DatabaseDAO;
using Oracle.ManagedDataAccess.Client;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Utils;

namespace GamesManagementApp
{
    /// <summary>
    /// Interaction logic for ConfigRegisterWindow.xaml
    /// </summary>
    public partial class ConfigRegisterWindow : Window
    {
        public Config config = new Config();
        public ConfigRegisterWindow()
        {
            InitializeComponent();
            this.DataContext = config;
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if(config.DatabaseConfig != null)
            {
                config.DatabaseConfig.Password = txtPassword.Password;
            }
        }

        private void cbSaveType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmbType = (ComboBox)sender;
            var item = cmbType.SelectedItem as ComboBoxItem;
            if (item != null)
            {
                if (item.Content != null)
                {
                    if (item.Content.ToString() == "File")
                    {
                        //if (txtPassword != null) txtPassword.IsEnabled = false;
                        //if (txtPort != null) txtPort.IsEnabled = false;
                        //if (txtServerAddress != null) txtServerAddress.IsEnabled = false;
                        //if (txtService != null) txtService.IsEnabled = false;
                        //if (txtUsername != null) txtUsername.IsEnabled = false;
                        //if (chkSysdba != null) chkSysdba.IsEnabled = false;
                        btnTest.IsEnabled = false;
                        panelDatabase.IsEnabled = false;
                    }
                    else
                    {
                        //if (txtPassword != null) txtPassword.IsEnabled = true;
                        //if (txtPort != null) txtPort.IsEnabled = true;
                        //if (txtServerAddress != null) txtServerAddress.IsEnabled = true;
                        //if (txtService != null) txtService.IsEnabled = true;
                        //if (txtUsername != null) txtUsername.IsEnabled = true;
                        //if (chkSysdba != null) chkSysdba.IsEnabled = true;
                        btnTest.IsEnabled = true;
                        panelDatabase.IsEnabled = true;
                    }
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            //Application.Current.Shutdown();
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = DatabaseUtils.CreateConnectionStringFromConfig(config);
            if (connectionString != null)
            {
                bool connectSuccess = DatabaseUtils.TestConnection(connectionString);
                if (connectSuccess)
                {
                    MessageBox.Show("Connected Successfully!");
                }
                else
                {
                    MessageBox.Show("Failed to connect to the database!");
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbSaveType.SelectedIndex = 0;

            //Adjust window size
            this.Width = SystemParameters.PrimaryScreenWidth * 0.45;
            this.Height = SystemParameters.PrimaryScreenHeight * 0.85;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                XmlUtils.WriteToXml(config,
                System.IO.Path.Combine(Directory.GetCurrentDirectory(), AppConstants.ResourceFolderName),
                AppConstants.ConfigFileName);
                if(config.SaveType == "Database")
                {
                    string connectionString = DatabaseUtils.CreateConnectionStringFromConfig(config);
                    using (OracleConnection connection = DatabaseUtils.MakeConnection(connectionString))
                    {
                        connection.Open();
                        DatabaseUtils.CreateGameTable(connection);
                        DatabaseUtils.CreateGenreTable(connection);
                        DatabaseUtils.InsertGenres(connection);
                    }
                }
                MessageBox.Show("Initialized successfully!");
                this.DialogResult = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
