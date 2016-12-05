using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public GameObject laserPrefab;
    public float health = 250f;
    public float speed = 5;
    public float padding = 1;
    public float laserSpeed;
    public float firingRate = 0.2f;

    float xmin;
    float xmax;

	void Start () {

        //Calculates the boundaries of the camera
        float distance = gameObject.GetComponent<Transform>().position.z - Camera.main.GetComponent<Transform>().position.z;
        Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
        Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));

        xmin = leftmost.x + padding;
        xmax = rightmost.x - padding;
    }
	
	void Update () {

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            gameObject.GetComponent<Transform>().position += Vector3.left * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            gameObject.GetComponent<Transform>().position += Vector3.right * speed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            InvokeRepeating("Fire", 0.00001f, firingRate);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            CancelInvoke("Fire");
        }

        //Restrict the player to the gamespace
        float newX = Mathf.Clamp(gameObject.GetComponent<Transform>().position.x, xmin, xmax);
        gameObject.GetComponent<Transform>().position = new Vector3(newX, gameObject.GetComponent<Transform>().position.y, gameObject.GetComponent<Transform>().position.z);

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Projectile missile = col.gameObject.GetComponent<Projectile>();
        if (missile)
        {
            health -= missile.GetDamage();
            missile.Hit();
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    void Fire()
    {
        Vector3 offset = new Vector3(0, 1, 0);
        GameObject laser = Instantiate(laserPrefab, gameObject.GetComponent<Transform>().position, Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector3(0, laserSpeed, 0);
    }
}
