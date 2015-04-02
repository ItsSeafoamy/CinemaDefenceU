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

public class GameOver : MonoBehaviour 
{
	public AudioClip bgm;
	public Texture2D background;
	public Texture2D resetBtn;
	public GUISkin menuGUI;
	
	void Start () 
	{
		Music.Change(bgm);
	}
	
	void OnGUI()
	{
		
		GUI.skin = menuGUI;
		
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), background);
		if (GUI.Button (new Rect (Screen.width / 2 - resetBtn.width/4, Screen.height - resetBtn.height/2 - 8, resetBtn.width/2, resetBtn.height/2), "")) 
		{
			Game.Reset();
			Application.LoadLevel ("MainMenu");
		}
	}
}
//can be used for failure state and success state
