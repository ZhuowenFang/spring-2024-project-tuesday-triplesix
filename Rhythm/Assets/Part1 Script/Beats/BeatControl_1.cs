using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class BeatControl_1 : MonoBehaviour
{
    public GameObject circle;
    public TextMeshProUGUI scoreText;
    public bool updateScore = false;
    private float perfect = 0.5f;
    private float good = 1.0f;
    private float pass = 1.5f;
    private float health = 5;
    private ScoreController_1 sc;
    // Start is called before the first frame update

    void Start()
    {
        sc = scoreText.GetComponent<ScoreController_1>();
    }
    float GetAbs(float tar) 
    {
        if (tar <= 0) return -tar;
        else return tar;
    }

    int GetStatus(float distance)
    {
        GameObject[] platforms = GameObject.FindGameObjectsWithTag("elevator");
        GameObject platform = platforms[0];
        Platform pfu = platform.GetComponent<Platform>();

        Debug.Log("B");
        if (distance <= perfect) {
            if (updateScore) health -= 1f;
            pfu.allowMove();
            return 0;
        }
        else if (distance <= good) {
            if (updateScore) health -= 1f;
            pfu.allowMove();
            //platform.MoveUpCoroutine();
            return 1;
        }
        else if (distance <= pass) {
            if (updateScore) health -= 1f;
            pfu.allowMove();
            //platform.MoveUpCoroutine();
            return 2;
        }
        else {
            return 3;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float distance = 10;
        if (gameObject.CompareTag("Single") && Input.GetKeyDown(KeyCode.J)){
            distance = GetAbs(transform.position.x - circle.transform.position.x);
            // Debug.Log("dist:" + distance);
            int status = GetStatus(distance);
            Debug.Log(health);
            // Debug.Log("Jump: " + status);
            if (status < 3) gameObject.SetActive(false);
        } else if (gameObject.CompareTag("Long") && Input.GetKeyDown(KeyCode.J)){
            distance = GetAbs(transform.position.x - (transform.localScale.x / 2) - circle.transform.position.x);
            int status = GetStatus(distance);
            // Debug.Log("Begin shrink: " + status);
        } else if (gameObject.CompareTag("Long") && Input.GetKeyUp(KeyCode.J)) {
            distance = GetAbs(transform.position.x + (transform.localScale.x / 2) - circle.transform.position.x);
            int status = GetStatus(distance);
            // Debug.Log("After shrink: " + status);
            if (status < 3) gameObject.SetActive(false);
        }
        // Debug.Log("health: " + health);
    }
}
