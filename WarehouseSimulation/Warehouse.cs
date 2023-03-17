using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
            bool valid = false;
            int numOfDocks = 0;
            Random rand = new Random();
            StreamWriter writer = null;
            string fileName = "simulationReport.csv";
            string filePath = $"../../../ReportFile/{fileName}";

            while(!valid)
            {
                try
                {
                    Console.Write("Enter the number of docks you would like to sim: ");
                    numOfDocks = int.Parse(Console.ReadLine());
                    valid = true;
                }
                catch(FormatException e)
                {
                    Console.WriteLine("Please enter an integer value.");
                }
            }

            int maxTime = 48;
            int increment = 0;
            int nameCounter = 0;
            string longestDock = "";
            int longestDockLine = 0;
            double truckValue = 0;

            for (int i = 0; i < numOfDocks; i++)
            {
                Dock dock = new Dock();
                dock.Id = $"TWD{i}";
                Docks.Add(dock);
            }

            try
            {
                writer = new StreamWriter(new FileStream(filePath, FileMode.Create));
                writer.WriteLine("Dock ID" + "," + "Time" + "," + "Driver" + "," + "Company" + "," + "Crate ID" + "," + "Crate Value");

                while (increment < maxTime)
                {
                    int dockIndex = ShortestDock(Docks);

                    int arrivalTime = rand.Next(increment, maxTime + 1);
                    Queue<int> times = new Queue<int>();
                    times.Enqueue(arrivalTime);
                    if (times.Peek() == increment)
                    {
                        times.Dequeue();
                        Console.WriteLine("Arrival Time " + arrivalTime);
                        Truck truck = new Truck();
                        truck.Driver = $"Driver {nameCounter}"; //Change to random name
                        truck.DeliveryCompany = $"Company {nameCounter++}"; //Change to random company name

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

                    if (Entrance.Count > 0)
                    {
                        Truck arrivedTruck = Entrance.Dequeue();
                        Docks[dockIndex].JoinLine(arrivedTruck);
                        Docks[dockIndex].TotalTrucks++;

                        for (int i = 0; i < Docks.Count; i++)
                        {
                            if (Docks[i].Line.Count > longestDockLine)
                            {
                                longestDockLine = Docks[i].Line.Count;
                                longestDock = Docks[i].Id;
                            }
                        }
                    }

                    foreach (Dock dock in Docks)
                    {
                        if (dock.Line.Count > 0)
                        {
                            Truck dockedTruck = dock.Line.Peek();
                            Crate unloadedCrate = dockedTruck.Unload();
                            truckValue += unloadedCrate.Price;
                            Docks[dockIndex].TotalCrates++;

                            Console.WriteLine($"Time Unloaded: {increment}");
                            Console.WriteLine($"Driver: {dockedTruck.Driver}");
                            Console.WriteLine($"Company: {dockedTruck.DeliveryCompany}");
                            Console.WriteLine($"Crate ID: {unloadedCrate.Id}");
                            Console.WriteLine($"Crate Value: {String.Format("{0:0.00}", unloadedCrate.Price)}");
                            Console.WriteLine($"Dock used: {dock.Id}");

                            writer.Write($"{dock.Id},");
                            writer.Write($"{increment},");
                            writer.Write($"{dockedTruck.Driver},");
                            writer.Write($"{dockedTruck.DeliveryCompany},");
                            writer.Write($"{unloadedCrate.Id},");
                            writer.Write($"{String.Format("{0:0.00}", unloadedCrate.Price)}");
                            writer.WriteLine();

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
                    increment++;
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if(writer != null)
                {
                    writer.Close();
                }
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

            int truckCounter = 0;
            int crateCounter = 0;
            double crateProfit = 0;
            double averageTimeInUse = 0;

            foreach (Dock dock in Docks)
            {
                truckCounter += dock.TotalTrucks;
                crateCounter += dock.TotalCrates;
                crateProfit += dock.TotalSales;
                averageTimeInUse += dock.TimeInUse;
            }

            Console.WriteLine();
            double totalProfit = totalRevenue - totalCost;
            Console.WriteLine($"Total docks open: {numOfDocks}");
            Console.WriteLine($"Longest dock line: Dock {longestDock}");
            Console.WriteLine($"Total trucks processed: {truckCounter}");
            Console.WriteLine($"Total crates processed: {crateCounter}");
            Console.WriteLine($"Total crate profits: ${String.Format("{0:0.00}", crateProfit)}");
            Console.WriteLine($"Average crate value: ${String.Format("{0:0.00}",crateProfit / crateCounter)}");
            Console.WriteLine($"Average truck value: ${String.Format("{0:0.00}", truckValue / truckCounter)}");
            Console.WriteLine($"Average time in use: {String.Format("{0:0.00}",averageTimeInUse / maxTime)}");
            Console.WriteLine($"Total profit: ${String.Format("{0:0.00}", totalProfit)}");
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
