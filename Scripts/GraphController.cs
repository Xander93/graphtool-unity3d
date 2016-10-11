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
        public List<Vector3> lineData = new List<Vector3>();
        public float lineDepth;
    }

    [Header("Line Settings")]
    public List<ListWrapper> lineCount = new List<ListWrapper>();

    [Header("Camera Settings")]
    public Camera renderCamera;

    private float cameraSize;
    private List<GameObject> goList = new List<GameObject>();

    void Start () {
		CreateGraphLines(lineCount.Count);
    }

    void Update() {
        UpdateGraphLines(goList.Count);
    }

	void CreateGraphLines(int count) {
		for (int i = 0; i < count; i++) {
			GameObject go = new GameObject ();
            go.transform.SetParent(this.transform);
            go.name = "Line" + lineCount[i].lineName;
            go.AddComponent<Graph> ();
            Graph goGraph = go.GetComponent<Graph> ();
            goGraph.LinePoints = lineCount[i].lineData;
            goGraph.material.color = lineCount[i].lineColor;
            goGraph.lineWidth = lineCount[i].lineWidth;
            goGraph.dotTexture = lineCount[i].dotTexture;
            goGraph.dotRadius = lineCount[i].dotRadius;
            goList.Add(go);
        }
	}

    void UpdateGraphLines(int count)
    {
        for (int i = 0; i < count; i++)
        {
            goList[i].GetComponent<Graph>().material.color = lineCount[i].lineColor;
            goList[i].GetComponent<Graph>().pointMat.color = lineCount[i].lineColor;
            Vector3[] positions = lineCount[i].lineData.ToArray();
            goList[i].GetComponent<Graph>().line.SetPositions(positions);
            goList[i].GetComponent<Graph>().UpdateDepth(lineCount[i].lineDepth);
        }
    }
}
