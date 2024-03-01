using Cinemachine;
using UnityEngine;

public class PlayableCharacterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private GameObject virtualCameraPrefab;

    private void OnEnable()
    {
        Spawn();
    }

    private void Spawn()
    {
        var selfTransform = transform;
        var characterInstance = Pool.Instance.Spawn(characterPrefab, selfTransform.position, selfTransform.rotation);

        var cameraInstance = Pool.Instance.Spawn(virtualCameraPrefab);
        var virtualCamera = cameraInstance.GetComponent<CinemachineVirtualCamera>();
        virtualCamera.Follow = characterInstance.transform;
        virtualCamera.LookAt = characterInstance.transform;
        
        PlayableCharacterTracker.Instance.Register(characterInstance);
    }
}
