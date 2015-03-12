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

public class SkeletonEnemy : Enemy {
	
	protected override void Move (){ //Delete this method to have normal movement
		//code for irregular movement
	}
	
	protected override void Damage (Bullet bullet){ //Delete this method for normal damage control
		//If changing damage, do it before base.Damage(bullet)!
		//Change damage by changing bullet.damage
		
		base.Damage (bullet); //Delete this line to cancel the damage
		//Code for stuff that happens when enemy is damaged
	}
	
	protected override void Kill (){ //Delete this method if nothing special happens when enemy is killed
		base.Kill (); //Delete this line to cancel the death
		
		//Code for stuff that happens when this enemy is killed
	}
}