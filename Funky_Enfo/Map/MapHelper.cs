using FunkyEnfo.Models;
using Lillheaton.Monogame.Dijkstra;
using Lillheaton.Monogame.Steering;
using Microsoft.Xna.Framework;
using System.Linq;

namespace FunkyEnfo.Map
{
    public class MapHelper
    {
        public static bool ClearViewFrom(Vector2 pointA, Vector2 pointB, IRectangleObstacle[] obstacles)
        {
            bool clearView = true;
            foreach (var obstacle in obstacles)
            {
                if (LineClipping.LineIntersectsRect(pointA, pointB, obstacle.Rectangle))
                {
                    clearView = false;
                }
            }

            return clearView;
        }

        public static Vector2[] CalculatePath(TileEngine engine, Vector2 start, Vector2 end)
        {
            Tile startTile = engine.TileAt(start);
            Tile endTile = engine.TileAt(end);

            // Create start position and goal as waypoints
            var startWaypoint = new Waypoint { Position = startTile.Center };
            var endWaypoint = new Waypoint { Position = endTile.Center };

            // Calculate there related waypoints
            engine.CalculateRelatedWaypoints(startWaypoint);
            engine.CalculateRelatedWaypoints(endWaypoint);

            // Copy list waypointList
            var waypointsCopy = engine.Waypoints.Concat(new[] { startWaypoint, endWaypoint });

            // Those who is related to endWaypoint should also be related to end
            foreach (var relatedWaypoint in endWaypoint.RelatedPoints)
            {
                relatedWaypoint.RelatedPoints.Add(endWaypoint);
            }

            // Calculate path and return solution
            var solution = Dijkstra.CalculatePath(waypointsCopy.OfType<IWaypoint>().ToList(), startWaypoint, endWaypoint).Reverse().ToArray();

            // Need to do some cleanup
            foreach (var relatedWaypoint in endWaypoint.RelatedPoints)
            {
                relatedWaypoint.RelatedPoints.Remove(endWaypoint);
            }

            // Return the solution
            return solution;
        }

        public static string TranslateMapChar(char ch, out bool obstacle)
        {
            obstacle = false;

            switch (ch)
            {
                case 'A':
                    obstacle = true;
                    return "Cliff";
                case 'B':
                    obstacle = true;
                    return "CliffCornerNE";
                case 'C':
                    obstacle = true;
                    return "CliffCornerNW";
                case 'D':
                    obstacle = true;
                    return "CliffCornerSE1";
                case 'E':
                    obstacle = true;
                    return "CliffCornerSE2";
                case 'F':
                    obstacle = true;
                    return "CliffCornerSW1";
                case 'G':
                    obstacle = true;
                    return "CliffCornerSW2";
                case 'H':
                    obstacle = true;
                    return "CliffE";
                case 'I':
                    obstacle = true;
                    return "CliffS";
                case 'J':
                    return "DarkMarble";
                case 'K':
                    return "Dirt";
                case 'L':
                    return "Grass";
                case 'M':
                    return "LightMarble";
                default:
                    obstacle = true;
                    return "Cliff";
            }
        }
    }
}
