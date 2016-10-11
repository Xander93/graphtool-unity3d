using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Graph : MonoBehaviour {

    public List<Vector3> LinePoints = new List<Vector3>();
    public List<GameObject> LineDots = new List<GameObject>();
    public float lineWidth;
    public Texture dotTexture;
    public float dotRadius;
    public Material material;
    public Material pointMat;
    public LineRenderer line;

    private GraphController gControl;

    // Use this for initialization
    void Awake () {
        material = new Material(Shader.Find("VertexLit"));
        this.gameObject.layer = 8;
        this.gameObject.AddComponent<LineRenderer> ();
        line = this.gameObject.GetComponent<LineRenderer>();
        gControl = GameObject.Find("GraphController").GetComponent<GraphController>();
        pointMat = new Material(Shader.Find("Sprites/Default"));
    }

    void Start ()
    {
        DrawLine();
    }

    void Update() {
        UpdateDots();
    }

    void DrawLine()
    {
        line.numPositions = LinePoints.Count;
        line.widthMultiplier = lineWidth;
        line.material = material;
        float cameraSize = gControl.renderCamera.orthographicSize;

        float maxY = FindMaxY(LinePoints);
        float minY = FindMinY(LinePoints);
        float offset;

        if (dotTexture) {
            offset = dotRadius * 0.5f;
        }
        else {
            offset = lineWidth;
        }

        for (int i = 0; i < LinePoints.Count; i++)
        {
            if (LinePoints[i].y > 0)
            {
                LinePoints[i] = new Vector3(LinePoints[i].x, (LinePoints[i].y / maxY) * cameraSize - offset, LinePoints[i].z);
            } else if (LinePoints[i].y < 0)
            {
                LinePoints[i] = new Vector3(LinePoints[i].x, (LinePoints[i].y / minY) * -cameraSize + offset, LinePoints[i].z);
            }
        }

        line.SetPositions(LinePoints.ToArray());

        if (dotTexture)
        {
            DrawDots(dotRadius);
        }
    }

    public float FindMaxY(List<Vector3> list)
    {
        if (list.Count == 0)
        {
            throw new InvalidOperationException("Empty list");
        }
        float maxY = float.MinValue;
        foreach (Vector3 type in list)
        {
            if (type.y > maxY)
            {
                maxY = type.y;
            }
        }
        return maxY;
    }

    public float FindMinY(List<Vector3> list)
    {
        if (list.Count == 0)
        {
            throw new InvalidOperationException("Empty list");
        }
        float minY = float.MaxValue;
        foreach (Vector3 type in list)
        {
            if (type.y < minY)
            {
                minY = type.y;
            }
        }
        return minY;
    }

    void DrawDots(float scale)
    {
        foreach (var item in LinePoints)
        {
            GameObject linePointGo = GameObject.CreatePrimitive(PrimitiveType.Quad);
            pointMat.mainTexture = dotTexture;
            pointMat.color = material.color;
            linePointGo.GetComponent<MeshRenderer>().material = pointMat;
            linePointGo.transform.position = item + new Vector3(0, 0, -0.25f);
            linePointGo.layer = 8;
            linePointGo.name = "Dot";
            linePointGo.transform.localScale = new Vector3(scale, scale, scale);
            linePointGo.transform.SetParent(this.transform);
            LineDots.Add(linePointGo);
        }
    }

    void UpdateDots() {
        int i = 0;
        foreach (var item in LineDots)
        {
            item.transform.position = LinePoints[i];
            i++;
        }
    }

    public void UpdateDepth(float depth) {
        for (int i = 0; i < LinePoints.Count; i++)
        {
            LinePoints[i] = new Vector3(LinePoints[i].x, LinePoints[i].y, depth);
        }
    }
}
