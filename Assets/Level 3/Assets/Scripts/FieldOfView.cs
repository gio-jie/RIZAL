using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
public class FieldOfView : MonoBehaviour
{
    [Header("FOV Settings")]
    public float viewRadius = 6f;
    [Range(0, 360)]
    public float viewAngle = 90f; // total angle

    public LayerMask obstacleMask;    // walls/obstacles layer
    public LayerMask targetMask;      // player layer (optional usage)

    [Header("Mesh Settings")]
    [Tooltip("Higher = smoother cone mesh, but more raycasts")]
    public int meshResolution = 30;
    public float edgeResolveIterations = 4;
    public float edgeDistanceThreshold = 0.5f;

    MeshFilter viewMeshFilter;
    Mesh viewMesh;

    void Awake()
    {
        viewMeshFilter = GetComponent<MeshFilter>();
        viewMesh = new Mesh();
        viewMesh.name = "FOV Mesh";
        viewMeshFilter.mesh = viewMesh;
    }

    void LateUpdate()
    {
        DrawFieldOfView();
    }

    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;

        List<Vector3> viewPoints = new List<Vector3>();

        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.z - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo vc = ViewCast(angle);
            viewPoints.Add(vc.point);
        }

        int vertexCount = viewPoints.Count + 1; // center + points
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero; // origin at local pos

        for (int i = 0; i < viewPoints.Count; i++)
        {
            // convert world to local
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < viewPoints.Count - 1)
            {
                int ti = i * 3;
                triangles[ti] = 0;
                triangles[ti + 1] = i + 1;
                triangles[ti + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    // Casts a ray at global angle (degrees) and returns hit point or max point
    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, viewRadius, obstacleMask);
        if (hit)
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
    }

    // helper to get direction vector from angle
    // angleIsGlobal: true if angle is in world space
    public Vector3 DirFromAngle(float angleDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal) angleDegrees += transform.eulerAngles.z;
        float rad = angleDegrees * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f);
    }

    // small struct used for viewcasts
    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float distance;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dist, float _angle)
        {
            hit = _hit;
            point = _point;
            distance = _dist;
            angle = _angle;
        }
    }
}
