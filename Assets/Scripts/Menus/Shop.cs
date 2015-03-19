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

public class Shop : MonoBehaviour {
	
	public Texture2D background;
	public Texture2D iceCreamBtn;
	public Texture2D hotdogBtn;
	public Texture2D popcornBtn;
	public Texture2D sodaBtn;
	public Texture2D coffeeBtn;
	public AudioClip iceCream;
	public AudioClip hotdog;
	public AudioClip popcorn;
	public AudioClip soda;
	public AudioClip coffee;
	public AudioClip welcome;
	public AudioClip bye;
	public Texture2D upgradeArrow;
	public Texture2D backArrow;
	
	void OnGUI(){
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);
		
		//for (int i = 0; i < towers.Length; i++){
			//TowerSet towerset = towers[i];
			//int level = (int) towerset[0].GetType().GetProperty("currentLevel").GetValue(null, null);
			
			//Tower current = level == 0 ? null : towerset[level - 1];
			//Tower next = level == towerset.towers.Length ? null : towerset[level];
			//}
		if (GUI.Button (new Rect (Screen.width / 2 - 250, Screen.height / 2 - 250, 100, 100), (iceCreamBtn)) ) {
			Debug.Log ("Clicked the button!");
			audio.PlayOneShot(iceCream);
		if (GUI.Button (new Rect(Screen.width/2, Screen.height/2, 100, 100), (hotdogBtn))){
				Debug.Log ("clicked the button");
				audio.PlayOneShot(hotdog);
				}
		if (GUI.Button (new Rect (Screen.width / 2 - 125, Screen.height/2, 100, 100(popcornBtn))){
				Debug.Log ("clicked the button");
				audio.PlayOneShot(popcorn);
			}
		if (GUI.Button (new Rect(Screen.width/2, Screen.height - 300, 100, 100), (sodaBtn))){
				Debug.Log ("clicked button");
				audio.PlayOneShot(soda);			
			}
		int getcurrentLevel = Tower.currentLevel;		
			if (GUI.Button (new Rect (Screen.width / 2 - 125, Screen.height / 2 -250, 25, 25), (upgradeArrow)) && Game.money <= 200 && IceCreamGun.currentLevel < 4 ) {
				IceCreamGun.currentLevel ++;
			}
				if (GUI.Button (new Rect (Screen.width / 2 + 125, Screen.height/2, 25, 25), (upgradeArrow))&& Game.money <= 200 && HotdogCannon.currentLevel < 4) {
					HotdogCannon.currentLevel ++;
			}
					if(GUI.Button (new Rect(Screen.width /2, Screen.height/2, 25, 25),(upgradeArrow))&& Game.money <=200 && PopcornGuncurrentLevel < 4) {
					PopcornGun.currentLevel ++;
			}
						if (GUI.Button (new Rect(Screen.width/2 +125, Screen.height -300, 100, 100), (upgradeArrow))&& Game.money <=200 && SodaGuncurrentLevel < 4) {
							SodaGun.currentLevel ++; //don't know variable name for soda gun, guessing it'll be this 
			}

			}
			if (GUI.Button (new Rect(-Screen.height +50, Screen.width, 50, 50) (backArrow))) {
				Application.LoadLevel("Lobby");
			}
	}
}	
