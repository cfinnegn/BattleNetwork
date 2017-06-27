using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon;
using UnityEngine.UI;
using TrueSync;
using UnityEngine.SceneManagement;

public enum PanelType { Info, Main, Nick, Match, Multiplayer, Options, Replay, Tutorial};

// Connects to Photon Cloud, manages GUI to create/join game rooms.
public class Menu : PunBehaviour {

    private const int TIME_TO_START_MATCH = 3;

    public Text infoText;
	public GameObject background;

	[Header("Main Panel")]
	public GameObject mainPanel;

	[Header("Nick Panel")]
	public GameObject nickPanel;
	public Text nickWelcomeText;
	public InputField nickInput;

	[Header("Match Panel")]
	public GameObject matchPanel;
	public Text matchJoinText;
	public RectTransform matchListContent;
	public GameObject matchPrefabBtn;

	[Header("Multiplayer Panel")]
	public GameObject multiplayerPanel;
    public Transform[] playerBoxes;
	public Transform[] naviBoxes;
	public int selectedNavi;
	public Button[] readyButtons;
	public bool ready = false;
    public Button multiplayerStartMatch;
	public Text startCountdown;
	public GameObject chatPanel;
	public Text chatText;
	public InputField chatInput;
	private ScrollRect chatScroll;
    public GameObject configPanel;
    public GameObject configBtn;
    public InputField configSyncWindow;
    public InputField configPanicWindow;
    public InputField configRollbackWindow;

	[Header("Tutorial Panel")]
	public GameObject tutorialPanel;
	public Image tutorialimage;
	public Text tutorialheader;
	public Text tutorialtext;
	public Button tutorial_next;
	public Button tutorial_prev;
	public Button tutindex_game;
	public Button tutindex_move;
	public Button tutindex_buster;
	public Button tutindex_navpower;
	public Button tutindex_chips;
	public Button tutindex_combo;
	public Button tutindex_terrain;
	public Color default_color;
	public Color topic_color;

	[Header("Tutorial Images")]
	public Sprite playerinfo_tut;
	public Sprite screen_tut;
	public Sprite movement_tut;
	public Sprite buster_tut;
	public Sprite NPactive_tut;
	public Sprite weaponget_tut;
	public Sprite chip_tut;
	public Sprite barrier1_tut;
	public Sprite barrier2_tut;
	public Sprite combo_tut;
	public Sprite terrain_tut;

	protected Button[] tut_topics;
	protected int current_topic = 0;
	protected string[][] text_array;
	public Sprite[][] images;
	protected int current_page = 0;

	[Header("Replay Panel")]
    public GameObject replayPanel;
    public Text replayInfoText;
    public RectTransform replayListContent;
    public GameObject replayPrefabBtn;

    private string nickname;

	private bool toStart;
	private float countDown = 0;

    private string lobbyName;
    private string levelToLoad;

	public static Menu instance;

	public float[] playerPing;
	public float sendPingTimer = 0f;
	public bool display_ping;

