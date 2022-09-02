using UniRx;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.Input
{
    public interface IInputService
    {
        ReactiveProperty<Vector3> InputVector { get; set; }
    }
}