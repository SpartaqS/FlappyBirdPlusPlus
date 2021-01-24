﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FlappyBirdPlusPlus
{
    public class BirdController : MonoBehaviour
    {
        bool isAlive = true;//keep track whether we have died or not

        private Action startGame;
        public Action onDeath;
        public Action tryUseBomb;
        private Rigidbody2D birdRigidbody = null;
        public Rigidbody2D BirdRigidbody { get => birdRigidbody; }
        private new Renderer renderer = null;
        public Renderer Renderer { get => renderer; }

        private float flapVelocity = 100f;
        [SerializeField]
        private float TIME_FOR_DOUBLE_TAP = 1f;
        [SerializeField]
        private float timeSinceLastTap;
        [SerializeField]
        private int quickTapCount;

        public void Initialize(GameSettings gameSettings, Vector3 initialPosition, Action StartGameMethod, Action HintImageMethod)
        {
            isAlive = false;
            birdRigidbody.simulated = false;
            flapVelocity = gameSettings.FlapVelocity;
            birdRigidbody.gravityScale = gameSettings.GravityScale;
            LeanTween.move(gameObject, initialPosition, 1f);// when finished start the game I guess            

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

        private void Awake()
        {
            birdRigidbody = GetComponent<Rigidbody2D>();
            renderer = GetComponent<Renderer>();
            timeSinceLastTap = TIME_FOR_DOUBLE_TAP * 2; // initialize the time so that the first tap ever is not read as double tap (so that we can give the bird some initial bombs if we want)
            quickTapCount = 0;
        }

        private void Update()
        {
            if (isAlive)
            {
                timeSinceLastTap += Time.deltaTime;
                if (Input.GetKeyDown(KeyCode.Mouse0))
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
                        Debug.Log("DOUBLE TAP DETECTED");
                        tryUseBomb?.Invoke();
                    }

                    timeSinceLastTap = 0;
                }
            }
            else if (!birdRigidbody.simulated) // Have not started the game yet
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    startGame?.Invoke();
                    birdRigidbody.velocity = Vector2.up * flapVelocity;
                    startGame = null;
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