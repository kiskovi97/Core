using System.Collections.Generic;

using Unity.Cinemachine;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Kiskovi.Core
{
    internal class MultiPlayerManager : MonoBehaviour
    {
        public PlayerInputManager playerInputManager;
        public List<LayerMask> playerLayers = new List<LayerMask>();

        private List<PlayerInput> players = new List<PlayerInput>();

        private void OnEnable()
        {
            playerInputManager.onPlayerJoined += PlayerInputManager_onPlayerJoined;
        }

        private void OnDisable()
        {
            playerInputManager.onPlayerJoined -= PlayerInputManager_onPlayerJoined;
        }

        private void PlayerInputManager_onPlayerJoined(PlayerInput obj)
        {
            players.Add(obj);

            var layer = (int)Mathf.Log(playerLayers[players.Count - 1], 2);

            obj.camera.cullingMask |= 1 << layer;
            obj.camera.transform.parent.GetComponentInChildren<CinemachineCamera>().gameObject.layer = layer;
        }
    }
}
