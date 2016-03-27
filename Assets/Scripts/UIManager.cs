﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Net;

public class UIManager : MonoBehaviour {

	public Canvas creditPage;
	public Canvas settingPage;
	public Canvas numberPage;
	public Button numberButton;
	public Sprite soundOnSprite;
	public Sprite soundOffSprite;
	public GameObject score;
	public GameObject time;
	static Text scoreText;
	static Text timeText;
	private const string FACEBOOK_URL = "http://www.facebook.com/dialog/feed";
	private const string FACEBOOK_APP_ID = "794667970397816";

	void Start(){
		scoreText = score.GetComponent<Text>();
		timeText = time.GetComponent<Text> ();
	}

	public void LoadTetrisLevel(){
		SceneManager.LoadScene ("Tetris Level");
	}

	public void LoadSpaceLevel(){
		SceneManager.LoadScene ("Space Level");
	}

	public void ToggleCreditPage(){
		ToggleButton ();
		creditPage.enabled =  creditPage.enabled ? false : true;
	}

	public void ToggleSettingPage(){
		ToggleButton ();
		settingPage.enabled = settingPage.enabled ? false : true;
	}

	public void ToggleNumberPage(){
		ToggleButton ();
		numberPage.enabled = numberPage.enabled ? false : true;
	}

	private void ToggleButton(){
		numberButton.image.enabled = numberButton.enabled ? false : true;
		numberButton.enabled = numberButton.enabled ? false : true;
	}

	public void ToggleSound(Button soundButton){
		soundButton.image.sprite = soundButton.image.sprite.name.Equals ("sound on") ? soundOffSprite : soundOnSprite;
	}

	public static void updateScore(int score){
		scoreText.text = score+"";
	}

	public static void updateTime(int time){
		int minutes = time / 60;
		int seconds = time % 60;
		timeText.text = minutes.ToString("0#")+" : "+seconds.ToString("0#");
	}
		
	public static bool isHavingWiFi()
	{
		try
		{
			using (WebClient client = new WebClient())
				using (var stream = client.OpenRead("http://www.google.com"))
				{
					return true;
				}
		}
		catch
		{
			return false;
		}
	}

	public void FacebookShare(){
		
	}

	void ShareToFacebook (string linkParameter, string nameParameter, string captionParameter, string descriptionParameter, string pictureParameter, string redirectParameter)
	{
		Application.OpenURL (FACEBOOK_URL + "?app_id=" + FACEBOOK_APP_ID +
			"&link=" + WWW.EscapeURL(linkParameter) +
			"&name=" + WWW.EscapeURL(nameParameter) +
			"&caption=" + WWW.EscapeURL(captionParameter) + 
			"&description=" + WWW.EscapeURL(descriptionParameter) + 
			"&picture=" + WWW.EscapeURL(pictureParameter) + 
			"&redirect_uri=" + WWW.EscapeURL(redirectParameter));
	}
}
