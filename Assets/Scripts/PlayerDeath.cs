using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Boulder")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
