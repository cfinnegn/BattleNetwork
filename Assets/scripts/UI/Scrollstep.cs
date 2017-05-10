using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scrollstep : MonoBehaviour {

	public GameObject content;
	public Scrollbar scrollbar;
	public int visible_rows;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		int num_children = content.transform.childCount;
		// assuming constrained based on # columns
		int columns = content.GetComponent<GridLayoutGroup>().constraintCount;
		//Debug.Log(num_children);

		scrollbar.numberOfSteps = ((num_children) / columns) - visible_rows + 1;
		scrollbar.numberOfSteps += (num_children % columns > 0) ? 1 : 0; 
	}
}
