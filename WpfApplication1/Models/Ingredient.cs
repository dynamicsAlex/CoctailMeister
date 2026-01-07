using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    public class Ingredient
    {
        public Guid Id { get; set; }
        public byte ContainerNumber { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Остаточный объём ингридиента в ёмкости (в граммах)
        /// </summary>
        public ushort ResidualVolume { get; set; }
    public Ingredient()
        {
            Id = Guid.NewGuid();
        }

        public Ingredient(Guid id)
        {
            Id = id;
        }
    }

}
