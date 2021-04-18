using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleCameraInAspectRatio : MonoBehaviour
{
    //長寬比，預設為9:16
    public float aspectRatio = 16f / 9f;

    void Start()
    {
        Camera cam = GetComponent<Camera>();

        //裝置螢幕的長寬比
        float screenRatio = (float)Screen.width / (float)Screen.height;

        //裝置螢幕長寬比和目標長寬比的比值
        float scale = screenRatio / aspectRatio;

        //裝置螢幕長寬比大於目標長寬比(Main Camera高度應維持，寬度縮小)
        if (scale > 1f)
        {
            Rect pixRect = cam.pixelRect; //Main Camera的矩形

            //設定寬度
            pixRect.width = pixRect.height * aspectRatio;
            pixRect.y = 0f;

            //顯示寬度要在(過寬的)螢幕正中央，所以要除2
            pixRect.x = ((float)Screen.width - pixRect.width) / 2f;

            //設定新的Main Camera矩形
            cam.pixelRect = pixRect;
        }

        //裝置螢幕長寬比小於目標長寬比(Main Camera寬度應維持，長度縮小)
        else
        {
            Rect pixRect = cam.pixelRect;

            //設定高度
            pixRect.height = pixRect.width / aspectRatio;
            pixRect.x = 0f;

            //顯示高度要在(過高的)螢幕正中央，所以要除2
            pixRect.y = ((float)Screen.height - pixRect.height) / 2f;

            //設定新的Main Camera矩形
            cam.pixelRect = pixRect;
        }
    }
}