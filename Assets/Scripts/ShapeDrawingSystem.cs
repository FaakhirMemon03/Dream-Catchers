using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class ShapeDrawingSystem : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float minDistance = 0.1f;
    public float recognitionThreshold = 0.8f;

    private List<Vector2> points = new List<Vector2>();
    private bool isDrawing = false;

    [Header("Events")]
    public UnityEvent onCircleDrawn;
    public UnityEvent onStarDrawn;
    public UnityEvent onZigZagDrawn;
    public UnityEvent onHeartDrawn;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartDrawing();
        }
        else if (Input.GetMouseButton(0) && isDrawing)
        {
            Draw();
        }
        else if (Input.GetMouseButtonUp(0) && isDrawing)
        {
            EndDrawing();
        }
    }

    void StartDrawing()
    {
        isDrawing = true;
        points.Clear();
        lineRenderer.positionCount = 0;
        lineRenderer.enabled = true;
    }

    void Draw()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if (points.Count == 0 || Vector2.Distance(mousePos, points[points.Count - 1]) > minDistance)
        {
            points.Add(mousePos);
            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPosition(points.Count - 1, new Vector3(mousePos.x, mousePos.y, 0));
        }
    }

    void EndDrawing()
    {
        isDrawing = false;
        lineRenderer.enabled = false;
        RecognizeShape();
    }

    void RecognizeShape()
    {
        if (points.Count < 10) return;

        // Simple Circle Detection Logic:
        // Check if start and end points are close, and if the points are roughly equidistant from a center
        if (IsCircle())
        {
            Debug.Log("Shape Recognized: Circle");
            onCircleDrawn?.Invoke();
        }
        else
        {
            Debug.Log("Shape not recognized");
        }
    }

    bool IsCircle()
    {
        Vector2 start = points[0];
        Vector2 end = points[points.Count - 1];

        // Start and end must be close
        if (Vector2.Distance(start, end) > 2.0f) return false;

        // Calculate centroid
        Vector2 centroid = Vector2.zero;
        foreach (var p in points) centroid += p;
        centroid /= points.Count;

        // Calculate average radius
        float avgRadius = 0;
        foreach (var p in points) avgRadius += Vector2.Distance(p, centroid);
        avgRadius /= points.Count;

        // Check variance in radius
        float variance = 0;
        foreach (var p in points)
        {
            float r = Vector2.Distance(p, centroid);
            variance += Mathf.Abs(r - avgRadius);
        }
        variance /= points.Count;

        return (variance / avgRadius) < 0.2f; // Tight threshold for circle
    }
}
