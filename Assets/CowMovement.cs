using UnityEngine;
using System.Collections;

public class CowMovement : MonoBehaviour
{
    private RectTransform image;
    private SkeletonAnimation anim;
    private Vector3 startPosition;
    private Rigidbody2D body;
    private int state = 666;
    // Use this for initialization
    void Start()
    {
        anim = this.GetComponent<SkeletonAnimation>();
        image = this.GetComponent<RectTransform>();
        startPosition = transform.position;
        body = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        //run the poke animation when the left mouse button is clicked

        if (Input.GetMouseButtonDown(0))
        {
            anim.state.SetAnimation(2, "Poke", false);
        }

        //Check to see if the right arrow key is pressed
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightArrow))
        {
            if (state != 3)
            {
                state = 3;
                //anim.state.ClearTrack(1);
                anim.state.SetAnimation(1, "run", true);
            }
            body.velocity = new Vector3(30, 0, 0);
        }
        else if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftArrow))
        {
            if (state != 3)
            {
                state = 3;
                //anim.state.ClearTrack(1);
                anim.state.SetAnimation(1, "run", true);
            }
            body.velocity = new Vector3(-30, 0, 0);
        }
        else if (Input.GetKey("right"))
        {
            //If the state is not currently set to walk (1), setup begins
            if (state != 1)
            {
                state = 1;
                //Face the character forward
                transform.localScale = new Vector3(0.05F, 0.05F, 1F);
                //Set the animation to track 0, set the animation to the walking animation
                // "animation" and then set looping to true
                //anim.state.ClearTrack(1);
                anim.state.SetAnimation(1, "animation", true);
            }
            body.velocity = new Vector3(7, 0, 0);
            //Shift the Transform to the right
            //transform.position += (Vector3.right * Time.time * 0.015F);

            return;
        }
        else if (Input.GetKey("left"))
        {
            if (state != 2)
            {
                state = 2;
                //anim.state.ClearTrack(1);
                transform.localScale = new Vector3(-0.05F, 0.05F, 1F);
                anim.state.SetAnimation(1, "animation", true);

            }
            body.velocity = new Vector3(-7, 0, 0);
            //transform.position += (Vector3.left * Time.time * 0.015F);

            return;
        }

        // If they are not moving, set the animation to Standing, and the state to 0
        else
        {
            if (state != 0)
            {
                state = 0;
               // anim.state.ClearTrack(1);
                //anim.state.SetAnimation(2, "Poke", false);
                anim.state.SetAnimation(1, "Standing", true);

            }
        }

        return;
    }
}
