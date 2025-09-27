using System;
using UnityEngine;

[Serializable]
public struct Vec3 : IEquatable<Vec3>
{
    #region Variables

    public float x;
    public float y;
    public float z;

    public float sqrMagnitude => SqrMagnitude(this);

    public Vec3 normalized
    {
        get
        {
            float mag = magnitude;

            if (magnitude > epsilon)
                return new Vec3(x / mag, y / mag, z / mag);

            return new Vec3(0.0f, 0.0f, 0.0f);
        }
    }

    public float magnitude => Magnitude(this);

    #endregion

    #region constants

    public const float epsilon = 1e-05f;

    #endregion

    #region Default Values

    public static Vec3 Zero => new(0.0f, 0.0f, 0.0f);

    public static Vec3 One => new(1.0f, 1.0f, 1.0f);

    public static Vec3 Forward => new(0.0f, 0.0f, 1.0f);

    public static Vec3 Back => new(0.0f, 0.0f, -1.0f);

    public static Vec3 Right => new(1.0f, 0.0f, 0.0f);

    public static Vec3 Left => new(-1.0f, 0.0f, 0.0f);

    public static Vec3 Up => new(0.0f, 1.0f, 0.0f);

    public static Vec3 Down => new(0.0f, -1.0f, 0.0f);

