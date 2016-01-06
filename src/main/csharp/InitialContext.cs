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
using System.Collections.Specialized;
using Apache.NMS.XMS.Util;
using Apache.NMS.Policies;
using Apache.NMS.Util;
using IBM.XMS;

namespace Apache.NMS.XMS
{
	/// <summary>
	/// An Initial Context for querying object repositories for object
    /// definitions.
	/// </summary>
	public class InitialContext : IDisposable
	{
        public IBM.XMS.InitialContext xmsInitialContext;

        #region Constructors

        /// <summary>
        /// Constructs an <c>InitialContext</c> object.
        /// </summary>
        /// <param name="environment">Environment settings.</param>
		public InitialContext(Hashtable environment)
		{
            this.xmsInitialContext = new IBM.XMS.InitialContext(environment);
		}

        /// <summary>
        /// Constructs an <c>InitialContext</c> object specifying the
        /// repository URL.
        /// </summary>
        /// <param name="environment">Environment settings.</param>
        /// <param name="repositoryURL">Repository URL.</param>
		public InitialContext(Hashtable environment, string repositoryURL)
		{
            this.xmsInitialContext = new IBM.XMS.InitialContext(environment);
            this.RepositoryURL = repositoryURL;
		}

		#endregion

		#region Initial Context Properties (configure via ConnectionFactory URL parameters)

        // http://www-01.ibm.com/support/knowledgecenter/SSFKSJ_8.0.0/com.ibm.mq.msc.doc/props_inctx.htm?lang=en

        /// <summary>
        /// Repository URL.
        /// </summary>
		[UriAttribute("ic.RepositoryURL")]
        public string RepositoryURL
        {
            get { return (string)this.xmsInitialContext.Environment[XMSC.IC_URL]; }
            set { this.xmsInitialContext.Environment[XMSC.IC_URL] = value; }
        }

        /// <summary>
        /// Initial context provider URL.
        /// </summary>
		[UriAttribute("ic.ProviderURL")]
        public string ProviderURL
        {
            get { return (string)this.xmsInitialContext.Environment[XMSC.IC_PROVIDER_URL]; }
            set { this.xmsInitialContext.Environment[XMSC.IC_PROVIDER_URL] = value; }
        }

        /// <summary>
        /// Initial context security protocol.
        /// </summary>
		[UriAttribute("ic.SecurityProtocol")]
        public string SecurityProtocol
        {
            get { return (string)this.xmsInitialContext.Environment[XMSC.IC_SECURITY_PROTOCOL]; }
            set { this.xmsInitialContext.Environment[XMSC.IC_SECURITY_PROTOCOL] = value; }
        }

        /// <summary>
        /// Initial context security authentication.
        /// </summary>
		[UriAttribute("ic.SecurityAuthentication")]
        public string SecurityAuthentication
        {
            get { return (string)this.xmsInitialContext.Environment[XMSC.IC_SECURITY_AUTHENTICATION]; }
            set { this.xmsInitialContext.Environment[XMSC.IC_SECURITY_AUTHENTICATION] = value; }
        }

        /// <summary>
        /// Initial context security principal.
        /// </summary>
		[UriAttribute("ic.SecurityPrincipal")]
        public string SecurityPrincipal
        {
            get { return (string)this.xmsInitialContext.Environment[XMSC.IC_SECURITY_PRINCIPAL]; }
            set { this.xmsInitialContext.Environment[XMSC.IC_SECURITY_PRINCIPAL] = value; }
        }

        /// <summary>
        /// Initial context security credentials.
        /// </summary>
		[UriAttribute("ic.SecurityCredentials")]
        public string SecurityCredentials
        {
            get { return (string)this.xmsInitialContext.Environment[XMSC.IC_SECURITY_CREDENTIALS]; }
            set { this.xmsInitialContext.Environment[XMSC.IC_SECURITY_CREDENTIALS] = value; }
        }

        #endregion

        #region InitialContext Methods

        /// <summary>
        /// Create an object from an object definition that is retrieved from
        /// the repository of administered objects.
        /// </summary>
        /// <param name="objectName">Requested object name.</param>
        /// <returns>Requested object, or null if the requested object is
        /// not found.</returns>
        public object Lookup(string objectName)
        {
            return this.xmsInitialContext.Lookup(objectName);
        }

        /// <summary>
        /// Add a new property to the environment.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <param name="propertyValue">Property value.</param>
        /// <returns>Old property value.</returns>
        public object AddToEnvironment(
            string propertyName, object propertyValue)
        {
            return this.xmsInitialContext.AddToEnvironment(
                propertyName, propertyValue);
        }

        /// <summary>
        /// Remove a property from the environment.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <returns>Old property value.</returns>
        public object RemoveFromEnvironment(string propertyName)
        {
            return this.xmsInitialContext.RemoveFromEnvironment(propertyName);
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            this.xmsInitialContext.Close();
        }

        #endregion
	}
}
