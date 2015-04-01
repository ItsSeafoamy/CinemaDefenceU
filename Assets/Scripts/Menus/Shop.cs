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
	
	public TowerSet[] towers;
	
	public AudioClip bgm;
	
	void Start(){
		Music.Change(bgm);
	}
	
	void OnGUI(){
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);
		
		for (int i = 0; i < towers.Length; i++){
			TowerSet towerset = towers[i];
			int level = (int) towerset[0].GetType().GetProperty("currentLevel").GetValue(null, null);
			
			Tower current = level == 0 ? null : towerset[level - 1];
			Tower next = level == towerset.towers.Length ? null : towerset[level];
			
			Tower display = next == null ? current : next;
			
			if (GUI.Button(new Rect((i*210) + 16, Screen.height/2f + 110, 100, 100), display.GetComponent<SpriteRenderer>().sprite.texture)){
					GetComponent<AudioSource>().Stop();
					GetComponent<AudioSource>().PlayOneShot(display.info);
			}
			
			GUI.Box(new Rect((i*210), 48, 132, 300), "");
			GUI.Label(new Rect((i*210) + 8, 64, 132, 25), "<b>" + display.name + "</b>");
			
			if (current != null){
				GUI.Label(new Rect((i*210) + 8, 64 + 25, 132, 25), "Lv. " + (next == null ? "Max" : level.ToString()));
				GUI.Label(new Rect((i*210) + 8, 64 + 50, 132, 25), "Damage: " + current.baseDamage);
				GUI.Label(new Rect((i*210) + 8, 64 + 75, 132, 25), "Fire Rate: " + (int) (60 / current.fireRate));
				GUI.Label(new Rect((i*210) + 8, 64 + 100, 132, 25), "Range: " + current.GetComponent<CircleCollider2D>().radius);
			
				if (display is IceCreamGun){
					GUI.Label(new Rect((i*210) + 8, 64 + 125, 132, 25), "Slowness: " + ((IceCreamGun) current).multipler);
				} else if (display is Coffee){
					GUI.Label(new Rect((i*210) + 8, 64 + 125, 132, 25), "DpS: " + ((Coffee) current).damagePerSecond);
				} else if (display is SodaGun){
					GUI.Label(new Rect((i*210) + 8, 64 + 125, 132, 25), "AoE: " + ((SodaGun) current).range);
					GUI.Label(new Rect((i*210) + 8, 64 + 150, 132, 25), "Splash: " + (((int) (current.baseDamage * ((SodaGun) current).damagePercent * 100f)) / 100f));
				}
			
				if (next != null){
					GUI.color = new Color(0, 255, 0, 255);
					GUI.Label(new Rect((i*210) + 90, 64 + 25, 132, 25), "→ " + (level+1));
					
					if (next.baseDamage > current.baseDamage){
						GUI.color = new Color(0, 255, 0, 255);
					} else if (next.baseDamage == current.baseDamage){
						GUI.color = new Color(0.8f, 0.8f, 0.8f, 255);
					} else if (next.baseDamage < current.baseDamage){
						GUI.color = new Color(255, 0, 0, 255);
					}
					GUI.Label(new Rect((i*210) + 90, 64 + 50, 132, 25), "→ " + next.baseDamage);
				
					if (next.fireRate < current.fireRate){
						GUI.color = new Color(0, 255, 0, 255);
					} else if (next.fireRate == current.fireRate){
						GUI.color = new Color(0.8f, 0.8f, 0.8f, 255);
					} else if (next.fireRate > current.fireRate){
						GUI.color = new Color(255, 0, 0, 255);
					}
					GUI.Label(new Rect((i*210) + 90, 64 + 75, 132, 25), "→ " + (int) (60 / next.fireRate));
				
					if (next.GetComponent<CircleCollider2D>().radius > current.GetComponent<CircleCollider2D>().radius){
						GUI.color = new Color(0, 255, 0, 255);
					} else if (next.GetComponent<CircleCollider2D>().radius == current.GetComponent<CircleCollider2D>().radius){
						GUI.color = new Color(0.8f, 0.8f, 0.8f, 255);
					} else if (next.GetComponent<CircleCollider2D>().radius < current.GetComponent<CircleCollider2D>().radius){
						GUI.color = new Color(255, 0, 0, 255);
					}
					GUI.Label(new Rect((i*210) + 90, 64 + 100, 132, 25), "→ " + next.GetComponent<CircleCollider2D>().radius);
				
					if (display is IceCreamGun){
						if (((IceCreamGun) next).multipler > ((IceCreamGun) current).multipler){
							GUI.color = new Color(0, 255, 0, 255);
						} else if (((IceCreamGun) next).multipler == ((IceCreamGun) current).multipler){
							GUI.color = new Color(0.8f, 0.8f, 0.8f, 255);
						} else if (((IceCreamGun) next).multipler < ((IceCreamGun) current).multipler){
							GUI.color = new Color(255, 0, 0, 255);
						}
						GUI.Label(new Rect((i*210) + 90, 64 + 125, 132, 25), "→ " + ((IceCreamGun) next).multipler);
					} else if (display is Coffee){
						if (((Coffee) next).damagePerSecond > ((Coffee) current).damagePerSecond){
							GUI.color = new Color(0, 255, 0, 255);
						} else if (((Coffee) next).damagePerSecond == ((Coffee) current).damagePerSecond){
							GUI.color = new Color(0.8f, 0.8f, 0.8f, 255);
						} else if (((Coffee) next).damagePerSecond < ((Coffee) current).damagePerSecond){
							GUI.color = new Color(255, 0, 0, 255);
						}
						GUI.Label(new Rect((i*210) + 90, 64 + 125, 132, 25), "→ " + ((Coffee) next).damagePerSecond);
					} else if (display is SodaGun){
						if (((SodaGun) next).range > ((SodaGun) current).range){
							GUI.color = new Color(0, 255, 0, 255);
						} else if (((SodaGun) next).range == ((SodaGun) current).range){
							GUI.color = new Color(0.8f, 0.8f, 0.8f, 255);
						} else if (((SodaGun) next).range < ((SodaGun) current).range){
							GUI.color = new Color(255, 0, 0, 255);
						}
						GUI.Label(new Rect((i*210) + 90, 64 + 125, 132, 25), "→ " + ((SodaGun) next).range);
						
						if (((SodaGun) next).damagePercent > ((SodaGun) current).damagePercent){
							GUI.color = new Color(0, 255, 0, 255);
						} else if (((SodaGun) next).damagePercent == ((SodaGun) current).damagePercent){
							GUI.color = new Color(0.8f, 0.8f, 0.8f, 255);
						} else if (((SodaGun) next).damagePercent < ((SodaGun) current).damagePercent){
							GUI.color = new Color(255, 0, 0, 255);
						}
						GUI.Label(new Rect((i*210) + 90, 64 + 150, 132, 25), "→ " + (((int) (next.baseDamage * ((SodaGun) next).damagePercent * 100f)) / 100f));
					}
				
					GUI.color = new Color(255, 255, 255, 255);
				}	
			} else {
				GUI.color = new Color(255, 0, 0, 255);
				GUI.Label(new Rect((i*210) + 8, 64 + 25, 132, 25), "Locked");
				GUI.color = new Color(255, 255, 255, 255);
			}
			
			if (next != null){
				string cost = "Cost: " + next.cost + "G";
				Vector2 size = GUI.skin.label.CalcSize(new GUIContent(cost));
				
				GUI.Label(new Rect((i*210) + 16 + 50 - size.x/2f, Screen.height/2f + 220, 100, 100), cost);
				if (GUI.Button (new Rect((i*210) + 116, Screen.height/2f + 110, 25, 25), upgradeArrow) && Game.money >= next.cost) {
					Game.money -= next.cost;
					towerset[0].GetType().GetProperty("currentLevel").SetValue(null, ++level, null);
					Debug.Log("Upgraded to level: " + level);
				}
			} else {
				string maxed = "Maxed";
				Vector2 size = GUI.skin.label.CalcSize(new GUIContent(maxed));
				
				GUI.Label(new Rect((i*210) + 16 + 50 - size.x/2f, Screen.height/2f + 220, 100, 100), maxed);
			}
		}
		
		/*if (GUI.Button (new Rect (Screen.width / 2 - 270, Screen.height / 2 -100, 100, 100), (iceCreamBtn))) {
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
		}*/
		
		if (GUI.Button (new Rect(Screen.width-58, Screen.height/2 + 135, 50, 50), backArrow)) {
			Application.LoadLevel(Game.nextLevel);
		}
	}
}	
