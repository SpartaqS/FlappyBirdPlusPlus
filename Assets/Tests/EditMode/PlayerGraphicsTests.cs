using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class PlayerGraphicsTests
    {
        [Test]
        public void PlayerPrefabExists()
        {
            GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/PlayerBird");
            Assert.AreNotEqual(null, playerPrefab);
        }

        [Test]
        public void PrefabHasSpriteRenderer()
        {
            GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/PlayerBird");

            SpriteRenderer playerSpriteRenderer = null;
            Assert.IsTrue(playerPrefab.TryGetComponent<SpriteRenderer>(out playerSpriteRenderer));            
        }

        [Test]
        public void PlayerGraphicsCorrectlyDefined()
        {
            GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/PlayerBird");
            SpriteRenderer playerSpriteRenderer = playerPrefab.GetComponent<SpriteRenderer>();

            Assert.AreNotEqual(null, playerSpriteRenderer.sprite); // test if a sprite has been assigned            
        }
    }
}
