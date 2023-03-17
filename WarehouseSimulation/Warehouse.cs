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
            int longestDockIndex = 0;
            int longestDockLine = 0;
            int truckCounter = 0;

            while (increment < maxTime)
            {
                int dockIndex = ShortestDock(Docks);

                if (Entrance.Count > 0)
                {
                    Truck arrivedTruck = Entrance.Dequeue();
                    Docks[dockIndex].JoinLine(arrivedTruck);
                    Docks[dockIndex].TotalTrucks++;
                    Docks[dockIndex].TotalCrates += arrivedTruck.Trailer.Count;

                    for (int i = 0; i < Docks.Count; i++)
                    {
                        if (Docks[i].Line.Count > longestDockLine)
                        {
                            longestDockIndex = i;
                        }
                    }
                }

                foreach(Dock dock in Docks)
                {
                    if (dock.Line.Count > 0)
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

                        dock.TotalSales += unloadedCrate.Price;
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

                int arrivalTime = rand.Next(increment, maxTime + 1);
                Queue<int> times = new Queue<int>();
                times.Enqueue(arrivalTime);
                if (times.Peek() == increment)
                {
                    times.Dequeue();
                    Console.WriteLine("Arrival Time " + arrivalTime);
                    Truck truck = new Truck();
                    truck.Driver = $"Driver {nameCounter}";
                    truck.DeliveryCompany = $"Company {nameCounter++}";

                    int crateCount = rand.Next(8, 17);
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

            double totalRevenue = 0;
            double totalCost = 0;

            foreach (Dock dock in Docks)
            {
                double dockCost = dock.TimeInUse * 100;

                Console.WriteLine($"Time in use: {dock.TimeInUse}");
                Console.WriteLine($"Time not in use: {dock.TimeNotInUse}");
                Console.WriteLine($"Trucks processed: {dock.TotalTrucks}");
                Console.WriteLine($"Total dock cost: ${dockCost}");
                Console.WriteLine($"Total crate profits: ${dock.TotalSales}");

                totalRevenue += dock.TotalSales;
                totalCost += dockCost;
            }

            foreach(Dock dock in Docks)
            {
                truckCounter += dock.TotalTrucks;
            }

            Console.WriteLine();
            double totalProfit = totalRevenue - totalCost;
            Console.WriteLine($"Total docks open: {numOfDocks}");
            Console.WriteLine($"Longest dock line: Dock {longestDockIndex}");
            Console.WriteLine($"Total trucks processed: {truckCounter}");
            Console.WriteLine($"Total profit: ${totalProfit}");
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

        public int LongestDock(List<Dock> docks)
        {
            int longestLine = docks[0].Line.Count;
            int dockIndex = 0;
            for(int i = 1; i < docks.Count; i++)
            {
                if (docks[i].Line.Count > longestLine)
                {
                    longestLine = docks[i].Line.Count;
                    dockIndex = i;
                }
            }
            return dockIndex;
        }
    }
}
