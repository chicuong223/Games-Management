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
                this.game = game;
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
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            GetData();
            string message = string.Empty;
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
    }
}