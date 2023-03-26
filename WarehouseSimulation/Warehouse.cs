///////////////////////////////////////////////////////////////////////////////
//
// Author: Travis Nagle, Naglet@etsu.edu
// Course: CSCI-2210-001 - Data Structures
// Assignment: Project 4 - Warehouse Simulation
// Description: Implementation of the Warehouse class that takes care of all the features of the sim
//
///////////////////////////////////////////////////////////////////////////////
namespace WarehouseSimulation
{
    /// <summary>
    /// Implementation of a Warehouse object that handles all features of the simulation
    /// </summary>
    internal class Warehouse
    {
        private List<Dock> Docks = new List<Dock>();
        private Queue<Truck> Entrance = new Queue<Truck>();

        /// <summary>
        /// Run method that runs the sim and displays the UI
        /// </summary>
        public void Run()
        {
            bool valid = false;
            int numOfDocks = 0;
            Random rand = new Random();
            StreamWriter writer = null;
            string fileName = "simulationReport.csv";
            string filePath = $"../../../ReportFile/{fileName}";

            while (!valid)
            {
                try
                {
                    Console.Write("Enter the number of docks you would like to sim: ");
                    numOfDocks = int.Parse(Console.ReadLine());
                    valid = true;
                }
                catch (FormatException e)
                {
                    Console.WriteLine("Please enter an integer value.");
                }
            }

            int maxTime = 48;
            int increment = 0;
            string longestDock = "";
            int longestDockLine = 0;
            double truckValue = 0;
            int crateCounter = 0;

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
                    bool trucksUnloading = false;

                    int arrivalTime = 0;

                    if (increment < 12 || increment > 36)
                    {
                        arrivalTime = rand.Next(1, 60);
                    }
                    else
                    {
                        arrivalTime = rand.Next(100);
                    }

                    if (arrivalTime > 50)
                    {
                        Console.WriteLine("Arrival Time " + increment);
                        Truck truck = new Truck();

                        int crateCount = rand.Next(10, 21);
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
                            dock.TotalCrates++;
                            trucksUnloading = true;

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
                                Console.WriteLine($"{dock.Id}'s truck has more crates");
                            }
                            else if (dockedTruck.Trailer.Count == 0)
                            {
                                dock.SendOff();
                                if (dock.Line.Count != 0)
                                {
                                    dock.TotalTrucks++;
                                    Console.WriteLine($"{dock.Id}'s truck is empty and another truck is in line");
                                }
                                else
                                {
                                    dock.TotalTrucks++;
                                    Console.WriteLine($"{dock.Id}'s truck is empty and no other trucks in line");
                                }
                            }
                            Console.WriteLine();
                        }
                        else
                        {
                            dock.TimeNotInUse++;
                        }
                    }
                    UserInterface(Docks, trucksUnloading);
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
                if (writer != null)
                {
                    writer.Close();
                }
            }

            double totalRevenue = 0;
            double totalCost = 0;

            foreach (Dock dock in Docks)
            {
                double dockCost = dock.TimeInUse * 100;

                Console.WriteLine($"Dock ID: {dock.Id}");
                Console.WriteLine($"Time in use: {dock.TimeInUse}");
                Console.WriteLine($"Time not in use: {dock.TimeNotInUse}");
                Console.WriteLine($"Trucks processed: {dock.TotalTrucks}");
                Console.WriteLine($"Total dock cost: ${dockCost}");
                Console.WriteLine($"Total crate profits: ${String.Format("{0:0.00}", dock.TotalSales)}");
                Console.WriteLine();

                totalRevenue += dock.TotalSales;
                totalCost += dockCost;
            }

            int truckCounter = 0;
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
            Console.WriteLine($"Average crate value: ${String.Format("{0:0.00}", crateProfit / crateCounter)}");
            Console.WriteLine($"Average truck value: ${String.Format("{0:0.00}", truckValue / truckCounter)}");
            Console.WriteLine($"Average time in use: {String.Format("{0:0.00}", averageTimeInUse / maxTime)}");
            Console.WriteLine($"Total profit: ${String.Format("{0:0.00}", totalProfit)}");
        }

        /// <summary>
        /// ShortestDock method that checks for what the shortest dock is
        /// to maximize the amount of crates that can be processed
        /// </summary>
        /// <param name="docks">List of docks being used</param>
        /// <returns>The index value of the shortest dock</returns>
        public int ShortestDock(List<Dock> docks)
        {
            int shortestLine = docks[0].Line.Count;
            int dockIndex = 0;
            for (int i = 1; i < docks.Count; i++)
            {
                if (docks[i].Line.Count < shortestLine)
                {
                    shortestLine = docks[i].Line.Count;
                    dockIndex = i;
                }
            }
            return dockIndex;
        }

        /// <summary>
        /// LongestDock method that checks for what the longest dock line is
        /// during the simulation
        /// </summary>
        /// <param name="docks">List of docks being used</param>
        /// <returns>Index value of the longest dock line</returns>
        public int LongestDock(List<Dock> docks)
        {
            int longestLine = docks[0].Line.Count;
            int dockIndex = 0;
            for (int i = 1; i < docks.Count; i++)
            {
                if (docks[i].Line.Count > longestLine)
                {
                    longestLine = docks[i].Line.Count;
                    dockIndex = i;
                }
            }
            return dockIndex;
        }
        
        /// <summary>
        /// Console based user interface that displays each dock and the current
        /// number of trucks, the crates being processed, and the total number of crates
        /// </summary>
        /// <param name="docks">List of docks</param>
        /// <param name="trucksUnloading">If the trucks are being unloaded or if </param>
        public void UserInterface(List<Dock> docks, bool trucksUnloading)
        {
            foreach (var dock in Docks)
            {
                if (trucksUnloading)
                {
                    Console.Write($"|-------|      Crates: \n");
                    Console.Write($"|-------|              {dock.TotalCrates}\n");
                    Console.Write($"|   {dock.Line.Count}   |\n");
                    Console.Write($"|-------|\n");
                    Console.Write($"|-------|\n");
                    Console.Write($"|-------|\n");
                    Console.WriteLine();
                }
            }
        }
    }
}
