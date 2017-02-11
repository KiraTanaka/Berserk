using UnityEngine;

namespace Assets.Scripts.UI.Transformations
{
    public class Transformation
    {
        public Vector3? Position { get; set; }

        public Quaternion? Rotation { get; set; }

        public Vector3? Scale { get; set; }


        public Transformation(Vector3? position)
            : this(position, null, null)
        {
        }

        public Transformation(Quaternion? rotation)
            : this(null, rotation, null)
        {
        }

        public Transformation(Vector3? position, Quaternion? rotation) 
            : this(position, rotation, null)
        {
        }

        public Transformation(Vector3? position, Vector3? scale) 
            : this(position, null, scale)
        {
        }

        public Transformation(Quaternion? rotation, Vector3? scale) 
            : this(null, rotation, scale)
        {
        }

        public Transformation(Vector3? position, Quaternion? rotation, Vector3? scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }
    }
}

