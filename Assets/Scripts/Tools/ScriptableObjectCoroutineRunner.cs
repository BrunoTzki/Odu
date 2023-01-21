using System.Collections;
using UnityEngine;

namespace Tools
{
    public class ScriptableObjectCoroutineRunner : MonoBehaviour
    {
        public static ScriptableObjectCoroutineRunner Instance;

        private void Awake()
        {
            if (Instance is null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        public void RunCoroutine(IEnumerator coroutine)
        {
            StartCoroutine(coroutine);
        }
    }
}