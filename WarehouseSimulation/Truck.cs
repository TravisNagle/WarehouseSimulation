using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseSimulation
{
    internal class Truck
    {
        public string Driver { get; set; }
        public string DeliveryCompany { get; set; }
        private Stack<Crate> Trailer = new Stack<Crate>();

        public void Load(Crate crate)
        {
            Trailer.Push(crate);
        }

        public Crate Unload()
        {
            return Trailer.Pop();
        }
    }
}
