using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SplitScreen
{
    public class CameraController : MonoBehaviour
    {
        //Outlets
        public Transform target;

        //Configuration
        public Vector3 offset;
        public float smoothness;

        //State tracking
        Vector3 _velocity;

        void FixedUpdate()
        {
            if (target) {
                transform.position = Vector3.SmoothDamp(
                    transform.position,
                    target.position + offset,
                    ref _velocity,
                    smoothness
                    );
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            if (target) {
                offset = transform.position - target.position;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}