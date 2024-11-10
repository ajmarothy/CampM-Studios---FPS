using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IntroScript : MonoBehaviour
{
    public GameObject dateTextObject;
    public GameObject storyTextObject;
    public Button continueButton;

    [TextArea]
    public string dateContent;

    [TextArea]
    public string storyContent;

    public float typingSpeed = 0.1f;

    private TextMeshProUGUI dateText;
    private TextMeshProUGUI storyText;

    private void Start()
    {
        dateText = dateTextObject.GetComponent<TextMeshProUGUI>();
        storyText = storyTextObject.GetComponent<TextMeshProUGUI>();

        dateText.text = string.Empty;
        storyText.text = string.Empty;
        continueButton.gameObject.SetActive(false);

        StartCoroutine(PlayIntro());
    }

    private IEnumerator PlayIntro()
    {
        yield return StartCoroutine(TypeText(dateText, dateContent));
        yield return new WaitForSeconds(1.5f);

        yield return StartCoroutine(FadeInText(storyText, storyContent));
        yield return new WaitForSeconds(2f);

        continueButton.gameObject.SetActive(true);
    }

    private IEnumerator TypeText(TextMeshProUGUI textComponent, string content)
    {
        foreach (char letter in content.ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
    }

    private IEnumerator FadeInText(TextMeshProUGUI textComponent, string content)
    {
        textComponent.text = content;
        Color originalColor = textComponent.color;
        textComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);

        float fadeDuration = 2f;
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            textComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(0, 1, elapsedTime / fadeDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        textComponent.color = originalColor;
    }
}