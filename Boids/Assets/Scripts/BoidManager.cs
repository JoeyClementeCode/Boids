using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BoidManager : MonoBehaviour
{
    [SerializeField] private Vector2 screenSize;
    [SerializeField] private int numBoids;
    [SerializeField] private GameObject boid;
    private List<GameObject> totalBoids = new List<GameObject>();

    private void Start()
    {
        SpawnBoids(numBoids);
    }

    void SpawnBoids(int numBoids)
    {
        for (int i = 0; i < numBoids; i++)
        {
            Vector3 randLocation = new Vector3(Random.Range(-1 * screenSize.x / 2, screenSize.x / 2), Random.Range(-1 * screenSize.y / 2, screenSize.y / 2), 0);
            Quaternion randRotation = Quaternion.Euler(0f, 0f, Random.Range(0, 359));
            GameObject newBoid = Instantiate(this.boid, randLocation, randRotation);
            newBoid.GetComponent<Boid>().screenSize = screenSize;
            newBoid.GetComponent<Boid>().boidManager = this;
            newBoid.GetComponent<Rigidbody2D>().velocity = newBoid.transform.up;
            newBoid.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
            totalBoids.Add(newBoid);
        }
    }

    public List<GameObject> FindGameObjectsInRange(float radius, Vector3 boidLocation, GameObject requestingBoid)
    {
        List<GameObject> tempBoid = new List<GameObject>();
        foreach (var boid in totalBoids)
        {
            if ((boid != requestingBoid ) && (boidLocation - boid.transform.position).sqrMagnitude <= radius * radius)
            {
                tempBoid.Add(boid);
            }
        }

        return tempBoid;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, screenSize);
    }
}

