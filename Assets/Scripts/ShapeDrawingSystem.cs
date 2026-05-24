using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class ShapeDrawingSystem : MonoBehaviour
{
    [Header("Drawing Settings")]
    public LineRenderer lineRenderer;
    public float minDistance = 0.1f;
    public GameObject bubblePrefab;

    [Header("Events")]
    public UnityEvent onCircleDrawn;
    public UnityEvent onZigZagDrawn;
    public UnityEvent onStarDrawn;
    public UnityEvent onHeartDrawn;

    private List<Vector2> points = new List<Vector2>();
    private bool isDrawing = false;

    void Update()
    {
        // Support both Mouse and Mobile Touch
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

        if (IsCircle())
        {
            Debug.Log("Shape Recognized: Circle");
            SpawnBubble();
            onCircleDrawn?.Invoke();
        }
        else if (IsZigZag())
        {
            Debug.Log("Shape Recognized: ZigZag");
            onZigZagDrawn?.Invoke();
        }
        else if (IsStar())
        {
            Debug.Log("Shape Recognized: Star");
            onStarDrawn?.Invoke();
        }
        else if (IsHeart())
        {
            Debug.Log("Shape Recognized: Heart");
            onHeartDrawn?.Invoke();
        }
        else
        {
            Debug.Log("Shape not recognized");
        }
    }

    void SpawnBubble()
    {
        if (bubblePrefab != null)
        {
            Vector2 centroid = GetCentroid();
            Instantiate(bubblePrefab, new Vector3(centroid.x, centroid.y, 0), Quaternion.identity);
        }
    }

    Vector2 GetCentroid()
    {
        Vector2 centroid = Vector2.zero;
        foreach (var p in points) centroid += p;
        return centroid / points.Count;
    }

    bool IsCircle()
    {
        Vector2 start = points[0];
        Vector2 end = points[points.Count - 1];

        if (Vector2.Distance(start, end) > 2.0f) return false;

        Vector2 centroid = GetCentroid();
        float avgRadius = 0;
        foreach (var p in points) avgRadius += Vector2.Distance(p, centroid);
        avgRadius /= points.Count;

        float variance = 0;
        foreach (var p in points)
        {
            float r = Vector2.Distance(p, centroid);
            variance += Mathf.Abs(r - avgRadius);
        }
        variance /= points.Count;

        return (variance / avgRadius) < 0.25f; 
    }

    bool IsZigZag()
    {
        int directionChanges = 0;
        for (int i = 2; i < points.Count; i++)
        {
            Vector2 v1 = (points[i-1] - points[i-2]).normalized;
            Vector2 v2 = (points[i] - points[i-1]).normalized;

            if (Vector2.Dot(v1, v2) < 0.5f)
            {
                directionChanges++;
            }
        }
        return directionChanges > 3;
    }

    bool IsStar()
    {
        // Simple star heuristic: 5 sharp turns (points)
        int sharpTurns = 0;
        for (int i = 2; i < points.Count; i++)
        {
            Vector2 v1 = (points[i - 1] - points[i - 2]).normalized;
            Vector2 v2 = (points[i] - points[i - 1]).normalized;

            if (Vector2.Dot(v1, v2) < -0.2f) // Very sharp turn
            {
                sharpTurns++;
            }
        }
        return sharpTurns >= 4 && sharpTurns <= 6;
    }

    bool IsHeart()
    {
        // Heuristic: starts at bottom, goes up/around, ends near start
        Vector2 start = points[0];
        Vector2 end = points[points.Count - 1];
        if (Vector2.Distance(start, end) > 1.5f) return false;

        // Check if top points are higher than start/end
        float topY = float.MinValue;
        foreach (var p in points) if (p.y > topY) topY = p.y;

        return (topY - start.y) > 1.0f;
    }
}
