using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using BTAI;
using Gamekit2D;
using UnityEngine;
using UnityEngine.Events;

public class CharacterStatsString
{

    public string Pause;
    public string Interact;
    public string MeleeAttack;
    public string RangedAttack;
    public string Jump;
    public string HorizontalPositive;
    public string HorizontalNegative;
    public string VerticalPositive;
    public string VerticalNegative;

    public float jumpHeight;
    public float inputLatency;

    public CharacterStatsString(CharacterStatsSO stats)
    {
        Pause = TranslateKey(stats.Pause);
        Interact = TranslateKey(stats.Interact);
        MeleeAttack = TranslateKey(stats.MeleeAttack);
        RangedAttack = TranslateKey(stats.RangedAttack);
        Jump = TranslateKey(stats.Jump);
        HorizontalPositive = TranslateKey(stats.HorizontalPositive);
        HorizontalNegative = TranslateKey(stats.HorizontalNegative);
        VerticalPositive = TranslateKey(stats.VerticalPositive);
        VerticalNegative = TranslateKey(stats.VerticalNegative);

        jumpHeight = stats.jumpHeight;
        inputLatency = stats.inputLatency;
    }

    public CharacterStatsString(CharacterStats stats)
    {
        Pause = TranslateKey(stats.Pause);
        Interact = TranslateKey(stats.Interact);
        MeleeAttack = TranslateKey(stats.MeleeAttack);
        RangedAttack = TranslateKey(stats.RangedAttack);
        Jump = TranslateKey(stats.Jump);
        HorizontalPositive = TranslateKey(stats.HorizontalPositive);
        HorizontalNegative = TranslateKey(stats.HorizontalNegative);
        VerticalPositive = TranslateKey(stats.VerticalPositive);
        VerticalNegative = TranslateKey(stats.VerticalNegative);

        jumpHeight = stats.jumpHeight;
        inputLatency = stats.inputLatency;
    }
    
    private string TranslateKey(KeyCode key)
    {

        switch (key)
        {
            case KeyCode.A:
                return "A";
            case KeyCode.B:
                return "B";
            case KeyCode.C:
                return "C";
            case KeyCode.D:
                return "D";
            case KeyCode.E:
                return "E";
            case KeyCode.F:
                return "F";
            case KeyCode.G:
                return "G";
            case KeyCode.H:
                return "H";
            case KeyCode.I:
                return "I";
            case KeyCode.J:
                return "J";
            case KeyCode.K:
                return "K";
            case KeyCode.L:
                return "L";
            case KeyCode.M:
                return "M";
            case KeyCode.N:
                return "N";
            case KeyCode.O:
                return "O";
            case KeyCode.P:
                return "P";
            case KeyCode.Q:
                return "Q";
            case KeyCode.R:
                return "R";
            case KeyCode.S:
                return "S";
            case KeyCode.T:
                return "T";
            case KeyCode.U:
                return "U";
            case KeyCode.V:
                return "V";
            case KeyCode.W:
                return "W";
            case KeyCode.X:
                return "X";
            case KeyCode.Y:
                return "Y";
            case KeyCode.Z:
                return "Z";
            case KeyCode.Space:
                return "SPACE";
            case KeyCode.Escape:
                return "ESCAPE";
            default:
                return "";
        }
    }
}

public class CharacterStats
{
    public KeyCode Pause;
    public KeyCode Interact;
    public KeyCode MeleeAttack;
    public KeyCode RangedAttack;
    public KeyCode Jump;
    public KeyCode HorizontalPositive;
    public KeyCode HorizontalNegative;
    public KeyCode VerticalPositive;
    public KeyCode VerticalNegative;

    public float jumpHeight;
    public float inputLatency;

    public CharacterStats(string _Pause, string _Interact, string _MeleeAttack, string _RangedAttack, string _Jump, string _HorizontalPositive, string _HorizontalNegative, string _VerticalPositive, string _VerticalNegative, float _jumpHeight, float _inputLatency)
    {
        Pause = InterpretKey(_Pause);
        Interact = InterpretKey(_Interact);
        MeleeAttack = InterpretKey(_MeleeAttack);
        RangedAttack = InterpretKey(_RangedAttack);
        Jump = InterpretKey(_Jump);
        HorizontalPositive = InterpretKey(_HorizontalPositive);
        HorizontalNegative = InterpretKey(_HorizontalNegative);
        VerticalPositive = InterpretKey(_VerticalPositive);
        VerticalNegative = InterpretKey(_VerticalNegative);

        jumpHeight = _jumpHeight;
        inputLatency = _inputLatency;
    }
    public CharacterStats(CharacterStatsString statsString)
    {
        Pause = InterpretKey(statsString.Pause);
        Interact = InterpretKey(statsString.Interact);
        MeleeAttack = InterpretKey(statsString.MeleeAttack);
        RangedAttack = InterpretKey(statsString.RangedAttack);
        Jump = InterpretKey(statsString.Jump);
        HorizontalPositive = InterpretKey(statsString.HorizontalPositive);
        HorizontalNegative = InterpretKey(statsString.HorizontalNegative);
        VerticalPositive = InterpretKey(statsString.VerticalPositive);
        VerticalNegative = InterpretKey(statsString.VerticalNegative);

        jumpHeight = statsString.jumpHeight;
        inputLatency = statsString.inputLatency;
    }
    private KeyCode InterpretKey(string key)
    {

        switch (key.ToUpper())
        {
            case "A":
                return KeyCode.A;
            case "B":
                return KeyCode.B;
            case "C":
                return KeyCode.C;
            case "D":
                return KeyCode.D;
            case "E":
                return KeyCode.E;
            case "F":
                return KeyCode.F;
            case "G":
                return KeyCode.G;
            case "H":
                return KeyCode.H;
            case "I":
                return KeyCode.I;
            case "J":
                return KeyCode.J;
            case "K":
                return KeyCode.K;
            case "L":
                return KeyCode.L;
            case "M":
                return KeyCode.M;
            case "N":
                return KeyCode.N;
            case "O":
                return KeyCode.O;
            case "P":
                return KeyCode.P;
            case "Q":
                return KeyCode.Q;
            case "R":
                return KeyCode.R;
            case "S":
                return KeyCode.S;
            case "T":
                return KeyCode.T;
            case "U":
                return KeyCode.U;
            case "V":
                return KeyCode.V;
            case "W":
                return KeyCode.W;
            case "X":
                return KeyCode.X;
            case "Y":
                return KeyCode.Y;
            case "Z":
                return KeyCode.Z;
            case "SPACE":
            case "ESPACE":
            case " ":
                return KeyCode.Space;
            case "ESCAPE":
            case "ECHAP":
                return KeyCode.Escape;

            default:
                return KeyCode.None;
        }
    }
}

