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
	float phase;
	
	int targetLane;
	
	Vector3 startPos;
	
	void Start(){
		startPos = transform.position;
		
		int index = Random.Range(0, Level.instance.upSpawnPoints.Length - 1);
		targetLane = (int) Level.instance.upSpawnPoints[index].x;
	}
	
	void Update () {
		phase += Time.deltaTime / launchTime;
		
		if (phase >= 1){
			Instantiate(toSpawn, new Vector3(((targetLane * Level.instance.scale) / 100f) + 0.25f, startPos.y - launchDistance), Quaternion.identity);
			Destroy(gameObject);
		} else {
			float modifier = (Mathf.Sin(2*(Mathf.PI)*(phase-0.25f)) + 1) / 2.0f;
			transform.localScale = Vector3.one * (modifier + 1); //Change the size to give the illusion of us moving towards and away from the screen
			
			if (phase > 0.5f){
				modifier = -modifier + 2;
			}
			
			modifier = modifier / 2.0f * launchDistance;
			float x = ((targetLane * Level.instance.scale) / 100f) + 0.25f;
			float xMod = (x - startPos.x) * phase;
			
			transform.position = new Vector3(startPos.x + xMod, startPos.y - modifier, startPos.z);
		}
	}
}