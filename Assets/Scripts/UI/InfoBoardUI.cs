using UnityEngine;
using TMPro;
using System.Collections;

public class InfoBoardUI : MonoBehaviour
{
    [SerializeField] private GameObject infoBoardPanel;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private TextMeshProUGUI inputPromptText;
    
    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 0.3f;
    [SerializeField] private AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    private Coroutine inputPromptCoroutine;
    private Coroutine scaleAnimationCoroutine;

    private void Start()
    {
        infoBoardPanel.SetActive(false);
        inputPromptText.gameObject.SetActive(false);
        
        GameEvents.Instance.AddListener<InfoBoardData>("OnInfoBoardShow", ShowInfoBoard);
        GameEvents.Instance.AddListener("OnInfoBoardClose", HideInfoBoard);
    }
    
    private void OnDestroy()
    {
        if (GameEvents.Instance != null)
        {
            GameEvents.Instance.RemoveListener<InfoBoardData>("OnInfoBoardShow", ShowInfoBoard);
            GameEvents.Instance.RemoveListener("OnInfoBoardClose", HideInfoBoard);
        }
    }
    
    private void ShowInfoBoard(InfoBoardData data)
    {
        if (data == null) return;
        
        infoBoardPanel.SetActive(true);
        infoBoardPanel.transform.localScale = Vector3.zero;
        messageText.text = data.message;
        inputPromptText.gameObject.SetActive(false);
        
        if (scaleAnimationCoroutine != null)
            StopCoroutine(scaleAnimationCoroutine);
            
        if (inputPromptCoroutine != null)
            StopCoroutine(inputPromptCoroutine);
            
        scaleAnimationCoroutine = StartCoroutine(AnimateScale(0f, 1f));
        inputPromptCoroutine = StartCoroutine(ShowInputPrompt(data));
    }
    
    private IEnumerator ShowInputPrompt(InfoBoardData data)
    {
        yield return new WaitForSecondsRealtime(data.delayForInputMessage);
        inputPromptText.text = data.inputPromptMessage;
        inputPromptText.gameObject.SetActive(true);
    }
    
    private void HideInfoBoard()
    {
        if (inputPromptCoroutine != null)
        {
            StopCoroutine(inputPromptCoroutine);
            inputPromptCoroutine = null;
        }
        
        if (scaleAnimationCoroutine != null)
            StopCoroutine(scaleAnimationCoroutine);
            
        scaleAnimationCoroutine = StartCoroutine(AnimateScale(1f, 0f, true));
    }

    private IEnumerator AnimateScale(float startScale, float endScale, bool hideAfter = false)
    {
        float elapsedTime = 0;
        Vector3 startSize = Vector3.one * startScale;
        Vector3 endSize = Vector3.one * endScale;
        
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float normalizedTime = elapsedTime / animationDuration;
            float curveValue = animationCurve.Evaluate(normalizedTime);
            
            infoBoardPanel.transform.localScale = Vector3.Lerp(startSize, endSize, curveValue);
            
            yield return null;
        }
        
        infoBoardPanel.transform.localScale = endSize;
        
        if (hideAfter)
            infoBoardPanel.SetActive(false);
    }

    private void OnDisable()
    {
        if (scaleAnimationCoroutine != null)
            StopCoroutine(scaleAnimationCoroutine);
        if (inputPromptCoroutine != null)
            StopCoroutine(inputPromptCoroutine);
            
        infoBoardPanel.transform.localScale = Vector3.one;
        HideInfoBoard();
    }
}
