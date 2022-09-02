using UniRx;
using Unity.VisualScripting;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.Input
{
    public class InputService : IInputService
    {
        public ReactiveProperty<Vector3> InputVector { get; set; } = new();
        private Vector3 _lastInput;

        public InputService()
        {
            Observable
            .EveryUpdate()
            .Subscribe(_ => ProcessInput());
        }
            

        private void ProcessInput()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse0))
            {
                _lastInput = UnityEngine.Input.mousePosition;
            }
            else if (UnityEngine.Input.GetKey(KeyCode.Mouse0))
            {
                var currentInput = UnityEngine.Input.mousePosition;
                var delta = currentInput - _lastInput;
                InputVector.Value = delta;
                _lastInput = currentInput;
            }
        }
    }
}