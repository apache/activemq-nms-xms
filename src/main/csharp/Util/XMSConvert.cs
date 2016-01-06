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
using System.Collections;
using IBM.XMS;

namespace Apache.NMS.XMS.Util
{
	/// <summary>
	/// This class implements conversion methods between the IBM XMS and
	/// the Apache NMS models.
	/// </summary>
	public class XMSConvert
	{
		#region IBM XMS to Apache NMS objects conversion

		/// <summary>
		/// Converts an IBM XMS connection interface
		/// into an NMS connection interface.
		/// </summary>
		/// <param name="xmsConnection">IBM XMS connection interface.</param>
		/// <returns>Apache NMS connection interface.</returns>
		public static Apache.NMS.IConnection ToNMSConnection(
			IBM.XMS.IConnection xmsConnection)
		{
			return (xmsConnection != null
				? new Apache.NMS.XMS.Connection(xmsConnection)
				: null);
		}

		/// <summary>
		/// Converts an IBM XMS session interface
		/// into an NMS session interface.
		/// </summary>
		/// <param name="xmsSession">IBM XMS session interface.</param>
		/// <returns>Apache NMS session interface.</returns>
		public static Apache.NMS.ISession ToNMSSession(
			IBM.XMS.ISession xmsSession)
		{
			return (xmsSession != null
				? new Apache.NMS.XMS.Session(xmsSession)
				: null);
		}

		/// <summary>
		/// Converts an IBM XMS destination interface
		/// into an NMS destination interface.
		/// </summary>
		/// <param name="xmsDestination">XMS destination.</param>
		/// <returns>Apache NMS destination interface.</returns>
		public static Apache.NMS.IDestination ToNMSDestination(
				IBM.XMS.IDestination xmsDestination)
		{
			return ToNMSDestination(xmsDestination, false);
		}

		/// <summary>
		/// Converts an IBM XMS destination interface
		/// into an NMS destination interface.
		/// </summary>
		/// <param name="xmsDestination">XMS destination.</param>
		/// <param name="isTemporary">Destination is temporary.</param>
		/// <returns>Apache NMS destination interface.</returns>
		public static Apache.NMS.IDestination ToNMSDestination(
				IBM.XMS.IDestination xmsDestination,
				bool isTemporary)
		{
			if(xmsDestination.TypeId == IBM.XMS.DestinationType.Queue)
			{
				return (isTemporary ? ToNMSTemporaryQueue(xmsDestination)
									: ToNMSQueue(xmsDestination));
			}

			if(xmsDestination.TypeId == IBM.XMS.DestinationType.Topic)
			{
				return (isTemporary ? ToNMSTemporaryTopic(xmsDestination)
									: ToNMSTopic(xmsDestination));
			}

			return null;
		}

		/// <summary>
		/// Converts an IBM XMS queue interface
		/// into an NMS queue interface.
		/// </summary>
		/// <param name="xmsQueue">XMS destination of type
		/// <c>DestinationType.Queue</c>.</param>
		/// <returns>Apache NMS queue interface.</returns>
		public static Apache.NMS.IQueue ToNMSQueue(
			IBM.XMS.IDestination xmsQueue)
		{
			if((xmsQueue != null) &&
			(xmsQueue.TypeId != IBM.XMS.DestinationType.Queue))
			{ throw new ArgumentException(
				"Cannot convert IBM XMS destination to NMS destination: invalid destination type id.",
				"xmsQueue");
			}
			return (xmsQueue != null
				? new Apache.NMS.XMS.Queue(xmsQueue)
				: null);
		}

		/// <summary>
		/// Converts an IBM XMS topic interface
		/// into an NMS topic interface.
		/// </summary>
		/// <param name="xmsTopic">XMS destination of type
		/// <c>DestinationType.Topic</c>.</param>
		/// <returns>Apache NMS topic interface.</returns>
		public static Apache.NMS.ITopic ToNMSTopic(
			IBM.XMS.IDestination xmsTopic)
		{
			if((xmsTopic != null) &&
			(xmsTopic.TypeId != IBM.XMS.DestinationType.Topic))
			{ throw new ArgumentException(
				"Cannot convert IBM XMS destination to NMS destination: invalid destination type id.",
				"xmsTopic");
			}
			return (xmsTopic != null
				? new Apache.NMS.XMS.Topic(xmsTopic)
				: null);
		}

