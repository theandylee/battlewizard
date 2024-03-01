using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs = new();
    [SerializeField] private int maxNumber;
    [SerializeField] private Vector2 distanceRange;
    [SerializeField] private float spawnPeriod;

    private List<GameObject> _instances = new();

    private float _timer;

    private void OnEnable()
    {
        Spawn();
    }

    private void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer > 0f) return;
        
        _timer = spawnPeriod;
            
        if (_instances.Count < maxNumber)
            Spawn();
    }

    private void Spawn()
    {
        var playableCharacter = PlayableCharacterTracker.Instance.PlayableCharacters.FirstOrDefault();
        if (playableCharacter == null) return;
        
        var randomDirection = (Vector3)Random.insideUnitCircle.normalized;
        randomDirection.z = randomDirection.y;
        randomDirection.y = 0f;
        randomDirection *= Random.Range(distanceRange.x, distanceRange.y);
        
        for (var angle = 0f; angle < 360f; angle += 30f)
        {
            var randomPosition = playableCharacter.transform.position + Quaternion.Euler(0f, angle, 0f) * randomDirection;

            NavMesh.SamplePosition(randomPosition, out var hit, distanceRange.y, 1);
            
            if (Vector3.Distance(hit.position, playableCharacter.transform.position) < distanceRange.x) continue;


            var instance = Pool.Instance.Spawn(prefabs.Random(), hit.position);
            _instances.Add(instance);
            instance.GetComponent<HP>().onDeath += OnDeath;
            break;
        }
    }
    
    private void OnDeath(GameObject enemy)
    {
        enemy.GetComponent<HP>().onDeath -= OnDeath;
        _instances.Remove(enemy);
        
        Spawn();
    }
}
