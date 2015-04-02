/*
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
using System.Collections.Generic;
using UnityEditor;

public class Level : MonoBehaviour {

	public static Level instance;
	
	public float scaleX; //How big, in pixels, each square on the invisible grid would be
	public float scaleY;
	
	public Vector2 offset;
	
	bool isPlacing = false; //If we're currently trying to place a tower.
	
	HoloTower holoTower; //The "holotower" is the semi-transparent tower that shows you what tower and where you're placing it
	public HoloTower[] holoTowers;
	
	//public List<int> allowedLanes; //The lanes we can have towers in
	//public int minY, maxY; //The highest and lowest point we can place a tower (So we can't place towers outside the screen)
	
	public List<Vector2> invalidAreas;
	
	List<Tower> placedTowers = new List<Tower>(); //A list of all towers currently on the field.
	
	public Vector2[] upSpawnPoints; //A list of spawnpoints where enemies spawn, then move up
	public Vector2[] rightSpawnPoints; //A list of spawnpoints where enemies spawn, then move right
	public Vector2[] downSpawnPoints; //A list of spawnpoints where enemies spawn, then move down
	public Vector2[] leftSpawnPoints; //A list of spawnpoints where enemies spawn, then move left
	
	bool isSpawning = false;
	public float minTime = 1f;
	public float maxTime = 2f;
	
	public Wave[] waves; //The waves of enemies
	int wave = 0; //Which wave we are currently on
	int enemyNumber;
	bool waiting = true; //If we are currently waiting to advance to the next wave.
	bool waitingForNextLevel = false;
	bool gameOver = false;
	
	public int childPrice, studentPrice, adultPrice;
	public float popularity; //How 'popular' the cinema is. Determines how often someone makes a purchase
	float nextPurchase = 3f; //Time until next purchase
	public int victoryBonus; //How much money we earn for beating the level
	public int investorBonus;
	
	[System.NonSerialized]
	public float happiness = 100f; //Your life
	float displayedHappiness;
	float displayedMoney;
	
	public string nextLevel; //The level to go to after the shop
	
	public Texture2D health;
	public Texture2D money;
	
	Tower selectedTower;
	
	public Texture2D circle;
	
	public AudioClip bgm;
	
	public bool finalLevel;
	
	public bool infiniteMoney, infiniteHealth, unlockAllTowers, maxAllTowers;
	public float timeScale;
	
	IEnumerator SpawnObject(int index, float seconds){
		//make sure they're not all spawning on top of each oter
		yield return new WaitForSeconds(seconds);
		
		if (waves[wave][index] is Boss){ //If the enemy is a boss
			Boss boss = (Boss) waves[wave][index];
			
			AudioClip clip = boss.introClip[Random.Range(0, boss.introClip.Length - 1)];
			
			GetComponent<AudioSource>().PlayOneShot(clip); //Play that bosses intro sound before we spawn it
			yield return new WaitForSeconds(clip.length); //Wait until it shuts up before spawning
		} 
		
		if (waves[wave][index] != null && !gameOver){
			System.Random rand = new System.Random();
			
			Enemy toSpawn = waves[wave][index];
			Vector2 point = new Vector2();
			
			if (toSpawn.direction == Enemy.UP){
				point = upSpawnPoints[rand.Next(upSpawnPoints.Length)];
			} else if (toSpawn.direction == Enemy.RIGHT){
				point = rightSpawnPoints[rand.Next(rightSpawnPoints.Length)];
			} else if (toSpawn.direction == Enemy.DOWN){
				point = downSpawnPoints[rand.Next(downSpawnPoints.Length)];
			} else if (toSpawn.direction == Enemy.LEFT){
				point = leftSpawnPoints[rand.Next(leftSpawnPoints.Length)];
			}
			
			Enemy enemy = (Enemy) Instantiate(toSpawn, GridPosToTransformPos(point), transform.rotation);
			enemy.transform.localScale = new Vector3(70 / 64f, 70 / 64f, 1);
		}
		
		isSpawning = false;
	}
	
	void Start(){
		instance = this;
		
		displayedMoney = Game.money;
		displayedHappiness = happiness;
		
		bool cheatsEnabled = false;
		
		if (infiniteMoney){
			Game.money = System.Int32.MaxValue;
			cheatsEnabled = true;
		}
		if (infiniteHealth){
			happiness = 10000000;
			cheatsEnabled = true;
		}
		if (unlockAllTowers){
			foreach (System.Type t in Game.towers){
				t.GetProperty("currentLevel").SetValue(null, 1, null);
			}
			cheatsEnabled = true;
		}
		if (maxAllTowers){
			foreach (System.Type t in Game.towers){
				t.GetProperty("currentLevel").SetValue(null, 3, null);
			}
			cheatsEnabled = true;
		}
		
		if (timeScale != 1 && timeScale != 0){
			cheatsEnabled = true;
		}
		
		if (cheatsEnabled){
			NotificationList.AddNotification(new Notification("Warning!\nCheats are enabled\nDid you forget to\ndisable them?", 20));
		}
				
		Music.Change(bgm);
	}
	
	void Update(){
		Time.timeScale = timeScale == 0 ? 1 : timeScale;
		if (Random.Range(0, 777776) == 0){
			NotificationList.AddNotification(new Notification("Easter Egg!\nThere was a\n1 in 777,777 chance\nof this appearing\nLucky You!", 5));
		}
		if (!waiting && !waitingForNextLevel && !gameOver){
			if (nextPurchase <= 0){
				switch (Random.Range(0, 4)){
				case 0:
					int sales = Random.Range(1,2);
					int gained = adultPrice * sales;
					Game.money += gained;
					NotificationList.AddNotification(new Notification(sales + " adult ticket" + (sales == 2 ? "s" : "") + " sold for " + gained + "G!", 2));
					break;
				case 1:
					int sales1 = Random.Range(1, 4);
					int gained1 = adultPrice * sales1;
					Game.money += gained1;
					NotificationList.AddNotification(new Notification(sales1 + " adult ticket" + (sales1 == 2 ? "s" : "") + " sold for " + gained1 + "G!", 2));
					break;
				case 2:
					int children = Random.Range(1, 3);
					int adult = Random.Range(1, 2);
					int gained2 = (adultPrice * adult) + (childPrice * children);
					Game.money += gained2;
					NotificationList.AddNotification(new Notification(adult + " adult ticket" + (adult == 2 ? "s" : "") + " and\n" + children + " child ticket" + (children == 2 ? "s" : "") + " sold for " + gained2 + "G!", 2));
					break;
				case 3:
					int students = Random.Range(1,2);
					int gained3 = studentPrice * students;
					Game.money += gained3;
					NotificationList.AddNotification(new Notification(students + " student ticket" + (students == 2 ? "s" : "") + "\nsold for " + gained3 + "G!", 2));
					break;
				case 4:
					int students2 = Random.Range(1, 6);
					int gained4 = adultPrice * students2;
					Game.money += gained4;
					NotificationList.AddNotification(new Notification(students2 + " student ticket" + (students2 == 2 ? "s" : "") + " sold for " + gained4 + "G!", 2));
					break;
				case 5:
					int childs = Random.Range(3, 10);
					int gained5 = childPrice * childs;
					Game.money += gained5;
					NotificationList.AddNotification(new Notification(childs + " student ticket" + (childs == 2 ? "s" : "") + " sold for " + childs + "G! ", 2));
					break;
				}
				
				nextPurchase = 10 - (popularity * Random.Range(0.5f, 1));
			} else {
				nextPurchase -= Time.deltaTime;
			}
		}
		
		//check if spawned and if possible to spawn
		if(!isSpawning && !waiting && !waitingForNextLevel && !gameOver){
			if (enemyNumber < waves[wave].enemies.Length){
				isSpawning = true; 
				//int enemyIndex = Random.Range(0, waves[wave].enemies.Length);
				StartCoroutine(SpawnObject(enemyNumber, Random.Range(minTime, maxTime)));
				enemyNumber++;
			} else {
				if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0){ //No more enemies, wave has been defeated
					if (wave < waves.Length - 1){
						wave++;
						enemyNumber = 0;
						waiting = true;
						NotificationList.AddNotification(new Notification("Wave Cleared\nPress ENTER to continue\nto the next wave", 5));
					} else {
						if (Game.money < 0){
							NotificationList.AddNotification(new Notification("You did not make enough\nmoney and have went\nbankrupt", 10));
							gameOver = true;
						} else if (happiness > 0 && !finalLevel){
							Game.money += victoryBonus;
							NotificationList.AddNotification(new Notification("Congratulations!\n Here's " + victoryBonus + "G\nfor your hard work", 10));
							waitingForNextLevel = true;
						} else if (happiness > 0 && finalLevel){
							Application.LoadLevel("Victory");
						}
						
						if (holoTower != null){
							Destroy(holoTower.gameObject);
						}
					}
				}
			}
		}
		
		if (happiness <= 0 && !gameOver){
			happiness = 0;
			gameOver = true;
			NotificationList.AddNotification(new Notification("Game Over!\nYour customers are\nunhappy and have all left", 10));
			
			if (holoTower != null){
				Destroy(holoTower.gameObject);
			}
			
			foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")){
				Destroy(enemy);
			}
		}
		
		if (waiting && Input.GetKeyDown(KeyCode.Return)){
			waiting = false;
		} 
		
		if (waitingForNextLevel && Input.GetKeyDown(KeyCode.Return)){
			Game.nextLevel = nextLevel;
			Game.investorBonus = investorBonus;
			Application.LoadLevel("Shop");
		}
		
		if (gameOver && Input.GetKeyDown(KeyCode.Return)){
			Application.LoadLevel("GameOver");
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha1)){
			if (holoTower != null){ //If there is already a holoTower, destroy it
				Destroy(holoTower.gameObject);
			}
			
			if (holoTowers[0].toSpawn[0].getLevel() > 0){
				holoTower = (HoloTower) Instantiate(holoTowers[0]); //Create the new holotower
				holoTower.transform.localScale = new Vector3(70 / 64f, 70 / 64f, 1);
			
				isPlacing = true; //We are now placing a tower
			} else {
				isPlacing = false;
			}
		} else if (Input.GetKeyDown(KeyCode.Alpha2)){
			if (holoTower != null){
				Destroy(holoTower.gameObject);
			}
			
			if (holoTowers[1].toSpawn[0].getLevel() > 0){
				holoTower = (HoloTower) Instantiate(holoTowers[1]); //Create the new holotower
				holoTower.transform.localScale = new Vector3(70 / 64f, 70 / 64f, 1);
				
				isPlacing = true; //We are now placing a tower
			} else {
				isPlacing = false;
			}
		} else if (Input.GetKeyDown(KeyCode.Alpha3)){
			if (holoTower != null){
				Destroy(holoTower.gameObject);
			}
			
			if (holoTowers[2].toSpawn[0].getLevel() > 0){
				holoTower = (HoloTower) Instantiate(holoTowers[2]); //Create the new holotower
				holoTower.transform.localScale = new Vector3(70 / 64f, 70 / 64f, 1);
				
				isPlacing = true; //We are now placing a tower
			} else {
				isPlacing = false;
			}
		} else if (Input.GetKeyDown(KeyCode.Alpha4)){
			if (holoTower != null){
				Destroy(holoTower.gameObject);
			}
			
			if (holoTowers[3].toSpawn[0].getLevel() > 0){
				holoTower = (HoloTower) Instantiate(holoTowers[3]); //Create the new holotower
				holoTower.transform.localScale = new Vector3(70 / 64f, 70 / 64f, 1);
				
				isPlacing = true; //We are now placing a tower
			} else {
				isPlacing = false;
			}
		} else if (Input.GetKeyDown(KeyCode.Alpha5)){
			if (holoTower != null){
				Destroy(holoTower.gameObject);
			}
			
			if (holoTowers[4].toSpawn[0].getLevel() > 0){
				holoTower = (HoloTower) Instantiate(holoTowers[4]); //Create the new holotower
				holoTower.transform.localScale = new Vector3(70 / 64f, 70 / 64f, 1);
				
				isPlacing = true; //We are now placing a tower
			} else {
				isPlacing = false;
			}
		} else if (Input.GetKeyDown(KeyCode.Escape)){
			if (holoTower != null){
				Destroy(holoTower.gameObject);
			}
			
			isPlacing = false; //No longer placing towers
		} 
		
		if (isPlacing && !waitingForNextLevel && !gameOver){
			Vector2 point = ScreenPosToGridPos();
			Vector3 transPos = GridPosToTransformPos(point);
			
			holoTower.transform.position = transPos; //Move the holo tower to the nearest snap-point to the mouse
			
			bool enemyLane = false;
			
			foreach (Vector2 v in upSpawnPoints){
				if (v.x == point.x){
					enemyLane = true;
					break;
				}
			}
			
			foreach (Vector2 v in downSpawnPoints){
				if (v.x == point.x){
					enemyLane = true;
					break;
				}
			}
			
			foreach (Vector2 v in leftSpawnPoints){
				if (v.y == point.y){
					enemyLane = true;
					break;
				}
			}
			
			foreach (Vector2 v in rightSpawnPoints){
				if (v.y == point.y){
					enemyLane = true;
					break;
				}
			}
			
			if (holoTower.toSpawn[holoTower.toSpawn[0].getLevel()-1].buy <= Game.money && transPos.x > -5.4f + (scaleX/200f) && transPos.x < 4.5f - 0.9f - (scaleX/200f)&& transPos.y > -3f + (scaleY/200f) && transPos.y < 3f - (scaleY/200f)
				&& !invalidAreas.Contains(new Vector2(point.x, point.y)) && !enemyLane){ 
				
				if (Input.GetButtonUp("Fire1")){ //When the mouse button is pressed
					Tower tower = GetTower(holoTower.transform.position);
					
					if (tower == null){
						Tower t = (Tower) Instantiate(holoTower.toSpawn[holoTower.toSpawn[0].getLevel()-1], holoTower.transform.position, holoTower.transform.rotation); //Spawn the tower
						t.transform.localScale = new Vector3(70 / 64f, 70 / 64f, 1);
						t.GetComponent<SpriteRenderer>().sortingOrder = (int) -t.transform.position.y;
						t.tile = point;
						
						placedTowers.Add(t);
						Game.money -= t.buy;
						
						selectedTower = t;
						GetComponent<AudioSource>().Stop();
						AudioClip playSound = holoTower.placeSounds[Random.Range(0, holoTower.placeSounds.Length - 1)];
						GetComponent<AudioSource>().PlayOneShot(playSound);
					} else {
						selectedTower = tower;
					}
				}
				
				holoTower.GetComponent<SpriteRenderer>().sprite = holoTower.valid; //Change the sprite to the "valid" sprite to show the player they can place the tower here
			} else {				
				holoTower.GetComponent<SpriteRenderer>().sprite = holoTower.invalid; //Change the sprite to the "invalid" sprite
				
				if (Input.GetButtonUp("Fire1") && Input.mousePosition.x < Screen.width - 180){
					Vector3 pos = GridPosToTransformPos(ScreenPosToGridPos());
					if (GetTower(pos) != null){
						selectedTower = GetTower(pos);
					}
				}
			}
		} else if (!waitingForNextLevel && !gameOver){
			if (Input.GetButtonUp("Fire1") && Input.mousePosition.x < Screen.width - 180){
				selectedTower = GetTower(GridPosToTransformPos(ScreenPosToGridPos()));
			}
		}
		
		if ((int) displayedHappiness < (int) happiness) displayedHappiness++;
		else if ((int) displayedHappiness > (int) happiness) displayedHappiness--;
		else displayedHappiness = happiness;
		
		if ((int) displayedMoney < (int) Game.money) displayedMoney++;
		else if ((int) displayedMoney > (int) Game.money) displayedMoney--;
		else displayedMoney = Game.money;
	}
	
	/**
	*	Gets the tower at the specified position
	*	Or null, if the space is empty.
	*/
	Tower GetTower(Vector3 position){
		foreach (Tower t in placedTowers){
			if (t.transform.position == position){
				return t;
			}
		}
		
		return null;
	}
	
	public Vector2 ScreenPosToGridPos(){
		Vector3 point3 = Input.mousePosition; //Gets the position of the mouse on the screen
		Vector2 point2 = new Vector2(point3.x - (Screen.width / 2f), point3.y - (Screen.height / 2f)); //Convert to a 2D point with the origin in the center of the screen
		
		point2 += offset;
		
		point2.x /= scaleX; //Divide by the scales
		point2.y /= scaleY;
		
		float sin = Mathf.Sin(Mathf.PI / 4f);
		float cos = Mathf.Cos(Mathf.PI / 4f);
		float x = (point2.x * cos) + (point2.y * -sin); //Rotate the point by 45 degrees counter-clockwise
		float y = (point2.x * sin) + (point2.y * cos);
		
		return new Vector2(Mathf.Floor(x), Mathf.Floor(y)); //Round the values down to snap to grid
	}
	
	public Vector3 GridPosToTransformPos(Vector2 pos){
		float sin = Mathf.Sin(-Mathf.PI / 4f);
		float cos = Mathf.Cos(-Mathf.PI / 4f);
		float x = ((pos.x + 0.5f) * cos) + ((pos.y + 0.5f) * -sin); //Rotate point by 45 degrees clockwise
		float y = ((pos.x + 0.5f) * sin) + ((pos.y + 0.5f) * cos);
		
		return new Vector3((x * scaleX - offset.x) / 100f, (y * scaleY - offset.y) / 100f, 0);
	}
	
	public Vector2 TransformPosToGridPos(Vector3 pos){
		Vector2 point = new Vector2((pos.x * 100 + offset.x) / scaleX, (pos.y * 100 + offset.y) / scaleY);
		Debug.Log(pos.x + ", " + pos.y + ", " + pos.z + " --> " + point.x + ", " + point.y);
		
		float sin = Mathf.Sin(Mathf.PI / 4f);
		float cos = Mathf.Cos(Mathf.PI / 4f);
		float x = (point.x * cos) + (point.y * -sin); //Rotate the point by 45 degrees counter-clockwise
		float y = (point.x * sin) + (point.y * cos);
		
		return new Vector2(Mathf.Floor(x), Mathf.Floor(y)); //Round the values down to snap to grid
	}
	
	void OnGUI(){
		GUI.DrawTexture(new Rect(Screen.width - 128, 32, health.width / 2, health.height / 2), health);
		GUI.DrawTexture(new Rect(Screen.width - 128, 32 + health.height + 8, money.width / 2, money.height), money);
		
		GUI.Label(new Rect(Screen.width - 128 + health.width/2 + 8, 32, 200, 20), (int)displayedHappiness + "");
		GUI.Label(new Rect(Screen.width - 128 + health.width/2 + 8, 32 + health.height + 8, 200, 20), displayedMoney + "G");
		
		if (selectedTower != null){
			
			selectedTower.targetMode = GUI.SelectionGrid(new Rect(Screen.width - 128, 160, 169, 24*3), selectedTower.targetMode, new string[]{"First Spotted", "Strongest", "Weakest"}, 1, EditorStyles.radioButton);
			
			if (GUI.Button(new Rect(Screen.width - 170, 168 + (24*3), 160, 24), "Sell for " + selectedTower.sell + "G")){
				Game.money += selectedTower.sell;
				NotificationList.AddNotification(new Notification("Sold tower for " + selectedTower.sell + "G", 2));
				placedTowers.Remove(selectedTower);
				Destroy(selectedTower.gameObject);
				selectedTower = null;
			}
		}
	}
}
