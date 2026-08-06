using System;
using System.Collections.Generic;
using UnityEngine;

public enum Space
{
    World,
    Self
}

[Serializable]
public class Trans
{
    public Vec3 localPosition;
    public Quat localRotation;

    public Vec3 localScale;

    public M4X4 localToWorldMatrix => LocalToWorldMatrix();
    public Vec3 forward => new(0, 0, 1);
    public Vec3 right => new(1, 0, 0);
    public Vec3 up => new(0, 1, 0);

    public const float epsilon = 1e-05f;

    private Trans parent;
    private List<Trans> children = new();

    public void AddChild(Trans child)
    {
        if (!children.Contains(child))
        {
            children.Add(child);
            child.parent = this;
        }
    }

    public void RemoveChild(Trans child)
    {
        if (children.Contains(child))
        {
            children.Remove(child);
            child.parent = null;
        }
    }

    public Trans()
    {
        localPosition = new Vec3(0, 0, 0);
        localRotation = Quat.identity;
        localScale = new Vec3(1, 1, 1);
    }

    public Trans(Vec3 localPosition, Quat rotation, Vec3 scale)
    {
        this.localPosition = localPosition;
        localRotation = rotation;
        localScale = scale;
    }

    // Defaults to Space.Self, matching Unity's Transform.Translate.
    public void Translate(Vec3 translation)
    {
        Translate(translation, Space.Self);
    }

    public void Translate(float x, float y, float z)
    {
        Translate(new Vec3(x, y, z), Space.Self);
    }

    public void Translate(float x, float y, float z, Space relativeTo)
    {
        Translate(new Vec3(x, y, z), relativeTo);
    }

    public void Translate(Vec3 translation, Space relativeTo)
    {
        // Self: move along the object's own (rotated) axes.
        // World: move along world axes.
        if (relativeTo == Space.Self)
            localPosition += localRotation * translation;
        else
            localPosition += translation;
    }

    public void Rotate(Vec3 eulerAngles)
    {
        Rotate(eulerAngles.x, eulerAngles.y, eulerAngles.z);
    }

    public void Rotate(Vec3 eulerAngles, Space relativeTo)
    {
        Rotate(eulerAngles.x, eulerAngles.y, eulerAngles.z, relativeTo);
    }

    public void Rotate(float x, float y, float z)
    {
        Quat delta = Quat.Euler(new Vec3(x, y, z));
        localRotation *= delta;
    }

    public void Rotate(float x, float y, float z, Space relativeTo)
    {
        Quat delta = Quat.Euler(new Vec3(x, y, z));
        if (relativeTo == Space.Self)
            localRotation *= delta;             // post-multiply: local axes
        else
            localRotation = delta * localRotation; // pre-multiply: world axes
    }

    public M4X4 LocalToWorldMatrix()
    {
        M4X4 local = M4X4.Translate(localPosition) * M4X4.Rotate(localRotation) * M4X4.Scale(localScale);

        if (parent != null)
            return parent.LocalToWorldMatrix() * local;
        return local;
    }
}