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
using Apache.NMS;
using Apache.NMS.Util;
using Apache.NMS.XMS.Util;
using IBM.XMS;

namespace Apache.NMS.XMS
{
	/// <summary>
	/// Represents a NMS session to IBM XMS.
	/// </summary>
	public class Session : Apache.NMS.ISession
	{
		public readonly IBM.XMS.ISession xmsSession;
		private bool closed = false;
		private bool disposed = false;
		
		public Session(IBM.XMS.ISession session)
		{
			this.xmsSession = session;
		}
		
		~Session()
		{
			Dispose(false);
		}
		
		#region ISession Members
		
		public Apache.NMS.IMessageProducer CreateProducer()
		{
			return CreateProducer(null);
		}
		
		public Apache.NMS.IMessageProducer CreateProducer(
			Apache.NMS.IDestination destination)
		{
			Apache.NMS.XMS.Destination destinationObj =
				(Apache.NMS.XMS.Destination)destination;
		
			try
			{
				Apache.NMS.IMessageProducer producer =
					XMSConvert.ToNMSMessageProducer(this,
						this.xmsSession.CreateProducer(
							destinationObj.xmsDestination));
				ConfigureProducer(producer);
				return producer;
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return null;
			}
		}
		
		public Apache.NMS.IMessageConsumer CreateConsumer(
			Apache.NMS.IDestination destination)
		{
			Apache.NMS.XMS.Destination destinationObj =
				(Apache.NMS.XMS.Destination)destination;
		
			try
			{
				Apache.NMS.IMessageConsumer consumer =
					XMSConvert.ToNMSMessageConsumer(this,
						this.xmsSession.CreateConsumer(
							destinationObj.xmsDestination));
				ConfigureConsumer(consumer);
				return consumer;
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return null;
			}
		}
		
		public Apache.NMS.IMessageConsumer CreateConsumer(
			Apache.NMS.IDestination destination, string selector)
		{
			Apache.NMS.XMS.Destination destinationObj =
				(Apache.NMS.XMS.Destination)destination;
		
			try
			{
				Apache.NMS.IMessageConsumer consumer =
					XMSConvert.ToNMSMessageConsumer(this,
						this.xmsSession.CreateConsumer(
							destinationObj.xmsDestination, selector));
				ConfigureConsumer(consumer);
				return consumer;
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return null;
			}
		}
		
		public Apache.NMS.IMessageConsumer CreateConsumer(
			Apache.NMS.IDestination destination, string selector, bool noLocal)
		{
			Apache.NMS.XMS.Destination destinationObj = 
				(Apache.NMS.XMS.Destination)destination;
		
			try
			{
				Apache.NMS.IMessageConsumer consumer =
					XMSConvert.ToNMSMessageConsumer(this,
						this.xmsSession.CreateConsumer(
							destinationObj.xmsDestination, selector, noLocal));
				ConfigureConsumer(consumer);
				return consumer;
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return null;
			}
		}
		
		public Apache.NMS.IMessageConsumer CreateDurableConsumer(
			Apache.NMS.ITopic destination, string name, string selector,
			bool noLocal)
		{
			Apache.NMS.XMS.Topic topicObj = (Apache.NMS.XMS.Topic)destination;
		
			try
			{
				Apache.NMS.IMessageConsumer consumer =
					XMSConvert.ToNMSMessageConsumer(this,
						this.xmsSession.CreateDurableSubscriber(
							topicObj.xmsDestination, name, selector, noLocal));
				ConfigureConsumer(consumer);
				return consumer;
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return null;
			}
		}
		
		private void ConfigureProducer(Apache.NMS.IMessageProducer producer)
		{
			producer.ProducerTransformer = this.ProducerTransformer;
		}
		
		private void ConfigureConsumer(Apache.NMS.IMessageConsumer consumer)
		{
			consumer.ConsumerTransformer = this.ConsumerTransformer;
		}
		
