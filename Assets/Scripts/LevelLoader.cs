using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

	[SerializeField] [Tooltip ("Delay in seconds the game waits on the Splash Screen.")]
	private float splashScreenDelay = 3f;

	private void Start () {
		int buildIndex = SceneManager.GetActiveScene().buildIndex;

		byte splashScreenIndex = 0;
		if(buildIndex == splashScreenIndex)
		{
			Invoke("LoadNextLevel", splashScreenDelay);
		}
	}
	
	public void LoadNextLevel () {
		int buildIndex = SceneManager.GetActiveScene().buildIndex;

		SceneManager.LoadScene(buildIndex + 1);
	}

	public void LoadScene(int sceneToLoad)
	{
		SceneManager.LoadScene(sceneToLoad);
	}

	public void Quit()
	{
		Application.Quit();
	}
}
