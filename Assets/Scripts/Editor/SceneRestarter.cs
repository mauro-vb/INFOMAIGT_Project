using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
  public class SceneRestarter : MonoBehaviour
  {
      [MenuItem("Helpers/Restart Scene #R")]
      public static void RestartScene()
      {
        var currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
      }
  }
}
