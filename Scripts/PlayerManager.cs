using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    PhotonView PV;

    GameObject controller;

    int kills;
    int deaths;


    private void Awake() {
        PV = GetComponent<PhotonView>();
    }


    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();

        if (PV.IsMine) {
            CreateController();
            print("Points needed to win: " + (int) PhotonNetwork.CurrentRoom.CustomProperties["pointsToWin"]);
        }
    }




    void CreateController() {
        Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });

    }

    public void Die() {
        PhotonNetwork.Destroy(controller);

        CreateController();

        deaths++;

        Hashtable hash = new Hashtable();
        hash.Add("deaths", deaths);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    [PunRPC]
    public void GameOver()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        print(PV.ViewID + " received end game");
        FindObjectOfType<GameManager>().EndGame();
    }

    public void GetKill() {
        PV.RPC(nameof(RPC_GetKill), PV.Owner);
    }

    [PunRPC]
    void RPC_GetKill()
    {
        kills++;

        Hashtable hash = new Hashtable();
        hash.Add("kills", kills);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        print(kills);
        print((int)PhotonNetwork.CurrentRoom.CustomProperties["pointsToWin"]);
        if (kills >= (int) PhotonNetwork.CurrentRoom.CustomProperties["pointsToWin"])
        {
            PV.RPC(nameof(GameOver), RpcTarget.All);
        }
    }

    public static PlayerManager Find(Player player) {
        return FindObjectsOfType<PlayerManager>().SingleOrDefault(x => x.PV.Owner == player);
    }
}
