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

namespace WpfApplication1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadCocktailButtons();
        }

        private void OpenTara_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new TaraPage(RefreshCocktailButtons));
        }

        private void OpenIngredient_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new IngredientPage(RefreshCocktailButtons));
        }

        private void OpenCocktail_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new CocktailPage(RefreshCocktailButtons));
        }

        private void LoadCocktailButtons()
        {
            List<Cocktail> cocktails = XmlStorage.LoadCocktails();
            foreach (Cocktail cocktail in cocktails)
            {
                CreateCocktailButton(cocktail);
            }
        }

        public void RefreshCocktailButtons()
        {
            CocktailButtonsPanel.Children.Clear();
            LoadCocktailButtons();
        }

        private void CreateCocktailButton(Cocktail cocktail)
        {
            Button btn = new Button
            {
                Content = cocktail.Name,
                Margin = new Thickness(8),
                Padding = new Thickness(10),
                Tag = cocktail,
                Width = 180,
                Height = 40
            };
            btn.Click += (s, ev) => ShowCocktailDetails(cocktail);
            CocktailButtonsPanel.Children.Add(btn);
        }

        private void ShowCocktailDetails(Cocktail cocktail)
        {
            string ingredients = "";
            foreach (CocktailIngredient ci in cocktail.Ingredients)
            {
                ingredients += string.Format("{0} ({1}%, ёмкость #{2})\n", ci.Name, ci.Percentage, ci.ContainerNumber);
            }

            MessageBox.Show(
                string.Format("Коктейль: {0}\n\nИнгредиенты:\n{1}", cocktail.Name, ingredients),
                "Наливаю",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}
