///////////////////////////////////////////////////////////////////////////////
//
// Author: Travis Nagle, Naglet@etsu.edu
// Course: CSCI-2210-001 - Data Structures
// Assignment: Project 4 - Warehouse Simulation
// Description: Implementation of the Dock class that tracks the important logistics of the sim
//
///////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseSimulation
{
    /// <summary>
    /// Implementation of a dock object
    /// </summary>
    internal class Dock
    {
        public string Id { get; set; }
        public Queue<Truck> Line = new Queue<Truck>();
        public double TotalSales { get; set; }
        public int TotalCrates { get; set; }
        public int TotalTrucks { get; set; }
        public int TimeInUse { get; set; }
        public int TimeNotInUse { get; set; }

        /// <summary>
        /// JoinLine method that adds a truck to the docks queue
        /// </summary>
        /// <param name="truck">Truck to be enqueued</param>
        public void JoinLine(Truck truck)
        {
            Line.Enqueue(truck);
        }

        /// <summary>
        /// SendOff method that removes a truck from the docks queue
        /// </summary>
        /// <returns>The first truck in line</returns>
        public Truck SendOff()
        {
            return Line.Dequeue();
        }
    }
}
