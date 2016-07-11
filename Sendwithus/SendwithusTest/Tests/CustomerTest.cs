﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading.Tasks;
using Sendwithus;
using System.Net;
using System.Collections.Generic;

namespace SendwithusTest
{
    /// <summary>
    /// Unit testing class for the Customer API calls
    /// </summary>
    [TestClass]
    public class CustomerTest
    {
        public const string DEFAULT_CUSTOMER_EMAIL_ADDRESS = "sendwithus.test@gmail.com";
        public const string NEW_CUSTOMER_EMAIL_ADDRESS = "sendwithus.test+new@gmail.com";
        public const string INVALID_CUSTOMER_EMAIL_ADDRESS = "invalid_email_address";
        public const string DEFAULT_CUSTOMER_LOCALE = "de-DE";
        public const string DEFAULT_GROUP_ID = "grp_7zpRYpExEBPpd6dGvyAfcT";
        private const Int64 LOG_CREATED_AFTER_TIME = 1234567890;
        private const Int64 LOG_CREATED_BEFORE_TIME = 9876543210;

        /// <summary>
        /// Tests the API call GET /customers/customer@example.com
        /// </summary>
        /// <returns>The associated task</returns>
        [TestMethod]
        public async Task TestGetCustomerAsync()
        {
            // Make the API call
            Trace.WriteLine(String.Format("GET /customers/{0}", DEFAULT_CUSTOMER_EMAIL_ADDRESS));
            Sendwithus.SendwithusClient.ApiKey = SendwithusClientTest.API_KEY_TEST;
            var response = await Customer.GetCustomerAsync(DEFAULT_CUSTOMER_EMAIL_ADDRESS);

            // Validate the response
            SendwithusClientTest.ValidateResponse(response);
        }

