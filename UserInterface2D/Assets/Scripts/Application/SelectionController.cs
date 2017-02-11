using System.Diagnostics.CodeAnalysis;
using Assets.Scripts.Application.Transformations;
using UnityEngine;

namespace Assets.Scripts.Application
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class SelectionController : MonoBehaviour
    {
        public GameObject Border { get; set; }

        public Vector3 SelectedPosition { get; set; }

        public bool IsSelected { get; set; } = false;

        private TransformationControl _transformControl;


        public void SetTransformation(Transformation transformation)
        {
            _transformControl = new TransformationControl(gameObject, transformation);
        }


        void OnMouseEnter()
        {
            if (IsSelected) return;
            _transformControl.Transform();
            Border.SetActive(true);
            Border.transform.position = transform.position;
        }
        void OnMouseExit()
        {
            if (IsSelected) return;
            _transformControl.TransformToOriginalPosition();
            Border.SetActive(false);
        }
    }
}
