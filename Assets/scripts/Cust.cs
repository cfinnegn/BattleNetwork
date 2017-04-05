using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TrueSync;

public class Cust : TrueSyncBehaviour {
	public FP gauge;
	public FP energy;
	public FP used_energy = 0;
	public FP cust_speed = 2.0f;
	public Transform cust_num;

	public bool pending_energy_use = false;

	// Use this for initialization
	void Start() {
		cust_num = transform.GetChild(10);  // assuming 9 bars with num as 10th child!!
		gauge = 3.0f;
		energy = 3;
	}

	// Update is called once per frame
	public override void OnSyncedUpdate() {
		gauge += TrueSyncManager.DeltaTime / cust_speed;
		if(pending_energy_use) {
			gauge -= used_energy;
		}
		pending_energy_use = false;
		gauge = (gauge > 10.0f) ? 10.0f : gauge;
		energy = (FP)TrueSync.TSMath.Floor(gauge);
		cust_num.GetChild(0).GetComponent<Text>().text = "" + energy;

		// gauge filling
		int i = 0;
		while(i < 10) { //increment through each bar; the first 9 children
			if(i < energy) {   // full bars
				transform.GetChild(i).GetChild(0).GetComponent<Image>().fillAmount = 1.0f;
			}
			else if(i == energy) {  // filling bar
				transform.GetChild(i).GetChild(0).GetComponent<Image>().fillAmount = gauge.AsFloat() - (float)energy.AsFloat();
				//Debug.Log(gauge - (float)energy);
			}
			else {
				transform.GetChild(i).GetChild(0).GetComponent<Image>().fillAmount = 0.0f;
			}
			i++;
		}
	}

	public void spend(FP cost) {
		used_energy = cost;
		Debug.Log("spend");
		pending_energy_use = true;
	}
}