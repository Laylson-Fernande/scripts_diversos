using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    SettingData settingsData;
    public ProgressData progressData = new ProgressData();
    SaveAndLoadData saveAndLoad = new SaveAndLoadData();
    public GameObject player;
    public GameObject camPlayer;
    public GameObject mentor;
    public GameObject targetCamPlayer;
    public GameObject canvasUI;
    public GameObject canvasPause;
    Dictionary<string, Player.PositionCam> _DicPosiCam = new Dictionary<string, Player.PositionCam>();
    public List<int> enemysDead = new List<int>();
    
    
    #region JOYSTICK
    [Header("Joystick")]
    [SerializeField]
    GameObject joystick_GameObject;
    [SerializeField]
    RectTransform joystick_Rect;
    [SerializeField]
    Image[] joystick_Img;
    #endregion
    #region D-PAD
    [Header("D-Pad")]
    [SerializeField]
    GameObject dPad_GameObject;
    [SerializeField]
    RectTransform dPad_Rect;
    [SerializeField]
    Image[] dPad_Img;
    #endregion
    #region JUMP
    [Header("Jump")]
    [SerializeField]
    RectTransform jump_Rect;
    [SerializeField]
    Image jump_Img;
    #endregion
    #region SHIELD
    [Header("Shield")]
    [SerializeField]
    RectTransform shield_Rect;
    [SerializeField]
    Image shield_Img;
    #endregion
    #region SWORD
    [Header("Sword")]
    [SerializeField]
    RectTransform sword_Rect;
    [SerializeField]
    Image sword_Img;
    #endregion
    #region ACTION
    [Header("Action")]
    [SerializeField]
    RectTransform action_Rect;
    [SerializeField]
    Image action_Img;
    #endregion
    #region SPIN_R
    [Header("Spin R")]
    [SerializeField]
    RectTransform spinR_Rect;
    [SerializeField]
    Image spingR_Img;
    #endregion
    #region SPIN_L
    [Header("Spin L")]
    [SerializeField]
    RectTransform spinL_Rect;
    [SerializeField]
    Image spinL_Img;
    #endregion

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateSavedDataSettings();
        progressData = saveAndLoad.LoadProgress("ProgressSave");

        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(player);
        DontDestroyOnLoad(mentor);
        DontDestroyOnLoad(targetCamPlayer);
        DontDestroyOnLoad(canvasUI);
        DontDestroyOnLoad(canvasPause);
        _DicPosiCam.Add(Player.PositionCam.back.ToString(), Player.PositionCam.back);
        _DicPosiCam.Add(Player.PositionCam.front.ToString(), Player.PositionCam.front);
        _DicPosiCam.Add(Player.PositionCam.left.ToString(), Player.PositionCam.left);
        _DicPosiCam.Add(Player.PositionCam.right.ToString(), Player.PositionCam.right);

        if (progressData.enemysDead == null)
            progressData.enemysDead = new int[0];

        Debug.Log(progressData.enemysDead);
        Debug.Log(progressData.lesteSavePositon);
        if (progressData.enemysDead.Length != 0)
            for (int i = 0; i < progressData.enemysDead.Length; i++)
            {
                enemysDead.Add(progressData.enemysDead[i]);
            }
    }

    public void UpdatePlayerPosi() {
        player.transform.position = progressData.lesteSavePositon;
        mentor.transform.position = progressData.lesteSavePositon;
        targetCamPlayer.transform.position = progressData.lesteSavePositon;
        player.GetComponent<Player>().posiCam = _DicPosiCam[progressData.positionCam];
    }
    public void UpdateProgressSave()
    {
        progressData.positionCam = player.GetComponent<Player>().posiCam.ToString();
        progressData.enemysDead = enemysDead.ToArray();
        saveAndLoad.SaveProgress(progressData,"ProgressSave");
    }

    public void UpdateSavedDataSettings() {
        settingsData = new SettingData();
        settingsData = saveAndLoad.Load("CustonSettings").setting;
        UpdateCanvasButtons();
    }

    public void UpdateCanvasButtons() {
        if (settingsData._TypeControls == 0)
        {
            joystick_GameObject.SetActive(true);
            dPad_GameObject.SetActive(false);
            player.GetComponent<ControlsPlayer>().mobileJoystick = true;
            
        }
        else {
            joystick_GameObject.SetActive(false);
            dPad_GameObject.SetActive(true);
            player.GetComponent<ControlsPlayer>().mobileJoystick = false;
        }
        joystick_GameObject.GetComponent<Joystick>().fixedJoystick = settingsData.fixedJoystick;  
        //#region Joystick
        //joystick_Rect.position = settingsData._PosiJoystick;
        //joystick_Rect.localScale = settingsData._ScaleJoystick;
        //for (int i = 0; i < joystick_Img.Length; i++)
        //{
        //    joystick_Img[i].color = new Color(1, 1, 1, settingsData._AlphaJoystick);
        //}
        //#endregion
        //#region Dpad
        //dPad_Rect.position = settingsData._PosiDpad;
        //dPad_Rect.localScale = settingsData._ScaleDpad;
        //for (int i = 0; i < dPad_Img.Length; i++)
        //{
        //    dPad_Img[i].color = new Color(255, 255, 255, settingsData._AlphaDpad);
        //}
        //#endregion
        //#region Action
        //action_Rect.position = settingsData._PosiBtAction;
        //action_Rect.localScale = settingsData._ScaleBtAction;
        //action_Img.color = new Color(255, 255, 255, settingsData._AlphaBtAction);
        //#endregion
        //#region Jump
        //jump_Rect.position = settingsData._PosiBtJump;
        //jump_Rect.localScale = settingsData._ScaleBtJump;
        //jump_Img.color = new Color(255, 255, 255, settingsData._AlphaBtJump);
        //#endregion
        //#region Shield
        //shield_Rect.position = settingsData._PosiBtShield;
        //shield_Rect.localScale = settingsData._ScaleBtShield;
        //shield_Img.color = new Color(255, 255, 255, settingsData._AlphaBtShield);
        //#endregion
        //#region Sword
        //sword_Rect.position = settingsData._PosiBtSword;
        //sword_Rect.localScale = settingsData._ScaleBtSword;
        //sword_Img.color = new Color(255, 255, 255, settingsData._AlphaBtSword);
        //#endregion
        //#region SpinR
        //spinR_Rect.position = settingsData._PosiBtSpinR;
        //spinR_Rect.localScale = settingsData._ScaleBtSpinR;
        //spingR_Img.color = new Color(255, 255, 255, settingsData._AlphaBtSpinR);
        //#endregion
        //#region SpinL
        //spinL_Rect.position = settingsData._PosiBtSpinL;
        //spinL_Rect.GetComponent<RectTransform>().localScale = settingsData._ScaleBtSpinL;
        //spinL_Img.GetComponent<Image>().color = new Color(255, 255, 255, settingsData._AlphaBtSpinL);
        //#endregion
    }

    public void AddEnemyDead(string id) {
        int count = enemysDead.Count;
        count++;
    }
}
