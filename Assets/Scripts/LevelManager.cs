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

    /*
        About the level construction: the bounding box is a bunch
        of box colliders (no rigidbody). The player should have the only
        other collider without isTrigger set to true, so it's the only
        one that has physics interactions with the bounding box.
        We may want to redo the bounding box with our own code later, but
        it's a decent strategy for now.
    */
    public void init(int whichLevel)
    {
        // setup bounding box
        // setup spawners
        // setup camera?
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
