using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class CoordinateParser
{
    public CoordinateParser() { }
    public List<Vector3> Parse(string fileName)
    {
        List<Vector3> vectors = new List<Vector3>();
        using (var stream = new StreamReader(fileName))
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
}

