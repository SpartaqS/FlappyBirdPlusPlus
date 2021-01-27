using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

namespace FlappyBirdPlusPlus
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class BackgroundScroller : MonoBehaviour
    {
        [SerializeField] float scrollMultiplier = 1f;

        private float speed = 0;
        public float Speed { get => speed; set => speed = value; }
        private bool keepMoving = false;
        public bool KeepMoving { get => keepMoving; set => keepMoving = value; }
        private float textureUnitSize;

        public void Initialize(BirdController birdController)
        {
            birdController.onDeath += HandleDeath;           
        }

        private void Awake()
        {
            Sprite sprite = GetComponent<SpriteRenderer>().sprite;
            textureUnitSize = sprite.texture.width / sprite.pixelsPerUnit;
        }

        private void Update()
        {
            if (keepMoving)
            {
                transform.position -= new Vector3(speed * scrollMultiplier * Time.deltaTime, 0f, 0f);
                if (transform.position.x < -textureUnitSize)
                {
                    transform.position += new Vector3(textureUnitSize, 0f, 0f);
                }
            }
        }

        private void HandleDeath()
        {            
            keepMoving = false;
        }
    }
}