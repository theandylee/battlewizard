using System.Collections.Generic;
using UnityEngine;

public class PlayableCharacterTracker
{
    public static PlayableCharacterTracker Instance
    {
        get { return _instance ??= new PlayableCharacterTracker(); }
    }

    private static PlayableCharacterTracker _instance;
    
    private List<GameObject> _playableCharacters = new();

    public IEnumerable<GameObject> PlayableCharacters => _playableCharacters;
    
    public  void Register(GameObject playableCharacter)
    {
        if (_playableCharacters.Contains(playableCharacter)) return;
        
        _playableCharacters.Add(playableCharacter);

        playableCharacter.GetComponent<HP>().onDeath += OnDeath;
    }

    private void OnDeath(GameObject deadPlayableCharacter)
    {
        _playableCharacters.Remove(deadPlayableCharacter);
    }
}
