using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	private int level;
	private GameManager gmanager;
	private LevelManager lmanager;

	// Logic vars for changing music
	private int lastaggro;

	// These are the objects that are attached to the 
	public AudioSource music;
	public AudioSource sfx;


	public void init (int whichLevel, LevelManager lmanager, GameManager gmanager) {
		this.level = whichLevel;
		this.gmanager = gmanager;
		this.lmanager = lmanager;
	}

	// Use this for initialization
	void Start () {
		lastaggro = lmanager.aggro;

		if (level == 1) {

		}
	}
	
	// Update is called once per frame
	void Update () {
		int delta = lmanager.aggro - lastaggro;
		if (delta > 5) {
			// Add filter here?
		}
		lastaggro = lmanager.aggro;
	}

	void drop() {

	}
}
