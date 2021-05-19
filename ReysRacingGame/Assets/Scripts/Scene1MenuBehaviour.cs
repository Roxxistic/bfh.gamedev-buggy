using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene1MenuBehaviour : MonoBehaviour
{
	public void OnMenuClick()
	{
		SceneManager.LoadScene("SceneMenu");
	}
}
