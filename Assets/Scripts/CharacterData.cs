using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public enum EmotionType
{
    Normal,
    Angry,
    Sad,
    Smile,
    Surprised
}

[Serializable]
public struct FaceDataEnum
{
    public EmotionType emotionType; 
    public Sprite faceSprite; // ここは画像データ
}

// プロジェクトウィンドウの右クリックメニューから作れるようにする
[CreateAssetMenu(fileName = "NewCharacter", menuName = "Story/CharacterData")]
public class CharacterData : ScriptableObject
{   
    public string characterName;        
    public List<FaceDataEnum> faces = new List<FaceDataEnum>();

    public Sprite GetFace(EmotionType emotion)
    {
        var target = faces.FirstOrDefault(f => f.emotionType == emotion);
        return target.faceSprite;
    }
}