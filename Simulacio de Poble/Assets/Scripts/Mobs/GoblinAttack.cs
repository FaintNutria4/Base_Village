using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAttack : MonoBehaviour
{
    public float attackDistance;
    public float attackDamage;
    public void Attack(GoblinController goblinController)
    {
        Transform head = goblinController.head;
        Ray ray = new Ray(head.position, head.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, attackDistance))
        {
            hitInfo.transform.gameObject.TryGetComponent(out HealthManager interactable);

            List<Item_Info> newLoot = interactable.TakeDamage(attackDamage);
            if (newLoot != null) goblinController.goblinHealth.loot.AddRange(newLoot);
        }

    }
}
