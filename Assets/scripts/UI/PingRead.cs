using UnityEngine;
using UnityEngine.UI;
using TrueSync;
using System.Collections.Generic;

public class PingRead : MonoBehaviour {

	float pingTimer;
	int pingCount;
	float[] pinged;

	public void Awake(){
		pinged = new float[20];
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// Ping
		pingTimer -= Time.deltaTime;
		if (pingTimer <= 0) {
			GetPing (pingCount);
			if (pingCount < 19) {
				pingCount++;
			} else {
				pingCount = 0;
			}
			pingTimer = 0.1f;
		}
	}
	public void GetPing(int count){
		if (count < 19)
			pinged[count] = PhotonNetwork.GetPing ();
		if (count == 19) {
			pinged [count] = PhotonNetwork.GetPing ();
			float pingAvg = (pinged [0] + pinged [1] + pinged [2] + pinged [3] + pinged [4] + pinged [5] + pinged [6] + pinged [7] + pinged [8] + pinged [9] + pinged [10] + pinged [11] + pinged [12] + pinged [13] + pinged [14] + pinged [15] + pinged [16] + pinged [17] + pinged [18] + pinged [19]) / 20;
			Text pingText = GetComponent<Text> ();
			pingText.text = "" + Mathf.RoundToInt (pingAvg);
			if (pingAvg < 110)
				pingText.color = Color.green;
			if (pingAvg >= 110 && pingAvg < 220)
				pingText.color = Color.yellow;
			if (pingAvg >= 220)
				pingText.color = Color.red;
		}
	}
}