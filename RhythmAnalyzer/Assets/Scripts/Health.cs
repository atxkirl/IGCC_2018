using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;

    public void ModifyCurrentHealth(int _amountToModify)
    {
        currentHealth += _amountToModify;

        Numbers.Instance.ClampBetweenValues(ref currentHealth, 0, maxHealth);
    }
}