        /// <summary>
        /// Tests the API call GET /customers/customer@example.com with an invalid email address
        /// </summary>
        /// <returns>The associated task</returns>
        [TestMethod]
        public async Task TestGetCustomerWithInvalidEmailAddressAsync()
        {
            // Make the API call
            Trace.WriteLine(String.Format("GET /customers/{0}", INVALID_CUSTOMER_EMAIL_ADDRESS));
            Sendwithus.SendwithusClient.ApiKey = SendwithusClientTest.API_KEY_TEST;
            try
            {
                var response = await Customer.GetCustomerAsync(INVALID_CUSTOMER_EMAIL_ADDRESS);
                Assert.Fail("Failed to throw exception");
            }
            catch (SendwithusException exception)
            {
                // Make sure the response was HTTP 400 Bad Request
                SendwithusClientTest.ValidateException(exception, HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Tests the API call POST /customers
        /// </summary>
        /// <returns>The associated task</returns>
        [TestMethod]
        public async Task TestCreateOrUpdateCustomerAsync()
        {
            
            Trace.WriteLine("POST /customers");
            Sendwithus.SendwithusClient.ApiKey = SendwithusClientTest.API_KEY_TEST;

            // Build the new customer
            var customer = new Customer(NEW_CUSTOMER_EMAIL_ADDRESS);
            customer.data.Add("first_name", "Matt");
            customer.data.Add("city", "San Francisco");
            customer.locale = DEFAULT_CUSTOMER_LOCALE;
            customer.groups.Add(DEFAULT_GROUP_ID);

            // Make the API call
            var response = await Customer.CreateOrUpdateCustomerAsync(customer);

            // Validate the response
            SendwithusClientTest.ValidateResponse(response);
        }

        /// <summary>
        /// Tests the API call DELETE /customers/(:email)
        /// </summary>
        /// <returns>The associated task</returns>
        [TestMethod]
        public async Task TestDeleteCustomerAsync()
        {
            Trace.WriteLine(String.Format("DELETE /customers",NEW_CUSTOMER_EMAIL_ADDRESS));
            Sendwithus.SendwithusClient.ApiKey = SendwithusClientTest.API_KEY_TEST;

            // Make the API call
            var response = await Customer.DeleteCustomerAsync(NEW_CUSTOMER_EMAIL_ADDRESS);

            // Validate the response
            SendwithusClientTest.ValidateResponse(response);
        }

        /// <summary>
        /// Tests the API call GET /customers/matt@sendwithus.com/logs?count={count}&created_lt={timestamp}&created_gt={timestamp} with no parameters
        /// </summary>
        /// <returns>The associated task</returns>
        [TestMethod]
        public async Task TestGetCustomerEmailLogsWithNoParametersAsync()
        {
            // Make the API call
            Trace.WriteLine(String.Format("GET /customers/{0}/logs", DEFAULT_CUSTOMER_EMAIL_ADDRESS));
            Sendwithus.SendwithusClient.ApiKey = SendwithusClientTest.API_KEY_TEST;
            var response = await Customer.GetCustomerEmailLogsAsync(DEFAULT_CUSTOMER_EMAIL_ADDRESS);

            // Validate the response
            SendwithusClientTest.ValidateResponse(response);
        }

        /// <summary>
        /// Tests the API call GET /customers/matt@sendwithus.com/logs?count={count}&created_lt={timestamp}&created_gt={timestamp} with all parameters
        /// </summary>
        /// <returns>The associated task</returns>
        [TestMethod]
        public async Task TestGetCustomerEmailLogsWithAllParametersAsync()
        {
            
            Trace.WriteLine(String.Format("GET /customers/{0}/logs", DEFAULT_CUSTOMER_EMAIL_ADDRESS));
            Sendwithus.SendwithusClient.ApiKey = SendwithusClientTest.API_KEY_TEST;

            // Build the query parameters
            var queryParameters = new Dictionary<string, object>();
            queryParameters.Add("count", 2);
            queryParameters.Add("created", LOG_CREATED_BEFORE_TIME);
            queryParameters.Add("created_gt", LOG_CREATED_AFTER_TIME);

            // Make the API call
            var response = await Customer.GetCustomerEmailLogsAsync(DEFAULT_CUSTOMER_EMAIL_ADDRESS, queryParameters);

            // Validate the response
            SendwithusClientTest.ValidateResponse(response);
        }

        /// <summary>
        /// Tests the API call POST /customers/(:email)/groups/(:group_id)
        /// </summary>
        /// <returns>The associated task</returns>
        [TestMethod]
        public async Task TestAddCustomerToGroupAsync()
        {
            // Make the API call
            Trace.WriteLine(String.Format("POST /customers/{0}/groups/{1}", DEFAULT_CUSTOMER_EMAIL_ADDRESS, DEFAULT_GROUP_ID));
            Sendwithus.SendwithusClient.ApiKey = SendwithusClientTest.API_KEY_TEST;
            var response = await Customer.AddCustomerToGroupAsync(DEFAULT_CUSTOMER_EMAIL_ADDRESS, DEFAULT_GROUP_ID);

            // Validate the response
            SendwithusClientTest.ValidateResponse(response);
        }

        /// <summary>
        /// Tests the API call DELETE /customers/(:email)/groups/(:group_id)
        /// </summary>
        /// <returns>The associated task</returns>
        [TestMethod]
        public async Task TestRemoveCustomerFromGroupAsync()
        {
            // Make the API call
            Trace.WriteLine(String.Format("DELETE /customers/{0}/groups/{1}", DEFAULT_CUSTOMER_EMAIL_ADDRESS, DEFAULT_GROUP_ID));
            Sendwithus.SendwithusClient.ApiKey = SendwithusClientTest.API_KEY_TEST;
            var response = await Customer.RemoveCustomerFromGroupAsync(DEFAULT_CUSTOMER_EMAIL_ADDRESS, DEFAULT_GROUP_ID);

            // Validate the response
            SendwithusClientTest.ValidateResponse(response);
        }
    }
}