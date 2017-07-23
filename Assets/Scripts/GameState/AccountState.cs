using System;
using System.Collections.Generic;

[Serializable]
public class AccountState{

    private string login_;
    private int characterSlotLimit_;

    public string Login
    {
        get { return login_; }
    }

    public AccountState()
    {
        login_ = "default";
        characterSlotLimit_ = 5;
    }
}
