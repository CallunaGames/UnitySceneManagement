using System;
using UnityEngine;

namespace Calluna.SceneManagement
{
    [Serializable]
    public class SceneChangeSettings
    {
        [field: SerializeField] public string Scene { get; private set; }
        [field: SerializeField] public SceneChangeMode Mode { get; private set; } = SceneChangeMode.Load;

        public SceneChange Create()
        {
            return new SceneChange
            {
                Scene = Scene,
                Mode = Mode
            };
        }
    }
}
