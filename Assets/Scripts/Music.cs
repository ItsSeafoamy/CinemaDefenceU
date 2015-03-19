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