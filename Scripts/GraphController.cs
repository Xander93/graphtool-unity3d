using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public class GraphController : MonoBehaviour {

    [System.Serializable]
    public class ListWrapper
    {
        public string lineName;
        public Color lineColor;
        public float lineWidth = 2.0f;
        public Texture dotTexture;
        public float dotRadius;
        public TextAsset textData;
        public List<Vector3> lineData = new List<Vector3>();
    }

    [Header("Line Settings")]
    public List<ListWrapper> lineCount = new List<ListWrapper>();
    [Header("Camera Settings")]
    public Camera renderCamera;
    private float cameraSize;
    private List<Graph> goList = new List<Graph>();

    void Awake ()
    {
        lineDataUpdate();
        CreateGraphs(lineCount.Count);
        UpdateGraphs();
    }

    void CreateGraphs(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject go = new GameObject();
            go.transform.SetParent(this.transform);
            go.name = "Line" + lineCount[i].lineName;
            go.AddComponent<Graph>();
            Graph goGraph = go.GetComponent<Graph>();
            goGraph.GetComponent<LineRenderer>().SetPositions(lineCount[i].lineData.ToArray());
            goList.Add(goGraph);
        }
    }

    void Update ()
    {
        UpdateGraphs();
        lineDataUpdate();
    }

    void UpdateGraphs()
    {
        for (int i = 0; i < lineCount.Count; i++)
        {
            goList[i].LinePoints = lineCount[i].lineData;

            if (lineCount[i].lineWidth != goList[i].LineWidth)
                goList[i].LineWidth = lineCount[i].lineWidth;
            if (lineCount[i].lineColor != goList[i].Color)
                goList[i].Color = lineCount[i].lineColor;
            if (lineCount[i].dotTexture != goList[i].DotTexture)
                goList[i].DotTexture = lineCount[i].dotTexture;
            if (lineCount[i].dotRadius != goList[i].DotRadius)
                goList[i].DotRadius = lineCount[i].dotRadius;
        }
    }

    private void lineDataUpdate()
    {
        bool textData = false;
        //Start line position with or withouth a text file
        for (int i = 0; i < lineCount.Count; i++)
        {
            if (textData == false)
            {
                for (int x = 0; x < lineCount[i].lineData.Count; x++)
                {
                    if (lineCount[i].textData)
                    {
                        List<double> newYlist = TextSeperator(lineCount[i].textData);
                        float value = (float)newYlist[x];
                        lineCount[i].lineData[x] = new Vector3(lineCount[i].lineData[x].x, value, lineCount[i].lineData[x].z);
                        textData = true;
                    }
                    lineCount[i].lineData[x] = new Vector3(25 * x, lineCount[i].lineData[x].y, lineCount[i].lineData[x].z);
                }
            }else if (!lineCount[i].textData)
            {
                textData = false;
            }
        }
    }

    private List<double> TextSeperator(TextAsset textAsset)
    {
        string text = textAsset.ToString();
        List<double> stringList = new List<double>();
        char[] separatingChars = { ',', ' ' };

        string[] words = text.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string s in words)
        {
            stringList.Add(double.Parse(s));
        }

        return stringList;
    }
}
