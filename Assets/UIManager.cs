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
        messages.Add(new DialogueMessage("Game", "Mensagem generica para o jogo", true, false, false));
        messages.Add(new DialogueMessage("Game", "Mensagem generica 2 para o jogo", false, false,false));
        messages.Add(new DialogueMessage("Game", "Mensagem generica 3 para o jogo", false, true, true));

        // Player a falar que vai destruir tudo
        messages.Add(new DialogueMessage("Player", "Mensagem generica para o player", true, false, false));
        messages.Add(new DialogueMessage("Player", "Mensagem generica 2 para o player", false, false, false));
        messages.Add(new DialogueMessage("Player", "Mensagem generica 2 para o player", false, true, true));

        // Boss a falar que vai destruir o player
        messages.Add(new DialogueMessage("Boss", "Mensagem generica para o boss", true, false, false));
        messages.Add(new DialogueMessage("Boss", "Mensagem generica2 para o boss", false, false, false));
        messages.Add(new DialogueMessage("Boss", "Mensagem generica2 para o boss", false, true, true));

        // Player depois de derrotar o 1º boss, e que vai ter que lutar mais
        messages.Add(new DialogueMessage("Player", "Mensagem generica para o player", true, false, false));
        messages.Add(new DialogueMessage("Player", "Mensagem generica 2 para o player", false, false, false));
        messages.Add(new DialogueMessage("Player", "Mensagem generica 3 para o player", false, true, true));

        // Player depois de derrotar todos no 2º level - a dizer que vai tomar o escudo que apareceu
        messages.Add(new DialogueMessage("Player", "Mensagem generica para o player", true, false, false));
        messages.Add(new DialogueMessage("Player", "Mensagem generica 2 para o player", false, false, false));
        messages.Add(new DialogueMessage("Player", "Mensagem generica 3 para o player", false, true, true));

        // Player depois de entrar  no 3º level 
        messages.Add(new DialogueMessage("Player", "Mensagem generica para o player", true, false, false));
        messages.Add(new DialogueMessage("Player", "Mensagem generica 2 para o player", false, false, false));
        messages.Add(new DialogueMessage("Player", "Mensagem generica 3 para o player", false, true, true));

        // Boss a apresentar-se e a dizer que vai destruir o player
        messages.Add(new DialogueMessage("FinalBoss", "Mensagem generica para o final boss", true, false, false));
        messages.Add(new DialogueMessage("FinalBoss", "Mensagem generica 2 para o final boss", false, false, false));
        messages.Add(new DialogueMessage("FinalBoss", "Mensagem generica 3 para o final boss", false, true, false));

        // Boss a falar      
        messages.Add(new DialogueMessage("FinalBoss", "Mensagem generica para o final boss", true, false, false));
        messages.Add(new DialogueMessage("FinalBoss", "Mensagem generica 2 para o final boss", false, false, false));
        messages.Add(new DialogueMessage("FinalBoss", "Mensagem generica 3 para o final boss", false, true, false));

        // Player a responder ao boss
        messages.Add(new DialogueMessage("Player", "Mensagem generica para o player", true, false, false));
        messages.Add(new DialogueMessage("Player", "Mensagem generica 2 para o player", false, false, false));
        messages.Add(new DialogueMessage("Player", "Mensagem generica 2 para o player", false, true, true));

        // boss a morrer
        messages.Add(new DialogueMessage("FinalBoss", "Mensagem generica para o final boss a morrer", true, false, false));
        messages.Add(new DialogueMessage("FinalBoss", "Mensagem generica 2 para o final boss", false, false, false));
        messages.Add(new DialogueMessage("FinalBoss", "Mensagem generica 3 para o final boss", false, true, false));

        // PLayer a celebrar
        messages.Add(new DialogueMessage("Player", "Mensagem generica para o player", true, false, false));
        messages.Add(new DialogueMessage("Player", "Mensagem generica 2 para o player", false, false, false));
        messages.Add(new DialogueMessage("Player", "Mensagem generica 2 para o player", false, true, true));

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
