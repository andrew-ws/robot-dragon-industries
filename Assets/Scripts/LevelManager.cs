using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public Player player;

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
