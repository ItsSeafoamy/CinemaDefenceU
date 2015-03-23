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
using System.Collections.Generic;

public abstract class Enemy : MonoBehaviour {

	public float health = 5; //The health of this enemy
	float maxHealth;
	float displayedHealth;
	public float movementSpeed = 10; //How fast this enemy moves
	
	[System.NonSerialized]
	public List<Tower> watchers = new List<Tower>(); //The towers that are currently tracking this enemy
	
	[System.NonSerialized]
	public List<Effect> effects = new List<Effect>(); //A list of effects applied to this tower.
	
	public Vector2 direction;
	public static readonly Vector2 UP = new Vector2(1, 1);
	public static readonly Vector2 RIGHT = new Vector2(1, -1);
	public static readonly Vector2 DOWN = new Vector2(-1, -1);
	public static readonly Vector2 LEFT = new Vector2(-1, 1);
	
	void Start(){
		maxHealth = health;
		displayedHealth = health;
	}
		
	// Update is called once per frame
	void Update (){
		foreach (Effect e in effects){
			e.OnUpdate(this); //Update any effects
		}
		
		Move(); //Move this enemy
		
		if (displayedHealth > health + 0.5f) displayedHealth -= 0.5f;
		else if (displayedHealth < health - 0.5f) displayedHealth += 0.5f;
		else displayedHealth = health;
		
		Transform healthBar = transform.FindChild("Health Bar");
		healthBar.localScale = new Vector3(20f / maxHealth * displayedHealth, 4, 1);
		if(health <= 0){
			Kill(); //Kill if health <= 0
		}
	}

	void OnCollisionEnter2D(Collision2D hit){		
		if (hit.gameObject.tag == "Bullet"){
			Bullet b = hit.gameObject.GetComponent<Bullet>();
			
			foreach (Effect e in b.effects){
				AddEffect(e); //Add any effects the bullet was carrying
			}
			
			Damage(b); //Damage this enemy
			
			Destroy(b.gameObject); //Destroy the bullet
			
			if (health <= 0){
				Kill(); //Kill if health <= 0
			}
		} else {
			Physics2D.IgnoreCollision(hit.collider, GetComponent<Collider2D>()); //Ignore any collisions from objects that aren't bullets
		}
	}
	
	/**
	*	Called every update to move this enemy
	*	Should be overriden for enemies with irregular movement patterns (e.g. The old lady with the rabbit hopping then stopping)
	*/
	protected virtual void Move(){
		transform.Translate(new Vector3(direction.x, direction.y / 2f) * movementSpeed * Time.deltaTime);
	}
	
	/**
	*	Called when this enemy is damaged.
	*	This should be overriden by enemies that have weaknesses, resistances or some effect that occurs when damaged (e.g. Baby speeding up)
	*/
	protected virtual void Damage(Bullet bullet){
		health -= bullet.damage;
	}
	
	/**
	*	Called when this enemy is destroyed.
	*	This should be overriden if this enemy does something when killed.
	*	For a boss, this could end the level,
	*	Or to spawn the kids when the crazy mum is killed.
	*	Unless for some reason the death is to be cancelled, this base method should be called from overriding methods
	*/
	public virtual void Kill(){
		foreach (Tower t in watchers){
			t.tracked.Remove(this); //Stop towers from tracking this enemy
			t.SelectTarget(); //And update the towers' targets
		}
		
		Destroy(gameObject); //Destroy this enemy
	}
	
	/**
	*	Apply a new effect to this enemy
	*	If callEvent is true, effect.OnApplied will be called
	*/
	public void AddEffect(Effect effect, bool callEvent = true){
		effects.Add(effect);
		
		if (callEvent) effect.OnApplied(this);
	}
	
	/**
	*	Remove an existing effect from this enemy
	*	Does nothing if the effect was not applied
	*	If callEvent is true, effect.OnRemoved will be called
	*/
	public void RemoveEffect(Effect effect, bool callEvent = true){
		if (effects.Contains(effect)){
			effects.Remove(effect);
			
			if (callEvent) effect.OnRemoved(this);
		}
	}
}