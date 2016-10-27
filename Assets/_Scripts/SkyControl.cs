using UnityEngine;
using System.Collections;
/*Author: Ashley Tjon-Hing
Date: October 17th 2016*/
public class SkyControl : MonoBehaviour {
    //Private Instance Variables
    private Transform _transform;
    public float speed = 0.00001f;

    //Public Instance Variables


    //Public Properties

	// Use this for initialization
	void Start () {
        this._transform = this.GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
        this.Move();
        this.Boundary();
	}

    //Method to move sky
    private void Move()
    {
        Vector2 newPos = this._transform.position; //use old position to create new position
        newPos.x -= (this.speed); //create new pos
        this._transform.position = newPos; //set new pos

        if (Input.GetKey(KeyCode.LeftArrow)) //
        {
            Vector2 newPosition = this.transform.position; //Old position and new position are equal; must use vector(can be temporary) to modify transform components
            newPosition.x -= this.speed; //brings the image downwards every fram
            this._transform.position = newPosition; //resets so that it can continue bringing the image down
        }

        if (Input.GetKey(KeyCode.RightArrow)) //
        {       
                Vector2 newPosition = this.transform.position; //Old position and new position are equal; must use vector(can be temporary) to modify transform components
                newPosition.x -= this.speed; //brings the image up every fram
                this._transform.position = newPosition; //resets so that it can continue bringing the image down

        }

    }

    //Prevent background from going off screen
    private void Boundary()
    {
        if (this._transform.position.x <= -64)
        { this.reset(); }
    }

    //Resets Position
    private void reset()
    {
        this._transform.position = new Vector2(204f, 0.28f);
    }
}
