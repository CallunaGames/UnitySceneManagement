using System.Collections.Generic;
using System.Linq;
using Calluna.DI;
using UnityEngine;

namespace Calluna.SceneManagement
{
    public class ExecuteSceneChangesOnStart : MonoBehaviour, Injectable, Initializable
    {
        [SerializeField] private List<SceneChangeSettings> _changes = new List<SceneChangeSettings>();
        private SceneChangesExecutor _sceneChangesExecutor;
        
        public void Inject(Resolver resolver)
        {
            _sceneChangesExecutor = resolver.Resolve<SceneChangesExecutor>();
        }

        public void Initialize()
        {
            _sceneChangesExecutor.Execute(_changes.Select(c => c.Create()).ToList());
        }
    }
}