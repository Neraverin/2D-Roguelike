using UnityEngine;

public class Loader : MonoBehaviour 
{

    public GameObject GameManager;

    void Awake () 
    {
        if (global::GameManager.Instance == null)
        {
            Instantiate(GameManager);
        }
    }
}
