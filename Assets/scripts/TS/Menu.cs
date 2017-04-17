using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon;
using UnityEngine.UI;
using TrueSync;
using UnityEngine.SceneManagement;

public enum PanelType { Info, Main, Nick, Match, Multiplayer, Options, Replay};

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

	// Connects to photon
	void Start () {
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

		ActivePanel (PanelType.Nick);
	}

	void Update() {
		if (chatPanel.activeSelf && Input.GetKeyDown(KeyCode.Return)) {
			MultiplayerPanel_ChatSend ();
		}

		if (toStart) {
			countDown += Time.deltaTime;

			startCountdown.text = string.Format("Jacking in: {0}...", TIME_TO_START_MATCH - Mathf.FloorToInt(countDown));

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
        PhotonNetwork.ConnectUsingSettings("v1.0");

        ReplayUtils.replayContext = lobbyName;

        this.infoText.text = "Connecting...";
		ActivePanel (PanelType.Info);
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
		PhotonNetwork.LeaveRoom ();
		ActivePanel (PanelType.Match);
	}

	// start a match and send the same command to all other players
	public void MultiplayerPanel_StartMatchBtn() {
        PhotonNetwork.room.IsVisible = false;

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

    public override void OnPhotonPlayerDisconnected(PhotonPlayer disconnetedPlayer) {        
        UpdatePlayerList();
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

			Text playerNameText = playerBox.FindChild("PlayerNameText").GetComponent<Text>();
            playerNameText.text = PhotonNetwork.playerList[index].NickName.Trim();
        }

	}

	private void ClearPlayersGUI() {
		foreach (Transform playerBox in playerBoxes) {
            playerBox.GetComponent<Image>().enabled = false;
            playerBox.FindChild("PlayerNameText").GetComponent<Text>().text = "";
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

        ReplayRecord.replayMode = ReplayMode.NO_REPLAY;

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
    }

	public void Online_Ready_clicked() {
		ready = !ready; // toggle readiness
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
		photonView.RPC("Update_opponent_ready", PhotonTargets.Others, ready);
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

}