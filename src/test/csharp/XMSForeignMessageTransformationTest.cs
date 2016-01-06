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
using Apache.NMS.Util;
using Apache.NMS.Test;
using NUnit.Framework;

namespace Apache.NMS.XMS.Test
{
    [TestFixture]
    public class XMSForeignMessageTransformationTest : ForeignMessageTransformationTest
    {
		protected static string DEFAULT_TEST_TOPIC = "defaultTestTopic";

		public XMSForeignMessageTransformationTest()
			: base(new XMSTestSupport())
		{
		}

		[Test]
		public void TestSendReceiveForeignMessage(
			[Values(MsgDeliveryMode.Persistent, MsgDeliveryMode.NonPersistent)]
			MsgDeliveryMode deliveryMode)
		{
			base.TestSendReceiveForeignMessage(deliveryMode, DEFAULT_TEST_TOPIC);
		}

		[Test]
		public void TestSendReceiveForeignTextMessage(
			[Values(MsgDeliveryMode.Persistent, MsgDeliveryMode.NonPersistent)]
			MsgDeliveryMode deliveryMode)
		{
			base.TestSendReceiveForeignTextMessage(deliveryMode, DEFAULT_TEST_TOPIC);
		}

		[Test]
		public void TestSendReceiveForeignBytesMessage(
			[Values(MsgDeliveryMode.Persistent, MsgDeliveryMode.NonPersistent)]
			MsgDeliveryMode deliveryMode)
		{
			base.TestSendReceiveForeignBytesMessage(deliveryMode, DEFAULT_TEST_TOPIC);
		}

		[Test]
		public void TestSendReceiveForeignMapMessage(
			[Values(MsgDeliveryMode.Persistent, MsgDeliveryMode.NonPersistent)]
			MsgDeliveryMode deliveryMode)
		{
			base.TestSendReceiveForeignMapMessage(deliveryMode, DEFAULT_TEST_TOPIC);
		}

		[Test]
		public void TestSendReceiveForeignStreamMessage(
			[Values(MsgDeliveryMode.Persistent, MsgDeliveryMode.NonPersistent)]
			MsgDeliveryMode deliveryMode)
		{
			base.TestSendReceiveForeignStreamMessage(deliveryMode, DEFAULT_TEST_TOPIC);
		}
	}
}