	void Awake(){
		playerPing = new float[9];
	}
	// Connects to photon
	void Start () {
		if(PlayerPrefs.HasKey("Nick"))
			nickInput.text = PlayerPrefs.GetString ("Nick");
        instance = this;
        PhotonNetwork.CrcCheckEnabled = true;
        ReplayUtils.Init();

        this.chatScroll = this.chatPanel.transform.Find ("ChatScroll").GetComponent<ScrollRect> ();

		// Checks if it is already connected
		if (PhotonNetwork.connected) {
			OnReceivedRoomListUpdate ();
			ActivePanel (PanelType.Match);
			return;
		}

		// tutorial data initialization
		tut_topics = new Button[] { tutindex_game, tutindex_move, tutindex_buster, tutindex_navpower, tutindex_chips, tutindex_combo, tutindex_terrain };
		text_array = new string[tut_topics.Length][];
		text_array[0] = new string[3];	// game screen
		text_array[0][0] = "This is the main battle screen. 2 players do battle on this 6x3 grid and attempt to reduce their opponent's HP to 0 using a variety of attacks and abilities.";
		text_array[0][1] = "This display (found at the top of the screen) presents a lot of information about a player's character. The image represents what character the player is using, their remaining HP is displayed in front of the character's face, and the number behind the character's face shows how many chips they are holding.";
		text_array[0][2] = "The bar at the bottom of this display is the 'Custom Gauge' which shows how much energy is available for activating chips and abilities.";
		text_array[1] = new string[2];  // movement
		text_array[1][0] = "Characters can only move within their own territory (designated by blue and red borders), and cannot move across broken panels (unless aided by certain abilites).";
		text_array[1][1] = "The movement pad (in the bottom left corner of the screen) may be set to 'tap' or 'swipe'. Swipe in one of 4 directions, or tap 1 of 4 directional buttons to move your character.";
		text_array[2] = new string[4]; // buster
		text_array[2][0] = "Every character has a buster which can be fired by tapping the button in the bottom right corner of the screen. The buster fires a straight shot for 1 damage, but can be upgraded by some abilites.";
		text_array[2][1] = "The buster can be charged up to 3 levels. The ring around the buster button will fill as you approach full charge, and up to 3 star icons will appear above it to show your current level of charge.";
		text_array[2][2] = "A spinning ball of energy will appear over a character that is charging their buster, and it will change color to reflect the level of charge that character has reached.";
		text_array[2][3] = "Each character has their own unique charged buster attacks. Megaman's fires just like a normal buster shot, but does higher damage and staggers opponents (*Megaman is currently the only playable character).";
		text_array[3] = new string[3];	// Navi Power
		text_array[3][0] = "The button above the Buster Button activates a character's unique 'Navi Power'. This power often comes at the cost of some energy from your 'Custom Gauge'.";
		text_array[3][1] = "Megaman's ability 'Weapon Get' will be unusable at the start of a game and appear as 'No Data' until your opponent activates a Battlechip.";
		text_array[3][2] = "Once your opponent uses a Battlechip, Megaman will be able to spend 3 Custom Energy to activate 'Weapon Get', adding a copy of the last Battlechip used by your opponent to your hand with a white chip code (more on color codes in 'Chip Combos').";
		text_array[4] = new string[5]; //battlechips
		text_array[4][0] = "Along the bottom of your screen is your hand of Battlechips. Battlechips provide access to a wide variety of attacks and effects in exchange for Custom energy. You start each game with 3 chips, but can draw more at any time with the 'Add Chip' button for 2 energy.";
		text_array[4][1] = "Battlechips can be recognized by their image and name. The icon in the top right shows how much Custom Energy a chip needs to activate, as well as its color code (more on color codes and what they mean in the 'Chip Combos' section).";
		text_array[4][2] = "At the bottom left of a Battlechip you can see its element and a number representing its power. This number shows how much damage or healing a chip will do, or sometimes represents the potency or duration of its effect.";
		text_array[4][3] = "You can activate any chip from your hand by clicking on it, so long as you have enough energy in your Custom Gauge.";
		text_array[4][4] = "Some chips (Such as Barrier), apply a temporary effect to your character. The 'Active Chip Display' in the top corners of the screen show when a chip's effect is active, and how much longer it will last. Activating another chip with a lasting effect will overwrite any currently active chips displayed here.";
		text_array[5] = new string[5]; //chip combos
		text_array[5][0] = "The cost display on a Battlechip can be 1 of 8 different colors, as well as white and grey. This color represents the chip's color code. When chips with the same color code are activated consecutively, you activate a color combo.";
		text_array[5][1] = "Color combos reduce the activation cost of same-colored chips in your hand. If you activate a Battlechip, then follow it up with another chip of the same color, the second chip will cost 1 less than it normally would.";
		text_array[5][2] = "You can recognize that a color combo is active when a Battlechip's cost icon has bold outline. Color combos can be extended by activating more chips of the same color. When you activate a chip of another color, your old combo ends, and a new one can start.";
		text_array[5][3] = "Chips with white and grey color codes interact with combos in special ways. White code chips cannot combo with each other, but can be activated in the middle of another color combo without interrupting or resetting it.";
		text_array[5][4] = "Grey code chips also cannot combo with each other, but they will interrupt active color combos. The 'Add Chip' button, as well as some Navi Powers will also interrupt color combos.";
		text_array[6] = new string[3]; //terrain
		text_array[6][0] = "At the start of every game, all 18 tiles that make up the playing field will be plain, but a variety of effects can be applied to these tiles through the use of certain abilities.";
		text_array[6][1] = "You cannot stand on broken tiles, and certain attacks will be stopped by them. If you step on a cracked tile, it will break when you step off of it. Broken tiles automatically repair themselves after a short time.";
		text_array[6][2] = "Standing on a volcano, water, or grass tile will affect the power of certain elemental attacks that you use and that hit you. Standing on a poison tile will slowly damage you, ice tiles will cause you to continue sliding when you step on one, and sanctuary tiles will slowly heal you.";

		ActivePanel(PanelType.Nick);
	}

