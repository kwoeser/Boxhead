using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    CharacterScriptableObject characterData;

    // stats
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentRecovery;
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentMight;
    [HideInInspector]
    public float currentProjectileSpeed;


    //Spawned wep
    public List<GameObject> spawnedWeapons;


    [Header("Experience/Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;

    [System.Serializable]
    public class LevelRange {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }


    // I=Frames 
    [Header("I-Frames")]
    public float invincibilityDuration;
    float invincibilityTimer;
    bool isInvincible;

    public List<LevelRange> levelRanges;

    void Awake() {


        characterData = CharacterSelector.GetData();
        CharacterSelector.instance.DestroySingleton();
        // Variabel Assignment
        currentHealth = characterData.MaxHealth;
        currentRecovery = characterData.Recovery;
        currentMoveSpeed = characterData.MoveSpeed;
        currentMight = characterData.Might;
        currentProjectileSpeed = characterData.ProjectileSpeed;
    
        SpawnWeapon(characterData.StartingWeapon);
    }


    void Start() {
        experienceCap = levelRanges[0].experienceCapIncrease;
    }

    void Update() {

        if (invincibilityTimer > 0) {
            invincibilityTimer -= Time.deltaTime;
        } 
        else if (isInvincible) {
            isInvincible = false;
        }

        Recover();
    }

    public void IncreaseExperience(int amount) {
        experience += amount;
    }

    void LevelUpChecker() {
        if (experience >= experienceCap) {
            level++;
            experience -= experienceCap;

            int experienceCapIncrease = 0;
            foreach (LevelRange range in levelRanges)
            {
                if(level >= range.startLevel && level <= range.endLevel) 
                {
                    experienceCapIncrease = range.experienceCapIncrease;
                    break;
                }
            }
            experienceCap += experienceCapIncrease;
        }
    }

    public void TakeDamage(float damage)
    {
        
        // If the player has been hit it will be invincible  
        if (!isInvincible) 
        {
            currentHealth -= damage;

            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            if (currentHealth <= 0)
            {
                Kill();
            }
        }
        
    }

    public void Kill()
    {
        Debug.Log("Player has died!");
    }
 

    public void RestoreHealth(float amount)
    {

        if (currentHealth < characterData.MaxHealth)
        {
            currentHealth += amount;

            if (currentHealth > characterData.MaxHealth) 
            {
                currentHealth = characterData.MaxHealth;
            }
        }
    }

    void Recover()
     {
        if (currentHealth < characterData.MaxHealth)
        {
            currentHealth += currentRecovery * Time.deltaTime;

        }
    }

    public void SpawnWeapon(GameObject weapon)
    {
        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform);
        spawnedWeapons.Add(spawnedWeapon);
    }
}
