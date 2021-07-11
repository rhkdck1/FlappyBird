using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OneCommand : MonoBehaviour
{
    public AudioSource BirdSound_1;
    public AudioSource BirdSound_2;
    public AudioSource BirdSound_3;
    public AudioSource BirdSound_4;
    public GameObject m_objColumn;
    public GameObject m_objFloor;
    public GameObject m_objWhite;
    public GameObject m_objQuit;
    public GameObject[] gameObjects = new GameObject[3];
    public Text Score;

    Rigidbody2D m_rig;
    float nextTime;
    int j = 0;
    int i = 0;
    int iBest = 0;
    bool isStop = false;

    void Start()
    {
        m_rig = GetComponent<Rigidbody2D>();
        m_rig.AddForce(Vector3.up * 270);
        BirdSound_1.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        GetComponent<Animator>().SetFloat("Velocity", m_rig.velocity.y);
        if (transform.position.y > 4.75f)
        {
            transform.position = new Vector3(-1.5f, 4.75f, 0);
        }
        else if(transform.position.y < - 2.55f)
        {
            m_rig.simulated = false;
            GameOver();
        }

        //회전
        if (m_rig.velocity.y > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(transform.rotation.z, 30f, m_rig.velocity.y / 8));
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(transform.rotation.z, -90f, -m_rig.velocity.y / 8));
        }

        if(isStop)
        {
            return;
        }

        //입력
        if ((Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0))
        {
            m_rig.velocity = Vector3.zero;
            m_rig.AddForce(Vector3.up * 270);
            BirdSound_1.Play();
        }

        //기둥 생성기
        if(Time.time > nextTime)
        {
            nextTime = Time.time + 1.7f;
            gameObjects[j] = (GameObject)Instantiate(m_objColumn, new Vector3(4, Random.Range(-1f, 3.2f), 0), Quaternion.identity);
            if(++j == 3)
            {
                j = 0;
            }
        }

        if(gameObjects[0])
        {
            gameObjects[0].transform.Translate(-0.03f, 0, 0);
            if(gameObjects[0].transform.position.x < -4)
            {
                Destroy(gameObjects[0]);
            }
        }

        if (gameObjects[1])
        {
            gameObjects[1].transform.Translate(-0.03f, 0, 0);
            if (gameObjects[1].transform.position.x < -4)
            {
                Destroy(gameObjects[1]);
            }
        }

        if (gameObjects[2])
        {
            gameObjects[2].transform.Translate(-0.03f, 0, 0);
            if (gameObjects[2].transform.position.x < -4)
            {
                Destroy(gameObjects[2]);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Column(Clone)")
        {
            Score.text = (++i).ToString();
            BirdSound_2.Play();
        }
        else if(!isStop)
        {
            m_rig.velocity = Vector3.zero;
            BirdSound_4.Play();
            GameOver();
        }
    }

    void GameOver()
    {
        //게임오버
        if(!isStop)
        {
            BirdSound_3.Play();
        }
        isStop = true;
        m_objFloor.GetComponent<Animator>().enabled = false;
        m_objWhite.SetActive(true);
        Score.gameObject.SetActive(false);
        if(PlayerPrefs.GetInt("BestScore", 0) < int.Parse(Score.text))
        {
            PlayerPrefs.SetInt("BestScore", int.Parse(Score.text));
        }
        if(transform.position.y < -2.55f)
        {
            m_objQuit.SetActive(true);
            m_objQuit.transform.Find("ScoreScreen").GetComponent<Text>().text = Score.text;
            m_objQuit.transform.Find("BestScreen").GetComponent<Text>().text = PlayerPrefs.GetInt("BestScore").ToString();
        }
    }

    public void ReStart()
    {
        //재시작
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
