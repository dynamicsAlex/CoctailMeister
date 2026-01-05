using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;


namespace WpfApplication1
{
    public static class XmlStorage
    {
        private const string TaraFile = "tara.xml";
        private const string IngredientFile = "ingredients.xml";
        private const string CocktailFile = "cocktails.xml";

        public static List<Tara> LoadTara()
        {
            if (!File.Exists(TaraFile)) return new List<Tara>();
            var serializer = new XmlSerializer(typeof(List<Tara>));
            using (var reader = new StreamReader(TaraFile))
            {
                return (List<Tara>)serializer.Deserialize(reader);
            }
        }

        public static void SaveTara(List<Tara> data)
        {
            var serializer = new XmlSerializer(typeof(List<Tara>));
            using (var writer = new StreamWriter(TaraFile))
            {
                serializer.Serialize(writer, data);
            }
        }

        public static List<Ingredient> LoadIngredients()
        {
            if (!File.Exists(IngredientFile)) return new List<Ingredient>();
            var serializer = new XmlSerializer(typeof(List<Ingredient>));
            using (var reader = new StreamReader(IngredientFile))
            {
                return (List<Ingredient>)serializer.Deserialize(reader);
            }
        }

        public static void SaveIngredients(List<Ingredient> data)
        {
            var serializer = new XmlSerializer(typeof(List<Ingredient>));
            using (var writer = new StreamWriter(IngredientFile))
            {
                serializer.Serialize(writer, data);
            }
        }

        public static List<Cocktail> LoadCocktails()
        {
            if (!File.Exists(CocktailFile)) return new List<Cocktail>();
            var serializer = new XmlSerializer(typeof(List<Cocktail>));
            using (var reader = new StreamReader(CocktailFile))
            {
                return (List<Cocktail>)serializer.Deserialize(reader);
            }
        }

        public static void SaveCocktails(List<Cocktail> data)
        {
            var serializer = new XmlSerializer(typeof(List<Cocktail>));
            using (var writer = new StreamWriter(CocktailFile))
            {
                serializer.Serialize(writer, data);
            }
        }
    }
}