	void Update() {
		// Update players' pings in room
		if (PhotonNetwork.connected) {
			if (PhotonNetwork.player.ID > 0) {
				sendPingTimer -= Time.deltaTime;
				float myPing = PhotonNetwork.GetPing ();
				if (sendPingTimer <= 0) {
					ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable ();
					playerProps.Add("hisPing",myPing);
					PhotonNetwork.player.SetCustomProperties(playerProps);
					sendPingTimer = 2f;
				}
				UpdatePlayerList ();
			}
		}

		if (chatPanel.activeSelf && Input.GetKeyDown(KeyCode.Return)) {
			MultiplayerPanel_ChatSend ();
		}

		if (toStart) {
			countDown += Time.deltaTime;

			startCountdown.text = string.Format("Jacking in: {0}...", TIME_TO_START_MATCH - Mathf.FloorToInt(countDown));

			if (PhotonNetwork.lobby.Name == "battleB") // KEEP THIS UP TO DATE WITH NEW LOBBIES/SCENES!
				levelToLoad = "Scenes/battleB";
			if (countDown >= TIME_TO_START_MATCH) {
				PhotonNetwork.LoadLevel (this.levelToLoad);
				toStart = false;
			}
		}
	}

	// aftert the user put his username and hit ok the main planel is shown
	public void NickPanel_OkBtn() {
		this.nickname = nickInput.text;
		this.nickWelcomeText.text = string.Format ("Welcome {0}", this.nickname);

		this.nickPanel.SetActive (false);
		this.mainPanel.SetActive (true);
		PlayerPrefs.SetString ("Nick", this.nickname);
		PlayerPrefs.Save ();
	}

    public void MainPanel_SetLobby(string config) {
        string[] configSplitted = config.Split(';');
        this.lobbyName = configSplitted[0];
        this.levelToLoad = configSplitted[1];

        MainPanel_MultilayerBtn();
    }

	// show multiplayer menu, with options to create or join a match
	public void MainPanel_MultilayerBtn() {
		PhotonNetwork.player.NickName = this.nickname;
        PhotonNetwork.lobby = new TypedLobby(lobbyName, LobbyType.Default);

		// #######################	Game Version set for connecting here ##############################
		// #######################	Game Version set for connecting here ##############################
		PhotonNetwork.ConnectUsingSettings("v1.0");
		// #######################	Game Version set for connecting here ##############################
		// #######################	Game Version set for connecting here ##############################

		ReplayUtils.replayContext = lobbyName;

        this.infoText.text = "Connecting...";
		ActivePanel (PanelType.Info);
	}

	public void MainPanel_TutorialBtn() {
		current_topic = 1;
		Tutorial_set_topic(0);

		ActivePanel(PanelType.Tutorial);
	}

	// Method called when a tutorial topic button is pressed to jump to a topic
	public void Tutorial_set_topic(int t) {
		if(current_topic != t) {
			current_topic = t;
			current_page = 0;

			tutorialheader.text = tut_topics[t].GetComponentInChildren<Text>().text;
			tutorialtext.text = text_array[current_topic][current_page];
			int i = 0;
			while(i < tut_topics.Length) {
				ColorBlock cb = tut_topics[i].colors;
				cb.normalColor = (i == t) ? topic_color : default_color;
				tut_topics[i].colors = cb;
				i++;
			}
		}
		Tutorial_pagecheck();
	}

	public void Tutorial_page(int i) {
		current_page += i;
		if(current_page >= text_array[current_topic].Length) {
			Tutorial_set_topic(current_topic + 1);
		}
		else if(current_page < 0) {
			Tutorial_set_topic(current_topic - 1);
			current_page = text_array[current_topic].Length - 1;
			tutorialtext.text = text_array[current_topic][current_page];
		}
		else {
			tutorialtext.text = text_array[current_topic][current_page];
		}
		Tutorial_pagecheck();
	}

