using System.Collections;
using TMPro;
using UnityEngine;

public class NewsManager : MonoBehaviour
{
    private enum ChatStates
    {
        Starting,
        Writing,
        Ending
    }

    private ChatStates state;

    [SerializeField] private float numCharsPerSecond;
    [SerializeField] private float tvStartingTime;
    [SerializeField] private float introWalkingTime;

    [SerializeField] private GameObject tvMaskGO;
    [SerializeField] private GameObject chatBox;
    [SerializeField] private TextMeshProUGUI chatBoxText;

    [SerializeField] private string[] chatLines;

    private Animator tvMaskAnim;
    private Animator anim;

    private int lineCounter = 0;

    private bool starting = false;
    private bool writing = false;
    private bool finishedTalking = false;

    private void Start()
    {
        state = ChatStates.Starting;
        anim = GetComponent<Animator>();
        tvMaskAnim = tvMaskGO.GetComponent<Animator>();
    }

    private void Update()
    {
        if (!writing)
        {
            if (state == ChatStates.Starting)
            {
                if (!starting)
                    StartCoroutine(StartingTVCoroutine());
            }
            else if (state == ChatStates.Writing)
            {
                chatBox.SetActive(true);

                if (lineCounter < chatLines.Length)
                {
                    WriteText(chatLines[lineCounter]);
                    lineCounter++;
                }
                else if (lineCounter == chatLines.Length)
                    state = ChatStates.Ending;
            }
            else if (state == ChatStates.Ending)
            {
                chatBox.SetActive(false);
                NewsSceneLoader.instance.LoadScene(2);
            }
        }

        if (finishedTalking)
        {
            finishedTalking = false;
            writing = false;
        }

        UpdateAnimator();
    }

    public void WriteText(string text)
    {
        writing = true;
        chatBox.SetActive(true);
        StartCoroutine(WritingTextCoroutine(text));
    }

    IEnumerator WritingTextCoroutine(string text)
    {
        char[] charArray = text.ToCharArray();
        chatBoxText.overflowMode = TextOverflowModes.Truncate;
        chatBoxText.renderMode = TextRenderFlags.DontRender;
        chatBoxText.text = text;
        chatBoxText.ForceMeshUpdate();

        TMP_TextInfo tMP_TextInfo = chatBoxText.textInfo;
        TMP_LineInfo[] tMP_LineInfos = new TMP_LineInfo[tMP_TextInfo.lineCount];
        tMP_LineInfos = tMP_TextInfo.lineInfo;
        chatBoxText.maxVisibleLines = 4;

        chatBoxText.overflowMode = TextOverflowModes.Overflow;
        chatBoxText.renderMode = TextRenderFlags.Render;
        chatBoxText.text = "";
        chatBoxText.ForceMeshUpdate();

        int totalWritten = 0;
        while (totalWritten < charArray.Length)
        {
            while (chatBoxText.textInfo.lineCount <= chatBoxText.maxVisibleLines &&
                totalWritten < charArray.Length)
            {
                chatBoxText.text += charArray[totalWritten];
                yield return new WaitForSeconds(1.0f / numCharsPerSecond);
                totalWritten++;
            }

            yield return new WaitForSeconds(2.0f);

            chatBoxText.text = "";
            chatBoxText.ForceMeshUpdate();
        }

        finishedTalking = true;
        if (totalWritten == charArray.Length)
            Debug.Log("Reached end of char array");
    }

    IEnumerator StartingTVCoroutine()
    {
        starting = true;
        chatBox.SetActive(false);
        NewsSceneLoader.instance.FadeIn();
        while (NewsSceneLoader.instance.isBusy)
            yield return null;
        NewsSceneAudioManager.instance.PlayIntroSteps();
        yield return new WaitForSeconds(introWalkingTime);
        NewsSceneAudioManager.instance.PlayTVOnSFX();
        tvMaskAnim.SetBool("tvOn", true);
        yield return new WaitForSeconds(tvStartingTime);
        state = ChatStates.Writing;
    }

    private void UpdateAnimator()
    {
        anim.SetBool("talk", writing);
    }

}
