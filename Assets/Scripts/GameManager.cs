using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    LevelManager currentLvl;

	// Use this for initialization
	void Start () {
        currentLvl = (new GameObject()).AddComponent<LevelManager>();
        currentLvl.gameObject.name = "Level 1 Manager";
        currentLvl.init(1);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
