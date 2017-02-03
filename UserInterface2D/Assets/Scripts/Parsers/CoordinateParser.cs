using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class CoordinateParser
{
    private string _pathToActiveCardsPositions = @"Assets\Scripts\PositionOfActiveCards";
    private string _pathToCoinsPositions = @"Assets\Scripts\PositionsOfCoins";
    public List<Vector3> GetActiveCardsPositions(string namePlayer)
        =>Parse(FullPath(_pathToActiveCardsPositions, namePlayer));
    public List<Vector3> GetCoinPositions(string namePlayer)
        => Parse(FullPath(_pathToCoinsPositions, namePlayer));
    private List<Vector3> Parse(string path)
    {
        List<Vector3> vectors = new List<Vector3>();
        using (var stream = new StreamReader(path))
        {
            while (true)
            {
                string line = stream.ReadLine();
                if (line == null)
                {
                    stream.Close();
                    break;
                }
                var coordinate = line.Split(',');
                vectors.Add(new Vector3(float.Parse(coordinate[0]), float.Parse(coordinate[1]), float.Parse(coordinate[2])));
            }
        }
        return vectors;
    }
    private string FullPath(string path, string namePlayer)
    {
        return path + namePlayer + ".txt";
    }
}

