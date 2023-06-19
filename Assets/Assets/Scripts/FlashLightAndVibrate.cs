using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightAndVibrate : MonoBehaviour
{
    // Start is called before the first frame update
    private AndroidJavaObject currentActivity;
    private AndroidJavaObject context;
    
    void Start () 
    {
        AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        currentActivity = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
        context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
    }
 
    public void  FlashlightOn() 
    {
        AndroidJavaObject params1 = new AndroidJavaObject("java.lang.String","FlashlightOn");
        currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(() => { 
        AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");
        AndroidJavaObject javaString = new AndroidJavaObject("java.lang.String", "FlashlightOn");
        Toast.CallStatic<AndroidJavaObject>("makeText", context, javaString, Toast.GetStatic<int>("LENGTH_SHORT")).Call("show");
        }));
        currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(() => { 
            AndroidJavaClass cls = new AndroidJavaClass("android.hardware.camera2.CameraManager");
            AndroidJavaObject obj = context.Call<AndroidJavaObject>("getSystemService", new AndroidJavaObject("java.lang.String","camera"));
            obj.Call("setTorchMode", "0", true);
        }));
    }

    public void VibrateScreen() 
    {
        AndroidJavaObject params1 = new AndroidJavaObject("java.lang.String","VibrateScreen");
        currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(() => { 
        AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");
        AndroidJavaObject javaString = new AndroidJavaObject("java.lang.String", "VibrateScreen");
        Toast.CallStatic<AndroidJavaObject>("makeText", context, javaString, Toast.GetStatic<int>("LENGTH_SHORT")).Call("show");
        }));
        AndroidJavaClass vibrateClass = new AndroidJavaClass("com.example.myapplication.MainActivity");
        int milliseconds = 500;
        vibrateClass.CallStatic("Vibrate", milliseconds);
    }
}
