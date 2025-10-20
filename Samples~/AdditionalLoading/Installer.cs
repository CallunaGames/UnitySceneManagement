using Calluna.DI;
using Calluna.Process;
using UnityEngine;

namespace Calluna.SceneManagement.Samples
{
    public class Installer : MonoInstaller
    {
        [SerializeField] private Processor _processor;
        [SerializeField] private SceneChangesExecutor _executor;
        [SerializeField] private ProcessLoggerSettings _settings = new ProcessLoggerSettings();
        
        public override void InstallBindings(Binder binder)
        {
            binder.BindInstance(_processor);
            binder.BindInstance(_executor);
            binder.BindInstance(_settings);
        }
    }
}
