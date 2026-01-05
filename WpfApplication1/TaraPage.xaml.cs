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
    /// Логика взаимодействия для TaraPage.xaml
    /// </summary>
    public partial class TaraPage : Page
    {
        private List<Tara> _taraList;
        private Action _refreshCallback;
        private List<Cocktail> _coctails;

        public TaraPage(Action refreshCallback)
        {
            InitializeComponent();
            _refreshCallback = refreshCallback;
            _taraList = XmlStorage.LoadTara();
            TaraGrid.ItemsSource = _taraList;
            // Список коктейлей для привязки к таре + 1 пустой для удаления привязки
            _coctails = XmlStorage.LoadCocktails();
            _coctails.Add(new Cocktail(Guid.Empty));
            comboBoxColum.ItemsSource = _coctails;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

            // Проверка бизнес-логики перед сохранением
            if (_taraList == null)
                return;

            if (_taraList.Find(x => (x.Volume <= 0) || (x.WeightNetto <= 0)) != null)
            {
                MessageBox.Show("Необходимо указать Вес и объём тары", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            // Удаляем пустые строки (где не заполнен RFID)
            _taraList.RemoveAll(t => string.IsNullOrEmpty(t.RFID_Metka));

            // Удалим неактуальные гуиды коктейлей
            foreach (Tara tara in _taraList)
            {
                if (_coctails.FindAll(x => x.Id.Equals(tara.CoctailId)).Count == 0)
                    tara.CoctailId = Guid.Empty;
            }
        
            XmlStorage.SaveTara(_taraList);
            MessageBox.Show("Данные сохранены в файл tara.xml", "Сохранено",
                MessageBoxButton.OK, MessageBoxImage.Information);

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
