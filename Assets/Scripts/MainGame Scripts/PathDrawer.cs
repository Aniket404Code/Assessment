using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(EdgeCollider2D))]
public class PathDrawer : MonoBehaviour
{
    public Path path;                       // for editor compatibility
    public int MyCurrentNumber;             // for old code compatibility
    public LineRenderer myLineRenderer;     // LineRenderer for path drawing
    public EdgeCollider2D edgeCollider;     // EdgeCollider2D for pointer checking

    private void Awake()
    {
        // Ensure LineRenderer exists
        if (myLineRenderer == null)
        {
            myLineRenderer = gameObject.GetComponent<LineRenderer>();
            if (myLineRenderer == null)
            {
                myLineRenderer = gameObject.AddComponent<LineRenderer>();
                myLineRenderer.widthMultiplier = 0.2f;
            }
        }

        // Ensure EdgeCollider2D exists
        if (edgeCollider == null)
        {
            edgeCollider = gameObject.GetComponent<EdgeCollider2D>();
            if (edgeCollider == null)
            {
                edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
            }
        }

        if (path == null)
        {
            CreatePath();
        }
    }

    public void CreatePath()
    {
        path = new Path(transform.position);
        EnsureLineRendererExists();
        myLineRenderer.positionCount = path.points.Count;
    }

    private void EnsureLineRendererExists()
    {
        if (myLineRenderer == null)
        {
            myLineRenderer = gameObject.GetComponent<LineRenderer>();
            if (myLineRenderer == null)
            {
                myLineRenderer = gameObject.AddComponent<LineRenderer>();
                myLineRenderer.widthMultiplier = 0.2f;
            }
        }
    }

    public void DrawPath(List<Vector2> points)
    {
        if (points == null || points.Count == 0)
        {
            Debug.LogWarning("DrawPath called with null or empty points list.");
            if (myLineRenderer != null)
                myLineRenderer.positionCount = 0;
            return;
        }

        EnsureLineRendererExists();
        myLineRenderer.positionCount = points.Count;

        // Set positions for LineRenderer
        for (int i = 0; i < points.Count; i++)
        {
            myLineRenderer.SetPosition(i, new Vector3(points[i].x, points[i].y, 0));
        }

        // Use the public edgeCollider instead of local variable
        if (edgeCollider == null)
        {
            edgeCollider = gameObject.GetComponent<EdgeCollider2D>();
            if (edgeCollider == null)
                edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
        }

        // Assign points to EdgeCollider
        Vector2[] edgePoints = new Vector2[points.Count];
        for (int i = 0; i < points.Count; i++)
        {
            edgePoints[i] = points[i]; // no offset unless required
        }
        edgeCollider.points = edgePoints;
    }
}
