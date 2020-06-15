using UnityEngine;

public class ButtonStart : MonoBehaviour
{
    public void LoadLevel()
    {
        print("ButtonClicked");
        GameManager.Instance.LoadNextLevel();
    }
}
