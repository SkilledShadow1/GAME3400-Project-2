using System.ComponentModel;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private float timeSpikeUp;
    [SerializeField] private float timeSpikeDown;
    
    private bool spikesUp;
    private float timeSpent;
    private Animator animator;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        timeSpent = 0;
    }

    private void Update()
    {
        if (spikesUp) {
            if (timeSpent >= timeSpikeUp) {
                timeSpent = 0;
                animator.Play("SpikesDown");
                spikesUp = false;
            } else {
                timeSpent += Time.deltaTime;
            }
        } else {
            if (timeSpent >= timeSpikeDown) {
                timeSpent = 0;
                animator.Play("SpikesUp");
                spikesUp = true;
            } else {
                timeSpent += Time.deltaTime;
            }
        }
        
    }
}
