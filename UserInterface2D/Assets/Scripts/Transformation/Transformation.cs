using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


    public class Transformation
    {
        public Vector3? Position { get; set; }
        public Quaternion? Rotation { get; set; }
        public Vector3? Scale { get; set; }
        public Transformation(Vector3? position)
        {
            Initialization(position, null, null);
        }
        public Transformation(Quaternion? rotation)
        {
            Initialization(null, rotation, null);
        }
        public Transformation(Vector3? position, Quaternion? rotation)
        {
            Initialization(position, rotation, null);
        }
        public Transformation(Vector3? position, Vector3? scale)
        {
            Initialization(position, null, scale);
        }
        public Transformation(Quaternion? rotation, Vector3? scale)
        {
            Initialization(null, rotation, scale);
        }
        public Transformation(Vector3? position, Quaternion? rotation, Vector3? scale)
        {
            Initialization(position, rotation, scale);
        }
        void Initialization(Vector3? position, Quaternion? rotation, Vector3? scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }
    }

