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

public class KillZone : MonoBehaviour {
	
	void OnTriggerEnter2D(Collider2D collider){
		if (collider.tag == "Enemy"){
			Enemy enemy = collider.GetComponent<Enemy>();
			Level.instance.happiness -= enemy.health;
			
			if (Level.instance.happiness <= 0){
				//TODO: Lose the game
				Debug.Log("You lost :(");
			}
						
			float refunds = enemy.health * 5;
			
			if (Game.money >= 0 && Game.money - refunds < 0){
				NotificationList.AddNotification(new Notification("You are now in debt!\nIf you do not get above 0G\nbefore the movie ends,\nYou will be shut down!", 10));
			}
			    
			    
			Game.money -= (int) refunds;
			NotificationList.AddNotification(new Notification("You lost " + (int) refunds + " from refunds!", 2));
			
			foreach (Tower tower in enemy.watchers){
				tower.tracked.Remove(enemy);
				tower.SelectTarget();
			}
			
			Destroy(enemy.gameObject);
		}
	}
}