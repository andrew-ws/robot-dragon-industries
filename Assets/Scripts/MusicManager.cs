using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	private int level;
	private GameManager manager;

	// These are the objects that are attached to the 
	public AudioSource music;
	public AudioSource sfx;


	public void init (int whichLevel, GameManager manager) {
		this.level = whichLevel;
		this.manager = manager;
	}

	// Use this for initialization
	void Start () {
		if (level == 1) {

		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void drop() {

	}
}
