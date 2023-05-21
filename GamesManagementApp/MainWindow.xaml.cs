using DataAccess;
using DataAccess.FileDAO;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
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
using Utils;

namespace GamesManagementApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private IEnumerable<Game> games = new List<Game>();
        private IEnumerable<Genre> genres = new List<Genre>();

        private List<string> selectedGenres = new List<string>();
        private string searchTitle = "";
        private ObservableCollection<Game> gameObservable = new ObservableCollection<Game>();
        //private FileGamesDAO Globals.GamesDAO = new FileGamesDAO();
        private FileGenresDAO genresDAO = new FileGenresDAO();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadGenres();
            LoadGames();
            this.MinWidth = filterGrid.ActualWidth + gamesGrid.ActualWidth;
            //this.Height = SystemParameters.PrimaryScreenHeight * 0.85;
            //lvGenres.ItemsSource = genres;
        }

        private void reload()
        {
            //Globals.GamesDAO.ReloadGames();
            Cache.Reload();
            LoadGenres();
            LoadGames();
            btnDelete.IsEnabled = false;
        }

        private void LoadGames()
        {
            //Error: Cache.Games are modified with gameObservable
            //gameObservable = new ObservableCollection<Game>(Cache.Games);
            //IEnumerable<Game> games = Globals.GamesDAO.GetGames();

            gameObservable.Clear();

            foreach(var game in Cache.Games)
            {
                Game observeGame = new Game(game.Id, game.Title, game.ImagePath, game.ExecutablePath, game.Genres);
                gameObservable.Add(observeGame);
            }

            //set image
            foreach (var game in gameObservable)
            {
                if (!File.Exists(game.ImagePath))
                {
                    //game.ImagePath = "C:\\Users\\GIGABYTE\\OneDrive\\Pictures\\cyberpunkcity.jpg";
                    game.ImagePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), AppConstants.ResourceFolderName, AppConstants.DefaultImageFileName);
                }
            }
            //gameObservable = new ObservableCollection<Game>(games);
            lsGames.ItemsSource = gameObservable;
        }

        private void LoadGenres()
        {
            panelGenres.Children.Clear();
            genres = genresDAO.GetGenres();
            foreach (var genre in genres)
            {
                CheckBox cb = new CheckBox();
                cb.Tag = genre;
                cb.Content = genre.Name;
                cb.Margin = new Thickness(5);
                cb.FontSize = 16;
                cb.Click += checkbox_Click;
                panelGenres.Children.Add(cb);
            }
        }

        private void checkbox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            string? content = cb.Content.ToString();
            if (!string.IsNullOrEmpty(content))
            {
                if (cb.IsChecked == true)
                {
                    selectedGenres.Add(content);
                }
                else
                {
                    if (selectedGenres.Contains(content))
                    {
                        selectedGenres.Remove(content);
                    }
                }
            }
            filter();
        }

        private void filter()
        {
            //gameObservable = new(Globals.GamesDAO.GetGames(searchTitle, selectedGenres.ToArray()));
            gameObservable.Clear();
            var filteredGames = Globals.GamesDAO.GetGames(searchTitle, selectedGenres.ToArray());
            foreach (var searchedGame in filteredGames)
            {
                Game game = new Game(searchedGame.Id, searchedGame.Title, searchedGame.ImagePath, searchedGame.ExecutablePath, searchedGame.Genres);
                if (!FileUtils.FileExists(game.ImagePath))
                {
                    game.ImagePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), AppConstants.ResourceFolderName, AppConstants.DefaultImageFileName);
                }
                gameObservable.Add(game);
            }
            lsGames.ItemsSource = gameObservable;
            //lsGames.SelectedItem = null;
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Game? game = button.DataContext as Game;
            if (game != null)
            {
                try
                {
                    ExecuteFileUtils.Execute(game.ExecutablePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchTitle = txtSearch.Text;
            filter();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var detailsResult = (new DetailsWindow().ShowDialog());
            if (detailsResult == true)
            {
                reload();
            }
        }

        private void btnDetails_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Game? game = button.DataContext as Game;
            if (game != null)
            {
                var detailsResult = (new DetailsWindow(game, true).ShowDialog());
                if (detailsResult == true)
                {
                    reload();
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var game = (Game)lsGames.SelectedItem;
            if (game != null)
            {
                var confirm = MessageBox.Show("Are you sure?", "Delete game", MessageBoxButton.YesNo);
                if (confirm == MessageBoxResult.Yes)
                {
                    try
                    {
                        Globals.GamesDAO.DeleteGame(game);
                        MessageBox.Show("Deleted game successfully");
                        reload();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void lsGames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lsGames.SelectedItem != null)
            {
                btnDelete.IsEnabled = true;
            }
            else
            {
                btnDelete.IsEnabled = false;
            }
        }

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                reload();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}