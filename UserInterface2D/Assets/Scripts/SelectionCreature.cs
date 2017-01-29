using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionCreature : MonoBehaviour {
    public GameObject border;
    TransformationControl transformControl;
    public Vector3 selectPosition;
    public bool IsSelected = false;
    public void SetTransformation(Transformation transformation)
    {
        transformControl = new TransformationControl(gameObject, transformation);
    }
    void OnMouseEnter()
    {
        if (!IsSelected)
        {
            transformControl.Transform();
            border.SetActive(true);
            border.transform.position = transform.position;
        }
    }
    void OnMouseExit()
    {
        if (!IsSelected)
        {
            transformControl.TransformToOriginalPosition();
            border.SetActive(false);
        }
    }
}
