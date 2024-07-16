using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Ships")]
    public GameObject[] ships;
    public EnemyScript enemyScript;
    private ShipScript shipScript;
    private List<int[]> enemyShips;
    private int shipIndex = 0;
    public List<TileScript> allTileScripts;    

    [Header("HUD")]
    public Button nextBtn;
    public Button rotateBtn;
    public Button replayBtn;
    public Button exitBtn;

    public Text topText;
    public Text playerShipText;
    public Text enemyShipText;

    [Header("Objects")]
    public GameObject missilePrefab;
    public GameObject enemyMissilePrefab;
    public GameObject firePrefab;
    public GameObject woodDock;

    //variabili aggiunte
    
    public GameObject nave1;
    public GameObject nave2;
    public GameObject nave3;
    public GameObject nave4;
    public GameObject nave5;

    private int cont;
    
    //fin qua

    private bool setupComplete = false;
    private bool playerTurn = true;
    
    private List<GameObject> playerFires = new List<GameObject>();
    private List<GameObject> enemyFires = new List<GameObject>();
    
    private int enemyShipCount = 5;
    private int playerShipCount = 5;

    // Start is called before the first frame update

    [SerializeField] private AudioSource source;
    [SerializeField] private AudioSource sourceDamage;
    [SerializeField] private AudioSource sourceMissed;

    void Start()
    {//modifica cont
        cont = 1;
        shipScript = ships[shipIndex].GetComponent<ShipScript>();
        nextBtn.onClick.AddListener(() => NextShipClicked());
        rotateBtn.onClick.AddListener(() => RotateClicked());
        replayBtn.onClick.AddListener(() => ReplayClicked());
        enemyShips = enemyScript.PlaceEnemyShips();
        
    }

    private void Update()
    {
        
    }

    private void NextShipClicked()
    {
        if (!shipScript.OnGameBoard())
        {
            shipScript.FlashColor(Color.red);
        } else
        {
            if(shipIndex <= ships.Length - 2)
            {
                shipIndex++;
                shipScript = ships[shipIndex].GetComponent<ShipScript>();
                shipScript.FlashColor(Color.yellow);
            }
            else
            {
                rotateBtn.gameObject.SetActive(false);
                nextBtn.gameObject.SetActive(false);
                woodDock.SetActive(false);
                topText.text = "Seleziona la cella da attaccare";
                setupComplete = true;
                for (int i = 0; i < ships.Length; i++) ships[i].SetActive(false);
            }
        }
    }

    public void TileClicked(GameObject tile)
    {
        if(setupComplete && playerTurn)
        {
            Vector3 tilePos = tile.transform.position;
            tilePos.y += 15;
            playerTurn = false;
            Instantiate(missilePrefab, tilePos, missilePrefab.transform.rotation);
        } else if (!setupComplete)
        {
            PlaceShip(tile);
            shipScript.SetClickedTile(tile);
        }
    }

    private void PlaceShip(GameObject tile)
    {
        shipScript = ships[shipIndex].GetComponent<ShipScript>();
        shipScript.ClearTileList();
        Vector3 newVec = shipScript.GetOffsetVec(tile.transform.position);
        ships[shipIndex].transform.localPosition = newVec;
    }

    void RotateClicked()
    {
        shipScript.RotateShip();
    }

    public void CheckHit(GameObject tile)
    {
        int tileNum = Int32.Parse(Regex.Match(tile.name, @"\d+").Value);
        Debug.Log("tilenum:" + tileNum);
        int tilesum = tileNum + 100;
        GameObject tile1 = GameObject.Find("Tile " + "(" + tilesum + ")");
        int hitCount = 0;
        int cont = 0;
        foreach(int[] tileNumArray in enemyShips)
        {
            //Debug.Log("tilenumArray:" + tileNumArray[0]);
            //Debug.Log("tilenumArray1:" + tileNumArray[1]);
            cont++;
            if (tileNumArray.Contains(tileNum))
            {
                for (int i = 0; i < tileNumArray.Length; i++)
                {
                    if (tileNumArray[i] == tileNum)
                    {
                        tileNumArray[i] = -5;
                        hitCount++;
                    }
                    else if (tileNumArray[i] == -5)
                    {
                        hitCount++;
                    }
                }
                if (hitCount == tileNumArray.Length)
                {
                    //QUA QUANDO E sunk DEVI RENDERE VISIBILE LA NAVE SU UN'ALTRA GRIGLIA!
                    enemyShipCount--;
                    topText.text = "AFFONDATA!!!!!!";
                    //Qua setti attive le barche affondate sulla griglia avversaria
                    if (tileNumArray.Length== 2)
                        nave5.active = true;
                    if (tileNumArray.Length == 3) {                       
                        if(cont==4)
                        nave4.active = true;                      
                        if(cont==3)
                        nave3.active = true;
                    }
                    if (tileNumArray.Length == 4)
                        nave2.active = true;
                    if (tileNumArray.Length == 5)
                        nave1.active = true;
                    //fin qua

                    enemyFires.Add(Instantiate(firePrefab, tile.transform.position, Quaternion.identity));
                    tile.GetComponent<TileScript>().SetTileColor(1, new Color32(38, 255, 76, 255));
                    tile.GetComponent<TileScript>().SwitchColors(1);

                   
                    tile1.GetComponent<TileScript>().SetTileColor(1, new Color32(38, 255, 76, 255));
                    tile1.GetComponent<TileScript>().SwitchColors(1);
                    SetDamageClip();
                }
                else
                {
                    topText.text = "COLPITA!!";
                    tile.GetComponent<TileScript>().SetTileColor(1, new Color32(38, 255, 76, 255));
                    tile.GetComponent<TileScript>().SwitchColors(1);
                    tile1.GetComponent<TileScript>().SetTileColor(1, new Color32(38, 255, 76, 255));
                    tile1.GetComponent<TileScript>().SwitchColors(1);
                    SetDamageClip();
                }
                break;
            }
        }
        if(hitCount == 0)
        {
            tile.GetComponent<TileScript>().SetTileColor(1, new Color32(38, 57, 76, 255));
            tile.GetComponent<TileScript>().SwitchColors(1);
            topText.text = "Mancata";
            tile1.GetComponent<TileScript>().SetTileColor(1, new Color32(38, 57, 76, 255));
            tile1.GetComponent<TileScript>().SwitchColors(1);
            SetMissedClip();
        }
        Invoke("EndPlayerTurn", 1.0f);
    }
    public void EnemyHitPlayer(Vector3 tile, int tileNum, GameObject hitObj)
    {
        enemyScript.MissileHit(tileNum);
        tile.y += 0.2f;
        playerFires.Add(Instantiate(firePrefab, tile, Quaternion.identity));
        SetDamageClip();
        if (hitObj.GetComponent<ShipScript>().HitCheckSank())
        {
            playerShipCount--;
            playerShipText.text = playerShipCount.ToString();
            enemyScript.SunkPlayer();
        }
       Invoke("EndEnemyTurn", 2.0f);
    }

    private void EndPlayerTurn()
    {
        for (int i = 0; i < ships.Length; i++) ships[i].SetActive(true);
        foreach (GameObject fire in playerFires) fire.SetActive(true);
        foreach (GameObject fire in enemyFires) fire.SetActive(false);
        enemyShipText.text = enemyShipCount.ToString();
        topText.text = "Turno nemico";
        enemyScript.NPCTurn();
        ColorAllTiles(0);
        if (playerShipCount < 1) GameOver("Il nemico vince!!!");
    }
    public void EndEnemyTurn()
    {
        for (int i = 0; i < ships.Length; i++) ships[i].SetActive(false);
        foreach (GameObject fire in playerFires) fire.SetActive(false);
        foreach (GameObject fire in enemyFires) fire.SetActive(true);
        playerShipText.text = playerShipCount.ToString();
        topText.text = "Seleziona una cella";
        playerTurn = true;
        ColorAllTiles(1);
        if (enemyShipCount < 1) GameOver("HAI VINTO!!");
    }

    private void ColorAllTiles(int colorIndex)
    {
        foreach (TileScript tileScript in allTileScripts)
        {
            tileScript.SwitchColors(colorIndex);
        }
    }

    void GameOver(string winner)
    {
        topText.text = "Fine partita: " + winner;
        replayBtn.gameObject.SetActive(true);
        exitBtn.gameObject.SetActive(true);
        playerTurn = false;

        Time.timeScale = 0;
    }

    void ReplayClicked()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetDefaultClip()
    {
        source.Play();
    }

    public void SetDamageClip()
    {
        sourceDamage.Play();
    }


    public void SetMissedClip()
    {
        sourceMissed.Play();
       
    }

}