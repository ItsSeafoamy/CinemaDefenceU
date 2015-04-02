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

/**
*	The main class that holds all persistant data
*/
public class Game {
	
	public static int money = 100; //How much money the player currently has.
	
	public static string nextLevel;
	
	public static int investorBonus;
	
	//Array of all tower types. This means we can easily add or remove towers without having to change too much code
	public static System.Type[] towers = new System.Type[]{
		typeof(PopcornGun),
		typeof(HotdogCannon),
		typeof(IceCreamGun),
		typeof(Coffee),
		typeof(SodaGun)
	};
	
	/**
	*	Resets money and towers. Used when starting a new game
	*/
	public static void Reset(){
		money = 100;
		nextLevel = null;
		
		foreach (System.Type t in towers){
			if (t == typeof(PopcornGun) || t == typeof(HotdogCannon)){
				t.GetProperty("currentLevel").SetValue(null, 1, null); //We start with the popcorn gun and hotdog cannon already unlocked :)
			} else {
				t.GetProperty("currentLevel").SetValue(null, 0, null);
			}
		}
	}
}