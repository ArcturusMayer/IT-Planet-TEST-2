using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class HPLose : MonoBehaviour
{
    private Color fade;
    private PhotonView photonView;
    public int coolDown = 500;

    // Start is called before the first frame update
    void Start()
    {
        photonView = gameObject.GetComponent<PhotonView>();
        fade = gameObject.GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Renderer>().material.color = fade;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameObject.GetComponentInParent<HPSystem>().loseHP();
            new Thread(doFade).Start();
        }
    }

    private void doFade()
    {
        fade = new Color(fade.r, fade.g, fade.b, 0.5f);
        Thread.Sleep(coolDown);
        fade = new Color(fade.r, fade.g, fade.b, 1.0f);
    }
}
