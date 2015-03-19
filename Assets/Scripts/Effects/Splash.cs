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

public class Splash : Effect {

	float damage;
	float range;
	
	public Splash(float damage, float range){
		this.damage = damage;
		this.range = range;
	}
	
	public void OnApplied(Enemy enemy){
		RaycastHit2D[] hits = Physics2D.CircleCastAll(enemy.transform.position, range, Vector2.zero);
		
		foreach (RaycastHit2D hit in hits){
			if (hit.collider.tag == "Enemy"){
				Enemy e = hit.collider.GetComponent<Enemy>();
				if (e != enemy){
					e.health -= damage;
				}
			}
		}
	}
	
	public void OnUpdate(Enemy enemy){}
	public void OnRemoved(Enemy enemy){}
}