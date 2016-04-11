using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    LevelManager currentLvl;

	// Need these for accessing music objects
	public AudioSource music;
	public AudioSource sfx;
	private AudioClip gametrack;

	private int lastaggro;

	// Use this for initialization
	void Start () {
        currentLvl = (new GameObject()).AddComponent<LevelManager>();
        currentLvl.gameObject.name = "Level 1 Manager";
		currentLvl.init(1); // Passing in for accessing some music objects in the scene
		lastaggro = currentLvl.aggro;

		// Music
		//gametrack = Resources.Load<AudioClip>("Music/03 King for a Day.m4a");
		//PlayMusic (gametrack);

	}
	
	// Update is called once per frame
	void Update () {
		int delta = currentLvl.aggro - lastaggro;
		if (delta > 5) {
			// Add filter here?
		}
		lastaggro = currentLvl.aggro;
	}

	public void PlayMusic(AudioClip clip)
	{
		music.loop = true;
		music.clip = clip;
		music.Play();
	}

	public void PlayEffect(AudioClip clip)
	{
		sfx.clip = clip;
		sfx.Play();
	}

	void drop() {
		
	}

}
