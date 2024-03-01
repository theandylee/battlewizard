using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (!Input.anyKeyDown) return;
        
        SceneManager.LoadScene("World");
        Pool.Instance.Reset();
    }
}