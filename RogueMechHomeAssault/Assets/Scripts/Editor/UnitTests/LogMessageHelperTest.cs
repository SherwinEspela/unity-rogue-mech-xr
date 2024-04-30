using System.Collections.Generic;
using NUnit.Framework;

public class LogMessageHelperTest
{
    [Test]
    public void GetMessagesTest()
    {
        string result1 = LogMessageHelper.GetMessages("message 1");
        Assert.AreEqual("message 1", result1);

        string result2 = LogMessageHelper.GetMessages("message 2");
        string expectedValue = "message 2\n";
        expectedValue += "message 1\n";

        Assert.IsTrue(result2.Length > 0);
        Assert.AreEqual(expectedValue, result2);
    }
}
