using N_Awakening.DungeonCrawler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    #region References

    [SerializeField] protected Teleport _otherTeleport;

    #endregion

    #region UnityMethods

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && other.GetComponent<PlayersAvatar>().CanTeleport)
        {
            other.transform.position= _otherTeleport.transform.position;
            other.GetComponent<PlayersAvatar>().Teleport();
        }
    }

    #endregion
}
