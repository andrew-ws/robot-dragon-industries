using UnityEngine;
using System.Collections;

public class PlotModel : MonoBehaviour {

    private int type;
    private Plot owner;
    private Material mat;

    public void init(int type, Plot owner)
    {
        this.type = type;
        this.owner = owner;

        transform.parent = owner.transform;

        System.Random rnd = new System.Random();

        mat = GetComponent<Renderer>().material;
        mat.shader = Shader.Find("Sprites/Default");
        mat.color = new Color(1, 1, 1);

        if (type == 0)
        {
            mat.mainTexture = Resources.Load<Texture2D>("Textures/house1");
            transform.localPosition = new Vector3(0, 0, 0);
            gameObject.tag = "house";
        }
        else if (type == 1)
        {
            mat.mainTexture = Resources.Load<Texture2D>("Textures/mailbox");
            transform.localPosition = new Vector3(0, 0, 0);
            gameObject.tag = "mailbox";
        }
        else if (type == 2)
        {
            mat.mainTexture = Resources.Load<Texture2D>("Textures/fenceWood1");
            transform.localPosition = new Vector3(0, 0, 0);
            gameObject.tag = "scenery";
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
