using UnityEngine;
using System.Collections;
/*Author: Ashley Tjon-Hing
Date: October 17th 2016*/
public class SkyControl : MonoBehaviour {
    //Private Instance Variables
    private Transform _transform;
    public int speed = 1;

    //Public Instance Variables


    //Public Properties
    public int Speed {
        get { return this.speed; }
        set { this.speed = value; }
    } 

	// Use this for initialization
	void Start () {
        this._transform = this.GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        StartCoroutine(Delay());
        this.Boundary();
	}

    IEnumerator Delay()
    {
        this.Move();
        yield return new WaitForSeconds(5*Time.deltaTime);
        this.Move();
    }

    //Method to move sky
    private void Move()
    {
        Vector2 newPos = this._transform.position; //use old position to create new position
        newPos.x -= (this.Speed); //create new pos
        this._transform.position = newPos; //set new pos
    }

    //Prevent background from going off screen
    private void Boundary()
    {
        if (this._transform.position.x <= -18.6)
        { this.reset(); }
    }

    private void reset()
    {
        this._transform.position = new Vector2(18.6f, 0.28f);
    }
}