		/// <summary>
		/// Converts an IBM XMS temporary queue interface
		/// into an NMS temporary queue interface.
		/// </summary>
		/// <param name="xmsTemporaryQueue">XMS destination of type
		/// <c>DestinationType.Queue</c>.</param>
		/// <returns>Apache NMS temporary queue interface.</returns>
		// Couldn't find a means to test whether a XMS destination is temporary.
		public static Apache.NMS.ITemporaryQueue ToNMSTemporaryQueue(
			IBM.XMS.IDestination xmsTemporaryQueue)
		{
			if((xmsTemporaryQueue != null) &&
			(xmsTemporaryQueue.TypeId != IBM.XMS.DestinationType.Queue))
			{ throw new ArgumentException(
				"Cannot convert IBM XMS destination to NMS destination: invalid destination type id.",
				"xmsTemporaryQueue");
			}
			return (xmsTemporaryQueue != null
				? new Apache.NMS.XMS.TemporaryQueue(xmsTemporaryQueue)
				: null);
		}

		/// <summary>
		/// Converts an IBM XMS temporary topic interface
		/// into an NMS temporary topic interface.
		/// </summary>
		/// <param name="xmsTemporaryTopic">XMS destination of type
		/// <c>DestinationType.Topic</c>.</param>
		/// <returns>Apache NMS temporary topic interface.</returns>
		// Couldn't find a means to test whether a XMS destination is temporary.
		public static Apache.NMS.ITemporaryTopic ToNMSTemporaryTopic(
			IBM.XMS.IDestination xmsTemporaryTopic)
		{
			if((xmsTemporaryTopic != null) &&
			(xmsTemporaryTopic.TypeId != IBM.XMS.DestinationType.Queue))
			{ throw new ArgumentException(
				"Cannot convert IBM XMS destination to NMS destination: invalid destination type id.",
				"xmsTemporaryTopic");
			}
			return (xmsTemporaryTopic != null
				? new Apache.NMS.XMS.TemporaryTopic(xmsTemporaryTopic)
				: null);
		}

		/// <summary>
		/// Converts an IBM XMS message producer interface
		/// into an NMS message producer interface.
		/// </summary>
		/// <param name="session">NMS session.</param>
		/// <param name="xmsMessageProducer">XMS message producer.</param>
		/// <returns>Apache NMS message producer interface.</returns>
		public static Apache.NMS.IMessageProducer ToNMSMessageProducer(
			Apache.NMS.XMS.Session session,
			IBM.XMS.IMessageProducer xmsMessageProducer)
		{
			return (xmsMessageProducer != null
				? new Apache.NMS.XMS.MessageProducer(session, xmsMessageProducer)
				: null);
		}

		/// <summary>
		/// Converts an IBM XMS message consumer interface
		/// into an NMS message consumer interface.
		/// </summary>
		/// <param name="session">NMS session.</param>
		/// <param name="xmsMessageConsumer">XMS message consumer.</param>
		/// <returns>Apache NMS message consumer interface.</returns>
		public static Apache.NMS.IMessageConsumer ToNMSMessageConsumer(
			Apache.NMS.XMS.Session session,
			IBM.XMS.IMessageConsumer xmsMessageConsumer)
		{
			return (xmsMessageConsumer != null
				? new Apache.NMS.XMS.MessageConsumer(session, xmsMessageConsumer)
				: null);
		}

		/// <summary>
		/// Converts an IBM XMS queue browser interface
		/// into an NMS queue browser interface.
		/// </summary>
		/// <param name="xmsQueueBrowser">XMS queue browser.</param>
		/// <returns>Apache NMS queue browser interface.</returns>
		public static Apache.NMS.IQueueBrowser ToNMSQueueBrowser(
			IBM.XMS.IQueueBrowser xmsQueueBrowser)
		{
			return (xmsQueueBrowser != null
				? new Apache.NMS.XMS.QueueBrowser(xmsQueueBrowser)
				: null);
		}

		/// <summary>
		/// Converts an IBM XMS message
		/// into an NMS message.
		/// </summary>
		/// <param name="xmsMessage">IBM XMS message.</param>
		/// <returns>NMS message.</returns>
		public static Apache.NMS.IMessage ToNMSMessage(IBM.XMS.IMessage xmsMessage)
		{
			if(xmsMessage is IBM.XMS.ITextMessage)
			{
				return ToNMSTextMessage((IBM.XMS.ITextMessage)xmsMessage);
			}

			if(xmsMessage is IBM.XMS.IBytesMessage)
			{
				return ToNMSBytesMessage((IBM.XMS.IBytesMessage)xmsMessage);
			}

			if(xmsMessage is IBM.XMS.IStreamMessage)
			{
				return ToNMSStreamMessage((IBM.XMS.IStreamMessage)xmsMessage);
			}

			if(xmsMessage is IBM.XMS.IMapMessage)
			{
				return ToNMSMapMessage((IBM.XMS.IMapMessage)xmsMessage);
			}

			if(xmsMessage is IBM.XMS.IObjectMessage)
			{
				return ToNMSObjectMessage((IBM.XMS.IObjectMessage)xmsMessage);
			}

			return (xmsMessage != null
				? new Apache.NMS.XMS.Message(xmsMessage)
				: null);
		}

