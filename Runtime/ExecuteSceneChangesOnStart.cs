using UnityEngine;

namespace Calluna.SceneManagement
{
    public class ExecuteSceneChangesOnStart : MonoBehaviour
    {
        [SerializeField] private SceneChangesExecutor _sceneChangesExecutor;

        private void Reset()
        {
            _sceneChangesExecutor = GetComponent<SceneChangesExecutor>();
        }

        private void Start()
        {
            _sceneChangesExecutor.Execute();
        }
    }
}