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
            bool trucksUnloading = false;

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
                    Queue<int> trucksProcessing = new Queue<int>();
                    int dockIndex = -1;
                    int truckLeavingIndex = -1;
                    int arrivalTime = 0;

                    if (increment < 12 || increment > 36) //Simulates morning and evening hours with a reduced chance for a truck to appear
                    {
                        arrivalTime = rand.Next(1, 60);
                    }
                    else
                    {
                        arrivalTime = rand.Next(100);
                    }

                    if (arrivalTime > 50)
                    {
                        Truck truck = new Truck();

                        int crateCount = rand.Next(10, 21);
                        while (crateCount > 0)
                        {
                            Crate crate = new Crate();
                            truck.Load(crate);
                            crateCount--;
                        }
                        Entrance.Enqueue(truck);
                        Console.WriteLine("A truck has entered the entrance at increment " + increment);
                    }

                    if (Entrance.Count > 0)
                    {
                        dockIndex = ShortestDock(Docks);
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

                    for (int i = 0; i < Docks.Count; i++)
                    {
                        if (Docks[i].Line.Count > 0) //Checks if a dock has a truck currently in it
                        {
                            Truck dockedTruck = Docks[i].Line.Peek();
                            Crate unloadedCrate = dockedTruck.Unload();
                            truckValue += unloadedCrate.Price;
                            Docks[i].TotalCrates++;
                            trucksUnloading = true;

                            writer.Write($"{Docks[i].Id},");
                            writer.Write($"{increment},");
                            writer.Write($"{dockedTruck.Driver},");
                            writer.Write($"{dockedTruck.DeliveryCompany},");
                            writer.Write($"{unloadedCrate.Id},");
                            writer.Write($"{String.Format("{0:0.00}", unloadedCrate.Price)}");
                            writer.WriteLine();

                            Docks[i].TotalSales += unloadedCrate.Price;
                            Docks[i].TimeInUse++;

                            if (dockedTruck.Trailer.Count != 0)
                            {
                                Console.WriteLine($"{Docks[i].Id}'s truck has more crates");
                            }
                            else if (dockedTruck.Trailer.Count == 0)
                            {
                                Docks[i].SendOff();
                                if (Docks[i].Line.Count != 0)
                                {
                                    Docks[i].TotalTrucks++;
                                    truckLeavingIndex = i;
                                    Console.WriteLine($"{Docks[i].Id}'s truck is empty and another truck is in line");
                                }
                                else
                                {
                                    Docks[i].TotalTrucks++;
                                    truckLeavingIndex = i;
                                    Console.WriteLine($"{Docks[i].Id}'s truck is empty and no other trucks in line");
                                }
                            }
                            Console.WriteLine();
                        }
                        else
                        {
                            Docks[i].TimeNotInUse++;
                        }
                    }
                    //UserInterface(Docks, trucksUnloading);
                    GUI(increment, dockIndex, numOfDocks, trucksUnloading, truckLeavingIndex, Docks);
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

            Console.WriteLine("////////////////////////////////////");
            Console.WriteLine("          SIMULATION REPORT         ");
            Console.WriteLine("////////////////////////////////////");

            double totalRevenue = 0;
            double totalCost = 0;

            foreach (Dock dock in Docks)
            {
                double dockCost = maxTime * 100;

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
        /// Simple GUI that displays green when a truck arrives at a dock and red once a truck leaves
        /// </summary>
        /// <param name="increment">Time increment of sim</param>
        /// <param name="dockIndex">Index of dock that a truck arrived at</param>
        /// <param name="totalDocks">Total number of docks in sim</param>
        /// <param name="trucksUnloading">Boolean value that checks if a truck is currently unloading at a dock</param>
        /// <param name="truckLeavingIndex">Index of dock that a truck is leaving from</param>
        /// <param name="Docks">List of Docks used in sim</param>
        public void GUI(int increment, int dockIndex, int totalDocks, bool trucksUnloading, int truckLeavingIndex, List<Dock> Docks)
        {
            string[] dockDesign = new string[totalDocks];
            if(dockIndex != -1 || trucksUnloading)
            {
                Console.WriteLine($"Time Increment: {increment}");
                for (int i = 0; i < dockDesign.Length; i++)
                {
                    if (dockIndex == i) Console.ForegroundColor = ConsoleColor.Green;
                    if (truckLeavingIndex == i) Console.ForegroundColor = ConsoleColor.Red;

                    Console.Write($"|----{i}----|");
                    Console.ResetColor();
                    Console.Write($"          Total Crates: {Docks[i].TotalCrates}\n");
                }
                if (increment != 47) Console.Write("Press \"ENTER\" to continue...\n");
                else Console.Write("Press \"ENTER\" to end the simulation...\n");
                Console.ReadLine();
                Console.Clear();
            }
        }
    }
}