		/// <summary>
		/// Converts an IBM XMS text message
		/// into an NMS text message.
		/// </summary>
		/// <param name="xmsTextMessage">IBM XMS text message.</param>
		/// <returns>NMS text message.</returns>
		public static Apache.NMS.ITextMessage ToNMSTextMessage(
			IBM.XMS.ITextMessage xmsTextMessage)
		{
			return (xmsTextMessage != null
				? new Apache.NMS.XMS.TextMessage(xmsTextMessage)
				: null);
		}

		/// <summary>
		/// Converts an IBM XMS bytes message
		/// into an NMS bytes message.
		/// </summary>
		/// <param name="xmsBytesMessage">IBM XMS bytes message.</param>
		/// <returns>NMS bytes message.</returns>
		public static Apache.NMS.IBytesMessage ToNMSBytesMessage(
			IBM.XMS.IBytesMessage xmsBytesMessage)
		{
			return (xmsBytesMessage != null
				? new Apache.NMS.XMS.BytesMessage(xmsBytesMessage)
				: null);
		}

		/// <summary>
		/// Converts an IBM XMS stream message
		/// into an NMS stream message.
		/// </summary>
		/// <param name="xmsStreamMessage">IBM XMS stream message.</param>
		/// <returns>NMS stream message.</returns>
		public static Apache.NMS.IStreamMessage ToNMSStreamMessage(
			IBM.XMS.IStreamMessage xmsStreamMessage)
		{
			return (xmsStreamMessage != null
				? new Apache.NMS.XMS.StreamMessage(xmsStreamMessage)
				: null);
		}

		/// <summary>
		/// Converts an IBM XMS map message
		/// into an NMS map message.
		/// </summary>
		/// <param name="xmsMapMessage">IBM XMS map message.</param>
		/// <returns>NMS map message.</returns>
		public static Apache.NMS.IMapMessage ToNMSMapMessage(
			IBM.XMS.IMapMessage xmsMapMessage)
		{
			return (xmsMapMessage != null
				? new Apache.NMS.XMS.MapMessage(xmsMapMessage)
				: null);
		}

		/// <summary>
		/// Converts an IBM XMS object message
		/// into an NMS object message.
		/// </summary>
		/// <param name="xmsObjectMessage">IBM XMS object message.</param>
		/// <returns>NMS object message.</returns>
		public static Apache.NMS.IObjectMessage ToNMSObjectMessage(
			IBM.XMS.IObjectMessage xmsObjectMessage)
		{
			return (xmsObjectMessage != null
				? new Apache.NMS.XMS.ObjectMessage(xmsObjectMessage)
				: null);
		}

		/// <summary>
		/// Converts an IBM XMS message
		/// into an NMS primitive map.
		/// </summary>
		/// <param name="xmsMessage">IBM XMS message.</param>
		/// <returns>NMS primitive map.</returns>
		public static Apache.NMS.IPrimitiveMap ToMessageProperties(
			IBM.XMS.IMessage xmsMessage)
		{
			return (xmsMessage != null
				? new Apache.NMS.XMS.MessageProperties(xmsMessage)
				: null);
		}

		#endregion

		#region Apache NMS to IBM XMS objects conversion

		/// <summary>
		/// Converts an NMS destination
		/// into an IBM XMS destination.
		/// </summary>
		/// <param name="nmsDestination">NMS destination.</param>
		/// <returns>IBM XMS destination.</returns>
		public static IBM.XMS.IDestination ToXMSDestination(
			Apache.NMS.IDestination nmsDestination)
		{
			if(nmsDestination is Apache.NMS.XMS.Destination)
			{
				return ((Apache.NMS.XMS.Destination)nmsDestination).xmsDestination;
			}
			return null;
		}

		#endregion

		#region Property values conversion

		#region Exception handling

		/// <summary>
		/// Throws a conversion exception.
		/// </summary>
		private static void ThrowCantConvertValueException(object value,
					string conversionMethod)
		{
			throw new ArgumentException(string.Format(
				"Cannot convert {0} using {1}.", value, conversionMethod),
				conversionMethod);
		}

		#endregion

		#region Encoding

		/// <summary>
		/// Converts an XMS encoding key.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Encoding ToEncoding(Int32 inputValue)
		{
			return (Encoding)inputValue;
		}

		/// <summary>
		/// Converts an encoding to the equivalent XMS value.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Int32 ToXMSEncoding(Encoding inputValue)
		{
			return (Int32)inputValue;
		}

		#endregion

		#region Message Type

