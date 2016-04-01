using UnityEngine;

namespace Assets.Scripts
{
    public class Loader : MonoBehaviour 
    {
        public GameObject GameManager;

        void Awake () 
        {
            if (global::Assets.Scripts.GameManager.Instance == null)
            {
                Instantiate(GameManager);
            }
        }
    }
}
