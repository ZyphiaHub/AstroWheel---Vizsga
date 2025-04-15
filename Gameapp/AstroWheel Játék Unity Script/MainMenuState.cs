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
        
    }

    public void ExitState()
    {
        Debug.Log("Kiléptél a fõmenübõl!");
    }
}