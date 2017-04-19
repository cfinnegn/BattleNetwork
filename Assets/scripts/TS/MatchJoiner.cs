using UnityEngine;
using UnityEngine.UI;

// attached to each room on join list to do a join if the players hit the button
public class MatchJoiner : MonoBehaviour {

	public Text btnText;

	private string roomName;

	public void Join () {
		PhotonNetwork.JoinRoom(this.roomName);
	}

	public void UpdateRoom(RoomInfo room) {
		this.roomName = room.name;
		if(room.IsOpen)
			btnText.text = "("+room.PlayerCount +"/"+ room.MaxPlayers+") " + room.Name;
		if (!room.IsOpen) {
			btnText.text = "("+room.PlayerCount +"/"+ room.MaxPlayers+") " + room.Name + "  [In Progress]";
			var colors = GetComponent<Button> ().colors;
			colors.normalColor = Color.gray;
			colors.highlightedColor = Color.gray;
			GetComponent<Button> ().colors = colors;
		}
	}

}