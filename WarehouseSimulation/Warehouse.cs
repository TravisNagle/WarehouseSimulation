using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseSimulation
{
    internal class Warehouse
    {
        private List<Dock> Docks = new List<Dock>();
        private Queue<Truck> Entrance = new Queue<Truck>();

        public void Run()
        {
            Random rand = new Random();
            Dock dock1 = new Dock();

            int maxTime = 48;
            int increment = 0;
            int nameCounter = 0;
            while (increment <= maxTime)
            {
                if(Entrance.Count > 0)
                {
                    Truck arrivedTruck = Entrance.Dequeue();
                    dock1.JoinLine(arrivedTruck);
                    dock1.TotalTrucks++;
                    dock1.TotalCrates += arrivedTruck.Trailer.Count;
                }

                if(dock1.Line.Count > 0) 
                {
                    dock1.TimeInUse++;
                    Truck dockedTruck = dock1.Line.Peek();

                    if (dockedTruck.Trailer.Count == 0) 
                    {
                        break;
                    }

                    Crate unloadedCrate = dockedTruck.Unload();
                    Console.WriteLine($"Time Unloaded: {increment}");
                    Console.WriteLine($"Driver: {dockedTruck.Driver}");
                    Console.WriteLine($"Company: {dockedTruck.DeliveryCompany}");
                    Console.WriteLine($"Crate ID: {unloadedCrate.Id}");
                    Console.WriteLine($"Crate Value: {unloadedCrate.Price}");

                    if (dockedTruck.Trailer.Count != 0)
                    {
                        Console.WriteLine("Truck has more crates");
                    }
                    
                    if(dockedTruck.Trailer.Count == 0)
                    {
                        dock1.SendOff();
                        if (dock1.Line.Contains(dockedTruck) == false)
                        {
                            Console.WriteLine("Truck has no more crates and new truck already in dock ");
                        }
                        else
                        {
                            Console.WriteLine("Truck has no more crates and no new truck already in dock");
                        }
                    }
                }

                int arrivalTime = rand.Next(increment, maxTime + 1);
                if (arrivalTime == increment)
                {
                    Console.WriteLine("Arrival Time " + arrivalTime);
                    Truck truck = new Truck();
                    truck.Driver = $"Driver {nameCounter}";
                    truck.DeliveryCompany = $"Company {nameCounter++}";

                    int crateCount = rand.Next(1, 6);
                    while (crateCount > 0)
                    {
                        Crate crate = new Crate();
                        truck.Load(crate);
                        crateCount--;
                    }
                    Entrance.Enqueue(truck);
                }
                increment++;
            }
        }

        public bool DockEmpty(Dock dock)
        {
            if(dock.Line.Peek() == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
