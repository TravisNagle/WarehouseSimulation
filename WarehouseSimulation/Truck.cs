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
    internal class Truck
    {
        public string _driver;
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
                        _driver = "Dave";
                        break;
                    case 1:
                        _driver = "Bob";
                        break;
                    case 2:
                        _driver = "Jeff";
                        break;
                    case 3:
                        _driver = "Ryan";
                        break;
                    case 4:
                        _driver = "George";
                        break;
                    case 5:
                        _driver = "Fred";
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

        public void Load(Crate crate)
        {
            Trailer.Push(crate);
        }

        public Crate Unload()
        {
            return Trailer.Pop();
        }
    }
}
