using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public Player player;
	public GameManager manager;
	// I need our main GameManager to be passed along into MusicManager to access the audio
	// that is attached to it

    public int aggro = 1;

	// Use this for initialization
	void Start () {
        this.transform.position = Vector3.zero;

        player = (new GameObject()).AddComponent<Player>();
        player.gameObject.name = "Player";
        player.gameObject.transform.localPosition = Vector3.zero;

	}

	public void init(int whichLevel)
    {}
	
	// Update is called once per frame
	void Update () {
	
	}


}
