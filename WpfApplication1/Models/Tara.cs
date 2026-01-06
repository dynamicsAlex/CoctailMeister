using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WpfApplication1
{
    public class Tara
    {
        public Guid Id { get; set; }
        public string RFID_Metka { get; set; }
        public string Name { get; set; }
        public ushort WeightNetto { get; set; }
        public ushort Volume { get; set; }
        public Guid CoctailId { get; set; }

        public Tara()
        {
            Id = Guid.NewGuid();
        }
    }
}