	//enables/disables next/prev buttons based on page/topic
	public void Tutorial_pagecheck() {
		tutorial_next.interactable = ((current_topic < tut_topics.Length-1) || (current_page < text_array[current_topic].Length-1));
		tutorial_prev.interactable = ((current_topic > 0) || (current_page > 0));

		// display correct image
		if(current_topic == 0) {    // info
			if(current_page == 0) { tutorialimage.sprite = screen_tut; }
			else { tutorialimage.sprite = playerinfo_tut; }
		}
		else if(current_topic == 1) { // movement
			if(current_page == 0) { tutorialimage.sprite = screen_tut; }
			else { tutorialimage.sprite = movement_tut; }
		}
		else if(current_topic == 2) { // buster
			if(current_page == 0) { tutorialimage.sprite = screen_tut; }
			else { tutorialimage.sprite = buster_tut; }
		}
		else if(current_topic == 3) { // navi power
			if(current_page == 0) { tutorialimage.sprite = screen_tut; }
			else if(current_page == 1) { tutorialimage.sprite = NPactive_tut; }
			else { tutorialimage.sprite = weaponget_tut; }
		}
		else if(current_topic == 4) { // battlechips
			if(current_page == 0) { tutorialimage.sprite = screen_tut; }
			else if(current_page == 1 || current_page == 2) { tutorialimage.sprite = chip_tut; }
			else if(current_page == 3) { tutorialimage.sprite = barrier1_tut; }
			else { tutorialimage.sprite = barrier2_tut; }
		}
		else if(current_topic == 5) { tutorialimage.sprite = combo_tut; }
		else { tutorialimage.sprite = terrain_tut; }
	}


	public void MainPanel_ExitBtn() {
		Application.Quit ();
	}

	// go back from main menu to username menu
	public void MainPanel_BackBtn() {
		ActivePanel (PanelType.Nick);
	}

	// go back from a match menu to main menu
	public void MatchPanel_BackBtn() {
		PhotonNetwork.Disconnect ();
		ActivePanel (PanelType.Main);
	}

	// create a new match and go to match menu
	public void MatchPanel_NewMatchBtn() {
		RoomOptions roomOptions = new RoomOptions();
		roomOptions.MaxPlayers = 2;
		PhotonNetwork.CreateRoom(this.nickname, roomOptions, PhotonNetwork.lobby);

		infoText.text = "Creating match... " + lobbyName;
		ActivePanel (PanelType.Info);
	}

    public void MatchPanel_ReplayBtn() {
        List<ReplayRecordInfo> replayRecords = ReplayUtils.GetContextRecords();
        if (replayRecords.Count == 0) {
            replayInfoText.text = "No Replays";
        } else {
            replayInfoText.text = "Replays: " + replayRecords.Count;
        }

        foreach (Transform child in this.replayListContent) {
            Destroy(child.gameObject);
        }
                
        for (int index = 0; index < replayRecords.Count; index++) {
            ReplayRecordInfo replayRecord = replayRecords[index];

            GameObject newReplayBtn = Instantiate(replayPrefabBtn);
            newReplayBtn.transform.SetParent(this.replayListContent, false);

			//Display info about the replay
            newReplayBtn.transform.Find("DateText").GetComponent<Text>().text = replayRecord.creationDate.ToString("yyyy-MM-dd");
            newReplayBtn.transform.Find("TimeText").GetComponent<Text>().text = replayRecord.creationDate.ToString("HH:mm");
            newReplayBtn.transform.Find("PlayersText").GetComponent<Text>().text = "Players: " + replayRecord.numberOfPlayers;

            newReplayBtn.GetComponent<ReplayPicker>().replayRecord = replayRecord;

            RectTransform newReplayBtnRect = newReplayBtn.transform as RectTransform;
            newReplayBtnRect.localPosition = new Vector3((index % 3) * (newReplayBtnRect.sizeDelta.x + 10), -((index / 3) * (newReplayBtnRect.sizeDelta.y + 10)), 0);
        }

        this.replayListContent.sizeDelta = new Vector2(this.replayListContent.sizeDelta.x, ((replayRecords.Count-1) / 3 + 1) * ( replayPrefabBtn.GetComponent<RectTransform>().sizeDelta.y + 10));

        ActivePanel(PanelType.Replay);
    }

	public void MultiplayerPanel_BackBtn() {
		//Online_Ready_clicked (false);
		PhotonNetwork.LeaveRoom ();
		ActivePanel (PanelType.Match);
	}

