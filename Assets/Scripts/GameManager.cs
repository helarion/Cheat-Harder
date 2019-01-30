using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {

    // instance du gamemanager en singleton
    public static GameManager instance;

    // constantes de gameplay
    public int numberRoom;
    public float camXTP;
    float initXCam; // valeur x initiale du transform de la camera

    // variables de gameplay
    public int currentRoom;
    public int bonusFound;
    bool introRewindDone;
    public bool isRewinding;
    bool endRewindDone;
    bool rewindLoopDone;
    public bool isDead;
    public float time;
    float savedTime;

    // Objets à assigner
    public Camera mainCam;
    public GameObject filter;
    public PlayerController player;
    public AudioClip loop;
    public AudioSource musicSource;
    public AudioSource rewindSource;
    public AudioClip introRewind;
    public AudioClip loopRewind;
    public AudioClip outroRewind;
    public TextMeshProUGUI currentBonusText;
    public TextMeshProUGUI currentTimeText;
    public TextMeshProUGUI scoreTimeText;
    public TextMeshProUGUI scoreBonusText;
    public Canvas deathCanvas;
    public Canvas menuCanvas;
    public Canvas endCanvas;

    // Objets à récupérer
    TimeBody playerTB;
    public List<GameObject> lastBonus;
    public CameraShake shakeScreen;

    void Awake()
    {
        //on check si l'instance existe déja
        if (instance == null)
        {
            // si non : on l'attribue à celle en cours
            instance = this;
        }
        else if (instance != this)
        {
            // s'il en existe déja une, on détruit cet objet car on en a besoin que d'un seul
            Destroy(gameObject);
        }

        // ne pas détruire lorsqu'on change de scène
       // DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        //initalisation des variables

        // temps
            time = 0;
            savedTime = 0;

        // rewind
            rewindLoopDone = false;
            isRewinding = false;
            endRewindDone = true;
            introRewindDone = false;

        // musique
            musicSource.volume = 0.4f;
            musicSource.Play();

        // Jeu
            currentRoom = 1;
            //Physics.gravity = new Vector3(0, -15, 0);

        // mort
        isDead = false;
        // bonus
        bonusFound = 0;

        // récupération des variables
        initXCam = mainCam.transform.position.x; 
        shakeScreen = GetComponent<CameraShake>();
        playerTB = player.GetComponent<TimeBody>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!isDead) // Si le joueur est en vie
        {
            // Debug : teleportation  dans une salle donnée
            if (Input.GetKeyDown(KeyCode.F1))
            {
                MoveCamera(1);
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                MoveCamera(2);
            }
            else if (Input.GetKeyDown(KeyCode.F3))
            {
                MoveCamera(3);
            }
            else if (Input.GetKeyDown(KeyCode.F4))
            {
                MoveCamera(4);
            }
            else if (Input.GetKeyDown(KeyCode.F5))
            {
                MoveCamera(5);
            }
            else if (Input.GetKeyDown(KeyCode.F6))
            {
                MoveCamera(6);
            }
            else if (Input.GetKeyDown(KeyCode.F7))
            {
                MoveCamera(7);
            }

            // Si on rewind
            if (isRewinding)
            {
                time -= Time.deltaTime; // on retire le temps enregistré
                if (time < savedTime) time = savedTime; // on bloque au dernier temps sauvegardé
                RewindingSound(); // on joue le son du rewind
            }
            else // sinon
            {
                time += Time.deltaTime; // on ajoute le temps en temps réel
                StopRewindingSound(); // on cesse le son du rewind
            }
        }
        else // s'il est mort
        {
            if (Input.GetAxis("Revive") != 0) // Si on utilise l'input "Revive"
            {
                Revive(); // On lance la fonction revive
            }
        }

        if (!musicSource.isPlaying) // Si la musique s'arrête : que l'intro est terminée donc
        {
            musicSource.clip = loop; // on change l'intro par la boucle de musique
            musicSource.Play(); // on fait jouer la boucle
            musicSource.loop = true; // on modifie la source pour qu'elle tourne en boucle
        }

        // Affichage des points en temps réel
        currentBonusText.text = bonusFound.ToString(); //on affiche le nombre de bonus récupérés
        currentTimeText.text = System.Math.Round(time, 3).ToString(); //on affiche le temps

        if (Input.GetAxisRaw("Menu") != 0) // si on active l'input "menu"
        {
            SetMenu(true); // on ouvre le menu echap
        }
    }

    // Changement de camera
    public void MoveCamera(int newRoom) 
    {
        savedTime = time; // on assigne le temps actuel comme dernier temps sauvegardé
        // on supprime le dernier bonus de la variable pour indiquer qu'il est maintenant 
        // validé et qu'il ne pourra pas être supprimé si on meurt
        player.savedHP = player.hp;
        lastBonus.Clear();
        // on détermine la nouvelle position x de la camera avec la translation nécéssaire
        // et le nombre de fois qu'il faut la réaliser depuis le X initial suivant la salle correspondante
        float newX = initXCam + (newRoom - 1) * camXTP; 

        // On bouge la camera en changeant l'origine sur le script Shakescreen qui controle sa position
        shakeScreen.setNewOrigin(new Vector3(newX, mainCam.transform.position.y, mainCam.transform.position.z));

        currentRoom = newRoom; // on définit la salle actuelle comme étant la nouvelle atteinte

        TpToSpawn(); // on téléporte le joueur au spawn de la nouvelle salle

        playerTB.ClearRecord(); // on supprime les enregistrements de position pour le rewind du joueur
    }

    // Téléportation au spawn de la salle actuelle
    public void TpToSpawn()
    {
        player.transform.position = GameObject.FindWithTag("spawn" + currentRoom).transform.position;
        player.rb.velocity = Vector3.zero;
        player.rb.angularVelocity = Vector3.zero;
    }

    /*
    public void tpToEnd()
    {
        player.transform.position = GameObject.FindWithTag("end" + currentRoom).transform.position+new Vector3(-0.3f,0);
        player.rb.velocity = Vector3.zero;
        player.rb.angularVelocity = Vector3.zero;
    }
    */

    // Récupération d'un bonus
    public void GetBonus(GameObject bonus)
    {
        lastBonus.Add(bonus);
        bonusFound++;
        bonus.GetComponent<AudioSource>().Play();
        EnableBonus(false);
    }

    // Perte du dernier bonus s'il est enregistré
    public void LoseBonus()
    {
        if(lastBonus.Count>0)
        {
            bonusFound-= lastBonus.Count;
            EnableBonus(true);
            lastBonus.Clear();
        }
    }

    // Activation ou désactivation de l'affichage et du collider d'un bonus
    public void EnableBonus(bool boolean)
    {
        for(int i=0;i<lastBonus.Count;i++)
        {
            lastBonus[i].GetComponent<SphereCollider>().enabled = boolean;
            lastBonus[i].GetComponent<MeshRenderer>().enabled = boolean;
            lastBonus[i].GetComponentInChildren<Light>().enabled = boolean;
        }
        
    }

    // Gestion du son de rewind
    public void RewindingSound()
    {
        if (!introRewindDone) // si on a pas lançée l'intro du son
        {
            rewindSource.clip = introRewind; // on change le clip pour mettre l'intro
            rewindSource.Play(); // on joue la source
            introRewindDone = true; // on définit l'intro comme lançée
            endRewindDone = false; // on définit la fin du son comme non lançée
        }

        // sinon si l'intro a été lançée mais qu'elle se termine et que la boucle n'est pas lançé
        else if(introRewindDone && !rewindSource.isPlaying && !rewindLoopDone)
        {
            rewindSource.clip = loopRewind; // on change le clip opur mettre la boucle
            rewindSource.Play(); // on joue la source
            rewindSource.loop = true; // on transforme la source en boucle
            rewindLoopDone = true; // on définit la boucle comme lançée
        }
    }

    // Gestion de fin du son de rewind
    public void StopRewindingSound()
    {
        if (!endRewindDone) // si la fin du rewind n'est pas faite
        {
            rewindSource.Stop(); // on arrête la source
            rewindSource.clip = outroRewind; // on change le clip pour mettre la fin du rewind
            rewindSource.Play(); // on joue la source
            rewindSource.loop = false; // on arrête la boucle de la source
            endRewindDone = true; // on définit la fin du rewind comme faite
            // on remet à zero l'intro et la boucle pour le prochain rewind
            introRewindDone = false; 
            rewindLoopDone = false;
        }
    }

    // mort du joueur
    public void Dead()
    {
        player.body.SetActive(false); // on désactive l'affichage du corps
        deathCanvas.gameObject.SetActive(true); // on affiche le message de mort
        isDead = true; // on définit le joueur comme mort
    }

    // resurection du joueur
    public void Revive()
    {
        // on cherche tous les lasers à l'écran
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Laser");
        // on les supprime
        for (int i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }

        deathCanvas.gameObject.SetActive(false); // on enlève le message de mort
        isDead = false; // on définti le joueur comme viavnt
        player.body.SetActive(true); // on affiche le corps du joueur
        TpToSpawn(); // on le téléporte au spawn de la salle
        LoseBonus(); // on lui fait perdre son dernier bonus enregistré
        player.ResetHP(player.savedHP); // on lui remet sa vie à sa dernière sauvegarde
        time = savedTime; // on remet le temps au dernier temps sauvegardé
    }

    // affichage du menu echap
    public void SetMenu(bool isEnabled)
    {
        isDead = isEnabled;
        menuCanvas.gameObject.SetActive(isEnabled);
        player.enabled = !isEnabled;
    }

    // Fin du niveau
    public void EndLevel()
    {
        endCanvas.gameObject.SetActive(true);
        isDead = true; // désactive les mécaniques du jeu

        scoreBonusText.text = bonusFound.ToString();
        scoreTimeText.text = System.Math.Round(time, 3).ToString();
    }

    public void SaveLastRecord(string playerName)
    {
        for (int i = 1; i <= 10; i++)
        {
            Debug.Log("Saving");
            if (time < PlayerPrefs.GetFloat("Time" + i) || PlayerPrefs.GetFloat("Time" + i) == 0)
            {
                Debug.Log("Boucle: " + i + " Time: " + time + " Name: " + playerName);
                PlayerPrefs.SetFloat("Time" + i, time);
                PlayerPrefs.SetString("Nick" + i, playerName);
                PlayerPrefs.SetInt("Bonus" + i, bonusFound);
                break;
            }
        }

    }
}
