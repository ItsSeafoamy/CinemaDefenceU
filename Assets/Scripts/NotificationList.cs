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
using System.Collections.Generic;

public class NotificationList : MonoBehaviour {

	List<Notification> notifications = new List<Notification>();
	public Texture2D notifitexture;
	
	public static NotificationList instance;

	void Start () {
		if (instance == null){
			instance = this;
			DontDestroyOnLoad(gameObject);
		} else {
			Destroy(gameObject);
		}
	}
	
	void Update () {
		for (int i = notifications.Count - 1; i >= 0; i--){
			int j = notifications.Count - i;
			Notification noti = notifications[i];
			
			noti.time -= Time.deltaTime;
			
			if (noti.time <= 0){
				notifications.Remove(noti);
				continue;
			}
			
			int diff = notifitexture.height + 16;
			
			if (noti.y - 4 > Screen.height - (j)*diff) noti.y -= 4;
			else if (noti.y  + 4 < Screen.height - (j)*diff) noti.y += 4;
			else noti.y = Screen.height - (j)*diff;
		}
	}
	
	void OnGUI(){
		for (int i = notifications.Count - 1; i >= 0; i--){
			Notification noti = notifications[i];
			
			Color orig = GUI.color;
			Color colour = GUI.color;
			colour.a = Mathf.Min(noti.time, 1);
			GUI.color = colour;
			
			GUI.DrawTexture(new Rect(Screen.width  - notifitexture.width, noti.y, notifitexture.width, notifitexture.height), notifitexture);
			
			GUI.color = new Color(0, 0, 0, colour.a);
			
			string display = noti.message;
			Vector2 size = GUI.skin.label.CalcSize(new GUIContent(display));
			GUI.Label(new Rect(Screen.width - size.x, noti.y + notifitexture.height / 2 - size.y / 2, size.x, size.y), display);
			GUI.color = orig;
		}
	}
	
	public static void AddNotification(Notification notification){
		instance.notifications.Add(notification);
	}
}
