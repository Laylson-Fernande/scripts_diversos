using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveAndLoadData
{

    public void Save(SavedData saved, string nameData) {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath +"/"+ nameData + ".dat", FileMode.OpenOrCreate);
        SettingDataSerializable savedSerializable = SerializeSaveData(saved);
        bf.Serialize(file,savedSerializable);
        file.Close();
        Debug.Log(nameData+" Salvo");
    }

    public void SaveProgress(ProgressData progress, string nameData)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        try
        {
            file = File.Open(Application.persistentDataPath + "/" + nameData + ".dat", FileMode.Open);
        }
        catch (System.Exception)
        {

            file = File.Open(Application.persistentDataPath + "/" + nameData + ".dat", FileMode.Create);
        }
        ProgressSerializable serializable = new ProgressSerializable();
        serializable.enemysDead = progress.enemysDead;
        serializable.tutorialComplete = progress.tutorialComplete;
        serializable.animHeart1 = progress.animHeart1;
        serializable.posiCam = progress.posiCam;
        serializable.positionCam = progress.positionCam;
        serializable.wayLesteP1 = progress.wayLesteP1;
        serializable.wayLesteP2 = progress.wayLesteP2;
        serializable.wayLesteP3 = progress.wayLesteP3;
        serializable.wayLesteP4 = progress.wayLesteP4;
        serializable.wayLesteP5 = progress.wayLesteP5;
        serializable.wayLestePilar = progress.wayLestePilar;
        Vector3 vector3 = progress.lesteSavePositon;
        serializable.lesteSavePositon[0] = vector3.x;
        serializable.lesteSavePositon[1] = vector3.y;
        serializable.lesteSavePositon[2] = vector3.z;
        bf.Serialize(file, serializable);
        file.Close();
        Debug.Log(nameData + " Salvo");
    }

    public SavedData Load(string nameData) {
        BinaryFormatter bf = new BinaryFormatter();
        try
        {
            FileStream file = File.Open(Application.persistentDataPath + "/" + nameData + ".dat", FileMode.Open);
            SettingDataSerializable savedDeserializable = (SettingDataSerializable)bf.Deserialize(file);
            SavedData saved = DeserializeSaveData(savedDeserializable);
            file.Close();
            Debug.Log(nameData+ " Encontrado");
            return saved;
        }
        catch (System.Exception)
        {
            Debug.Log("Não encontrei Save "+nameData);
            return null;
        }
    }
    public ProgressData LoadProgress(string nameData)
    {
        BinaryFormatter bf = new BinaryFormatter();
        try
        {
            FileStream file = File.Open(Application.persistentDataPath + "/" + nameData + ".dat", FileMode.Open);
            ProgressSerializable progress = new ProgressSerializable();
            progress = (ProgressSerializable)bf.Deserialize(file);
            ProgressData progressData = new ProgressData();
            progressData.enemysDead = progress.enemysDead;
            progressData.tutorialComplete = progress.tutorialComplete;
            progressData.animHeart1 = progress.animHeart1;
            progressData.posiCam = progress.posiCam;
            progressData.positionCam = progress.positionCam;
            progressData.wayLesteP1 = progress.wayLesteP1;
            progressData.wayLesteP2 = progress.wayLesteP2;
            progressData.wayLesteP3 = progress.wayLesteP3;
            progressData.wayLesteP4 = progress.wayLesteP4;
            progressData.wayLesteP5 = progress.wayLesteP5;
            progressData.wayLestePilar = progress.wayLestePilar;
            Vector3 vector = new Vector3(progress.lesteSavePositon[0], progress.lesteSavePositon[1], progress.lesteSavePositon[2]);
            progressData.lesteSavePositon = vector;
            file.Close();
            Debug.Log(nameData + " Encontrado");
            return progressData;
        }
        catch (System.Exception)
        {
            ProgressData progressData = new ProgressData();
            SaveProgress(progressData,nameData);
            return progressData;
        }

    }

    SettingDataSerializable SerializeSaveData(SavedData savedData) {
        SettingDataSerializable serializad = new SettingDataSerializable();
        serializad.fixedJoystick = savedData.setting.fixedJoystick;
        serializad._TypeControls = savedData.setting._TypeControls;
        serializad._PosiJoystick = VectorToArray(savedData.setting._PosiJoystick);
        serializad._PosiDpad = VectorToArray(savedData.setting._PosiDpad);
        serializad._PosiBtJump = VectorToArray(savedData.setting._PosiBtJump);
        serializad._PosiBtShield = VectorToArray(savedData.setting._PosiBtShield);
        serializad._PosiBtSword = VectorToArray(savedData.setting._PosiBtSword);
        serializad._PosiBtAction = VectorToArray(savedData.setting._PosiBtAction);
        serializad._PosiBtSpinR = VectorToArray(savedData.setting._PosiBtSpinR);
        serializad._PosiBtSpinL = VectorToArray(savedData.setting._PosiBtSpinL);

        serializad._ScaleJoystick = VectorToArray(savedData.setting._ScaleJoystick);
        serializad._ScaleDpad = VectorToArray(savedData.setting._ScaleDpad);
        serializad._ScaleBtJump = VectorToArray(savedData.setting._ScaleBtJump);
        serializad._ScaleBtShield = VectorToArray(savedData.setting._ScaleBtShield);
        serializad._ScaleBtSword = VectorToArray(savedData.setting._ScaleBtSword);
        serializad._ScaleBtAction = VectorToArray(savedData.setting._ScaleBtAction);
        serializad._ScaleBtSpinR = VectorToArray(savedData.setting._ScaleBtSpinR);
        serializad._ScaleBtSpinL = VectorToArray(savedData.setting._ScaleBtSpinL);

        serializad._AlphaJoystick = savedData.setting._AlphaJoystick;
        serializad._AlphaDpad = savedData.setting._AlphaDpad;
        serializad._AlphaBtJump = savedData.setting._AlphaBtJump;
        serializad._AlphaBtSword = savedData.setting._AlphaBtSword;
        serializad._AlphaBtAction = savedData.setting._AlphaBtSpinR;
        serializad._AlphaBtSpinL = savedData.setting._AlphaBtSpinL;
        serializad._AlphaBtShield = savedData.setting._AlphaBtShield;
        serializad._AlphaBtSpinR = savedData.setting._AlphaBtSpinR;

        return serializad;
    }
    SavedData DeserializeSaveData(SettingDataSerializable serializable) {
        SavedData deserializad = new SavedData();
        deserializad.setting.fixedJoystick = serializable.fixedJoystick;
        deserializad.setting._TypeControls = serializable._TypeControls;
        deserializad.setting._PosiJoystick = ArrayToVector(serializable._PosiJoystick);
        deserializad.setting._PosiDpad = ArrayToVector(serializable._PosiDpad);
        deserializad.setting._PosiBtJump = ArrayToVector(serializable._PosiBtJump);
        deserializad.setting._PosiBtShield = ArrayToVector(serializable._PosiBtShield);
        deserializad.setting._PosiBtSword = ArrayToVector(serializable._PosiBtSword);
        deserializad.setting._PosiBtAction = ArrayToVector(serializable._PosiBtAction);
        deserializad.setting._PosiBtSpinR = ArrayToVector(serializable._PosiBtSpinR);
        deserializad.setting._PosiBtSpinL = ArrayToVector(serializable._PosiBtSpinL);

        deserializad.setting._ScaleJoystick = ArrayToVector(serializable._ScaleJoystick);
        deserializad.setting._ScaleDpad = ArrayToVector(serializable._ScaleDpad);
        deserializad.setting._ScaleBtJump = ArrayToVector(serializable._ScaleBtJump);
        deserializad.setting._ScaleBtShield = ArrayToVector(serializable._ScaleBtShield);
        deserializad.setting._ScaleBtSword = ArrayToVector(serializable._ScaleBtSword);
        deserializad.setting._ScaleBtAction = ArrayToVector(serializable._ScaleBtAction);
        deserializad.setting._ScaleBtSpinR = ArrayToVector(serializable._ScaleBtSpinR);
        deserializad.setting._ScaleBtSpinL = ArrayToVector(serializable._ScaleBtSpinL);

        deserializad.setting._AlphaJoystick = serializable._AlphaJoystick;
        deserializad.setting._AlphaDpad = serializable._AlphaDpad;
        deserializad.setting._AlphaBtJump = serializable._AlphaBtJump;
        deserializad.setting._AlphaBtSword = serializable._AlphaBtSword;
        deserializad.setting._AlphaBtAction = serializable._AlphaBtSpinR;
        deserializad.setting._AlphaBtSpinL = serializable._AlphaBtSpinL;
        deserializad.setting._AlphaBtShield = serializable._AlphaBtShield;
        deserializad.setting._AlphaBtSpinR = serializable._AlphaBtSpinR;

        return deserializad;
    }

    public void ClearDat(string nameData) {
        //FileStream file = File.Delete(Application.persistentDataPath + "/" + nameData + ".dat");
        File.Delete(Application.persistentDataPath + "/" + nameData + ".dat");
    }


    float[] VectorToArray(Vector2 v2) {
        float[] vetor = new float[2];
        vetor[0] = v2.x;
        vetor[1] = v2.y;
        return vetor;
    }

    Vector2 ArrayToVector(float[] f) {
        Vector2 v2 = new Vector2(f[0],f[1]);
        return v2;
    }
}
