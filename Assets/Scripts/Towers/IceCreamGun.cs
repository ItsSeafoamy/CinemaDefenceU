﻿/*
*	Copyright (C) 2015 Dylan McCormack, Alexander Prince
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

public class IceCreamGun : Tower {

	public static int currentLevel {get; set;}
	
	public float multipler;
	
	static IceCreamGun() {
		currentLevel = 0;
	}
	
	public override void Fire(){
		Bullet bullet = (Bullet) Instantiate(this.bullet, transform.position, Quaternion.identity);
		bullet.shooter = this;
		Vector3 dir = (target.transform.position - transform.position).normalized;
		bullet.direction = dir;
		
		if (target is TallGuy) { //checks if target is tall guy
			tracked.Remove(target); //stops targeting
			SelectTarget(); //gets new target
		} else { //if not a tallguy effect stands
			bullet.damage = baseDamage;
			bullet.AddEffect(new Slowness(multipler));
			tracked.Remove(target);
			SelectTarget();
		}
	}
}
