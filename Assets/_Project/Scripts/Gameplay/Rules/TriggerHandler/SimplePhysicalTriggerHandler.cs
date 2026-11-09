using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Rules.TriggerHandler
{
    public class SimplePhysicalTriggerHandler : MonoBehaviour
    {
        [SerializeField]
        private SimpleTriggerHandler _handler;
        [SerializeField]
        private string _tag;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_tag))
                _handler.Execute();

            Destroy(this);
        }
    }
}