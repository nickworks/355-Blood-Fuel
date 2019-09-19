using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarWeapons : MonoBehaviour {

    public Transform weaponSlot;
    public Weapon[] weaponLoadout;
    int weaponIndex = 0;
    Weapon currentWeapon;

    void Start() {
        
    }

    void Update() {
        
    }
    void NextWeapon() {
        weaponIndex++;
        LoadWeapon();
    }
    void PrevWeapon() {
        weaponIndex--;
        LoadWeapon();
    }
    void LoadWeapon() {
        if (weaponIndex < 0) weaponIndex = weaponLoadout.Length - 1;
        if (weaponIndex < 0) return; // no weapons...
        if (weaponIndex == weaponLoadout.Length) weaponIndex = 0;

        Weapon nextWeapon = weaponLoadout[weaponIndex];

        if (nextWeapon == null) return; // weapon is null...

        if (currentWeapon.gameObject != null) Destroy(currentWeapon);

        currentWeapon = Instantiate(nextWeapon, weaponSlot);
    }
}
