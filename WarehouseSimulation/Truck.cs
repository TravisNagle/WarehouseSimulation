///////////////////////////////////////////////////////////////////////////////
//
// Author: Travis Nagle, Naglet@etsu.edu
// Course: CSCI-2210-001 - Data Structures
// Assignment: Project 4 - Warehouse Simulation
// Description: Implementation of the Truck class
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
    /// Implementation of a truck object
    /// </summary>
    internal class Truck
    {
        public string _driver;

        /// <summary>
        /// Driver property that randomly selects a driver name
        /// </summary>
        public string Driver 
        { 
            get
            {
                Random rand = new Random();
                string _driver = "";
                int randomNum = rand.Next(0, 11);

                switch (randomNum)
                {
                    case 0:
                        _driver = "Blade";
                        break;
                    case 1:
                        _driver = "Laura";
                        break;
                    case 2:
                        _driver = "Patrick";
                        break;
                    case 3:
                        _driver = "Flynn";
                        break;
                    case 4:
                        _driver = "Tyler";
                        break;
                    case 5:
                        _driver = "Colby";
                        break;
                    case 6:
                        _driver = "Chris";
                        break;
                    case 7:
                        _driver = "Kyle";
                        break;
                    case 8:
                        _driver = "Seth";
                        break;
                    case 9:
                        _driver = "Danny";
                        break;
                    case 10:
                        _driver = "Alivia";
                        break;
                }
                return _driver;
            }
        }

        public string _deliveryCompany;

        /// <summary>
        /// Delivery company property that randomly sets 
        /// a delivery company name
        /// </summary>
        public string DeliveryCompany
        {
            get
            {
                Random rand = new Random();
                string _deliverCompany = "";
                int randomNum = rand.Next(0, 11);

                switch (randomNum)
                {
                    case 0:
                        _deliveryCompany = "Crates R Us";
                        break;
                    case 1:
                        _deliveryCompany = "Microsoft Truck Company";
                        break;
                    case 2:
                        _deliveryCompany = "Spicy Integer Inc.";
                        break;
                    case 3:
                        _deliveryCompany = "UAC";
                        break;
                    case 4:
                        _deliveryCompany = "UNSC";
                        break;
                    case 5:
                        _deliveryCompany = "StackStyle LLC";
                        break;
                    case 6:
                        _deliveryCompany = "T.G.B.A.E. International";
                        break;
                    case 7:
                        _deliveryCompany = "TeamSport";
                        break;
                    case 8:
                        _deliveryCompany = "Pointer Hades Co.";
                        break;
                    case 9:
                        _deliveryCompany = "ItDepends.com";
                        break;
                    case 10:
                        _deliveryCompany = "The Abstraction Company";
                        break;
                }
                return _deliveryCompany;
            }
        }
        public Stack<Crate> Trailer = new Stack<Crate>();

        /// <summary>
        /// Load method that adds crates to the truck's stack
        /// </summary>
        /// <param name="crate">Crate to be added</param>
        public void Load(Crate crate)
        {
            Trailer.Push(crate);
        }

        /// <summary>
        /// Unload method that pops off the latest crate added
        /// </summary>
        /// <returns>Unloaded crate</returns>
        public Crate Unload()
        {
            return Trailer.Pop();
        }
    }
}
