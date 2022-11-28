using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace SpaceShooter
{
    public class Ship : MonoBehaviour
    {
        //outlet
        public GameObject projectilePrefab;
    

        //State Tracking
        public float firingDelay = 1f;

        void FireProjectile()
        {
            Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        }

        IEnumerator FiringTimer()
        {
            yield return new WaitForSeconds(firingDelay);

            FireProjectile();

            StartCoroutine("FiringTimer");
        }

        void OnCollisionEnter2D(Collision2D other)
        {
        }

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine("FiringTimer");
        }

        // Update is called once per frame
        void Update()
        {
            float yPosition = Mathf.Sin(SpaceShooter.GameController.instance.timeElapsed) * 3f;
            transform.position = new Vector2(0, yPosition);
        }
    }
}