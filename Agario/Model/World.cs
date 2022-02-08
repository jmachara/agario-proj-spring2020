using Microsoft.Extensions.Logging;
using NetworkingNS;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
/// <summary> 
/// Author:    Jack Machara
/// Partner:   None
/// Date:      04/8/20
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and Jack Machara - This work may not be copied for use in Academic Coursework. 
/// 
/// I, Jack Machara, certify that I wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in my README file. 
/// 
/// File Contents 
/// 
///    World class keeps track of the size of the world along with all of the players. It also stores the players circle and the number of players 
///    playing.
/// </summary>
namespace Model
{
    public class World
    {
        private int WorldHeight {get;}
        private int WorldWidth { get; }
        public Dictionary<int, Circle> circleList;
        public int NumberOfPlayerCircles { get; set; }
        public int NumberOfFoodCircles { get; set; }

        /// <summary>
        /// Constuctor for the world class
        /// </summary>
        /// <param name="logger">logger to log debug information</param>
        /// <param name="playerCircle">Current players circle</param>
        /// <param name="height">height of the world</param>
        /// <param name="width">width of the world</param>
        public World(int height, int width)
        {
            this.circleList = new Dictionary<int, Circle>();
            this.WorldWidth = width;
            this.WorldHeight = height;
        }
    }
}