	// start a match and send the same command to all other players
	public void MultiplayerPanel_StartMatchBtn() {
		//PhotonNetwork.room.IsVisible = false; // Makes the room not show from the lobby listing
		PhotonNetwork.room.IsOpen = false; // Closes the room, but still shows in lobby. How a closed room appears is modified in MatchJoiner.cs

        int syncWindow = int.Parse(configSyncWindow.text);
        int rollbackWindow = int.Parse(configRollbackWindow.text);
        int panicWindow = int.Parse(configPanicWindow.text);

        TrueSyncConfig globalConfig = TrueSyncManager.TrueSyncGlobalConfig;

        if (!(syncWindow != globalConfig.syncWindow || rollbackWindow != globalConfig.rollbackWindow || panicWindow != globalConfig.panicWindow)) {
            syncWindow = -1;
        }

        photonView.RPC ("StartMatch", PhotonTargets.All, syncWindow, rollbackWindow, panicWindow);
	}

	public void MultiplayerPanel_ChatSend() {
		string text = this.chatInput.text;
		if (text != "") {
			this.chatInput.text = "";

            int indexPlayer = System.Array.IndexOf(PhotonNetwork.playerList, PhotonNetwork.player);
            MultiplayerPanel_ChatReceived (PhotonNetwork.playerName, text, indexPlayer);
			photonView.RPC ("MultiplayerPanel_ChatReceived", PhotonTargets.Others, PhotonNetwork.playerName, text, indexPlayer);

			this.chatInput.ActivateInputField ();
		}
	}

	[PunRPC]
	public void MultiplayerPanel_ChatReceived(string playerName, string text, int spawnIndex) {
		if (spawnIndex < 0) {
			spawnIndex = 0;
		}
		this.chatText.text += string.Format("{0}: {1}\n", playerName, text);
		this.chatScroll.normalizedPosition = new Vector2(0, 0);
	}

	public void MultiplayerPanel_ChatToggle() {
        configPanel.SetActive(false);
        chatPanel.SetActive (!chatPanel.activeSelf);

		if (chatPanel.activeSelf) {
			this.chatInput.ActivateInputField ();
			this.chatScroll.normalizedPosition = new Vector2(0, 0);
		}
	}

    public void MultiplayerPanel_ConfigToggle() {
        chatPanel.SetActive(false);
        configPanel.SetActive(!configPanel.activeSelf);
    }

    public void OptionsPanel_BackBtn() {
		ActivePanel (PanelType.Main);
	}    

    public void ReplayPanel_BackBtn() {
        ActivePanel(PanelType.Match);
    }

    public void ReplayPanel_ClearBtn() {
        ReplayUtils.ClearAllReplayRecords();
        MatchPanel_ReplayBtn();
    }

    public void ReplayPanel_LoadLevel() {
        SceneManager.LoadScene(levelToLoad);
    }

    public override void OnConnectionFail (DisconnectCause cause) {
		ActivePanel (PanelType.Main);
	}
		
    public override void OnConnectedToMaster() {
		infoText.text = "Entering lobby...";
        PhotonNetwork.JoinLobby(PhotonNetwork.lobby);
    }

	// updates the possible matches list
	public override void OnReceivedRoomListUpdate () {
		RoomInfo[] roomList = PhotonNetwork.GetRoomList ();
		if (roomList.Length == 0) {
			matchJoinText.text = "No Matches Online";
		} else {
			matchJoinText.text = "Join Match";
		}

		int currentMatchesCount = this.matchListContent.transform.childCount;

		if (roomList.Length >= currentMatchesCount) {
			for (int index = 0; index < (roomList.Length - currentMatchesCount); index++) {
				GameObject newMatchBtn = Instantiate (matchPrefabBtn);
				newMatchBtn.transform.SetParent(this.matchListContent, false);
			}
		} else {
			for (int index = 0; index < (currentMatchesCount - roomList.Length); index++) {
				Destroy(this.matchListContent.transform.GetChild (currentMatchesCount - (index + 1)).gameObject);
			}
		}

		for (int index = 0; index < roomList.Length; index++) {
			MatchJoiner matchJoiner = this.matchListContent.transform.GetChild (index).GetComponent<MatchJoiner>();
			matchJoiner.UpdateRoom (roomList[index]);

			RectTransform matchJointRect = matchJoiner.GetComponent<RectTransform> ();
			matchJointRect.localPosition = new Vector3 (matchJointRect.localPosition.x, -(index * matchJointRect.sizeDelta.y), 0);
		}

		this.matchListContent.sizeDelta = new Vector2 (this.matchListContent.sizeDelta.x, roomList.Length * matchPrefabBtn.GetComponent<RectTransform>().sizeDelta.y);
	}

