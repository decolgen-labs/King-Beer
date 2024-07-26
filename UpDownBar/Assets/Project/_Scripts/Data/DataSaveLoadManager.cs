using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using NOOD;
using NOOD.Data;
using UnityEngine;

namespace Game
{
    public class DataSaveLoadManager : MonoBehaviorInstance<DataSaveLoadManager>
    {
        public TableData TableData;
        public int Day;
        public int Money;
        public int Target;

        protected override void ChildAwake()
        {
            Load();
        }

        void Start()
        {
            GameplayManager.Instance.OnEndDay += Save;
            GameplayManager.Instance.OnNextDay += Save;
        }

        private void Save()
        {
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                {"day", TimeManager.Instance.CurrentDay},
                {"money", MoneyManager.Instance.CurrentTotalMoney},
                {"Target", MoneyManager.Instance.CurrentTarget},
                {"TableData", TableData}
            };

            string json = JsonConvert.SerializeObject(data);
            PlayerPrefs.SetString("GameData", json);
            PlayerPrefs.Save();
        }
        private void Load()
        {
            if(PlayerPrefs.HasKey("GameData"))
            {
                string json = PlayerPrefs.GetString("GameData");
                Dictionary<string, object> data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                Day = (int)data["day"];
                Money = (int)data["money"];
                TableData = (TableData)data["TableData"];
                Target = (int)data["Target"];
            }
            else
            {
                Day = 1;
                Money = 0;
                TableData = new TableData();
                Target = 100;
            }
        }
    }
}
