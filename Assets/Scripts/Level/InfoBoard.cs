using UnityEngine;
using System.Collections;

public class InfoBoard : MonoBehaviour
{
    [SerializeField] private InfoBoardData boardData;
    [SerializeField] private bool showOnce = true;
    
    private bool wasActivated = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && (!showOnce || !wasActivated))
        {
            wasActivated = true;
            GameManager.Instance.ShowInfoBoard(boardData);
            
            if (showOnce)
            {
                // Opcional: desativar o collider após uso
                GetComponent<Collider2D>().enabled = false;
            }
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // Adiciona dica visual no editor
        if (showOnce)
        {
            gameObject.name = gameObject.name.Replace("(Reusable)", "");
            if (!gameObject.name.Contains("(Once)"))
                gameObject.name += " (Once)";
        }
        else
        {
            gameObject.name = gameObject.name.Replace("(Once)", "");
            if (!gameObject.name.Contains("(Reusable)"))
                gameObject.name += " (Reusable)";
        }
    }
#endif
}
