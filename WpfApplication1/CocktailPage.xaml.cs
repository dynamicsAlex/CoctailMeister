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
using System.Collections.ObjectModel;


namespace WpfApplication1
{
    /// <summary>
    /// Логика взаимодействия для CocktailPage.xaml
    /// </summary>
    public partial class CocktailPage : Page
    {
        private List<Cocktail> _cocktails;
        private List<Ingredient> _ingredients;
        private Cocktail _currentCocktail;
        private Action _refreshCallback;

        public CocktailPage(Action refreshCallback)
        {
            InitializeComponent();
            InitializeData();

            _refreshCallback = refreshCallback;
        }

        private void InitializeData()
        {
            _cocktails = XmlStorage.LoadCocktails();
            _ingredients = XmlStorage.LoadIngredients();

            CocktailGrid.ItemsSource = _cocktails;

            _ingredients.Add(new Ingredient(Guid.Empty)); //Пустой, для удаления ингридиентов
            comboBoxColum.ItemsSource = _ingredients;

        }

        private void SaveCocktail_Click(object sender, RoutedEventArgs e)
        {
            // Удаляем коктейли без названия
            _cocktails.RemoveAll(c => string.IsNullOrEmpty(c.Name));

            // Удаляем ингридиенты, если их удалили
            foreach (Cocktail coctail in _cocktails)
            {
                coctail.Ingredients.RemoveAll(x => x.IngredientId.Equals(Guid.Empty));
            }

            // Проверка бизнес-логики
            foreach (Cocktail coctail in _cocktails)
            {
                if (coctail.Ingredients.Find(x => x.Percentage <= 0) != null)
                {
                    MessageBox.Show("% Ингридиента должен быть больше 0", "Ошибка",
                   MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (coctail.Ingredients.Sum(x => x.Percentage) > 100)
                {
                    MessageBox.Show("Сумма всех ингридиентов не может быть больше 100%", "Ошибка",
                   MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            XmlStorage.SaveCocktails(_cocktails);
            MessageBox.Show("Данные сохранены в файл cocktails.xml", "Сохранено",
                MessageBoxButton.OK, MessageBoxImage.Information);

            // Обновляем контролы
            InitializeData();

            _refreshCallback.Invoke();
        }

        private void DeleteCocktail_Click(object sender, RoutedEventArgs e)
        {
            if (CocktailGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите коктейль для удаления!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var cocktailToDelete = (Cocktail)CocktailGrid.SelectedItem;
            _cocktails.Remove(cocktailToDelete);

            XmlStorage.SaveCocktails(_cocktails);
            

            // Обновляем UI
            CocktailGrid.ItemsSource = _cocktails;
            MessageBox.Show("Коктейль удалён!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _currentCocktail = (Cocktail)CocktailGrid.SelectedItem;

            if (_currentCocktail == null)
            {
                MessageBox.Show(
                "Коктейль не выбран", "Внимание",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
                return;
            }

            string ingredients = "";
            foreach (CocktailIngredient ci in _currentCocktail.Ingredients)
            {
                ingredients += string.Format("{0} ({1}%)\n", ci.Name, ci.Percentage);
            }

            MessageBox.Show(
                string.Format("Коктейль: {0}\n\nИнгредиенты:\n{1}", _currentCocktail.Name, ingredients),
                "Информация о коктейле",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        /// <summary>
        /// закрываем страницу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Content = null;
        }


        /// <summary>
        /// При смене выбраной яцейки обновляем источник данных компонента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CocktailGrid_SelectedCellsChanged_1(object sender, SelectedCellsChangedEventArgs e)
        {
            if ((CocktailGrid.SelectedItem != null) && (CocktailGrid.SelectedItem.GetType() == typeof(Cocktail)))
            {
                _currentCocktail = (Cocktail)CocktailGrid.SelectedItem;
                if (_currentCocktail != null)
                    CocktailIngridientsGrid.ItemsSource = _currentCocktail.Ingredients;
            }
        }
        
    }
}
