using DataAccess;
using Models;
using System;
using System.Collections.Generic;
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
        private IEnumerable<Game> games = new List<Game>();
        private IEnumerable<Genre> genres = new List<Genre>();
        private List<string> selectedGenres = new List<string>();
        private string searchTitle = "";

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

        private void Reload()
        {
            LoadGenres();
            LoadGenres();
        }

        private void LoadGames()
        {
            games = GamesDAO.Instance.GetGames();
            lsGames.ItemsSource = games;
        }

        private void LoadGenres()
        {
            genres = GenresDAO.Instance.GetGenres();
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
            Filter();
        }

        private void Filter()
        {
            games = GamesDAO.Instance.GetGames(searchTitle, selectedGenres.ToArray());
            lsGames.ItemsSource = games;
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
            Filter();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var detailsResult = (new DetailsWindow().ShowDialog());
            if (detailsResult == true)
            {
                Reload();
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
                    Reload();
                }
            }
        }
    }
}