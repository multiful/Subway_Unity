using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Unity.VisualScripting;

public class FileDataHandler
{
    // 데이터가 저장될 디렉토리 경로
    private string dataDirPath = "";
    // 데이터 파일의 이름
    private string dataFileName = "";

    
    // 백업 파일 확장자
    private readonly string backupExtension = ".bak";



    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        // dataDirPath 와 dataFileName 을 초기화.
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        
    }

    public UserData Load(string profileId, bool allowRestoreFromBackup = true)
    {

        // profileId가 null이면 null을 반환
        if (profileId == null)
        {
            return null;
        }
        
        // dataDirPath, profileId, dataFileName을 결합하여 데이터 파일 경로를 만든다.
        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        Debug.Log($" 파일 경로 : {fullPath}");
        UserData loadedData = null;
        // ScriptData loadedscriptData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                // 파일이 존재하면 데이터를 로드하여 UserData 객체로 변환
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                
                

                // 역직렬화
                loadedData = JsonUtility.FromJson<UserData>(dataToLoad);
                Debug.Log(" 파일 데이터 -> UserData 로드되었습니다.");
            }
            catch (Exception e)
            {
                // 예외가 발생하면 백업 파일에서 데이터를 복원하려고 시도
                if (allowRestoreFromBackup)
                {
                    Debug.LogWarning("Failed to load data file. Attempting to roll back.\n" + e);
                    bool rollbackSuccess = AttemptRollback(fullPath);
                    if (rollbackSuccess)
                    {
                        loadedData = Load(profileId, false);
                    }
                    Debug.Log("예외 발생 후 파일 데이터 백업되었습니다.");
                }
                // if we hit this else block, one possibility is that the backup file is also corrupt
                else
                {
                    Debug.LogError("Error occured when trying to load file at path: "
                        + fullPath + " and backup did not work.\n" + e);
                }
            }
        }
        return loadedData;
    }


    public void Save(UserData data, string profileId)
    {
        // profileId가 null이면 반환한다.
        if (profileId == null)
        {
            return;
        }

        // dataDirPath, profileId, dataFileName을 결합하여 파일 경로를 만든다.
        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        string backupFilePath = fullPath + backupExtension;
        try
        {
            // create the directory the file will be written to if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // UserData 객체를 JSON 형식으로 직렬화한다.
            string dataToStore = JsonUtility.ToJson(data, true);


            // 직렬화된 데이터를 파일에 쓴다.
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }

            // 저장된 파일을 검증하고 백업 파일을 생성한다.
            UserData verifiedGameData = Load(profileId);

            if (verifiedGameData != null)
            {
                File.Copy(fullPath, backupFilePath, true);
            }
            // otherwise, something went wrong and we should throw an exception
            else
            {
                throw new Exception("Save file could not be verified and backup could not be created.");
            }
            Debug.Log($" UserData -> 파일 데이터 세이브 되었습니다.");
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    public Dictionary<string, UserData> LoadAllProfiles() 
    {
        Dictionary<string, UserData> profileDictionary = new Dictionary<string, UserData>();

        // 데이터 디렉토리의 모든 프로필을 로드하여 딕셔너리에 저장한다.
        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
        foreach (DirectoryInfo dirInfo in dirInfos)
        {
            string profileId = dirInfo.Name;

            // defensive programming - check if the data file exists
            // if it doesn't, then this folder isn't a profile and should be skipped
            string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning("Skipping directory when loading all profiles because it does not contain data: "
                    + profileId);
                continue;
            }

            // 각 프로필의 데이터를 로드하여 UserData 객체로 변환
            UserData profileData = Load(profileId);
            // defensive programming - ensure the profile data isn't null,
            // because if it is then something went wrong and we should let ourselves know
            if (profileData != null)
            {
                profileDictionary.Add(profileId, profileData);
            }
            else
            {
                Debug.LogError("Tried to load profile but something went wrong. ProfileId: " + profileId);
            }
        }

        return profileDictionary;
    }

    public string GetMostRecentlyUpdatedProfileId()
    {
        string mostRecentProfileId = null;
        // 모든 프로필을 로드하여 가장 최근에 업데이트된 프로필 ID를 반환.

        Dictionary<string, UserData> profilesGameData = LoadAllProfiles();
        foreach (KeyValuePair<string, UserData> pair in profilesGameData)
        {
            string profileId = pair.Key;
            UserData gameData = pair.Value;

            // 게임 데이터가 null 이면 생략.
            if (gameData == null)
            {
                continue;
            }

            // mostRecentProfileld 가 null 이면 최근 값이다.
            if (mostRecentProfileId == null)
            {
                mostRecentProfileId = profileId;
            }
            // 각 프로필의 lastUpdated 시간을 비교하여 가장 최신의 프로필을 선택.
            else
            {
                DateTime mostRecentDateTime = DateTime.FromBinary(profilesGameData[mostRecentProfileId].lastUpdated);
                DateTime newDateTime = DateTime.FromBinary(gameData.lastUpdated);
                // the greatest DateTime value is the most recent
                if (newDateTime > mostRecentDateTime)
                {
                    mostRecentProfileId = profileId;
                }
            }
        }
        return mostRecentProfileId;
    }

    public void Delete(string profileId)
    {
        // profileId가 null이면 바로 반환한다.
        if (profileId == null)
        {
            return;
        }

        // 데이터 파일 경로를 생성하고 파일이 존재하면 디렉토리를 삭제
        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        try
        {
            
            if (File.Exists(fullPath))
            {
                
                Directory.Delete(Path.GetDirectoryName(fullPath), true);
            }
            else
            {
                Debug.LogWarning("Tried to delete profile data, but data was not found at path: " + fullPath);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to delete profile data for profileId: "
                + profileId + " at path: " + fullPath + "\n" + e);
        }
    }

    private bool AttemptRollback(string fullPath)
    {
        // 백업 파일을 사용하여 원래 파일을 복원
        bool success = false;
        string backupFilePath = fullPath + backupExtension;
        try
        {
            // 파일 존재하면 복원
            if (File.Exists(backupFilePath))
            {
                File.Copy(backupFilePath, fullPath, true);
                success = true;
                Debug.LogWarning("Had to roll back to backup file at: " + backupFilePath);
            }
            // otherwise, we don't yet have a backup file - so there's nothing to roll back to
            else
            {
                throw new Exception("Tried to roll back, but no backup file exists to roll back to.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to roll back to backup file at: "
                + backupFilePath + "\n" + e);
        }

        return success;
    }
}
