using UnityEngine;

public class ButtonModeSelecter : MonoBehaviour
{
    [SerializeField] GameManager.GameMode gameMode;
    public void Starter()
    {
        GameManager.Instance.StartGame(gameMode);
    }
}
