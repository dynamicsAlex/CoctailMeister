using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Xml.Serialization;

namespace WpfApplication1
{
    public class Cocktail
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<CocktailIngredient> Ingredients { get; set; }

        /// <summary>
        /// Процент заполнения тары, вычисляемое поле
        /// </summary>
        [XmlIgnore]
        public int? FillingPercentage { 
            get {
                return Ingredients != null ? Ingredients.Sum(x => x.Percentage) : (int?)null;
            }
        }

        public Cocktail()
        {
            Id = Guid.NewGuid();
            Ingredients = new List<CocktailIngredient>();
        }

        /// <summary>
        /// Этот конструктор для создания "Пустого" коктейлья, для удаления привязки к таре
        /// </summary>
        /// <param name="id"></param>
        public Cocktail(Guid id)
        {
            Id = id;
            Ingredients = new List<CocktailIngredient>();
        }
    }

    /// <summary>
    /// Связывает коктейль и его ингридиенты
    /// </summary>
    public class CocktailIngredient
    {
        /// <summary>
        /// Кэшируем справичник ингридиентов
        /// </summary>
        private List<Ingredient> _ingridients;

        public Guid IngredientId { get; set; }
        public byte Percentage { get; set; }

        /// <summary>
        /// Имя ингридиента
        /// </summary>
        public string Name {

            get {
                // Получаем по id
                if(_ingridients == null)
                    _ingridients = XmlStorage.LoadIngredients();
                Ingredient ing = (Ingredient)_ingridients.Find(x => x.Id.Equals(IngredientId));
                return ing.Name;
            }
        }
        
        /// <summary>
        /// Номер ёмкости ингридиента
        /// </summary>
        public byte ContainerNumber { 
            get {
                if (_ingridients == null)
                    _ingridients = XmlStorage.LoadIngredients();
                Ingredient ing = (Ingredient)_ingridients.Find(x => x.Id.Equals(IngredientId));
                return ing.ContainerNumber;
            }
        }

    }
}

