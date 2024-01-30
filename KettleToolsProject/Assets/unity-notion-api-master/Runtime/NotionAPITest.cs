using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace BennyKok.NotionAPI
{
    public class NotionAPITest : MonoBehaviour
    {
        public string apiKey;
        public string database_id;
        public string dbjson;

        private IEnumerator Start()
        {
            var api = new NotionAPI(apiKey);

            yield return api.GetDatabase<CardDatabasePropertiesDefinition>(database_id, (db) =>
            {
                Debug.Log(db.id);
                Debug.Log(db.created_time);
//                Debug.Log(db.title.First().text.content);
                dbjson = JsonUtility.ToJson(db);
                Debug.Log(dbjson);
                // Save dbjson to file
                var path = "Assets/testjson.json";
                System.IO.File.WriteAllText(path, dbjson);
            });

            // yield return api.QueryDatabase<CardDatabaseProperties>(database_id, (db) =>
            // {
            //     Debug.Log(JsonUtility.ToJson(db));
            // });
        }

        // Block properties
        // [Serializable]
        // public class 

        [Serializable]
        public class CardDatabasePropertiesDefinition
        {
            public MultiSelectPropertyDefinition Tags;
            public TitleProperty Name;
            public CheckboxProperty IsActive;
            public DateProperty Date;
            public SelectPropertyDefinition Type;
            public NumberProperty number;
            public TextProperty Description;
        }

        [Serializable]
        public class CardDatabaseProperties
        {
            public MultiSelectProperty Tags;
            public TitleProperty Name;
            public CheckboxProperty IsActive;
            public DateProperty Date;
            public SelectProperty Type;
            public NumberProperty number;
            public NumberProperty UseTime;
            public TextProperty Description;
        }
    }
}