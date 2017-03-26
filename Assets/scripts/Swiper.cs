using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swiper : MonoBehaviour {
	public GameObject buttons;
	public bool tap_mode = false;

	public bool swiping = false;
	int start_x;
	int start_y;
	int curr_x;
	int curr_y;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void toggle_tap() {
		tap_mode = !tap_mode;   // toggle boolean first
		if(tap_mode) { buttons.SetActive(true); }
		else { buttons.SetActive(false); }
	}

	public void swipe_start(int x, int y) {
	}
}
