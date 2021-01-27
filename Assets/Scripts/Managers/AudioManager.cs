using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace FlappyBirdPlusPlus
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField]
        AudioSource flap = null;

        [SerializeField]
        AudioSource death = null;

        [SerializeField]
        AudioSource passPipe = null;

        [SerializeField]
        AudioSource explosion = null;

        public void Initialize(BirdController birdController, GameplayManager gameplayManager)
        {
            birdController.onFlap += playFlap;
            birdController.onDeath += playDeath;
            gameplayManager.updateScore += playPassPipe;
            gameplayManager.useBomb += playExplosion;
        }

        private void playFlap()
        {
            flap.Play();
        }

        private void playDeath()
        {
            death.Play();
        }

        private void playPassPipe(int score) // will ignore the score since we just want to play a sound
        {
            passPipe.Play();
        }

        private void playExplosion()
        {
            explosion.Play();
        }

    }
}