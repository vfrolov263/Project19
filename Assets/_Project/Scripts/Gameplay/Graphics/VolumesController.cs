using Assets._Project.Scripts.Player;
using Unity.Cinemachine;
using UnityEngine;
using VContainer;

namespace Assets._Project.Scripts.Gameplay.Graphics
{
    [RequireComponent(typeof(CinemachineCamera))]
    public class VolumesController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _unfocusRealWorldVolume;
        private PlayerController _playerController;

        [Inject]
        private void Construct(PlayerController playerController)
        {
            _playerController = playerController;
        }

        private void OnEnable()
        {
            _playerController.WorldSwitched += OnWorldSwitched;
        }

        private void OnDisable()
        {
            _playerController.WorldSwitched -= OnWorldSwitched;
        }

        private void OnWorldSwitched(bool isAlter)
        {
            _unfocusRealWorldVolume.SetActive(isAlter);
        }
    }
}