using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cust : MonoBehaviour {
	public float gauge;
	public int energy;
	public float cust_speed = 2.0f;
	public Transform cust_num;

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
		cust_num.GetChild(0).GetComponent<Text>().text = "" + energy;

		// gauge filling
		int i = 0;
		while(i < 10) { //increment through each bar; the first 9 children
			if(i < energy) {   // full bars
				transform.GetChild(i).GetChild(0).GetComponent<Image>().fillAmount = 1.0f;
			}
			else if(i == energy) {  // filling bar
				transform.GetChild(i).GetChild(0).GetComponent<Image>().fillAmount = gauge - (float)energy;
				//Debug.Log(gauge - (float)energy);
			}
			else {
				transform.GetChild(i).GetChild(0).GetComponent<Image>().fillAmount = 0.0f;
			}
			i++;
		}
	}
}
