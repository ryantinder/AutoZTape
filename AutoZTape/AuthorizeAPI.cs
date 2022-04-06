using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;

namespace AutoZTape
{
    public class Authorize
    {
        public static ANetApiResponse GetTransactionDetails(string transactionId)
        {
            Console.WriteLine("Get transaction details sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.PRODUCTION;
            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = Program.APILoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = Program.TransactionKey,
            };

            var request = new getTransactionDetailsRequest();
            request.transId = transactionId;

            // instantiate the controller that will call the service
            var controller = new getTransactionDetailsController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                if (response.transaction == null)
                    return response;

                Console.WriteLine("Transaction Id: {0}", response.transaction.transId);
                Console.WriteLine("Transaction type: {0}", response.transaction.transactionType);
                Console.WriteLine("Transaction status: {0}", response.transaction.transactionStatus);
                Console.WriteLine("Transaction auth amount: {0}", response.transaction.authAmount);
                Console.WriteLine("Transaction settle amount: {0}", response.transaction.settleAmount);
            }
            else if (response != null)
            {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " +
                                  response.messages.message[0].text);
            }

            return response;
        }
        public static ANetApiResponse GetUnsettledTransactionList()
        {
            Console.WriteLine("Get unsettled transaction list sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.PRODUCTION;
            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = Program.APILoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = Program.TransactionKey,
            };

            var request = new getUnsettledTransactionListRequest();
            request.status = TransactionGroupStatusEnum.any;
            request.statusSpecified = true;
            request.paging = new Paging
            {
                limit = 10,
                offset = 1
            };
            request.sorting = new TransactionListSorting
            {
                orderBy = TransactionListOrderFieldEnum.id,
                orderDescending = true
            };
            // instantiate the controller that will call the service
            var controller = new getUnsettledTransactionListController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();
            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                if (response.transactions == null)
                    return response;

                foreach (var item in response.transactions)
                {
                    Console.WriteLine("Transaction Id: {0} was submitted on {1}", item.transId,
                        item.submitTimeLocal);
                }
            }
            else if (response != null)
            {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " +
                                  response.messages.message[0].text);
            }

            return response;
        }
        public static ANetApiResponse GetTransactionListForID(String BatchID)
        {
            Console.WriteLine("\nGet transaction list for ID = " + BatchID);

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.PRODUCTION;
            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = Program.APILoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = Program.TransactionKey,
            };

            // unique batch id
            string batchId = BatchID;

            var request = new getTransactionListRequest();
            request.batchId = batchId;
            request.paging = new Paging
            {
                limit = 100,
                offset = 1
            };
            request.sorting = new TransactionListSorting
            {
                orderBy = TransactionListOrderFieldEnum.id,
                orderDescending = true
            };

            // instantiate the controller that will call the service
            var controller = new getTransactionListController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                if (response.transactions == null)
                    return response;

                foreach (var transaction in response.transactions)
                {
                    Console.WriteLine("Transaction Id: {0}", transaction.transId);
                    Console.WriteLine("Submitted on (Local): {0}", transaction.submitTimeLocal);
                    Console.WriteLine("Status: {0}", transaction.transactionStatus);
                    Console.WriteLine("Settle amount: {0}", transaction.settleAmount);
                }
            }
            else if (response != null)
            {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " +
                                  response.messages.message[0].text);
            }

            return response;
        }
        public static ANetApiResponse GetSettledBatchListForDateRange(DateTime startDate, DateTime endDate)
        {
            Console.WriteLine("Get settled batch list sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.PRODUCTION;
            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = Program.APILoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = Program.TransactionKey,
            };

            //Get a date 1 week in the past
            var firstSettlementDate = startDate;
            var lastSettlementDate = endDate;
            Console.WriteLine("First settlement date: {0} Last settlement date:{1}", firstSettlementDate,
                lastSettlementDate);

            var request = new getSettledBatchListRequest();
            request.firstSettlementDate = firstSettlementDate;
            request.lastSettlementDate = lastSettlementDate;
            request.includeStatistics = true;

            // instantiate the controller that will call the service
            var controller = new getSettledBatchListController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();


            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                if (response.batchList == null)
                    return response;

                foreach (var batch in response.batchList)
                {
                    Console.WriteLine("Batch Id: {0}", batch.batchId);
                    Console.WriteLine("Batch settled on (UTC): {0}", batch.settlementTimeUTC);
                    Console.WriteLine("Batch settled on (Local): {0}", batch.settlementTimeLocal);
                    Console.WriteLine("Batch settlement state: {0}", batch.settlementState);
                    Console.WriteLine("Batch market type: {0}", batch.marketType);
                    Console.WriteLine("Batch product: {0}", batch.product);
                    foreach (var statistics in batch.statistics)
                    {
                        Console.WriteLine(
                            "Account type: {0} Total charge amount: {1} Charge count: {2} Refund amount: {3} Refund count: {4} Void count: {5} Decline count: {6} Error amount: {7}",
                            statistics.accountType, statistics.chargeAmount, statistics.chargeCount,
                            statistics.refundAmount, statistics.refundCount,
                            statistics.voidCount, statistics.declineCount, statistics.errorCount);
                    }
                }
            }
            else if (response != null)
            {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " +
                                  response.messages.message[0].text);
            }

            return response;
        }
    }
}