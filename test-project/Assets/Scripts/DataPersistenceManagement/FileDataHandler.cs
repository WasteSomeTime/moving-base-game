using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler {
    private string dataDirPath = "";
    private string dataFileName = "";
    private readonly string encryptionCodeWord = "somethinggoeshere";

    public FileDataHandler(string dataDirPath, string dataFileName) {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load() {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData data = null;
        if (File.Exists(fullPath)) {
            try {
                string loadedData = "";
                using(FileStream stream = new FileStream(fullPath, FileMode.Open)) {
                    using(StreamReader reader = new StreamReader(stream)) {
                        loadedData = reader.ReadToEnd();
                    }
                }
                loadedData = EncryptDecrypt(loadedData);
                data = JsonUtility.FromJson<GameData>(loadedData);
            }
            catch(Exception exception) {
            Debug.LogError($"Error occured when trying to load data from file: {fullPath}\n{exception}");
            }
        }

        return data;
    }

    public void Save(GameData data) {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try {
            // create directory if it doesn't exists
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string saveData = JsonUtility.ToJson(data, true);
            saveData = EncryptDecrypt(saveData);

            using(FileStream stream = new FileStream(fullPath, FileMode.Create)) {
                using(StreamWriter writer = new StreamWriter(stream)) {
                    writer.Write(saveData);
                }
            }
        }
        catch(Exception exception) {
            Debug.LogError($"Error occured when trying to save data to file: {fullPath}\n{exception}");
        }
    }

    private string EncryptDecrypt(string data) {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++) {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }
}
