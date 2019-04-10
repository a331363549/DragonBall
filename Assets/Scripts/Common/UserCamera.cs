using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserCamera : MonoBehaviour {

    private static UserCamera sInstance = null;
    public static UserCamera Instance
    {
        get
        {
            return sInstance;
        }
    }

    private Camera gameCamera;
    public Camera GameCamera
    {
        get
        {
            return gameCamera;
        }
    }



    //public GUIContent logTxt;
    ////public GUIContent ucode;
    //public void OnGUI()
    //{
    //    GUIStyle style = new GUIStyle();
    //    style.fontSize = 30;
    //    style.normal.textColor = Color.red;
    //    GUI.Label(new Rect(0, 0, 100, 100), logTxt, style);

    //    //GUIStyle s2 = new GUIStyle();
    //    //s2.fontSize = 30;
    //    //s2.normal.textColor = Color.blue;
    //    //GUI.Label(new Rect(0, 200, 100, 100), ucode, s2);

    //    //GUI.Label(new Rect(0, 250, 100, 100), "v7", s2);
    //}

    private Action showCompleted = null;
    public void PlayShow(Action showCompleted,string anim)
    {
        this.showCompleted = showCompleted;
        //Animator animator = this.GetComponent<Animator>();
        //if (animator != null)
        //{
        //    animator.enabled = true;
        //    animator.SetBool(anim, true);
        //}
        Animation animation = this.GetComponent<Animation>();
        animation.Play(anim);
    }


    public void InitShow(string anim)
    {
        if (anim == "Square")
        {
            transform.localPosition = new Vector3(0f, 7f, 0f);
            transform.localEulerAngles = new Vector3(90f, 90f, 0f);
        }
        if (anim == "Street")
        {
            transform.localPosition = new Vector3(0f, 4f, -4f);
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        if (anim == "GuStreet")
        {
            transform.localPosition = new Vector3(-4.69f, 14.63f, 2.95f);
            transform.localEulerAngles = new Vector3(41.5f, 14.3f, 0f);
        }

        //Animator animator = this.GetComponent<Animator>();
        //if (animator != null)
        //{
        //    animator.enabled = false;
        //    animator.SetBool("Street", false);
        //    animator.SetBool("Square", false);
        //    animator.SetBool("GuStreet", false);
        //}
    }

    public void DisableCamAnim(string anim)
    {
        Animator animator = this.GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = false;
            animator.SetBool("Street", false);
            animator.SetBool("Square", false);
            animator.SetBool("GuStreet", false);
        }
    }

    private bool saveEnable = false;
    private Vector3 position = Vector3.zero;
    private Vector3 eulerAngles = Vector3.zero;
    public void SaveCamTrans()
    {
        saveEnable = true;
        position = transform.position;
        eulerAngles = transform.eulerAngles;
    }

    public void ResetCamTrans()
    {
        if (saveEnable)
        {
            transform.position = position;
            transform.eulerAngles = eulerAngles;
        }
    }

    private void Awake()
    {
        sInstance = this;
    }

    private void Start()
    {
        gameCamera = GetComponent<Camera>();
    }

    private void OnDestroy()
    {
        if (sInstance == this)
        {
            sInstance = null;
        }
    }

    private void OnCameraReady()
    {
        //Animator animator = this.GetComponent<Animator>();
        //if (animator)
        //{
        //    animator.enabled = false;
        //}
        if (this.showCompleted != null)
        {
            this.showCompleted();
        }

    }
}
