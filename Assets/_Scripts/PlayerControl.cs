using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
    //Private Instance Variables
    private Transform _transform;
    private Rigidbody2D _rigidbody;
    private float _move;
    private bool _isFacingLeft;
    private bool _isGrounded;
    private float _jump;
    private SpriteRenderer spriteRender;

    //Public Instance Variables
    public Camera _camera;
    public Transform spawnPoint;

    //Public Properties
    public float velocity = 10f;
    public float jumpForce = 100f;

	// Use this for initialization
	void Start () {
        this.initialize();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (this._isGrounded)
        {
            this._move = Input.GetAxis("Horizontal");
            if (this._move > 0f)
            {
                this._move = 1f;
                this.flip();
                this._isFacingLeft = false;
            }
            else if (this._move < 0f)
            {
                this._move = -1f;
                this.flip();
                this._isFacingLeft = true;
            }
            else
            {
                this._move = 0f;
            }
            //Debug.Log (this._move);

            //Jump Input
            if (Input.GetKeyDown(KeyCode.Space))
            {
                this._jump = 1;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {

            }

            //Movement
            this._rigidbody.AddForce(new Vector2(this._move * Mathf.Clamp(this.velocity, 1f, 10f), this._jump * this.jumpForce), ForceMode2D.Force);

        }
        else
        {
            this._move = 0f;
            this._jump = 0f;            
        }

        //Camera Movement
        this._camera.transform.position = new Vector3(Mathf.Clamp(this._transform.position.x, 0f, 128.4f), Mathf.Clamp(this._transform.position.y, 0f, 6.56f), -10f);
	}

    private void initialize()
    {
        this._transform = GetComponent<Transform>();
        this._rigidbody = GetComponent<Rigidbody2D>();
        this._move = 0f;
        this._isFacingLeft = true;
        this._isGrounded = false;
        this.spriteRender = GetComponent<SpriteRenderer>();
    }

    //Flips character direction
    private void flip()
    {
        if (this._isFacingLeft)
        {
            spriteRender.flipX = false;
        }
        else
        {
            spriteRender.flipX = true;
        }
    }

    //Causes events when player hits deathplane
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("DeathPlane"))
        {
            this._transform.position = this.spawnPoint.position;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
            this._isGrounded = true;
        Debug.Log(this._isGrounded);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
            this._isGrounded = false;
        Debug.Log(this._isGrounded);
    }


}
