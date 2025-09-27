using System;
using UnityEngine;

[Serializable]
public struct M4X4
{
    public float m00, m01, m02, m03;
    public float m10, m11, m12, m13;
    public float m20, m21, m22, m23;
    public float m30, m31, m32, m33;

    public static M4X4 Identity => new
    (
        1, 0, 0, 0,
        0, 1, 0, 0,
        0, 0, 1, 0,
        0, 0, 0, 1
    );


    public static M4X4 zero => new
    (
        0, 0, 0, 0,
        0, 0, 0, 0,
        0, 0, 0, 0,
        0, 0, 0, 0
    );

    public M4X4(
        float m00, float m01, float m02, float m03,
        float m10, float m11, float m12, float m13,
        float m20, float m21, float m22, float m23,
        float m30, float m31, float m32, float m33)
    {
        this.m00 = m00;
        this.m01 = m01;
        this.m02 = m02;
        this.m03 = m03;
        this.m10 = m10;
        this.m11 = m11;
        this.m12 = m12;
        this.m13 = m13;
        this.m20 = m20;
        this.m21 = m21;
        this.m22 = m22;
        this.m23 = m23;
        this.m30 = m30;
        this.m31 = m31;
        this.m32 = m32;
        this.m33 = m33;
    }

    public M4X4(Quat c0, Quat c1, Quat c2, Quat c3)
    {
        m00 = c0.x;
        m10 = c0.y;
        m20 = c0.z;
        m30 = c0.w;
        m01 = c1.x;
        m11 = c1.y;
        m21 = c1.z;
        m31 = c1.w;
        m02 = c2.x;
        m12 = c2.y;
        m22 = c2.z;
        m32 = c2.w;
        m03 = c3.x;
        m13 = c3.y;
        m23 = c3.z;
        m33 = c3.w;
    }

    public M4X4(M4X4 other)
    {
        m00 = other.m00;
        m01 = other.m01;
        m02 = other.m02;
        m03 = other.m03;
        m10 = other.m10;
        m11 = other.m11;
        m12 = other.m12;
        m13 = other.m13;
        m20 = other.m20;
        m21 = other.m21;
        m22 = other.m22;
        m23 = other.m23;
        m30 = other.m30;
        m31 = other.m31;
        m32 = other.m32;
        m33 = other.m33;
    }

    public Vec3 GetColumn(int index)
    {
        if (index < 0 || index > 3)
            throw new IndexOutOfRangeException("Indices must be between 0 and 3.");

        switch (index)
        {
            case 0: return new Vec3(m00, m01, m02);
            case 1: return new Vec3(m10, m11, m12);
            case 2: return new Vec3(m20, m21, m22);
            case 3: return new Vec3(m30, m31, m32);
        }

        return new Vec3(0, 0, 0);
    }

    public void SetColumn(int index, Vec3 column)
    {
        if (index < 0 || index > 3)
            throw new IndexOutOfRangeException("Indices must be between 0 and 3.");

        switch (index)
        {
            case 0:
                m00 = column.x;
                m10 = column.y;
                m20 = column.z;
                break;

            case 1:
                m01 = column.x;
                m11 = column.y;
                m21 = column.z;
                break;

            case 2:
                m02 = column.x;
                m12 = column.y;
                m22 = column.z;
                break;

            case 3:
                m03 = column.x;
                m13 = column.y;
                m33 = column.z;
                break;
        }
    }

    public Vec3 GetRow(int index)
    {
        switch (index)
        {
            case 0: return new Vec3(m00, m01, m03);
            case 1: return new Vec3(m01, m12, m13);
            case 2: return new Vec3(m02, m21, m23);
            case 3: return new Vec3(m03, m31, m33);
            default:
                throw new IndexOutOfRangeException("Row index must be 0 to 3");
        }
    }

    public void SetRow(int index, Vec3 row)
    {
        switch (index)
        {
            case 0:
                m00 = row.x;
                m01 = row.y;
                m02 = row.z;
                m03 = 0f;
                break;
            case 1:
                m10 = row.x;
                m11 = row.y;
                m12 = row.z;
                m13 = 0f;
                break;
            case 2:
                m20 = row.x;
                m21 = row.y;
                m22 = row.z;
                m23 = 0f;
                break;
            case 3:
                m30 = row.x;
                m31 = row.y;
                m32 = row.z;
                m33 = 0f;
                break;
            default:
                throw new IndexOutOfRangeException("Row index must be 0 to 3");
        }
    }

