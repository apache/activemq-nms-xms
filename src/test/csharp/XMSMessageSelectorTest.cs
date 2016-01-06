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
	[Category("LongRunning")]
	public class XMSMessageSelectorTest : MessageSelectorTest
	{
		protected const string SELECTOR_TEST_QUEUE = "messageSelectorTestQueue";
		protected const string SELECTOR_TEST_TOPIC = "messageSelectorTestTopic";

		public XMSMessageSelectorTest()
			: base(new XMSTestSupport())
		{
		}

		[Test]
		public override void TestFilterIgnoredMessages(
			[Values(SELECTOR_TEST_QUEUE, SELECTOR_TEST_TOPIC)]
			string testDestinationRef)
		{
			string testDestinationURI = GetDestinationURI(
				testDestinationRef == SELECTOR_TEST_QUEUE ? DestinationType.Queue : DestinationType.Topic,
				testDestinationRef);

			base.TestFilterIgnoredMessages(testDestinationURI);
		}

		[Test]
		public override void TestFilterIgnoredMessagesSlowConsumer(
			[Values(SELECTOR_TEST_QUEUE, SELECTOR_TEST_TOPIC)]
			string testDestinationRef)
		{
			string testDestinationURI = GetDestinationURI(
				testDestinationRef == SELECTOR_TEST_QUEUE ? DestinationType.Queue : DestinationType.Topic,
				testDestinationRef);

			base.TestFilterIgnoredMessagesSlowConsumer(testDestinationURI);
		}
	}
}
