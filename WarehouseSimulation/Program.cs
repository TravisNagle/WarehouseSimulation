///////////////////////////////////////////////////////////////////////////////
//
// Author: Travis Nagle, Naglet@etsu.edu
// Course: CSCI-2210-001 - Data Structures
// Assignment: Project 4 - Warehouse Simulation
// Description: Driver that runs the sim in the console
//
///////////////////////////////////////////////////////////////////////////////
namespace WarehouseSimulation
{
    internal class Program
    {
        /// <summary>
        /// Main method that creates a new warehouse and runs the sim
        /// </summary>
        /// <param name="args">Command line arguments</param>
        static void Main(string[] args)
        {
            Warehouse warehouse = new Warehouse();
            warehouse.Run();
        }
    }
}