    public static M4X4 operator *(M4X4 lhs, M4X4 rhs)
    {
        return new M4X4
        (
            // fila 0
            lhs.m00 * rhs.m00 + lhs.m01 * rhs.m10 + lhs.m02 * rhs.m20 + lhs.m03 * rhs.m30,
            lhs.m00 * rhs.m01 + lhs.m01 * rhs.m11 + lhs.m02 * rhs.m21 + lhs.m03 * rhs.m31,
            lhs.m00 * rhs.m02 + lhs.m01 * rhs.m12 + lhs.m02 * rhs.m22 + lhs.m03 * rhs.m32,
            lhs.m00 * rhs.m03 + lhs.m01 * rhs.m13 + lhs.m02 * rhs.m23 + lhs.m03 * rhs.m33,

            // fila 1
            lhs.m10 * rhs.m00 + lhs.m11 * rhs.m10 + lhs.m12 * rhs.m20 + lhs.m13 * rhs.m30,
            lhs.m10 * rhs.m01 + lhs.m11 * rhs.m11 + lhs.m12 * rhs.m21 + lhs.m13 * rhs.m31,
            lhs.m10 * rhs.m02 + lhs.m11 * rhs.m12 + lhs.m12 * rhs.m22 + lhs.m13 * rhs.m32,
            lhs.m10 * rhs.m03 + lhs.m11 * rhs.m13 + lhs.m12 * rhs.m23 + lhs.m13 * rhs.m33,

            // fila 2
            lhs.m20 * rhs.m00 + lhs.m21 * rhs.m10 + lhs.m22 * rhs.m20 + lhs.m23 * rhs.m30,
            lhs.m20 * rhs.m01 + lhs.m21 * rhs.m11 + lhs.m22 * rhs.m21 + lhs.m23 * rhs.m31,
            lhs.m20 * rhs.m02 + lhs.m21 * rhs.m12 + lhs.m22 * rhs.m22 + lhs.m23 * rhs.m32,
            lhs.m20 * rhs.m03 + lhs.m21 * rhs.m13 + lhs.m22 * rhs.m23 + lhs.m23 * rhs.m33,

            // fila 3
            lhs.m30 * rhs.m00 + lhs.m31 * rhs.m10 + lhs.m32 * rhs.m20 + lhs.m33 * rhs.m30,
            lhs.m30 * rhs.m01 + lhs.m31 * rhs.m11 + lhs.m32 * rhs.m21 + lhs.m33 * rhs.m31,
            lhs.m30 * rhs.m02 + lhs.m31 * rhs.m12 + lhs.m32 * rhs.m22 + lhs.m33 * rhs.m32,
            lhs.m30 * rhs.m03 + lhs.m31 * rhs.m13 + lhs.m32 * rhs.m23 + lhs.m33 * rhs.m33
        );
    }

    public static M4X4 Translate(Vec3 v)
    {
        M4X4 m = Identity;
        m.m03 = v.x;
        m.m13 = v.y;
        m.m23 = v.z;

        return m;
    }

    public static M4X4 Rotate(Quat q)
    {
        float xx = q.x * q.x;
        float yy = q.y * q.y;
        float zz = q.z * q.z;
        float xy = q.x * q.y;
        float xz = q.x * q.z;
        float yz = q.y * q.z;
        float wx = q.w * q.x;
        float wy = q.w * q.y;
        float wz = q.w * q.z;

        M4X4 m = Identity;
        m.m00 = 1f - 2f * (yy + zz);
        m.m01 = 2f * (xy - wz);
        m.m02 = 2f * (xz + wy);

        m.m10 = 2f * (xy + wz);
        m.m11 = 1f - 2f * (xx + zz);
        m.m12 = 2f * (yz - wx);

        m.m20 = 2f * (xz - wy);
        m.m21 = 2f * (yz + wx);
        m.m22 = 1f - 2f * (xx + yy);

        return m;
    }

    public static M4X4 Scale(Vec3 v)
    {
        return new M4X4(
            v.x, 0f, 0f, 0f,
            0f, v.y, 0f, 0f,
            0f, 0f, v.z, 0f,
            0f, 0f, 0f, 1f
        );
    }

    public static M4X4 TRS(Vec3 pos, Quat q, Vec3 s)
    {
        M4X4 S = Scale(s);
        
        M4X4 R = Rotate(q);
        
        M4X4 T = Translate(pos);
        
        return T * R * S;
    }

    public Vec3 GetTranslation()
    {
        return new Vec3(m03, m13, m23);
    }
    
    public Quat GetRotation()
    {
        float trace = m00 + m11 + m22;
        Quat q = new Quat();

        if (trace > 0f)
        {
            float s = Mathf.Sqrt(trace + 1f) * 2f;
            q.w = 0.25f * s;
            q.x = (m21 - m12) / s;
            q.y = (m02 - m20) / s;
            q.z = (m10 - m01) / s;
        }
        else if (m00 > m11 && m00 > m22)
        {
            float s = Mathf.Sqrt(1f + m00 - m11 - m22) * 2f;
            q.w = (m21 - m12) / s;
            q.x = 0.25f * s;
            q.y = (m01 + m10) / s;
            q.z = (m02 + m20) / s;
        }
        else if (m11 > m22)
        {
            float s = Mathf.Sqrt(1f + m11 - m00 - m22) * 2f;
            q.w = (m02 - m20) / s;
            q.x = (m01 + m10) / s;
            q.y = 0.25f * s;
            q.z = (m12 + m21) / s;
        }
        else
        {
            float s = Mathf.Sqrt(1f + m22 - m00 - m11) * 2f;
            q.w = (m10 - m01) / s;
            q.x = (m02 + m20) / s;
            q.y = (m12 + m21) / s;
            q.z = 0.25f * s;
        }

        return q;
    }
    
    public Vec3 GetScale()
    {
        float sx = new Vec3(m00, m10, m20).magnitude;
        float sy = new Vec3(m01, m11, m21).magnitude;
        float sz = new Vec3(m02, m12, m22).magnitude;
        
        return new Vec3(sx, sy, sz);
    }
    
    public Matrix4x4 ToUnityMatrix()
    {
        Matrix4x4 mat = new Matrix4x4();

        mat.m00 = m00; mat.m01 = m01; mat.m02 = m02; mat.m03 = m03;
        mat.m10 = m10; mat.m11 = m11; mat.m12 = m12; mat.m13 = m13;
        mat.m20 = m20; mat.m21 = m21; mat.m22 = m22; mat.m23 = m23;
        mat.m30 = m30; mat.m31 = m31; mat.m32 = m32; mat.m33 = m33;

        return mat;
    }
}