[CreateAssetMenu(fileName = "CharacterStats", menuName = "ScriptableObjects/CharacterStats", order = 1)]
public class CharacterStatsSO : ScriptableObject
{
    private readonly string _baseDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                       "/RepairGame/ConfigurationFiles/";

    private string path => _baseDir + "spline.json";
    public KeyCode Pause = KeyCode.Escape;
    public KeyCode Interact = KeyCode.E;
    public KeyCode MeleeAttack = KeyCode.K;
    public KeyCode RangedAttack = KeyCode.O;
    public KeyCode Jump = KeyCode.Space;
    public KeyCode HorizontalPositive = KeyCode.D;
    public KeyCode HorizontalNegative = KeyCode.A;
    public KeyCode VerticalPositive = KeyCode.W;
    public KeyCode VerticalNegative = KeyCode.S;

    public float jumpHeight;
    public float inputLatency;

    public int highestCheckpoint = 0;

    public bool shouldReset = false;

    public void ResetValue()
    {
        if (shouldReset)
        {
            shouldReset = false;
            Pause = KeyCode.Escape;
            Interact = KeyCode.E;
            MeleeAttack = KeyCode.K;
            RangedAttack = KeyCode.O;
            Jump = KeyCode.Space;
            HorizontalPositive = KeyCode.D;
            HorizontalNegative = KeyCode.A;
            VerticalPositive = KeyCode.W;
            VerticalNegative = KeyCode.S;
        }
    }

    private static CharacterStats[] __initialCharStats = new[]
    {
        new CharacterStats("ESCAPE", "E", "O", "P", "W", "D", "A", "W", "S", 0.0f, 0.0f),
        new CharacterStats("ESCAPE", "E", "O", "P", " ", "A", "D", "W", "S", 0.0f, 0.0f),
        new CharacterStats("ESCAPE", "E", "O", "P", "SPACE", "E", "A", "W", "S", 0.0f, 0.0f)
    };

    private List<CharacterStats> checkpoints = new List<CharacterStats>(__initialCharStats);

    public event Action<CharacterStatsSO> OnInputUpdate;

    public void ReadFromConfigFile()
    {
        ResetValue();
        if (File.Exists(path))
        {
            var reader = new StreamReader(path);
            // Debug.Log();
            var content = reader.ReadToEnd();
            var json = JsonUtility.FromJson<CharacterStatsString>(content);
            var stats = new CharacterStats(json);
            Pause = stats.Pause;
            Interact = stats.Interact;
            MeleeAttack = stats.MeleeAttack;
            RangedAttack = stats.RangedAttack;
            Jump = stats.Jump;
            HorizontalPositive = stats.HorizontalPositive;
            HorizontalNegative = stats.HorizontalNegative;
            VerticalPositive = stats.VerticalPositive;
            VerticalNegative = stats.VerticalNegative;
            jumpHeight = stats.jumpHeight;
            inputLatency = stats.inputLatency;
            if (OnInputUpdate != null)
                OnInputUpdate(this);
            reader.Close();
        }
        else
        {
            WriteFromConfigFile();
        }

    }

    public IEnumerator WatchFile()
    {
        while (true)
        {
            ReadFromConfigFile();
            yield return new WaitForSeconds(5);
        }
    }


    public void WriteFromConfigFile()
    {
        Directory.CreateDirectory(_baseDir);
        var writer = new StreamWriter(path);
        var charString = new CharacterStatsString(this);
        writer.Write(JsonUtility.ToJson(charString));
        writer.Close();

    }

    public void OverwriteFromConfigFile(CharacterStats charStats)
    {
        Directory.CreateDirectory(_baseDir);
        var writer = new StreamWriter(path);
        var charString = new CharacterStatsString(charStats);
        writer.Write(JsonUtility.ToJson(charString));
        writer.Close();
        ReadFromConfigFile();
    }

    public void ScrambleStats(int checkpoint)
    {
        Debug.Log("checkpoint " + checkpoint);
        if (checkpoint > highestCheckpoint)
        {
            Debug.Log("is higher ");
            highestCheckpoint = checkpoint;
            var charStats = checkpoints[checkpoint - 1];
            if (charStats != null)
            {
                OverwriteFromConfigFile(charStats);
            }
        }
    }
}
