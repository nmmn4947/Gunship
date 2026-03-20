using UnityEngine;

public class PlayerChaingun
{
    private ShipData shipData;
    private float currentFireRate = 0;
    private float timer;
    private GameObject playerObj;
    private float stopShootingThreshold = 3;
    
    public void SetUp(GameObject player, ShipData shipData)
    {
        playerObj = player;
        this.shipData = shipData;
        currentFireRate = stopShootingThreshold;
    }
    
    public void UpdateGun()
    {
        timer -= Time.deltaTime; // should be +
        if (timer < 0 && currentFireRate > 0 && currentFireRate < stopShootingThreshold)
        {
            //fire a gun
            Object.Instantiate(shipData.bulletPrefab, playerObj.transform.position, playerObj.transform.rotation);
            timer = currentFireRate;
        }
    }

    public void RampHandlingChainGun(bool isRampUp)
    {
        if (isRampUp)
        {
            currentFireRate = Mathf.MoveTowards(currentFireRate, shipData.maxFireTimeEachShot, shipData.rampUpSpeed * Time.deltaTime);
        }
        else
        {
            currentFireRate = Mathf.MoveTowards(currentFireRate, stopShootingThreshold + 1, shipData.rampDownSpeed * Time.deltaTime);
        }
        Debug.Log(currentFireRate);
    }
}
