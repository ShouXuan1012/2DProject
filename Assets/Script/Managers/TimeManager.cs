using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private class CooldownInfo
    {
        public float duration;
        public float remaining;
    }

    private Dictionary<GameObject, CooldownInfo> cooldownDict = new();

    public void StartCooldown(GameObject owner, float duration)
    {
        if (!cooldownDict.ContainsKey(owner))
            cooldownDict[owner] = new CooldownInfo();

        cooldownDict[owner].duration = duration;
        cooldownDict[owner].remaining = duration;
    }

    public bool IsCooldown(GameObject owner)
    {
        return cooldownDict.ContainsKey(owner) && cooldownDict[owner].remaining > 0f;
    }

    public float GetRemaining(GameObject owner)
    {
        if (!cooldownDict.ContainsKey(owner)) return 0f;
        return cooldownDict[owner].remaining;
    }

    public float GetDuration(GameObject owner)
    {
        if (!cooldownDict.ContainsKey(owner)) return 0f;
        return cooldownDict[owner].duration;
    }

    private void Update()
    {
        foreach (var kvp in cooldownDict)
        {
            if (kvp.Value.remaining > 0f)
                kvp.Value.remaining -= Time.deltaTime;
        }
    }   
}
