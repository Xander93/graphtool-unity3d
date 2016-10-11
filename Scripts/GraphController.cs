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
        public List<Vector3> lineData = new List<Vector3>();
    }

    [Header("Line Settings")]
    public List<ListWrapper> lineCount = new List<ListWrapper>();

    [Header("Camera Settings")]
    public Camera renderCamera;
    private float cameraSize;

    void Start () {
		CreateGraphLines (lineCount.Count);
    }

	void CreateGraphLines(int count) {
		for (int i = 0; i < count; i++) {
			GameObject go = new GameObject ();
            go.transform.SetParent(this.transform);
            go.name = "Line" + lineCount[i].lineName;
            go.AddComponent<Graph> ();
            Graph goGraph = go.GetComponent<Graph> ();
            goGraph.LinePoints = lineCount[i].lineData;
            goGraph.lineColor = lineCount[i].lineColor;
            goGraph.lineWidth = lineCount[i].lineWidth;
            goGraph.dotTexture = lineCount[i].dotTexture;
        }
	}
}
