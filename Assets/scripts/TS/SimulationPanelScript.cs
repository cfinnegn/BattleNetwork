using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TrueSync;

public class SimulationPanelScript : MonoBehaviour {

	public void Update() {
		if(ReplayRecord.replayMode == ReplayMode.LOAD_REPLAY)
			gameObject.SetActive(true);
		else
			gameObject.SetActive(false);
	}

	public void BtnRun() {
		TrueSyncManager.RunSimulation();
	}

	public void BtnPause() {
		TrueSyncManager.PauseSimulation();
	}

	public void BtnEnd() {
		TrueSyncManager.EndSimulation ();
		PhotonNetwork.LeaveRoom ();
		SceneManager.LoadScene ("Menu");
	}

}