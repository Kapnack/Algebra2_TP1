using System;


[Serializable]
public struct Plane3
{
    public Vec3 normal;
    public float d;
    
    public static Plane3 BisectorPlane(Vec3 a, Vec3 b)
    {
        Vec3 dir = b - a;
        Vec3 mid = new Vec3((a.x + b.x) * 0.5f, (a.y + b.y) * 0.5f, (a.z + b.z) * 0.5f);
        return new Plane3(dir, mid);
    }
    
    public Plane3(Vec3 normal, Vec3 pointOnPlane)
    {
        float mag = normal.magnitude;
        
        if (mag > Vec3.epsilon)
            this.normal = normal / mag;
        else
            this.normal = new Vec3(0, 1, 0);
        
        d = Vec3.Dot(this.normal, pointOnPlane);
    }
    
    public float SignedDistance(Vec3 p) => Vec3.Dot(normal, p) - d;
    
    
    public bool IsPositiveSide(Vec3 p) => SignedDistance(p) > 0f;
    
}