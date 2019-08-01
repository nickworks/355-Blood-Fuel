
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    static public PlayerManager activeManager;
    static public List<DriverPlayer> activePlayers = new List<DriverPlayer>();

    public FollowTarget camera;

    public Car playerCarPrefab;
    public GameObject prefabGuiGameOver;

    void Start()
    {
        activeManager = this;
        SpawnPlayer();
    }
    void SpawnPlayer() {

        DriverPlayer player = new DriverPlayer();
        Car car = Instantiate(playerCarPrefab);
        player.TakeControl(car);

        activePlayers.Add(player);

        camera.target = car.transform;
    }

    public static DriverPlayer FindNearest(Vector3 position) {
        float nearestPlayerDistance = 0;
        DriverPlayer nearestPlayer = null;
        for (int i = 0; i < activePlayers.Count; i++) {
            DriverPlayer player = activePlayers[i];
            if (player.car == null) continue;
            float dis2 = (player.car.transform.position - position).sqrMagnitude;
            if(i == 0 || dis2 < nearestPlayerDistance) {
                nearestPlayerDistance = dis2;
                nearestPlayer = player;
            }
        }
        return nearestPlayer;
    }

    static public void Remove(DriverPlayer player) {
        activePlayers.Remove(player);
        if (activePlayers.Count == 0) activeManager.GameOver();
    }
    static public DriverPlayer PickRandom() {
        if (activePlayers.Count == 0) return null;
        return activePlayers[Random.Range(0, activePlayers.Count)];
    }
    public void GameOver() {
        if(prefabGuiGameOver != null) Instantiate(prefabGuiGameOver);
    }
}
