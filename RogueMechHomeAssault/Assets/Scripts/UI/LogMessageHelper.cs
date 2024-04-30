using System.Collections.Generic;

public class LogMessageHelper
{
    private static List<string> messages = new List<string>();

    public static string GetMessages(string newMessage)
    {
        messages.Add(newMessage);
        if (messages.Count == 1) return messages[0];

        int lastIndex = messages.Count - 1;
        string messageStack = string.Empty;

        for (int i = lastIndex; i >= 0; i--)
        {
            messageStack += $"{messages[i]}\n";
        }

        return messageStack;
    }
}
