using DataAccess;
using DataAccess.Configuration;
using DataAccess.DatabaseDAO;
using DataAccess.FileDAO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Utils;

namespace GamesManagementApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly string configPath = Path.Combine(Directory.GetCurrentDirectory(),
               AppConstants.ResourceFolderName,
               AppConstants.ConfigFileName);
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            if (!FileUtils.FileExists(configPath))
            {
                ConfigRegisterWindow configRegisterWindow = new ConfigRegisterWindow();
                var result = configRegisterWindow.ShowDialog();
                if (result == false)
                {
                    return;
                }
            }
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                var config = XmlUtils.ReadFromXml<Config>(configPath);
                if (config == null)
                {
                    MessageBox.Show("Error!");
                    return;
                }
                Globals.Config = config;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            if (Globals.Config != null)
            {
                if (Globals.Config.SaveType == "File")
                {
                    Globals.GamesDAO = new FileGamesDAO();
                }
                else if (Globals.Config.SaveType == "Database")
                {
                    Globals.GamesDAO = new DatabaseGamesDAO();
                }
            }

            Cache.ReloadGenres();
            Cache.ReloadGames();
        }
    }
}