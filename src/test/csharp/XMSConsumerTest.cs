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
using System;
using System.Threading;
using Apache.NMS.Test;
using NUnit.Framework;

namespace Apache.NMS.XMS.Test
{
	[TestFixture]
	public class XMSConsumerTest : ConsumerTest
	{
		protected static string DEFAULT_TEST_QUEUE = "defaultTestQueue";
		protected static string DEFAULT_TEST_TOPIC = "defaultTestTopic";

		public XMSConsumerTest()
			: base(new XMSTestSupport())
		{
		}

// The .NET CF does not have the ability to interrupt threads, so this test is impossible.
#if !NETCF
		[Test]
		public override void TestNoTimeoutConsumer(
			[Values(AcknowledgementMode.AutoAcknowledge, AcknowledgementMode.ClientAcknowledge,
				AcknowledgementMode.DupsOkAcknowledge, AcknowledgementMode.Transactional)]
			AcknowledgementMode ackMode)
		{
			base.TestNoTimeoutConsumer(ackMode);
		}

		[Test]
		public override void TestSyncReceiveConsumerClose(
			[Values(AcknowledgementMode.AutoAcknowledge, AcknowledgementMode.ClientAcknowledge,
				AcknowledgementMode.DupsOkAcknowledge, AcknowledgementMode.Transactional)]
			AcknowledgementMode ackMode)
		{
			base.TestSyncReceiveConsumerClose(ackMode);
		}

		[Test]
		public override void TestDoChangeSentMessage(
			[Values(AcknowledgementMode.AutoAcknowledge, AcknowledgementMode.ClientAcknowledge,
				AcknowledgementMode.DupsOkAcknowledge, AcknowledgementMode.Transactional)]
			AcknowledgementMode ackMode,
			[Values(true, false)] bool doClear)
		{
			base.TestDoChangeSentMessage(ackMode, doClear);
		}

		[Test]
		public override void TestConsumerReceiveBeforeMessageDispatched(
			[Values(AcknowledgementMode.AutoAcknowledge, AcknowledgementMode.ClientAcknowledge,
				AcknowledgementMode.DupsOkAcknowledge, AcknowledgementMode.Transactional)]
			AcknowledgementMode ackMode)
		{
			base.TestConsumerReceiveBeforeMessageDispatched(ackMode);
		}

		[Test]
		public void TestDontStart(
			[Values(MsgDeliveryMode.NonPersistent)]
			MsgDeliveryMode deliveryMode,
			[Values(DestinationType.Queue, DestinationType.Topic)]
			DestinationType destinationType)
		{
			base.TestDontStart(deliveryMode, destinationType,
				destinationType == DestinationType.Queue ? DEFAULT_TEST_QUEUE : DEFAULT_TEST_TOPIC);
		}

		[Test]
		public void TestSendReceiveTransacted(
			[Values(MsgDeliveryMode.NonPersistent, MsgDeliveryMode.Persistent)]
			MsgDeliveryMode deliveryMode,
			[Values(DestinationType.Queue, DestinationType.Topic, DestinationType.TemporaryQueue, DestinationType.TemporaryTopic)]
			DestinationType destinationType)
		{
			string testDestinationRef;
			switch(destinationType)
			{
			case DestinationType.Queue: testDestinationRef = DEFAULT_TEST_QUEUE; break;
			case DestinationType.Topic: testDestinationRef = DEFAULT_TEST_TOPIC; break;
			default:                    testDestinationRef = "";                 break;
			}

			base.TestSendReceiveTransacted(deliveryMode, destinationType, testDestinationRef);
		}

		[Test]
		public void TestAckedMessageAreConsumed()
		{
			base.TestAckedMessageAreConsumed(DEFAULT_TEST_QUEUE);
		}

		[Test]
		public void TestLastMessageAcked()
		{
			base.TestLastMessageAcked(DEFAULT_TEST_QUEUE);
		}

		[Test]
		public void TestUnAckedMessageAreNotConsumedOnSessionClose()
		{
			base.TestUnAckedMessageAreNotConsumedOnSessionClose(DEFAULT_TEST_QUEUE);
		}

		[Test]
		public void TestAsyncAckedMessageAreConsumed()
		{
			base.TestAsyncAckedMessageAreConsumed(DEFAULT_TEST_QUEUE);
		}

		[Test]
		public void TestAsyncUnAckedMessageAreNotConsumedOnSessionClose()
		{
			base.TestAsyncUnAckedMessageAreNotConsumedOnSessionClose(DEFAULT_TEST_QUEUE);
		}

		[Test]
		public override void TestAddRemoveAsnycMessageListener()
		{
			base.TestAddRemoveAsnycMessageListener();
		}

		[Test]
		public override void TestReceiveNoWait(
			[Values(AcknowledgementMode.AutoAcknowledge, AcknowledgementMode.ClientAcknowledge,
				AcknowledgementMode.DupsOkAcknowledge, AcknowledgementMode.Transactional)]
			AcknowledgementMode ackMode,
			[Values(MsgDeliveryMode.NonPersistent, MsgDeliveryMode.Persistent)]
			MsgDeliveryMode deliveryMode)
		{
			base.TestReceiveNoWait(ackMode, deliveryMode);
		}

#endif

    }
}
