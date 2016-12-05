using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

    public GameObject projectile;
    public float health = 100;
    public float projectileSpeed;
    public float shotsPerSeconds = 0.5f;

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
                Destroy(gameObject);
            }
        }
    }

    void Fire()
    {
        Vector3 startPosition = gameObject.GetComponent<Transform>().position;
        GameObject missile = Instantiate(projectile, startPosition, Quaternion.identity) as GameObject;
        missile.GetComponent<Rigidbody2D>().velocity += new Vector2(0, -projectileSpeed);
    }



}
