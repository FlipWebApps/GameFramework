//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//
// Please direct any bugs/comments/suggestions to http://www.flipwebapps.com
// 
// The copyright owner grants to the end user a non-exclusive, worldwide, and perpetual license to this Asset
// to integrate only as incorporated and embedded components of electronic games and interactive media and 
// distribute such electronic game and interactive media. End user may modify Assets. End user may otherwise 
// not reproduce, distribute, sublicense, rent, lease or lend the Assets. It is emphasized that the end 
// user shall not be entitled to distribute or transfer in any way (including, without, limitation by way of 
// sublicense) the Assets in any other way than as integrated components of electronic games and interactive media. 

// The above copyright notice and this permission notice must not be removed from any files.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//----------------------------------------------

#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2
#else
using NUnit.Framework;
using System;

namespace GameFramework.Messaging
{
    /// <summary>
    /// Test cases for messaging. You can also view these to see how you might use the API.
    /// </summary>
    public class MessagingTests {

        #region Helper functions for verifying testing
        bool _testHandlerCalled;
        bool _testCustomHandlerCalled;
        int _handlerCallCount;

        public class CustomMessage1 : BaseMessage
        {
            public readonly bool DummyValue;
            public CustomMessage1(bool dummyValue) {
                DummyValue = dummyValue;
            }
        }

        public class CustomMessage2 : BaseMessage
        {
            public readonly bool DummyValue;
            public CustomMessage2(bool dummyValue)
            {
                DummyValue = dummyValue;
            }
        }

        /// <summary>
        /// A dummy test handler that will update a global value to show it has been called.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool TestHandler(BaseMessage message)
        {
            _testHandlerCalled = true;
            _handlerCallCount++;
            return true;
        }

        /// <summary>
        /// A dummy test handler that will update a global value to show it has been called.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool TestHandlerProcessFor1Second(BaseMessage message)
        {
            var startTime = DateTime.Now;
            while ((DateTime.Now - startTime).TotalSeconds < 1) { } // not coding best practice, but fine for test case
            _testHandlerCalled = true;
            _handlerCallCount++;
            return true;
        }

        /// <summary>
        /// A dummy test handler that will update a global value to show it has been called.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool TestCustomHandler(BaseMessage message)
        {
            var customMessage = (CustomMessage1)message;
            _testCustomHandlerCalled = customMessage.DummyValue;
            _handlerCallCount++;
            return true;
        }

        #endregion Helper functions for verifying testing

        #region Basic Trigger Tests

        [Test]
        public void TriggerMessage()
        {
            // Arrange
            var messenger = new Messenger();
            _testHandlerCalled = false;
            messenger.AddListener<BaseMessage>(TestHandler);

            // Act
            messenger.TriggerMessage(new BaseMessage());

            // Assert
            Assert.IsTrue(_testHandlerCalled, "The test handler was not called!");

            // Cleanup
            messenger.RemoveListener<BaseMessage>(TestHandler);
        }

        #endregion Basic Trigger Tests

        #region Basic Queue Message Tests

        [Test]
        public void QueueMessage()
        {
            // Arrange
            var messenger = new Messenger();
            _testHandlerCalled = false;
            messenger.AddListener<BaseMessage>(TestHandler);

            // Act
            messenger.QueueMessage(new BaseMessage());
            messenger.ProcessQueue();

            // Assert
            Assert.IsTrue(_testHandlerCalled, "The test handler was not called!");

            // Cleanup
            messenger.RemoveListener<BaseMessage>(TestHandler);
        }


        [Test]
        public void QueueMessageOnlyWhenProcessQueueCalled()
        {
            // Arrange
            var messenger = new Messenger();
            _testHandlerCalled = false;
            messenger.AddListener<BaseMessage>(TestHandler);

            // Act
            messenger.QueueMessage(new BaseMessage());

            // Assert
            Assert.IsFalse(_testHandlerCalled, "The message was processed, however we didn't call ProcessQueue!");

            // Cleanup
            messenger.RemoveListener<BaseMessage>(TestHandler);
        }

        [Test]
        public void AllQueuedMessagesRun()
        {
            // Arrange
            var messenger = new Messenger();
            _testCustomHandlerCalled = false;
            _handlerCallCount = 0;
            messenger.AddListener<BaseMessage>(TestHandler);
            messenger.QueueMessage(new BaseMessage());
            messenger.QueueMessage(new BaseMessage());

            // Act
            messenger.ProcessQueue();

            // Assert
            Assert.AreEqual(2, _handlerCallCount, "Not all messages were processed!");

            // Cleanup
            messenger.RemoveListener<BaseMessage>(TestHandler);
        }

        #endregion Basic Queue Message Tests

        #region Custom Message Tests

        [Test]
        public void CustomMessageTriggeredCorrectly()
        {
            // Arrange
            var messenger = new Messenger();
            _testCustomHandlerCalled = false;
            messenger.AddListener<CustomMessage1>(TestCustomHandler);

            // Act
            messenger.TriggerMessage(new CustomMessage1(true));

            // Assert
            Assert.IsTrue(_testCustomHandlerCalled, "The custom test handler was not called or the value was not assigned correctly!");

            // Cleanup
            messenger.RemoveListener<CustomMessage1>(TestCustomHandler);
        }


        [Test]
        public void CustomMessageProcessedByCorrecthandler()
        {
            // Arrange
            var messenger = new Messenger();
            _testCustomHandlerCalled = false;
            messenger.AddListener<CustomMessage1>(TestCustomHandler);

            // Act
            messenger.TriggerMessage(new CustomMessage2(true)); // note: create CustomMessage2

            // Assert
            Assert.IsFalse(_testCustomHandlerCalled, "The custom test handler was called for an incorrect message type!");

            // Cleanup
            messenger.RemoveListener<CustomMessage1>(TestCustomHandler);
        }

        #endregion Custom Message Tests

        #region Multiple Handlers Tests

        [Test]
        public void MultipleHandlersCalled()
        {
            // Arrange
            var messenger = new Messenger();
            _testCustomHandlerCalled = false;
            _handlerCallCount = 0;
            messenger.AddListener<BaseMessage>(TestHandler);
            messenger.AddListener<BaseMessage>(TestHandlerProcessFor1Second);
            messenger.QueueMessage(new BaseMessage());

            // Act
            messenger.ProcessQueue();

            // Assert
            Assert.AreEqual(2, _handlerCallCount, "The message wasn't processed by all handers!");

            // Cleanup
            messenger.RemoveListener<BaseMessage>(TestHandlerProcessFor1Second);
            messenger.RemoveListener<BaseMessage>(TestHandler);
        }

        #endregion Multiple Handlers Tests

    }
}
#endif