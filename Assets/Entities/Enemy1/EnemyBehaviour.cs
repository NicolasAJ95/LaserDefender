using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

    public GameObject projectile;
    public float health = 100;
    public float projectileSpeed;
    public float shotsPerSeconds = 0.5f;
    public int scoreValue = 150;

    public AudioClip fireSound;
    public AudioClip deathSound;

    private ScoreKeeper scoreKeeper;

    void Start()
    {
        scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
    }

    void Update()
    {
        float probability = Time.deltaTime * shotsPerSeconds;
        if (Random.value < probability )
        {
            Fire();
        }
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
                Die();
            }
        }
    }

    void Fire()
    {
        GameObject missile = Instantiate(projectile, gameObject.GetComponent<Transform>().position, Quaternion.identity) as GameObject;
        missile.GetComponent<Rigidbody2D>().velocity += new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(fireSound, gameObject.GetComponent<Transform>().position);
    }

    void Die()
    {
        Destroy(gameObject);
        scoreKeeper.Score(scoreValue);
        AudioSource.PlayClipAtPoint(deathSound, gameObject.GetComponent<Transform>().position);
    }



}
