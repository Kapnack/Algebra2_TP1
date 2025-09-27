using UnityEngine;

public class Test : MonoBehaviour
{
    public Mesh mesh1;
    public Mesh mesh2;
    public Mesh mesh3;

    [SerializeField] private Trans root;
    [SerializeField] private Trans child1;
    [SerializeField] private Trans child2;

    private void Awake()
    {
        root = new Trans(new Vec3(0, 0, 0), Quat.identity, new Vec3(3, 3, 3));
        child1 = new Trans(new Vec3(2, 0, 0), Quat.identity, new Vec3(1, 1, 1));
        child2 = new Trans(new Vec3(0, 1, 0), Quat.identity, new Vec3(0.5f, 0.5f, 0.5f));

        root.AddChild(child1);
        child1.AddChild(child2);
    }

    private void Update()
    {
        root.localRotation = Quat.AngleAxis(Time.time * 45f, new Vec3(0, 1, 0));
        child1.localRotation = Quat.AngleAxis(Time.time * 90f, new Vec3(0, 0, 1));
    }

    private void OnValidate()
    {
        root.OnValidate();
        child1.OnValidate();
        child2.OnValidate();
    }
    
    private void OnDrawGizmos()
    {
        if (root == null)
            return;

        DrawObject(root, mesh1, Color.red);
        DrawObject(child1, mesh2, Color.green);
        DrawObject(child2, mesh3, Color.blue);
    }

    private void DrawObject(Trans t, Mesh mesh, Color color)
    {
        Gizmos.color = color;
        Gizmos.matrix = t.localToWorldMatrix.ToUnityMatrix();
        Gizmos.DrawMesh(mesh);
    }
}