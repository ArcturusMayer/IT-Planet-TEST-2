using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Connect : MonoBehaviourPunCallbacks
{
    public Text playerName;
    public Text roomName;
    public Text serverID;
    public InputField playerInput;
    public InputField roomInput;
    public InputField serverInput;
    public GameObject connect;
    public GameObject guideButton;
    public GameObject guideText;

    // Start is called before the first frame update
    void Start()
    {
        guideText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClick()
    {
        connect.SetActive(false);
        PhotonNetwork.GameVersion = "1.0";
        if (serverID.text == "" || serverID.text == null)
        {
            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = "8e4db021-8bb8-41d3-a24d-eefc9b30865b";
            if (playerName.text != "" && roomName.text != "" && playerName.text != null && roomName.text != null)
            {
                if (!PhotonNetwork.IsConnected)
                {
                    PhotonNetwork.ConnectUsingSettings();
                }
                else
                {
                    OnConnectedToMaster();
                }
            }
            else
            {
                connect.SetActive(true);
            }
        }
        else
        {
            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = serverID.text.ToString();
            if (playerName.text != "" && roomName.text != "" && playerName.text != null && roomName.text != null)
            {
                if (!PhotonNetwork.IsConnected)
                {
                    PhotonNetwork.ConnectUsingSettings();
                }
                else
                {
                    OnConnectedToMaster();
                }
            }
            else
            {
                connect.SetActive(true);
            }
        }
    }

    public void onGuideClick(){
        if(!guideText.active){
            guideText.SetActive(true);
            guideButton.GetComponentInChildren<Text>().text = "Hide guide";
        }else{
            guideText.SetActive(false);
            guideButton.GetComponentInChildren<Text>().text = "Show guide";
        }
    }
            

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.LocalPlayer.NickName = playerName.text;
        Photon.Realtime.RoomOptions roomOptions = new Photon.Realtime.RoomOptions();
        roomOptions.MaxPlayers = 16;
        roomOptions.IsOpen = true;
        PhotonNetwork.JoinOrCreateRoom(roomName.text, roomOptions, new Photon.Realtime.TypedLobby(roomName.text, Photon.Realtime.LobbyType.Default));
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("SampleScene");
        if(PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            PlayerPrefs.SetString("await", "victory");
        }
        else
        {
            PlayerPrefs.SetString("await", "await");
        }
    }

    private void OnFailedToConnect()
    {
        connect.SetActive(true);
    }
}
