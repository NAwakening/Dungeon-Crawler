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
        [SerializeField] protected Transform[] _enemiespos;

        #endregion

        #region RunTimeMethods

        protected bool _spawnEnemy;

        #endregion

        #region UnityMethods

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _spawnEnemy = true;
                Debug.Log("el player entro");
                for (int i = 0; i < _enemies.Length; i++)
                {
                    if (_enemies[i].activeSelf)
                    {
                       _spawnEnemy = false;
                    }
                }
                if (_spawnEnemy)
                {
                    for (int i = 0; i < _enemies.Length; i++)
                    {
                        _enemies[i].SetActive(true);
                        _enemies[i].transform.position = _enemiespos[i].position;
                    }
                }
            }
        }

        #endregion
    }
}
