using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace WpfApplication1
{
    public class Cocktail
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<CocktailIngredient> Ingredients { get; set; }

        public Cocktail()
        {
            Id = Guid.NewGuid();
            Ingredients = new List<CocktailIngredient>();
        }

        // "Пустой" коктейль для удаления привязки к таре
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
        private List<Ingredient> _ingridients = XmlStorage.LoadIngredients();

        public Guid IngredientId { get; set; }
        public int Percentage { get; set; }

        public string Name {

            get {
                Ingredient ing = (Ingredient)_ingridients.Find(x => x.Id.Equals(IngredientId));
                return ing.Name;
            }
    }

    }
}
