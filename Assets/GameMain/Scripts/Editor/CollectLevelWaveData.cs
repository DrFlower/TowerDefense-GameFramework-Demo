//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using TowerDefense.Level;
//using System.IO;

//public class CollectLevelWaveData
//{
//    public class WaveInfo
//    {
//        public int index;
//        public int enemyId;
//        public string enemyName;
//        public float spawnTime;
//    }

//    private static int waveElementIndex = 10001;
//    private static int waveIndex = 1001;
//    private static int levelIndex = 1;

//    [MenuItem("GameObject/Collect")]
//    private static void Collect()
//    {
//        GameObject select = Selection.gameObjects[0];
//        if (select == null)
//            return;

//        //int index = 1001;
//        //Debug.Log("start index:" + waveElementIndex);

//        Dictionary<string, int> dic = new Dictionary<string, int>();
//        dic.Add("Hoverbuggy", 101);
//        dic.Add("Hovercopter", 102);
//        dic.Add("Hovertank", 103);
//        dic.Add("Hoverboss", 104);
//        dic.Add("Super Hoverbuggy", 105);
//        dic.Add("Super Hovercopter", 106);
//        dic.Add("Super Hovertank", 107);
//        dic.Add("Super Hoverboss", 108);

//        Dictionary<int, string> dicNum = new Dictionary<int, string>();
//        dicNum.Add(1, "一");
//        dicNum.Add(2, "二");
//        dicNum.Add(3, "三");
//        dicNum.Add(4, "四");
//        dicNum.Add(5, "五");
//        dicNum.Add(6, "六");
//        dicNum.Add(7, "七");
//        dicNum.Add(8, "八");
//        dicNum.Add(9, "九");
//        dicNum.Add(10, "十");

//        List<WaveInfo> waveInfos = new List<WaveInfo>();

//        string temp = string.Empty;

        

//        Transform root = select.transform;
//        for (int i = 0; i < root.childCount; i++)
//        {
//            Wave wave = root.GetChild(i).GetComponent<Wave>();
//            int starWavetIndex = waveElementIndex;
//            foreach (var item in wave.spawnInstructions)
//            {
//                waveInfos.Add(new WaveInfo() { index = waveElementIndex++, enemyName = item.agentConfiguration.name, enemyId = dic[item.agentConfiguration.name], spawnTime = item.delayToSpawn });
//            }

//            float waitTime = 0;

//            TimedWave timedWave = wave as TimedWave;
//            if (timedWave != null)
//                waitTime = timedWave.timeToNextWave;

//            temp += string.Format("\t{0}\t{1}\t{2}\t{3}\n", waveIndex, string.Format("第{0}关第{1}波", dicNum[levelIndex], dicNum[i + 1]), waitTime, string.Format("\"\"\"{0},{1}\"\"\"", starWavetIndex, waveElementIndex - 1));

//            waveIndex++;
//        }


//        levelIndex++;

//        //Debug.Log("end index:" + waveElementIndex);
//        //foreach (var item in waveInfos)
//        //{
//        //    Debug.Log(string.Format("index{0},enemy id:{1},enemy name:{2},spawn time:{3}", item.index, item.enemyId, item.enemyName, item.spawnTime));
//        //}
//        //string path = @"C:\Users\未闻花名\Desktop\WaveElement.txt";
//        //string context = File.ReadAllText(path);
//        //foreach (var item in waveInfos)
//        //{
//        //    context += string.Format("\t{0}\t{1}\t{2}\t{3}\n", item.index, "", item.enemyId, item.spawnTime);
//        //}

//        //File.WriteAllText(path, context);

//        string path = @"C:\Users\未闻花名\Desktop\Wave.txt";
//        string context = File.ReadAllText(path);
//        context += temp;

//        File.WriteAllText(path, context);
//    }

//}
