using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FlappyBirdPlusPlus
{
    public class HintImage : MonoBehaviour
    {
        public void Hide()
        {
            RectTransform thisRectTransform = GetComponent<RectTransform>();
            LeanTween.cancel(thisRectTransform);
            LeanTween.alpha(thisRectTransform, 0f, 0.5f).setOnComplete(x => gameObject.SetActive(false));
        }
    }
}