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
using System.Text;
using System.Collections.Generic;
using IBM.XMS;

namespace Apache.NMS.XMS.Util
{
	class ExceptionUtil
	{
		/// <summary>
		/// Wrap the provider specific exception inside an NMS exception to
		/// more tightly integrate the provider extensions into the NMS API.
		/// </summary>
		/// <param name="ex">Original exception.</param>
		public static void WrapAndThrowNMSException(Exception ex)
		{
			if(ex is Apache.NMS.NMSException)
			{
				// Already derived from NMSException
				throw ex;
			}

			if(ex is IBM.XMS.IllegalStateException)
			{
				IBM.XMS.IllegalStateException xmsEx =
					(IBM.XMS.IllegalStateException)ex;
				throw new Apache.NMS.IllegalStateException(
					xmsEx.Message, xmsEx.ErrorCode, xmsEx);
			}

			if(ex is IBM.XMS.InvalidClientIDException)
			{
				IBM.XMS.InvalidClientIDException xmsEx =
					(IBM.XMS.InvalidClientIDException)ex;
				throw new Apache.NMS.InvalidClientIDException(
					xmsEx.Message, xmsEx.ErrorCode, xmsEx);
			}

			if(ex is IBM.XMS.InvalidDestinationException)
			{
				IBM.XMS.InvalidDestinationException xmsEx =
					(IBM.XMS.InvalidDestinationException)ex;
				throw new Apache.NMS.InvalidDestinationException(
					xmsEx.Message, xmsEx.ErrorCode, xmsEx);
			}

			if(ex is IBM.XMS.InvalidSelectorException)
			{
				IBM.XMS.InvalidSelectorException xmsEx =
					(IBM.XMS.InvalidSelectorException)ex;
				throw new Apache.NMS.InvalidSelectorException(
					xmsEx.Message, xmsEx.ErrorCode, xmsEx);
			}

			if(ex is IBM.XMS.MessageEOFException)
			{
				IBM.XMS.MessageEOFException xmsEx =
					(IBM.XMS.MessageEOFException)ex;
				throw new Apache.NMS.MessageEOFException(
					xmsEx.Message, xmsEx.ErrorCode, xmsEx);
			}

			if(ex is IBM.XMS.MessageFormatException)
			{
				IBM.XMS.MessageFormatException xmsEx =
					(IBM.XMS.MessageFormatException)ex;
				throw new Apache.NMS.MessageFormatException(
					xmsEx.Message, xmsEx.ErrorCode, xmsEx);
			}

			if(ex is IBM.XMS.MessageNotReadableException)
			{
				IBM.XMS.MessageNotReadableException xmsEx =
					(IBM.XMS.MessageNotReadableException)ex;
				throw new Apache.NMS.MessageNotReadableException(
					xmsEx.Message, xmsEx.ErrorCode, xmsEx);
			}

			if(ex is IBM.XMS.MessageNotWriteableException)
			{
				IBM.XMS.MessageNotWriteableException xmsEx =
					(IBM.XMS.MessageNotWriteableException)ex;
				throw new Apache.NMS.MessageNotWriteableException(
					xmsEx.Message, xmsEx.ErrorCode, xmsEx);
			}

			if(ex is IBM.XMS.ResourceAllocationException)
			{
				IBM.XMS.ResourceAllocationException xmsEx =
					(IBM.XMS.ResourceAllocationException)ex;
				throw new Apache.NMS.ResourceAllocationException(
					xmsEx.Message, xmsEx.ErrorCode, xmsEx);
			}

			if(ex is IBM.XMS.SecurityException)
			{
				IBM.XMS.SecurityException xmsEx =
					(IBM.XMS.SecurityException)ex;
				throw new Apache.NMS.NMSSecurityException(
					xmsEx.Message, xmsEx.ErrorCode, xmsEx);
			}

			if(ex is IBM.XMS.TransactionInProgressException)
			{
				IBM.XMS.TransactionInProgressException xmsEx =
					(IBM.XMS.TransactionInProgressException)ex;
				throw new Apache.NMS.TransactionInProgressException(
					xmsEx.Message, xmsEx.ErrorCode, xmsEx);
			}

			if(ex is IBM.XMS.TransactionRolledBackException)
			{
				IBM.XMS.TransactionRolledBackException xmsEx =
					(IBM.XMS.TransactionRolledBackException)ex;
				throw new Apache.NMS.TransactionRolledBackException(
					xmsEx.Message, xmsEx.ErrorCode, xmsEx);
			}

			if(ex is IBM.XMS.XMSException)
			{
				IBM.XMS.XMSException xmsEx =
					(IBM.XMS.XMSException)ex;
				throw new Apache.NMS.NMSException(
					xmsEx.Message, xmsEx.ErrorCode, xmsEx);
			}

			// Not an EMS exception that should be wrapped.
			throw ex;
		}
	}
}
