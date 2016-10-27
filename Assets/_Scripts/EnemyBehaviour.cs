using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

    //Private Instance Variables
    private Transform _transform;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer spriteRender;
    private bool _isGrounded;
    private bool _GroundAhead;
    private bool _isFacingLeft;
    private bool _isPlayerThere;
    private Collider2D enemy;

    //Public Instance Variables
    public float Speed = 5f;
    public float MaxSpeed = 8f;
    public float timeBetweenFires = 3f;
    public float lastFired = -100f;
    public float timeTilNextFire = 1f;
    public Transform sightStart;
    public Transform sightEnd;
    public Transform playerInSight;
    public Transform playerLocation;
    public GameObject attack;
    public GameObject goodVibe;



    // Use this for initialization
    void Start () {
        this._transform = GetComponent<Transform>();
        this._rigidbody = GetComponent<Rigidbody2D>();
        this._isGrounded = false;
        this._GroundAhead = true;
        this.spriteRender = GetComponent<SpriteRenderer>();
        this._isFacingLeft = true;
        this._isPlayerThere = false;
        this.enemy = GetComponent<Collider2D>();

        //Makes sure player is seen
        playerLocation = GameObject.Find("Hero").transform;
        if (!playerLocation)
            Debug.Log("ERROR could not find Player!");
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        //Check if player is there
        this._isPlayerThere = Physics2D.Linecast(this.sightStart.position, this.playerInSight.position, 1 << LayerMask.NameToLayer("Player"));

        //If normal enemy is grounded, move
        if (this._isGrounded && this.gameObject.CompareTag("Enemy"))
        {
            this._rigidbody.velocity = new Vector2(this._transform.localScale.x, 0) * this.Speed;
            this._GroundAhead = Physics2D.Linecast(this.sightStart.position, this.sightEnd.position, 1 << LayerMask.NameToLayer("Solid"));           

            if (this._GroundAhead == false)
            {
                this.flip();
            }

        }

        //Lines to help see sight
        Debug.DrawLine(this.sightStart.position, this.playerInSight.position);
        Debug.DrawLine(this.sightStart.position, this.sightEnd.position);

        //if player is in sight
        if (this._isPlayerThere == true)
        {
            if (this.gameObject.CompareTag("Boss")) //if boss, attack ability enabled
            { Invoke("Attack", timeTilNextFire); }

            if (this.gameObject.CompareTag("Enemy"))
            {
                this.Speed *= 2;
                if (this.Speed >= MaxSpeed)
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

        //flip enemy when colliding into things
        if (other.gameObject.CompareTag("Enemy"))
        { this.flip(); }

        if (other.gameObject.name == "Bench (1)")
        { this.flip(); }

        if (other.gameObject.name == "Bench (2)")
        { this.flip(); }

        if (other.gameObject.name == "Bench (3)")
        { this.flip(); }

        if (other.gameObject.name == "Bench (4)")
        { this.flip(); }

        if (other.gameObject.name == "Bench (5)")
        { this.flip(); }

        if (other.gameObject.name == "Bench")
        { this.flip(); }

        if (other.gameObject.name == "Exit")
        { this.flip(); }

        if (other.gameObject.name == "Telebooth")
        { this.flip(); }

        //when attacked by player
        if (other.gameObject.CompareTag("Love"))
        {
            Destroy(this.gameObject);
            Instantiate(goodVibe, transform.position, transform.rotation);//leaves bonus point when enemy defeated
                
        }

        if (other.gameObject.CompareTag("Love"))
        {
            Destroy(this.gameObject);
            Instantiate(goodVibe, transform.position, transform.rotation);

            if (this.gameObject.name == "Boss")
            {
                int loop = 4;

                if(loop >= 0)
                {
                  Instantiate(goodVibe, transform.position, transform.rotation);
                  loop++;
                }
                
            }

        }

        if (other.gameObject.CompareTag("Anger"))
        {
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), other.gameObject.GetComponent<Collider2D>());
        }

    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        { this._isGrounded = false; }
        //Debug.Log(this._isGrounded);
    }

    private void flip()//flip enemy when turning around
    {
        if (this._isFacingLeft)
        {
            spriteRender.flipX = false;
            this._isFacingLeft = false;
            Vector3 theScale = _transform.localScale;
            theScale.x *= 1;
            _transform.localScale = theScale;

            //Debug.Log(_transform.localScale.x);
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

    void Attack()
    {

        if (Time.time < lastFired + timeBetweenFires)
        {
            return;
        }

        lastFired = Time.time;

        GameObject attackShot = (GameObject)Instantiate(attack, transform.position, transform.rotation);
        attackShot.transform.position = transform.position;

        Vector2 direction = playerLocation.transform.position - attackShot.transform.position;

        attackShot.GetComponent<AttackProjectiles>().setDirection(direction);

    }
}
