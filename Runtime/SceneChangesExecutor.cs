using System;
using System.Collections.Generic;
using Calluna.Process;
using UnityEngine;

namespace Calluna.SceneManagement
{
    public class SceneChangesExecutor : MonoBehaviour
    {
        [SerializeField] private List<SceneChange> _changes = new List<SceneChange>();
        [SerializeField] private Processor _processor;

        private void Reset()
        {
            _processor = GetComponent<Processor>();
        }

        public void Execute()
        {
            _processor.Process(CreateProcess());
        }

        private ControllableProcess CreateProcess()
        {
            if (_changes.Count == 0)
            {
                throw new InvalidOperationException(
                    "Failed to execute scene management commands. There are no commands defined");
            }

            if (_changes.Count == 1)
            {
                return new SceneChangeProcess(_changes[0]);
            }

            SceneChangeProcess[] processes = new SceneChangeProcess[_changes.Count];
            for (int i = 0; i < _changes.Count; i++)
            {
                processes[i] = new SceneChangeProcess(_changes[i]);
            }

            return new ProcessSequence(processes, "Scene Management Commands");
        }
    }
}