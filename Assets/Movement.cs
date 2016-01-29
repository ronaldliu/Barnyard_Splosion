using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {
    private RectTransform image;
    private SkeletonAnimation anim;
    private Vector3 startPosition;
    private Rigidbody2D body;
    private int state = 666;
    private float xVel = 0;
    private float yVel = 0;
	// Use this for initialization
	void Start () {
        anim = this.GetComponent<SkeletonAnimation>();
        image = this.GetComponent<RectTransform>();
        startPosition = transform.position;
        body = this.GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.RightArrow))
        {
            xVel += 0.5F;
            if(xVel > 30)
            {
                xVel = 30;
            }
        }else if (Input.GetKey(KeyCode.LeftArrow))
        {

        }
        body.velocity = new Vector3(xVel, yVel, 0);
        return;
	}
}
