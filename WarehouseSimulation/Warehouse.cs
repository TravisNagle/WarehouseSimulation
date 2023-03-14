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
            Console.Write("Enter the number of docks you would like to sim: ");
            int numOfDocks = int.Parse(Console.ReadLine());

            for(int i = 0; i < numOfDocks; i++)
            {
                Dock dock = new Dock();
                Docks.Add(dock);
            }

            int maxTime = 48;
            int increment = 0;
            int nameCounter = 0;
            while (increment < maxTime)
            {
                if(Entrance.Count > 0)
                {
                    Truck arrivedTruck = Entrance.Dequeue();
                    Docks[0].JoinLine(arrivedTruck);
                    Docks[0].TotalTrucks++;
                    Docks[0].TotalCrates += arrivedTruck.Trailer.Count;
                }

                if (Docks[0].Line.Count > 0) //For multiple docks, check Docks list if a Dock has no line
                {
                    Docks[0].TimeInUse++;
                    Truck dockedTruck = Docks[0].Line.Peek();

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
                    else if(dockedTruck.Trailer.Count == 0)
                    {
                        Docks[0].SendOff();
                        if(Docks[0].Line.Count != 0)
                        {
                            Console.WriteLine("Truck is empty and another truck is in line");
                        }
                        else
                        {
                            Console.WriteLine("Truck is empty and no other trucks in line");
                        }
                    }
                    Console.WriteLine();                  
                }
                else
                {
                    Docks[0].TimeNotInUse++;
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

            foreach(Dock dock in Docks)
            {
                Console.WriteLine($"Time in use: {dock.TimeInUse}");
                Console.WriteLine($"Time not in use: {dock.TimeNotInUse}");
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

        public int ShortestDock(List<Dock> docks)
        {
            int shortestLine = docks[0].Line.Count;
            int dockIndex = 0;
            for(int i = 1; i < docks.Count; i++)
            {
                if (docks[i].Line.Count < shortestLine)
                {
                    shortestLine = docks[i].Line.Count;
                    dockIndex = i;
                }

            }
            return dockIndex;
        }
    }
}
