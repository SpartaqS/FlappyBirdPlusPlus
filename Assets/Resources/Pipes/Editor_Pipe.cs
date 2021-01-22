using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FlappyBirdPlusPlus
{
    [CustomEditor(typeof(Pipe))]
    public class Editior_Pipe : Editor
    {        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            Pipe pipe = (Pipe)target; // get access to fields

            EditorGUILayout.PropertyField(serializedObject.FindProperty("topPipe"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("bottomPipe"));

            if(pipe.topPipe.PipeSprite == null)
            {
                pipe.topPipe.PipeSprite = Resources.Load<Sprite>("Textures/DefaultPipeTop");
            }
            if (pipe.bottomPipe.PipeSprite == null)
            {
                pipe.bottomPipe.PipeSprite = Resources.Load<Sprite>("Textures/DefaultPipeBottom");
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("minimumScore"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("restrictMaximumScore"));
            if (pipe.RestrictMaximumScore)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("maximumScore"));
                if(pipe.MinimumScore > pipe.MaximumScore)
                {
                    pipe.MaximumScore = pipe.MinimumScore;
                }
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("customSpawnWeight"));
            if (pipe.CustomSpawnWeight)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("weight"));
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
