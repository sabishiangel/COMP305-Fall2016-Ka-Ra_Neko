using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
    //Private Instance Variables
    private Transform _transform;
    private Rigidbody2D _rigidbody;
    private float _move;
    private bool _isFacingLeft;
    private bool _isGrounded;
    private bool _GroundAhead;
    private bool invincible;
    private float _jump;
    private SpriteRenderer spriteRender;
    private int _healthValue;
    private int powerLvl;

    //Public Instance Variables
    public Camera _camera;
    public Transform spawnPoint;
    public AudioSource backgroundMusic;
    public GameObject Enemy;
    public GameObject attack;
    public float timeBetweenFires = 3f;
    public float lastFired = -100f;
    public float attackSpeed;

    //Public Properties
    public float velocity = 10f;
    public float jumpForce = 100f;
    public GameController gameController; //references game controller script
    public Transform sightStart;
    public Transform sightEnd;

    public int scoreValue // updates score
    {
        get { return this.gameController.score; }
        set
        {
            this.gameController.score = value;
            this.gameController.scoreLabel.text = "Score: " + this.gameController.score;
        }
    }

    public int healthValue //updates lives
    {
        get { return this._healthValue; }
        set
        {
            this._healthValue = value;
            this.gameController.livesLabel.text = "Health: " + this._healthValue;
        }
    }

    public int powerValue //updates power
    {
        get { return this.powerLvl; }
        set
        {
            this.powerLvl = value;
            this.gameController.powerLabel.text = "Power: " + this.powerLvl;
        }
    }

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
            if ((Input.GetKeyDown(KeyCode.Space)) && (this._GroundAhead == false))
            {
                this._jump = 1;
            }

            //Movement
            this._rigidbody.AddForce(new Vector2(this._move * Mathf.Clamp(this.velocity, 1f, 10f), Mathf.Clamp(this._jump, 0f, 1f) * Mathf.Clamp(this.jumpForce, 0f, 220f)), ForceMode2D.Force);

        }
        else
        {
            this._move = 0f;
            this._jump = 0f;            
        }

        //Camera Movement
        this._camera.transform.position = new Vector3(Mathf.Clamp(this._transform.position.x, 0f, 128.4f), Mathf.Clamp(this._transform.position.y, 0f, 6.56f), -10f);

        //Check for ground ahead
        this._GroundAhead = Physics2D.Linecast(this.sightStart.position, this.sightEnd.position, 1 << LayerMask.NameToLayer("Solid"));
        Debug.DrawLine(this.sightStart.position, this.sightEnd.position);

        //Attack
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (this.powerValue >= 1)
            {
                this.Attack();
            }

        }

    }

    private void initialize()
    {
        this._transform = GetComponent<Transform>();
        this._rigidbody = GetComponent<Rigidbody2D>();
        this._move = 0f;
        this._isFacingLeft = true;
        this._isGrounded = false;
        this.spriteRender = GetComponent<SpriteRenderer>();
        this.invincible = false;
        this.powerLvl = 5;
        this._healthValue = 100;
        this.attackSpeed = 20f;
}

    //Flips character direction
    private void flip()
    {
        if (this._isFacingLeft)
        {
            spriteRender.flipX = false;
            this._isFacingLeft = false;
            Vector3 theScale = _transform.localScale;
            theScale.x *= -1;
            _transform.localScale = theScale;
        }
        else
        {
            spriteRender.flipX = true;
            this._isFacingLeft = true;
            Vector3 theScale = _transform.localScale;
            theScale.x *= 1;
            _transform.localScale = theScale;
        }
    }

    //Causes events when player hits deathplane
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("DeathPlane"))
        {
            this._transform.position = this.spawnPoint.position;
        }

        if (other.gameObject.CompareTag("Finish"))
        {
            this.scoreValue += 50;
            gameController.winGame();
        }

        if (other.gameObject.CompareTag("Enemy")) // hurt
        {
            this.healthValue -= 10;
            invincible = true;
            Debug.Log(invincible);
            StartCoroutine(_damager());
            Invoke("resetInvulnerability", 5 * Time.deltaTime);

            if (healthValue <= 0)
            {
                backgroundMusic.Stop();
                backgroundMusic.loop = false;
                gameController.endGame();
            }

        }

        if (other.gameObject.CompareTag("Anger"))
        {
            this.healthValue -= 10;
            invincible = true;
            Debug.Log(invincible);
            StartCoroutine(_damager());
            Invoke("resetInvulnerability", 5 * Time.deltaTime);

            if (healthValue <= 0)
            {
                backgroundMusic.Stop();
                backgroundMusic.loop = false;
                gameController.endGame();
            }
        }

        if (other.gameObject.CompareTag("GVibe"))
        {
            this.scoreValue += 10;
        }

        if (other.gameObject.CompareTag("AttackPower"))
        {
            this.scoreValue += 20;
            this.powerValue += 1;
        }
    }
    private void resetInvulnerability()
    {
        this.invincible = false;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
            this._isGrounded = true;
        //Debug.Log(this._isGrounded);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
            this._isGrounded = false;
        //Debug.Log(this._isGrounded);
    }

    IEnumerator _damager() // colour effect when hit
    {
        int looper = 0;

        if(looper <= 5)
        {
            spriteRender.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            spriteRender.color = Color.white;
            yield return new WaitForSeconds(0.2f);
            looper++;
            Debug.Log(looper);
        }

        
    }

    void Attack()
    {

        if (Time.time < lastFired + timeBetweenFires)
        {
            return;
        }

        lastFired = Time.time;

        GameObject attackShot = (GameObject)Instantiate(attack, transform.position, transform.rotation);
        attackShot.transform.position = transform.position;
        Vector2 direction = sightEnd.transform.position - attackShot.transform.position;

        attackShot.GetComponent<AttackProjectiles>().setDirection(direction);

        this.powerValue -= 1;

    }


}
