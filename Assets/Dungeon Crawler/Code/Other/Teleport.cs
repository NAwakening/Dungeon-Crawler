using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    #region References

    [SerializeField] protected Teleport _otherTeleport;

    #endregion

    #region publicMethhods

    public void PlayerTeleport(Transform player)
    {
        player.position = _otherTeleport.transform.position;
    }

    #endregion
}
