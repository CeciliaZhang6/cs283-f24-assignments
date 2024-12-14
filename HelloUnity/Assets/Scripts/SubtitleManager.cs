using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SubtitleManager : MonoBehaviour
{
    public Text subtitleText; 
    public float displayDuration = 2f; 

    private Queue<string> subtitleQueue = new Queue<string>(); 
    private bool isDisplaying = false; 

    public void AddSubtitle(string message)
    {
        subtitleQueue.Enqueue(message);

        if (!isDisplaying)
        {
            StartCoroutine(DisplaySubtitles());
        }
    }

    private IEnumerator DisplaySubtitles()
    {
        isDisplaying = true;

        string concatenatedText = "";

        while (subtitleQueue.Count > 0)
        {
            concatenatedText += subtitleQueue.Dequeue() + "\n"; 
            Debug.Log(concatenatedText);

            // upddate subtitle
            subtitleText.text = concatenatedText;

            yield return new WaitForSeconds(displayDuration);  

            // clear text after displaying for a while
            subtitleText.text = "";

            // reset
            concatenatedText = "";
        }

        isDisplaying = false; 
    }

    
}
