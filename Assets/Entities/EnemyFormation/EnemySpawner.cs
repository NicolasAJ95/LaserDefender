using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

    public GameObject enemyPrefab;
    public float width = 10f;
    public float height = 5f;
    public float speed = 30f;
    public float spawnDelay = 0.5f;

    private bool movingRight = true;
    private float xmax;
    private float xmin;

	// Use this for initialization
	void Start () {

        SpawnUntilFull();

    }

    void SpawnUntilFull()
    {
        float distanceToCamera = gameObject.GetComponent<Transform>().position.z - Camera.main.GetComponent<Transform>().position.z;
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distanceToCamera));
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distanceToCamera));

        xmax = rightEdge.x;
        xmin = leftEdge.x;

        Transform freePosition = NextFreePosition();
        if (freePosition)
        {
            GameObject enemy = Instantiate(enemyPrefab, freePosition.GetComponent<Transform>().position, Quaternion.identity) as GameObject;
            enemy.GetComponent<Transform>().parent = freePosition;
        }
        if (NextFreePosition())
        {
            Invoke("SpawnUntilFull", spawnDelay);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height));
    }
	
	// Update is called once per frame
	void Update () {
        if(movingRight)
        {
            gameObject.GetComponent<Transform>().position += Vector3.right * speed * Time.deltaTime;
        }
        else
        {
            gameObject.GetComponent<Transform>().position += Vector3.left * speed * Time.deltaTime;
        }

        float rightEgdeOfFormation = transform.position.x + (0.45f * width);
        float leftEgdeOfFormation = transform.position.x - (0.45f * width);

        if(leftEgdeOfFormation < xmin) 
        {
            movingRight = true;
        }

        if (rightEgdeOfFormation  > xmax)
        {
            movingRight = false;
        }

        if (AllMembersAreDead())
        {
            Debug.Log("Empty Formation.");
            SpawnUntilFull();
        }

    }

    public void SpawnEnemies()
    {
        float distanceToCamera = gameObject.GetComponent<Transform>().position.z - Camera.main.GetComponent<Transform>().position.z;
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distanceToCamera));
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distanceToCamera));

        xmax = rightEdge.x;
        xmin = leftEdge.x;

        foreach (Transform child in transform)
        {
            GameObject enemy = Instantiate(enemyPrefab, child.GetComponent<Transform>().position, Quaternion.identity) as GameObject;
            enemy.GetComponent<Transform>().parent = child;

        }
    }



    Transform NextFreePosition()
    {
        foreach (Transform childPositionGameObject in transform)
        {
            if (childPositionGameObject.childCount == 0)
            {
                return childPositionGameObject;
            }
            
        }
        return null;
    }

    bool AllMembersAreDead()
    {
        foreach (Transform childPositionGameObject in transform)
        {
            if(childPositionGameObject .childCount > 0)
            {
                return false;
            }
        }
        return true;
    }
}
