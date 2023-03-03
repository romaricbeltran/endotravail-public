using System;
using UnityEngine;
using UnityEngine.Events;

// Composant qui créer un évènement onTriggerEnterEvent qui est une superposition de onTriggerEnter
// Le tuple permet de récupérer à la fois le collider et le sender sous la forme d'un unique argument (Invoke n'accepte qu'un argument)
// Dans un script, on peut ajouter le composant dynamiquement à un objet et écouter onTriggerEnterEvent pour récupérer son évènement onTriggerEnter
public class TriggerEvent : MonoBehaviour
{
    public UnityEvent<Tuple<GameObject, Collider>> onTriggerEnterEvent = new UnityEvent<Tuple<GameObject, Collider>>();
    [SerializeField] private int m_indexMission;

    public int GetIndexMission()
    {
        return m_indexMission;
    }

    public void SetIndexMission(int indexMission)
    {
        m_indexMission = indexMission;
    }

    void OnTriggerEnter(Collider other)
    {
        var sender = gameObject;
        var args = new Tuple<GameObject, Collider>(sender, other);
        onTriggerEnterEvent.Invoke(args);
    }
}