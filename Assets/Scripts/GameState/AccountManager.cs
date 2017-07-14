using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class AccountManager{

    const string FILE = "accounts.dat";
    private static string dataPath_;
    public static string DataPath
    {
        set { dataPath_ = value; }
    }

    private Dictionary<string, AccountState> accounts_;

    public AccountManager()
    {
        load();
    }

    ~AccountManager()
    {
        save();
    }

    //adds specified account
    public void add(AccountState accountState)
    {
        accounts_.Add(accountState.Login, accountState);
    }

    public bool exists(string key)
    {
        return accounts_.ContainsKey(key);
    }

    public AccountState get(string key)
    {
        if (exists(key))
        {
            return accounts_[key];
        }
        else return new AccountState();
    }

    public void load()
    {
        if (File.Exists(path()))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path(), FileMode.Open);

            AccountManager temp = formatter.Deserialize(fileStream) as AccountManager;
            accounts_ = temp.accounts_;

            fileStream.Close();
        }
        else
        {
            accounts_ = new Dictionary<string, AccountState>();
        }
    }

    public void save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(path(), FileMode.Create);

        formatter.Serialize(fileStream, this);
        fileStream.Close();
    }

    private string path()
    {
        return dataPath_ + "/" + FILE;
    }
}
