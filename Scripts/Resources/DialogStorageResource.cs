using Godot;
using System;
using System.Collections.Generic;
[GlobalClass]
public partial class DialogStorageResource : Resource
{

    [Export(PropertyHint.MultilineText)]
    public Godot.Collections.Array<string[]> ExportedData { get; set; }
    [Export] public bool useQuotes;

    public DialogStorageResource()
    {
        ExportedData = new Godot.Collections.Array<string[]>();
    }

    public DialogStorage LoadDialog()
    {
        DialogStorage storage = new DialogStorage();
        for (int i = 0; i < ExportedData.Count; i++)
        {
            storage.Dialog.Add(new List<string>(ExportedData[i]));
        }
        storage.useQuotes = useQuotes;
        return storage;
    }
}

public class DialogStorage
{
    // first list is set of dialog groups
    // interior list is each line in dialog group
    public List<List<string>> Dialog = new List<List<string>>();
    // public int lastDialogID = -1;
    public int nextLineID = 0;
    public bool useQuotes;
    public string GetDialog(int dialogID, int lineID)
    {
        if (Dialog.Count <= dialogID)
        {
            return "";
        }
        List<string> dialogGroup = Dialog[dialogID];
        lineID %= dialogGroup.Count;
        return useQuotes ? $"\"{dialogGroup[lineID]}\"" : dialogGroup[lineID];
        // if (lastDialogID == id)
        // {
        //     if (nextLineID >= dialogGroup.Count)
        //     {
        //         nextLineID = 0;
        //     }
        //     string line = dialogGroup[nextLineID];
        //     nextLineID++;
        //     return line;
        // }
        // else
        // {
        //     lastDialogID = id;
        //     nextLineID = 1;
        //     return dialogGroup[0];
        // }
    }
    public bool HasMoreDialog(int dialogID, int lineID)
    {
        if (Dialog.Count <= dialogID)
        {
            // GD.Print("dummy check failed");
            return false;
        }
        List<string> dialogGroup = Dialog[dialogID];
        lineID %= dialogGroup.Count;
        // GD.Print($"line ID: {lineID}, dialog count: {dialogGroup.Count}");
        return dialogGroup.Count != (lineID + 1);
    }
}