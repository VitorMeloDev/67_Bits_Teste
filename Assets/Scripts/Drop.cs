using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    public Rigidbody myRb;
    
    public void DropAddForce()
    {
        Vector3 force = new Vector3(Random.Range(5,15),Random.Range(2,5),Random.Range(5,15));
        myRb.AddForce(force, ForceMode.Impulse);
        Destroy(this.gameObject, 5f);
    }
}
