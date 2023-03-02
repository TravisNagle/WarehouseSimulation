using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseSimulation
{
    internal class Warehouse
    {
        private List<Dock> Docks= new List<Dock>();
        private Queue<Truck> Entrance = new Queue<Truck>();

        public void Run()
        {
            Random rand = new Random();
            int timeIncrement = 0;
            int maxTime = 48;
            int totalRevenue;
            int totalCratePrice;
            int totalCost;

            Dock dock = new Dock();
            Docks.Add(dock);

            while(timeIncrement <= maxTime)
            {
                Truck truck = new Truck();
                truck.Driver = "Dave";
                int truckArrivalTime = rand.Next(timeIncrement, maxTime);
                int crateAmount = rand.Next(10);
                while(crateAmount > 0)
                {
                    int cratePrice = rand.Next(50, 501);
                    Crate crate = new Crate();
                    crate.Price = cratePrice;
                    truck.Load(crate);
                    crateAmount--;
                }

                if(timeIncrement == truckArrivalTime)
                {
                    dock.JoinLine(truck);
                    Console.WriteLine($"Time Increment: {timeIncrement}");
                    Console.WriteLine($"Driver: {truck.Driver}");
                }
                timeIncrement++;
            }


            //Time increment that crate was unloaded
            //Truck driver's name
            //Delivery company's name
            //Crates ID #
            //Crate's value
            //3 scenarios
        }
    }
}
