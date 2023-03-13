using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseSimulation
{
    internal class Crate
    {
        public string _id = "";
        public string Id 
        { 
            get
            {
                return _id;
            }
            set
            {
                Random rand = new Random();
                _id = "51432";             
            }
        }
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