	// When connected to Photon Lobby, disable nickname editing and messages text, enables room list
	public override void OnJoinedLobby () {
		ActivePanel (PanelType.Match);
	}

	// if we join (or create) a room, no need for the create button anymore;
	public override void OnJoinedRoom () {
		MultiplayerUpdateVisibility ();        
		ActivePanel (PanelType.Multiplayer);
		ExitGames.Client.Photon.Hashtable prematch_properties = new ExitGames.Client.Photon.Hashtable() { { "ready", false }, { "navi", 0 } };
		PhotonNetwork.player.SetCustomProperties(prematch_properties);
		UpdatePlayerList();
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer) {
		
		UpdatePlayerList();
    }

    public override void OnMasterClientSwitched(PhotonPlayer newMasterClient) {
		MultiplayerUpdateVisibility ();
    }

    private void MultiplayerUpdateVisibility() {
        this.configBtn.SetActive(PhotonNetwork.isMasterClient);
        if (this.configBtn.activeSelf) {
            configSyncWindow.text = TrueSyncManager.TrueSyncGlobalConfig.syncWindow + "";
            configRollbackWindow.text = TrueSyncManager.TrueSyncGlobalConfig.rollbackWindow + "";
            configPanicWindow.text = TrueSyncManager.TrueSyncGlobalConfig.panicWindow + "";
        }

        multiplayerStartMatch.gameObject.SetActive (PhotonNetwork.isMasterClient);
	}

    public override void OnPhotonPlayerDisconnected(PhotonPlayer disconnectedPlayer) {        
        UpdatePlayerList();
		Online_Ready_clicked (false);
		photonView.RPC("Update_opponent_ready", PhotonTargets.AllBuffered, false);
    }    
	public void OnLeftRoom(){
		Online_Ready_clicked (false);
	}

	// updates players position and plane on gui
	public void UpdatePlayerList() {
		ClearPlayersGUI ();

        for (int index = 0; index < PhotonNetwork.playerList.Length; index++) {
			int side;
			if(PhotonNetwork.playerList[index].IsLocal) { side = 0; }   // local player on left
			else { side = 1; }	// opponent on right
			Transform playerBox = playerBoxes[side];
            playerBox.GetComponent<Image>().enabled = true;
			naviBoxes[side].gameObject.SetActive(true);
			readyButtons[side].gameObject.SetActive(true);

			Text playerNameText = playerBox.Find("PlayerNameText").GetComponent<Text>();
			playerNameText.text = PhotonNetwork.playerList[index].NickName.Trim();
			if(display_ping) { playerNameText.text += "  :  (" + PhotonNetwork.playerList[index].CustomProperties["hisPing"] + ")"; } // Shows player name + ping
        }

	}

	private void ClearPlayersGUI() {
		foreach (Transform playerBox in playerBoxes) {
            playerBox.GetComponent<Image>().enabled = false;
            playerBox.Find("PlayerNameText").GetComponent<Text>().text = "";
		}
		foreach(Transform naviBox in naviBoxes) {
			naviBox.gameObject.SetActive(false);
		}
		foreach(Button readyButton in readyButtons) {
			readyButton.gameObject.SetActive(false);
		}
	}

	[PunRPC]
	public void StartMatch(int syncWindow, int rollbackWindow, int panicWindow) {
        if (syncWindow == -1) {
            TrueSyncManager.TrueSyncCustomConfig = null;
        } else {
            TrueSyncManager.TrueSyncCustomConfig = ScriptableObject.CreateInstance<TrueSyncConfig>();
            TrueSyncManager.TrueSyncCustomConfig.syncWindow = syncWindow;
            TrueSyncManager.TrueSyncCustomConfig.rollbackWindow = rollbackWindow;
            TrueSyncManager.TrueSyncCustomConfig.panicWindow = panicWindow;
        }

		ReplayRecord.replayMode = ReplayMode.RECORD_REPLAY;

        this.toStart = true;
		background.GetComponent<Animator>().Play("JackIn");
		background.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.8f);

