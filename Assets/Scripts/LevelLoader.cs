using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

	[SerializeField] [Tooltip ("Delay in seconds the game waits on the Splash Screen.")]
	private float splashScreenDelay = 3f;
	[SerializeField][Tooltip ("Delay before reloading the level on player death.")]
	private float reloadDelay = 3f;

	private void Start () {
		int buildIndex = SceneManager.GetActiveScene().buildIndex;

		int splashScreenIndex = 0;
		if(buildIndex == splashScreenIndex)
		{
			Invoke("LoadNextLevel", splashScreenDelay);
			print(buildIndex);
		}

		if (FindObjectOfType<PlayerController>())
		{
			var collisionHandler = GetComponent<PlayerCollisonHandler>();
			collisionHandler.PlayerCollisionObservers += OnPlayerCollision;
		}
	}

	private void OnPlayerCollision()
	{
		Invoke("ReloadLevel", reloadDelay);
	}

	public void LoadNextLevel () {
		int buildIndex = SceneManager.GetActiveScene().buildIndex;

		SceneManager.LoadScene(buildIndex + 1);
	}

	public void ReloadLevel()
	{
		int buildIndex = SceneManager.GetActiveScene().buildIndex;

		SceneManager.LoadScene(buildIndex);
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
