using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebCamBackground : MonoBehaviour
{
    /// <summary>
    /// The faraway.摄像机背景距离相机距离
    /// </summary>
    public float faraway = 1999;
    /// <summary>
    /// The width.请求视频的宽度
    /// </summary>
    public int width = 1280;
    /// <summary>
    /// The height.请求视频的高度
    /// </summary>
    public int height = 0;
    /// <summary>
    /// The fps.请求的帧率
    /// </summary>
    public int fps = 64;
    /// <summary>
    /// The ratio.屏幕宽高比率
    /// </summary>
    [Range(1.333f, 1.777f)]
    public float ratio = 1.333f;



    private int screenWidth, screenHeight;

    /// <summary>
    /// Gets the camera fov position by distance.获取一定距离视口的四个定点
    /// </summary>
    /// <returns>The camera fov position by distance.</returns>
    /// <param name="cam">Cam.</param>
    /// <param name="distance">Distance.</param>
    public static Vector3[] GetCameraFovPositionByDistance(Camera cam, float distance)
    {
        Vector3[] corners = new Vector3[4];

        float halfFOV = (cam.fieldOfView * 0.5f) * Mathf.Deg2Rad;
        float aspect = cam.aspect;

        float height = distance * Mathf.Tan(halfFOV);
        float width = height * aspect;

        Transform tx = cam.transform;

        // 左上角
        corners[0] = tx.position - (tx.right * width);
        corners[0] += tx.up * height;
        corners[0] += tx.forward * distance;

        // 右上角
        corners[1] = tx.position + (tx.right * width);
        corners[1] += tx.up * height;
        corners[1] += tx.forward * distance;

        // 左下角
        corners[2] = tx.position - (tx.right * width);
        corners[2] -= tx.up * height;
        corners[2] += tx.forward * distance;

        // 右下角
        corners[3] = tx.position + (tx.right * width);
        corners[3] -= tx.up * height;
        corners[3] += tx.forward * distance;

        return corners;
    }

    /// <summary>
    /// Adapters the screen.全屏匹配方法
    /// </summary>
    void AdapterScreen()
    {

        if (Screen.width > Screen.height)
        {
            height = (int)(width / (Screen.width / (float)Screen.height));
            ratio = Screen.width / (float)Screen.height;
        }
        else
        {

            height = (int)(width / (Screen.height / (float)Screen.width));
            ratio = Screen.height / (float)Screen.width;
        }
        Camera camera = GetComponentInParent<Camera>();
        Vector3[] fovPositions = GetCameraFovPositionByDistance(camera, faraway);
        float x = Vector3.Distance(fovPositions[0], fovPositions[1]);
        float y = Vector3.Distance(fovPositions[0], fovPositions[2]);
        float bgX = x / 10;
        float bgY = y / 10;
        float rate = width / (float)height;



        this.transform.localPosition = Vector3.forward * faraway;

        //当屏幕为横屏时
        if (bgX > bgY)
        {


#if UNITY_ANDROID
            this.transform.localEulerAngles = new Vector3(-270f, 180f, 0f);
            this.transform.localScale = new Vector3(bgX, 1, bgY * rate / ratio);
#elif UNITY_IOS

            this.transform.localEulerAngles = new Vector3(-270f, 180f, 0f);
            this.transform.localScale = new Vector3(bgX, 1, -bgY * rate / ratio);
#elif UNITY_EDITOR
            this.transform.localEulerAngles = new Vector3(-270f, 180f, 0f);
            this.transform.localScale = new Vector3(bgX, 1, bgY*rate/ratio);
#endif



            if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
            {

                this.transform.localEulerAngles = new Vector3(-270f, 180f, 0f);
                this.transform.localScale = new Vector3(bgX, 1, bgY * rate / ratio);
            }

        }
        else
        {


#if UNITY_ANDROID
            this.transform.localEulerAngles = new Vector3(-180f, 90f, -90f);
            this.transform.localScale = new Vector3(bgX * rate * ratio, 1, bgY);
#elif UNITY_IOS

            this.transform.localEulerAngles = new Vector3(-180f, 90f, -90f);
            this.transform.localScale = new Vector3(bgX * rate * ratio, 1, -bgY);

#elif UNITY_EDITOR
            this.transform.localEulerAngles = new Vector3(-270f, 180f, 0f);
            this.transform.localScale = new Vector3(bgX * rate * ratio, 1, bgY);
#endif

            if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
            {

                this.transform.localEulerAngles = new Vector3(-270f, 180f, 0f);
                this.transform.localScale = new Vector3(bgX * rate * ratio, 1, bgY);
            }

        }

    }

    IEnumerator Start()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        AdapterScreen();
        //获取摄像头
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            if (devices != null)
            {
                WebCamTexture tex = new WebCamTexture(devices[0].name, width, height, fps);
                tex.Play();
                MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
                meshRenderer.material.mainTexture = tex;
            }
        }
    }

    void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            AdapterScreen();
        }
    }




    // Update is called once per frame
    void LateUpdate()
    {

        if (screenWidth != Screen.width || screenHeight != Screen.height)
        {

            AdapterScreen();

            screenWidth = Screen.width;
            screenHeight = Screen.height;
        }


    }

}
