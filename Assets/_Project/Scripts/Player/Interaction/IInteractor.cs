using UnityEngine;

namespace Assets._Project.Scripts.Player.Interaction
{
    public interface IInteractor
    {
        Ray Ray { get; }

        LayerMask Mask { get => default; } 
    }
}