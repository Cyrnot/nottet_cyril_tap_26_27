using GameLibrary.Core;
using GameLibrary.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibraryTests
{
    internal class LogManagerTests
    {
        private class FakeLogWriter : ILogWriter
        {
            public readonly List<string> _logMessageTable = new List<string>();

            public void WriteLine(string line)
            {
                _logMessageTable.Add(line);
            }
        }

        [Test]
        public void LogMessage_WriteTheCorrectSentence()
        {
            EventManager event_manager = new EventManager();
            FakeLogWriter fake_log_writer = new FakeLogWriter();
            LogManager log_manager = new LogManager(fake_log_writer, event_manager);

            event_manager.TriggerEvent(new LogMessageGameEvent("test message"));

            Assert.That(fake_log_writer._logMessageTable.Count, Is.EqualTo(1));
            Assert.That(fake_log_writer._logMessageTable[0], Does.EndWith("test message\n"));
        }

        [Test]
        public void LogMessage_DoesNotReorderSentences()
        {
            EventManager event_manager = new EventManager();
            FakeLogWriter fake_log_writer = new FakeLogWriter();
            LogManager log_manager = new LogManager(fake_log_writer, event_manager);

            event_manager.TriggerEvent(new LogMessageGameEvent("first test message"));
            event_manager.TriggerEvent(new LogMessageGameEvent("second test message"));

            Assert.That(fake_log_writer._logMessageTable.Count, Is.EqualTo(2));
            Assert.That(fake_log_writer._logMessageTable[0], Does.EndWith("first test message\n"));
            Assert.That(fake_log_writer._logMessageTable[1], Does.EndWith("second test message\n"));
        }
    }
}
