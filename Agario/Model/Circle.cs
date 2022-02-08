using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Numerics;
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
///    Cicle class is an object for the agario game that keeps track of all the different circles.
///    Circles can be a food, player, heartbeat, or admit type. 
/// </summary>
namespace Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Circle
    {
        [JsonProperty(PropertyName = "loc")]
        public Vector2 position { get; }
        [JsonProperty(PropertyName = "argb_color")]
        public int argbColor { get; }
        [JsonProperty(PropertyName = "id")]
        public int idNumber { get; }
        [JsonProperty(PropertyName = "belongs_to")]
        public int PlayerId { get; }
        [JsonProperty(PropertyName = "type")]
        public int Type { get; }
        [JsonProperty(PropertyName = "Name")]
        public string PlayerName { get; }
        [JsonProperty(PropertyName = "Mass")]
        public long Mass { get; set; }
        public double radius { get; }

        /// <summary>
        /// Constuctor that builds the circle object
        /// </summary>
        /// <param name="position">location of the circle</param>
        /// <param name="circleColor">color of the circle</param>
        /// <param name="idNumber">unqiue id number of the circle</param>
        /// <param name="Type">type of the circle</param>
        /// <param name="PlayerName">name of the player who owns the circle</param>
        /// <param name="Mass">mass of the circle</param>
        public Circle(Vector2 position, int circleColor, int idNumber, int Type, string PlayerName, long Mass)
        {
            this.position = position;
            this.argbColor = circleColor;
            this.idNumber = idNumber;
            this.Type = Type;
            this.PlayerName = PlayerName;
            this.Mass = Mass;
            //keeps track of radius for drawing purposes.
            this.radius = MassToRadius(Mass);
        }
        /// <summary>
        /// Used to calculate the radius of the circle from the mass
        /// </summary>
        /// <param name="mass"></param>
        /// <returns></returns>
        private double MassToRadius(long mass)
        {
            return Math.Sqrt(mass/ 3.14);
        }
    }
}
