using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health;
    public PlayerMove player;
    public GameObject[] stages;

    public Image[] UIhealth;
    public Text UIPoint;
    public Text UIStage;
    public Text UImaxPoint;
    public GameObject RestartBtn;
    public GameObject GameOverBtn;

    private void Update()
    {
        UIPoint.text = (totalPoint + stagePoint).ToString();
    }

    public void NextStage()
    {
        //chang stage
        if (stageIndex < stages.Length - 1)
        {
            stages[stageIndex].SetActive(false);
            stageIndex++;
            stages[stageIndex].SetActive(true);
            health = 3;
            UIhealth[0].color = new Color(1, 1, 1, 1);
            UIhealth[1].color = new Color(1, 1, 1, 1);
            UIhealth[2].color = new Color(1, 1, 1, 1);
            UIStage.text = "STAGE " + (stageIndex + 1);

            player.transform.position = new Vector3(-9.5f, -4.55f, -1);

            StartCoroutine(waittime());
            
        }
        else {//game claer
              //player stop
            Time.timeScale = 0;
            //Result ui
            Debug.Log("게임을 클리어하셨습니다!");
            //Restart ui
            RestartBtn.SetActive(true);
            Text btnText = RestartBtn.GetComponentInChildren<Text>();
            btnText.text = "Clear!";
            RestartBtn.SetActive(true);
            //calculate Point
            totalPoint += stagePoint;
            stagePoint = 0;
            //health = 3;
            UImaxPoint.text = "점수 : " + totalPoint.ToString();
        }
        //calculate Point
        totalPoint += stagePoint;
        stagePoint = 0;
        health = 3;
    }
    public void HealthDown() {
        if (health > 1) {
            health--;
            UIhealth[health].color = new Color(1, 0, 0, 0.4f);
        } else {
            //all health ui off
            UIhealth[0].color = new Color(1, 0, 0, 0.4f);
            //plyer die effect
            player.OnDie();
            //Result ui
            Debug.Log("죽었습니다!");
            //Restart button ui
            GameOverBtn.SetActive(true);
            Text gbtnText = GameOverBtn.GetComponentInChildren<Text>();
            gbtnText.text = "Game over!";
            GameOverBtn.SetActive(true);
        }
    }
    
    void PlyerReposition() {
        player.transform.position = new Vector3(-9.5f, -4.55f, -1);
        player.VelocityZero();
    }

    public void Restart() {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void Gameover()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
    public void helthup() {
        if (health < 3)
        {
            health++;
            UIhealth[health-1].color = new Color(1, 1, 1, 1);
        }
    }
    IEnumerator waittime() {
        yield return new WaitForSeconds(1f);
        player.stageend = false;
    }
}
