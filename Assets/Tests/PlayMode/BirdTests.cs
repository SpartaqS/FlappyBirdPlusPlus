using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using FlappyBirdPlusPlus;

using NUnit.Framework;

namespace Tests
{
    public class BirdTests
    {
        private BirdController birdController;
        private GameSettings testGameSettings;
        private TestInputWrapper testInputWrapper;

        [SetUp]
        public void Setup()
        {
            GameObject playerBirdPrefab = Resources.Load<GameObject>("Prefabs/PlayerBird");
            GameObject player = GameObject.Instantiate(playerBirdPrefab);
            birdController = player.GetComponent<BirdController>();
            testGameSettings = ScriptableObject.CreateInstance<GameSettings>();
            testInputWrapper = new TestInputWrapper();
            birdController.Initialize(testInputWrapper, testGameSettings, new Vector3(-30f, 0f, 0f), null, null);

        }

        [UnityTest]
        public IEnumerator BirdCanMoveOffTheScreen() // the only way to move off the screen without instantly dying is upwards, so that is the only way that will be tested
        {                                  
            yield return new WaitForSeconds(GameSceneConstants.BIRD_TWEEN_IN_TIME); // wait for the bird to tween into the initial position
            
            for(int i = 0; i < 10; i++) // send 10 taps in a quick succession (simulate "spam tapping")
            {
                testInputWrapper.registerTap = true;
                yield return new WaitForSeconds(birdController.TimeForDoubleTap * 0.9f);
            }
            
            Assert.IsTrue(birdController.transform.position.y > GameSceneConstants.CAMERA_SIZE + 1.5 * birdController.BirdRigidbody.GetComponent<CircleCollider2D>().radius); // check if the player is offscreen
        }      
        
        [TearDown]
        public void TearDown()
        {
            GameObject.Destroy(birdController.gameObject);
        }

        public class TestInputWrapper : IInputWrapper
        {
            public bool registerTap = false;

            public bool IsTapped()
            {
                if(registerTap)
                {
                    registerTap = false;
                    return true;
                }
                return false;
            }
        }
    }
}
