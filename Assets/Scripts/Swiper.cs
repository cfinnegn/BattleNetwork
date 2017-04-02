using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Swiper : MonoBehaviour {
	public Navi Navi;

	public GameObject buttons;
	public bool tap_mode = false;

	public bool swiping = false;
	float start_x;
	float start_y;

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

	public void swipe_start(BaseEventData data) {
		if(!tap_mode) {
			PointerEventData pointer = data as PointerEventData;
			start_x = pointer.position.x;
			start_y = pointer.position.y;
			swiping = true;
			//Debug.Log("x: " + start_x + ", y: " + start_y);         //!!!!!! DEBUG STATEMENT !!!!!!
		}
	}

	public void swipe_end(BaseEventData data) {
		if(!tap_mode) {
			if(swiping) { // avoids unnecesarry triggers when passed over without swiping
				PointerEventData pointer = data as PointerEventData;
				//Debug.Log("x: " + pointer.position.x + ", y: " + pointer.position.y);    //!!!!!! DEBUG STATEMENT !!!!!!
				float delta_x = start_x - pointer.position.x;
				float delta_y = start_y - pointer.position.y;
				if(Mathf.Abs(delta_x) > 50.0f || Mathf.Abs(delta_y) > 50.0f) {	// discounts swipes of length < 50
					if(Mathf.Abs(delta_x) >= Mathf.Abs(delta_y)) {  //greater horizontal change (slight bias)
						if(delta_x > 0) { Navi.moveLeft(); }
						else { Navi.moveRight(); }
					}
					else {      // greater vertical change
						if(delta_y > 0) { Navi.moveDown(); }
						else { Navi.moveUp(); }
					}
				}
				swiping = false;
			}
		}
	}
}
