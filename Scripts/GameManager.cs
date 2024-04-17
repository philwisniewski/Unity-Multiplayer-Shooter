using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    Transform gameOverCanvas;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            LeaveRoom();
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.LeaveRoom();
    }

    private void Start()
    {
        for (int i = 0; i < gameOverCanvas.childCount; i++)
        {
            gameOverCanvas.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void RestartRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.LoadLevel(2);
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(1);
    }

    public void EndGame()
    {
        gameOverCanvas.GetChild(0).gameObject.SetActive(true);
        gameOverCanvas.GetChild(1).gameObject.SetActive(true);
        if (PhotonNetwork.IsMasterClient)
        {
            gameOverCanvas.GetChild(2).gameObject.SetActive(true);
        }
    }

    /*
        public override void OnLeftRoom()
        {

            Debug.Log("Left room");

            base.OnLeftRoom();
            PhotonNetwork.LoadLevel(0);
        }
    */

}
