using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuState : IGameState {
    public void EnterState()
    {
        Debug.Log("Beléptél a fõmenübe!");
        SceneManager.LoadScene("Main_Menu");
    }

    public void UpdateState()
    {
        // Itt kezelheted a fõmenü frissítését
    }

    public void ExitState()
    {
        Debug.Log("Kiléptél a fõmenübõl!");
    }
}