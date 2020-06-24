using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.ObjectModel;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; } = null;

    [SerializeField]
    private Text enemyCounterText;

    // game
   [SerializeField]
    private GameObject gameDialogWindow; 

    //[SerializeField]
    //private GameObject player;

    [SerializeField]
    private GameObject gameDialogText;

    private GameObject gTextDialogue;

    private TextMesh gTextMesh;

    private GameObject gWindowDialogue;

    private List<DialogueMessage> messages = new List<DialogueMessage>();

    private int msgIndex = 0;

    // player
    [SerializeField]
    private GameObject playerDialogWindow; 

    [SerializeField]
    private GameObject playerDialogText;

    private GameObject pTextDialogue;

    private TextMesh pTextMesh;

    private GameObject pWindowDialogue;

    // boss
    [SerializeField]
    private GameObject bossDialogWindow; 

    [SerializeField]
    private GameObject bossDialogText;

    private GameObject bTextDialogue;

    private TextMesh bTextMesh;

    private GameObject bWindowDialogue;

    // final boss
    [SerializeField]
    private GameObject finalBossDialogWindow; 

    [SerializeField]
    private GameObject finalBossDialogText;

    private GameObject fTextDialogue;

    private TextMesh fTextMesh;

    private GameObject fWindowDialogue;

    // pause menu
    [SerializeField]
    private GameObject pausePanel;

    // game over
    [SerializeField]
    private GameObject gameOverPanel;

    // game win
    [SerializeField]
    private GameObject gameWinPanel;


    private void Start() {
        setupDialogueMsgs();
    }

    public void resetMessages()
    {
        msgIndex = 0;
        messages = new List<DialogueMessage>();
        setupDialogueMsgs();
    }

    private void setupDialogueMsgs() {
        // Explicação do contexto
        messages.Add(new DialogueMessage("Game", "Kira, you are the best samurai alive\nWe all know that you search peace\nAlso we do\nBut we all need this...", true, false, false));
        messages.Add(new DialogueMessage("Game", "You are the last hope of this city\nYou have to revenge your master\nHe gave everything to you when \nYou were lost...", false, false,false));
        messages.Add(new DialogueMessage("Game", "You have to protect everyone\nYour wife and your children\nAll the community, just...do it!", false, true, true));

        // Player a falar que vai destruir tudo
        messages.Add(new DialogueMessage("Player", "I see there is no other choise\nI must to do this...", true, false, false));
        messages.Add(new DialogueMessage("Player", "There seems this floor is empty\nBut this can't be possible!\nThere must be some one defending \nthis floor", false, false, false));
        messages.Add(new DialogueMessage("Player", "Oh no, i was right!!! I see these\n samurais\nbut if they are here, their master should\n also be...", false, true, true));

        // Boss a falar que vai destruir o player
        messages.Add(new DialogueMessage("Boss", "Oh Kira, I remember you\nWe fight each other back\n in the days\nBut tonight you will be defeated!", true, false, false));
        messages.Add(new DialogueMessage("Boss", "The Yakuza are gaining more \nand more power here, city by city\nCountry by country, we will conquer\nthe world!", false, false, false));
        messages.Add(new DialogueMessage("Boss", "Knee behind me or die\nBacause i am the...\nYahiko Saori!!!", false, true, true));

        // Player depois de derrotar o 1º boss, e que vai ter que lutar mais
        messages.Add(new DialogueMessage("Player", "This was not easy, I am wounded!", true, false, false));
        messages.Add(new DialogueMessage("Player", "How many of them are here?\nI dont know if I am able to complete\nthis mission!", false, false, false));
        messages.Add(new DialogueMessage("Player", "I think I will have to fight\to the death!\nIt is me or them!", false, true, true));

        // Player depois de derrotar todos no 2º level - a dizer que vai tomar o escudo que apareceu
        messages.Add(new DialogueMessage("Player", "Im tired, I need to rest!", true, false, false));
        messages.Add(new DialogueMessage("Player", "Wait, what it is there?What is that shield?", false, false, false));
        messages.Add(new DialogueMessage("Player", "Oh, I see, the secret shield, it will \nhelp me gain some energy!", false, true, true));

        // Player depois de entrar  no 3º level 
        messages.Add(new DialogueMessage("Player", "So this is the last floor\nSo what if the Yamato isn't here?\nIs this all worthless?", true, false, false));
        messages.Add(new DialogueMessage("Player", "No! I will make my oath \npure truth words\nAnd search for him to\nthe end of my days!", false, false, false));
        messages.Add(new DialogueMessage("Player", "I will fulfill the \nsamuri's path and avenge my master's\n death, I can do it!", false, true, true));

        // Boss a apresentar-se e a dizer que vai destruir o player
        messages.Add(new DialogueMessage("FinalBoss", "EHEHEHE Hello, Kira\nI noticed you are searching\nme, the blood trail you are\nleaving behind..", true, false, false));
        messages.Add(new DialogueMessage("FinalBoss", "You know now it is or me or you\nUntil the last breathe", false, false, false));
        messages.Add(new DialogueMessage("FinalBoss", "Im just joking\n AHAHAHA", false, true, false));

        // Boss a falar      
        messages.Add(new DialogueMessage("FinalBoss", "Never have they teached\nyou that you should never\nmess wth the big fish?", true, false, false));
        messages.Add(new DialogueMessage("FinalBoss", "The path you have chosen\nIs a wrong path...", false, false, false));
        messages.Add(new DialogueMessage("FinalBoss", "Maybe you could start\nworking for me\nAnd forget what happend here?", false, true, false));

        // Player a responder ao boss
        messages.Add(new DialogueMessage("Player", "No! Never!", true, false, false));
        messages.Add(new DialogueMessage("Player", "I will revenge my men\nThe men you took life away\nThe honest men that saved me..", false, false, false));
        messages.Add(new DialogueMessage("Player", "From all the emptiness the\nsamurai's life offers", false, true, true));

    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateEnemyCounter(int enemyCounter)
    {
        enemyCounterText.text = enemyCounter.ToString();
    }

    
    public void HandleNextMessage() {

        bool allowContinue = true;

        if(msgIndex > 0) {

         DialogueMessage previousDm= messages[msgIndex-1];

        if(!previousDm.WasClosed && previousDm.LastMsg) {

           allowContinue = false;
           messages[msgIndex-1].WasClosed = true;

        if(previousDm.MsgType == "Game") {
            Destroy(gWindowDialogue);
            Destroy(gTextDialogue);
            if(previousDm.AllowMovingAfter) {
                GameManager.Instance.allowMoving = true;
            }
        }

        if(previousDm.MsgType == "Player") {
            Destroy(pWindowDialogue);
            Destroy(pTextDialogue);
            if(previousDm.AllowMovingAfter) {
                GameManager.Instance.allowMoving = true;
            }
        }

        if(previousDm.MsgType == "Boss") {
            Destroy(bWindowDialogue);
            Destroy(bTextDialogue);
            if(previousDm.AllowMovingAfter) {
                GameManager.Instance.allowMoving = true;
            }
        }

        if(previousDm.MsgType == "FinalBoss") {
            Destroy(fWindowDialogue);
            Destroy(fTextDialogue);
            if(previousDm.AllowMovingAfter) {
                GameManager.Instance.allowMoving = true;
            }
         }
        }
       }

        if(allowContinue && msgIndex >= 0 && (msgIndex+1) <= messages.Count) {

        DialogueMessage dm = messages[msgIndex];

        string message = dm.Msg;

        if(dm.MsgType == "Game") {

            if(dm.FirstMsg) {

               gWindowDialogue = Instantiate(gameDialogWindow);   

               gTextDialogue = Instantiate(gameDialogText);

               Renderer r = gTextDialogue.GetComponent<Renderer>();

               r.sortingLayerName = "PlayerLayer";

               gTextMesh = gTextDialogue.GetComponent<TextMesh>();

               gTextMesh.text = dm.Msg;

            } else {
               gTextMesh.text = dm.Msg;
            }
        }

        if(dm.MsgType == "Player") {

            if(dm.FirstMsg) {

            print("1ª mensagem do player");

               pWindowDialogue = Instantiate(playerDialogWindow);   

               pTextDialogue = Instantiate(playerDialogText);

               Renderer r = pTextDialogue.GetComponent<Renderer>();

               r.sortingLayerName = "PlayerLayer";

               pTextMesh = pTextDialogue.GetComponent<TextMesh>();

               pTextMesh.text = dm.Msg;

            } else {
               pTextMesh.text = dm.Msg;
            }       
          }

        if(dm.MsgType == "Boss") {

               if(dm.FirstMsg) {

               bWindowDialogue = Instantiate(bossDialogWindow);   

               bTextDialogue = Instantiate(bossDialogText);

               Renderer r = bTextDialogue.GetComponent<Renderer>();

               r.sortingLayerName = "PlayerLayer";

               bTextMesh = bTextDialogue.GetComponent<TextMesh>();

               bTextMesh.text = dm.Msg;

            } else {
               bTextMesh.text = dm.Msg;
            }   
        }

        if(dm.MsgType == "FinalBoss") {

               if(dm.FirstMsg) {

               fWindowDialogue = Instantiate(finalBossDialogWindow);   

               fTextDialogue = Instantiate(finalBossDialogText);

               Renderer r = fTextDialogue.GetComponent<Renderer>();

               r.sortingLayerName = "PlayerLayer";

               fTextMesh = fTextDialogue.GetComponent<TextMesh>();

               fTextMesh.text = dm.Msg;

            } else {
               fTextMesh.text = dm.Msg;
            }           
          }

             msgIndex++;
        }
      
    }

    public void ShowPausePanel(bool value)
    {
        pausePanel.SetActive(value);
    }

    public void ShowGameOver(bool value)
    {
        gameOverPanel.SetActive(value);
    }

    public void ShowGameWin(bool value)
    {
        gameWinPanel.SetActive(value);
    }


}

public class DialogueMessage {
    public string MsgType {get; set;}

    public string Msg {get; set;}

    public bool FirstMsg {get; set;}

    public bool LastMsg {get; set;}

    public bool WasClosed {get; set;}

    public bool AllowMovingAfter {get; set;}

    public DialogueMessage(string mt, string m, bool f, bool l, bool am, bool wc = false) {
        MsgType = mt;
        Msg = m;
        FirstMsg = f;
        LastMsg = l;
        WasClosed = wc;
        AllowMovingAfter = am;
    }
}
