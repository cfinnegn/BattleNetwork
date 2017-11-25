using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FolderSlot : MonoBehaviour {

	public DeckBuilder deckbuilder;
	public int chipID;

	public Image chipImage;
	public Text chipName;
	public Image elem;
	public Text cost;

	public int numCopies = 0;
	public int?[] colorcodes = new int?[] { null, null, null };
	public Image[] colorCode_disp = new Image[3];

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void init(DeckSlot ds) {	// TODO: add multi-copy handling
		this.chipID = ds.cardID;
		this.chipImage.sprite = ChipDatabase.chipDB[ds.cardID].chipimg;
		this.chipName.text = ChipDatabase.chipDB[ds.cardID].chipName;
		this.elem.sprite = ChipData.elems[ChipDatabase.chipDB[ds.cardID].elem];
		this.cost.text = ""+ ChipDatabase.chipDB[ds.cardID].base_cost;

		// TODO: multi-copy handling
		numCopies = 1;
		this.colorcodes[0] = ds.color_code;
		this.colorCode_disp[0].color = ChipData.color_codes[ds.color_code];
		this.colorCode_disp[0].gameObject.SetActive(true);
	}

	public void add_copy(DeckSlot ds) {
		numCopies++;
		this.colorcodes[numCopies-1] = ds.color_code;
		this.colorCode_disp[numCopies-1].color = ChipData.color_codes[ds.color_code];
		this.colorCode_disp[numCopies-1].gameObject.SetActive(true);
	}

	public void remove() {
		if(numCopies > 1) {	// reduce size, remove dot, remove record of colorcode
			numCopies--;
			colorCode_disp[numCopies].gameObject.SetActive(false);
			colorcodes[numCopies] = null;
			deckbuilder.deck_count();
		}
		else {	// pass removal handling to deckbuilder to destroy folder slot instance
			deckbuilder.remove(this);
		}
	}
}
