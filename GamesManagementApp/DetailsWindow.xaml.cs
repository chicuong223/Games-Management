using DataAccess;
using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace GamesManagementApp
{
    /// <summary>
    /// Interaction logic for DetailsWindow.xaml
    /// </summary>
    public partial class DetailsWindow : Window
    {
        private Game game = new Game();
        private bool isUpdating = false;

        public DetailsWindow(Game? game = null, bool isUpdating = false)
        {
            InitializeComponent();
            if (game != null)
            {
                //this.game = game;
                try
                {
                    var findGame = GamesDAO.Instance.FindGameById(game.Id);
                    if (findGame == null)
                    {
                        MessageBox.Show("Could not find game!");
                        this.DialogResult = false;
                        return;
                    }
                    this.game = findGame;
                }
                catch
                {
                    MessageBox.Show("Error loading game!");
                    this.DialogResult = false;
                    return;
                }
            }
            this.isUpdating = isUpdating;
            this.DataContext = this.game;
        }

        private void LoadGenres()
        {
            var genres = GenresDAO.Instance.GetGenres();
            foreach (var item in genres)
            {
                CheckBox cb = new CheckBox();
                cb.Tag = item;
                cb.Content = item.Name;
                cb.FontSize = 16;
                cb.Margin = new Thickness(0, 0, 10, 15);

                //If game genres contain this genre, check the checkbox
                if (game.Genres.Any(g => g.Name.Equals(item.Name)))
                {
                    cb.IsChecked = true;
                }

                lvGenres.Children.Add(cb);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //SetTextBox();
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth * 0.45;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight * 0.75;
            LoadGenres();
        }

        private void GetData()
        {
            game.Title = txtTitle.Text;
            game.ExecutablePath = txtExecutablePath.Text;
            game.ImagePath = txtImagePath.Text;
            foreach (CheckBox cb in lvGenres.Children)
            {
                if (cb != null && cb.IsChecked == true)
                {
                    Genre? genre = cb.Tag as Genre;
                    if (genre != null)
                    {
                        game.Genres.Add(genre);
                    }
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string message = string.Empty;
            message = ValidateData();
            if (message != string.Empty)
            {
                MessageBox.Show(message);
                return;
            }

            GetData();
            try
            {
                if (!isUpdating)
                {
                    game.Id = Guid.NewGuid();
                    GamesDAO.Instance.AddGame(game);
                    message = "Added game successfully!";
                }
                else
                {
                    GamesDAO.Instance.UpdateGame(game);
                    message = "Updated game successfully!";
                }
                MessageBox.Show(message);
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string ValidateData()
        {
            string result = string.Empty;
            if (string.IsNullOrEmpty(txtTitle.Text))
            {
                result += "Title is required!\n";
            }
            if (string.IsNullOrEmpty(txtExecutablePath.Text))
            {
                result += "Executable Path is required!\n";
            }
            return result;
        }

        private void btnImagePath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".jpg";
            dialog.Filter = "JPEG Image (*.jpeg)|*.jpeg|PNG Image (*.png)|*.png|JPG Image (*.jpg)|*.jpg|GIF Image (*.gif)|*.gif";
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                txtImagePath.Text = dialog.FileName;
            }
        }
    }
}