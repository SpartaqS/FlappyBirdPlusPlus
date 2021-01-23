using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyBirdPlusPlus
{
    public class ExplosionManager : MonoBehaviour
    {
        Rigidbody2D birdRigidbody = null;
        GameObject explosionPrefab = null;

        public void Initialize(GameplayManager gameplayManager, BirdController birdController, GameObject explosionPrefab)
        {
            gameplayManager.useBomb += CreateExplosionFX;
            birdRigidbody = birdController.BirdRigidbody;
            this.explosionPrefab = explosionPrefab;
        }

        private void CreateExplosionFX()
        {
            Instantiate(explosionPrefab,new Vector3(birdRigidbody.position.x, birdRigidbody.position.y,1f), Quaternion.identity);
        }
    }
}