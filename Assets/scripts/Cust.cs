using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cust : MonoBehaviour {
	public float gauge;
	public int energy;
	public float cust_speed = 2.0f;
	public Transform cust_num;

	public Navi navi;

	// Use this for initialization
	void Start () {
		cust_num = transform.GetChild(10);	// assuming 9 bars with num as 10th child!!
		gauge = 3.0f;
		energy = 3;
	}
	
	// Update is called once per frame
	void Update () {
		gauge += Time.deltaTime / cust_speed;
		gauge = (gauge > 10.0f) ? 10.0f : gauge;
		energy = (int)Mathf.Floor(gauge);

		if(navi != null) {
			navi.cust_energy = energy;
			navi.cust_fill = gauge;
			navi.updateCust();
		}
	}
}
