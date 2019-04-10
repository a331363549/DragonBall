using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBroadcastMsg : MonoBehaviour {

    public Text message;
    public float speed = 5f;
    public GameObject normalBg;
    public GameObject redPacketBg;

    [HideInInspector]
    public int targetPos = -3000;
    
    private Vector3 translation = Vector3.left;
    private RectTransform rectTransform = null;

    //public void InitMessage(string msg, float referenceWidth, float height)
    //{
    //    Debug.Log("InitMessage:" + msg);
    //    message.text = msg;
    //    float width = msg.Length * message.fontSize + 100f;
    //    rectTransform = gameObject.GetComponent<RectTransform>();
    //    rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    //    rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    //    Vector3 position = rectTransform.localPosition;
    //    position.x = (referenceWidth + width) * 0.5f;
    //    rectTransform.localPosition = position;
    //    targetPos = Mathf.CeilToInt(position.x) * -1 - 100;
    //    gameObject.SetActive(true);
    //}

    // Use this for initialization
    void Start ()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
    }
	
	// Update is called once per frame
	void Update () {

        if (rectTransform == null)
        {
            return;
        }

        if (rectTransform.localPosition.x < targetPos)
        {
            enabled = false;
            GameObject.Destroy(gameObject);
            return;
        }

        translation = rectTransform.localPosition;
        translation.x -= Time.deltaTime * speed;
        rectTransform.localPosition = translation;
    }
}
