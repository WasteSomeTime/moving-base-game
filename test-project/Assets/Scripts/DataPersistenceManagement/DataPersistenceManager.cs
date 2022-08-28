using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour {
    public static DataPersistenceManager instance { get; private set; }

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    [SerializeField] private string fileName;

    private void Awake() {
        if (instance != null) {
            Debug.LogError("Found more than one Data Storage Manager in the scene!");
        }
        instance = this;
    }

    private void Start() {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
    }

    public void NewGame() {
        this.gameData = new GameData();
    }

    public void LoadGame() {
        this.gameData = dataHandler.Load();

        // initialize new game if no data can be loaded
        if (this.gameData == null) {
            Debug.Log("No data found. Initializing default data.");
            NewGame();
        }

        // push loaded data to other scripts
        foreach(IDataPersistence dataObject in dataPersistenceObjects) {
            dataObject.LoadData(gameData);
        }
    }

    public void SaveGame() {
        foreach(IDataPersistence dataObject in dataPersistenceObjects) {
            dataObject.SaveData(ref gameData);
        }
        dataHandler.Save(gameData);
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects() {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
