using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class obstracleClass
{
    public ObstracleType _obstracleType;
    public CoinType _coinType;
}
public class LevelEditor : EditorWindow {
    private static LevelEditor window;
    public static obstracleClass[] obstracleArray = new obstracleClass[240];
    const int colums = 3;
    const int rows = 80;
    int levelNumber = 1;
    private Vector2 scrollViewVector;
    ObstracleType mObstracleType;
    CoinType mCointType;

    [MenuItem("Window/Game editor")]

    public static void showWindow()
    {
        // Get existing open window or if none, make a new one:
        window = (LevelEditor)EditorWindow.GetWindow(typeof(LevelEditor));
        window.Show();
    }
    void Initialize()
    {
        obstracleArray = new obstracleClass[colums * rows];

        for (int i = 0; i < obstracleArray.Length; i++)
        {
            obstracleClass obs = new obstracleClass();
            obs._obstracleType = ObstracleType.NONE;
            obs._coinType = CoinType.None;
            obstracleArray[i] = obs;
        }
    }
    void OnFocus()
    {
        Initialize();
        LoadDataFromLocal(levelNumber);
    }

    void OnGUI()
    {
        scrollViewVector = GUI.BeginScrollView(new Rect(25, 45, position.width - 30, position.height), scrollViewVector, new Rect(0, 0, 400, 3700));
        GUILayout.Space(20);
        GUILayout.Label("LEVEL EDITOR", EditorStyles.boldLabel);
        GUILayout.Space(20);
        GUILevel();
        GUILayout.Space(20);
        GuiBlocks();
        GUILayout.Space(20);
        GuiEditor();
        GUI.EndScrollView();
    }

