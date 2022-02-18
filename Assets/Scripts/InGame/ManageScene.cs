using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


//public class GameClass : IConnectionCallbacks {

//}

public class ManageScene : MonoBehaviourPunCallbacks
{
    public Joystick joystick;
    public Text awaiting;

    private GameObject player;
    private string awaitFlag;
    private bool won = false;

    // Start is called before the first frame update
    void Start()
    {
        joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<Joystick>();

        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("StartUI");
        }

        player = PhotonNetwork.Instantiate("Player", new Vector3(0,0,0.1f), Quaternion.identity, 0);
        joystick.isJoined = true;
        player.GetComponentInChildren<ShadowMove>().isJoined = true;
        //player.GetComponent<PlayerController>().nickname = PhotonNetwork.CurrentRoom.Players[PhotonNetwork.CurrentRoom.Players.Count - 1].NickName;

        awaitFlag = PlayerPrefs.GetString("await");
    }

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.Home) || Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Menu)){
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("StartUI");
        }
        foreach (GameObject plr in GameObject.FindGameObjectsWithTag("Player"))
        {
            foreach (KeyValuePair<int, Photon.Realtime.Player> photonPlr in PhotonNetwork.CurrentRoom.Players)
            {
                if (plr.GetComponent<PhotonView>().OwnerActorNr == photonPlr.Value.ActorNumber)
                {
                    plr.GetComponent<PlayerController>().nickname = photonPlr.Value.NickName;
                }
            }
        }
        if (PhotonNetwork.CurrentRoom.Players.Count > 1)
        {
            awaiting.text = "";
            awaitFlag = "victory";
        }
        else
        {
            if(awaitFlag == "await")
            {
                awaiting.text = "Awaitng for players";
            }
            else
            {
                if(awaitFlag == "victory")
                {
                    awaiting.text = "You won!";
                    new Thread(victoryLetters).Start();
                }
            }
        }
        if (won)
        {
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("StartUI");
        }
    }

    public void victoryLetters()
    {
        Thread.Sleep(1500);
        won = true;
    }
}
