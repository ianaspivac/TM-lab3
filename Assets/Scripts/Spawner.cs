using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public GameObject[] spawnObjects;
	public float spawnInterval;

	void Start () {

		InvokeRepeating("SpawnObject", Random.Range(0, spawnInterval), spawnInterval  );		
	}

	void SpawnObject(){

		if (Random.value < 0.75){
			transform.position = new Vector3(Random.Range(-3, 3), -2.8f, 1);
			Instantiate( 
				spawnObjects[0],
				transform
			);
		}
	}
	

	void Update () {
		
	}

}
