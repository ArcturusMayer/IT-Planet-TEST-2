using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    public float thrust = 200.0f;
    public Vector3 target_move = new Vector3(0, 0, 0);
    public Vector3 dash = new Vector3(0, 0, 0);
    public Sprite greenPlayerSkin;
    public Text nicknamebox;
    public string nickname;

    private Vector3 pos = new Vector3(0, 0, 0);
    private int bounds = 5;
    private Rigidbody2D physCore;
    private PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        physCore = gameObject.GetComponent<Rigidbody2D>();
        photonView = gameObject.GetComponent<PhotonView>();

        physCore.freezeRotation = true;
        if (photonView.IsMine)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = greenPlayerSkin;
        }

        nicknamebox.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 720 * 200, Screen.height / 360 * 110);
        nicknamebox.fontSize = Screen.height / 16;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -11);
            //target_move.Normalize();
            transform.Translate(target_move * speed * Time.deltaTime);
            dash.Normalize();
            physCore.AddForce(dash * thrust, ForceMode2D.Impulse);
            dash = new Vector3(0, 0, 0);
        }
        nicknamebox.text = nickname;
    }
}
