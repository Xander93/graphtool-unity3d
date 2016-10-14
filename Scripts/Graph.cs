using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Graph : MonoBehaviour
{

    private List<Vector3> _linepoints = new List<Vector3>();
    public List<Vector3> LinePoints
    {
        get
        {
            return _linepoints;
        }
        set
        {
            _linepoints = value;
            UpdateLine();
            UpdateDots();
        }
    }

    private float _linewidth;
    public float LineWidth
    {
        get
        {
            return _linewidth;
        }
        set
        {
            _linewidth = value;
            UpdateLine();
        }
    }

    [SerializeField]
    private Color _color;
    public Color Color
    {
        get
        {
            return _color;
        }
        set
        {
            _color = value;
            UpdateDots();
            UpdateLine();
        }
    }

    private float _dotradius;
    public float DotRadius
    {
        get
        {
            return _dotradius;
        }
        set
        {
            _dotradius = value;
            UpdateDots();
            Debug.Log("Dot Updated!");
        }
    }

    private Texture _dotTexture;
    public Texture DotTexture
    {
        get
        {
            return _dotTexture;
        }
        set
        {
            _dotTexture = value;
            UpdateDots();
            Debug.Log("Dot Updated!");
        }
    }

    private float _graphDepth;
    public float GraphDepth
    {
        get
        {
            return _graphDepth;
        }
        set
        {
            UpdateGraphDepth();
            _graphDepth = value;
        }
    }

    private LineRenderer line;
    private GraphController gControl;
    private Material lineMat;
    private Material dotMat;
    private List<GameObject> LineDots = new List<GameObject>();

    // Use this for initialization
    void Awake()
    {
        gControl = GameObject.Find("GraphController").GetComponent<GraphController>();
        lineMat = new Material(Shader.Find("VertexLit"));
        dotMat = new Material(Shader.Find("Sprites/Default"));
        this.gameObject.layer = 8;
        this.gameObject.AddComponent<LineRenderer>();
        line = this.gameObject.GetComponent<LineRenderer>();
    }

    void Start()
    {
        DrawLine();
        UpdateLine();
        DrawDots();
        UpdateDots();
    }

    void DrawLine()
    {
        line.numPositions = _linepoints.Count;
        float cameraSize = gControl.renderCamera.orthographicSize;

        /*float maxY = FindMaxY(_linepoints);
        float minY = FindMinY(_linepoints);
        float offset = 20f;

        if (_dotTexture)
        {
            offset = _dotradius * offset;
        }
        else
        {
            offset = _linewidth * offset;
        }

        for (int i = 0; i < _linepoints.Count; i++)
        {
            if (_linepoints[i].y > 0)
            {
                _linepoints[i] = new Vector3(_linepoints[i].x, (_linepoints[i].y / maxY) * cameraSize - offset, _linepoints[i].z);
            }
            else if (_linepoints[i].y < 0)
            {
                _linepoints[i] = new Vector3(_linepoints[i].x, (_linepoints[i].y / minY) * -cameraSize + offset, _linepoints[i].z);
            }
        }*/

        line.SetPositions(_linepoints.ToArray());        
    }

    void UpdateLine() {
        if (lineMat.color != _color)
        {
            lineMat.color = _color;
            line.material = lineMat;
        }
        if (line.widthMultiplier != _linewidth)
            line.widthMultiplier = _linewidth;

        if (!_dotTexture)
        {
            foreach (var item in LineDots)
            {
                item.SetActive(false);
            }
        } else {
            foreach (var item in LineDots)
            {
                item.SetActive(true);
            }
        }

        line.SetPositions(_linepoints.ToArray());
    }

    private float FindMaxY(List<Vector3> list)
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

    private float FindMinY(List<Vector3> list)
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

    void DrawDots()
    {
        foreach (var item in _linepoints)
        {
            GameObject linePointGo = GameObject.CreatePrimitive(PrimitiveType.Quad);
            dotMat.mainTexture = _dotTexture;
            dotMat.color = lineMat.color;
            linePointGo.GetComponent<MeshRenderer>().material = dotMat;
            linePointGo.layer = 8;
            linePointGo.name = "Dot";
            linePointGo.transform.SetParent(this.transform);
            LineDots.Add(linePointGo);
        }
    }

    void UpdateDots()
    {
        for (int i = 0; i < LineDots.Count; i++)
        {
            if (LineDots[i].transform.position != _linepoints[i] + new Vector3(0, 0, -0.50f))
                LineDots[i].transform.position = _linepoints[i] + new Vector3(0, 0, -0.50f);
            if (LineDots[i].transform.localScale != new Vector3(DotRadius, DotRadius, DotRadius))
                LineDots[i].transform.localScale = new Vector3(DotRadius, DotRadius, DotRadius);
            if (LineDots[i].GetComponent<MeshRenderer>().material.mainTexture != _dotTexture)
                LineDots[i].GetComponent<MeshRenderer>().material.mainTexture = _dotTexture;
            if (LineDots[i].GetComponent<MeshRenderer>().material.color != _color)
                LineDots[i].GetComponent<MeshRenderer>().material.color = _color;
        }
    }

   void UpdateGraphDepth() {
        for (int i = 0; i < _linepoints.Count; i++)
        {
            _linepoints[i] = new Vector3(_linepoints[i].x, _linepoints[i].y, _graphDepth);
        }
    }
}
