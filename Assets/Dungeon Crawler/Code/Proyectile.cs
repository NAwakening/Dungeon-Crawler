using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectile : MonoBehaviour
{
    #region Knobs

    [SerializeField] protected float movementSpeed, duration;

    #endregion

    #region References

    [SerializeField, HideInInspector] protected Rigidbody2D _rigidbody;

    #endregion

    #region RunTimeMethods

    protected Transform _playerPosition;

    #endregion

    #region UnityMethods

    void FixedUpdate()
    {
        _rigidbody.velocity = (_playerPosition.position - transform.position).normalized * movementSpeed;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnEnable()
    {
        StartCoroutine(TimerToDestruction());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        { 
            gameObject.SetActive(false);
        }
    }

    #endregion

    #region Corrutinas

    IEnumerator TimerToDestruction()
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }

    #endregion

    #region GettersAndSetters

    public Transform SetPlayerPosition
    {
        set { _playerPosition = value; }
    }

    #endregion
}
