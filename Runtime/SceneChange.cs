using System;
using UnityEngine;

namespace Calluna.SceneManagement
{
    [Serializable]
    public class SceneChange
    {
        [field: SerializeField] public string Scene { get; private set; }
        [field: SerializeField] public SceneChangeType Mode { get; private set; } = SceneChangeType.Load;
    }
}
