using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAttack : MonoBehaviour
{
    public float attackDistance;
    public float attackDamage;

    public float attackSpeed;

    public float nextAttackTime = 0;
    public bool Attack(GoblinController goblinController)
    {
        if (Time.time < nextAttackTime) return false;
        Transform head = goblinController.head;
        Ray ray = new Ray(head.position, head.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, attackDistance))
        {


            if (hitInfo.transform.gameObject.TryGetComponent(out HealthManager interactable))
            {
                interactable.TakeDamage(goblinController.goblinHealth.loot, attackDamage);
                nextAttackTime = Time.time + attackSpeed;

                if (hitInfo.transform == null) return true;

            }
        }

        return false;

    }
}
