using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool SuperMobDefeated = false;
    public bool BossDefeated = false;

    void Start()
    {
       GetComponent<SpriteRenderer>().enabled = false;
    }

    public void DefeatBoss() {
        BossDefeated = true;
    }

    public void DefeatSuperMob() {
        SuperMobDefeated = true;
    }

    public void aliveBoss() {
        BossDefeated = false;
    }

    public void aliveSuperMob() {
        SuperMobDefeated = false;
    }

    public void ActivateFinishLine()
    {
        if (SuperMobDefeated && BossDefeated) {
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}

