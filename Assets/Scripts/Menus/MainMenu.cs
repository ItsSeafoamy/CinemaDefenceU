/*
*	Copyright (C) 2015 Alexander Prince
*	
*	This program is free software; you can redistribute it and/or modify
*	it under the terms of the GNU General Public License as published by
*	the Free Software Foundation; either version 2 of the License, or
*	any later version.
*		
*	This program is distributed in the hope that it will be useful,
*	but WITHOUT ANY WARRANTY; without even the implied warranty of
*	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*	GNU General Public License for more details.
*			
*	You should have received a copy of the GNU General Public License along
*	with this program; if not, write to the Free Software Foundation, Inc.,
*	51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public AudioClip bgm;
	public Texture2D background;
	public Texture2D startBtn;
	public Texture2D tutorialBtn;
	public Texture2D quitBtn;
	
	public GUISkin startGUI, tutorialGUI, quitGUI, defaultGUI;
	
	void Start () {
		Music.Change(bgm);
	}

	void OnGUI(){
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), background);
		
		GUI.skin = startGUI;

		if (GUI.Button (new Rect (Screen.width / 2 - startBtn.width/4, Screen.height - (startBtn.height/2+8)*3, startBtn.width/2, startBtn.height/2), "")) {
			GUI.skin = null;
			Application.LoadLevel ("CarPark");
		}
		
		GUI.skin = tutorialGUI;
		
		if (GUI.Button (new Rect (Screen.width / 2 - startBtn.width/4, Screen.height - (startBtn.height/2+8)*2, startBtn.width/2, startBtn.height/2), "")) {
			GUI.skin = null;
			Application.LoadLevel ("Tutorial");
		}
		
		GUI.skin = quitGUI;
		
		if (GUI.Button (new Rect (Screen.width / 2 - startBtn.width/4, Screen.height - (startBtn.height/2+8)*1, startBtn.width/2, startBtn.height/2), "")) {
			GUI.skin = null;
			Application.Quit();
		}
	}
}
