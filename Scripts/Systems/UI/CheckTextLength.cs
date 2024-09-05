namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using System.Collections.Generic;
using MyECS.Components;

public static class UIUtils
{
    // public Filter EntityFilter;


    public static void CheckTextBox(RichTextLabel textbox, List<DialogStorage> dialogStorage)
    {
        for (int npc = 0; npc < dialogStorage.Count; npc++)
        {
            DialogStorage storage = dialogStorage[npc];
            for (int group = 0; group < storage.Dialog.Count; group++)
            {
                List<string> dialogGroup = storage.Dialog[group];
                for (int line = 0; line < dialogGroup.Count; line++)
                {
                    string dialog = storage.GetDialog(group, line);
                    textbox.Text = dialog;
                    if (textbox.GetLineCount() > 5)
                    {
                        GD.Print($"too long: npc {npc}, group {group}, line {line}, {dialog}");
                    }
                }
            }
        }
    }

    public static AudioStreamPlayer[] CreateAudioPlayers(AudioStream[] audioStreams, Node parent, string busName)
    {
        AudioStreamPlayer[] players = new AudioStreamPlayer[audioStreams.Length];
        for (int i = 0; i < audioStreams.Length; i++)
        {
            AudioStreamPlayer audio = new AudioStreamPlayer
            {
                Stream = audioStreams[i],
                Bus = busName
            };
            parent.AddChild(audio);
            players[i] = audio;
        }
        return players;
    }
}