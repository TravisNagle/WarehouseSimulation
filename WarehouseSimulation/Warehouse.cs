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
                int dockIndex = ShortestDock(Docks);

                if (Entrance.Count > 0)
                {
                    Truck arrivedTruck = Entrance.Dequeue();
                    Docks[dockIndex].JoinLine(arrivedTruck);
                    Docks[dockIndex].TotalTrucks++;
                    Docks[dockIndex].TotalCrates += arrivedTruck.Trailer.Count;
                }

                foreach(Dock dock in Docks)
                {
                    if (dock.Line.Count > 0) //For multiple docks, check Docks list if a Dock has no line
                    {
                        int index = Docks.IndexOf(dock);
                        Truck dockedTruck = dock.Line.Peek();

                        Crate unloadedCrate = dockedTruck.Unload();
                        Console.WriteLine($"Time Unloaded: {increment}");
                        Console.WriteLine($"Driver: {dockedTruck.Driver}");
                        Console.WriteLine($"Company: {dockedTruck.DeliveryCompany}");
                        Console.WriteLine($"Crate ID: {unloadedCrate.Id}");
                        Console.WriteLine($"Crate Value: {unloadedCrate.Price}");
                        Console.WriteLine($"Dock used: {index}");

                        dock.TimeInUse++;

                        if (dockedTruck.Trailer.Count != 0)
                        {
                            Console.WriteLine("Truck has more crates");
                        }
                        else if (dockedTruck.Trailer.Count == 0)
                        {
                            dock.SendOff();
                            if (dock.Line.Count != 0)
                            {
                                dock.TotalTrucks++;
                                Console.WriteLine("Truck is empty and another truck is in line");
                            }
                            else
                            {
                                dock.TotalTrucks++;
                                Console.WriteLine("Truck is empty and no other trucks in line");
                            }
                        }
                        Console.WriteLine();
                    }
                    else
                    {
                        dock.TimeNotInUse++;
                    }
                }

                /*
                if (Docks[dockIndex].Line.Count > 0) //For multiple docks, check Docks list if a Dock has no line
                {
                    //Docks[dockIndex].TimeInUse++;
                    Truck dockedTruck = Docks[dockIndex].Line.Peek();

                    Crate unloadedCrate = dockedTruck.Unload();
                    Console.WriteLine($"Time Unloaded: {increment}");
                    Console.WriteLine($"Driver: {dockedTruck.Driver}");
                    Console.WriteLine($"Company: {dockedTruck.DeliveryCompany}");
                    Console.WriteLine($"Crate ID: {unloadedCrate.Id}");
                    Console.WriteLine($"Crate Value: {unloadedCrate.Price}");
                    Console.WriteLine($"Dock used: {dockIndex}");

                    if (dockedTruck.Trailer.Count != 0)
                    {
                        Console.WriteLine("Truck has more crates");
                    }
                    else if(dockedTruck.Trailer.Count == 0)
                    {
                        Docks[dockIndex].SendOff();
                        if(Docks[dockIndex].Line.Count != 0)
                        {
                            Docks[dockIndex].TotalTrucks++;
                            Console.WriteLine("Truck is empty and another truck is in line");
                        }
                        else
                        {
                            Docks[dockIndex].TotalTrucks++;
                            Console.WriteLine("Truck is empty and no other trucks in line");
                        }
                    }
                    Console.WriteLine();                  
                }
                else
                {
                    //Docks[dockIndex].TimeNotInUse++;
                }
                */

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
                    Console.WriteLine("A truck has entered the entrance");
                }
                increment++;
            }

            foreach(Dock dock in Docks)
            {
                Console.WriteLine($"Time in use: {dock.TimeInUse}");
                Console.WriteLine($"Time not in use: {dock.TimeNotInUse}");
                Console.WriteLine($"Trucks processed: {dock.TotalTrucks}");
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
