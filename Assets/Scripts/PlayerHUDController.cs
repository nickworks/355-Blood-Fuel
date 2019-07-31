using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHUDController : MonoBehaviour {

    public PlayerHUD prefabHUD;
    static PlayerHUD hud;

    void Start()
    {
        if (!hud) hud = Instantiate(prefabHUD);
    }
    void OnDestroy()
    {
        if (hud) Destroy(hud.gameObject);
    }
}