		public void DeleteDurableConsumer(string name)
		{
			try
			{
				this.xmsSession.Unsubscribe(name);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}
		
		public IQueueBrowser CreateBrowser(IQueue queue)
		{
			Apache.NMS.XMS.Queue queueObj = (Apache.NMS.XMS.Queue)queue;
		
			try
			{
				return XMSConvert.ToNMSQueueBrowser(this.xmsSession.CreateBrowser(
					queueObj.xmsDestination));
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return null;
			}
		}
		
		public IQueueBrowser CreateBrowser(IQueue queue, string selector)
		{
			Apache.NMS.XMS.Queue queueObj = (Apache.NMS.XMS.Queue) queue;
		
			try
			{
				return XMSConvert.ToNMSQueueBrowser(this.xmsSession.CreateBrowser(
					queueObj.xmsDestination, selector));
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return null;
			}
		}
		
		public Apache.NMS.IQueue GetQueue(string name)
		{
			try
			{
				return XMSConvert.ToNMSQueue(this.xmsSession.CreateQueue(name));
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return null;
			}
		}
		
		public Apache.NMS.ITopic GetTopic(string name)
		{
			try
			{
				return XMSConvert.ToNMSTopic(this.xmsSession.CreateTopic(name));
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return null;
			}
		}
		
		public Apache.NMS.ITemporaryQueue CreateTemporaryQueue()
		{
			try
			{
				return XMSConvert.ToNMSTemporaryQueue(
					this.xmsSession.CreateTemporaryQueue());
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return null;
			}
		}
		
		public Apache.NMS.ITemporaryTopic CreateTemporaryTopic()
		{
			try
			{
				return XMSConvert.ToNMSTemporaryTopic(
					this.xmsSession.CreateTemporaryTopic());
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return null;
			}
		}
		
		/// <summary>
		/// Delete a destination (Queue, Topic, Temp Queue, Temp Topic).
		/// </summary>
		public void DeleteDestination(IDestination destination)
		{
			// The IBM.XMS API does not support destination deletion
			throw new NotSupportedException();
		}
		
		public Apache.NMS.IMessage CreateMessage()
		{
			try
			{
				return XMSConvert.ToNMSMessage(
					this.xmsSession.CreateMessage());
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return null;
			}
		}
		
		public Apache.NMS.ITextMessage CreateTextMessage()
		{
			try
			{
				return XMSConvert.ToNMSTextMessage(
					this.xmsSession.CreateTextMessage());
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return null;
			}
		}
		
		public Apache.NMS.ITextMessage CreateTextMessage(string text)
		{
			try
			{
				return XMSConvert.ToNMSTextMessage(
					this.xmsSession.CreateTextMessage(text));
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return null;
			}
		}
		
		public Apache.NMS.IMapMessage CreateMapMessage()
		{
			try
			{
				return XMSConvert.ToNMSMapMessage(
					this.xmsSession.CreateMapMessage());
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return null;
			}
		}
		
		public Apache.NMS.IBytesMessage CreateBytesMessage()
		{
			try
			{
				return XMSConvert.ToNMSBytesMessage(
					this.xmsSession.CreateBytesMessage());
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return null;
			}
		}
		
		public Apache.NMS.IBytesMessage CreateBytesMessage(byte[] body)
		{
			try
			{
				Apache.NMS.IBytesMessage bytesMessage = CreateBytesMessage();
		
				if(null != bytesMessage)
				{
					bytesMessage.Content = body;
				}
		
				return bytesMessage;
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return null;
			}
		}
		
		public Apache.NMS.IStreamMessage CreateStreamMessage()
		{
			try
			{
				return XMSConvert.ToNMSStreamMessage(
					this.xmsSession.CreateStreamMessage());
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return null;
			}
		}
		
		public Apache.NMS.IObjectMessage CreateObjectMessage(Object body)
		{
			try
			{
				IBM.XMS.IObjectMessage xmsObjectMessage =
					this.xmsSession.CreateObjectMessage();
				xmsObjectMessage.Object = body;
				return XMSConvert.ToNMSObjectMessage(xmsObjectMessage);
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
				return null;
			}
		}
		
		public void Commit()
		{
			try
			{
				this.xmsSession.Commit();
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}
		
		public void Rollback()
		{
			try
			{
				this.xmsSession.Rollback();
			}
			catch(Exception ex)
			{
				ExceptionUtil.WrapAndThrowNMSException(ex);
			}
		}
		
		public void Recover()
		{
			throw new NotSupportedException();
		}
		
		private ConsumerTransformerDelegate consumerTransformer;
		/// <summary>
		/// A Delegate that is called each time a Message is dispatched to
		/// allow the client to do any necessary transformations on the
		/// received message before it is delivered. The Session instance
		/// sets the delegate on each Consumer it creates.
		/// </summary>
		public ConsumerTransformerDelegate ConsumerTransformer
		{
			get { return this.consumerTransformer; }
			set { this.consumerTransformer = value; }
		}
		
		private ProducerTransformerDelegate producerTransformer;
		/// <summary>
		/// A delegate that is called each time a Message is sent from this
		/// Producer which allows the application to perform any needed
		/// transformations on the Message before it is sent. The Session
		/// instance sets the delegate on each Producer it creates.
		/// </summary>
		public ProducerTransformerDelegate ProducerTransformer
		{
			get { return this.producerTransformer; }
			set { this.producerTransformer = value; }
		}
		
		#region Transaction State Events
		
		#pragma warning disable 0067
		public event SessionTxEventDelegate TransactionStartedListener;
		public event SessionTxEventDelegate TransactionCommittedListener;
		public event SessionTxEventDelegate TransactionRolledBackListener;
		#pragma warning restore 0067
		
		#endregion
		
		// Properties
		
		/// <summary>
		/// The default timeout for network requests.
		/// </summary>
		private TimeSpan requestTimeout =
			Apache.NMS.NMSConstants.defaultRequestTimeout;
		public TimeSpan RequestTimeout
		{
			get { return this.requestTimeout; }
			set { this.requestTimeout = value; }
		}
		
		public bool Transacted
		{
			get { return this.xmsSession.Transacted; }
		}
		
		public Apache.NMS.AcknowledgementMode AcknowledgementMode
		{
			get { return XMSConvert.ToAcknowledgementMode(this.xmsSession.AcknowledgeMode); }
		}
		
		public void Close()
		{
			lock(this)
			{
				if(closed)
				{
					return;
				}
		
				try
				{
					this.xmsSession.Close();
				}
				catch(Exception ex)
				{
					ExceptionUtil.WrapAndThrowNMSException(ex);
				}
				finally
				{
					closed = true;
				}
			}
		}
		
		#endregion
		
		#region IDisposable Members
		
		///<summary>
		/// Performs application-defined tasks associated with freeing,
		/// releasing, or resetting unmanaged resources.
		///</summary>
		///<filterpriority>2</filterpriority>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		
		protected void Dispose(bool disposing)
		{
			if(disposed)
			{
				return;
			}
		
			if(disposing)
			{
				// Dispose managed code here.
			}
		
			try
			{
				Close();
			}
			catch
			{
				// Ignore errors.
			}
		
			disposed = true;
		}
		
		#endregion
	}
}
