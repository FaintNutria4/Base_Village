using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public Transform entrancePos;
    public Transform exitPos;
    public Bed bed;
    public Transform door;
    private Animator doorAnimator;
    private HousesStateManager housesState;

    // Start is called before the first frame update
    void Start()
    {
        bed.SetUpHouse(this);
        doorAnimator = door.GetComponentInParent<Animator>();

        housesState = HousesStateManager.GetInstance();
        housesState.AddHouse(this);
    }

    public bool IsDoorOpen()
    {
        return doorAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "DoorOpen";
    }
}
