using DilmerGames.Core.Extensions;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class ButtonTrigger : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onButtonPressed;

    private float delayPress = 0.5f;
    private float delayPressTmp;

    private bool pressedInProgress = false;

    private void Update()
    {
        if (!pressedInProgress)
            delayPressTmp += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.IsTriggerButton() && !pressedInProgress && delayPressTmp > delayPress)
        {
            pressedInProgress = true;
            onButtonPressed?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.IsTriggerButton() && pressedInProgress)
        {
            pressedInProgress = false;
            delayPressTmp = 0;
            Debug.Log("ALEDDDDDDDDDDDDDD");
        }
    }
}
