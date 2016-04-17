using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour {

    public LevelManager lm;

	// Need these for accessing music objects
	public AudioSource music;
	public AudioSource sfx;

	private AudioClip intense_music;
	private AudioClip nonintense_music;

	public AudioMixerSnapshot nonintense;
	public AudioMixerSnapshot drums;
	public AudioMixerSnapshot intense;

	bool areDrums = false;

    private bool dropped;

	// Use this for initialization
	void Start () {
        GameObject go = new GameObject();
        lm = go.AddComponent<LevelManager>();
        go.name = "Level 1 Manager";
        // Passing in for accessing some music objects in the scene
        lm.init(1, this);

        // Music
        /*intense_music = Resources.Load<AudioClip>("Music/Intense Loop");
		nonintense_music = Resources.Load<AudioClip>("Music/Non-Intense Loop");
		PlayMusic (nonintense_music);*/

	}
	
	// Update is called once per frame
	void Update () {
		if (lm.readyForDrop && !dropped) {
            dropped = true;
			drop ();
		}

		if (Input.GetKeyDown(KeyCode.Space)) {
			lm.aggro++;
		}

		if (lm.aggro > 10 && areDrums == false) {
			drums.TransitionTo (5f);
			areDrums = !areDrums;
		} else if (lm.aggro <= 10 && areDrums == true) {
			areDrums = !areDrums;
			nonintense.TransitionTo (5f);
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
		intense.TransitionTo (0.01f);
        lm.drop();
	}
}
