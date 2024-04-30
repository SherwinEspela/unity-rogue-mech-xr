using System.Collections.Generic;
using NUnit.Framework;

public class LogMessageHelperTest
{
    [Test]
    public void GetMessagesTest()
    {
        List<string> emptyList = new List<string>();
        string result1 = LogMessageHelper.GetMessages(emptyList);
        Assert.AreEqual(string.Empty, result1);

        string result2 = LogMessageHelper.GetMessages(new List<string> { "one message" });
        Assert.AreEqual("one message", result2);

        string result3 = LogMessageHelper.GetMessages(new List<string> { "message 1", "message 2", "message 3" });
        string expectedValue = "message 3\n";
        expectedValue += "message 2\n";
        expectedValue += "message 1\n";

        Assert.IsTrue(result3.Length > 0);
        Assert.AreEqual(expectedValue, result3);
    }
}
