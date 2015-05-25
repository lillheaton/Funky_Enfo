using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FunkyEnfo.Extensions
{
    public static class EnumerableExtensions
    {
        private static T GetNeigbour<T>(T[][] twoDimensionalArray, Vector2 position) where T : class
        {
            var xLength = twoDimensionalArray.Length;
            var yLength = twoDimensionalArray[0].Length;

            if (position.X > -1 && position.X < xLength &&
                position.Y > -1 && position.Y < yLength)
            {
                return twoDimensionalArray[(int)position.X][(int)position.Y];
            }

            return null;
        }

        public static IEnumerable<T> GetEnumerableNeighbours<T>(this T[][] twoDimensionalArray, Vector2 position) where T : class
        {
            yield return GetNeigbour(twoDimensionalArray, position + VecDirection.East);
            yield return GetNeigbour(twoDimensionalArray, position + VecDirection.West);
            yield return GetNeigbour(twoDimensionalArray, position + VecDirection.North);
            yield return GetNeigbour(twoDimensionalArray, position + VecDirection.South);

            yield return GetNeigbour(twoDimensionalArray, position + VecDirection.North + VecDirection.West);
            yield return GetNeigbour(twoDimensionalArray, position + VecDirection.North + VecDirection.East);
            yield return GetNeigbour(twoDimensionalArray, position + VecDirection.South + VecDirection.West);
            yield return GetNeigbour(twoDimensionalArray, position + VecDirection.South + VecDirection.East);
        }

        public static IEnumerable<T> GetNorthWestNeighbours<T>(this T[][] twoDimensionalArray, Vector2 position) where T : class
        {
            yield return GetNeigbour(twoDimensionalArray, position + VecDirection.North);
            yield return GetNeigbour(twoDimensionalArray, position + VecDirection.West);
            yield return GetNeigbour(twoDimensionalArray, position + VecDirection.North + VecDirection.West);
        }

        public static IEnumerable<T> GetNorthEastNeighbours<T>(this T[][] twoDimensionalArray, Vector2 position) where T : class
        {
            yield return GetNeigbour(twoDimensionalArray, position + VecDirection.North);
            yield return GetNeigbour(twoDimensionalArray, position + VecDirection.East);
            yield return GetNeigbour(twoDimensionalArray, position + VecDirection.North + VecDirection.East);
        }

        public static IEnumerable<T> GetSouthWestNeighbours<T>(this T[][] twoDimensionalArray, Vector2 position) where T : class
        {
            yield return GetNeigbour(twoDimensionalArray, position + VecDirection.South);
            yield return GetNeigbour(twoDimensionalArray, position + VecDirection.West);
            yield return GetNeigbour(twoDimensionalArray, position + VecDirection.South + VecDirection.West);
        }

        public static IEnumerable<T> GetSouthEastNeighbours<T>(this T[][] twoDimensionalArray, Vector2 position) where T : class
        {
            yield return GetNeigbour(twoDimensionalArray, position + VecDirection.South);
            yield return GetNeigbour(twoDimensionalArray, position + VecDirection.East);
            yield return GetNeigbour(twoDimensionalArray, position + VecDirection.South + VecDirection.East);
        }

        public static T GetValue<T>(this T[][] twoDimensionalArray, Point arrayPosition) where T : class
        {
            return twoDimensionalArray[arrayPosition.X][arrayPosition.Y];
        }
    }
}