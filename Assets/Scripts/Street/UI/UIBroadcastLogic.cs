using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NewEngine.Framework.UI;

public class UIBroadcastLogic : UILogic {

    public RectTransform[] message;
    public GameObject uiMessage;

    private readonly Color RedpacketMsgColor = new Color(250 / 255f, 1, 193 / 255f);

    private float[] msgCD;
    
    private CanvasScaler canvasScaler;
    
	// Use this for initialization
	void Start ()
    {
        canvasScaler = gameObject.GetComponentInParent<CanvasScaler>();
        msgCD = new float[message.Length];
    }

    private float waitingTime = 0;
	// Update is called once per frame
	void Update () {
        if (NewMsgData.Instance.MsgQueue.Count == 0)
        {
            return;
        }
        waitingTime += Time.deltaTime;
        if (waitingTime < 1)
        {
            return;
        }

        for (int idx = Random.Range(0, msgCD.Length), count = 0; count < msgCD.Length; idx = (idx + 1) % msgCD.Length, count++)
        {
            if (msgCD[idx] < 0)
            {
                GameObject msgGo = GameObject.Instantiate(uiMessage);
                msgGo.transform.SetParent(transform);
                RectTransform rectTransform = msgGo.GetComponent<RectTransform>();
                rectTransform.localPosition = message[idx].localPosition;
                rectTransform.localRotation = message[idx].localRotation;
                rectTransform.localScale = message[idx].localScale;
                rectTransform.pivot = message[idx].pivot;
                rectTransform.offsetMax = message[idx].offsetMax;
                rectTransform.offsetMin = message[idx].offsetMin;
                UIBroadcastMsg uiMsg = msgGo.GetComponent<UIBroadcastMsg>();

                NewMsg msg = NewMsgData.Instance.MsgQueue.Dequeue();
                Debug.Log("InitMessage:" + msg.msg);
                if (msg.msgType == 0)
                {
                    uiMsg.normalBg.SetActive(false);
                    uiMsg.redPacketBg.SetActive(true);
                    uiMsg.message.color = RedpacketMsgColor;
                }
                else
                {
                    uiMsg.normalBg.SetActive(true);
                    uiMsg.redPacketBg.SetActive(false);
                    uiMsg.message.color = Color.white;
                }
                uiMsg.message.text = msg.msg;
                float height = rectTransform.rect.height;
                float width = msg.msg.Length * uiMsg.message.fontSize + 100f;
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
                Vector3 position = rectTransform.localPosition;
                position.x = (canvasScaler.referenceResolution.x + width) * 0.5f;
                rectTransform.localPosition = position;
                uiMsg.targetPos = Mathf.CeilToInt(position.x) * -1 - 100;
                msgGo.SetActive(true);

                msgCD[idx] = (width + Random.Range(20, 100)) / uiMsg.speed;
                waitingTime = 0;

                return;
            }
            else
            {
                msgCD[idx] -= Time.deltaTime;
            }
        }
	}
}
