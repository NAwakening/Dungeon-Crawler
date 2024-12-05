using N_Awakening.DungeonCrawler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heart : MonoBehaviour
{
    #region UnityMethods

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayersAvatar>().GainLife();
            gameObject.SetActive(false);
        }
    }

    #endregion
}
