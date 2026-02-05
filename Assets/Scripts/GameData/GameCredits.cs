using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CreditsElement
{
    public string name;
    public string contribution;
}

[CreateAssetMenu(fileName = "GameCredits", menuName = "Scriptable Objects/GameCredits")]
public class GameCredits : ScriptableObject
{
    public List<CreditsElement> credits = new List<CreditsElement>();
}
