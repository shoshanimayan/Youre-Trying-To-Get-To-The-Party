using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class TextReader 
{
    public TextReader()
    { 
    }

    public void ReadAddressableTextAsset(AssetReference addressableTextAsset)
    {
        string result = "";
        addressableTextAsset.LoadAssetAsync<TextAsset>().Completed += handle =>
        {
            result = handle.Result.text;
            GameManager.SetTextMode(result);
            Addressables.Release(handle);
        };
        
    }
}
