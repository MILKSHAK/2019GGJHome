using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

[ExecuteInEditMode]
public class RendererSortingLayerOverride : MonoBehaviour {
    public int sortingLayerID;
    public int sortingOrder;

    public void Apply() {
        Renderer r = GetComponent<Renderer>();
        if (r) {
            r.sortingLayerID = sortingLayerID;
            r.sortingOrder = sortingOrder;
        }
    }

    void Start() {
        if (Application.isPlaying) {
            Destroy(this);
        }
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(RendererSortingLayerOverride))]
public class RenderSortingLayerOverrideEditor : Editor {
    
    public override void OnInspectorGUI() {
        var t = (RendererSortingLayerOverride) target;

        var layers = SortingLayer.layers;
        var names = new string[layers.Length];
        var ints = new int[layers.Length];
        for (int i = 0; i < layers.Length; ++i) {
            names[i] = layers[i].name;
            ints[i] = layers[i].id;
        }

        Undo.RecordObject(t, "Change Sorting Info");
        t.sortingLayerID = EditorGUILayout.IntPopup("Sorting Layer", t.sortingLayerID, names, ints);
        t.sortingOrder = EditorGUILayout.IntField("Sorting Order", t.sortingOrder);

        if (GUILayout.Button("Apply")) {
            t.Apply();
        }
    }

}

#endif
