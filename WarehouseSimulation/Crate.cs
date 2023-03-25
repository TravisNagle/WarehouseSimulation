///////////////////////////////////////////////////////////////////////////////
//
// Author: Travis Nagle, Naglet@etsu.edu
// Course: CSCI-2210-001 - Data Structures
// Assignment: Project 4 - Warehouse Simulation
// Description: Crate class that keeps track of the crate ID and value
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
    /// Implementation of a crate object 
    /// </summary>
    internal class Crate
    {
        public string _id = "";
        /// <summary>
        /// ID property that randomly sets the crates ID to 4 numbers between 0-9
        /// </summary>
        public string Id 
        { 
            get
            {
                Random rand = new Random();
                while(_id.Length < 4)
                {
                    _id += rand.Next(0, 10);
                }
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        public double _price;
        /// <summary>
        /// Price property that sets a random crate value between 50-500
        /// </summary>
        public double Price
        {
            get
            {
                Random rand = new Random();
                _price = rand.NextDouble() * (501 - 50) + 50;
                return _price;
            }
        }
    }
}