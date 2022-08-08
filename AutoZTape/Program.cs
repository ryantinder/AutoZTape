using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Dapper;
using System.Data.Odbc;
using System.IO;
using System.Threading;
using AuthorizeNet.Api.Contracts.V1;
using System.Net;
using System.IO.Compression;
using System.Xml;
using Microsoft.Win32.TaskScheduler;

namespace AutoZTape
{
    class Program 
    {
        public static List<string> logArray = new List<string>();
        public static Version currentVersion = new Version("1.5.10");
        public static string target = DES.decrypt(ConfigurationManager.AppSettings.Get("pointedTo"));
        public static string sybase = DES.decrypt("Sybase");
        public static string APILoginID;
        public static string TransactionKey;
        public static ZTape ztape;
        public static DateTime date;
        
        static void Main(string[] args)
        {
            if (ConfigurationManager.AppSettings.Get("disableProgram") == "true")
            {
                Log("Program Disabled");
                Environment.Exit(-1);
            }
            if (ConfigurationManager.AppSettings.Get("testConnections") == "true")
            {
                Log("Program configured to only test connection");

                IDbConnection con1 = new System.Data.SqlClient.SqlConnection(target);
                Log("Connecting to target...");

                con1.Open();
                Log("Connection  " + (con1.State == ConnectionState.Open ? "successful" : "failed"));

                con1.Close();

                OdbcConnection con2 = new OdbcConnection(sybase);
                Log("Connecting to Sybase");

                con2.Open();
                Log("Connection  " + (con2.State == ConnectionState.Open ? "successful" : "failed"));
                con2.Close();

                Console.ReadKey();
                Environment.Exit(1);

            }
            Log("Program starting...");

            if (ConfigurationManager.AppSettings.Get("disableAutoUpdate") != "true")
            {
                try
                {
                    // checkForUpdates();
                } catch (Exception e)
                {
                    Log(e.ToString());
                }
            }
            else
            {
                Log("Auto update disabled");
            }
            APILoginID = findApiLoginId();
            TransactionKey = findTransactionKey();


            //Monitoring System
            IDbConnection targetConnection = new System.Data.SqlClient.SqlConnection(target);
   
            targetConnection.Open();
            targetConnection.Execute("update AutoZTapeMonitoring set runningversion = '" + currentVersion.ToString() + "', processinitiated = '" + DateTime.Now.ToString() + "', CP1_PacketGenerated = 0, CP2_MobileSalesGenerated = 0, " +
                                "CP3_PacketPushed = 0, CP4_EndOfProgramReached = 0 where store = '" + ConfigurationManager.AppSettings.Get("Store") + "'");
            

            System.Windows.Forms.Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            System.Windows.Forms.Application.SetUnhandledExceptionMode(System.Windows.Forms.UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            
            if (ConfigurationManager.AppSettings.Get("dateOverride") != "false")
            {
                date = new DateTime(Convert.ToInt32(ConfigurationManager.AppSettings.Get("dateOverride").Substring(0,4)),
                                    Convert.ToInt32(ConfigurationManager.AppSettings.Get("dateOverride").Substring(5,2)),
                                    Convert.ToInt32(ConfigurationManager.AppSettings.Get("dateOverride").Substring(8,2)));
            }
            else if (DateTime.Now.Hour < 18) {
                date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                date = date.AddDays(-1);
            }
            else
            {
                date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            }

            Log("Disable push to live = " + ConfigurationManager.AppSettings.Get("disableLivePush"));
            bool shouldPushTolive = ConfigurationManager.AppSettings.Get("disableLivePush") == "true" ? false : true;
            Log("Console Read Key = " + ConfigurationManager.AppSettings.Get("consoleReadKey()"));
            bool shouldHoldTerminal = ConfigurationManager.AppSettings.Get("consoleReadKey()") == "true" ? true : false;
            Log("Date Override = " + ((ConfigurationManager.AppSettings.Get("dateOverride") == "") ? "false" : ConfigurationManager.AppSettings.Get("dateOverride")));
            Log("mobile sales fetching = " + ConfigurationManager.AppSettings.Get("disableMobileAPI"));
            bool shouldFetchMobileSales = ConfigurationManager.AppSettings.Get("disableMobileAPI") == "true" ? false : true;
            object buffer;
            
            Log(((targetConnection.State == ConnectionState.Open) ? "Successful" : "Unsuccesful") + " connection to dev server");
            buffer = targetConnection.ExecuteScalar("select ztapeid from ZTape where ztapedate = '" + date + "' and storeid = " + ConfigurationManager.AppSettings.Get("StoreId"));

            if (buffer != null && buffer != DBNull.Value)
            {
                Log("ZTape already found for date " + date + ", ZTapeId = " + (int)buffer);
            }

            else
            {
                ztape = new ZTape();
                ztape.ZTapeDate = date;
                ztape.ManagerId = 0;
                Log("Fresh ztape created");
                targetConnection.Execute("dbo.addZTape @ZTapeDate, @StoreId, @Store, @ManagerId, @Manager, @SubmitDate, @GrossSales, @ItemTTLCount, @MgrMealsCount, @MgrMealsSales, @SrDiscountCount, "
                                    + "@SrDiscountSales, @CouponsCount, @CouponsSales, @TenPercentDiscountCount, @TenPercentDiscountSales, @PromotionsCount, @PromotionsSales, @EmployeeMealsCount, "
                                    + "@EmployeeMealsSales, @ReportableSales, @SalesTax, @GiftCardsCount, @GiftCardsSales, @GCCount, @GCSales, @CashTD, @PullTotal, @EatInCount, @EatInSales, "
                                    + "@CarryOutCount, @CarryOutSales, @DThruCount, @DThruSales, @AveDThruTimeAverage, @DThruOversAverage, @ErrorCorrectCount, @ErrorCorrectSales, @VoidsCount, "
                                    + "@VoidsSales, @CashierCncCount, @CashierCncSales, @AllVoidsCount, @AllVoidsSales, @DeletesCount, @DeletesSales, @NoSalesCount, @NoSales, @NonTaxTransactionsCount, "
                                    + "@NonTaxTransactionsSales, @TrainingWagesCount, @TrainingWagesSales, @GTTapeSales, @GrandSales, @BankDeposit1, @BankDeposit2, @BankDeposit3, @CreditCardVisaMc1, "
                                    + "@CreditCardVisaMc2, @CreditCardAmEx1, @CreditCardAmEx2, @CreditCardDiscover1, @CreditCardDiscover2, @GCSold, @GCRedemption , @GiftCertificate, @EmpMealCharges, "
                                    + "@PdOutRepair, @PdOutCleaning, @PdOutOperating, @PdOutMiscellaneous, @PdOut_Txt_Repair, @PdOut_Txt_Cleaning, @PdOut_Txt_Operating, @PdOut_Txt_Miscellaneous, "
                                    + "@LoyPtsRedCount, @LoyPtsRedSales, @LoyPtsRewCount_10to2, @LoyPtsRewPoints_10to2, @LoyPtsRewCount_2to5, @LoyPtsRewPoints_2to5, @LoyPtsRewCount_5to9, "
                                    + "@LoyPtsRewPoints_5to9, @LoyPtsRewCount_9toC, @LoyPtsRewPoints_9toC, @MaintRepairs1, @MaintRepDesc1, @MaintRepairs2, @MaintRepDesc2, @MaintRepairs3, "
                                    + "@MaintRepDesc3, @OperatingEquip, @OperatingEquipDesc, @MiscPurchases1, @MiscPurchDesc1, @MiscPurchases2, @MiscPurchDesc2, @MGMTUniforms, @MGMTUniDesc, "
                                    + "@PaidTimeOffCount, @PaidTimeOff, @DepositBy1, @DepositDate1, @DepositValid1, @DepositBy2, @DepositDate2, @DepositValid2, @DepositBy3, @DepositDate3, "
                                    + "@DepositValid3, @CCBatchInside, @CCBatchDrive, @CCBatchMobile, @CCMobile", ztape);

                Log("ZTape added for date " + date.ToString());

            }            

            
            Log("Updating previous day ZTape");
            //Lets get those values
            ztape = new ZTape();
            ztape.ZTapeDate = date;
            OdbcCommand cmd = new OdbcCommand();
            string paramText;
            string startTime = date.ToString("yyyy-MM-dd") + " 06:00:00.000";
            string endTime = date.AddDays(1).ToString("yyyy-MM-dd") + " 3:00:00.000";

            OdbcConnection sybaseConnection = new System.Data.Odbc.OdbcConnection(sybase);
            sybaseConnection.Open();

            paramText = "select coalesce(sum(nettotal), 0) from(select transact, avg(finaltotal) finaltotal, avg(nettotal) nettotal, avg(tax1) tax from(select posheader.opendate, posheader.status, posheader.timeend, posheader.finaltotal, "
                        + "posheader.numcust, posdetail.costeach, posdetail.quan, posheader.saletypeindex, posheader.transact, posdetail.prodtype, salestype.descript, posheader.nettotal, posheader.tax1 from dba.posheader left outer join "
                        + "dba.posdetail on posheader.transact = posdetail.transact inner join dba.salestype on posheader.saletypeindex = salestype.saletypeindex where posheader.opendate BETWEEN '" + startTime + "' AND '" + endTime + "'"
                        + "and posheader.status = 3 and posdetail.prodtype <> 100 and finaltotal <> 0) x group by transact) x";
            Log("Executing '" + paramText + "'");
            cmd = new OdbcCommand(paramText, sybaseConnection);
            buffer = cmd.ExecuteScalar();
            ztape.ReportableSales = (buffer == null || buffer == DBNull.Value) ? 0 : (double)buffer;
            Log("Reportable Sales = " + ztape.ReportableSales);


            paramText = "select coalesce(sum(tax), 0) from(select transact, avg(finaltotal) finaltotal, avg(nettotal) nettotal, avg(tax1) tax from(select posheader.opendate, posheader.status, posheader.timeend, posheader.finaltotal, "
                        + "posheader.numcust, posdetail.costeach, posdetail.quan, posheader.saletypeindex, posheader.transact, posdetail.prodtype, salestype.descript, posheader.nettotal, posheader.tax1 from dba.posheader left outer join "
                        + "dba.posdetail on posheader.transact = posdetail.transact inner join dba.salestype on posheader.saletypeindex = salestype.saletypeindex where posheader.opendate BETWEEN '" + startTime + "' AND '" + endTime + "'"
                        + "and posheader.status = 3 and posdetail.prodtype <> 100 and finaltotal <> 0) x group by transact) x";
            Log("Executing '" + paramText + "'");
            cmd = new OdbcCommand(paramText, sybaseConnection);
            buffer = cmd.ExecuteScalar();
            ztape.SalesTax = (buffer == null || buffer == DBNull.Value) ? 0 : (double)buffer;
            Log("Sales Tax = " + ztape.SalesTax);


            paramText = "SELECT coalesce(SUM(dba.POSDETAIL.QUAN), 0) FROM dba.POSDETAIL INNER JOIN dba.promo ON dba.POSDETAIL.PRODNUM = dba.promo.PROMONUM WHERE dba.POSDETAIL.OpenDate BETWEEN '" + startTime +
                "' AND '" + endTime + "' AND dba.POSDETAIL.COSTEACH <> 0 AND dba.POSDETAIL.PRODTYPE = 100 AND dba.promo.descript = 'COMP'";
            Log("Executing '" + paramText + "'");
            cmd = new OdbcCommand(paramText, sybaseConnection);
            buffer = cmd.ExecuteScalar();
            ztape.MgrMealsCount = (buffer == null || buffer == DBNull.Value) ? 0 : Convert.ToInt32(buffer);
            Log("Mgr Meals Count = " + ztape.MgrMealsCount);

            paramText = "SELECT coalesce(SUM(dba.POSDETAIL.COSTEACH), 0) FROM dba.POSDETAIL INNER JOIN dba.promo ON dba.POSDETAIL.PRODNUM = dba.promo.PROMONUM WHERE dba.POSDETAIL.OpenDate BETWEEN '" + startTime +
                "' AND '" + endTime + "' AND dba.POSDETAIL.COSTEACH <> 0 AND dba.POSDETAIL.PRODTYPE = 100 AND dba.promo.descript = 'COMP'";
            Log("Executing '" + paramText + "'");
            cmd = new OdbcCommand(paramText, sybaseConnection);
            buffer = cmd.ExecuteScalar();
            ztape.MgrMealsSales = (buffer == null || buffer == DBNull.Value) ? 0 : Math.Abs((double)buffer);
            Log("Mgr Meals Sales = " + ztape.MgrMealsSales);


            paramText = "SELECT coalesce(SUM(dba.POSDETAIL.QUAN), 0) FROM dba.POSDETAIL INNER JOIN dba.promo ON dba.POSDETAIL.PRODNUM = dba.promo.PROMONUM WHERE dba.POSDETAIL.OpenDate BETWEEN '" + startTime
                + "' AND '" + endTime + "' AND dba.POSDETAIL.COSTEACH <> 0 AND dba.POSDETAIL.PRODTYPE = 100 AND dba.promo.descript not in ('COMP', 'SR DISCOUNT', 'EMPLOYEE DISCOUNT')";
            Log("Executing '" + paramText + "'");
            cmd = new OdbcCommand(paramText, sybaseConnection);
            buffer = cmd.ExecuteScalar();
            ztape.CouponsCount = (buffer == null || buffer == DBNull.Value) ? 0 : Convert.ToInt32(buffer);
            Log("Coupon Count = " + ztape.CouponsCount);

            paramText = "SELECT coalesce( SUM(dba.POSDETAIL.COSTEACH), 0) FROM dba.POSDETAIL INNER JOIN dba.promo ON dba.POSDETAIL.PRODNUM = dba.promo.PROMONUM WHERE dba.POSDETAIL.OpenDate BETWEEN '" + startTime
                + "' AND '" + endTime + "' AND dba.POSDETAIL.COSTEACH <> 0 AND dba.POSDETAIL.PRODTYPE = 100 AND dba.promo.descript not in ('COMP', 'SR DISCOUNT', 'EMPLOYEE DISCOUNT')";
            Log("Executing '" + paramText + "'");
            cmd = new OdbcCommand(paramText, sybaseConnection);
            buffer = cmd.ExecuteScalar();
            ztape.CouponsSales = (buffer == null || buffer == DBNull.Value) ? 0 : Math.Abs((double)buffer);
            Log("Coupon Sales = " + ztape.CouponsSales);


            paramText = "SELECT coalesce(SUM(dba.POSDETAIL.QUAN), 0) FROM dba.POSDETAIL INNER JOIN dba.promo ON dba.POSDETAIL.PRODNUM = dba.promo.PROMONUM WHERE dba.POSDETAIL.OpenDate BETWEEN '" + startTime + "' AND '" + endTime +
                "' AND dba.POSDETAIL.COSTEACH <> 0 AND dba.POSDETAIL.PRODTYPE = 100 AND dba.promo.descript = 'SR DISCOUNT'";
            Log("Executing '" + paramText + "'");
            cmd = new OdbcCommand(paramText, sybaseConnection);
            buffer = cmd.ExecuteScalar();
            ztape.TenPercentDiscountCount = (buffer == null || buffer == DBNull.Value) ? 0 : Convert.ToInt32(buffer);
            Log("10% Count = " + ztape.TenPercentDiscountCount);


            paramText = "SELECT coalesce(SUM(dba.POSDETAIL.COSTEACH), 0) FROM dba.POSDETAIL INNER JOIN dba.promo ON dba.POSDETAIL.PRODNUM = dba.promo.PROMONUM WHERE dba.POSDETAIL.OpenDate BETWEEN '" + startTime + "' AND '" + endTime +
                "' AND dba.POSDETAIL.COSTEACH <> 0 AND dba.POSDETAIL.PRODTYPE = 100 AND dba.promo.descript = 'SR DISCOUNT'";
            Log("Executing '" + paramText + "'");
            cmd = new OdbcCommand(paramText, sybaseConnection);
            buffer = cmd.ExecuteScalar();
            ztape.TenPercentDiscountSales = (buffer == null || buffer == DBNull.Value) ? 0 : Math.Abs((double)buffer);
            Log("10% sales = " + ztape.TenPercentDiscountSales);

            paramText = "SELECT coalesce(SUM(dba.POSDETAIL.QUAN), 0) as TOTAL FROM dba.POSDETAIL INNER JOIN dba.promo ON dba.POSDETAIL.PRODNUM = dba.promo.PROMONUM WHERE dba.POSDETAIL.OpenDate BETWEEN '" + startTime
                + "' AND '" + endTime + "' AND dba.POSDETAIL.COSTEACH <> 0 AND dba.POSDETAIL.PRODTYPE = 100 AND dba.promo.descript = 'EMPLOYEE DISCOUNT'";
            Log("Executing '" + paramText + "'");
            cmd = new OdbcCommand(paramText, sybaseConnection);
            buffer = cmd.ExecuteScalar();
            ztape.EmployeeMealsCount = (buffer == null || buffer == DBNull.Value) ? 0 : Convert.ToInt32(buffer);
            Log("Employee Meals Count = " + ztape.EmployeeMealsCount);

            paramText = "SELECT coalesce(SUM(dba.POSDETAIL.COSTEACH), 0) as TOTAL FROM dba.POSDETAIL INNER JOIN dba.promo ON dba.POSDETAIL.PRODNUM = dba.promo.PROMONUM WHERE dba.POSDETAIL.OpenDate BETWEEN '" + startTime
                + "' AND '" + endTime + "' AND dba.POSDETAIL.COSTEACH <> 0 AND dba.POSDETAIL.PRODTYPE = 100 AND dba.promo.descript = 'EMPLOYEE DISCOUNT'";
            Log("Executing '" + paramText + "'");
            cmd = new OdbcCommand(paramText, sybaseConnection);
            buffer = cmd.ExecuteScalar();
            ztape.EmployeeMealsSales = (buffer == null || buffer == DBNull.Value) ? 0 : Math.Abs((double)buffer);
            Log("Employee Meals Sales = " + ztape.EmployeeMealsSales);

            ztape.GrossSales = ztape.MgrMealsSales + ztape.CouponsSales + ztape.TenPercentDiscountSales + ztape.EmployeeMealsSales + ztape.ReportableSales;
            Log("Gross Sales = " + ztape.GrossSales);

            paramText = "select coalesce(sum(numcust), 0) from (select transact, avg(finaltotal) finaltotal, avg(nettotal) nettotal, avg(tax1) tax, numcust from (select posheader.opendate, posheader.status, posheader.timeend,"
                + "posheader.finaltotal,posheader.numcust, posdetail.costeach, posdetail.quan, posheader.saletypeindex, posheader.transact, posdetail.prodtype, salestype.descript, posheader.nettotal, posheader.tax1 from dba.posheader"
                + " left outer join dba.posdetail on posheader.transact = posdetail.transact inner join dba.salestype on posheader.saletypeindex = salestype.saletypeindex where posheader.opendate BETWEEN '" + startTime + "' AND '"
                + endTime + "' and posheader.status = 3   and salestype.descript = 'QUICKSRV') x group by transact, numcust) x";
            Log("Executing '" + paramText + "'");
            cmd = new OdbcCommand(paramText, sybaseConnection);
            buffer = cmd.ExecuteScalar();
            ztape.EatInCount = (buffer == null || buffer == DBNull.Value) ? 0 : Convert.ToInt32(buffer);
            Log("Eat In Count = " + ztape.EatInCount);

            paramText = "select coalesce(sum(nettotal), 0) from (select transact, avg(finaltotal) finaltotal, avg(nettotal) nettotal, avg(tax1) tax, numcust from (select posheader.opendate, posheader.status, posheader.timeend,"
                + "posheader.finaltotal,posheader.numcust, posdetail.costeach, posdetail.quan, posheader.saletypeindex, posheader.transact, posdetail.prodtype, salestype.descript, posheader.nettotal, posheader.tax1 from dba.posheader"
                + " left outer join dba.posdetail on posheader.transact = posdetail.transact inner join dba.salestype on posheader.saletypeindex = salestype.saletypeindex where posheader.opendate BETWEEN '" + startTime + "' AND '"
                + endTime + "' and posheader.status = 3   and salestype.descript = 'QUICKSRV') x group by transact, numcust) x";
            Log("Executing '" + paramText + "'");
            cmd = new OdbcCommand(paramText, sybaseConnection);
            buffer = cmd.ExecuteScalar();
            ztape.EatInSales = (buffer == null || buffer == DBNull.Value) ? 0 : Math.Abs((double)buffer);
            Log("Eat In Sales = " + ztape.EatInSales);

            paramText = "select coalesce(sum(numcust), 0) from(select transact, avg(finaltotal) finaltotal, avg(nettotal) nettotal, avg(tax1) tax, numcust from(select posheader.opendate, posheader.status, posheader.timeend, posheader.finaltotal, "
                + "posheader.numcust, posdetail.costeach, posdetail.quan, posheader.saletypeindex, posheader.transact, posdetail.prodtype, salestype.descript, posheader.nettotal, posheader.tax1 from dba.posheader left outer join dba.posdetail on "
                + "posheader.transact = posdetail.transact inner join dba.salestype on posheader.saletypeindex = salestype.saletypeindex where posheader.opendate BETWEEN '" + startTime + "' AND '" + endTime + "' and posheader.status = 3   and "
                + "salestype.descript = 'PICK UP') x group by transact, numcust) x";
            Log("Executing '" + paramText + "'");
            cmd = new OdbcCommand(paramText, sybaseConnection);
            buffer = cmd.ExecuteScalar();
            ztape.CarryOutCount = (buffer == null || buffer == DBNull.Value) ? 0 : Convert.ToInt32(buffer);
            Log("Carry Out Count = " + ztape.CarryOutCount);

            paramText = "select coalesce(sum(nettotal), 0) from(select transact, avg(finaltotal) finaltotal, avg(nettotal) nettotal, avg(tax1) tax, numcust from(select posheader.opendate, posheader.status, posheader.timeend, posheader.finaltotal, "
                + "posheader.numcust, posdetail.costeach, posdetail.quan, posheader.saletypeindex, posheader.transact, posdetail.prodtype, salestype.descript, posheader.nettotal, posheader.tax1 from dba.posheader left outer join dba.posdetail on "
                + "posheader.transact = posdetail.transact inner join dba.salestype on posheader.saletypeindex = salestype.saletypeindex where posheader.opendate BETWEEN '" + startTime + "' AND '" + endTime + "' and posheader.status = 3   and "
                + "salestype.descript = 'PICK UP') x group by transact, numcust) x";
            Log("Executing '" + paramText + "'");
            cmd = new OdbcCommand(paramText, sybaseConnection);
            buffer = cmd.ExecuteScalar();
            ztape.CarryOutSales = (buffer == null || buffer == DBNull.Value) ? 0 : Math.Abs((double)buffer);
            Log("Carry Out Sales = " + ztape.CarryOutSales);

            paramText = "select coalesce(sum(numcust), 0) from(select transact, avg(finaltotal) finaltotal, avg(nettotal) nettotal, avg(tax1) tax, numcust from(select posheader.opendate, posheader.status, posheader.timeend, posheader.finaltotal, "
                + "posheader.numcust, posdetail.costeach, posdetail.quan, posheader.saletypeindex, posheader.transact, posdetail.prodtype, salestype.descript, posheader.nettotal, posheader.tax1 from dba.posheader left outer join dba.posdetail "
                + "on posheader.transact = posdetail.transact inner join dba.salestype on posheader.saletypeindex = salestype.saletypeindex where posheader.opendate BETWEEN '" + startTime + "' AND '" + endTime + "' and posheader.status = 3 "
                + "and salestype.descript = 'DRIVTHRU') x group by transact, numcust) x";
            Log("Executing '" + paramText + "'");
            cmd = new OdbcCommand(paramText, sybaseConnection);
            buffer = cmd.ExecuteScalar();
            ztape.DThruCount = (buffer == null || buffer == DBNull.Value) ? 0 : Convert.ToInt32(buffer);
            Log("DThru Count = " + ztape.DThruCount);

            paramText = "select coalesce(sum(nettotal), 0) from(select transact, avg(finaltotal) finaltotal, avg(nettotal) nettotal, avg(tax1) tax, numcust from(select posheader.opendate, posheader.status, posheader.timeend, posheader.finaltotal, "
                + "posheader.numcust, posdetail.costeach, posdetail.quan, posheader.saletypeindex, posheader.transact, posdetail.prodtype, salestype.descript, posheader.nettotal, posheader.tax1 from dba.posheader left outer join dba.posdetail "
                + "on posheader.transact = posdetail.transact inner join dba.salestype on posheader.saletypeindex = salestype.saletypeindex where posheader.opendate BETWEEN '" + startTime + "' AND '" + endTime + "' and posheader.status = 3 "
                + "and salestype.descript = 'DRIVTHRU') x group by transact, numcust) x";
            Log("Executing '" + paramText + "'");
            cmd = new OdbcCommand(paramText, sybaseConnection);
            buffer = cmd.ExecuteScalar();
            ztape.DThruSales = (buffer == null || buffer == DBNull.Value) ? 0 : Math.Abs((double)buffer);
            Log("DThru Sales = " + ztape.DThruSales);

            paramText = "select coalesce(count(quan), 0) from(select* from dba.posdetail left outer join dba.refundreasons on posdetail.howordered = refundreasons.refnum  where opendate BETWEEN '" + startTime + "' AND '" + endTime + "' and refnum > 1000 and costeach<> 0) x";
            Log("Executing '" + paramText + "'");
            cmd = new OdbcCommand(paramText, sybaseConnection);
            buffer = cmd.ExecuteScalar();
            ztape.VoidsCount = (buffer == null || buffer == DBNull.Value) ? 0 : Convert.ToInt32(buffer);
            Log("Voids count = " + ztape.VoidsCount);


            paramText = "select coalesce(sum(costeach * quan), 0) from(select* from dba.posdetail left outer join dba.refundreasons on posdetail.howordered = refundreasons.refnum  where opendate BETWEEN '" + startTime + "' AND '" + endTime + "' and refnum > 1000 and costeach<> 0) x";
            Log("Executing '" + paramText + "'");
            cmd = new OdbcCommand(paramText, sybaseConnection);
            buffer = cmd.ExecuteScalar();
            ztape.VoidsSales = (buffer == null || buffer == DBNull.Value) ? 0 : Math.Abs((double)buffer);
            Log("Voids sales = " + ztape.VoidsSales);


            paramText = "select coalesce(count(*), 0) from dba.posheader where tax1exempt = 1 and opendate BETWEEN '" + startTime + "' AND '" + endTime + "'";
            Log("Executing '" + paramText + "'");
            cmd = new OdbcCommand(paramText, sybaseConnection);
            buffer = cmd.ExecuteScalar();
            ztape.NonTaxTransactionsCount = (buffer == null || buffer == DBNull.Value) ? 0 : Convert.ToInt32(buffer);
            Log("Non tax count = " + ztape.NonTaxTransactionsCount);

            paramText = "select coalesce(sum(nettotal), 0) from dba.posheader where tax1exempt = 1 and opendate BETWEEN '" + startTime + "' AND '" + endTime + "'";
            Log("Executing '" + paramText + "'");
            cmd = new OdbcCommand(paramText, sybaseConnection);
            buffer = cmd.ExecuteScalar();
            ztape.NonTaxTransactionsSales = (buffer == null || buffer == DBNull.Value) ? 0 : Math.Abs((double)buffer);
            Log("Non tax sales = " + ztape.NonTaxTransactionsSales);


            paramText = "select coalesce(sum(tender), 0) from(select * from  dba.Howpaid left outer join dba.MethodPay on howpaid.methodnum = methodpay.methodnum where opendate BETWEEN '" + startTime + "' AND '" + endTime + "' and descript = 'Gift Card' and tender < 0) x";
            Log("Executing '" + paramText + "'");
            cmd = new OdbcCommand(paramText, sybaseConnection);
            buffer = cmd.ExecuteScalar();
            ztape.GiftCardsSales = (buffer == null || buffer == DBNull.Value) ? 0 : Math.Abs((double)buffer);
            Log("Gift card sales = " + ztape.GiftCardsSales);

            paramText = "select coalesce(sum(tender), 0) from(select * from  dba.Howpaid left outer join dba.MethodPay on howpaid.methodnum = methodpay.methodnum where opendate BETWEEN '" + startTime + "' AND '" + endTime + "' and descript = 'Gift Card' and tender > 0) x";
            Log("Executing '" + paramText + "'");
            cmd = new OdbcCommand(paramText, sybaseConnection);
            buffer = cmd.ExecuteScalar();
            ztape.GCRedemption = (buffer == null || buffer == DBNull.Value) ? 0 : Math.Abs((double)buffer);
            Log("Gift card redemption = " + ztape.GCRedemption);

            paramText = "select sum(tender) from(select* from  dba.Howpaid left outer join dba.MethodPay on howpaid.methodnum = methodpay.methodnum where opendate BETWEEN '" + startTime + "' AND '" + endTime + "' and descript = 'EMP Charge') x";
            Log("Executing '" + paramText + "'");
            cmd = new OdbcCommand(paramText, sybaseConnection);
            buffer = cmd.ExecuteScalar();
            ztape.EmpMealCharges = (buffer == null || buffer == DBNull.Value) ? 0 : Math.Abs((double)buffer);
            Log("Emp Meal Charges = " + ztape.EmpMealCharges);

            // Drive CC
            string paynums = ConfigurationManager.AppSettings.Get("methodpaynums");

            paramText = "select sum(tender) from howpayapproved inner join posheader on howpayapproved.transact = posheader.transact where howpayapproved.transdate between '" + startTime + "' AND '" + endTime + "' and saletypeindex in (1008, 1002, 1006) and methodnum in (" + paynums + ")";
            Log("Executing '" + paramText + "'");
            cmd = new OdbcCommand(paramText, sybaseConnection);
            buffer = cmd.ExecuteScalar();
            double DriveCC1 = (buffer == null || buffer == DBNull.Value) ? 0 : Math.Abs((double)buffer);
            Log("DriveCC1 = " + DriveCC1);

            paramText = "select sum(tender) from howpayapproved inner join posheader on howpayapproved.transact = posheader.transact where howpayapproved.transdate between '" + startTime + "' AND '" + endTime + "' and saletypeindex in (1008, 1002, 1006) and methodnum = 2005 and paytype = 114";
            Log("Executing '" + paramText + "'");
            cmd = new OdbcCommand(paramText, sybaseConnection);
            buffer = cmd.ExecuteScalar();
            double DriveCC3 = (buffer == null || buffer == DBNull.Value) ? 0 : Math.Abs((double)buffer);
            Log("DriveCC3 = " + DriveCC3);

            ztape.CreditCardVisaMc2 = DriveCC1 + DriveCC3;
            Log("Drive CC = " + ztape.CreditCardVisaMc2);
            sybaseConnection.Close();

            targetConnection.Execute("update AutoZTapeMonitoring set CP1_PacketGenerated = 1 where store = '" + ztape.Store + "'");
         

            if (shouldFetchMobileSales)
            {
                try
                {
                    ztape.CCMobile = fetchCCMobile(date);
                    Log("\nCCMobile = " + ztape.CCMobile);
                    targetConnection.Execute("update AutoZTapeMonitoring set CP2_MobileSalesGenerated = 1 where store = '" + ztape.Store + "'");
                }
                catch (Exception e)
                {
                    Log(e.Message);
                    Log(e.StackTrace);
                }
            }
            else
            {
                Log("Mobile fetching disabled");
            }

            


            if (shouldPushTolive)
            {
                Log("Push to live enabled, connecting to target");
                
                Log("Connection to target successful, executing update command");

                targetConnection.Execute("dbo.updateZTape @ZTapeDate, @StoreId, @Store, @SubmitDate, @GrossSales, @ReportableSales, @SalesTax, @MgrMealsCount, @MgrMealsSales, @CouponsCount, "
                                        + "@CouponsSales, @TenPercentDiscountCount, @TenPercentDiscountSales, @EmployeeMealsCount, @EmployeeMealsSales, @EatInCount, @EatInSales, "
                                        + "@CarryOutCount, @CarryOutSales, @DThruCount, @DThruSales, @VoidsCount, @VoidsSales, @NonTaxTransactionsCount, @NonTaxTransactionsSales, @GCSold, @GCRedemption, @EmpMealCharges, @CreditCardVisaMc2, @CCMobile", ztape);
                targetConnection.Execute("update autoZTapeMonitoring set CP3_PacketPushed = 1 where store = '" + ztape.Store + "'");
                Log("Execution successful...terminating");
                
            }
            else
            {
                Log("Live Push disabled");
            }

            Log("Program ending");
            DumpLog();

            targetConnection.Execute("update autoZTapeMonitoring set CP4_EndOfProgramReached = 1 where store = '" + ztape.Store + "'");
            targetConnection.Close();

            if (shouldHoldTerminal || !shouldPushTolive)
            {
                Console.ReadKey();
            }

            Environment.Exit(100);

        }


        public static double fetchCCMobile(DateTime date)
        {
            Log("*****Fetching CCMobile*****");
            double CCMobile = 0.0;
            DateTime startDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            DateTime searchEndDate = (startDate.AddDays(3).CompareTo(DateTime.Now) >= 0) ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0) : startDate.AddDays(3);
            DateTime timeframeEnd = date.AddHours(30);
            Log(startDate.ToLocalTime().ToString());
            var response = Authorize.GetSettledBatchListForDateRange(startDate, searchEndDate) as getSettledBatchListResponse;

            if (response == null)
            {
                Log("GetSettledBatchListForDateRange(" + startDate + ", " + searchEndDate + ") returned null.");
            }
            else if (response.batchList == null)
            {
                Log("response.batchlist is null for GetSettledBatchListForDateRange(" + startDate + ", " + searchEndDate + ") returned null.");
            }
            //All batches
            else
            {
                foreach (var batch in response.batchList)
                {
                    if (batch == null)
                    {
                        Log("Null batch");
                        continue;
                    }
                    var transactionList = Authorize.GetTransactionListForID(batch.batchId) as getTransactionListResponse;
                    if (transactionList == null || transactionList.transactions == null)
                    {
                        Log("Null transaction list");
                        continue;
                    }
                    //All transactions
                    foreach (var transaction in transactionList.transactions)
                    {
                        if (transaction == null)
                        {
                            Log("Null transaction in transaction list.");
                        }
                        //If settled, count it
                        else if (transaction.submitTimeLocal.CompareTo(startDate) >= 0 && transaction.submitTimeLocal.CompareTo(timeframeEnd) <= 0 && transaction.transactionStatus == "settledSuccessfully")
                        {
                            Log("Counting -> " + transaction.settleAmount);
                            try
                            {
                                CCMobile += Convert.ToDouble(transaction.settleAmount);
                            } catch(Exception e)
                            {
                                Log(e.Message);
                            }
                        }

                        //If refunded, check all batches for an equal amount, if equal amount is 
                        else if (transaction.submitTimeLocal.CompareTo(startDate) >= 0 && transaction.submitTimeLocal.CompareTo(timeframeEnd) <= 0 && transaction.transactionStatus == "refundSettledSuccessfully")
                        {

                            foreach (var checkBatch in response.batchList)
                            {
                                if (checkBatch == null) { continue; }
                                var checkTransactionList = Authorize.GetTransactionListForID(checkBatch.batchId) as getTransactionListResponse;
                                if (checkTransactionList == null) { continue; }
                                foreach (var checkTransaction in checkTransactionList.transactions)
                                {
                                    if (checkTransaction == null) { continue; }
                                    if (checkTransaction.settleAmount == transaction.settleAmount && checkTransaction.submitTimeLocal.CompareTo(startDate) >= 0 && checkTransaction.submitTimeLocal.CompareTo(timeframeEnd) <= 0 && checkTransaction.transId != transaction.transId)
                                    {
                                        Log("Refunding -> " + transaction.settleAmount);
                                        CCMobile = CCMobile - Convert.ToDouble(transaction.settleAmount);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            var unsettled = Authorize.GetUnsettledTransactionList() as getUnsettledTransactionListResponse;
            if (unsettled == null)
            {
                Log("No new unsettled transactions found.");
            }
            else if (unsettled.transactions == null)
            {
                Log("Transaction list null.");
            }
            else
            {
                foreach (var transaction in unsettled.transactions)
                {
                    if (transaction == null)
                    {
                        Log("Null transaction found.");
                        continue;
                    }
                    if (transaction.submitTimeLocal.CompareTo(startDate) >= 0 && transaction.submitTimeLocal.CompareTo(timeframeEnd) <= 0 && transaction.transactionStatus == "capturedPendingSettlement")
                    {
                        Log("Counting -> " + transaction.settleAmount);
                        CCMobile += Convert.ToDouble(transaction.settleAmount);
                    }
                }
            }
            Log("CCMobile = " + CCMobile);
            return CCMobile;
        }
        public static void Log(string s)
        {
            Console.WriteLine(s);
            logArray.Add(DateTime.Now.ToString("HH:mm:ss") + " : " + s);
        }
        public static void DumpLog()
        {
            string dayName = date.ToString("yyyy-MM-dd") + ".txt";
            string rootFolder = @"c:\AutoZTapeLogs";
            string pathString = System.IO.Path.Combine(rootFolder, dayName);
            if (!Directory.Exists(rootFolder))
            {
                System.IO.Directory.CreateDirectory(rootFolder);
            }
            if (!File.Exists(pathString))
            {
                using (StreamWriter f = new StreamWriter(pathString))
                {
                    f.WriteLine("****START OF LOG FILE****");
                    f.WriteLine("*File created: " + DateTime.Now.ToString());
                    f.WriteLine("*Day: " + date.ToString("dddd, MMMM dd, yyyy"));
                    f.WriteLine("*This is a log file that collects data from each run of AutoZTape.exe");
                    f.WriteLine("*\n*\n*\n");
                }
            }
            if (Directory.Exists(rootFolder) && File.Exists(pathString))
            {
                using (StreamWriter f = new StreamWriter(pathString, true))
                {
                    f.WriteLine("LOG CREATED: " + DateTime.Now.ToString());

                    foreach (string s in logArray)
                    {
                        f.WriteLine(s);
                    }
                    f.WriteLine("-------------------------------------------------------------------------------------------------\n");
                }
            }
        }

        public static string findApiLoginId()
        {
            string ApiLoginId = "";

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(target))
            {
                connection.Open();
                Log("Connection successful, finding API Login ID");

                ApiLoginId = connection.ExecuteScalar("select ApiLoginId from AuthorizeNetApiCredentials where store = '" + ConfigurationManager.AppSettings.Get("Store") + "'") as string;

                connection.Close();

                Log("Execution successful, API Login ID = " + APILoginID + "...terminating");
            }


            return ApiLoginId;
        }

        public static string findTransactionKey()
        {
            string transactionKey = "";

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(target))
            {
                connection.Open();
                Log("Connection to target successful, finding API login ID");

                transactionKey = connection.ExecuteScalar("select TransactionKey from AuthorizeNetApiCredentials where store = '" + ConfigurationManager.AppSettings.Get("Store") + "'") as string;

                connection.Close();

                Log("Execution successful, API Login ID = " + transactionKey + "...terminating");
            }


            return transactionKey;
        }

        public static void checkForUpdates()
        {

            Log("Checking for updates");
            string store = ConfigurationManager.AppSettings.Get("Store");
            IDbConnection connection = new System.Data.SqlClient.SqlConnection(target);
            connection.Open();



            connection.Execute("update dbo.automaticupdatemonitoring set currentVersion = '" + Convert.ToString(currentVersion.ToString()) + "' where Store = '" + store + "'");
            //Grab the latest version number
            string s;
            using (WebClient wc = new WebClient())
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                wc.Headers.Add("a", "a");
                Log("Downloading latest version NUMBER...");
                s = wc.DownloadString("https://raw.githubusercontent.com/tmmaster/TMVersioning/main/LatestZTapeVersion");
                Log("Successful, latest version: " + s);
            }
            //Read the actual number
            Version latestVersion = new Version(s);

            Log("Current version = " + currentVersion.ToString() + ", newest Version = " + latestVersion.ToString());
            Log("Needs update? " + (currentVersion < latestVersion));
            if (currentVersion >= latestVersion)
            {
                return;
            }
            // CP2: Determined update is needed
            connection.Execute("update dbo.automaticupdatemonitoring set CP1_DeterminedUpdateIsNeeded = 1 where Store = '" + store + "'");


            // New version procedures

            //Download new version ZIP
            Log("Downloading AutoZTape V" + latestVersion.ToString());
            string newVersionID = "https://raw.githubusercontent.com/tmmaster/TMVersioning/main/AutoZTape V" + latestVersion.ToString() + ".zip";
            string destinationPath = "C:/PixelPOS/AutoZTape/AutoZTape V" + latestVersion.ToString() + ".zip";
            using (WebClient wc = new WebClient())
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                wc.Headers.Add("a", "a");
                try
                {
                    //skips to extraction if file exists, downloads otherwise
                    if (File.Exists(destinationPath))
                    {
                        Log("***latest version ZIP alread exists, V" + latestVersion.ToString() + ", @ path: " + destinationPath + "--->returning");

                    }
                    else
                    {
                        wc.DownloadFile(newVersionID, destinationPath);
                        Log("Success");
                    }

                }
                catch (Exception e) { }
            }
            // CP3: New ZIP downloaded
            connection.Execute("update dbo.automaticupdatemonitoring set CP2_NewZIPDownloaded = 1 where Store = '" + store + "'");


            // Initial File extraction
            string firstDirPath = "C:/PixelPOS/AutoZTape/AutoZZTape V" + latestVersion.ToString();
            if (File.Exists(firstDirPath + "/AutoZTape V" + latestVersion.ToString() + "/bin/AutoZTape.exe"))
            {
                Log("***Initial Extraction Directory Path already exists, V" + latestVersion.ToString() + ", @ path: " + firstDirPath + "--> returning");

            }
            else if (Directory.Exists(firstDirPath))
            {
                Directory.Delete(firstDirPath);
                ZipFile.ExtractToDirectory(destinationPath, firstDirPath);
            }
            else
            {
                ZipFile.ExtractToDirectory(destinationPath, firstDirPath);
            }

            // CP4: Zip File extracted
            connection.Execute("update dbo.automaticupdatemonitoring set CP3_ZIPFileExtracted = 1 where Store = '" + store + "'");

            // Collapsing File Structure

            // Checking if final destination directory exists, then if executable exists, leave it alone, otherwise, delete it.
            bool flag = false;
            Console.WriteLine("Collapsing File Structure");
            string secDirPath = "C:/PixelPOS/AutoZTape/AutoZTape V" + latestVersion.ToString();
            if (Directory.Exists(secDirPath))
            {
                Console.WriteLine("***Final directory location already exists, V" + latestVersion.ToString() + ", @ path: " + secDirPath + ", checking if executable exists");
                if (File.Exists(secDirPath + "/bin/AutoZTape.exe"))
                {
                    Console.WriteLine("***Executable already exists, no need for move");
                    flag = true;
                }
                else
                {
                    Console.WriteLine("***Executable doesn't not exist, cleaning final directory location");
                    Directory.Delete(secDirPath, true);
                }
            }

            if (!flag)
            {
                Directory.Move("C:/PixelPOS/AutoZTape/AutoZZTape V" + latestVersion.ToString() + "/AutoZTape V" + latestVersion.ToString(), secDirPath);
            }
            // CP4: File Structure collapsed
            connection.Execute("update dbo.automaticupdatemonitoring set CP4_FileStructureCorrected = 1 where Store = '" + store + "'");


            //Clean-Up

            //Delete the temporary directory
            if (Directory.Exists("C:/PixelPOS/AutoZTape/AutoZZTape V" + latestVersion.ToString()))
            {
                Console.WriteLine("Deleting temporary directory");
                Directory.Delete("C:/PixelPOS/AutoZTape/AutoZZTape V" + latestVersion.ToString(), true);
            }
            // CP5: Temp directory no longer exists
            connection.Execute("update dbo.automaticupdatemonitoring set CP5_TempDirectoryNoLongerExists = 1 where Store = '" + store + "'");

            //Delete the zip file
            if (File.Exists(destinationPath))
            {
                Console.WriteLine("Deleting ZIP File");
                File.Delete(destinationPath);
            }
            // CP6: ZIP File no longer exists
            connection.Execute("update dbo.automaticupdatemonitoring set CP6_ZIPFileNoLongerExists = 1 where Store = '" + store + "'");


            //Modify the Settings.dll.config
            XmlDocument xml = new XmlDocument();
            string xmlPath = "C:/PixelPos/AutoZTape/AutoZTape V" + latestVersion.ToString() + "/bin/net5.0-windows/SettingsForm.dll.config";
            if (File.Exists(xmlPath))
            {
                xml.Load(xmlPath);
                Console.WriteLine("XML Loaded");
                Console.WriteLine("The new version's store: " + ReadKey("Store", xml));
                Console.WriteLine("But it should be: " + ConfigurationManager.AppSettings.Get("Store"));
                WriteKey("Store", ConfigurationManager.AppSettings.Get("Store"), xml, xmlPath);
                WriteKey("StoreId", ConfigurationManager.AppSettings.Get("StoreId"), xml, xmlPath);
                WriteKey("pointedTo", ConfigurationManager.AppSettings.Get("pointedTo"), xml, xmlPath);


                // CP7: Settings.dll.config modified successfully
                connection.Execute("update dbo.automaticupdatemonitoring set CP7_SettingsModifiedSuccessfully = 1 where Store = '" + store + "'");

            }
            else
            {
                Console.WriteLine("Couldn't Find XML File");
            }


            //Update the task
            Task t = TaskService.Instance.FindTask("AutoZTape");

            if (t != null)
            {
                t.Definition.Actions.RemoveAt(0);
                ExecAction action = new ExecAction("C:\\PixelPOS\\AutoZTape\\AutoZTape V" + latestVersion.ToString() + "\\bin\\AutoZTape.exe");
                t.Definition.Actions.Add(action);
                try
                {
                    t.RegisterChanges();
                }
                catch (Exception)
                {
               
                    Console.WriteLine("Error while registering task, attempting with password");
                    TaskService.Instance.RootFolder.RegisterTaskDefinition("AutoZTape", t.Definition, TaskCreation.CreateOrUpdate, "SYSTEM", null, TaskLogonType.ServiceAccount);
                }
                // CP8: Task updated successfully
                connection.Execute("update dbo.automaticupdatemonitoring set CP8_TaskUpdatedSuccessfully = 1 where Store = '" + store + "'");

            }


            

            // CP9: end of execution reached
            connection.Execute("update dbo.automaticupdatemonitoring set CP9_EndOfExecutionReached = 1 where Store = '" + store + "'");
            connection.Execute("update dbo.automaticupdatemonitoring set UpdateCompletionDate = '" + DateTime.Now.ToString() + "' where Store = '" + store + "'");
            connection.Close();

            //Process.Start("C:\\PixelPOS\\AutoZTape\\AutoZTape V" + latestVersion.ToString() + "/bin/AutoZTape.exe");
            //Environment.Exit(69);

        }
        private static string ReadKey(string key, XmlDocument xml)
        {
            XmlNodeList nodes = xml.SelectNodes("appSettings/add");
            foreach (XmlNode node in nodes)
            {
                XmlAttributeCollection nodeAtt = node.Attributes;
                if (nodeAtt["key"].Value == key)
                {
                    return nodeAtt["value"].Value;
                }
            }
            return "Node not found";
        }
        private static void WriteKey(string key, string value, XmlDocument xml, string xmlSavePath)
        {
            XmlNodeList nodes = xml.SelectNodes("appSettings/add");
            foreach (XmlNode node in nodes)
            {
                XmlAttributeCollection nodeAtt = node.Attributes;
                if (nodeAtt["key"].Value.ToString() == key)
                {
                    XmlAttribute nValue = node.Attributes["value"];
                    nValue.Value = value;
                    xml.Save(xmlSavePath);
                    return;
                }
            }
        }

        public static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Log(e.Exception.ToString());
            Log(e.Exception.StackTrace);
            DumpLog();
        }
        public static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log(((Exception)e.ExceptionObject).ToString());
            Log(((Exception)e.ExceptionObject).StackTrace);
            DumpLog();
        }
    }
}
