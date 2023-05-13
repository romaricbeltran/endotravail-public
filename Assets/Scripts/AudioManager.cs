using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    void Awake()
    {
        // Vérifie si une autre instance de existe déjà, dans ce cas détruit cet objet pour s'assurer qu'il n'y a qu'une seule instance
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
