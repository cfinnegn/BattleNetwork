using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;


public struct DeckSlot {
	public int cardID, color_code;

	public DeckSlot(int ID, int color) {
		cardID = ID;
		color_code = color;
	}
}


public class Deck : MonoBehaviour {
	public List<DeckSlot> decklist;
	public List<int> deck_Id_inspect;
	public List<int> deck_color_inspect;
	public List<DeckSlot> deck_chips;
	public List<DeckSlot> used_chips;

	public TextAsset csvFile; // Reference of CSV file
	private char lineSeperater = '\n'; // It defines line seperate character
	private char fieldSeperator = ','; // It defines field seperate chracter
									   // Use this for initialization


	void Start () {
		//Build_FileIn();
	}
	
	// Update is called once per frame
	void Update () {
		if(deck_chips.Count == 0) { // later should be reloaded when hand is empty too
			Reload();
		}
	}

	public DeckSlot Draw_chip() {
		DeckSlot drawn = new DeckSlot(deck_chips[0].cardID, deck_chips[0].color_code);
		deck_chips.RemoveAt(0);
		used_chips.Add(drawn);
		return drawn;
	}

	public void Shuffle() {
		int i = deck_chips.Count;
		while(i > 1) {
			int n = UnityEngine.Random.Range(0, i);
			i--;
			DeckSlot swap = deck_chips[n];
			deck_chips[n] = deck_chips[i];
			deck_chips[i] = swap;
		}
	}

	public void Reload() {
		deck_chips.AddRange(used_chips);
		used_chips.Clear();
		Shuffle();
	}

	public void init() {
		deck_chips = decklist;
		foreach(DeckSlot d in decklist) {
			deck_Id_inspect.Add(d.cardID);
			deck_color_inspect.Add(d.color_code);
		}
		used_chips = new List<DeckSlot>();
		Shuffle();
	}

	public void Build_FileIn() {    // create deck from a CSV file
		//Debug.Log("building deck from " + csvFile);
		//	TODO: add exception handling?
		decklist = new List<DeckSlot>();
		string[] records = csvFile.text.Split(lineSeperater);
		foreach(string record in records) {
			string[] fields = record.Split(fieldSeperator);
			int? i = null;
			int? c = null;
			bool valsOK = true;
			foreach(string field in fields) {	// grab values from line
					try {
						if(i == null) {
							i = Int32.Parse(field);
						}
						else {
							c = Int32.Parse(field);
						}
					}
					catch(FormatException e) {
					valsOK = false;
						Debug.Log("CSV field error: " + field);
					}
			}
			if(valsOK) {	// no errors in reading data
				decklist.Add(new DeckSlot((int)i, (int)c));
				//Debug.Log("deck loaded chip id: " + i);
			}
		}
	}

	public void Build_FileOut(string filename) {   // generate a CSV from decklist
		foreach(DeckSlot d in decklist) {
			File.AppendAllText(Application.dataPath + "/Rescources/CSV/" + filename, 
				""+lineSeperater + d.cardID +fieldSeperator + d.color_code);
		}
	}
}
