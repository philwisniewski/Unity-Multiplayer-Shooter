using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;
using System;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] GameObject playerListItemPrefab;
    [SerializeField] GameObject startGameButton;
    [SerializeField] GameObject changeGameButton;
    [SerializeField] TMP_Text gameTypeText;

    public string[] gameModeOptions;
    private int gameTypeIndex = 0;

    public int pointsToWin = 3;


    private void Awake() {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();

    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("title");
        Debug.Log("Joined Lobby");
    }

    public void CreateRoom() {
        if (string.IsNullOrEmpty(roomNameInputField.text)) {
            return;
        }

        RoomOptions ropts = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = 8 };
        ExitGames.Client.Photon.Hashtable roomProps = new ExitGames.Client.Photon.Hashtable();
        roomProps.Add("gameType", gameModeOptions[0]);
        roomProps.Add("pointsToWin", pointsToWin);
        gameTypeIndex = 0;
        ropts.CustomRoomProperties = roomProps;

        PhotonNetwork.CreateRoom(roomNameInputField.text, ropts);
        MenuManager.Instance.OpenMenu("loading");
        

    }

    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        gameTypeText.text = (string) PhotonNetwork.CurrentRoom.CustomProperties["gameType"];


        Player[] players = PhotonNetwork.PlayerList;

        foreach(Transform child in playerListContent) {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Count(); i++) {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        changeGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation failed: " + message;
        MenuManager.Instance.OpenMenu("Error");
        
    }

    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");

    }

    public void JoinRoom(RoomInfo info) {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("title");
    }

    public void StartGame() {
        PhotonNetwork.LoadLevel(2);
    }

    public void ChangeGameType() {
        gameTypeIndex++;
        if (gameTypeIndex >= gameModeOptions.Length) {
            gameTypeIndex = 0;
        }
        PhotonNetwork.CurrentRoom.CustomProperties["gameType"] = gameModeOptions[gameTypeIndex];
    }

   private void Update() {
    try {
        gameTypeText.text = (string) PhotonNetwork.CurrentRoom.CustomProperties["gameType"];
    } catch (Exception e) {
        // sloppy code but thats okay :)
    }
   }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in roomListContent) {
            Destroy(trans.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++) {
            if (roomList[i].RemovedFromList) {
                continue;
            }
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);

        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    

}
