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

    public Vec3 eulerAngles => ToEulerAngles(this);

    public Quat(Vec3 v, float w)
    {
        x = v.x;
        y = v.y;
        z = v.z;
        this.w = w;
    }

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

        return new Quat
        (
            axis.normalized * Mathf.Sin(halfAngle),
            Mathf.Cos(halfAngle)
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
        Quat qx = AngleAxis(x, Vec3.Right); // Pitch
        Quat qy = AngleAxis(y, Vec3.Up); // Yaw
        Quat qz = AngleAxis(z, Vec3.Forward); // Roll

        Quat q = qz * qx * qy;

        return q.normalized;
    }

    public static Vec3 ToEulerAngles(Quat q)
    {
        q = Normalize(q);

        float x = q.x;
        float y = q.y;
        float z = q.z;
        float w = q.w;

        float sinPitch = 2f * (y * z + w * x);
        sinPitch = Mathf.Clamp(sinPitch, -1f, 1f);
        float pitch = Mathf.Asin(sinPitch);

        float yaw = 0.0f;
        float roll = 0.0f;

        if (Mathf.Abs(sinPitch) < 0.99999f)
        {
            yaw = Mathf.Atan2(2f * (w * y - x * z), 1f - 2f * (x * x + y * y));
            roll = Mathf.Atan2(2f * (w * z - x * y), 1f - 2f * (x * x + z * z));
        }
        else
        {
            // Gimbal lock (pitch ~ +/-90): pin yaw and solve roll.
            yaw = 0f;
            roll = Mathf.Atan2(2f * (x * y + w * z), 1f - 2f * (y * y + z * z));
        }

        return new Vec3(pitch, yaw, roll) * Mathf.Rad2Deg;
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
            lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y,
            lhs.w * rhs.y + lhs.y * rhs.w + lhs.z * rhs.x - lhs.x * rhs.z,
            lhs.w * rhs.z + lhs.z * rhs.w + lhs.x * rhs.y - lhs.y * rhs.x,
            lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z
        );
    }

    public static Vec3 operator *(Quat rotation, Vec3 point)
    {
        Vec3 u = new(rotation.x, rotation.y, rotation.z);
        Vec3 uv = Vec3.Cross(u, point);
        Vec3 uuv = Vec3.Cross(u, uv);

        uv *= (2.0f * rotation.w);
        uuv *= 2.0f;

        return point + uv + uuv;
    }
}