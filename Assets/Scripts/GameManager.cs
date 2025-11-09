using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public int currentLevel = 1;

    public AudioSource audioSource;
    public AudioClip moveBlockSnd, correctMoveSnd, levelCompleteSnd;

    private GameObject[] blockList = new GameObject[0];
    private GameObject[] targetList = new GameObject[0];

    private bool[] wasBlockMoving = new bool[0];

    private bool isLoading = false;

    void Awake()
    {
        blockList = GameObject.FindGameObjectsWithTag("block");
        targetList = GameObject.FindGameObjectsWithTag("target");
        wasBlockMoving = new bool[blockList.Length];
    }

    void Update()
    {
        CheckBlockStates();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void CheckBlockStates()
    {
        if (isLoading) return;

        for (int i = 0; i < blockList.Length; i++)
        {
            Block b = blockList[i].GetComponent<Block>();
            if (!b.canMove)
            {
                continue;
            }

            if (wasBlockMoving[i])
            {
                if (b.state == Block.MoveStates.idle)
                {
                    OnBlockFinishMove(b);
                    wasBlockMoving[i] = false;
                }
            } 
            else
            {
                if (b.state == Block.MoveStates.moving)
                {
                    OnBlockStartMove(b);
                    wasBlockMoving[i] = true;
                }
            }
        }
    }

    private void OnBlockStartMove(Block b)
    {
        audioSource.PlayOneShot(moveBlockSnd);
    }

    private void OnBlockFinishMove(Block b)
    {
        float minDist = 10000f;
        foreach (GameObject target in targetList)
        {
            float dist = Vector3.Distance(b.transform.position, target.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
            }
        }

        if (minDist < 0.1f)
        {
            audioSource.PlayOneShot(correctMoveSnd);
            CheckWin();
        }
    }

    private void CheckWin()
    {
        if (isLoading) return;

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

        Debug.Log("Level " + currentLevel + " Complete");

        isLoading = true;
        StartCoroutine(NextLevelCo());
    }

    private IEnumerator NextLevelCo()
    {
        audioSource.PlayOneShot(levelCompleteSnd);

        yield return new WaitForSeconds(2f);

        if (currentLevel == 3)
        {
            SceneManager.LoadScene("WonScene");
        } 
        else
        {
            currentLevel += 1;
            SceneManager.LoadScene("Level" + currentLevel);
        }
    }

}
