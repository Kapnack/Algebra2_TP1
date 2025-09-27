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

    public Trans parent;
    public List<Trans> children = new();

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

    public void Translate(Vec3 translation)
    {
        Translate(translation.x, translation.y, translation.z);
    }

    public void Translate(Vec3 translation, Space relativeTo)
    {
        if (relativeTo == Space.Self)
            localPosition += translation;
        else
            localPosition += localRotation * translation;
    }

    public void Translate(float x, float y, float z)
    {
        localPosition.x += x;
        localPosition.y += x;
        localPosition.z += z;
    }

    public void Translate(float x, float y, float z, Space relativeTo)
    {
        Translate(new Vec3(x, y, z), relativeTo);
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
            localRotation *= delta;
        else
            localRotation = delta * localRotation;
    }

    public M4X4 LocalToWorldMatrix()
    {
        M4X4 local = M4X4.Translate(localPosition) * M4X4.Rotate(localRotation) * M4X4.Scale(localScale);
        
        if (parent != null)
            return parent.LocalToWorldMatrix() * local;
        return local;
    }
}