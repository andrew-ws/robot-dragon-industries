using UnityEngine;
using System.Collections;

public class PlotModel : MonoBehaviour {

    private int type;
    private Plot owner;
    protected SpriteRenderer sr;
    private int boxNum; //number for mailbox texture to make changing it easier

    public void init(int type, Plot owner)
    {
        this.type = type;
        this.owner = owner;

        transform.parent = owner.transform;

        sr = GetComponent<SpriteRenderer>();

        if (type == 0)
        {
            sr.sprite = Resources.Load<Sprite>("Sprites/house" + Random.Range(1,2));
            transform.localPosition = new Vector3(0, 0, 0);
            gameObject.name = "house";
        }
        else if (type == 1)
        {
            sr.sprite = Resources.Load<Sprite>("Sprites/fenceWood1");
            transform.localPosition = new Vector3(0, 0, 0);
            gameObject.name = "scenery";
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
}
