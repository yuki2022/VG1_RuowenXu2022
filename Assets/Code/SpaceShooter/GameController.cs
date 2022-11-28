using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace SpaceShooter
{
    public class GameController : MonoBehaviour
    {
        public static GameController instance;

        public Transform[] spawnPoints;
        public GameObject[] asteroidPrefabs;
        public GameObject explosionPrefab;



        public float minAsteroidDelay = 0.2f;
        public float maxAsteroidDelay = 2f;

        //State Tracking
        public float timeElapsed;
        public float asteroidDelay;

        void SpawnAsteroid()
        {
            int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
            Transform randomSpawnPoint = spawnPoints[randomSpawnIndex];
            int randomAsteroidIndex = Random.Range(0, asteroidPrefabs.Length);
            GameObject randomAsteroidPrefab = asteroidPrefabs[randomAsteroidIndex];

            Instantiate(randomAsteroidPrefab, randomSpawnPoint.position, Quaternion.identity);
        }

        IEnumerator AsteroidSpawnTimer()
        {
            //wait
            yield return new WaitForSeconds(asteroidDelay);

            //Spawn
            SpawnAsteroid();

            //report
            StartCoroutine("AsteroidSpawnTimer");
        }



        void Awake()
        {
            instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine("AsteroidSpawnTimer");
        }

        // Update is called once per frame
        void Update()
        {
            //Increment passage of time for each frame of the game
            timeElapsed += Time.deltaTime;

            //Computer asteroid delay
            float decreaseDelayOverTime = maxAsteroidDelay - ((maxAsteroidDelay - maxAsteroidDelay) / 30f * timeElapsed);
            asteroidDelay = Mathf.Clamp(decreaseDelayOverTime, minAsteroidDelay, maxAsteroidDelay);
        }

    }
}