		/// <summary>
		/// Converts an XMS message type key.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static MessageType ToMessageType(Int32 inputValue)
		{
			return (MessageType)inputValue;
		}

		/// <summary>
		/// Converts a message type to the equivalent XMS value.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Int32 ToXMSMessageType(MessageType inputValue)
		{
			return (Int32)inputValue;
		}

		#endregion

		#region Report Confirm On Arrival

		/// <summary>
		/// Converts an XMS "confirm on arrival" key.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static ReportConfirmOnArrival ToReportConfirmOnArrival(
			Int32 inputValue)
		{
			return (ReportConfirmOnArrival)inputValue;
		}

		/// <summary>
		/// Converts a "confirm on arrival" to the equivalent XMS value.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Int32 ToXMSReportConfirmOnArrival(
			ReportConfirmOnArrival inputValue)
		{
			return (Int32)inputValue;
		}

		#endregion

		#region Report Confirm On Delivery

		/// <summary>
		/// Converts an XMS "confirm on delivery" key.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static ReportConfirmOnDelivery ToReportConfirmOnDelivery(
			Int32 inputValue)
		{
			return (ReportConfirmOnDelivery)inputValue;
		}

		/// <summary>
		/// Converts a "confirm on delivery" to the equivalent XMS value.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Int32 ToXMSReportConfirmOnDelivery(
			ReportConfirmOnDelivery inputValue)
		{
			return (Int32)inputValue;
		}

		#endregion

		#region Report Exception

		/// <summary>
		/// Converts an XMS "report exceptions" key.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static ReportExceptions ToReportExceptions(
			Int32 inputValue)
		{
			return (ReportExceptions)inputValue;
		}

		/// <summary>
		/// Converts a "report exceptions" to the equivalent XMS value.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Int32 ToXMSReportExceptions(
			ReportExceptions inputValue)
		{
			return (Int32)inputValue;
		}

		#endregion

		#region Report Expiration

		/// <summary>
		/// Converts an XMS "report expiration" key.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static ReportExpiration ToReportExpiration(
			Int32 inputValue)
		{
			return (ReportExpiration)inputValue;
		}

		/// <summary>
		/// Converts a "report expiration" to the equivalent XMS value.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Int32 ToXMSReportExpiration(
			ReportExpiration inputValue)
		{
			return (Int32)inputValue;
		}

		#endregion

		#region Report Correlation Id

		/// <summary>
		/// Converts an XMS "report correlation id." key.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static ReportCorrelationId ToReportCorrelationId(
			Int32 inputValue)
		{
			return (ReportCorrelationId)inputValue;
		}

		/// <summary>
		/// Converts a "report correlation id." to the equivalent XMS value.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Int32 ToXMSReportCorrelationId(
			ReportCorrelationId inputValue)
		{
			return (Int32)inputValue;
		}

		#endregion

		#region Report Message Id

		/// <summary>
		/// Converts an XMS "report message id." key.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static ReportMessageId ToReportMessageId(
			Int32 inputValue)
		{
			return (ReportMessageId)inputValue;
		}

		/// <summary>
		/// Converts a "report message id." to the equivalent XMS value.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Int32 ToXMSReportMessageId(
			ReportMessageId inputValue)
		{
			return (Int32)inputValue;
		}

		#endregion

		#region Asynchronous Exceptions

		/// <summary>
		/// Converts an XMS asynchronous exceptions handling directive key.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static AsynchronousExceptions ToAsynchronousExceptions(
			Int32 inputValue)
		{
			return (AsynchronousExceptions)inputValue;
		}

		/// <summary>
		/// Converts an asynchronous exceptions handling directive to the
		/// equivalent XMS value.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Int32 ToXMSAsynchronousExceptions(
			AsynchronousExceptions inputValue)
		{
			return (Int32)inputValue;
		}

		#endregion

		#region Connection Type

		/// <summary>
		/// Converts an XMS connection type key.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static ConnectionType ToConnectionType(Int32 inputValue)
		{
			return (ConnectionType)inputValue;
		}

		/// <summary>
		/// Converts a connection type to the equivalent XMS value.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Int32 ToXMSConnectionType(ConnectionType inputValue)
		{
			return (Int32)inputValue;
		}

		#endregion

		#region Delivery Mode

		/// <summary>
		/// Converts an XMS delivery mode key.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static DeliveryMode ToDeliveryMode(Int32 inputValue)
		{
			return (DeliveryMode)inputValue;
		}

		/// <summary>
		/// Converts a delivery mode to the equivalent XMS value.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Int32 ToXMSDeliveryMode(DeliveryMode inputValue)
		{
			return (Int32)inputValue;
		}

		#endregion

		#region Priority

		/// <summary>
		/// Converts an XMS priority key.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Priority ToPriority(Int32 inputValue)
		{
			return (Priority)inputValue;
		}

		/// <summary>
		/// Converts a priority to the equivalent XMS value.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Int32 ToXMSPriority(Priority inputValue)
		{
			return (Int32)inputValue;
		}

		#endregion

		#region Connection Protocol (RTT)

		/// <summary>
		/// Converts an RTT connection protocol key.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static RTTConnectionProtocol ToRTTConnectionProtocol(
			Int32 inputValue)
		{
			return (RTTConnectionProtocol)inputValue;
		}

		/// <summary>
		/// Converts an RTT connection protocol to the equivalent XMS value.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Int32 ToXMSRTTConnectionProtocol(
			RTTConnectionProtocol inputValue)
		{
			return (Int32)inputValue;
		}

		#endregion

		#region Multicast (RTT)

		/// <summary>
		/// Converts an RTT multicast state key.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Multicast ToMulticast(Int32 inputValue)
		{
			return (Multicast)inputValue;
		}

		/// <summary>
		/// Converts a multicast state to the equivalent XMS value.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Int32 ToXMSMulticast(Multicast inputValue)
		{
			return (Int32)inputValue;
		}

		#endregion

		#region Broker Version (WMQ)

		/// <summary>
		/// Converts a WMQ broker version key.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static BrokerVersion ToBrokerVersion(Int32 inputValue)
		{
			return (BrokerVersion)inputValue;
		}

		/// <summary>
		/// Converts a broker version to the equivalent XMS value.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Int32 ToXMSBrokerVersion(BrokerVersion inputValue)
		{
			return (Int32)inputValue;
		}

		#endregion

		#region Reconnect Options (WMQ)

		/// <summary>
		/// Converts a WMQ reconnect option key.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static ReconnectOptions ToReconnectOptions(Int32 inputValue)
		{
			return (ReconnectOptions)inputValue;
		}

		/// <summary>
		/// Converts a reconnect option to the equivalent XMS value.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Int32 ToXMSReconnectOptions(ReconnectOptions inputValue)
		{
			return (Int32)inputValue;
		}

		#endregion

		#region Connection Mode (WMQ)

		/// <summary>
		/// Converts a WMQ connection mode key.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static ConnectionMode ToConnectionMode(Int32 inputValue)
		{
			return (ConnectionMode)inputValue;
		}

		/// <summary>
		/// Converts a connection mode to the equivalent XMS value.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Int32 ToXMSConnectionMode(ConnectionMode inputValue)
		{
			return (Int32)inputValue;
		}

		#endregion

		#region Fail If Quiesce (WMQ)

		/// <summary>
		/// Converts a WMQ yes/no key to the equivalent boolean.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static bool ToFailIfQuiesce(Int32 inputValue)
		{
			switch(inputValue)
			{
				case XMSC.WMQ_FIQ_YES: return true;
				case XMSC.WMQ_FIQ_NO : return false;
				default:
					ThrowCantConvertValueException(inputValue, "ToFailIfQuiesce");
					return false;

			}
		}

		/// <summary>
		/// Converts a WMQ boolean to the equivalent XMS yes/no value.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Int32 ToXMSFailIfQuiesce(bool inputValue)
		{
			return inputValue ? XMSC.WMQ_FIQ_YES : XMSC.WMQ_FIQ_NO;
		}

		#endregion

		#region Message Body (WMQ)

		/// <summary>
		/// Converts a WMQ message body key.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static MessageBody ToMessageBody(Int32 inputValue)
		{
			return (MessageBody)inputValue;
		}

		/// <summary>
		/// Converts a message body to the equivalent XMS value.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Int32 ToXMSMessageBody(MessageBody inputValue)
		{
			return (Int32)inputValue;
		}

		#endregion

		#region Message Context (WMQ)

		/// <summary>
		/// Converts a WMQ message context key.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static MessageContext ToMessageContext(Int32 inputValue)
		{
			return (MessageContext)inputValue;
		}

		/// <summary>
		/// Converts a message context to the equivalent XMS value.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Int32 ToXMSMessageContext(MessageContext inputValue)
		{
			return (Int32)inputValue;
		}

		#endregion

		#region MQMD Read Enabled (WMQ)

		/// <summary>
		/// Converts a WMQ yes/no key to the equivalent boolean.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static bool ToMQMDReadEnabled(Int32 inputValue)
		{
			return (inputValue != 0);
			//switch(inputValue)
			//{
			//case XMSC.WMQ_READ_ENABLED_YES: return true;
			//case XMSC.WMQ_READ_ENABLED_NO : return false;
			//default:
			//	ThrowCantConvertValueException(inputValue, "ToMQMDReadEnabled");
			//	return false;
			//}
		}

		/// <summary>
		/// Converts a WMQ boolean to the equivalent XMS yes/no value.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Int32 ToXMSMQMDReadEnabled(bool inputValue)
		{
			return inputValue ? 1 : 0;
			//	XMSC.WMQ_READ_ENABLED_YES : XMSC.WMQ_READ_ENABLED_NO;
		}

		#endregion

		#region MQMD Write Enabled (WMQ)

		/// <summary>
		/// Converts a WMQ yes/no key to the equivalent boolean.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static bool ToMQMDWriteEnabled(Int32 inputValue)
		{
			return (inputValue != 0);
			//switch(inputValue)
			//{
			//case XMSC.WMQ_WRITE_ENABLED_YES: return true;
			//case XMSC.WMQ_WRITE_ENABLED_NO : return false;
			//default:
			//	ThrowCantConvertValueException(inputValue, "ToMQMDWriteEnabled");
			//	return false;
			//}
		}

		/// <summary>
		/// Converts a WMQ boolean to the equivalent XMS yes/no value.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Int32 ToXMSMQMDWriteEnabled(bool inputValue)
		{
			return inputValue ? 1 : 0;
			//	XMSC.WMQ_WRITE_ENABLED_YES : XMSC.WMQ_WRITE_ENABLED_NO;
		}

		#endregion

		#region Asynchronous Puts Allowed (WMQ)

		/// <summary>
		/// Converts a WMQ asynchronous puts allowed key.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static AsynchronousPutsAllowed ToAsynchronousPutsAllowed(
			Int32 inputValue)
		{
			return (AsynchronousPutsAllowed)inputValue;
		}

		/// <summary>
		/// Converts a WMQ asynchronous puts allowed to the equivalent
		/// XMS value.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Int32 ToXMSAsynchronousPutsAllowed(
			AsynchronousPutsAllowed inputValue)
		{
			return (Int32)inputValue;
		}

		#endregion

		#region Read Ahead Allowed (WMQ)

		/// <summary>
		/// Converts a WMQ read ahead allowed key.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static ReadAheadAllowed ToReadAheadAllowed(
			Int32 inputValue)
		{
			return (ReadAheadAllowed)inputValue;
		}

		/// <summary>
		/// Converts a WMQ read ahead allowed to the equivalent
		/// XMS value.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Int32 ToXMSReadAheadAllowed(
			ReadAheadAllowed inputValue)
		{
			return (Int32)inputValue;
		}

		#endregion

		#region Read Ahead Close Policy (WMQ)

		/// <summary>
		/// Converts a WMQ read ahead close policy key.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static ReadAheadClosePolicy ToReadAheadClosePolicy(
			Int32 inputValue)
		{
			return (ReadAheadClosePolicy)inputValue;
		}

		/// <summary>
		/// Converts a WMQ read ahead close policy to the equivalent
		/// XMS value.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Int32 ToXMSReadAheadClosePolicy(
			ReadAheadClosePolicy inputValue)
		{
			return (Int32)inputValue;
		}

		#endregion

		#region Message Selection (WMQ)

		/// <summary>
		/// Converts a WMQ message selection key.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static MessageSelection ToMessageSelection(Int32 inputValue)
		{
			return (MessageSelection)inputValue;
		}

		/// <summary>
		/// Converts a WMQ message selection to the equivalent XMS value.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Int32 ToXMSMessageSelection(MessageSelection inputValue)
		{
			return (Int32)inputValue;
		}

		#endregion

		#region Receive Conversion (WMQ)

		/// <summary>
		/// Converts a WMQ receive conversion key.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static ReceiveConversion ToReceiveConversion(Int32 inputValue)
		{
			return (ReceiveConversion)inputValue;
		}

		/// <summary>
		/// Converts a WMQ receive conversion to the equivalent XMS value.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Int32 ToXMSReceiveConversion(ReceiveConversion inputValue)
		{
			return (Int32)inputValue;
		}

		#endregion

		#region Share Socket Allowed (WMQ)

		/// <summary>
		/// Converts a WMQ yes/no key to the equivalent boolean.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static bool ToShareSocketAllowed(Int32 inputValue)
		{
			switch(inputValue)
			{
			case XMSC.WMQ_SHARE_CONV_ALLOWED_YES: return true;
			case XMSC.WMQ_SHARE_CONV_ALLOWED_NO : return false;
			default:
				ThrowCantConvertValueException(inputValue, "ShareSocketAllowed");
				return false;
			}
		}

		/// <summary>
		/// Converts a WMQ boolean to the equivalent XMS yes/no value.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Int32 ToXMSShareSocketAllowed(bool inputValue)
		{
			return inputValue
				? XMSC.WMQ_SHARE_CONV_ALLOWED_YES
				: XMSC.WMQ_SHARE_CONV_ALLOWED_NO;
		}

		#endregion

		#region Target Client (WMQ)

		/// <summary>
		/// Converts a WMQ target client key.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static TargetClient ToTargetClient(Int32 inputValue)
		{
			return (TargetClient)inputValue;
		}

		/// <summary>
		/// Converts a WMQ target client to the equivalent XMS value.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Int32 ToXMSTargetClient(TargetClient inputValue)
		{
			return (Int32)inputValue;
		}

		#endregion

		#region Wildcard Format (WMQ)

		/// <summary>
		/// Converts a WMQ wildcard format key.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static WildcardFormat ToWildcardFormat(Int32 inputValue)
		{
			return (WildcardFormat)inputValue;
		}

		/// <summary>
		/// Converts a WMQ wildcard format to the equivalent XMS value.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Int32 ToXMSWildcardFormat(WildcardFormat inputValue)
		{
			return (Int32)inputValue;
		}

		#endregion

		#region Connection Protocol (WPM)

		/// <summary>
		/// Converts a WPM connection protocol key.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static WPMConnectionProtocol ToWPMConnectionProtocol(
			Int32 inputValue)
		{
			return (WPMConnectionProtocol)inputValue;
		}

		/// <summary>
		/// Converts a WPM connection protocol to the equivalent XMS value.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Int32 ToXMSWPMConnectionProtocol(
			WPMConnectionProtocol inputValue)
		{
			return (Int32)inputValue;
		}

		#endregion

		#region Connection Proximity (WPM)

		/// <summary>
		/// Converts a WPM connection proximity key.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static ConnectionProximity ToConnectionProximity(
			Int32 inputValue)
		{
			return (ConnectionProximity)inputValue;
		}

		/// <summary>
		/// Converts a WPM connection proximity to the equivalent XMS value.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Int32 ToXMSConnectionProximity(
			ConnectionProximity inputValue)
		{
			return (Int32)inputValue;
		}

		#endregion

		#region Mapping (WPM)

		/// <summary>
		/// Converts a WPM mapping key.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Mapping ToMapping(Int32 inputValue)
		{
			return (Mapping)inputValue;
		}

		/// <summary>
		/// Converts a WPM mapping to the equivalent XMS value.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Int32 ToXMSMapping(Mapping inputValue)
		{
			return (Int32)inputValue;
		}

		#endregion

		#region Target Significance (WPM)

		/// <summary>
		/// Converts a WPM target significance key.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static TargetSignificance ToTargetSignificance(
			Int32 inputValue)
		{
			return (TargetSignificance)inputValue;
		}

		/// <summary>
		/// Converts a WPM target significance to the equivalent XMS value.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Int32 ToXMSTargetSignificance(
			TargetSignificance inputValue)
		{
			return (Int32)inputValue;
		}

		#endregion

		#region Target Type (WPM)

		/// <summary>
		/// Converts a WPM target type key.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static TargetType ToTargetType(Int32 inputValue)
		{
			return (TargetType)inputValue;
		}

		/// <summary>
		/// Converts a WPM target type to the equivalent XMS value.
		/// </summary>
		/// <param name="inputValue">Input value.</param>
		/// <returns>Converted value.</returns>
		public static Int32 ToXMSTargetType(TargetType inputValue)
		{
			return (Int32)inputValue;
		}

		#endregion

		#endregion

		#region IBM XMS to Apache NMS enumerations conversion

		/// <summary>
		/// Converts an IBM XMS destination type
		/// to the equivalent NMS value.
		/// </summary>
		/// <param name="xmsDestinationType">XMS destination type.</param>
		/// <param name="isTemporary">Whether the destination is temporary.
		/// </param>
		/// <returns>NMS destination type.</returns>
		public static DestinationType ToDestinationType(
			IBM.XMS.DestinationType xmsDestinationType, bool isTemporary)
		{
			switch(xmsDestinationType)
			{
			case IBM.XMS.DestinationType.Queue:
				return(isTemporary
					? DestinationType.TemporaryQueue
					: DestinationType.Queue);

			case IBM.XMS.DestinationType.Topic:
				return(isTemporary
					? DestinationType.TemporaryTopic
					: DestinationType.Queue);

			default:
				ThrowCantConvertValueException(
					xmsDestinationType.ToString(),
					"ToDestinationType");
				return DestinationType.Queue;
			}
		}

		/// <summary>
		/// Converts an IBM XMS acknowledgement mode
		/// to the equivalent NMS value.
		/// </summary>
		/// <param name="acknowledgeMode">XMS acknowledgement mode.</param>
		/// <returns>NMS acknowledgement mode.</returns>
		public static Apache.NMS.AcknowledgementMode ToAcknowledgementMode(
			IBM.XMS.AcknowledgeMode acknowledgeMode)
		{
			Apache.NMS.AcknowledgementMode acknowledge =
				Apache.NMS.AcknowledgementMode.AutoAcknowledge;

			switch(acknowledgeMode)
			{
			case IBM.XMS.AcknowledgeMode.AutoAcknowledge:
				acknowledge = Apache.NMS.AcknowledgementMode.AutoAcknowledge;
				break;

			case IBM.XMS.AcknowledgeMode.ClientAcknowledge:
				acknowledge = Apache.NMS.AcknowledgementMode.ClientAcknowledge;
				break;

			case IBM.XMS.AcknowledgeMode.DupsOkAcknowledge:
				acknowledge = Apache.NMS.AcknowledgementMode.DupsOkAcknowledge;
				break;

			case IBM.XMS.AcknowledgeMode.SessionTransacted:
				acknowledge = Apache.NMS.AcknowledgementMode.Transactional;
				break;
			}

			return acknowledge;
		}

		/// <summary>
		/// Converts an IBM XMS delivery mode
		/// to the equivalent NMS value.
		/// </summary>
		/// <param name="deliveryMode">XMS delivery mode.</param>
		/// <returns>NMS delivery mode.</returns>
		public static MsgDeliveryMode ToNMSMsgDeliveryMode(
			IBM.XMS.DeliveryMode deliveryMode)
		{
			if(deliveryMode == IBM.XMS.DeliveryMode.Persistent)
			{
				return MsgDeliveryMode.Persistent;
			}

			if(deliveryMode == IBM.XMS.DeliveryMode.NonPersistent)
			{
				return MsgDeliveryMode.NonPersistent;
			}

			// Hard cast it to the enumeration.
			return (MsgDeliveryMode) deliveryMode;
		}

		#endregion

		#region Apache NMS to IBM XMS enumerations conversion

		/// <summary>
		/// Converts an NMS acknowledgement mode
		/// to the equivalent IBM XMS value.
		/// </summary>
		/// <param name="acknowledge">NMS acknowledgement mode.</param>
		/// <returns>IBM XMS acknowledgement mode.</returns>
		public static IBM.XMS.AcknowledgeMode ToAcknowledgeMode(
			Apache.NMS.AcknowledgementMode acknowledge)
		{
			IBM.XMS.AcknowledgeMode acknowledgeMode =
				(IBM.XMS.AcknowledgeMode)0;

			switch(acknowledge)
			{
			case Apache.NMS.AcknowledgementMode.AutoAcknowledge:
				acknowledgeMode = IBM.XMS.AcknowledgeMode.AutoAcknowledge;
				break;

			case Apache.NMS.AcknowledgementMode.ClientAcknowledge:
				acknowledgeMode = IBM.XMS.AcknowledgeMode.ClientAcknowledge;
				break;

			case Apache.NMS.AcknowledgementMode.DupsOkAcknowledge:
				acknowledgeMode = IBM.XMS.AcknowledgeMode.DupsOkAcknowledge;
				break;

			case Apache.NMS.AcknowledgementMode.Transactional:
				acknowledgeMode = IBM.XMS.AcknowledgeMode.SessionTransacted;
				break;
			}

			return acknowledgeMode;
		}

		/// <summary>
		/// Converts an NMS delivery mode
		/// to the equivalent IBM XMS value.
		/// </summary>
		/// <param name="deliveryMode">NMS delivery mode.</param>
		/// <returns>IBM XMS delivery mode.</returns>
		public static IBM.XMS.DeliveryMode ToJMSDeliveryMode(
			MsgDeliveryMode deliveryMode)
		{
			if(deliveryMode == MsgDeliveryMode.Persistent)
			{
				return IBM.XMS.DeliveryMode.Persistent;
			}

			if(deliveryMode == MsgDeliveryMode.NonPersistent)
			{
				return IBM.XMS.DeliveryMode.NonPersistent;
			}

			// Hard cast it to the enumeration.
			return (IBM.XMS.DeliveryMode) deliveryMode;
		}

		#endregion

		#region Enumerable adapter

		private class EnumerableAdapter : IEnumerable
		{
			private readonly IEnumerator enumerator;
			public EnumerableAdapter(IEnumerator _enumerator)
			{
				this.enumerator = _enumerator;
			}

			public IEnumerator GetEnumerator()
			{
				return this.enumerator;
			}
		}

		public static IEnumerable ToEnumerable(IEnumerator enumerator)
		{
			return new EnumerableAdapter(enumerator);
		}

		#endregion
	}
}
