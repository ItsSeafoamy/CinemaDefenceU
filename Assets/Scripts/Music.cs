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

public class Music : MonoBehaviour {
	
	AudioClip next;
	
	public static Music instance;
	
	// Use this for initialization
	void Start () {
		if (instance == null){
			DontDestroyOnLoad(gameObject);
			GetComponent<AudioSource>().loop = true;
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (next != GetComponent<AudioSource>().clip){
			if (GetComponent<AudioSource>().volume > 0){
				GetComponent<AudioSource>().volume -= 0.01f;
			} else {
				GetComponent<AudioSource>().clip = next;
				GetComponent<AudioSource>().Play();
			}
		} else if (GetComponent<AudioSource>().volume < 1){
			GetComponent<AudioSource>().volume += 0.01f;
		}
	}
	
	public static void Change(AudioClip neew){
		if (instance.GetComponent<AudioSource>().clip != neew){
			instance.next = neew;
		}
	}
}