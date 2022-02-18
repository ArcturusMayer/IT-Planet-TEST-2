using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class SlideDash : MonoBehaviour
{
    public GameObject slider;
    public GameObject joystick;
    public GameObject arrow;
    public int coolDown = 5000;
    public GameObject icon;
    public Text timer;
    public AudioSource dashSound;
    public AudioSource cooldownEndSound;

    private PhotonView photonView;
    private int sliderTouchID = -1;
    private float sliderRadius = 7.0f;
    private string timerText = "";

    private bool isCoolDown = false;

    private float firstClickTime = 0.01f;
    private float secondClickTime = 0.01f;
    private float radius = 2.1f;
    private float startTime = 0.0f;

    private Color fade;

    private Vector3 target_dash = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        photonView = gameObject.GetComponent<PhotonView>();

        slider = GameObject.FindGameObjectWithTag("Slider");
        joystick = GameObject.FindGameObjectWithTag("Joystick");
        arrow = GameObject.FindGameObjectWithTag("Arrow");
        icon = GameObject.FindGameObjectWithTag("SliderIcon");
        dashSound = GameObject.FindGameObjectWithTag("DashSound").GetComponent<AudioSource>();
        cooldownEndSound = GameObject.FindGameObjectWithTag("CoolDownEndSound").GetComponent<AudioSource>();
        Text[] buffer = FindObjectsOfType<Text>();
        foreach(Text txt in buffer)
        {
            if(txt.tag == "SliderText")
            {
                timer = txt;
            }
        }

        hideSlider();
        fade = icon.GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch t = Input.GetTouch(i);
                    Vector3 touch_pos = new Vector3(Camera.main.ScreenToWorldPoint(t.position).x, Camera.main.ScreenToWorldPoint(t.position).y, 0);

                    if (t.phase == TouchPhase.Began)
                    {
                        if (!(touch_pos.x > joystick.transform.position.x - radius && touch_pos.x < joystick.transform.position.x + radius && touch_pos.y > joystick.transform.position.y - radius && touch_pos.y < joystick.transform.position.y + radius))
                        {
                            secondClickTime = firstClickTime;
                            firstClickTime = Time.time;
                            if (firstClickTime - secondClickTime > 0.21f && secondClickTime != 0)
                            {
                                if (!isCoolDown)
                                {
                                    sliderTouchID = t.fingerId;
                                }
                            }
                        }
                    }

                    if (t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary)
                    {
                        if (t.fingerId == sliderTouchID)
                        {
                            target_dash = touch_pos - transform.position;
                            target_dash = (target_dash.magnitude < sliderRadius) ? target_dash : new Vector3(target_dash.x * sliderRadius / target_dash.magnitude, target_dash.y * sliderRadius / target_dash.magnitude, target_dash.z);

                            slider.transform.position = new Vector3(transform.position.x, transform.position.y, 0.2f);
                            slider.transform.rotation = Quaternion.Euler(0, 0, (touch_pos.x < transform.position.x) ? Vector3.Angle(Vector3.up, target_dash) : -Vector3.Angle(Vector3.up, (touch_pos - transform.position)));
                            slider.transform.localScale = new Vector3(1, target_dash.magnitude / 3.26f, 1);

                            arrow.transform.position = new Vector3(transform.position.x, transform.position.y, 0.2f);
                            arrow.transform.rotation = Quaternion.Euler(0, 0, (touch_pos.x < transform.position.x) ? -Vector3.Angle(Vector3.down, target_dash) : Vector3.Angle(Vector3.down, (touch_pos - transform.position)));
                        }
                    }

                    if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
                    {
                        if (t.fingerId == sliderTouchID)
                        {
                            secondClickTime = firstClickTime;
                            firstClickTime = Time.time;
                            if (firstClickTime - secondClickTime > 0.1f)
                            {
                                gameObject.GetComponent<PlayerController>().dash = new Vector3(-target_dash.x, -target_dash.y, target_dash.z);
                                dashSound.Play();
                                isCoolDown = true;
                                startTime = Time.time;
                                new Thread(doCoolDown).Start();
                            }
                            hideSlider();
                            sliderTouchID = -1;
                        }
                    }
                }
            }

            if (isCoolDown)
            {
                timerText = System.Math.Round(5.0f - (Time.time - startTime), 1).ToString("N1");
            }

            icon.GetComponent<Renderer>().material.color = fade;

            timer.text = timerText;
        }
    }

    private void hideSlider()
    {
        slider.transform.position = new Vector3(slider.transform.position.x, slider.transform.position.y, 1);
        arrow.transform.position = new Vector3(arrow.transform.position.x, arrow.transform.position.y, 1);
    }

    private void doCoolDown()
    {
        fade = new Color(fade.r, fade.g, fade.b, 0.5f);
        Thread.Sleep(coolDown);
        isCoolDown = false;
        cooldownEndSound.Play();
        fade = new Color(fade.r, fade.g, fade.b, 1.0f);
        timerText = "";
    }
}
