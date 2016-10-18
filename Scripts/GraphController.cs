using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GraphController : MonoBehaviour {

    [Header("Drag And Drop Connection")]
    public DropZone DropZone;
    public int Drops;

    [System.Serializable]
    public class ListWrapper
    {
        public string lineName;
        public Color lineColor;
        public float lineWidth;
        public Texture dotTexture;
        public float dotRadius;
        public TextAsset textData;
        public List<Vector3> lineData = new List<Vector3>();
        public float lineDepth;
        [HideInInspector] public bool textDataAvailable = false;
        [HideInInspector] public TextAsset tempTextData;
    }

    [Header("Line Settings")]
    public List<ListWrapper> lineCount = new List<ListWrapper>();
    [Header("Camera Settings")]
    public Camera renderCamera;
    private float cameraSize;
    private List<Graph> goList = new List<Graph>();
    private int tempLineCount;
    private int graphs;

    void Update()
    {
        CreateGraphs(lineCount.Count);
        UpdateGraphs();
        lineDataUpdate();
        if (DropZone)
        {
            DragAndDrop();
        }
    }

    void DragAndDrop()
    {
        if (Drops < DropZone.goList.Count)
        {
            for (; Drops < DropZone.goList.Count; Drops++)
            {
                ListWrapper line = new ListWrapper();
                line.lineColor = DropZone.goList[Drops].GetComponent<Image>().color;
                if (DropZone.goList[Drops].GetComponent<CardController>().textData)
                {
                    line.textData = DropZone.goList[Drops].GetComponent<CardController>().textData;
                }
                lineCount.Add(line);
            }
        }else if(Drops > DropZone.goList.Count) {
            Drops--;
        }
      
    }

    void CreateGraphs(int count)
    {
        if (count > tempLineCount)
        {
            int diffrence = count - tempLineCount;

            for (; graphs < count; graphs++)
            {
                GameObject go = new GameObject();
                go.transform.SetParent(this.transform);
                go.name = "Line: " + lineCount[graphs].lineName;
                go.AddComponent<Graph>();
                Graph goGraph = go.GetComponent<Graph>();
                goGraph.GetComponent<LineRenderer>().SetPositions(lineCount[graphs].lineData.ToArray());
                lineCount[graphs].lineData.AddRange(new Vector3[1000]);
                goList.Add(goGraph);
            }
            tempLineCount = count;
        }
        else if(count < tempLineCount) {
            int diffrence = tempLineCount - count;
            for (int i = 0; i < diffrence; i++)
            {
                graphs--;
                Destroy(goList[goList.Count - 1].gameObject);
                goList.RemoveAt(goList.Count - 1);
            }
            tempLineCount = count;
        }
    }

    void UpdateGraphs()
    {
        for (int i = 0; i < lineCount.Count; i++)
        {
            goList[i].LinePoints = lineCount[i].lineData;

            lineCount[i].lineWidth = (lineCount[i].lineWidth == 0) ? 2.0f : lineCount[i].lineWidth;

            if (lineCount[i].lineWidth != goList[i].LineWidth)
                goList[i].LineWidth = lineCount[i].lineWidth;
            if (lineCount[i].lineColor != goList[i].Color)
                goList[i].Color = lineCount[i].lineColor;
            if (lineCount[i].dotTexture != goList[i].DotTexture)
                goList[i].DotTexture = lineCount[i].dotTexture;
            if (lineCount[i].dotRadius != goList[i].DotRadius)
                goList[i].DotRadius = lineCount[i].dotRadius;
            if (lineCount[i].lineDepth != goList[i].GraphDepth)
                goList[i].GraphDepth = lineCount[i].lineDepth;
        }
    }

    private void lineDataUpdate()
    {
        // Hoeveel lines zijn er?
        for (int i = 0; i < lineCount.Count; i++)
        {
            // We gaan alleen door als er geen textDataAvailable is
            if (lineCount[i].textDataAvailable == false)
            {
                // We gaan nu langs alle lines hun lijn data.
                for (int x = 0; x < lineCount[i].lineData.Count; x++)
                {
                    // we lezen deze code alleen als er een textdata bestand aanwezig is
                    if (lineCount[i].textData)
                    {
                        List<double> newlist = TextSeperator(lineCount[i].textData);
                        for (int nL = 0; nL < newlist.Count; nL++)
                        {
                            float valueY = (float)newlist[nL];
                            lineCount[i].lineData[nL] = new Vector3(lineCount[i].lineData[nL].x, valueY, lineCount[i].lineData[nL].z);
                        }
                        
                        lineCount[i].tempTextData = lineCount[i].textData;
                        lineCount[i].textDataAvailable = true;
                    }
                    lineCount[i].lineData[x] = new Vector3(25 * x, lineCount[i].lineData[x].y, lineCount[i].lineData[x].z);
                }
            }else if (!lineCount[i].textData || lineCount[i].textData != lineCount[i].tempTextData)
            {
                lineCount[i].textDataAvailable = false;
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
