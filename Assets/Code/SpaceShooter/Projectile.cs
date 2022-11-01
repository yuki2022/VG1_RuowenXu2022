using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SpaceShooter
{
    public class Projectile : MonoBehaviour
    {
        //Outlets
        Rigidbody2D _rb;

        //State Tracking
        Transform target;

        void ChooseNearestTarget()
        {
            float closestDistance = 9999f;

            Asteroid[] asteroids = FindObjectsOfType<Asteroid>();

            for (int i = 0; i < asteroids.Length; i++)
            {
                Asteroid asteroid = asteroids[i];

                if (asteroid.transform.position.x > transform.position.x)
                {
                    Vector2 directionToTarget = asteroid.transform.position - transform.position;

                    if (directionToTarget.sqrMagnitude < closestDistance)
                    {
                        closestDistance = directionToTarget.sqrMagnitude;

                        target = asteroid.transform;
                    }
                }
            }
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.GetComponent<Asteroid>())
            {
                Destroy(other.gameObject);
                Destroy(gameObject);

                //create an explosion and destroy it soon after
                GameObject explosion = Instantiate(GameController.instance.explosionPrefab, transform.position,
                    Quaternion.identity);
                Destroy(explosion, 0.25f);

                GameController.instance.EarnPoints(10);
            }

        }


        // Start is called before the first frame update
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            float acceleration = GameController.instance.missileSpeed / 2f;
            float maxSpeed = GameController.instance.missileSpeed;


            //Home in on target
            ChooseNearestTarget();
            if (target != null)
            {
                //Rotate towards target
                Vector2 directionToTarget = target.position - transform.position;
                float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

                _rb.MoveRotation(angle);
            }

            _rb.AddForce(transform.right * acceleration);
            _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, maxSpeed);

        }

    }
}