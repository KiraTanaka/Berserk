using Assets.Scripts.Infrastructure;
using UnityEngine;

namespace Assets.Scripts.UI.Transformations
{
    public class TransformationControl
    {
        private readonly GameObject _gameObject;

        private Transformation _transform;

        private readonly Transformation _changeTransform;


        public TransformationControl(GameObject gameObject, Transformation changeTransform)
        {
            _gameObject = gameObject;
            _changeTransform = changeTransform;
        }


        public void Transform()
        {
            if (_transform == null)
                SaveTransform();
            ChangeTransform(_changeTransform);
        }

        public void TransformToOriginalPosition()
        {
            if (_transform != null)
                ChangeTransform(_transform);
        }

        private void ChangeTransform(Transformation transform)
        {
            _gameObject.transform.localPosition =
                transform.Position ?? _gameObject.transform.localPosition;
            _gameObject.transform.localRotation =
                transform.Rotation ?? _gameObject.transform.localRotation;
            _gameObject.transform.localScale =
                transform.Scale ?? _gameObject.transform.localScale;
        }

        private void SaveTransform()
        {
            Transform objectTransform = _gameObject.transform;
            _transform = new Transformation(objectTransform.localPosition, 
                objectTransform.localRotation, objectTransform.localScale);
        }
    }
}

