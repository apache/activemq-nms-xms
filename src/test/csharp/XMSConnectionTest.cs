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
using Apache.NMS.Test;
using NUnit.Framework;

namespace Apache.NMS.XMS.Test
{
	[TestFixture]
	public class XMSConnectionTest : ConnectionTest
	{
		protected static string DEFAULT_TEST_QUEUE = "defaultTestQueue";
		protected static string DEFAULT_TEST_TOPIC = "defaultTestTopic";

		public XMSConnectionTest()
			: base(new XMSTestSupport())
		{
		}

		[SetUp]
		public override void SetUp()
		{
			base.SetUp();
		}
	
		[TearDown]
		public override void TearDown()
		{
			base.TearDown();
		}
	
		/// <summary>
		/// Verify that it is possible to create multiple connections to the broker.
		/// There was a bug in the connection factory which set the clientId member which made
		/// it impossible to create an additional connection.
		/// </summary>
		[Test]
		public override void TestTwoConnections()
		{
			base.TestTwoConnections();
		}
	
		[Test]
		public void TestCreateAndDisposeWithConsumer(
			[Values(true, false)]
			bool disposeConsumer)
		{
			base.TestCreateAndDisposeWithConsumer(disposeConsumer, DEFAULT_TEST_QUEUE);
		}
	
		[Test]
		public void TestCreateAndDisposeWithProducer(
			[Values(true, false)]
			bool disposeProducer)
		{
			base.TestCreateAndDisposeWithProducer(disposeProducer, DEFAULT_TEST_QUEUE);
		}
	
		[Test]
		public void TestStartAfterSend(
			[Values(MsgDeliveryMode.Persistent, MsgDeliveryMode.NonPersistent)]
			MsgDeliveryMode deliveryMode,
			[Values(DestinationType.Queue, DestinationType.Topic)]
			DestinationType destinationType)
		{
			base.TestStartAfterSend(deliveryMode, destinationType,
				destinationType == DestinationType.Queue ? DEFAULT_TEST_QUEUE : DEFAULT_TEST_TOPIC);
		}

		/// <summary>
		/// Tests if the consumer receives the messages that were sent before the
		/// connection was started.
		/// </summary>
		[Test]
		public void TestStoppedConsumerHoldsMessagesTillStarted()
		{
			base.TestStoppedConsumerHoldsMessagesTillStarted(DEFAULT_TEST_TOPIC);
		}
	
		/// <summary>
		/// Tests if the consumer is able to receive messages eveb when the
		/// connecction restarts multiple times.
		/// </summary>
		[Test]
		public void TestMultipleConnectionStops()
		{
			base.TestMultipleConnectionStops(DEFAULT_TEST_TOPIC);
		}
	}
}
