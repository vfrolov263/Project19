using UniRx;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Common
{
    public class CompnentsActivitySync : MonoBehaviour
    {
        [SerializeField]
        private MonoBehaviour _syncComponent;

        private void OnEnable()
        {
            _syncComponent.enabled = true;
        }

        private void OnDisable()
        {
            _syncComponent.enabled = false;
        }
    }
}