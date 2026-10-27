using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Rules.TriggerHandler
{
    public class SimpleTriggerHandler : MonoBehaviour, ITriggerHandler
    {
        [Serializable]
        private class TriggerObject
        {
            public GameObject gameObject;
            public bool isActivateTrigger = true;
        }

        [Serializable]
        private class TriggerComponent
        {
            public Component component;
            public bool isActivateTrigger = true;
        }

        [field: SerializeField]
        public string Name { get; private set; }
        [SerializeField]
        private List<TriggerObject> _triggerObjects;

        [SerializeField]
        private List<TriggerComponent> _triggerComponents;

        public void Execute()
        {
            foreach (var triggerObject in _triggerObjects)
            {
                if (triggerObject.gameObject == null)
                    continue;

                triggerObject.gameObject.SetActive(triggerObject.isActivateTrigger);
            }

            foreach (var triggerComponent in _triggerComponents)
            {
                Component component = triggerComponent.component;

                if (component == null)
                    continue;

                switch (component)
                {
                    case Behaviour behaviour:
                        behaviour.enabled = triggerComponent.isActivateTrigger;
                        break;
                    case Collider collider:
                        collider.enabled = triggerComponent.isActivateTrigger;
                        break;
                    case Renderer renderer:
                        renderer.enabled = triggerComponent.isActivateTrigger;
                        break;
                    // case AudioSource audio:
                    //     audio.enabled = triggerComponent.isActivateTrigger;
                    //     break;
                    case Cloth cloth:
                        cloth.enabled = triggerComponent.isActivateTrigger;
                        break;
                    default:
                        Debug.LogWarning($"Component {component.GetType().Name} can't be enabled or disabled");
                        break;
                }
            }
        }
    }
}