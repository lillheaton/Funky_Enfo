using Lillheaton.Monogame.Dijkstra;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FunkyEnfo.Models
{
    public class Waypoint : IWaypoint
    {
        public Vector2 Position { get; set; }
        public List<IWaypoint> RelatedPoints { get; set; }

        public Waypoint()
        {
            this.RelatedPoints = new List<IWaypoint>();
        }
    }
}
