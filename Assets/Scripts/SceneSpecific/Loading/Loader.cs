using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour {

    const string PREF_KEY = "RememberedAccount";

    AccountManager accountManager_;

    void Awake()
    {
        AccountManager.DataPath = Application.persistentDataPath;
        accountManager_ = new AccountManager();

        //true if acc loaded to GlobalData
        bool loaded = false;
        //if there is a remembered login
        if (PlayerPrefs.HasKey(PREF_KEY))
        {
            string accKey = PlayerPrefs.GetString(PREF_KEY);
            //load acc if it exists
            if (accountManager_.exists(accKey))
            {
                GlobalData.Account = accountManager_.get(accKey);
                loaded = true;
            }
        }        
        //else create default acc
        if(!loaded)
        {
            AccountState account = new AccountState();
            accountManager_.add(account);
            PlayerPrefs.SetString(PREF_KEY, account.Login);
            GlobalData.Account = account;
        }

        //go to CharacterSelect
        SceneManager.LoadScene("CharacterSelect");
    }
}
