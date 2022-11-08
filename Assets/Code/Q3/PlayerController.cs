using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Platformer { 
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController instance;
        //outlets
        Rigidbody2D _rigidbody2D;
        public Transform aimPivot;
        public GameObject projectilePrefab;
        SpriteRenderer sprite;
        Animator animator;
        public TMP_Text scoreUI;


        //state tracking
        public int jumpsLeft;
        public int score;
        public bool isPaused;

         void Awake()
        {
            instance = this;
        }

        // Methods
        void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            sprite = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            score = PlayerPrefs.GetInt("Score");
        }

        public void ResetScore() {
            score = 0;
            PlayerPrefs.DeleteKey("Score");
        }

        private void FixedUpdate()
        {
            //This Update Event is sync'd with the Physics Engine
            animator.SetFloat("Speed", _rigidbody2D.velocity.magnitude);
            if (_rigidbody2D.velocity.magnitude > 0)
            {
                animator.speed = _rigidbody2D.velocity.magnitude / 3f;
            }
            else {
                animator.speed = 1f;
            }
        }

        // Update is called once per frame
        void Update()
        {
            //update UI
            scoreUI.text = score.ToString();
            if (isPaused) {
                return;
            }
            //Menu
            if (Input.GetKey(KeyCode.Escape))
            {
                MenuController.instance.Show();
            }


    
            //move player left
            if (Input.GetKey(KeyCode.A)) {
                _rigidbody2D.AddForce(Vector2.left * 18f * Time.deltaTime, ForceMode2D.Impulse);
                sprite.flipX = true;
            }
            //move player right
            if (Input.GetKey(KeyCode.D))
            {
                _rigidbody2D.AddForce(Vector2.right * 18f * Time.deltaTime, ForceMode2D.Impulse);
                sprite.flipX = false;
            }

            //Aim Toward Mouse
            Vector3 mousePosition = Input.mousePosition;
            Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector3 directionFromPlayerToMouse = mousePositionInWorld - transform.position;

            float radiansToMouse = Mathf.Atan2(directionFromPlayerToMouse.y, directionFromPlayerToMouse.x);
            float angleToMouse = radiansToMouse * Mathf.Rad2Deg;

            aimPivot.rotation = Quaternion.Euler(0, 0, angleToMouse);

            //shoot
            if (Input.GetMouseButtonDown(0))
            {
                GameObject newProjectile = Instantiate(projectilePrefab);
                newProjectile.transform.position = transform.position;
                newProjectile.transform.rotation = aimPivot.rotation;
            }

            //jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (jumpsLeft > 0)
                {
                    jumpsLeft--;
                    _rigidbody2D.AddForce(Vector2.up * 15f, ForceMode2D.Impulse);
                }
            }
            animator.SetInteger("JumpsLeft", jumpsLeft);
        }


        private void OnCollisionStay2D(Collision2D other)
        {
            //check taht we collided with Ground
            if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                //check what is directly below our character's feet
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, 0.85f);
                //Debug.DrawRay(transform.position, Vector3.down * 0.7f);

                //we minght have multiple things below our character's feet
                for (int i = 0; i < hits.Length; i++)
                {
                    RaycastHit2D hit = hits[i];
                    //check we collided with ground below our feet
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                    {
                        //Reset jump count
                        jumpsLeft = 2;
                    }
                }
            }
        }
    }
}