﻿using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

/*
 * credit:
 * https://github.com/ChrisMaire/unity-native-sharing 
 */

public class NativeShare : MonoBehaviour {
	public string ScreenshotName = "overflow_screenshot.png";

    public void ShareScreenshotWithText()
    {
		/*Unity screenshots are run asynchronously and as such we will need to check that the file has been written, 
		 * or put a delay between capturing the screenshot and sharing it using a coroutine. 
		 * Otherwise we will likely end up trying to access a file that does not yet exist or will access a previous version of the screenshot.*/
		StartCoroutine (Idk());
    }

	IEnumerator Idk(){
		string screenShotPath = Application.persistentDataPath + "/" + ScreenshotName;
		Application.CaptureScreenshot(ScreenshotName);
		yield return new WaitForEndOfFrame ();
		Share("Swag! I finally got "+LevelManager.score+" in #Overflow! Can you beat me in #Overflow? Search Overflow in AppStore!",screenShotPath,"","Overflow LOVE");
	}

	public void Share(string shareText, string imagePath, string url, string subject)
	{
#if UNITY_ANDROID
		AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
		AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
		
		intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
		AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
		AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + imagePath);
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
		intentObject.Call<AndroidJavaObject>("setType", "image/*");

		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), shareText);

		AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
		
		AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, subject);
		currentActivity.Call("startActivity", jChooser);
#elif UNITY_IOS
		CallSocialShareAdvanced(shareText, subject, url, imagePath);
#else
		Debug.Log("No sharing set up for this platform.");
#endif
	}

#if UNITY_IOS
	public struct ConfigStruct
	{
		public string title;
		public string message;
	}

	[DllImport ("__Internal")] private static extern void showAlertMessage(ref ConfigStruct conf);
	
	public struct SocialSharingStruct
	{
		public string text;
		public string url;
		public string image;
		public string subject;
	}
	
	[DllImport ("__Internal")] private static extern void showSocialSharing(ref SocialSharingStruct conf);
	
	public static void CallSocialShare(string title, string message)
	{
		ConfigStruct conf = new ConfigStruct();
		conf.title  = title;
		conf.message = message;
		showAlertMessage(ref conf);
	}

	public static void CallSocialShareAdvanced(string defaultTxt, string subject, string url, string img)
	{
		SocialSharingStruct conf = new SocialSharingStruct();
		conf.text = defaultTxt; 
		conf.url = url;
		conf.image = img;
		conf.subject = subject;
		
		showSocialSharing(ref conf);
	}
#endif
}
