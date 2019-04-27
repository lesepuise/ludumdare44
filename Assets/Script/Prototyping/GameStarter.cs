using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CleverCode;

namespace CleverCode
{
    public class GameStarter : MonoBehaviour
    {   
        [CheapReorderable]
        public List<GameObject> prefabToInstantiate;

        void Awake()
        {
            InitGame();
        }

        private void InitGame()
        {
            DontDestroyOnLoad(gameObject);

            name = "Managers";

            foreach (GameObject prefab in prefabToInstantiate)
            {
                if (!prefab)
                {
                    Debug.LogError("Empty prefab in GameStarter's list");
                    continue;
                }

                GameObject startElement = Instantiate(prefab, transform);
                TruncateName(startElement);
            }
        }

        private void TruncateName(GameObject go)
        {
            go.name = go.name.Remove(go.name.Length - 7); //Remove the "(Clone)" from the name
        }
    }
}