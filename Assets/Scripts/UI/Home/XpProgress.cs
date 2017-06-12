using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class XpProgress : MonoBehaviour {

    public Text text_;
    public Slider slider_;
    public Text levelText_;

    static int xp_, targetXp_;

    //animation
    const float
        animationStartSpeed_ = 0,
        animationAcceleration_ = 2000;

    bool animationStarted_;
    float animationCurrentSpeed_;


    void Awake()
    {
        animationStarted_ = true;
    }
    
    void Start () {
        targetXp_ = GlobalData.instance.playerData_.xp;
    }
	
	// Update is called once per frame
	void Update () {
		if(!animationStarted_ && xp_ != targetXp_)
        {
            startAnimation();
        }
        if (animationStarted_)
        {
            float dTime = Time.deltaTime;

            xp_ += (int)(animationCurrentSpeed_ * dTime);

            animationCurrentSpeed_ += animationAcceleration_ * dTime;

            if(xp_ >= targetXp_)
            {
                xp_ = targetXp_;
                stopAnimation();
            }

            int 
                currentXp = XpLevels.currentXp(xp_), 
                requiredXp = XpLevels.requiredXp(xp_);

            text_.text = currentXp.ToString() + " / " + requiredXp.ToString();
            if (requiredXp != 0)
                slider_.value = (float)currentXp / requiredXp;
            else slider_.value = 0;

            levelText_.text = XpLevels.level(xp_).ToString();

        }
    }

    void startAnimation()
    {
        animationStarted_ = true;
        animationCurrentSpeed_ = animationStartSpeed_;
    }

    void stopAnimation()
    {
        animationStarted_ = false;
    }
}
