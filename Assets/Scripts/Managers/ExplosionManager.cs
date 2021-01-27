using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyBirdPlusPlus
{
    public class ExplosionManager : MonoBehaviour
    {
        const float EXPLOSION_STICK_TIME = 0.2f;
        const float EXPLOSION_Z_OFFSET = -1f;

        Rigidbody2D birdRigidbody = null;
        ObjectPool explosionObjectPool = null;
        List<ParticleSystem> activeExplosions = new List<ParticleSystem>();

        public void Initialize(GameplayManager gameplayManager, BirdController birdController, ObjectPool explosionPool, GameSettings gameSettings)
        {
            gameplayManager.useBomb += CreateExplosionFX;
            birdRigidbody = birdController.BirdRigidbody;
            explosionObjectPool = explosionPool;
        }

        private void Update()
        {
            MoveExplosions();
        }

        private void CreateExplosionFX()
        {
            GameObject explosion = explosionObjectPool.TakeObjectFromPool();
            explosion.transform.position = new Vector3(birdRigidbody.position.x, birdRigidbody.position.y, EXPLOSION_Z_OFFSET);
            activeExplosions.Add(explosion.GetComponent<ParticleSystem>());
            explosion.GetComponent<ParticleSystem>().Play();
        }

        private void MoveExplosions()
        {
            for (int i = 0; i < activeExplosions.Count; i++)
            {
                ParticleSystem currentExplosion = activeExplosions[i];
                if(currentExplosion.time < EXPLOSION_STICK_TIME) // so that while the explosion is in "slow-mo" it is attached to the bird
                {
                    currentExplosion.transform.position = new Vector3(birdRigidbody.position.x, birdRigidbody.position.y, EXPLOSION_Z_OFFSET);
                }
                if (!currentExplosion.IsAlive())
                {
                    activeExplosions.RemoveAt(i);
                    explosionObjectPool.ReturnObjectToPool(currentExplosion.transform.gameObject);
                    --i;
                }
            }            
        }        
    }
}