using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowBack : MonoBehaviour
{
    public Transform target; // Référence au transform du personnage
    public float smoothSpeed = 0.125f; // Vitesse de mouvement de la caméra
    public Vector3 offset; // Offset de la caméra par rapport au personnage

    void LateUpdate()
    {
        Quaternion desiredRotation = target.rotation;
        Quaternion smoothedRotation = Quaternion.Lerp(transform.rotation, desiredRotation, smoothSpeed);
        transform.rotation = smoothedRotation;
    }
}
