using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour {

    public LevelManager lm;
	public int level = 1;

	// Need these for accessing music objects
	public AudioSource[] sources;
	private AudioSource source0;
	private AudioSource source1;
	private AudioSource source2;
	private AudioSource source3;

	private AudioClip nonintense_track;
	private AudioClip drum_track;
	private AudioClip intense_track;

	public AudioMixer master;
	public AudioMixer nonintense_mixer;
	public AudioMixerSnapshot nonintense;
	public AudioMixerSnapshot drums;
	public AudioMixerSnapshot intense;

	bool areDrums = false;

	public bool dropped;

	// Use this for initialization
	void Start () {
        GameObject go = new GameObject();
        lm = go.AddComponent<LevelManager>();
		go.name = "Level " + level + " Manager";
        // Passing in for accessing some music objects in the scene
		lm.init(level, this);

		manageAudio ();

	}
	
	// Update is called once per frame
	void Update () {
		if (lm.dropped && !dropped) {
            dropped = true;
			drop ();
		} else if (lm.dropped == false && dropped) {
			undrop ();
		}

		// TODO: get rid of this debugging command
		if (Input.GetKeyDown(KeyCode.Space)) {
			lm.aggro++;
		}

		if (lm.aggro > 10 && areDrums == false) {
			drums.TransitionTo (5f);
			areDrums = !areDrums;
		} else if (lm.aggro <= 5 && areDrums == true) {
			areDrums = !areDrums;
			nonintense.TransitionTo (5f);
		}

        if (Input.GetKeyDown(KeyCode.J))
            lm.annhilate();
	}

	/*public void PlayMusic(AudioClip clip)
	{
		music.loop = true;
		music.clip = clip;
		music.Play();
	}

	public void PlayEffect(AudioClip clip)
	{
		sfx.clip = clip;
		sfx.Play();
	}*/

	public void drop() {
		dropped = true;
		intense.TransitionTo (0.01f);
        lm.drop();
	}

	public void undrop() {
		dropped = false;
		drums.TransitionTo (0.01f);
		lm.undrop ();
	}

	public void manageAudio() {
		// Music
		intense_track = Resources.Load<AudioClip>("Music/Intense Loop");
		nonintense_track = Resources.Load<AudioClip>("Music/Non-Intense Loop");
		drum_track = Resources.Load<AudioClip>("Music/Drums");

		master = Resources.Load<AudioMixer> ("Music/Master");
		nonintense_mixer = Resources.Load<AudioMixer> ("Music/Non-Intense");
		nonintense = master.FindSnapshot ("Nonintense");
		intense = master.FindSnapshot ("Intense");
		drums = master.FindSnapshot ("Drums");

		sources = gameObject.GetComponents<AudioSource> ();
		source0 = sources [0];
		setAudioSource (source0, nonintense_track, true);
		source1 = sources [1];
		setAudioSource (source1, drum_track, true);
		source2 = sources [2];
		setAudioSource (source2, intense_track, true);

	}

	private void setAudioSource(AudioSource source, AudioClip clip, bool loop) {
		source.clip = clip;
		source.playOnAwake = true;
		source.loop = loop;
	}

	public void resetLevel(int level) {
		print ("reset here"); // TODO Add actual reset
		this.level = level;
		// Call annihilate here
		lm = this.gameObject.AddComponent<LevelManager>();
		this.gameObject.name = "Level " + this.level + " Manager";
		lm.init (level, this);
	
	}
}
