using System;
using System.Collections.Generic;

[Serializable]
public class AccountState{

    private string login_;
    private int characterSlotLimit_;
    private GameState[] characters_;

    public string Login
    {
        get { return login_; }
    }

    public AccountState()
    {
        login_ = "default";
        characterSlotLimit_ = 5;
        characters_ = new GameState[characterSlotLimit_];
    }
}
