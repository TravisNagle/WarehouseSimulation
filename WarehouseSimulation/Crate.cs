using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseSimulation
{
    internal class Crate
    {
        public string Id { get; set; }
        public double _price;
        public double Price
        {
            get
            {
                return _price;
            }
            set
            {
                Random rand = new Random();
                _price = rand.Next(50, 501);
            }
        }
    }
}
