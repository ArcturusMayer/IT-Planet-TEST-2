using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour
{
    public GameObject pad;

    Vector3 target_vector;

    public Rigidbody2D player;
    public PlayerController controller;

    private float radius = 2.1f;
    private int joystickTouchID = -1;
    public bool isJoined = false;

    // Start is called before the first frame update
    void Start()
    {
        clearPad();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isJoined)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject plr in players)
            {
                if (plr.GetComponent<PhotonView>().IsMine)
                {
                    player = plr.GetComponent<Rigidbody2D>();
                    controller = plr.GetComponent<PlayerController>();
                }
            }
            isJoined = false;
        }

        if (Input.touchCount > 0)
        {
            for(int i = 0; i < Input.touchCount; i++)
            {
                Touch t = Input.GetTouch(i);
                Vector3 touch_pos = new Vector3(Camera.main.ScreenToWorldPoint(t.position).x, Camera.main.ScreenToWorldPoint(t.position).y, 0);

                if (t.phase == TouchPhase.Began)
                {
                    if(touch_pos.x > transform.position.x - radius && touch_pos.x < transform.position.x + radius && touch_pos.y > transform.position.y - radius && touch_pos.y < transform.position.y + radius)
                    {
                        joystickTouchID = t.fingerId;

                        target_vector = touch_pos - transform.position;

                        if (target_vector.magnitude < radius)
                        {
                            pad.transform.position = touch_pos;
                            controller.target_move = target_vector;
                        }
                        else
                        {
                            pad.transform.position = new Vector3(radius * (target_vector.x / target_vector.magnitude) + transform.position.x, radius * (target_vector.y / target_vector.magnitude) + transform.position.y, 0);
                            controller.target_move = new Vector3(radius * (target_vector.x / target_vector.magnitude), radius * (target_vector.y / target_vector.magnitude), 0);
                        }
                    }
                }

                if(t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary)
                {
                    if (t.fingerId == joystickTouchID)
                    {
                        target_vector = touch_pos - transform.position;

                        if (target_vector.magnitude < radius)
                        {
                            pad.transform.position = touch_pos;
                            controller.target_move = target_vector;
                        }
                        else
                        {
                            pad.transform.position = new Vector3(radius * (target_vector.x / target_vector.magnitude) + transform.position.x, radius * (target_vector.y / target_vector.magnitude) + transform.position.y, 0);
                            controller.target_move = new Vector3(radius * (target_vector.x / target_vector.magnitude), radius * (target_vector.y / target_vector.magnitude), 0);
                        }
                    }
                }

                if(t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
                {
                    if (t.fingerId == joystickTouchID)
                    {
                        joystickTouchID = -1;
                        clearPad();
                        controller.target_move = new Vector3(0, 0, 0);
                    }
                }
            }
        }

        player.angularVelocity = 0;
    }

    private void clearPad()
    {
        pad.transform.position = transform.position;
    }
}
