using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FunkyEnfo.Models
{
    public class Spritesheet2D
    {
        public Texture2D Texture { get; set; }
        public Dictionary<string, Vector2> SpritePosition { get; set; }
        public float SpriteSize { get; set; }
        public string[] Names { get; set; }
    }
}