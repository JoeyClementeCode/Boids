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
    public List<Color> races;

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
            int randomNum = Random.Range(0, races.Count - 1);
            newBoid.GetComponent<Boid>().screenSize = screenSize;
            newBoid.GetComponent<Boid>().boidManager = this;
            newBoid.GetComponent<Boid>().flockID = randomNum;
            newBoid.GetComponent<SpriteRenderer>().color = races[randomNum];
            newBoid.GetComponent<Rigidbody2D>().velocity = newBoid.transform.up;

            totalBoids.Add(newBoid);
        }
    }

    public List<GameObject> FindGameObjectsInRange(float radius, Vector3 boidLocation, GameObject requestingBoid, bool racismOn)
    {
        List<GameObject> tempBoid = new List<GameObject>();
        foreach (var boid in totalBoids)
        {
            if (racismOn && boid.GetComponent<Boid>().flockID == requestingBoid.GetComponent<Boid>().flockID)
            {
                if ((boid != requestingBoid ) && (boidLocation - boid.transform.position).sqrMagnitude <= radius * radius)
                {
                    tempBoid.Add(boid);
                }
            }

            if (!racismOn && (boid != requestingBoid ) && (boidLocation - boid.transform.position).sqrMagnitude <= radius * radius)
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

