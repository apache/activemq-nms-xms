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
using Apache.NMS.XMS.Util;
using IBM.XMS;

namespace Apache.NMS.XMS
{
	/// <summary>
	/// Represents a message either to be sent to a message broker or received
	/// from a message broker.
	/// </summary>
	class Message : Apache.NMS.IMessage
	{
		public IBM.XMS.IMessage xmsMessage;

		#region Constructors

		/// <summary>
		/// Constructs a <c>Message</c> object.
		/// </summary>
		/// <param name="message">XMS message.</param>
		public Message(IBM.XMS.IMessage message)
		{
			this.xmsMessage = message;
		}

		#endregion

		#region IMessage Members

		#region Acknowledgement

		/// <summary>
		/// If using client acknowledgement mode on the session then this
		/// method will acknowledge that the message has been processed
		/// correctly.
		/// </summary>
		public void Acknowledge()
		{
			try
			{
				this.xmsMessage.Acknowledge();
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		#endregion

		#region Message body

		private bool readOnlyMsgBody = false;
		/// <summary>
		/// Whether the message body is read-only.
		/// </summary>
		public virtual bool ReadOnlyBody
		{
			get { return this.readOnlyMsgBody; }
			set { this.readOnlyMsgBody = value; }
		}

		/// <summary>
		/// Clears out the message body. Clearing a message's body does not
		/// clear its header values or property entries.
		/// If this message body was read-only, calling this method leaves
		/// the message body in the same state as an empty body in a newly
		/// created message.
		/// </summary>
		public void ClearBody()
		{
			try
			{
				this.ReadOnlyBody = false;
				this.xmsMessage.ClearBody();
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		#endregion

		#region Message properties

		#region General comments

		// https://docs.oracle.com/cd/E19798-01/821-1841/bnces/index.html
		// A JMS message has three parts: a header, properties, and a body.
		// A JMS message header contains a number of predefined fields that
		// contain values that both clients and providers use to identify
		// and to route messages:
		//   Header Field     Set By 
		//   JMSDestination   send or publish method
		//   JMSDeliveryMode  send or publish method
		//   JMSExpiration    send or publish method
		//   JMSPriority      send or publish method
		//   JMSMessageID     send or publish method
		//   JMSTimestamp     send or publish method
		//   JMSCorrelationID Client 
		//   JMSReplyTo       Client 
		//   JMSType          Client
		//   JMSRedelivered   JMS provider 
		// Properties can be created and set for messages if values are needed
		// in addition to those provided by the header fields.
		// The JMS API provides some predefined property names that a provider
		// can support. The use either of these predefined properties or of
		// user-defined properties is optional.
		// The JMS API defines five message body formats:
		//   Message Type   Body Contains
		//   TextMessage    A java.lang.String object.
		//   MapMessage     A set of name-value pairs, with names as String
		//                  objects and values as primitive types.
		//   BytesMessage   A stream of uninterpreted bytes.
		//   StreamMessage  A stream of primitive values, filled and read
		//                  sequentially. 
		//   ObjectMessage  A Serializable object.
		//   Message        Nothing. Composed of header fields and properties
		//                  only.
		//
		// http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/xms_cmesmod.htm?lang=en
		// The XMS message model is the same as the WebSphere® MQ classes for
		// JMS message model.
		// In particular, XMS implements the same message header fields and
		// message properties that WebSphere MQ classes for JMS implements:
		//   JMS header fields.       These fields have names that commence
		//                            with the prefix JMS.
		//   JMS defined properties.  These fields have properties whose names
		//                            commence with the prefix JMSX.
		//   IBM® defined properties. These fields have properties whose names
		//                            commence with the prefix JMS_IBM_.
		//
		// Apache.NMS redefines JMS message header fields with an "NMS" prefix:
		//   JMS                           NMS                               IBM.XMS
		//   Destination JMSDestination    IDestination    NMSDestination    IDestination JMSDestination
		//   int         JMSDeliveryMode   MsgDeliveryMode NMSDeliveryMode   DeliveryMode JMSDeliveryMode
		//   long        JMSExpiration     [TimeSpan       NMSTimeToLive]    Int64        JMSExpiration
		//   int         JMSPriority       MsgPriority     NMSPriority       Int32        JMSPriority
		//   String      JMSMessageID      string          NMSMessageId      String       JMSMessageID
		//   long        JMSTimestamp      DateTime        NMSTimestamp      Int64        JMSTimestamp
		//   String      JMSCorrelationID  string          NMSCorrelationID  String       JMSCorrelationID
		//   Destination JMSReplyTo        IDestination    NMSReplyTo        IDestination JMSReplyTo
		//   String      JMSType           string          NMSType           String       JMSType
		//   boolean     JMSRedelivered    bool            NMSRedelivered    Boolean      JMSRedelivered
		// Properties are set and retrieved through typed SetXxxProperty and
		// GetXxxProperty methods.
		// Unlike JMS, Apache.NMS does not expose those methods in the
		// IMessage interface, but through the IPrimitiveMap interface,
		// implemented by the MessageProperties class, exposed through
		// the IMessage.Properties property.
		// The MessagePropertyIntercepter propertyHelper intercepts get and
		// set invocations on properties whose name starts with "NMS", and
		// maps them to the equivalent message header fields through
		// reflection. Other invocations are routed to the
		// MessageProperties.Get/SetObjetProperty methods, which in turn
		// invokes xmsMessage.Get/SetObjectProperty.
		//
		// XMS message properties are:
		//   XMSC.JMS_DESTINATION               = "JMSDestination"
		//   XMSC.JMS_DELIVERY_MODE             = "JMSDeliveryMode"
		//   XMSC.JMS_EXPIRATION                = "JMSExpiration"
		//   XMSC.JMS_PRIORITY                  = "JMSPriority"
		//   XMSC.JMS_MESSAGEID                 = "JMSMessageID"
		//   XMSC.JMS_TIMESTAMP                 = "JMSTimestamp"
		//   XMSC.JMS_CORRELATIONID             = "JMSCorrelationID"
		//   XMSC.JMS_REPLYTO                   = "JMSReplyto"
		//   XMSC.JMS_TYPE                      = "JMSType"
		//   XMSC.JMS_REDELIVERED               = "JMSRedelivered"
		//
		//   XMSC.JMSX_USERID                   = "JMSXUserID"
		//   XMSC.JMSX_APPID                    = "JMSXAppID"
		//   XMSC.JMSX_DELIVERY_COUNT           = "JMSXDeliveryCount"
		//   XMSC.JMSX_GROUPID                  = "JMSXGroupID"
		//   XMSC.JMSX_GROUPSEQ                 = "JMSXGroupSeq"
		//   XMSC.JMSX_STATE                    = "JMSXState"
		//   XMSC.JMSX_PRODUCER_TXID            = "JMSXProducerTXID"
		//   XMSC.JMSX_CONSUMER_TXID            = "JMSXConsumerTXID"
		//   XMSC.JMSX_RCV_TIMESTAMP            = "JMSXRcvTimestamp"

		//   XMSC.JMS_IBM_REPORT_EXCEPTION      = "JMS_IBM_Report_Exception"
		//   XMSC.JMS_IBM_REPORT_EXPIRATION     = "JMS_IBM_Report_Expiration"
		//   XMSC.JMS_IBM_REPORT_COA            = "JMS_IBM_Report_COA"
		//   XMSC.JMS_IBM_REPORT_COD            = "JMS_IBM_Report_COD"
		//   XMSC.JMS_IBM_REPORT_NAN            = "JMS_IBM_Report_NAN"
		//   XMSC.JMS_IBM_REPORT_PAN            = "JMS_IBM_Report_PAN"
		//   XMSC.JMS_IBM_REPORT_PASS_MSG_ID    = "JMS_IBM_Report_Pass_Msg_ID"
		//   XMSC.JMS_IBM_REPORT_PASS_CORREL_ID = "JMS_IBM_Report_Pass_Correl_ID"
		//   XMSC.JMS_IBM_REPORT_DISCARD_MSG    = "JMS_IBM_Report_Discard_Msg"
		//   XMSC.JMS_IBM_MSGTYPE               = "JMS_IBM_MsgType"
		//   XMSC.JMS_IBM_FEEDBACK              = "JMS_IBM_Feedback"
		//   XMSC.JMS_IBM_FORMAT                = "JMS_IBM_Format"
		//   XMSC.JMS_IBM_PUTAPPLTYPE           = "JMS_IBM_PutApplType"
		//   XMSC.JMS_IBM_ENCODING              = "JMS_IBM_Encoding"
		//   XMSC.JMS_IBM_CHARACTER_SET         = "JMS_IBM_Character_Set"
		//   XMSC.JMS_IBM_PUTDATE               = "JMS_IBM_PutDate"
		//   XMSC.JMS_IBM_PUTTIME               = "JMS_IBM_PutTime"
		//   XMSC.JMS_IBM_LAST_MSG_IN_GROUP     = "JMS_IBM_Last_Msg_In_Group"
		//   XMSC.JMS_IBM_EXCEPTIONREASON       = "JMS_IBM_ExceptionReason"
		//   XMSC.JMS_IBM_EXCEPTIONTIMESTAMP    = "JMS_IBM_ExceptionTimestamp"
		//   XMSC.JMS_IBM_EXCEPTIONMESSAGE      = "JMS_IBM_ExceptionMessage"
		//   XMSC.JMS_IBM_SYSTEM_MESSAGEID      = "JMS_IBM_System_MessageID"
		//   XMSC.JMS_IBM_EXCEPTIONPROBLEMDESTINATION = "JMS_IBM_ExceptionProblemDestination"
		//   XMSC.JMS_IBM_ARM_CORRELATOR        = "JMS_IBM_ArmCorrelator"
		//   XMSC.JMS_IBM_WAS_RM_CORRELATOR     = "JMS_IBM_RMCorrelator"
		//   XMSC.JMS_IBM_CONNECTIONID          = "JMS_IBM_ConnectionID"
		//   XMSC.JMS_IBM_RETAIN                = "JMS_IBM_Retain"
		//   XMSC.JMS_IBM_MQMD_REPORT           = "JMS_IBM_MQMD_Report"
		//   XMSC.JMS_IBM_MQMD_MSGTYPE          = "JMS_IBM_MQMD_MsgType"
		//   XMSC.JMS_IBM_MQMD_EXPIRY           = "JMS_IBM_MQMD_Expiry"
		//   XMSC.JMS_IBM_MQMD_FEEDBACK         = "JMS_IBM_MQMD_Feedback"
		//   XMSC.JMS_IBM_MQMD_ENCODING         = "JMS_IBM_MQMD_Encoding"
		//   XMSC.JMS_IBM_MQMD_CODEDCHARSETID   = "JMS_IBM_MQMD_CodedCharSetId"
		//   XMSC.JMS_IBM_MQMD_FORMAT           = "JMS_IBM_MQMD_Format"
		//   XMSC.JMS_IBM_MQMD_PRIORITY         = "JMS_IBM_MQMD_Priority"
		//   XMSC.JMS_IBM_MQMD_PERSISTENCE      = "JMS_IBM_MQMD_Persistence"
		//   XMSC.JMS_IBM_MQMD_MSGID            = "JMS_IBM_MQMD_MsgId"
		//   XMSC.JMS_IBM_MQMD_CORRELID         = "JMS_IBM_MQMD_CorrelId"
		//   XMSC.JMS_IBM_MQMD_BACKOUTCOUNT     = "JMS_IBM_MQMD_BackoutCount"
		//   XMSC.JMS_IBM_MQMD_REPLYTOQ         = "JMS_IBM_MQMD_ReplyToQ"
		//   XMSC.JMS_IBM_MQMD_REPLYTOQMGR      = "JMS_IBM_MQMD_ReplyToQMgr"
		//   XMSC.JMS_IBM_MQMD_USERIDENTIFIER   = "JMS_IBM_MQMD_UserIdentifier"
		//   XMSC.JMS_IBM_MQMD_ACCOUNTINGTOKEN  = "JMS_IBM_MQMD_AccountingToken"
		//   XMSC.JMS_IBM_MQMD_APPLIDENTITYDATA = "JMS_IBM_MQMD_ApplIdentityData"
		//   XMSC.JMS_IBM_MQMD_PUTAPPLTYPE      = "JMS_IBM_MQMD_PutApplType"
		//   XMSC.JMS_IBM_MQMD_PUTAPPLNAME      = "JMS_IBM_MQMD_PutApplName"
		//   XMSC.JMS_IBM_MQMD_PUTDATE          = "JMS_IBM_MQMD_PutDate"
		//   XMSC.JMS_IBM_MQMD_PUTTIME          = "JMS_IBM_MQMD_PutTime"
		//   XMSC.JMS_IBM_MQMD_APPLORIGINDATA   = "JMS_IBM_MQMD_ApplOriginData"
		//   XMSC.JMS_IBM_MQMD_GROUPID          = "JMS_IBM_MQMD_GroupId"
		//   XMSC.JMS_IBM_MQMD_MSGSEQNUMBER     = "JMS_IBM_MQMD_MsgSeqNumber"
		//   XMSC.JMS_IBM_MQMD_OFFSET           = "JMS_IBM_MQMD_Offset"
		//   XMSC.JMS_IBM_MQMD_MSGFLAGS         = "JMS_IBM_MQMD_MsgFlags"
		//   XMSC.JMS_IBM_MQMD_ORIGINALLENGTH   = "JMS_IBM_MQMD_OriginalLength"
		//   XMSC.JMS_TOG_ARM_CORRELATOR        = "JMS_TOG_ARM_Correlator"

		#endregion

		#region General methods

		private bool readOnlyMsgProperties = false;
		/// <summary>
		/// Whether the message properties is read-only.
		/// </summary>
		public virtual bool ReadOnlyProperties
		{
			get { return this.readOnlyMsgProperties; }

			set
			{
				if(this.propertyHelper != null)
				{
					this.propertyHelper.ReadOnly = value;
				}
				this.readOnlyMsgProperties = value;
			}
		}

		/// <summary>
		/// Clears a message's properties.
		/// The message's header fields and body are not cleared.
		/// </summary>
		public void ClearProperties()
		{
			try
			{
				this.ReadOnlyProperties = false;
				this.xmsMessage.ClearProperties();
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}

		private Apache.NMS.IPrimitiveMap properties = null;
		private Apache.NMS.Util.MessagePropertyIntercepter propertyHelper;
		/// <summary>
		/// Provides access to the message properties (headers)
		/// </summary>
		public Apache.NMS.IPrimitiveMap Properties
		{
			get
			{
				if(properties == null)
				{
					properties = XMSConvert.ToMessageProperties(this.xmsMessage);
					propertyHelper = new Apache.NMS.Util.MessagePropertyIntercepter(
						this, properties, this.ReadOnlyProperties);
				}

				return propertyHelper;
			}
		}

		#endregion

		#region Message header fields

		/// <summary>
		/// The correlation ID used to correlate messages from conversations
		/// or long running business processes.
		/// </summary>
		public string NMSCorrelationID
		{
			get
			{
				try
				{
					return this.xmsMessage.JMSCorrelationID;
				}
				catch(Exception ex)
				{
					ExceptionUtil.WrapAndThrowNMSException(ex);
					return null;
				}
			}
			set
			{
				try
				{
					this.xmsMessage.JMSCorrelationID = value;
				}
				catch(Exception ex)
				{
					ExceptionUtil.WrapAndThrowNMSException(ex);
				}
			}
		}

		/// <summary>
		/// The destination of the message.
		/// </summary>
		public Apache.NMS.IDestination NMSDestination
		{
			get
			{
				return XMSConvert.ToNMSDestination(
					this.xmsMessage.JMSDestination);
			}
			set
			{
				try
				{
					this.xmsMessage.JMSDestination =
						XMSConvert.ToXMSDestination(value);
				}
				catch(Exception ex)
				{
					ExceptionUtil.WrapAndThrowNMSException(ex);
				}
			}
		}

		protected TimeSpan timeToLive;
		/// <summary>
		/// The amount of time that this message is valid for.
		/// <c>null</c> if this message does not expire.
		/// </summary>
		public TimeSpan NMSTimeToLive
		{
			get { return this.timeToLive; }
			set { this.timeToLive = value; }
		}

		/// <summary>
		/// The message ID which is set by the provider.
		/// </summary>
		public string NMSMessageId
		{
			get { return this.xmsMessage.JMSMessageID; }
			set { this.xmsMessage.JMSMessageID = value; }
		}

		/// <summary>
		/// Whether or not this message is persistent.
		/// </summary>
		public MsgDeliveryMode NMSDeliveryMode
		{
			get
			{
				return XMSConvert.ToNMSMsgDeliveryMode(
					this.xmsMessage.JMSDeliveryMode);
			}
			set
			{
				try
				{
					this.xmsMessage.JMSDeliveryMode =
						XMSConvert.ToJMSDeliveryMode(value);
				}
				catch(Exception ex)
				{
					ExceptionUtil.WrapAndThrowNMSException(ex);
				}
			}
		}

		/// <summary>
		/// The Priority on this message.
		/// </summary>
		public MsgPriority NMSPriority
		{
			get
			{
				return (MsgPriority)this.xmsMessage.JMSPriority;
			}
			set
			{
				try
				{
					this.xmsMessage.JMSPriority = (int)value;
				}
				catch(Exception ex)
				{
					ExceptionUtil.WrapAndThrowNMSException(ex);
				}
			}
		}

		/// <summary>
		/// Returns true if this message has been redelivered to this or
		/// another consumer before being acknowledged successfully.
		/// </summary>
		public bool NMSRedelivered
		{
			get { return this.xmsMessage.JMSRedelivered; }
			set
			{
				throw new NMSException("JMSRedelivered cannot be set.");
			}
		}

		/// <summary>
		/// The destination that the consumer of this message should send
		/// replies to.
		/// </summary>
		public Apache.NMS.IDestination NMSReplyTo
		{
			get
			{
				return XMSConvert.ToNMSDestination(
					this.xmsMessage.JMSReplyTo);
			}
			set
			{
				try
				{
					this.xmsMessage.JMSReplyTo =
						XMSConvert.ToXMSDestination(value);
				}
				catch(Exception ex)
				{
					ExceptionUtil.WrapAndThrowNMSException(ex);
				}
			}
		}

		/// <summary>
		/// The timestamp of when the message was pubished in UTC time. If the
		/// publisher disables setting the timestamp on the message, the time
		/// will be set to the start of the UNIX epoch (1970-01-01 00:00:00).
		/// </summary>
		public DateTime NMSTimestamp
		{
			get { return DateUtils.ToDateTime(this.xmsMessage.JMSTimestamp); }
			set { this.xmsMessage.JMSTimestamp = DateUtils.ToJavaTime(value); }
		}

		/// <summary>
		/// The type name of this message.
		/// </summary>
		public string NMSType
		{
			get { return this.xmsMessage.JMSType; }
			set
			{
				try
				{
					this.xmsMessage.JMSType = value;
				}
				catch(Exception ex)
				{
					ExceptionUtil.WrapAndThrowNMSException(ex);
				}
			}
		}

		#endregion

		#endregion

		#endregion

		#region Event handlers

		public virtual void OnSend()
		{
			this.ReadOnlyProperties = true;
			this.ReadOnlyBody = true;
		}

		public virtual void OnMessageRollback()
		{
		}

		#endregion
	}
}
