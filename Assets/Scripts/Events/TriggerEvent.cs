using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    public UnityEvent<Collider> onTriggerEnterEvent = new UnityEvent<Collider>();

    void OnTriggerEnter(Collider other)
    {
        onTriggerEnterEvent.Invoke(other);
    }
}