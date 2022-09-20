using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MazeObstacle : MonoBehaviour
{
    private void OnCoillsionEnter2D(Collision2D other)
    {
        //Reload scene only when colliding with player
        if (other.gameObject.GetComponent<RotatingShipController>())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
