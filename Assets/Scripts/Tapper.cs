using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapper : MonoBehaviour {
	public Navi Navi;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void Up(){
		Navi.moveUp();
	}
	public void Down(){
		Navi.moveDown();
	}
	public void Left(){
		Navi.moveLeft();
	}
	public void Right(){
		Navi.moveRight();
	}
}
