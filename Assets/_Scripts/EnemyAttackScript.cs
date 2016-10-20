using UnityEngine;
using System.Collections;

public class CanonballBehavior : MonoBehaviour
{

    // How fast will the canonball move
    public float speed;
    //Direction of the bullet
    Vector2 _direction;
    //Know when the bullet direction is set
    bool isReady;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isReady)
        {
            Vector2 position = transform.position; //find canonball
            position += _direction * speed * Time.deltaTime;  //new position for canonball
            //update canonball position
            transform.position = position;

            //camera limits
            Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
            Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

            if ((transform.position.x < min.x) || (transform.position.x > max.x) || (transform.position.y < min.y) || (transform.position.y > max.y))
            {
                Destroy(gameObject);
            }
        }
    }

    void Awake()
    {
        speed = 200;
        isReady = false;
    }

    public void setDirection(Vector2 direction)
    {
        _direction = direction.normalized;
        isReady = true;
    }
}
