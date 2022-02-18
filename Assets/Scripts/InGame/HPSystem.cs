using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class HPSystem : MonoBehaviour
{
    private int playersHP = 50;
    private PhotonView photonView;
    private bool isCoolDown = false;
    private Color fade;

    public Text hptext;
    public GameObject hp;
    public AudioSource loseHpSound;
    public int coolDown = 500;

    // Start is called before the first frame update
    void Start()
    {
        photonView = gameObject.GetComponent<PhotonView>();
        Text[] buffer = FindObjectsOfType<Text>();
        foreach (Text txt in buffer)
        {
            if (txt.tag == "HPText")
            {
                hptext = txt;
            }
        }
        hp = GameObject.FindGameObjectWithTag("HP");
        fade = gameObject.GetComponentInChildren<Renderer>().material.color;
        loseHpSound = GameObject.FindGameObjectWithTag("LoseHPSound").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            if (playersHP < 1)
            {
                PhotonNetwork.Disconnect();
                gameObject.SetActive(false);
                PhotonNetwork.LoadLevel("StartUI");
            }
            hptext.text = playersHP.ToString();
        }
        gameObject.GetComponentInChildren<Renderer>().material.color = fade;
    }

    [PunRPC]
    public void loseHP()
    {
        if (photonView.IsMine && !isCoolDown)
        {
            playersHP -= 1;
            loseHpSound.Play();
            isCoolDown = true;
        }
        new Thread(damageCooldown).Start();
    }

    private void damageCooldown()
    {
        fade = new Color(fade.r, fade.g, fade.b, 0.5f);
        Thread.Sleep(coolDown);
        isCoolDown = false;
        fade = new Color(fade.r, fade.g, fade.b, 1.0f);
    }
}
