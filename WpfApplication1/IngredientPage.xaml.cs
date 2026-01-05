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
    /// Логика взаимодействия для IngredientPage.xaml
    /// </summary>
    public partial class IngredientPage : Page
    {
        private List<Ingredient> _ingredients;
        private Action _refreshCallback;

        public IngredientPage(Action refreshCallback)
        {
            InitializeComponent();
            _refreshCallback = refreshCallback;
            _ingredients = XmlStorage.LoadIngredients();
            IngredientGrid.ItemsSource = _ingredients;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
          
            // Проверка бизнес-логики
            if (_ingredients.Exists(x => x.ContainerNumber > 7) || _ingredients.Exists(x => x.ContainerNumber < 0))
            {
                MessageBox.Show("Номер ёмкости должен быть в диапазоне [0..7]", "Ошибка",
                   MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            foreach (Ingredient igridient in _ingredients)
            {
                if (_ingredients.FindAll(x => x.ContainerNumber == igridient.ContainerNumber).Count > 1)
                {
                    MessageBox.Show("Номер ёмкости должен быть уникальным", "Ошибка",
                       MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            // Удаляем пустые строки
            _ingredients.RemoveAll(i => i.Name.Equals(String.Empty));

            // Сохраняем данные
            XmlStorage.SaveIngredients(_ingredients);
            MessageBox.Show("Данные сохранены в файл ingredients.xml", "Сохранено",
                MessageBoxButton.OK, MessageBoxImage.Information);

            // Если ингридиенты удалёны, необходимо обновить состав коктелей
            List<Cocktail> coctails = XmlStorage.LoadCocktails();
            foreach (Cocktail coctail in coctails)
            {
                // Пробегаем по всем ингридиентам в коктейле - не существующие удаляем
                foreach (CocktailIngredient ingridient in coctail.Ingredients)
                {
                    if (_ingredients.Find(x => x.Id.Equals(ingridient.IngredientId)) == null)
                        ingridient.IngredientId = Guid.Empty;
                }

                coctail.Ingredients.RemoveAll(x => x.IngredientId.Equals(Guid.Empty));
            }

            XmlStorage.SaveCocktails(coctails);

            _refreshCallback.Invoke();
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
    }
}
