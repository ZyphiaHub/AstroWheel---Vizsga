using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuState : IGameState {
    public void EnterState()
    {
        Debug.Log("Bel�pt�l a f�men�be!");
        SceneManager.LoadScene("Main_Menu");
    }

    public void UpdateState()
    {
        
    }

    public void ExitState()
    {
        Debug.Log("Kil�pt�l a f�men�b�l!");
    }
}