using UnityEngine;

[CreateAssetMenu(fileName = "InfoBoardData", menuName = "Game/Info Board Data")]
public class InfoBoardData : ScriptableObject
{
    [TextArea(3, 10)]
    public string message;
    public float delayForInputMessage = 2f;
    public string inputPromptMessage = "Pressione ESPAÇO para fechar";
}
