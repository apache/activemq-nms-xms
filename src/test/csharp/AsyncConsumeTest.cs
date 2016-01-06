/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Threading;
using NUnit.Framework;

namespace Apache.NMS.Test
{
	//[TestFixture]
	public class AsyncConsumeTest : NMSTest
	{
		protected string RESPONSE_CLIENT_ID;
		protected AutoResetEvent semaphore;
		protected bool received;
		protected IMessage receivedMsg;

		public AsyncConsumeTest(NMSTestSupport testSupport)
			: base(testSupport)
		{
		}

		//[SetUp]
		public override void SetUp()
		{
			base.SetUp();
			semaphore = new AutoResetEvent(false);
			received = false;
			receivedMsg = null;

			RESPONSE_CLIENT_ID = GetTestClientId() + ":RESPONSE";
		}

		//[TearDown]
		public override void TearDown()
		{
			receivedMsg = null;
			base.TearDown();
		}

		//[Test]
		public virtual void TestAsynchronousConsume(
			//[Values(MsgDeliveryMode.Persistent, MsgDeliveryMode.NonPersistent)]
			MsgDeliveryMode deliveryMode, string testQueueRef)
		{
			// IBM XMS doesn't support both synchronous and asynchronous operations
			// in the same session. Needs 2 separate sessions.
			using(IConnection connection = CreateConnectionAndStart(GetTestClientId()))
			using(ISession producerSession = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
			using(ISession consumerSession = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
			using(IDestination producerDestination = GetClearDestination(producerSession, DestinationType.Queue, testQueueRef))
			using(IDestination consumerDestination = GetDestination(consumerSession, DestinationType.Queue, testQueueRef))
			using(IMessageConsumer consumer = consumerSession.CreateConsumer(consumerDestination))
			using(IMessageProducer producer = producerSession.CreateProducer(producerDestination))
			{
				producer.DeliveryMode = deliveryMode;
				consumer.Listener += new MessageListener(OnMessage);

				IMessage request = producerSession.CreateMessage();
				request.NMSCorrelationID = "AsyncConsume";
				request.NMSType = "Test";
				producer.Send(request);

				WaitForMessageToArrive();
				Assert.AreEqual(request.NMSCorrelationID, receivedMsg.NMSCorrelationID, "Invalid correlation ID.");
			}
		}

		//[Test]
		public virtual void TestCreateConsumerAfterSend(
			//[Values(MsgDeliveryMode.Persistent, MsgDeliveryMode.NonPersistent)]
			MsgDeliveryMode deliveryMode, string testQueueRef)
		{
			using(IConnection connection = CreateConnectionAndStart(GetTestClientId()))
			using(ISession producerSession = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
			using(ISession consumerSession = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
			using(IDestination producerDestination = GetClearDestination(producerSession, DestinationType.Queue, testQueueRef))
			using(IDestination consumerDestination = GetDestination(consumerSession, DestinationType.Queue, testQueueRef))
			{
				string correlationId = "AsyncConsumeAfterSend";

				using(IMessageProducer producer = producerSession.CreateProducer(producerDestination))
				{
					producer.DeliveryMode = deliveryMode;
					IMessage request = producerSession.CreateMessage();
					request.NMSCorrelationID = correlationId;
					request.NMSType = "Test";
					producer.Send(request);
				}

				using(IMessageConsumer consumer = consumerSession.CreateConsumer(consumerDestination))
				{
					consumer.Listener += new MessageListener(OnMessage);
					WaitForMessageToArrive();
					Assert.AreEqual(correlationId, receivedMsg.NMSCorrelationID, "Invalid correlation ID.");
				}
			}
		}

		//[Test]
		public virtual void TestCreateConsumerBeforeSendAddListenerAfterSend(
			//[Values(MsgDeliveryMode.Persistent, MsgDeliveryMode.NonPersistent)]
			MsgDeliveryMode deliveryMode, string testQueueRef)
		{
			using(IConnection connection = CreateConnectionAndStart(GetTestClientId()))
			using(ISession producerSession = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
			using(ISession consumerSession = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
			using(IDestination producerDestination = GetClearDestination(producerSession, DestinationType.Queue, testQueueRef))
			using(IDestination consumerDestination = GetDestination(consumerSession, DestinationType.Queue, testQueueRef))
			using(IMessageConsumer consumer = consumerSession.CreateConsumer(consumerDestination))
			using(IMessageProducer producer = producerSession.CreateProducer(producerDestination))
			{
				producer.DeliveryMode = deliveryMode;

				IMessage request = producerSession.CreateMessage();
				request.NMSCorrelationID = "AsyncConsumeAfterSendLateListener";
				request.NMSType = "Test";
				producer.Send(request);

				// now lets add the listener
				consumer.Listener += new MessageListener(OnMessage);
				WaitForMessageToArrive();
				Assert.AreEqual(request.NMSCorrelationID, receivedMsg.NMSCorrelationID, "Invalid correlation ID.");
			}
		}

		//[Test]
		public virtual void TestAsynchronousTextMessageConsume(
			//[Values(MsgDeliveryMode.Persistent, MsgDeliveryMode.NonPersistent)]
			MsgDeliveryMode deliveryMode, string testQueueRef)
		{
			using(IConnection connection = CreateConnectionAndStart(GetTestClientId()))
			using(ISession producerSession = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
			using(ISession consumerSession = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
			using(IDestination producerDestination = GetClearDestination(producerSession, DestinationType.Queue, testQueueRef))
			using(IDestination consumerDestination = GetDestination(consumerSession, DestinationType.Queue, testQueueRef))
			using(IMessageConsumer consumer = consumerSession.CreateConsumer(consumerDestination))
			using(IMessageProducer producer = producerSession.CreateProducer(producerDestination))
			{
				consumer.Listener += new MessageListener(OnMessage);
				producer.DeliveryMode = deliveryMode;

				ITextMessage request = producerSession.CreateTextMessage("Hello, World!");
				request.NMSCorrelationID = "AsyncConsumeTextMessage";
				request.Properties["NMSXGroupID"] = "cheese";
				request.Properties["myHeader"] = "James";

				producer.Send(request);

				WaitForMessageToArrive();
				Assert.AreEqual(request.NMSCorrelationID, receivedMsg.NMSCorrelationID, "Invalid correlation ID.");
				Assert.AreEqual(request.Properties["NMSXGroupID"], receivedMsg.Properties["NMSXGroupID"], "Invalid NMSXGroupID.");
				Assert.AreEqual(request.Properties["myHeader"], receivedMsg.Properties["myHeader"], "Invalid myHeader.");
				Assert.AreEqual(request.Text, ((ITextMessage) receivedMsg).Text, "Invalid text body.");
			}
		}

		//[Test]
		public virtual void TestTemporaryQueueAsynchronousConsume(
			//[Values(MsgDeliveryMode.Persistent, MsgDeliveryMode.NonPersistent)]
			MsgDeliveryMode deliveryMode, string testQueueRef)
		{
			using(IConnection connection = CreateConnectionAndStart(GetTestClientId()))
			using(ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
			using(ITemporaryQueue tempReplyDestination = session.CreateTemporaryQueue())
			using(IDestination destination = GetClearDestination(session, DestinationType.Queue, testQueueRef))
			using(IMessageConsumer consumer = session.CreateConsumer(destination))
			using(IMessageConsumer tempConsumer = session.CreateConsumer(tempReplyDestination))
			using(IMessageProducer producer = session.CreateProducer(destination))
			{
				producer.DeliveryMode = deliveryMode;
				tempConsumer.Listener += new MessageListener(OnMessage);
				consumer.Listener += new MessageListener(OnQueueMessage);

				IMessage request = session.CreateMessage();
				request.NMSCorrelationID = "TemqQueueAsyncConsume";
				request.NMSType = "Test";
				request.NMSReplyTo = tempReplyDestination;
				producer.Send(request);

				WaitForMessageToArrive();
				Assert.AreEqual("TempQueueAsyncResponse", receivedMsg.NMSCorrelationID, "Invalid correlation ID.");
			}
		}

		protected void OnQueueMessage(IMessage message)
		{
			Assert.AreEqual("TemqQueueAsyncConsume", message.NMSCorrelationID, "Invalid correlation ID.");
			using(IConnection connection = CreateConnectionAndStart(RESPONSE_CLIENT_ID))
			using(ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
			using(IMessageProducer producer = session.CreateProducer(message.NMSReplyTo))
			{
				producer.DeliveryMode = message.NMSDeliveryMode;

				ITextMessage response = session.CreateTextMessage("Asynchronous Response Message Text");
				response.NMSCorrelationID = "TempQueueAsyncResponse";
				response.NMSType = message.NMSType;
				producer.Send(response);
			}
		}

		protected void OnMessage(IMessage message)
		{
			receivedMsg = message;
			received = true;
			semaphore.Set();
		}

		protected void WaitForMessageToArrive()
		{
			semaphore.WaitOne((int) receiveTimeout.TotalMilliseconds, true);
			Assert.IsTrue(received, "Should have received a message by now!");
		}
	}
}
