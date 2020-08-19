using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SavedData
{
    public bool HasDefault;
    public  SettingData setting = new SettingData();
    
}
[Serializable]
public class ProgressData {
    public bool tutorialComplete = false;
    public bool animHeart1 = false;
    public string posiCam = "";
    public string positionCam;
    public bool wayLesteP1, wayLesteP2, wayLesteP3, wayLesteP4,
        wayLesteP5, wayLestePilar = false;
    public Vector3 lesteSavePositon = new Vector3(0.285f, 0.396f, -3.14f);
    public int[] enemysDead = new int[1] { 1111 };
}
[Serializable]
public class SettingData {
    public bool  fixedJoystick;
    public int _TypeControls;

    public Vector2 _PosiJoystick, _PosiDpad, _PosiBtJump, _PosiBtShield,
_PosiBtSword, _PosiBtAction, _PosiBtSpinR, _PosiBtSpinL;

    public Vector2 _ScaleJoystick, _ScaleDpad, _ScaleBtJump, _ScaleBtShield,
    _ScaleBtSword, _ScaleBtAction, _ScaleBtSpinR, _ScaleBtSpinL;

    public float _AlphaJoystick, _AlphaDpad, _AlphaBtJump, _AlphaBtShield, _AlphaBtSword,
        _AlphaBtAction, _AlphaBtSpinR, _AlphaBtSpinL;
}

[Serializable]
public class ProgressSerializable {
    
    public bool tutorialComplete;
    public bool animHeart1 = false;
    public string posiCam = "";
    public string positionCam;
    public bool wayLesteP1, wayLesteP2, wayLesteP3, wayLesteP4,
    wayLesteP5, wayLestePilar = false;
    public float[] lesteSavePositon = new float[3];
    public int[] enemysDead = new int[1] {1111};
}
[Serializable]
public class SettingDataSerializable {
    public bool fixedJoystick;
    public int _TypeControls;

    public float[] _PosiJoystick, _PosiDpad, _PosiBtJump, _PosiBtShield,
    _PosiBtSword, _PosiBtAction, _PosiBtSpinR, _PosiBtSpinL = new float[2];

    public float[] _ScaleJoystick, _ScaleDpad, _ScaleBtJump, _ScaleBtShield,
    _ScaleBtSword, _ScaleBtAction, _ScaleBtSpinR, _ScaleBtSpinL = new float[2];

    public float _AlphaJoystick, _AlphaDpad, _AlphaBtJump, _AlphaBtShield, _AlphaBtSword,
    _AlphaBtAction, _AlphaBtSpinR, _AlphaBtSpinL;
}
