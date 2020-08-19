using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsPlayer : MonoBehaviour
{
    Player player;
    enum TypeControls {mobile,joystick,keyboard}
    TypeControls controls = TypeControls.mobile;
    public bool mobileJoystick;
    Dictionary<string, BT> buttons = new Dictionary<string, BT>();
    Dictionary<string, GameObject> goButtons = new Dictionary<string, GameObject>();
    [SerializeField]
    Joystick joystick;
    public bool buttonsDisable;
    [SerializeField]
    List<GameObject> goButtonsDisable = new List<GameObject>();
    [SerializeField]
    List<GameObject> tempButtons = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        //GameObject[] temp = GameObject.FindGameObjectsWithTag("ButtonsControls");
        for (int i=0;i<tempButtons.Count;i++) {
            string tempName = tempButtons[i].name;
            //if (tempName == "AreaJoystick")
            //    //joystick = temp[i].GetComponent<Joystick>();
            //else
            if (!buttons.ContainsKey(tempName)) {
                buttons.Add(tempName, tempButtons[i].GetComponent<BT>());
            }

            if (tempName == "ButtonAction")
            {
                goButtons.Add(tempName, tempButtons[i]);
            } else if (tempName == "ButtonSpinR")
            {
                goButtons.Add(tempName, tempButtons[i]);
            }
            else if (tempName == "ButtonSpinL")
            {
                goButtons.Add(tempName, tempButtons[i]);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(!buttonsDisable)
        switch (controls) {
            case TypeControls.mobile:
                MoveMobile();
                ButtonsMobile();
                break;
        }
    }

    #region Controls Mobile
    void MoveMobile() {
        Vector2 movement = Vector2.zero;
        if (mobileJoystick)
        {
            if (joystick==null) joystick = GameObject.Find("AreaJoystick").GetComponent<Joystick>();
            movement = joystick.direction;
        }
        else
        {
            if (buttons["DpadUp"].pressing) movement = Vector2.up;
            else if (buttons["DpadDown"].pressing) movement = Vector2.down;
            else if (buttons["DpadRight"].pressing) movement = Vector2.right;
            else if (buttons["DpadLeft"].pressing) movement = Vector2.left;
        }

        player.MovePlayer(movement);
        if (movement != Vector2.zero)
        {
            if (player.speed < 1)
                player.speed += 0.01f;
            if (player.speed > 1) player.speed = 1;
        }
        else player.speed = 0.4f;

    }

    void ButtonsMobile() {
        #region Enable And Disable Buttons
        switch (player.nameInterage)
        {
            case "Alavanca":
                goButtons["ButtonAction"].SetActive(true);
                break;
            case "MiniPilar":
                goButtons["ButtonSpinL"].SetActive(true);
                goButtons["ButtonSpinR"].SetActive(true);
                break;
            case "":
                goButtons["ButtonAction"].SetActive(false);
                goButtons["ButtonSpinL"].SetActive(false);
                goButtons["ButtonSpinR"].SetActive(false);
                break;
        }
        #endregion

        #region Jump
        if (buttons["ButtonJump"].pressing) {
            buttons["ButtonJump"].pressing = false;
            player.Jump();
        }
        #endregion
        #region Shield
        if (buttons["ButtonShield"].click && buttons["ButtonShield"].lastUp < 1.5f)
        {
            buttons["ButtonShield"].click = false;
            player.Shield();
        }else buttons["ButtonShield"].click = false;

        if (buttons["ButtonShield"].input >= 1.5f)
        {
            buttons["ButtonShield"].pressing = false;
            player.HoldShield();
        }
        #endregion
        #region Action
        if (buttons["ButtonAction"].pressing)
        {
            buttons["ButtonAction"].pressing = false;
            player.Ativar();
        }
        #endregion
        #region Spin L and R
        if (buttons["ButtonSpinL"].pressing)
        {
            buttons["ButtonSpinL"].pressing = false;
            player.Girar(true);
        }

        if (buttons["ButtonSpinR"].pressing)
        {
            buttons["ButtonSpinR"].pressing = false;
            player.Girar(false);
        }
        #endregion
    }
    #endregion


    public void ShowingScenery(bool b) {
        GameObject.Find("Camera").GetComponent<CameraPlayer>().showingScenery = b;
        joystick.ToZero();
        player.MovePlayer(Vector3.zero);
        if (goButtonsDisable.Count == 0)
        {
            GameObject[] temp = GameObject.FindGameObjectsWithTag("ButtonsControls");
            for (int i = 0; i < temp.Length; i++)
            {
                goButtonsDisable.Add(temp[i]);
                temp[i].SetActive(!b);

                try
                {
                    temp[i].GetComponent<BT>().pressing = false;
                }
                catch (System.Exception)
                {
                }
            }
        }
        else {
            for (int i = 0; i < goButtonsDisable.Count; i++) {
                goButtonsDisable[i].SetActive(!b);
                try
                {
                    goButtonsDisable[i].GetComponent<BT>().pressing = false;
                }
                catch (System.Exception)
                {
                }
                
            }
            goButtonsDisable.Clear();
        }

        
        
    }
}
