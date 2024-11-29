using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace N_Awakening.DungeonCrawler
{
    public class EnemyPool : MonoBehaviour
    {
        #region References

        [SerializeField] protected GameObject[] _enemies;

        #endregion

        #region UnityMethods

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("el player entro");
                for (int i = 0; i < _enemies.Length; i++)
                {
                    if (!_enemies[i].activeSelf)
                    {
                        _enemies[i].SetActive(true);
                    }
                }
            }
        }

        #endregion
    }
}
