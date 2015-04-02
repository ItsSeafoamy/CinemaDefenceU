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
*	
*/
using UnityEngine;
using System.Collections;

public class KidLauncher : MonoBehaviour {
	
	public Kid toSpawn;
	
	public float launchTime, launchDistance;
	[System.NonSerialized]
	public Vector2 start;
	float phase;
	
	int targetLane;
	
	Vector3 startPos;
	Vector2 target;
	
	Vector3 targetPos;
	
	void Start(){
		startPos = transform.position;
		
		int index = Random.Range(0, Level.instance.downSpawnPoints.Length - 1);
		targetLane = (int) Level.instance.downSpawnPoints[index].x;
		
		target = new Vector2(targetLane, start.y - launchDistance);
		targetPos = Level.instance.GridPosToTransformPos(targetPos);
	}
	
	void Update () {
		phase += Time.deltaTime / launchTime;
		
		if (phase >= 1){
			Kid kid = (Kid) Instantiate(toSpawn, targetPos, Quaternion.identity);
			kid.health /= 2f;
			Destroy(gameObject);
		} else {
			float modifier = (Mathf.Sin(2*(Mathf.PI)*(phase-0.25f)) + 1) / 2.0f;
			transform.localScale = Vector3.one * (modifier + 1); //Change the size to give the illusion of us moving towards and away from the screen
			
			if (phase > 0.5f){
				modifier = -modifier + 2;
			}
			
			modifier = modifier / 2.0f;
			
			transform.position = new Vector3(startPos.x + (targetPos.x - startPos.x)*modifier, startPos.y + (targetPos.y - startPos.y)*modifier);
		}
	}
}