using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameObject[] blockList = new GameObject[0];
    private static GameObject[] targetList = new GameObject[0];

    void Awake()
    {
        blockList = GameObject.FindGameObjectsWithTag("block");
        targetList = GameObject.FindGameObjectsWithTag("target");
    }

    void Update()
    {
        CheckWin();
    }

    public static void CheckWin()
    {
        foreach (GameObject go in blockList)
        {
            Block b = go.GetComponent<Block>();
            if (!b.canMove)
            {
                continue;
            }

            if (b.state != Block.MoveStates.idle)
            {
                return;
            }

            float minDist = 10000f;
            foreach (GameObject target in targetList)
            {
                float dist = Vector3.Distance(go.transform.position, target.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                }
            }

            if (minDist > 0.1f)
            {
                return;
            }
        }

        Debug.Log("Win");
    }

}
