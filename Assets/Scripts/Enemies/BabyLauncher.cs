using UnityEngine;
using System.Collections;

public class BabyLauncher : MonoBehaviour {
	
	public Baby toSpawn;
	
	public float launchTime;
	float phase;
	
	void Update () {
		phase += Time.deltaTime / launchTime;
		
		if (phase >= 1){
			Instantiate(toSpawn);
			Destroy(gameObject);
		} else {
			
		}
	}
}