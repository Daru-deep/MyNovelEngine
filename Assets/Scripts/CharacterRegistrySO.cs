using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterRegistry", menuName = "Story/CharacterRegistry")]
public class CharacterRegistrySO : ScriptableObject
{
    public List<CharacterData> characters = new List<CharacterData>();

    public CharacterData Find(string characterName)
    {
        return characters.Find(c => c.characterName == characterName);
    }
}
