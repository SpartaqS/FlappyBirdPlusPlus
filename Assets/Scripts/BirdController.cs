using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FlappyBirdPlusPlus
{
    public class BirdController : MonoBehaviour
    {
        public Action onDeath;
        public Action tryUseBomb;
        private Rigidbody2D birdRigidbody = null;
        public Rigidbody2D BirdRigidbody { get => birdRigidbody; }

        [SerializeField]
        private float FLAP_AMOUNT = 100f;
        [SerializeField]
        private float TIME_FOR_DOUBLE_TAP = 1f;
        [SerializeField]
        private float timeSinceLastTap;
        [SerializeField]
        private int quickTapCount;

        private void Awake()
        {
            birdRigidbody = GetComponent<Rigidbody2D>();
            timeSinceLastTap = TIME_FOR_DOUBLE_TAP * 2; // initialize the time so that the first tap ever is not read as double tap
            quickTapCount = 0;
        }

        void Update()
        {
            timeSinceLastTap += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {                
                birdRigidbody.velocity = Vector2.up * FLAP_AMOUNT;

                if (timeSinceLastTap > TIME_FOR_DOUBLE_TAP)
                {
                    quickTapCount = 0;
                }
                else
                {
                    quickTapCount++;
                }

                if (quickTapCount == 1)
                {
                    Debug.Log("DOUBLE TAP DETECTED");
                    tryUseBomb?.Invoke();
                }

                timeSinceLastTap = 0;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision) // if we collide with anyhting then we should die (destroyed pipes will probably need to have their colliders turned off or something like that)
        {
            Debug.Log("Bird Collided with: " + collision.gameObject);
            onDeath?.Invoke();
        }
        /*
        public void SubscribeToEvents(Action onDeathSubbingMethod)
        {
            onDeath += onDeathSubbingMethod;
        }

        public void UnsubscribeFromEvents(Action onDeathSubbedMethod)
        {
            onDeath -= onDeathSubbedMethod;
        }*/
    }
}