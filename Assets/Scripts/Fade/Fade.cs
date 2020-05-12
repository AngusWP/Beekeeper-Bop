using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour {

    public GameObject fade;

    void Start() {
        
    }

    public void fadeIn() {
        StartCoroutine(FadeCanvasGroup(fade.GetComponent<CanvasGroup>(), fade.GetComponent<CanvasGroup>().alpha, 1));
    }

    public void fadeOut() {
        StartCoroutine(FadeCanvasGroup(fade.GetComponent<CanvasGroup>(), fade.GetComponent<CanvasGroup>().alpha, 0));
    }

    public IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end) {
        float lerpTime = 1f;
        float timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (true) {
            timeSinceStarted = Time.time - timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);
            cg.alpha = currentValue;

            if (percentageComplete >= 1) {
                break; // finished the coroutine
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
