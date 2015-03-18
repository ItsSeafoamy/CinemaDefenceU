/*
*	Copyright (C) 2015 Dylan McCormack
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
	public Texture2D upgradeArrow;
	public Texture2D backArrow;
	
	public TowerSet[] towers;
	
	void OnGUI(){
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);
		
		for (int i = 0; i < towers.Length; i++){
			TowerSet towerset = towers[i];
			int level = (int) towerset[0].GetType().GetProperty("currentLevel").GetValue(null, null);
			
			Tower current = level == 0 ? null : towerset[level - 1];
			Tower next = level == towerset.towers.Length ? null : towerset[level];
			
			if (next != null){
				GUI.Button(new Rect(32 + (169*i), Screen.height - 169, 64, 64), next.GetComponent<SpriteRenderer>().sprite.texture);
			}
		}
	}
}