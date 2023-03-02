using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseSimulation
{
    internal class Dock
    {
        public string Id { get; set; }
        private Queue<Truck> Line = new Queue<Truck>();
        private double TotalSales { get; set; }
        private int TotalCrates { get; set; }
        private int TotalTrucks { get; set; }
        private int TimeInUse { get; set; }
        private int TimeNotInUse { get; set; }

        public void JoinLine(Truck truck)
        {
            Line.Enqueue(truck);
        }

        public Truck SendOff()
        {
            return Line.Dequeue();
        }
    }
}
