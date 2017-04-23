using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

//float pingTimer;
//int pingCount;
//public float[] pinged; (size 10)
//public void GetPing(int count) {
//	if(count < 9)
//		pinged[count] = PhotonNetwork.GetPing();
//	if(count == 9) {
//		pinged[count] = PhotonNetwork.GetPing();
//		float pingAvg = (pinged[0] + pinged[1] + pinged[2] + pinged[3] + pinged[4] + pinged[5] + pinged[6] + pinged[7] + pinged[8] + pinged[9]) / 10;
//		Text pingText = GameObject.Find("Ping").GetComponent<Text>();
//		pingText.text = "" + pingAvg;
//		if(PhotonNetwork.GetPing() < 100)
//			pingText.color = Color.green;
//		if(PhotonNetwork.GetPing() >= 100 && PhotonNetwork.GetPing() < 200)
//			pingText.color = Color.yellow;
//		if(PhotonNetwork.GetPing() >= 200)
//			pingText.color = Color.red;
//	}
//}
//And in update: 
//        // Ping
//        pingTimer -= Time.deltaTime;
//        if (pingTimer <= 0) {

//			GetPing(pingCount);
//            if (pingCount< 9) {
//                pingCount++;
//            } else {
//                pingCount = 0;
//            }
//            pingTimer = 0.1f;
//        }