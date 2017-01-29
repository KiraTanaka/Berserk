using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TransformationControl
    {
    GameObject gameObject;
    Transformation transform;
    Transformation changeTransform;
    public TransformationControl(GameObject gameObject, Transformation changeTransform)
    {
        this.gameObject = gameObject;
        this.changeTransform = changeTransform;
    }
    public void Transform()
    {
        if (transform == null)
            SaveTransform();
        ChangeTransform(changeTransform);
    }
    public void TransformToOriginalPosition()
    {
        if (transform != null)
            ChangeTransform(transform);
    }
    void ChangeTransform(Transformation transform)
    {
        gameObject.transform.localPosition = transform.Position ?? gameObject.transform.localPosition;
        gameObject.transform.localRotation = transform.Rotation ?? gameObject.transform.localRotation;
        gameObject.transform.localScale = transform.Scale ?? gameObject.transform.localScale;
    }
    void SaveTransform()
    {
        Transform objectTransform = gameObject.transform;
        transform = new Transformation(objectTransform.localPosition, objectTransform.localRotation, objectTransform.localScale);
    }
}

