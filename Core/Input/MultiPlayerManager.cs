using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Kiskovi.Core
{
    internal class MultiPlayerManager : MonoBehaviour
    {
        public PlayerInputManager playerInputManager;

        private static List<PlayerInput> players = new List<PlayerInput>();
        public static IEnumerable<PlayerInput> Players => players;

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
        }
    }
}
