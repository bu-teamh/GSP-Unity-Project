using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartButton()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void AttractButton()
	{
		Debug.Log("Attract Mode");
		SceneManager.LoadScene("Attract Mode");
	}

	public void QuitButton()
	{
		Debug.Log("Quit");
		Application.Quit();
	}

	public void CreditsButton()
	{
		Debug.Log("Credits");
	}
}
