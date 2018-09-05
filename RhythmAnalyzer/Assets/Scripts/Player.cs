using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SingletonMonoBehaviour<Player>
{
    public int currentHealth;
    public int maxHealth;
    public bool isDead;

    private void Update()
    {
        if (currentHealth <= 0)
            isDead = true;
    }

    public void ModifyCurrentHealth(int _amountToModify)
    {
        currentHealth += _amountToModify;

        Numbers.Instance.ClampBetweenValues(ref currentHealth, 0, maxHealth);
    }

    public void InstaKill()
    {
        currentHealth = 0;
    }
}
