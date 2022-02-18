using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Positioning : MonoBehaviour
{
    public GameObject joystick;
    public GameObject freeze;
    public GameObject slider;
    public GameObject hp;
    public Text freezeTimer;
    public Text sliderTimer;
    public Text hptext;
    public Text awaiting;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 buf_pos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width/5, Screen.height/3));
        joystick.transform.localPosition = new Vector3 (buf_pos.x, buf_pos.y, 11);
        buf_pos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 15 * 14, Screen.height / 8 * 7));
        freeze.transform.localPosition = new Vector3(buf_pos.x, buf_pos.y, 11);
        buf_pos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 18 * 15, Screen.height / 8 * 7));
        slider.transform.localPosition = new Vector3(buf_pos.x, buf_pos.y, 11);
        freezeTimer.transform.position = new Vector2(Screen.width / 15 * 14 + 4, Screen.height / 8 * 7);
        freezeTimer.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 720 * 74, Screen.height / 360 * 30);
        freezeTimer.fontSize = Screen.height / 16;
        sliderTimer.transform.position = new Vector2(Screen.width / 18 * 15 + 4, Screen.height / 8 * 7);
        sliderTimer.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 720 * 74, Screen.height / 360 * 30);
        sliderTimer.fontSize = Screen.height / 16;
        buf_pos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 15 * 1, Screen.height / 8 * 7));
        hp.transform.localPosition = new Vector3(buf_pos.x, buf_pos.y, 11);
        hptext.transform.position = new Vector2(Screen.width / 15 * 1 - 4, Screen.height / 8 * 7);
        hptext.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 720 * 74, Screen.height / 360 * 30);
        hptext.fontSize = Screen.height / 16;
        awaiting.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 720 * 600, Screen.height / 360 * 100);
        awaiting.fontSize = Screen.height / 16;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
