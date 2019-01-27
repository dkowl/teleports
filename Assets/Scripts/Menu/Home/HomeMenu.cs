using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeMenu : MonoBehaviour
{
    [SerializeField] private BasicProgressBar xpProgressBar;
    [SerializeField] private BasicProgressBar rpProgressBar;

    private void OnEnable()
    {
        int xp = Main.GameState.CurrentHeroData.Xp;
        int rp = Main.GameState.CurrentHeroData.RankPoints;

        xpProgressBar.EnsureSpawned();
        rpProgressBar.EnsureSpawned();

        xpProgressBar.InterpretAndSetValues(xp);
        rpProgressBar.InterpretAndSetValues(rp);
    }
}
