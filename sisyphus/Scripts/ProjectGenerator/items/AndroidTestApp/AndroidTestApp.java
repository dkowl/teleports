package com.SIS_REPLACE(APPNAME);

import android.app.Activity;
import android.widget.TextView;
import android.os.Bundle;

public class SIS_REPLACE(APPNAME) extends Activity
{
    /** Called when the activity is first created. */
    @Override
    public void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);

        /* Create a TextView and set its text to "Hello world" */
        TextView  tv = new TextView(this);

        try{
            System.loadLibrary("SIS_REPLACE(PROJNAME)");
            tv.setText("Test result: " + Integer.toString(runTest()));
        }
        catch(Exception e){
            tv.setText(e.getMessage());
        }

        setContentView(tv);
    }

    public static native int runTest();
}
