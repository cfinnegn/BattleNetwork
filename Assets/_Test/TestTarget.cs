using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class TestTarget : TrueSyncBehaviour {
	FP speed = 140;
	FP vSpeed = 0;
	int count;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public override void OnSyncedUpdate () {
		tsRigidBody2D.velocity = new TSVector2 (speed, vSpeed);
		if (count >= 4)
			count = 0;
		if (count == 0) {
			speed = 140;
			vSpeed = 0;
		}
		if (count == 1) {
			speed = 0;
			vSpeed = 140;
		}
		if (count == 2) {
			speed = -140;
			vSpeed = 0;
		}
		if (count == 3) {
			speed = 0;
			vSpeed = -140;
		}
		if (tsTransform2D.position.x >= 140 && speed > 0) {
			count++;
		}
		if (tsTransform2D.position.x <= -140 && speed < 0) {
			count++;
		}
		if (tsTransform2D.position.y >= 100 && vSpeed > 0) {
			count++;
		}
		if (tsTransform2D.position.y <= -100 && vSpeed < 0) {
			count++;
		}
	}
}
