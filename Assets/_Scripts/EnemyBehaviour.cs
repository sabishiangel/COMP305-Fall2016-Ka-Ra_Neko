using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

    //Private Instance Variables
    private Transform _transform;
    private Rigidbody2D _rigidbody;
    private bool _isGrounded;
    private bool _GroundAhead;
    private SpriteRenderer spriteRender;
    private bool _isFacingLeft;
    private bool _isPlayerThere;

    //Public Instance Variables
    public float Speed = 5f;
    public float MaxSpeed = 8f;
    public Transform sightStart;
    public Transform sightEnd;
    public Transform playerInSight;
    

	// Use this for initialization
	void Start () {
        this._transform = GetComponent<Transform>();
        this._rigidbody = GetComponent<Rigidbody2D>();
        this._isGrounded = false;
        this._GroundAhead = true;
        this.spriteRender = GetComponent<SpriteRenderer>();
        this._isFacingLeft = true;
        this._isPlayerThere = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if(this._isGrounded)
        {
            this._rigidbody.velocity = new Vector2(this._transform.localScale.x, 0) * this.Speed;

            this._GroundAhead = Physics2D.Linecast(this.sightStart.position, this.sightEnd.position, 1 << LayerMask.NameToLayer("Solid"));

            Debug.DrawLine(this.sightStart.position, this.sightEnd.position);

            this._isPlayerThere = Physics2D.Linecast(this.sightStart.position, this.playerInSight.position, 1 << LayerMask.NameToLayer("Player"));

            Debug.DrawLine(this.sightStart.position, this.playerInSight.position);

            if (this._GroundAhead == false)
            {
                this.flip();
            }

            if(this._isPlayerThere == true)
            {
                this.Speed *= 2;
                if(this.Speed >=MaxSpeed)
                { this.Speed = MaxSpeed; }
            }
        }
        
	}

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        { this._isGrounded = true; }
        //Debug.Log(this._isGrounded);

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        { this.flip(); }

        if (other.gameObject.CompareTag("Platform"))
        { this.flip(); }

    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        { this._isGrounded = false; }
        //Debug.Log(this._isGrounded);
    }

    private void flip()
    {
        if (this._isFacingLeft)
        {
            spriteRender.flipX = false;
            this._isFacingLeft = false;
            Vector3 theScale = _transform.localScale;
            theScale.x *= 1;
            _transform.localScale = theScale;

            Debug.Log(_transform.localScale.x);
        }
        else
        {
            spriteRender.flipX = true;
            this._isFacingLeft = true;
            Vector3 theScale = _transform.localScale;
            theScale.x *= -1;
            _transform.localScale = theScale;

        }
    }
}
