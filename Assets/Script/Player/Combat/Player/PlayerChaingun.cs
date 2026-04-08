using UnityEngine;
using Napadol.Tools;

public class PlayerChaingun
{
    private ShipData shipData;
    private float timer;
    private GameObject playerObj;
    private bool pressedOnce;
    public float currentMultiplier;

    public void SetUp(GameObject player, ShipData shipData)
    {
        playerObj = player;
        this.shipData = shipData;
        currentMultiplier = 0;
    }
    
    public void UpdateGun()
    {
        timer += Time.deltaTime * currentMultiplier; // should be +
        if (timer > shipData.maxFireRate)
        {
            //fire a gun
            float currentCone = Mathf.Lerp(0, shipData.maxConeShotOffset, Easing.EaseInCirc(currentMultiplier/shipData.maxRampUp));
            float offset = UnityEngine.Random.Range(-currentCone, currentCone);
            Quaternion shootRot = new Quaternion(playerObj.transform.rotation.x, playerObj.transform.rotation.y, playerObj.transform.rotation.z + offset,  playerObj.transform.rotation.w);
            GameObject spwn = Object.Instantiate(shipData.bulletPrefab, playerObj.transform.position, shootRot);
            spwn.GetComponent<SimpleDamager>().combatTeam = Health.CombatTeam.Player;
            //BulletPool.instance.SpawnBullet(playerObj.transform.position, shootRot);
            timer -= shipData.maxFireRate;
        }
    }
    
    public void RampHandlingChainGun(bool isRampUp)
    {
        if (isRampUp)
        {
            currentMultiplier = Mathf.MoveTowards(currentMultiplier, shipData.maxRampUp, shipData.rampUpStepMultiplier * Time.deltaTime);
            /*if (!pressedOnce)
            {
                Object.Instantiate(shipData.bulletPrefab, playerObj.transform.position, playerObj.transform.rotation);
                pressedOnce = true;
            }*/
        }
        else
        {
            currentMultiplier = Mathf.MoveTowards(currentMultiplier, 0, shipData.rampDownStepMultiplier * Time.deltaTime);
            //pressedOnce = false;
        }
        //Debug.Log(currentFireRate);
    }
}
