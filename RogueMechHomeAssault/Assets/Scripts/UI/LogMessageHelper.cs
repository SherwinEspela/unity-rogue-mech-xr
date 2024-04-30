using System.Collections.Generic;

public class LogMessageHelper
{
    public static string GetMessages(List<string> messages)
    {
        if (messages.Count == 0) return string.Empty;
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
