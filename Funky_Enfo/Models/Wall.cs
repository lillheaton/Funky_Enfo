using Lillheaton.Monogame.Steering;
using Microsoft.Xna.Framework;

namespace FunkyEnfo.Models
{
    public class Wall : IRectangleObstacle
    {
        public Vector3 Center { get; set; }
        public Rectangle Rectangle{get; set; }
    }
}