using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    LevelManager currentLvl;


	// Use this for initialization
	void Start () {
        currentLvl = (new GameObject()).AddComponent<LevelManager>();
        currentLvl.gameObject.name = "Level 1 Manager";
		currentLvl.init(1, this); // Passing in for accessing some music objects in the scene

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
