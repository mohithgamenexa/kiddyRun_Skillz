using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MissionData")]
public class MissionData : ScriptableObject
{
    public MissionSet[] missionSet;
}
[Serializable]
public class MissionSet
{
    public Mission[] _missionSet;
}
[Serializable]
public class Mission
{
    public string Tittle;
    public string Description;
    public string Reminder;
    public MissionType missionType;
    public bool inOneRun;
    public int targetCount;
}
public enum MissionType
{
    GOLD,
    RUN,
    JUMP,
    POWERUP,
    ROLL,
    WORDHUNT,
    MAGNET,
    Bicycle,
    SCORE,
    BicyclePurchase,
    JETPACK,
    MYSTERYBOX,
    KEY,
}
