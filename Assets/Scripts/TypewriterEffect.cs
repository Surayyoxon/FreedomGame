using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TypewriterEffect : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text text;
    [SerializeField] private Button continueButton;

    [Header("Typing")]
    [SerializeField] private float typingSpeed = 0.04f;
    [SerializeField] private float pauseBetweenMessages = 0.8f;

    private string[] messages =
    {
        "BU QISHLOQ TASHQI TA'MINOTGA QARAM.",

        "Elektr.\nSuv.\nOziq-ovqat.\nTexnologiya.",

        "Bularning barchasi tashqaridan keladi.",

        "Lekin qishloq o'z kuchi bilan yashay oladi.",

        "O'zingiz quring.\nO'zingiz ishlab chiqaring.\nO'z kelajagingizni o'zingiz yarating.",

        "QISHLOQNI MUSTAQIL QILING."
    };

    private void Start()
    {
        continueButton.gameObject.SetActive(false);

        StartCoroutine(PlayIntro());
    }

    private IEnumerator PlayIntro()
    {
        foreach (string message in messages)
        {
            yield return StartCoroutine(TypeMessage(message));

            yield return new WaitForSeconds(pauseBetweenMessages);

            text.text = "";
        }

        continueButton.gameObject.SetActive(true);
    }

    private IEnumerator TypeMessage(string message)
    {
        text.text = "";

        foreach (char letter in message)
        {
            text.text += letter;

            yield return new WaitForSeconds(typingSpeed);
        }
    }
}