using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public LevelManager lm;

	// Use this for initialization
	void Start () {
        lm = (new GameObject()).AddComponent<LevelManager>();
        lm.gameObject.name = "Level 1 Manager";
        lm.init(1);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
