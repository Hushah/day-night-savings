using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour {
    // Variables
    public bool hasGameEnded = false;

    // Method to end the game when the player wins
    public void EndGameWin() {
        if(hasGameEnded == false) {
            hasGameEnded = true;
            // Debug.Log("You Win!!!");     // DEBUG LINE

            // Unlocking mouse
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Loading Game Won Screen
            SceneManager.LoadScene("GameWonScreen");
        }
    }

    // Method to end the game when the player loses
    public void EndGameLose() {
        if (hasGameEnded == false) {
            hasGameEnded = true;
            // Debug.Log("You Lose!!!");    // DEBUG LINE

            // Unlocking mouse
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Loading Game Over Screen
            SceneManager.LoadScene("GameLoseScreen");
        }
        
    }

}
