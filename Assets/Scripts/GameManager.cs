using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour {

    public LevelManager lm;
	public int level = 1;

    public bool mainMenu = true;

	// Need these for accessing music objects
	public AudioSource[] sources;
	private AudioSource source0;
	private AudioSource source1;
	private AudioSource source2;
	private AudioSource source3;
	public AudioSource sfx;

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

	// Level 3 clips
	private AudioClip melody_clip_3;
	private AudioClip chords_clip_3;
	private AudioClip drums_clip_3;
	private AudioClip drop_clip_3;

	// Mixers
	private AudioMixer master_1;
	private AudioMixer master_2;
	private AudioMixer master_3;
	private AudioMixerSnapshot nonintense;
	private AudioMixerSnapshot drums;
	private AudioMixerSnapshot intense;
	private AudioMixerSnapshot arpeggios;

	// Sounds
	public AudioClip enemyHit;
	public AudioClip enemyHitVoice;
	public AudioClip enemyHitCow;
	public AudioClip playerHit;
	public AudioClip menu;
	public AudioClip money;
	public AudioClip pickup;
	public AudioClip throwPaper;

	bool areDrums = false;
	bool areArps = false;

	public bool dropped;

    public GameObject menuImg;

    bool isStyled = false;

	// Use this for initialization
	void Start () {
        loadMainMenu();
		manageMusic ();
		manageSounds ();
	}
	
	// Update is called once per frame
	void Update () {
        if (mainMenu) return;
		if (lm.dropped && !dropped) {
            dropped = true;
			drop ();
		} else if (lm.dropped == false && dropped) {
			undrop ();
		}

		if (level == 1 || level == 3) {
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

        // dev override
        if(Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.D) &&
            Input.GetKey(KeyCode.I))
        {
            PlayerPrefs.DeleteAll();
        }
	}

	public void PlayEffect(AudioClip clip)
	{
		sfx.clip = clip;
		sfx.loop = false;
		sfx.Play();
	}

	public void drop() {
		dropped = true;
		intense.TransitionTo (0.01f);
        lm.drop();
	}

	public void undrop() {
		dropped = false;
		if (level == 1 || level == 3) {
			drums.TransitionTo (0.01f);
		} else if (level == 2) {
			arpeggios.TransitionTo (0.01f);
		}
		lm.undrop ();
	}

	public void manageMusic() {

		sources = gameObject.GetComponents<AudioSource> ();
		master_1 = Resources.Load<AudioMixer> ("Music/Level 1/Level 1");
		master_2 = Resources.Load<AudioMixer> ("Music/Level 2/Level 2");
		master_3 = Resources.Load<AudioMixer> ("Music/Level 3/Level 3");

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

			for (int i = 4; i < 13; i++) {
				sources [i].Stop ();
			}

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
			for (int i = 0; i < 3; i++) {
				sources [i].Stop ();
			}
			for (int i = 9; i < 13; i++) {
				sources [i].Stop ();
			}

			nonintense.TransitionTo (0.01f);
		} else if (level == 3) {
			nonintense = master_3.FindSnapshot ("Base");
			intense = master_3.FindSnapshot ("Drop");
			drums = master_3.FindSnapshot ("Drums");

			// Music
			melody_clip_3 = Resources.Load<AudioClip> ("Music/Level 3/Level 3 - Melody");
			drums_clip_3 = Resources.Load<AudioClip> ("Music/Level 3/Level 3 - Drums");
			chords_clip_3 = Resources.Load<AudioClip> ("Music/Level 3/Level 3 - Chords");
			drop_clip_3 = Resources.Load<AudioClip> ("Music/Level 3/Level 3 - Drop 1");

			setAudioSource (sources [9], melody_clip_3, true);
			setAudioSource (sources [10], chords_clip_3, true);
			setAudioSource (sources [11], drums_clip_3, true);
			setAudioSource (sources [12], drop_clip_3, true);

			for (int i = 0; i < 9; i++) {
				sources [i].Stop ();
			}

			nonintense.TransitionTo (0.01f);

		}

	}

	private void manageSounds() {
		// Enemy sounds
		enemyHit = Resources.Load<AudioClip> ("Music/Sounds/Enemy Hit");
		enemyHitCow = Resources.Load<AudioClip> ("Music/Sounds/Enemy Hit - Cow");
		enemyHitVoice= Resources.Load<AudioClip> ("Music/Sounds/Enemy Hit - Voice");

		// Player Sounds
		pickup = Resources.Load<AudioClip> ("Music/Sounds/Paper Pick Up - 1");
		throwPaper = Resources.Load<AudioClip> ("Music/Sounds/Paper Throw - 1");
		playerHit = Resources.Load<AudioClip> ("Music/Sounds/Player Hit");

		// UI Sounds
		menu = Resources.Load<AudioClip> ("Music/Sounds/Menu Navigation - 1");
		money = Resources.Load<AudioClip> ("Music/Sounds/Money Sound - 1");
 	}

	private void setAudioSource(AudioSource source, AudioClip clip, bool loop) {
		source.clip = clip;
		source.Play ();
		source.loop = loop;
	}

	public void resetLevel(int level) {
        mainMenu = false;
		this.level = level;
		if (lm != null) lm.annihilate ();
        GameObject go = new GameObject();
		lm = go.AddComponent<LevelManager>();
		go.name = "Level " + this.level + " Manager";
		manageMusic ();
		nonintense.TransitionTo (0.01f);
		lm.init (level, this);
	}

    public void loadMainMenu()
    {
        if (lm != null) lm.annihilate();
        mainMenu = true;
        
        Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        cam.orthographicSize = (LevelManager.rdWidth / cam.aspect) / 2;
        cam.transform.position = new Vector3(0,0,-10);
        float backgroundHt = LevelManager.rdWidth * (7f / 8f);
        float backgroundy = ((cam.orthographicSize * 2) - backgroundHt) / 2;

        menuImg = new GameObject();
        SpriteRenderer sr = menuImg.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/title");
        menuImg.transform.position = new Vector3(0, backgroundy, 0);
        menuImg.name = "Menu";
        
    }

    public void unloadMenu()
    {
        Destroy(menuImg);
    }

    void OnGUI()
    {
        if (!isStyled)
        {
            GUIStyle button = GUI.skin.button;
            button.normal.textColor = Color.yellow;
            button.fontSize = 18;
        }

        if (mainMenu)
        {
            if ((GUI.Button(new Rect((Screen.width / 5)-100, 25, 200, 50), "Level 1")))

            {
                resetLevel(1);
                PlayEffect(menu);
                unloadMenu();
            }
            if (GUI.Button(new Rect((Screen.width / 5)*2-100, 25, 200, 50), "Level 2"))
            {
                resetLevel(2);
                PlayEffect(menu);
                unloadMenu();
            }
            if (GUI.Button(new Rect((Screen.width / 5)*3-100, 25, 200, 50), "Level 3"))
            {
                resetLevel(3);
                PlayEffect(menu);
                unloadMenu();
            }
            if (GUI.Button(new Rect((Screen.width / 5)*4-100, 25, 200, 50), "Quit"))
            {
                Application.Quit();
            }
        }
    }
}
