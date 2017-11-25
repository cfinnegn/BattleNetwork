using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DeckBuilder : MonoBehaviour {

	public Chip_InfoPanel chip_info;
	public Chip_Library CL;

	public Pallette pallette;

	public int decksize;
	public Text decksize_view;
	public GameObject fs;
	public GameObject list_view;
	public List<FolderSlot> Decklist = new List<FolderSlot>();

	public GameObject deck_build_warning;
	public Text warning_text;

	// Use this for initialization
	void Start () {
		deck_count();
	}
	
	// Update is called once per frame
	void Update () {
		if(pallette.color_code != null) {	// color of chip to be added selected, so verify addition and clear pallette color
			verify_chip(new DeckSlot(chip_info.chip_ID, pallette.color_code.GetValueOrDefault()));
			pallette.color_code = null;
		}
	}

	public void Chip_Added() {     
		if(CL.selected_color != null) {	// add chip with predetermined color (by filter)
			verify_chip(new DeckSlot(chip_info.chip_ID, CL.selected_color.GetValueOrDefault()));
		}
		else {	// open pallette to selected color of chip to add
			pallette.gameObject.SetActive(true);
			pallette.set_dots_active(ChipDatabase.chipDB[chip_info.chip_ID].color_opt);
		}
	}

	public bool verify_chip(DeckSlot ds) {
		if(decksize >= 30) {	// deck not at max size
			warning_deck_size();
			return false;
		}
		FolderSlot findslot = chip_in_deck(ds.cardID);
		if(findslot != null) {	// at least 1 copy of this chip is already in the deck
			if (findslot.numCopies >= 3) {	// deck cannot have more copies of this chip
				warning_chip_max();
				return false;
			}
			else {	// add to folder slot instead of making new one
				findslot.add_copy(ds);
				return true;
			}
		}

		else {
			// TODO: verify chip is legal to add to current deck
			GameObject newslot = Instantiate(fs, list_view.transform);  // create folderslot object in scrollview
																		//instantiate folderslot object with ID and color info
			newslot.GetComponent<FolderSlot>().deckbuilder = this;
			newslot.GetComponent<FolderSlot>().init(ds);
			Decklist.Add(newslot.GetComponent<FolderSlot>());
			deck_count();
			return true;
		}
	}

	public void deck_count() {
		decksize = 0;
		foreach (FolderSlot f in Decklist) {
			decksize += f.numCopies;
		}
		decksize_view.text = "" + decksize;
	}

	public FolderSlot chip_in_deck(int ID) {
		foreach( FolderSlot f in Decklist) {
			if(f.chipID == ID) {
				return f;
			}
		}
		return null;
	}

	public void remove(FolderSlot f) {
		Decklist.Remove(f);
		GameObject.Destroy(f.gameObject);
		deck_count();
	}

	public void warning_deck_size() {
		deck_build_warning.SetActive(true);
		warning_text.text = "Folders must contain exactly 30 chips.";
	}
	public void warning_chip_max() {
		deck_build_warning.SetActive(true);
		warning_text.text = "Folders may contain no more than 3 copies of any given chip.";
	}
	public void warning_color_max() {
		deck_build_warning.SetActive(true);
		warning_text.text = "Folders may contain no more than 20 chips of a single colorcode.";
	}
	public void warning_white_max() {
		deck_build_warning.SetActive(true);
		warning_text.text = "Folders may contain no more than 5 chips with a WHITE colorcode.";
	}
}
