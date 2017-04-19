﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey("escape")) {
			PhotonNetwork.LeaveRoom ();
			SceneManager.LoadScene(0, LoadSceneMode.Single);    // return to menu
		}
	}

	public void exit() {
		PhotonNetwork.LeaveRoom ();
		SceneManager.LoadScene(0, LoadSceneMode.Single);    // return to menu
	}

}
