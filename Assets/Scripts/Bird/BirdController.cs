using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FlappyBirdPlusPlus
{
    [RequireComponent(typeof(Rigidbody2D),typeof(Renderer), typeof(CircleCollider2D))]
    public class BirdController : MonoBehaviour
    {
        bool isAlive = false;//keep track whether we have died or not
       
        public Action startGame;
        public Action onFlap;
        public Action onDeath;
        public Action tryUseBomb;
        private IInputWrapper inputWrapper = null;
        private Rigidbody2D birdRigidbody = null;
        public Rigidbody2D BirdRigidbody { get => birdRigidbody; }
        private Renderer birdRenderer = null;
        public Renderer Renderer { get => birdRenderer; }

        private float flapVelocity = 100f;
        public float FlapVelocity { get => flapVelocity; }
        [SerializeField]
        private float TIME_FOR_DOUBLE_TAP = 0.3f; // should be treated as a constant since once it is set in the PlayerBird prefab, it should not be changed during playtime
        public float TimeForDoubleTap { get => TIME_FOR_DOUBLE_TAP; }
        [SerializeField]
        private float timeSinceLastTap;
        [SerializeField]
        private int quickTapCount;

        public void Initialize(IInputWrapper inputWrapper, GameSettings gameSettings, Vector3 initialPosition, Action StartGameMethod, Action HintImageMethod)
        {
            this.inputWrapper = inputWrapper;
            birdRigidbody = GetComponent<Rigidbody2D>();
            birdRenderer = GetComponent<Renderer>();
            timeSinceLastTap = TIME_FOR_DOUBLE_TAP * 2; // initialize the time so that the first tap ever is not read as double tap (so that we can give the bird some initial bombs if we want)
            quickTapCount = 0;

            isAlive = false;
            birdRigidbody.simulated = false;
            flapVelocity = gameSettings.FlapVelocity;
            birdRigidbody.gravityScale = gameSettings.GravityScale;
            LeanTween.move(gameObject, initialPosition, GameSceneConstants.BIRD_TWEEN_IN_TIME);// when finished start the game I guess            

            GetComponent<SpriteRenderer>().color = gameSettings.BirdColor;

            startGame += StartGameMethod;
            startGame += HintImageMethod;
            startGame += EnableControl;            
        }

        private void EnableControl()
        {
            isAlive = true;
            birdRigidbody.simulated = true;
            timeSinceLastTap = TIME_FOR_DOUBLE_TAP * 2; // initialize the time so that the first tap ever is not read as double tap (so that we can give the bird some initial bombs if we want)
            quickTapCount = 0;
        }

        private void Update()
        {
            if (isAlive)
            {
                timeSinceLastTap += Time.deltaTime;
                if (inputWrapper.IsTapped())
                {                    
                    birdRigidbody.velocity = Vector2.up * flapVelocity;

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
                        tryUseBomb?.Invoke();
                    }

                    timeSinceLastTap = 0;
                    onFlap?.Invoke();
                }
            }
            else if (!birdRigidbody.simulated) // Have not started the game yet
            {
                if (inputWrapper.IsTapped())
                {
                    startGame?.Invoke();
                    birdRigidbody.velocity = Vector2.up * flapVelocity;
                    startGame = null;
                    onFlap?.Invoke();
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision) // if we collide with anyhting then we should die (destroyed pipes will probably need to have their colliders turned off or something like that)
        {
            TryToDie();
        }

        public void TryToDie()
        {
            if (isAlive)
            {
                isAlive = false;
                onDeath?.Invoke();

                // stop the trail from moving                             
                ParticleSystem.MainModule main = GetComponent<ParticleSystem>().main;
                main.simulationSpeed = 0f;
            }
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