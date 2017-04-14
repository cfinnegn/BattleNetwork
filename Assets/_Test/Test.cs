using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class Test : TrueSyncBehaviour {
	public GameObject target;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public override void OnSyncedUpdate () {
		//TrueSync.TSMath replaces Math, TrueSyncManager.DeltaTime for time.deltatime
		//tsRigidBody2D.velocity, tsTrasnsform2D.position, etc take a new TSVector2(FP,FP)
		//tsTransform2D.rotation only takes (FP), just the z axis rotation. No Quaternions.

		tsTransform2D.rotation = positiveAngle(tsTransform2D.rotation);	// 0-360
		FP missleSpeed = 2.3f;  // use the missle speed from your code
		FP turnSpeed = 5.5f; // use the turn speed from your code
		FP near_angle = turnSpeed*.8;	// for near horizontal/vertical angles
		TSVector2 movement = new TSVector2(0f, 0f);
		FP diffx = target.GetComponent<TSTransform2D>().position.x - tsTransform2D.position.x;
		FP diffy = target.GetComponent<TSTransform2D>().position.y - tsTransform2D.position.y;
		//if((TrueSync.TSMath.Abs(diffx) < 2 * missleSpeed) && (TrueSync.TSMath.Abs(diffy) < 2 * missleSpeed)) { return; }    // stop when close


		// ****real logic starts here****
		// first find the rotation hat would point you directly at your target
		FP angle_to_target = TrueSync.TSMath.Atan(diffy / diffx) * (180f / TrueSync.TSMath.Pi);   // use trig to find angle from missle to target
		if(diffx < 0) {	// target to left
			angle_to_target += 180f;	// flip angle because Unity angle calculation is strange
		}
		angle_to_target = positiveAngle(angle_to_target);	// 0-360


		// next set the amount you will rotate on this frame (either +/- turnspeed, or set to target rotation if within limits
		if(TrueSync.TSMath.Abs(angle_to_target - tsTransform2D.rotation) > turnSpeed) { // difference in angle > max turn
			// determine if it would be faster to rotate clockwise or counter-clockwise to point to target
			if(positiveAngle(angle_to_target - tsTransform2D.rotation) < 180f) { // rotate CCW
				tsTransform2D.rotation += turnSpeed;
			}
			else {
				tsTransform2D.rotation -= turnSpeed;// rotate CW
			}
		}
		else {	// aim directly at target if within turn speed
			tsTransform2D.rotation = angle_to_target;
		}
		tsTransform2D.rotation = positiveAngle(tsTransform2D.rotation); // 0-360
		
		
		// check for near vert/horiz angle (this should help reduce the jitter)
		if(tsTransform2D.rotation < near_angle) {	// nearly 0
			tsTransform2D.rotation = 0f;
		}
		else if(TrueSync.TSMath.Abs(tsTransform2D.rotation - 90f) < near_angle) {	// nearly 90
			tsTransform2D.rotation = 90f;
		}
		else if(TrueSync.TSMath.Abs(tsTransform2D.rotation - 180f) < near_angle) {   // nearly 180
			tsTransform2D.rotation = 180f;
		}
		else if(TrueSync.TSMath.Abs(tsTransform2D.rotation - 270f) < near_angle) {   // nearly 270
			tsTransform2D.rotation = 270f;
		}
		else if(TrueSync.TSMath.Abs(tsTransform2D.rotation - 360f) < near_angle) {   // nearly 360
			tsTransform2D.rotation = 0f;
		}

		// adjust movement vector in proportion to angle
		// convert to radians with *(Pi/180) 
		// curse at Truesync and its stupid insistence on using Radians when Unity even displays in degrees
		movement.x = TrueSync.TSMath.Abs(missleSpeed * TrueSync.TSMath.Cos((tsTransform2D.rotation *(TrueSync.TSMath.Pi/180)))); 
		if((90f < tsTransform2D.rotation) && (tsTransform2D.rotation < 270f)) { // going left
			movement.x *= -1;
		}
		movement.y = TrueSync.TSMath.Abs(missleSpeed * TrueSync.TSMath.Sin((tsTransform2D.rotation * (TrueSync.TSMath.Pi / 180))));
		if((180f < tsTransform2D.rotation) && (tsTransform2D.rotation < 360f)) { // going down
			movement.y *= -1;
		}
		FP dir = tsTransform2D.rotation;
		tsTransform2D.position += movement;		// shift position by movement vector
	}

	public FP positiveAngle(FP angle) {	// helper function, sets a rotation angle between 0-360
		while(angle >= 360f) {
			angle -= 360f;
		}
		while(angle < 0f) {
			angle += 360f;
		}
		return angle;
	}
}
