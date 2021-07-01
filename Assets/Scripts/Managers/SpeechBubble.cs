using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeechBubble : MonoBehaviour
{
    [SerializeField] private SpriteRenderer backgroundRenderer;
    [SerializeField] private TextMeshPro textMeshPro;
    [SerializeField] private Vector2 textPadding;

    private float destroyDelayTime;
    private string textToWrite;
    private float timePerCharacter;
    private float timer;

    private int characterIndex;
    private bool writing;

    public void Setup(string text, float timePerCharacter, float destroyDelayTime = 2.0f)
    {
        this.timePerCharacter = timePerCharacter;
        this.textToWrite = text;
        this.destroyDelayTime = destroyDelayTime;

        timer = 0f;
        characterIndex = 0;

        writing = true;
    }

    private void Update()
    {
        if (writing)
        {
            timer -= Time.deltaTime;
            if(timer <= 0f)
            {
                timer += timePerCharacter;
                characterIndex++;
                textMeshPro.text = textToWrite.Substring(0, characterIndex);
                SetBackground();

                if(characterIndex >= textToWrite.Length)
                {
                    Destroy(this.gameObject, destroyDelayTime);
                    writing = false;
                }
            }
        }
    }

    private void SetBackground()
    {
        textMeshPro.ForceMeshUpdate();
        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        backgroundRenderer.size = textSize + textPadding;
        backgroundRenderer.transform.localPosition = new Vector3(backgroundRenderer.size.x / 2f, 0f);
    }
}
