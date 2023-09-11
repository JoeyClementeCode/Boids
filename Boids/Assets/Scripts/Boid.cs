using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Boid : MonoBehaviour
{
    [Header("General Variables and Checks")]
    private Rigidbody2D rb;
    [NonSerialized] public BoidManager boidManager;
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float edgeForce;
    [NonSerialized] public Vector2 screenSize;
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    private List<GameObject> minDistanceObjects = new List<GameObject>();
    private List<GameObject> maxDistanceObjects = new List<GameObject>();
    [SerializeField] private Vector2 newVelocity = Vector2.zero;
    [SerializeField] private Vector2 edgeVelocity = Vector2.zero;

    [Space(2)]
    [Header("Algorithms and Weights")]
    [SerializeField] private float cohesionWeight;
    [SerializeField] private float seperationWeight;
    [SerializeField] private float alignmentWeight;
    [SerializeField] private bool cohesionOn;
    [SerializeField] private bool seperationOn;
    [SerializeField] private bool alignmentOn;


    private bool _inBounds;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        ApplyRules();
        StayInBounds();
    }

    private void ApplyRules()
    {
        PopulateLists();
        Vector2 cVector = cohesionOn ? Cohesion() : Vector2.zero;
        Vector2 sVector = seperationOn ? Separation() : Vector2.zero;
        Vector2 aVector = alignmentOn ? Alignment() : Vector2.zero;

        CalculateVelocity(cVector, sVector, aVector);
    }

    private void PopulateLists()
    {
        Vector3 pos = transform.position;
        maxDistanceObjects = boidManager.FindGameObjectsInRange(maxDistance, pos, gameObject);
        minDistanceObjects = boidManager.FindGameObjectsInRange(minDistance, pos, gameObject);
    }

    private Vector2 Cohesion()
    {
        Vector3 centerMass = Vector3.zero;

        if (maxDistanceObjects.Count > 0)
        {
            centerMass = maxDistanceObjects.Aggregate(centerMass, (current, boid) => current + boid.transform.position);
            centerMass /= maxDistanceObjects.Count;
            Vector2 direction = centerMass - transform.position;

            return direction / maxDistance;
        }

        return Vector2.zero;
    }

    private Vector2 Separation()
    {
        Vector3 seperationForce = Vector3.zero;
        if (minDistanceObjects.Count > 0)
        {
            seperationForce = minDistanceObjects.Select(boid => transform.position - boid.transform.position)
                .Aggregate(seperationForce, (current, vector) => current + vector / vector.sqrMagnitude);
        }

        return seperationForce;
    }

    private Vector2 Alignment()
    {
        Vector2 average = Vector2.zero;
        if (minDistanceObjects.Count > 0)
        {
            foreach (var boid in maxDistanceObjects)
            {
                average += boid.GetComponent<Rigidbody2D>().velocity;
            }

            average /= maxDistanceObjects.Count;
        }

        return average;
    }

    private void CalculateVelocity(Vector3 cohesion, Vector3 separation, Vector3 alignment)
    {
        newVelocity = cohesion * cohesionWeight + separation * seperationWeight + alignment * alignmentWeight;
    }

    private void StayInBounds()
    {
        var pos = transform.position;
        _inBounds = true;

        if (pos.x > screenSize.x / 2)
        {
            edgeVelocity += Vector2.left * (edgeForce * Time.deltaTime);
            _inBounds = false;
        }

        if (pos.x < screenSize.x / 2 * -1)
        {
            edgeVelocity += Vector2.right * (edgeForce * Time.deltaTime);
            _inBounds = false;
        }

        if (pos.y > screenSize.y / 2)
        {
            edgeVelocity += Vector2.down * (edgeForce * Time.deltaTime);
            _inBounds = false;
        }

        if (pos.y < screenSize.y / 2 * -1)
        {
            edgeVelocity += Vector2.up * (edgeForce * Time.deltaTime);
            _inBounds = false;
        }

    }

    private void FixedUpdate()
    {
        Vector2 currVel = rb.velocity;

        rb.velocity += edgeVelocity + newVelocity * Time.deltaTime;

        Vector2 normVel = currVel.normalized;

        transform.up = normVel;

        /*if (_rb.velocity.sqrMagnitude < _speed*_speed)
        {
            //_rb.velocity += (Vector2)transform.up * (_speed * Time.deltaTime);
        }*/

        if (rb.velocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rb.velocity = normVel * (maxSpeed * Time.deltaTime);
        }

        if (_inBounds)
        {
            edgeVelocity *= .95f;
            if (edgeVelocity.sqrMagnitude is > -0.4f and < 0.4f)
            {
                edgeVelocity = Vector2.zero;
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maxDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, edgeVelocity);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, newVelocity);

    }

    private void OnDrawGizmos()
    {
        /*Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, _edgeVelocity);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, _newVelocity);*/

    }
}