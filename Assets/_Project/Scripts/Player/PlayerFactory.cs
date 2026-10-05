using Assets._Project.Scripts.Player.AdditionalControlls;
using DyrdaDev.FirstPersonController;
using UnityEngine;

namespace Assets._Project.Scripts.Player
{
    public class PlayerFactory
    {
        public PlayerController CreatePlayer(GameObject playerPrefab, Transform transform)
        {
            GameObject player = GameObject.Instantiate(playerPrefab, transform.position, transform.rotation);

            if (!player.TryGetComponent(out FirstPersonController fpController))
            {
                fpController = player.AddComponent<FirstPersonController>();
            }

            if (!player.TryGetComponent(out AdditionalController addController))
            {
                addController = player.AddComponent<AdditionalController>();
            }

            return new(fpController, addController, new(), new(null, null));
        }
    }
}