    void GuiBlocks()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(30);
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        GUILayout.Space(30);
        if (GUILayout.Button("Clear", new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(50) }))
        {
            for (int i = 0; i < obstracleArray.Length; i++)
            {
                obstracleArray[i]._obstracleType = ObstracleType.NONE;
                obstracleArray[i]._coinType = CoinType.None;
            }
            SaveMap();
        }

        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();


        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Space(30);
        GUILayout.BeginVertical();

        GUILayout.Label("Obstracles:", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.Space(50);
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();

        GUI.color = new Color(1, 1, 1, 1f);

        if (GUILayout.Button("X", new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(50) }))
        {
            mObstracleType = ObstracleType.NONE;
            mCointType = CoinType.None;
        }
        GUILayout.Label(" - none", EditorStyles.boldLabel);

        if (GUILayout.Button("CX", new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(50) }))
        {
            mCointType = CoinType.Clear;
        }
        GUILayout.Label(" -Coin None", EditorStyles.boldLabel);

        if (GUILayout.Button("1", new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(50) }))
        {
            mObstracleType = ObstracleType.Truck;
            mCointType = CoinType.None;
        }
        GUILayout.Label(" - Truck", EditorStyles.boldLabel);

        if (GUILayout.Button("2", new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(50) }))
        {
            mObstracleType = ObstracleType.BusIdle;
            mCointType = CoinType.None;
        }
        GUILayout.Label(" - busIdle", EditorStyles.boldLabel);

        GUILayout.EndHorizontal();
        GUILayout.Space(50);
        GUILayout.BeginHorizontal();

        GUI.color = new Color(0.7f, 0.36f, 0.92f, 1);
        if (GUILayout.Button("3", new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(50) }))
        {
            mObstracleType = ObstracleType.busMove;
            mCointType = CoinType.None;
        }
        GUILayout.Label(" - busMove", EditorStyles.boldLabel);

        

        GUI.color = new Color(0.93f, 0.4f, 0.6f, 1);
        if (GUILayout.Button("4", new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(50) }))
        {
            mObstracleType = ObstracleType.carIdle;
            mCointType = CoinType.None;
        }
        GUILayout.Label(" - carIdle", EditorStyles.boldLabel);

        GUI.color = new Color(1, 1, 1, 1f);

        if (GUILayout.Button("5", new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(50) }))
        {
            mObstracleType = ObstracleType.carMove;
            mCointType = CoinType.None;
        }
        GUILayout.Label(" - carMove", EditorStyles.boldLabel);

        if (GUILayout.Button("6", new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(50) }))
        {
            mObstracleType = ObstracleType.HudleJump;
            mCointType = CoinType.None;
        }
        GUILayout.Label(" - HudleJump", EditorStyles.boldLabel);

        GUILayout.EndHorizontal();
        GUILayout.Space(50);
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("7", new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(50) }))
        {
            mObstracleType = ObstracleType.HudleDow;
            mCointType = CoinType.None;
        }
        GUILayout.Label(" - HudleDow", EditorStyles.boldLabel);

        if (GUILayout.Button("8", new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(50) }))
        {
            mObstracleType = ObstracleType.HudleTwoWay;
            mCointType = CoinType.None;
        }
        GUILayout.Label(" - HudleTwoWay", EditorStyles.boldLabel);

        if (GUILayout.Button("c1", new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(50) }))
        {
            mObstracleType = ObstracleType.NONE;
            mCointType = CoinType.Down;
        }
        GUILayout.Label(" -CoinL", EditorStyles.boldLabel);
        if (GUILayout.Button("c2", new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(50) }))
        {
            mObstracleType = ObstracleType.NONE;
            mCointType = CoinType.Up;
        }
        GUILayout.Label(" - CoinU", EditorStyles.boldLabel);

        GUILayout.EndHorizontal();
        GUILayout.Space(50);
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("c3", new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(50) }))
        {
            mObstracleType = ObstracleType.NONE;
            mCointType = CoinType.CurveDown;
        }
        GUILayout.Label(" - curveDown", EditorStyles.boldLabel);
        if (GUILayout.Button("c4", new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(50) }))
        {
            mObstracleType = ObstracleType.NONE;
            mCointType = CoinType.CurveUp;
        }
        GUILayout.Label(" - curveUp", EditorStyles.boldLabel);

        if (GUILayout.Button("POW", new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(50) }))
        {
            mObstracleType = ObstracleType.NONE;
            mCointType = CoinType.PowerUp;
        }
        GUILayout.Label(" -PowerUp", EditorStyles.boldLabel);

        if (GUILayout.Button("COC", new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(50) }))
        {
            mObstracleType = ObstracleType.NONE;
            mCointType = CoinType.OnCar;
        }
        GUILayout.Label(" -OnCar", EditorStyles.boldLabel);

        GUILayout.EndHorizontal();
        GUILayout.Space(50);
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("BusF", new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(50) }))
        {
            mObstracleType = ObstracleType.BusFwd;
            mCointType = CoinType.None;
        }
        GUILayout.Label(" -BusF", EditorStyles.boldLabel);

        if (GUILayout.Button("Blkr", new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(50) }))
        {
            mObstracleType = ObstracleType.Blocker;
            mCointType = CoinType.None;
        }
        GUILayout.Label(" - Blocker", EditorStyles.boldLabel);

        if (GUILayout.Button("3B", new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(50) }))
        {
            mObstracleType = ObstracleType.BusThrible;
            mCointType = CoinType.None;
        }
        GUILayout.Label(" - 3Bus", EditorStyles.boldLabel);

        if (GUILayout.Button("Gate", new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(50) }))
        {
            mObstracleType = ObstracleType.Gate;
            mCointType = CoinType.None;
        }
        GUILayout.Label(" - Gate", EditorStyles.boldLabel);

        if (GUILayout.Button("Gate", new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(50) }))
        {
            mObstracleType = ObstracleType.Gate;
            mCointType = CoinType.None;
        }
        GUILayout.Label(" - Gate", EditorStyles.boldLabel);

        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    void GuiEditor()
    {
        GUILayout.EndHorizontal();
        GUILayout.Space(30);
        GUILayout.BeginHorizontal();
        GUILayout.Label("LEVEL", EditorStyles.boldLabel);
        GUILayout.EndHorizontal();
        GUILayout.BeginVertical();
        for (int row = 0; row < rows; row++)
        {
            GUILayout.BeginHorizontal();

            for (int col = 0; col < colums; col++)
            {
                Color squareColor = new Color(0.8f, 0.8f, 0.8f);
                var imageButton = new object();
                string n = "";
                
                if (obstracleArray[row * colums + col]._obstracleType == ObstracleType.NONE)
                {
                    n = " ";
                }
                if (obstracleArray[row * colums + col]._obstracleType == ObstracleType.Truck)
                {
                    n = "1";
                }
                if (obstracleArray[row * colums + col]._obstracleType == ObstracleType.BusIdle)
                {
                    n = "2";
                }
                if (obstracleArray[row * colums + col]._obstracleType == ObstracleType.busMove)
                {
                    n = "3";
                }
                if (obstracleArray[row * colums + col]._obstracleType == ObstracleType.carIdle)
                {
                    n = "4";
                }
                if (obstracleArray[row * colums + col]._obstracleType == ObstracleType.carMove)
                {
                    n = "5";
                }
                if (obstracleArray[row * colums + col]._obstracleType == ObstracleType.HudleJump)
                {
                    n = "6";
                }
                if (obstracleArray[row * colums + col]._obstracleType == ObstracleType.HudleDow)
                {
                    n = "7";
                }
                if (obstracleArray[row * colums + col]._obstracleType == ObstracleType.HudleTwoWay)
                {
                    n = "8";
                }
                if (obstracleArray[row * colums + col]._obstracleType == ObstracleType.Block)
                {
                    n = "B";
                }
                if (obstracleArray[row * colums + col]._obstracleType == ObstracleType.Gate)
                {
                    n = "GT";
                }
                if (obstracleArray[row * colums + col]._obstracleType == ObstracleType.CaveLeft)
                {
                    n = "CL";
                }
                if (obstracleArray[row * colums + col]._obstracleType == ObstracleType.BusFwd)
                {
                    n = "BF";
                }
                if (obstracleArray[row * colums + col]._obstracleType == ObstracleType.Blocker)
                {
                    n = "Blkr";
                }
                if (obstracleArray[row * colums + col]._coinType == CoinType.Down)
                {
                    squareColor = Color.yellow;
                }
                if (obstracleArray[row * colums + col]._coinType == CoinType.Up)
                {
                    squareColor = Color.red;
                }
                if (obstracleArray[row * colums + col]._coinType == CoinType.CurveDown)
                {
                    squareColor = Color.green;
                }
                if (obstracleArray[row * colums + col]._coinType == CoinType.CurveUp)
                {
                    squareColor = Color.cyan;
                }
                if (obstracleArray[row * colums + col]._coinType == CoinType.PowerUp)
                {
                    squareColor = Color.gray;
                }
                if (obstracleArray[row * colums + col]._coinType == CoinType.OnCar)
                {
                    squareColor = Color.blue;
                }
                if (obstracleArray[row * colums + col]._obstracleType == ObstracleType.BusThrible)
                {
                    n = "3B";
                }
                   GUI.color = squareColor;
                if (GUILayout.Button(n, new GUILayoutOption[] {
                    GUILayout.Width (30),
                    GUILayout.Height (30)
                }))
                {
                    SetType(col, row);
                }
            }
            GUILayout.EndHorizontal();
        }
    }
    void GUILevel()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Test level", new GUILayoutOption[] { GUILayout.Width(150) }))
        {
            //TestLevel ();
            //TestLevel();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Space(30);
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Level:", EditorStyles.boldLabel, new GUILayoutOption[] { GUILayout.Width(50) });
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Space(30);
        if (GUILayout.Button("<<", new GUILayoutOption[] { GUILayout.Width(50) }))
        {
            PreviousLevel();
        }

        string changeLvl = GUILayout.TextField(" " + levelNumber, new GUILayoutOption[] { GUILayout.Width(50) });
        try
        {
            if (int.Parse(changeLvl) != levelNumber)
            {
                if (LoadDataFromLocal(int.Parse(changeLvl)))
                    levelNumber = int.Parse(changeLvl);

            }
        }
        catch (Exception)
        {
            throw;
        }

        if (GUILayout.Button(">>", new GUILayoutOption[] { GUILayout.Width(50) }))
        {
            NextLevel();
        }

        if (GUILayout.Button("New level", new GUILayoutOption[] { GUILayout.Width(100) }))
        {
            AddLevel();
        }


        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Space(60);

        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
    }
    void PreviousLevel()
    {
        levelNumber--;
        if (levelNumber < 1)
            levelNumber = 1;
        if (!LoadDataFromLocal(levelNumber))
            levelNumber++;
    }
    void NextLevel()
    {
        levelNumber++;
        if (!LoadDataFromLocal(levelNumber))
            levelNumber--;
    }
    void AddLevel()
    {
        SaveMap();
        levelNumber = GetLastLevel() + 1;
        Initialize();
        SaveMap();
    }
    int GetLastLevel()
    {
        TextAsset mapText = null;
        for (int i = levelNumber; i < 50000; i++)
        {
            mapText = Resources.Load("Levels/" + i) as TextAsset;
            if (mapText == null)
            {
                return i - 1;
            }
        }
        return 0;
    }
    void SetType(int col, int row)
    {
        obstracleClass obs = obstracleArray[row * colums + col];
       
        if(mCointType != CoinType.None)
        {
            if(mCointType == CoinType.Clear)
            {
                obs._coinType = CoinType.None;
            }
            else
            {
                obs._coinType = mCointType;
            }          
        }
        else
        {
            if (mObstracleType == ObstracleType.NONE)
            {
                obs._obstracleType = mObstracleType;
            }
            else if (obs._obstracleType == ObstracleType.NONE)
            {
                obs._obstracleType = mObstracleType;
                if (obs._obstracleType == ObstracleType.BusIdle || obs._obstracleType == ObstracleType.busMove)
                {
                    BlockCol(7, col, row);
                }
                else if (obs._obstracleType == ObstracleType.Truck)
                {
                    BlockCol(6, col, row);
                }
                else if (obs._obstracleType == ObstracleType.carIdle || obs._obstracleType == ObstracleType.carMove)
                {
                    BlockCol(3, col, row);
                }
                else if (obs._obstracleType == ObstracleType.BusThrible)
                {
                    BlockCol(23, col, row);
                }
            }
        }      
        SaveMap();
    }

    void BlockCol(int count,int col,int row)
    {
        for(int i = 1; i <= count; i++)
        {
            if(row+i < rows)
            {
                obstracleArray[(row + i) * colums + col]._obstracleType = ObstracleType.Block;
            }
        }
    }
    
    void SaveMap()
    {
        string saveString = "";

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < colums; col++)
            {
                saveString += (int)obstracleArray[row * colums + col]._coinType;
                saveString += (int)obstracleArray[row * colums + col]._obstracleType;

                if (col < (colums - 1))
                    saveString += " ";
            }
            if (row < (rows - 1))
                saveString += "\r\n";
        }

        /// saving data file
        /// 
        if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
        {
            string activeDir = Application.dataPath + @"/Resources/Levels/";
            string newPath = System.IO.Path.Combine(activeDir, levelNumber + ".txt");
            StreamWriter sw = new StreamWriter(newPath);
            sw.Write(saveString);
            sw.Close();
        }
        AssetDatabase.Refresh();
    }
    public bool LoadDataFromLocal(int currentLevel)
    {
        TextAsset mapText = Resources.Load("Levels/" + currentLevel) as TextAsset;
        if (mapText == null)
        {
            return false;
        }
        ProcessGameDataFromString(mapText.text);
        return true;
    }

    void ProcessGameDataFromString(string mapText)
    {
        string[] lines = mapText.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
        int mapLine = 0;

        foreach (string line in lines)
        {
          string[] st = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
          for (int i = 0; i < st.Length; i++)
          {
                string _value = st[i].ToString();
                obstracleArray[mapLine * colums + i]._coinType = (CoinType)int.Parse(_value[0].ToString());
                obstracleArray[mapLine * colums + i]._obstracleType = (ObstracleType)int.Parse(_value.Substring(1,_value.Length-1).ToString());
          }
             mapLine++;
        }
    }
}
