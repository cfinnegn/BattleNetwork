using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buster : MonoBehaviour {
	public Navi Navi;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void buster_shot(){
		Navi.bust_shot();
	}
	public void charge_shot(){
		Navi.charge_release ();
	}
}
