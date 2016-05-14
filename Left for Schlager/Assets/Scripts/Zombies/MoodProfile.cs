using UnityEngine;
/// <summary>
/// Class for storing info about each mood zombies can have
/// </summary>
[System.Serializable]
public class MoodProfile {
    public float runSpeed;
    public float thinkTime;
    public bool playerDetectorEnabled;
    public float giveUpDistance;
    public Color eyeColor;
    public float strayDistance;
    public bool isAggressive;
}