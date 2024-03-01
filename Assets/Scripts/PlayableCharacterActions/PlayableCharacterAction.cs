using UnityEngine;

public class PlayableCharacterAction : MonoBehaviour, INeedInputActions, INeedStats
{
    protected StatsHolder statsHolder;
    protected InputActions inputActions;
    
    public void InjectInputActions(InputActions inputActions)
    {
        this.inputActions = inputActions;
    }

    public void InjectStatsHolder(StatsHolder statsHolder)
    {
        this.statsHolder = statsHolder;
    }
}