		this.multiplayerStartMatch.gameObject.SetActive (false);
		this.startCountdown.gameObject.SetActive (true);
	}

	private void ActivePanel(PanelType panelType) {
		this.infoText.gameObject.SetActive (panelType == PanelType.Info ? true : false);

		this.mainPanel.SetActive (panelType == PanelType.Main ? true : false);
		this.nickPanel.SetActive (panelType == PanelType.Nick ? true : false);
		this.matchPanel.SetActive (panelType == PanelType.Match ? true : false);
		this.multiplayerPanel.SetActive (panelType == PanelType.Multiplayer ? true : false);
        this.replayPanel.SetActive(panelType == PanelType.Replay ? true : false);
		this.tutorialPanel.SetActive(panelType == PanelType.Tutorial ? true : false);
    }

	public void Online_Ready_clicked(bool boolean) {
		ready = !ready; // toggle readiness
		if (!boolean)
			ready = false;
		ExitGames.Client.Photon.Hashtable readyhash = new ExitGames.Client.Photon.Hashtable() { { "ready", (bool)ready }};
		PhotonNetwork.player.SetCustomProperties(readyhash);
		int indexPlayer = System.Array.IndexOf(PhotonNetwork.playerList, PhotonNetwork.player);

		int side = 0;	// local player always left
		Text buttonText = readyButtons[side].GetComponentsInChildren<Text>()[0];
		if(ready) {
			readyButtons[side].image.color = readyButtons[side].colors.pressedColor;
			buttonText.text = "Ready!";
			buttonText.fontStyle = FontStyle.BoldAndItalic;
			buttonText.color = new Color(0.85f, 0.55f, 0f);
			buttonText.GetComponent<Outline>().effectColor = Color.black;
		}
		else {    // for local player
			readyButtons[side].image.color = readyButtons[side].colors.normalColor;
			buttonText.text = "Ready?";
			buttonText.fontStyle = FontStyle.Bold;
			buttonText.color = Color.black;
			buttonText.GetComponent<Outline>().effectColor = readyButtons[side].colors.normalColor;
		}
		photonView.RPC("Update_opponent_ready", PhotonTargets.OthersBuffered, ready);
		Check_Match_Start();
	}

	[PunRPC]
	public void Update_opponent_ready(bool opp_ready) {   // side 0 for local, 1 for opponent									
		// add navi change when multiple playable navis implemented
		int side = 1;	// opponent always right
		Text buttonText = readyButtons[side].GetComponentsInChildren<Text>()[0];
		if(opp_ready) {
			readyButtons[side].image.color = readyButtons[side].colors.pressedColor;
			buttonText.text = "Ready!";
			buttonText.fontStyle = FontStyle.BoldAndItalic;
			buttonText.color = new Color(0.85f, 0.55f, 0f);
			buttonText.GetComponent<Outline>().effectColor = Color.black;
		}
		else{    // for opponent
			readyButtons[side].image.color = readyButtons[side].colors.disabledColor;
			buttonText.text = "Waiting...";
			buttonText.fontStyle = FontStyle.Bold;
			buttonText.color = Color.black;
			buttonText.GetComponent<Outline>().effectColor = readyButtons[side].colors.disabledColor;
		}
		Check_Match_Start();
	}

	public void Check_Match_Start() {
		if(PhotonNetwork.isMasterClient) {  // only master client sees match start button
			if((PhotonNetwork.playerList.Length >= 2) && ((bool)PhotonNetwork.playerList[0].CustomProperties["ready"])
			  && ((bool)PhotonNetwork.playerList[1].CustomProperties["ready"])) { // both players in and ready
				multiplayerStartMatch.interactable = true;
				multiplayerStartMatch.GetComponentsInChildren<Text>()[0].text = "Start Match";
			}
			else {
				multiplayerStartMatch.interactable = false;
				multiplayerStartMatch.GetComponentsInChildren<Text>()[0].text = "Players not ready...";
			}
		}
	}

	public void Enter_Training() {
		SceneManager.LoadScene(2, LoadSceneMode.Single);	// Battle_Training is 2 in build order
	}

	public void Enter_Library() {
		SceneManager.LoadScene(3, LoadSceneMode.Single);    // Chip Library is 3 in build order
	}

	public void Toggle_Display_Ping() {
		display_ping = !display_ping;
	}

}