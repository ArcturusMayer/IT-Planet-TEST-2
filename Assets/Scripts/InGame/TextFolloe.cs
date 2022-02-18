using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFolloe : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PhotonView>().IsMine)
        {
            RectTransform rt = GetComponent<RectTransform>();
            Vector3 roboScreenPos = Camera.main.WorldToViewportPoint((player.transform.position));
            rt.anchorMax = roboScreenPos;
            rt.anchorMin = roboScreenPos;
        }
        else
        {
            RectTransform rt = GetComponent<RectTransform>();
            Vector3 roboScreenPos = Camera.main.WorldToViewportPoint((player.transform.position));
            rt.anchorMax = roboScreenPos;
            rt.anchorMin = roboScreenPos;
        }
    }
}
