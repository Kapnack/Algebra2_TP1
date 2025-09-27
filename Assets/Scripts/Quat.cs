using UnityEngine;
using System;

[Serializable]
public struct Quat
{
    [Range(-1f, 1f)]
    public float x;
    [Range(-1f, 1f)]
    public float y;
    [Range(-1f, 1f)]
    public float z;
    public float w;

    public const float epsilon = 1e-05f;
    public static Quat identity => new(0, 0, 0, 1);
    public Quat normalized => Normalize(this);

    public Quat eulerAngles => Euler(new Vec3(x, y, z));

    public Quat(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public static Quat AngleAxis(float angle, Vec3 axis)
    {
        float rad = angle * Mathf.Deg2Rad;

        float halfAngle = rad * 0.5f;
        float sinHalf = Mathf.Sin(halfAngle);
        float cosHalf = Mathf.Cos(halfAngle);

        Vec3 normAxis = axis.normalized;

        return new Quat
        (
            normAxis.x * sinHalf,
            normAxis.y * sinHalf,
            normAxis.z * sinHalf,
            cosHalf
        );
    }

    public static float Magnitude(Quat quat) => Mathf.Sqrt(Dot(quat, quat));


    public static float SqrMagnitude(Quat quat) => Dot(quat, quat);


    public static float Dot(Quat a, Quat b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
    }

    public static Quat Euler(Vec3 euler) => Euler(euler.x, euler.y, euler.z);
    

    public static Quat Euler(float x, float y, float z)
    {
        Quat qx = AngleAxis(x, new Vec3(1f, 0f, 0f)); // Pitch
        Quat qy = AngleAxis(y, new Vec3(0f, 1f, 0f)); // Yaw
        Quat qz = AngleAxis(z, new Vec3(0f, 0f, 1f)); // Roll

        Quat q = qz * qx * qy;

        return q.normalized;
    }

    public static Quat Normalize(Quat q)
    {
        float mag = SqrMagnitude(q);

        if (mag > epsilon * epsilon)
        {
            float invMag = 1f / Mathf.Sqrt(mag);
            return new Quat(q.x * invMag, q.y * invMag, q.z * invMag, q.w * invMag);
        }

        return identity;
    }

    public override string ToString()
    {
        return $"x: {x}, y: {y}, z: {z}, w: {w}";
    }

    public static Quat operator *(Quat lhs, Quat rhs)
    {
        return new Quat
        (
            lhs.x * rhs.x,
            lhs.y * rhs.y,
            lhs.z * rhs.z,
            lhs.w * rhs.w
        );
    }

    public static Vec3 operator *(Quat rotation, Vec3 point)
    {
        Vec3 u = new Vec3(rotation.x, rotation.y, rotation.z);
        Vec3 uv = Vec3.Cross(u, point);
        Vec3 uuv = Vec3.Cross(u, uv);

        uv *= (2.0f * rotation.w);
        uuv *= 2.0f;

        return point + uv + uuv;
    }
}