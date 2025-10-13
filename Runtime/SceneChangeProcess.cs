using System;
using Calluna.Process;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Calluna.SceneManagement
{
    public class SceneChangeProcess : ControllableProcessBase
    {
        private readonly SceneChange _sceneChange;
        private AsyncOperation _asyncOperation;

        public SceneChangeProcess(SceneChange sceneChange)
        {
            _sceneChange = sceneChange;
            _name.Value = GetName();
        }

        protected override void DoStart()
        {
            _asyncOperation = ExecuteCommand();
            ValidateOperation();
        }

        protected override void DoTick()
        {
            if (_asyncOperation.isDone)
            {
                FinishProcess();
            }
        }

        protected override void DoAbort()
        {
            Debug.LogError("Scene management processes can not be aborted");
        }

        protected override float GetProgress()
        {
            return _asyncOperation?.progress ?? 0;
        }

        private AsyncOperation ExecuteCommand()
        {
            switch (_sceneChange.Mode)
            {
                case SceneChangeMode.Load:
                    return SceneManager.LoadSceneAsync(_sceneChange.Scene, LoadSceneMode.Single);
                case SceneChangeMode.LoadAdditive:
                    return SceneManager.LoadSceneAsync(_sceneChange.Scene, LoadSceneMode.Additive);
                case SceneChangeMode.Unload:
                    return SceneManager.UnloadSceneAsync(_sceneChange.Scene);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private string GetName()
        {
            switch (_sceneChange.Mode)
            {
                case SceneChangeMode.Load:
                    return $"Loading {_sceneChange.Scene}";
                case SceneChangeMode.LoadAdditive:
                    return $"Loading {_sceneChange.Scene} additive";
                case SceneChangeMode.Unload:
                    return $"Unloading {_sceneChange.Scene}";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ValidateOperation()
        {
            if (_asyncOperation == null)
            {
                SetFailed();
            }
        }
    }
}