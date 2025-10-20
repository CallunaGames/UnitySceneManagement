using System;
using System.Collections.Generic;
using Calluna.DI;
using Calluna.Process;
using UnityEngine;

namespace Calluna.SceneManagement
{
    public class SceneChangesExecutor : MonoBehaviour, Injectable
    {
        private Processor _processor;
        
        public void Inject(Resolver resolver)
        {
            _processor = resolver.Resolve<Processor>();
        }
        
        private void Reset()
        {
            _processor = GetComponent<Processor>();
        }

        public void Execute(SceneChange change)
        {
            _processor.Process(CreateProcess(change));
        }

        public void Execute(List<SceneChange> changes)
        {
            _processor.Process(CreateProcess(changes));
        }

        private ControllableProcess CreateProcess(List<SceneChange> changes)
        {
            if (changes.Count == 0)
            {
                throw new InvalidOperationException(
                    "Failed to execute scene management commands. There are no commands defined");
            }

            if (changes.Count == 1)
            {
                return CreateProcess(changes[0]);
            }
            
            SceneChangeProcess[] processes = new SceneChangeProcess[changes.Count];
            for (int i = 0; i < changes.Count; i++)
            {
                processes[i] = CreateProcess(changes[i]);
            }
            return new ProcessSequence(processes, "Scene Management Commands");
        }

        private SceneChangeProcess CreateProcess(SceneChange change)
        {
            return new SceneChangeProcess(change);
        }
    }
}