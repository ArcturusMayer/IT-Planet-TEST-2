using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ShadowMove : MonoBehaviour
{
    public GameObject parent;
    public GameObject icon;
    public Text timer;
    public bool isClock;
    public bool isJoined = false;
    public float speed = 5.0f;
    public int duration = 5000;
    public int coolDown = 5000;
    public Sprite greenZoneSkin;
    public AudioSource freezeSound;
    public AudioSource cooldownEndSound;

    private PhotonView photonView;
    private string timerText = "";
    private float firstClickTime = 0.0f;
    private float secondClickTime = 0.0f;
    private bool isFreeze = false;
    private bool isCooldown = false;
    private float startTime = 0.0f;
    private Color fade;

    // Start is called before the first frame update
    void Start()
    {
        icon = GameObject.FindGameObjectWithTag("Freeze");
        freezeSound = GameObject.FindGameObjectWithTag("FreezeSound").GetComponent<AudioSource>();
        cooldownEndSound = GameObject.FindGameObjectWithTag("CoolDownEndSound").GetComponent<AudioSource>();
        Text[] buffer = FindObjectsOfType<Text>();
        foreach (Text txt in buffer)
        {
            if (txt.tag == "FreezeText")
            {
                timer = txt;
            }
        }
        fade = icon.GetComponent<Renderer>().material.color;
        photonView = gameObject.GetComponentInParent<PhotonView>();

        if (photonView.IsMine)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = greenZoneSkin;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isJoined)
        {
            isJoined = false;
        }

        if (photonView.IsMine)
        {
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch t = Input.GetTouch(i);

                    if (t.phase == TouchPhase.Began)
                    {
                        secondClickTime = firstClickTime;
                        firstClickTime = Time.time;
                        if (firstClickTime - secondClickTime < 0.21f && secondClickTime != 0)
                        {
                            if (!isFreeze && !isCooldown)
                            {
                                freezeSound.Play();
                                isFreeze = true;
                                isCooldown = true;
                                startTime = Time.time;
                                secondClickTime = 0;
                                new Thread(doFreeze).Start();
                            }
                        }
                    }
                }
            }

            icon.GetComponent<Renderer>().material.color = fade;

            timer.text = timerText;

            if (!isFreeze)
            {
                if (!isClock)
                {
                    transform.Rotate(Vector3.forward, speed);
                }
                else
                {
                    transform.Rotate(Vector3.back, speed);
                }

                if (isCooldown)
                {
                    timerText = System.Math.Round(10.0f - (Time.time - startTime), 1).ToString("N1");
                }
            }
        }
    }

    private void doFreeze()
    {
        fade = new Color(fade.r, fade.g, fade.b, 0.5f);
        Thread.Sleep(20);
        fade = new Color(fade.r, fade.g, fade.b, 1.0f);
        Thread.Sleep(duration);
        fade = new Color(fade.r, fade.g, fade.b, 0.5f);
        isFreeze = false;
        Thread.Sleep(coolDown);
        isCooldown = false;
        cooldownEndSound.Play();
        fade = new Color(fade.r, fade.g, fade.b, 1.0f);
        timerText = "";
    }
}
