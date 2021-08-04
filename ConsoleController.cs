using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class controls input into the console for "rc" commands.
/// </summary>
public class ConsoleController
{

    /// <summary>
    /// Revives the player.
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="reason"></param>
    public static void revivePlayer(int playerID, string reason, InputField f1MenuInputField)
    {
        if (f1MenuInputField == null) { return; }

        var rcCommand = string.Format("serverAdmin revive {0}", playerID, reason);
        f1MenuInputField.onEndEdit.Invoke(rcCommand);
    }

    public static void revivePlayerDelayed(int playerID, string reason, int time, InputField f1MenuInputField)
    {
        if (f1MenuInputField == null) { return; }
        var rcCommand = string.Format("delayed {0} serverAdmin revive {1}",time, playerID, reason);
        f1MenuInputField.onEndEdit.Invoke(rcCommand);
    }

    /// <summary>
    /// What is says on the tin.
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="reason"></param>
    public static void slayPlayer(int playerID, string reason, InputField f1MenuInputField)
    {
        if (f1MenuInputField == null) { return; }

        var rcCommand = string.Format("serverAdmin slay {0} {1}", playerID, reason);
        f1MenuInputField.onEndEdit.Invoke(rcCommand);
    }

    /// <summary>
    /// Yup slaps them.
    /// TODO: add a pm. 
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="damege"></param>
    /// <param name="reason"></param>
    public static void slapPlayer(int playerID, int damege, string reason, InputField f1MenuInputField)
    {
        if (f1MenuInputField == null) { return; }
        var rcCommand = string.Format("serverAdmin slap {0} {1} {2}", playerID, damege, reason);
        f1MenuInputField.onEndEdit.Invoke(rcCommand);
    }

    /// <summary>
    /// broadcast without admin prefix added
    /// </summary>
    /// <param name="message"></param>
    public static void broadcast(string message, InputField f1MenuInputField)
    {
        if (f1MenuInputField == null) { return; }
        f1MenuInputField.onEndEdit.Invoke("broadcast " + message);
    }
    /// <summary>
    /// Broadcasts with admin prefix added
    /// </summary>
    /// <param name="message"></param>
    /// <param name="f1MenuInputField"></param>
    public static void broadcast_prefix(string message, InputField f1MenuInputField)
    {
        if (f1MenuInputField == null) { return; }
        f1MenuInputField.onEndEdit.Invoke("broadcast " +AutoAdmin.MESSAGE_PREFIX +" " + message);
    }

    /// <summary>
    /// private message the player
    /// </summary>
    /// <param name="message"></param>
    public static void privateMessage(int playerID, string message, InputField f1MenuInputField)
    {
        if (f1MenuInputField == null) { return; }
        f1MenuInputField.onEndEdit.Invoke("serverAdmin privateMessage "+playerID+" " + message);
    }


    public static void sendInternalCharge(string message, InputField f1MenuInputField)
    {
        if (f1MenuInputField == null) { return; }
        f1MenuInputField.onEndEdit.Invoke(message);
    }

    public static void invoke(string message)
    {
        if (AutoAdmin.f1MenuInputField == null) { return; }
        AutoAdmin.f1MenuInputField.onEndEdit.Invoke(message);
    }

}
