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

	// Level One clips
	private AudioClip nonintense_track;
	private AudioClip drum_track;
	private AudioClip intense_track;

	// Level 2 clips
	private AudioClip melody_clip_2;
	private AudioClip chords_clip_2;
	private AudioClip drums_clip_2;
	private AudioClip arpeggios_clip_2;
	private AudioClip drop_clip_2;

	public AudioMixer master_1;
	public AudioMixer master_2;
	public AudioMixer nonintense_mixer;
	public AudioMixerSnapshot nonintense;
	public AudioMixerSnapshot drums;
	public AudioMixerSnapshot intense;
	public AudioMixerSnapshot arpeggios;

	bool areDrums = false;
	bool areArps = false;

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

		if (level == 1) {
			if (lm.aggro > 5 && areDrums == false) {
				drums.TransitionTo (2f);
				areDrums = true;
			} else if (lm.aggro <= 5 && areDrums == true) {
				areDrums = false;
				nonintense.TransitionTo (2f);
			}
		} else if (level == 2) {
			if (lm.aggro <= 3 && areDrums == true) {
				areDrums = false;
				areArps = false;
				nonintense.TransitionTo (2f);
			} else if (lm.aggro > 3 && lm.aggro <= 6 && areDrums == false) {
				areDrums = true;
				areArps = false;
				drums.TransitionTo (2f);
			} else if (lm.aggro > 6 && areArps == false) {
				areArps = true;
				arpeggios.TransitionTo (2f);
			}
		}

        if (Input.GetKeyDown(KeyCode.J))
            lm.annihilate();
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
		if (level == 1) {
			drums.TransitionTo (0.01f);
		} else if (level == 2) {
			arpeggios.TransitionTo (0.01f);
		}
		lm.undrop ();
	}

	public void manageAudio() {

		sources = gameObject.GetComponents<AudioSource> ();
		master_1 = Resources.Load<AudioMixer> ("Music/Level 1/Level 1");
		master_2 = Resources.Load<AudioMixer> ("Music/Level 2/Level 2");

		if (level == 1) {
			nonintense = master_1.FindSnapshot ("Nonintense_1");
			intense = master_1.FindSnapshot ("Intense_1");
			drums = master_1.FindSnapshot ("Drums_1");

			// Music
			intense_track = Resources.Load<AudioClip> ("Music/Level 1/Intense Loop 1");
			nonintense_track = Resources.Load<AudioClip> ("Music/Level 1/Non-Intense Loop 1");
			drum_track = Resources.Load<AudioClip> ("Music/Level 1/Drums 1");

			source0 = sources [0];
			setAudioSource (source0, nonintense_track, true);
			source1 = sources [1];
			setAudioSource (source1, drum_track, true);
			source2 = sources [2];
			setAudioSource (source2, intense_track, true);

			nonintense.TransitionTo (0.01f);

		} else if (level == 2) {
			nonintense = master_2.FindSnapshot ("Base");
			intense = master_2.FindSnapshot ("Drop");
			drums = master_2.FindSnapshot ("Drums");
			arpeggios = master_2.FindSnapshot ("Arpeggio");

			// Music
			melody_clip_2 = Resources.Load<AudioClip> ("Music/Level 2/Level 2 - Melody");
			drums_clip_2 = Resources.Load<AudioClip> ("Music/Level 2/Level 2 - Drums");
			arpeggios_clip_2 = Resources.Load<AudioClip> ("Music/Level 2/Level 2 - Arpeggio");
			chords_clip_2 = Resources.Load<AudioClip> ("Music/Level 2/Level 2 - Chords");
			drop_clip_2 = Resources.Load<AudioClip> ("Music/Level 2/Level 2 - Drop 2");

			setAudioSource (sources [4], melody_clip_2, true);
			setAudioSource (sources [5], chords_clip_2, true);
			setAudioSource (sources [6], drums_clip_2, true);
			setAudioSource (sources [7], arpeggios_clip_2, true);
			setAudioSource (sources [8], drop_clip_2, true);

			nonintense.TransitionTo (0.01f);
		}

	}

	private void setAudioSource(AudioSource source, AudioClip clip, bool loop) {
		source.clip = clip;
		source.Play ();
		source.loop = loop;
	}

	public void resetLevel(int level) {
		this.level = level;
		lm.annihilate ();
        GameObject go = new GameObject();
		lm = go.AddComponent<LevelManager>();
		go.name = "Level " + this.level + " Manager";
		manageAudio ();
		lm.init (level, this);
	
	}
}
