using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class StubAI : MonoBehaviour, INeedStats
{
    private Stat _speedStat;

    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void InjectStatsHolder(StatsHolder statsHolder)
    {
        _speedStat = statsHolder.GetActualStat(StatType.Speed);
    }

    private void Update()
    {
        var playableCharacter = PlayableCharacterTracker.Instance.PlayableCharacters.FirstOrDefault();

        if (playableCharacter == null) return;
        
        _agent.speed = _speedStat.value;
        _agent.destination = playableCharacter.transform.position;
    }
}