    public static Vec3 PositiveInfinity =>
        new(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

    public static Vec3 NegativeInfinity =>
        new(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

    #endregion

    #region Constructors

    public Vec3(float x, float y)
    {
        this.x = x;
        this.y = y;
        this.z = 0.0f;
    }

    public Vec3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vec3(Vec3 v3)
    {
        this.x = v3.x;
        this.y = v3.y;
        this.z = v3.z;
    }

    public Vec3(Vector3 v3)
    {
        this.x = v3.x;
        this.y = v3.y;
        this.z = v3.z;
    }

    public Vec3(Vector2 v2)
    {
        this.x = v2.x;
        this.y = v2.y;
        this.z = 0.0f;
    }

    #endregion

    #region Operators

    public static bool operator ==(Vec3 left, Vec3 right)
    {
        float diff_x = left.x - right.x;
        float diff_y = left.y - right.y;
        float diff_z = left.z - right.z;

        float sqrmag = diff_x * diff_x + diff_y * diff_y + diff_z * diff_z;

        return sqrmag < epsilon * epsilon;
    }

    public static bool operator !=(Vec3 left, Vec3 right)
    {
        return !(left == right);
    }

    public static Vec3 operator +(Vec3 leftV3, Vec3 rightV3)
    {
        return new Vec3(leftV3.x + rightV3.x, leftV3.y + rightV3.y, leftV3.z + rightV3.z);
    }

    public static Vec3 operator -(Vec3 leftV3, Vec3 rightV3)
    {
        return new Vec3(leftV3.x - rightV3.x, leftV3.y - rightV3.y, leftV3.z - rightV3.z);
    }

    public static Vec3 operator -(Vec3 v3)
    {
        return new Vec3(-v3.x, -v3.y, -v3.z);
    }

    public static Vec3 operator *(Vec3 v3, float scalar)
    {
        return new Vec3(v3.x * scalar, v3.y * scalar, v3.z * scalar);
    }

    public static Vec3 operator *(float scalar, Vec3 v3)
    {
        return v3 * scalar;
    }

    public static Vec3 operator /(Vec3 v3, float scalar)
    {
        return new Vec3(v3.x / scalar, v3.y / scalar, v3.z / scalar);
    }

    public static implicit operator Vector3(Vec3 v3)
    {
        return new Vector3(v3.x, v3.y, v3.z);
    }

    public static implicit operator Vector2(Vec3 v2)
    {
        return new Vector2(v2.x, v2.y);
    }

    #endregion

    #region Functions

    public override string ToString()
    {
        return $"X = {x}.  Y = {y}.  Z = {z}.";
    }

    public static float Angle(Vec3 from, Vec3 to)
    {
        float dot = Dot(from, to);

        float magFrom = Magnitude(from);
        float magTo = Magnitude(to);

        if (magFrom == 0 || magTo == 0)
        {
            Debug.LogError(
                $"The Magnitude of {(magFrom == 0 ? magFrom.ToString() : magTo.ToString())} cannot be zero.");

            return 0.0f;
        }

        float cosTheta = dot / (magFrom * magTo);

        cosTheta = Math.Clamp(cosTheta, -1f, 1f);

        return MathF.Acos(cosTheta);
    }

    public static Vec3 ClampMagnitude(Vec3 vector, float maxLength)
    {
        float sqrMag = SqrMagnitude(vector);

        if (sqrMag > maxLength * maxLength)
        {
            float mag = MathF.Sqrt(sqrMag);
            float scale = maxLength / mag;

            return new Vec3(vector.x * scale, vector.y * scale, vector.z * scale);
        }

        return vector;
    }

    public static float Magnitude(Vec3 vector)
    {
        return MathF.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
    }

    public static Vec3 Cross(Vec3 a, Vec3 b)
    {
        return new Vec3
        (
            a.y * b.z - a.z * b.y,
            a.z * b.x - a.x * b.z,
            a.x * b.y - a.y * b.x
        );
    }

    public static float Distance(Vec3 a, Vec3 b)
    {
        return MathF.Sqrt(MathF.Pow(b.x - a.x, 2) + MathF.Pow(b.y - a.y, 2) + MathF.Pow(b.z - a.z, 2));
    }

    public static float Dot(Vec3 a, Vec3 b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }

    public static Vec3 Lerp(Vec3 a, Vec3 b, float t)
    {
        t = Math.Clamp(t, 0f, 1f);

        return LerpUnclamped(a, b, t);
    }

    public static Vec3 LerpUnclamped(Vec3 a, Vec3 b, float t)
    {
        return new Vec3(a + (b - a) * t);
    }

    public static Vec3 Max(Vec3 a, Vec3 b)
    {
        float x = a.x > b.x ? a.x : b.x;
        float y = a.y > b.y ? a.y : b.y;
        float z = a.z > b.z ? a.z : b.z;

        return new Vec3(x, y, z);
    }

    public static Vec3 Min(Vec3 a, Vec3 b)
    {
        float x = a.x < b.x ? a.x : b.x;
        float y = a.y < b.y ? a.y : b.y;
        float z = a.z < b.z ? a.z : b.z;

        return new Vec3(x, y, z);
    }

    public static float SqrMagnitude(Vec3 vector)
    {
        return vector.x * vector.x + vector.y * vector.y + vector.z * vector.z;
    }

    public static Vec3 Project(Vec3 vector, Vec3 onNormal)
    {
        float dot = Dot(vector, onNormal);
        float sqrMag = SqrMagnitude(onNormal);

        if (sqrMag < epsilon)
            return new Vec3(0.0f, 0.0f, 0.0f);

        float scale = dot / sqrMag;
        return new Vec3(onNormal * scale);
    }

    public static Vec3 Reflect(Vec3 inDirection, Vec3 inNormal)
    {
        float dot = Dot(inDirection, inNormal);

        return new Vec3
        (
            inDirection.x - 2f * dot * inNormal.x,
            inDirection.y - 2f * dot * inNormal.y,
            inDirection.z - 2f * dot * inNormal.z
        );
    }

    public void Set(float newX, float newY, float newZ)
    {
        x = newX;
        y = newY;
        z = newZ;
    }

    public void Scale(Vec3 scale)
    {
        x *= scale.x;
        y *= scale.y;
        z *= scale.z;
    }

    public void Normalize()
    {
        float mag = Magnitude(this);

        if (mag > epsilon)
            this /= mag;
        else
            this = new Vec3(0.0f, 0.0f, 0.0f);
    }

    #endregion

    #region Internals

    public override bool Equals(object other)
    {
        if (!(other is Vec3)) return false;
        return Equals((Vec3)other);
    }

    public bool Equals(Vec3 other)
    {
        return Mathf.Approximately(x, other.x) && Mathf.Approximately(y, other.y) &&
               Mathf.Approximately(z, other.z);
    }

    public override int GetHashCode()
    {
        return x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2);
    }

    #endregion
}