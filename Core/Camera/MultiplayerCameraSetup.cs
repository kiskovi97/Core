using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public sealed class MultiplayerCameraSetup : MonoBehaviour
{
    private static readonly HashSet<OutputChannels> UsedChannels = new();

    [SerializeField]
    private CinemachineBrain brain;

    [SerializeField]
    private CinemachineCamera cinemachineCamera;

    private OutputChannels assignedChannel;

    private void OnEnable()
    {
        if (brain == null || cinemachineCamera == null)
        {
            Debug.LogError(
                "MultiplayerCameraSetup requires a CinemachineBrain and CinemachineCamera.",
                this
            );
            return;
        }

        for (var index = 0; index < 32; index++)
        {
            var channel = (OutputChannels)(1u << index);
            if (!UsedChannels.Add(channel))
                continue;

            assignedChannel = channel;
            brain.ChannelMask = channel;
            cinemachineCamera.OutputChannel = channel;
            return;
        }

        Debug.LogError("No unused Cinemachine output channels are available.", this);
    }

    private void OnDisable()
    {
        if (assignedChannel == OutputChannels.Default)
            return;

        UsedChannels.Remove(assignedChannel);
        assignedChannel = OutputChannels.Default;
    }
}
