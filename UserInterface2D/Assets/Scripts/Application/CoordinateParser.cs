﻿using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Application
{
    public class CoordinateParser
    {
        private const string PathToActiveCardsPositions = 
            @"Assets\Scripts\Application\Coordinates\PositionOfActiveCards";

        private const string PathToCoinsPositions = 
            @"Assets\Scripts\Application\Coordinates\PositionsOfCoins";

        public List<Vector3> GetActiveCardsPositions(string namePlayer)
            => Parse(GetFullPath(PathToActiveCardsPositions, namePlayer));

        public List<Vector3> GetCoinPositions(string namePlayer)
            => Parse(GetFullPath(PathToCoinsPositions, namePlayer));

        private static List<Vector3> Parse(string path)
        {
            var vectors = new List<Vector3>();
            using (var stream = new StreamReader(path))
            {
                while (true)
                {
                    string line = stream.ReadLine();
                    if (line == null) break;

                    var coordinates = line.Split(',');
                    var vector = new Vector3(
                        float.Parse(coordinates[0]), 
                        float.Parse(coordinates[1]), 
                        float.Parse(coordinates[2]));
                    vectors.Add(vector);
                }
            }
            return vectors;
        }

        private static string GetFullPath(string path, string namePlayer)
        {
            return path + namePlayer + ".txt";
        }
    }
}

