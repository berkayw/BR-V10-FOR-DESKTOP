using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStartManager : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI readyButtonText;

    private bool isReady = false;
    private int readyPlayerCount = 0;
    

    public void OnReadyButtonClick()
    {
        Debug.Log("Butona Basıldı.");
        isReady = !isReady;
        photonView.RPC("UpdateReadyState", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber, isReady);
        readyButtonText.text = isReady ? "Unready" : "Ready";
    }

    [PunRPC]
    private void UpdateReadyState(int playerNumber, bool ready)
    {
        if (ready)
        {
            readyPlayerCount++;
        }
        else
        {
            readyPlayerCount--;
        }

        CheckAllPlayersReady();
    }

    private void CheckAllPlayersReady()
    {
        if (readyPlayerCount == PhotonNetwork.PlayerList.Length)
        {
            photonView.RPC("StartGame", RpcTarget.All);
        }
    }

    [PunRPC]
    private void StartGame()
    {
        // Tüm oyuncular hazır olduğunda oyunu başlatmak için sahneyi yükle
        Debug.Log("All players are ready. Starting the game...");
        PhotonNetwork.LoadLevel(1); // Oyun sahnenizin ismini burada belirtin
    }
}