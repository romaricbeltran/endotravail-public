using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @characterName : Nom du personnage qui prend la parole
// @sentences : Phrases du personnage qui prend la parole
[System.Serializable]
public class Dialogue
{
    [SerializeField] private List<Monologue> monologues;

    public List<Monologue> GetMonologues()
    {
        return monologues;
    }
}
