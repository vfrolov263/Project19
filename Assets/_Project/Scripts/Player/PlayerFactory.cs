using Assets._Project.Scripts.Gameplay.Animation;
using Assets._Project.Scripts.Player.Controlls;
using Assets._Project.Scripts.Player.Interaction;
using Assets._Project.Scripts.Player.View;
using Unity.Cinemachine;
using UnityEngine;

namespace Assets._Project.Scripts.Player
{
    public class PlayerFactory
    {
        public PlayerController CreatePlayer(GameObject playerPrefab, Transform transform)
        {
            GameObject player = GameObject.Instantiate(playerPrefab, transform.position, transform.rotation);

            if (!player.TryGetComponent(out FirstPersonInputController controller))
            {
                controller = player.AddComponent<FirstPersonInputController>();
            }

            Camera _glassesCam = Camera.main;

            return new(controller, new(), new(_glassesCam.GetComponentInChildren<Animator>(),
                new CameraInteractor(_glassesCam, mask: LayerMask.GetMask("AlterWorld"), rayOffset: new(.5f, .5f)),
                _glassesCam.GetComponentInChildren<AnimationEventsHandler>(), "readyToUse", "motionStart", "motionEnd"),
                player.GetComponentInChildren<CinemachineCamera>(), player.GetComponentInChildren<ClueController>());
        }
    }
}