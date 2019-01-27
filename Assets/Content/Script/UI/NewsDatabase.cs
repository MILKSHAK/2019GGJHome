using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NewsDatabase : ScriptableObject
{

    public List<string> normalNews, coronalNews;

    public List<Sprite> normalSprites, coronalSprites;

}
