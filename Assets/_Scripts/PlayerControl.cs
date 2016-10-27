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
    private int _healthValue;
    private int powerLvl;

    //Public Instance Variables
    public Camera _camera;
    public Animator animator;
    public float timeBetweenFires = 3f;
    public float lastFired = -100f;
    public float attackSpeed;
    public float velocity = 10f;
    public float jumpForce = 100f;
    public GameController gameController; //references game controller script

    [Header("Transforms")]
    public Transform spawnPoint;
    public Transform sightStart;
    public Transform sightEnd;

    [Header("Sounds")]
    public AudioSource hurt;
    public AudioSource jump;
    public AudioSource point;
    public AudioSource attackSound;

    [Header("Game Objects")]
    public GameObject Enemy;
    public GameObject attack;
    public GameObject thought;



    //Public Properties

    public int scoreValue //updates score
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
            this.gameController.livesLabel.text = "Health: " + this._healthValue + "%";
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
                this.animator.SetInteger("HeroState", 1);
                this._move = 1f;
                this._isFacingLeft = false;
                this.flip();
            }
            else if (this._move < 0f)
            {
                this.animator.SetInteger("HeroState", 1);
                this._move = -1f;
                this._isFacingLeft = true;
                this.flip();
            }
            else
            {
                this.animator.SetInteger("HeroState", 0);
                this._move = 0f;
            }
            //Debug.Log (this._move);

            //Movement
            this._rigidbody.AddForce(new Vector2(this._move * Mathf.Clamp(this.velocity, 1f, 10f), Mathf.Clamp(this._jump, 0f, 1f) * Mathf.Clamp(this.jumpForce, 0f, 220f)), ForceMode2D.Force);
        }
        else
        {
            this._move = 0f;
            this._jump = 0f;            
        }

        //Camera Movement
        this._camera.transform.position = new Vector3(Mathf.Clamp(this._transform.position.x, 0.5f, 128.4f), Mathf.Clamp(this._transform.position.y, 0f, 6f), -10f);

        //Attack
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (this.powerValue > 0)
            {
                this.animator.SetInteger("HeroState", 3);
                this.Attack();
                attackSound.Play();
            }

        }

        //Jump Input
        if ((Input.GetKeyDown(KeyCode.Space)))
        {
            this._jump = 1;
            jump.Play();
            this._isGrounded = false;
        }

    }

    //Initializes variables
    private void initialize()
    {
        this._transform = GetComponent<Transform>();
        this._rigidbody = GetComponent<Rigidbody2D>();
        this._move = 0f;
        this._isFacingLeft = true;
        this._isGrounded = false;
        this.spriteRender = GetComponent<SpriteRenderer>();
        this.powerValue = 0;
        this._healthValue = 100;
        this.attackSpeed = 20f;
        thought.gameObject.SetActive(false);
    }

    //Flips character direction
    private void flip()
    {
        if (this._isFacingLeft)
        {
            spriteRender.flipX = false;
            sightEnd.transform.position = new Vector2(_transform.position.x * -1, _transform.position.y);
        }
        else
        {
            spriteRender.flipX = true;
            sightEnd.transform.position = new Vector2(_transform.position.x * 3, _transform.position.y); //flip line of sight to face right
        }
    }

    
    private void OnCollisionEnter2D(Collision2D other)
    {
        ////If spacebar is held down, ignore solids
        //if((Input.GetKeyDown(KeyCode.Space)))
        //{
        //    Physics2D.IgnoreLayerCollision(2 << LayerMask.NameToLayer("Solid"), 6 << LayerMask.NameToLayer("Player"), ignore: true);
        //}
        
        //Causes events when player hits deathplane
        if (other.gameObject.CompareTag("DeathPlane"))
        {
            this._transform.position = this.spawnPoint.position;
            this.healthValue -= 10;
            hurt.Play();
            StartCoroutine(_damager());
        }

        if (other.gameObject.CompareTag("Notification"))
        {
            thought.gameObject.SetActive(true);
        }


        //when player reaches the goal
        if (other.gameObject.CompareTag("Finish"))
        {
            if (this.scoreValue >= 710)
            {
                this.scoreValue += 57;
                gameController.winGame();
            }

            else
            {
                this.scoreValue += 50;
                gameController.winGame();
            }
        }

        //hit by enemy
        if (other.gameObject.CompareTag("Enemy")) // hurt
        {
            this.animator.SetInteger("HeroState", 4);
            hurt.Play();
            this.healthValue -= 10;
            StartCoroutine(_damager());

            if (healthValue <= 0)
            {
                gameController.endGame();
            }

        }

        //hit by boss
        if (other.gameObject.CompareTag("Anger"))
        {
            this.animator.SetInteger("HeroState", 4);
            this.healthValue -= 10;
            hurt.Play();
            StartCoroutine(_damager());

            if (healthValue <= 0)
            {
                gameController.endGame();
            }
        }

        //getting points
        if (other.gameObject.CompareTag("GVibe"))
        {
            this.scoreValue += 10;
            point.Play();
        }

        //getting powerups
        if (other.gameObject.CompareTag("AttackPower"))
        {
            this.scoreValue += 20;
            this.powerValue += 2;
            point.Play();
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        //if on platform, player is grounded
        if (other.gameObject.CompareTag("Platform"))
            this._isGrounded = true;
        //Debug.Log(this._isGrounded);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        this.animator.SetInteger("HeroState", 2);
        this._isGrounded = false;
        //Debug.Log(this._isGrounded);
    }

    IEnumerator _damager() //colour effect when hit
    {
        this.animator.SetInteger("HeroState", 4);
        spriteRender.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRender.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        spriteRender.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRender.color = Color.white;
        yield return new WaitForSeconds(0.2f);
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
        this.powerValue -= 1;//after attack, reduce attack amount

    }


}
