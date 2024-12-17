using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AdvisorScript : MonoBehaviour
{
    [SerializeField] private GameObject advisorPanel; // The Advisor UI Panel
    [SerializeField] private Text advisorText;        // Text field for the Advisor's message
    [SerializeField] private float displayDuration = 10f;

    private Coroutine displayCoroutine;

    void Start()
    {
        advisorPanel.SetActive(false);
    }

    // Call this method to show the Advisor with a specific message and image
    public void ShowAdvisor(string message)
    {
        advisorText.text = message;
        advisorPanel.SetActive(true);

        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
        }

        displayCoroutine = StartCoroutine(hideAfterDelay());
    }

    // Automatically hide the Advisor after the specified duration
    private IEnumerator hideAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        advisorPanel.SetActive(false);
        displayCoroutine = null;
    }

    // Optional: Immediately hide the Advisor
    public void HideAdvisor()
    {
        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
            displayCoroutine = null;
        }
        advisorPanel.SetActive(false);
    }
}