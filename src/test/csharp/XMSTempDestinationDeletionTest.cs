﻿/*
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
	public class XMSTempDestinationDeletionTest : TempDestinationDeletionTest
	{
		protected const string DELETION_TEST_QUEUE = "deletionTestQueue";
		protected const string DELETION_TEST_TOPIC = "deletionTestTopic";
		protected const string DELETION_TEST_TEMP_QUEUE = "deletionTestTempQueue";
		protected const string DELETION_TEST_TEMP_TOPIC = "deletionTestTempTopic";

		public XMSTempDestinationDeletionTest()
			: base(new XMSTestSupport())
		{
		}

		[Test]
		public override void TestTempDestinationDeletion(
			[Values(MsgDeliveryMode.Persistent, MsgDeliveryMode.NonPersistent)]
			MsgDeliveryMode deliveryMode,
			[Values(DELETION_TEST_QUEUE, DELETION_TEST_TOPIC, DELETION_TEST_TEMP_QUEUE, DELETION_TEST_TEMP_TOPIC)]
			string testDestinationRef)
		{
            DestinationType type;
			if     (testDestinationRef == DELETION_TEST_QUEUE)      type = DestinationType.Queue;
			else if(testDestinationRef == DELETION_TEST_TOPIC)      type = DestinationType.Topic;
			else if(testDestinationRef == DELETION_TEST_TEMP_QUEUE) type = DestinationType.TemporaryQueue;
			else                                                    type = DestinationType.TemporaryTopic;

			string testDestinationURI = GetDestinationURI(type, testDestinationRef);

			base.TestTempDestinationDeletion(deliveryMode, testDestinationURI);
		}
	}
}
