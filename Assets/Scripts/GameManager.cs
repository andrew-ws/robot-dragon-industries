using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public LevelManager lm;

	// Need these for accessing music objects
	public AudioSource music;
	public AudioSource sfx;

	private AudioClip intense;
	private AudioClip nonintense;

	private int lastaggro;

	// Use this for initialization
	void Start () {
        GameObject go = new GameObject();
        lm = go.AddComponent<LevelManager>();
        go.name = "Level 1 Manager";
		lm.init(1, this); // Passing in for accessing some music objects in the scene
		lastaggro = lm.aggro;

		// Music
		intense = Resources.Load<AudioClip>("Music/Intense Loop");
		nonintense = Resources.Load<AudioClip>("Music/Non-Intense Loop");
		PlayMusic (nonintense);

	}
	
	// Update is called once per frame
	void Update () {
		if (lm.aggro > 20) {
			drop ();
		}
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

	public void drop() {
		PlayMusic (intense);
	}

}
