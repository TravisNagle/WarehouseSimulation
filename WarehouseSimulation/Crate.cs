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
                Random rand = new Random();
                while(_id.Length < 4)
                {
                    _id += rand.Next(0, 10);
                }
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        public double _price;
        public double Price
        {
            get
            {
                Random rand = new Random();
                _price = rand.NextDouble() * (501 - 50) + 50;
                return _price;
            }
        }
    }
}