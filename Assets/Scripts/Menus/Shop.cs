/*
*	Copyright (C) 2015 Alexander Prince, Dylan McCormack
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
	
	public AudioClip bgm;
	
	void Start(){
		Music.Change(bgm);
	}
	
	void OnGUI(){
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);
		
		//for (int i = 0; i < towers.Length; i++){
			//TowerSet towerset = towers[i];
			//int level = (int) towerset[0].GetType().GetProperty("currentLevel").GetValue(null, null);
			
			//Tower current = level == 0 ? null : towerset[level - 1];
			//Tower next = level == towerset.towers.Length ? null : towerset[level];
			//}
		if (GUI.Button (new Rect (Screen.width / 2 - 270, Screen.height / 2 -100, 100, 100), (iceCreamBtn))) {
			Debug.Log ("Clicked the button!");
			GetComponent<AudioSource>().PlayOneShot(iceCream);
		}
		
		if (GUI.Button (new Rect(Screen.width/2 - 90, Screen.height/2 -100, 100, 100), (hotdogBtn))){
			Debug.Log ("clicked the button");
			GetComponent<AudioSource>().PlayOneShot(hotdog);
		}
		
		if (GUI.Button (new Rect (Screen.width / 2 - 440, Screen.height/2- 100, 100, 100), (popcornBtn))){
			Debug.Log ("clicked the button");
			GetComponent<AudioSource>().PlayOneShot(popcorn);
		}
		
		if (GUI.Button (new Rect(Screen.width/2 + 90, Screen.height/2 - 100, 100, 100), (sodaBtn))){
			Debug.Log ("clicked button");
			GetComponent<AudioSource>().PlayOneShot(soda);			
		}
		
		if (GUI.Button (new Rect(Screen.width/2 + 270, Screen.height /2 - 100, 100, 100), (coffeeBtn))){
			Debug.Log ("clicked button");
			GetComponent<AudioSource>().PlayOneShot(coffee);			
		}
			
		if (GUI.Button (new Rect (Screen.width / 2 - 170, Screen.height / 2 -100, 25, 25), (upgradeArrow)) && Game.money <= 200 && IceCreamGun.currentLevel < 4 ) {
			IceCreamGun.currentLevel ++;
		}
		
		if (GUI.Button (new Rect (Screen.width / 2 + 10, Screen.height/2 - 100, 25, 25), (upgradeArrow))&& Game.money <= 200 && HotdogCannon.currentLevel < 4) {
			HotdogCannon.currentLevel ++;
		}
		
		if(GUI.Button (new Rect(Screen.width /2 - 340, Screen.height/2 - 100, 25, 25),(upgradeArrow))&& Game.money <=200 && PopcornGun.currentLevel < 4) {
			PopcornGun.currentLevel ++;
		}
		
		if (GUI.Button (new Rect(Screen.width/2 +190, Screen.height/2 - 100, 25, 25), (upgradeArrow))&& Game.money <=200 && SodaGun.currentLevel < 4) {
			SodaGun.currentLevel ++; //don't know variable name for soda gun, guessing it'll be this 
		}
		
		if (GUI.Button (new Rect(Screen.width/2 +370, Screen.height/2 -100, 25, 25), (upgradeArrow))&& Game.money <=200 && Coffee.currentLevel < 4) {
			Coffee.currentLevel ++; 
		}
		
		if (GUI.Button (new Rect(Screen.width/2-60, Screen.height/2 + 170, 50, 50), (backArrow))) {
			Application.LoadLevel(Game.nextLevel);
		}
	}
}	
