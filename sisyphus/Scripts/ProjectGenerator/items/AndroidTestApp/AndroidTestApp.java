package com.SIS_REPLACE(APPNAME);

import android.app.Activity;
import android.widget.TextView;
import android.os.Bundle;
import android.content.res.AssetManager;

public class SIS_REPLACE(APPNAME) extends Activity
{
    /** Called when the activity is first created. */
    @Override
    public void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);

        assetManager = getAssets();

        TextView  tv = new TextView(this);

        try{
            System.loadLibrary("SIS_REPLACE(PROJNAME)");
            filesDir = getFilesDir().getAbsolutePath();
            tv.setText("Files dir: " + filesDir + "\nTest result: " + runTest(assetManager, filesDir));
        }
        catch(Exception e){
            tv.setText(e.getMessage());
        }

        setContentView(tv);
    }
    
    public static native String runTest(AssetManager assetManager, String filesDir);

    private AssetManager assetManager;
    private String filesDir;
}
