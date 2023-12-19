using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace AutoZTape
{
    class ZTape
    {
        private DateTime _ZTapeDate;
        public DateTime ZTapeDate
        {
            get { return _ZTapeDate; }
            set { _ZTapeDate = value; }
        }

        public int StoreId
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings.Get("StoreId")); }
        }


        public string Store
        {
            get { return ConfigurationManager.AppSettings.Get("Store"); } 
        }


        public DateTime SubmitDate
        {
            get
            {
                return DateTime.Now;
            }
        }

        private double _grossSales = 0;

        public double GrossSales
        {
            get { return _grossSales; }
            set { _grossSales = value; }
        }

        private int _MgrMealsCount = 0;

        public int MgrMealsCount
        {
            get { return _MgrMealsCount; }
            set { _MgrMealsCount = value; }
        }

        private double _MgrMealsSales = 0;

        public double MgrMealsSales
        {
            get { return _MgrMealsSales; }
            set { _MgrMealsSales = value; }
        }

        private int _CouponsCount = 0;

        public int CouponsCount
        {
            get { return _CouponsCount; }
            set { _CouponsCount = value; }
        }

        private double _CouponsSales = 0;

        public double CouponsSales
        {
            get { return _CouponsSales; }
            set { _CouponsSales = value; }
        }

        private int _TenPercentDiscountCount = 0;

        public int TenPercentDiscountCount
        {
            get { return _TenPercentDiscountCount; }
            set { _TenPercentDiscountCount = value; }
        }

        private double _TenPercentDiscountSales = 0;

        public double TenPercentDiscountSales
        {
            get { return _TenPercentDiscountSales; }
            set { _TenPercentDiscountSales = value; }
        }

        private int _EmployeeMealsCount = 0;

        public int EmployeeMealsCount
        {
            get { return _EmployeeMealsCount; }
            set { _EmployeeMealsCount = value; }
        }

        private double _EmployeeMealsSales = 0;

        public double EmployeeMealsSales
        {
            get { return _EmployeeMealsSales; }
            set { _EmployeeMealsSales = value; }
        }

        private double _ReportableSales = 0;

        public double ReportableSales
        {
            get { return _ReportableSales; }
            set { _ReportableSales = value; }
        }

        private double _SalesTax = 0;

        public double SalesTax
        {
            get { return _SalesTax; }
            set { _SalesTax = value; }
        }

        private int _EatInCount = 0;

        public int EatInCount
        {
            get { return _EatInCount; }
            set { _EatInCount = value; }
        }

        private double _EatInSales = 0;

        public double EatInSales
        {
            get { return _EatInSales; }
            set { _EatInSales = value; }
        }

        private int _CarryOutCount = 0;

        public int CarryOutCount
        {
            get { return _CarryOutCount; }
            set { _CarryOutCount = value; }
        }

        private double _CarryOutSales = 0;

        public double CarryOutSales
        {
            get { return _CarryOutSales; }
            set { _CarryOutSales = value; }
        }

        private int _DThruCount = 0;

        public int DThruCount
        {
            get { return _DThruCount; }
            set { _DThruCount = value; }
        }

        private double _DThruSales;

        public double DThruSales
        {
            get { return _DThruSales; }
            set { _DThruSales = value; }
        }


        private int _VoidsCount = 0;

        public int VoidsCount
        {
            get { return _VoidsCount; }
            set { _VoidsCount = value; }
        }

        private double _VoidsSales = 0;

        public double VoidsSales
        {
            get { return _VoidsSales; }
            set { _VoidsSales = value; }
        }

        private int _NonTaxTransactionsCount = 0;

        public int NonTaxTransactionsCount
        {
            get { return _NonTaxTransactionsCount; }
            set { _NonTaxTransactionsCount = value; }
        }

        private double _NonTaxTransactionsSales = 0;

        public double NonTaxTransactionsSales
        {
            get { return _NonTaxTransactionsSales; }
            set { _NonTaxTransactionsSales = value; }
        }

        private double _GCSold = 0;

        public double GCSold
        {
            get { return _GCSold; }
            set { _GCSold = value; }
        }

        private double _GCRedemption = 0;

        public double GCRedemption
        {
            get { return _GCRedemption; }
            set { _GCRedemption = value; }
        }

        private double _EmpMealCharges = 0;

        public double EmpMealCharges
        {
            get { return _EmpMealCharges; }
            set { _EmpMealCharges = value; }
        }

        private int _ManagerId = 0;
 
        public int ManagerId
        {
            get { return _ManagerId; }
            set { _ManagerId= value; }
        }

        private string _Manager = "";

        public string Manager
        {
            get { return _Manager; }
            set { _Manager = value; }
        }

        private int _ItemTTLCount = 0;

        public int  ItemTTLCount
        {
            get { return _ItemTTLCount; }
            set { _ItemTTLCount = value; }
        }

        private int _SrDiscountCount = 0;

        public int SrDiscountCount
        {
            get { return _SrDiscountCount; }
            set { _SrDiscountCount = value; }
        }

        private double _SrDiscountSales = 0;

        public double SrDiscountSales
        {
            get { return _SrDiscountSales; }
            set { _SrDiscountSales = value; }
        }

        private int _PromotionsCount = 0;

        public int PromotionsCount
        {
            get { return _PromotionsCount; }
            set { _PromotionsCount = value; }
        }

        private double _PromotionSales = 0;

        public double PromotionsSales
        {
            get { return _PromotionSales; }
            set { _PromotionSales = value; }
        }

        private int _GiftCardsCount = 0;

        public int GiftCardsCount
        {
            get { return _GiftCardsCount; }
            set { _GiftCardsCount = value; }
        }

        private double _GiftCardsSales = 0;

        public double GiftCardsSales
        {
            get { return _GiftCardsSales; }
            set { _GiftCardsSales = value; }
        }

        private int _GCCount;

        public int GCCount
        {
            get { return _GCCount; }
            set { _GCCount = value; }
        }

        private double _GCSales;

        public double GCSales
        {
            get { return _GCSales; }
            set { _GCSales = value; }
        }


        private double _CashTD = 0;

        public double CashTD
        {
            get { return _CashTD; }
            set { _CashTD = value; }
        }

        private double _PullTotal = 0;

        public double PullTotal
        {
            get { return _PullTotal; }
            set { _PullTotal = value; }
        }

        private double _AveDThruTimeAverage = 0;

        public double AveDThruTimeAverage
        {
            get { return _AveDThruTimeAverage; }
            set { _AveDThruTimeAverage = value; }
        }

        private double _DThruOversAverage = 0;

        public double DThruOversAverage
        {
            get { return _DThruOversAverage; }
            set { _DThruOversAverage = value; }
        }

        private int _ErrorCorrectCount = 0;

        public int ErrorCorrectCount
        {
            get { return _ErrorCorrectCount; }
            set { _ErrorCorrectCount = value; }
        }

        private double _ErrorCorrectSales = 0;

        public double ErrorCorrectSales
        {
            get { return _ErrorCorrectSales; }
            set { _ErrorCorrectSales = value; }
        }

        private int _CashierCncCount = 0;

        public int CashierCncCount
        {
            get { return _CashierCncCount; }
            set { _CashierCncCount = value; }
        }

        private double _CashierCncSales = 0;

        public double CashierCncSales
        {
            get { return _CashierCncSales; }
            set { _CashierCncSales = value; }
        }

        private int _AllVoidsCount = 0;

        public int AllVoidsCount
        {
            get { return _AllVoidsCount; }
            set { _AllVoidsCount = value; }
        }

        private double _AllVoidsSales = 0;

        public double AllVoidsSales
        {
            get { return _AllVoidsSales; }
            set { _AllVoidsSales = value; }
        }

        private int _DeletesCount = 0;

        public int DeletesCount
        {
            get { return _DeletesCount; }
            set { _DeletesCount = value; }
        }

        private double _DeletesSales = 0;

        public double DeletesSales
        {
            get { return _DeletesSales; }
            set { _DeletesSales = value; }
        }

        private int _NoSalesCount = 0;

        public int NoSalesCount
        {
            get { return _NoSalesCount; }
            set { _NoSalesCount = value; }
        }

        private double _NoSales = 0;

        public double NoSales
        {
            get { return _NoSales; }
            set { _NoSales = value; }
        }

        private int _TrainingWagesCount = 0;

        public int TrainingWagesCount
        {
            get { return _TrainingWagesCount; }
            set { _TrainingWagesCount = value; }
        }

        private double _TrainingWagesSales = 0;

        public double TrainingWagesSales
        {
            get { return _TrainingWagesSales; }
            set { _TrainingWagesSales = value; }
        }

        private double _GTTapeSales = 0;

        public double GTTapeSales
        {
            get { return _GTTapeSales; }
            set { _GTTapeSales = value; }
        }

        private double _GrandSales = 0;

        public double GrandSales
        {
            get { return _GrandSales; }
            set { _GrandSales = value; }
        }

        private double _BankDeposit1 = 0;

        public double BankDeposit1
        {
            get { return _BankDeposit1; }
            set { _BankDeposit1 = value; }
        }

        private double _BankDeposit2 = 0;

        public double BankDeposit2
        {
            get { return _BankDeposit2; }
            set { _BankDeposit2 = value; }
        }

        private double _BankDeposit3 = 0;

        public double BankDeposit3
        {
            get { return _BankDeposit3; }
            set { _BankDeposit3 = value; }
        }

        private double _CreditCardVisaMc1 = 0;

        public double CreditCardVisaMc1
        {
            get { return _CreditCardVisaMc1; }
            set { _CreditCardVisaMc1 = value; }
        }

        private double _CreditCardVisaMc2 = 0;

        public double CreditCardVisaMc2
        {
            get { return _CreditCardVisaMc2; }
            set { _CreditCardVisaMc2 = value; }
        }

        private double _CreditCardAmEx1 = 0;

        public double CreditCardAmEx1
        {
            get { return _CreditCardAmEx1; }
            set { _CreditCardAmEx1 = value; }
        }

        private double _CreditCardAmEx2 = 0;

        public double CreditCardAmEx2
        {
            get { return _CreditCardAmEx2; }
            set { _CreditCardAmEx2 = value; }
        }

        private double _CreditCardDiscover1 = 0;

        public double CreditCardDiscover1
        {
            get { return _CreditCardDiscover1; }
            set { _CreditCardDiscover1 = value; }
        }

        private double _CreditCardDiscover2 = 0;

        public double CreditCardDiscover2
        {
            get { return _CreditCardDiscover2; }
            set { _CreditCardDiscover2 = value; }
        }

        private double _GiftCertificate = 0;

        public double GiftCertificate
        {
            get { return _GiftCertificate; }
            set { _GiftCertificate = value; }
        }

        private double _PdOutRepair = 0;

        public double PdOutRepair
        {
            get { return _PdOutRepair; }
            set { _PdOutRepair = value; }
        }

        private double _PdOutCleaning = 0;

        public double PdOutCleaning
        {
            get { return _PdOutCleaning; }
            set { _PdOutCleaning = value; }
        }

        private double _PdOutOperating = 0;

        public double PdOutOperating
        {
            get { return _PdOutOperating; }
            set { _PdOutOperating = value; }
        }

        private double _PdOutMiscellaneous = 0;

        public double PdOutMiscellaneous
        {
            get { return _PdOutMiscellaneous; }
            set { _PdOutMiscellaneous = value; }
        }

        private string _PdOut_Txt_Repair = "";

        public string PdOut_Txt_Repair
        {
            get { return _PdOut_Txt_Repair; }
            set { _PdOut_Txt_Repair = value; }
        }

        private string _PdOut_Txt_Cleaning = "";

        public string PdOut_Txt_Cleaning
        {
            get { return _PdOut_Txt_Cleaning; }
            set { _PdOut_Txt_Cleaning = value; }
        }

        private string _PdOut_Txt_Operating = "";

        public string PdOut_Txt_Operating
        {
            get { return _PdOut_Txt_Operating; }
            set { _PdOut_Txt_Operating = value; }
        }

        private string _PdOut_Txt_Miscellaneous = "";

        public string PdOut_Txt_Miscellaneous
        {
            get { return _PdOut_Txt_Miscellaneous; }
            set { _PdOut_Txt_Miscellaneous = value; }
        }

        private int _LoyPtsRedCount = 0;

        public int LoyPtsRedCount
        {
            get { return _LoyPtsRedCount; }
            set { _LoyPtsRedCount = value; }
        }

        private double _LoyPtsRedSales = 0;

        public double LoyPtsRedSales
        {
            get { return _LoyPtsRedSales; }
            set { _LoyPtsRedSales = value; }
        }

        private int _LoyPtsRewCount_10to2 = 0;

        public int LoyPtsRewCount_10to2
        {
            get { return _LoyPtsRewCount_10to2; }
            set { _LoyPtsRewCount_10to2 = value; }
        }

        private int _LoyPtsRewPoints_10to2 = 0;

        public int LoyPtsRewPoints_10to2
        {
            get { return _LoyPtsRewPoints_10to2; }
            set { _LoyPtsRewPoints_10to2 = value; }
        }

        private int _LoyPtsRewCount_2to5 = 0;

        public int LoyPtsRewCount_2to5
        {
            get { return _LoyPtsRewCount_2to5; }
            set { _LoyPtsRewCount_2to5 = value; }
        }

        private int _LoyPtsRewPoints_2to5 = 0;

        public int LoyPtsRewPoints_2to5
        {
            get { return _LoyPtsRewPoints_2to5; }
            set { _LoyPtsRewPoints_2to5 = value; }
        }

        private int _LoyPtsRewCount_5to9 = 0;

        public int LoyPtsRewCount_5to9
        {
            get { return _LoyPtsRewCount_5to9; }
            set { _LoyPtsRewCount_5to9 = value; }
        }

        private int _LoyPtsRewPoints_5to9 = 0;

        public int LoyPtsRewPoints_5to9
        {
            get { return _LoyPtsRewPoints_5to9; }
            set { _LoyPtsRewPoints_5to9 = value; }
        }

        private int _LoyPtsRewCount_9toC = 0;

        public int LoyPtsRewCount_9toC
        {
            get { return _LoyPtsRewCount_9toC; }
            set { _LoyPtsRewCount_9toC = value; }
        }

        private int _LoyPtsRewPoints_9toC = 0;

        public int LoyPtsRewPoints_9toC
        {
            get { return _LoyPtsRewPoints_9toC; }
            set { _LoyPtsRewPoints_9toC = value; }
        }

        private double _MaintRepairs1 = 0;

        public double MaintRepairs1
        {
            get { return _MaintRepairs1; }
            set { _MaintRepairs1 = value; }
        }

        private string _MaintRepDesc1 = "";

        public string MaintRepDesc1
        {
            get { return _MaintRepDesc1; }
            set { _MaintRepDesc1 = value; }
        }

        private double _MaintRepairs2 = 0;

        public double MaintRepairs2
        {
            get { return _MaintRepairs2; }
            set { _MaintRepairs2 = value; }
        }

        private string _MaintRepDesc2 = "";

        public string MaintRepDesc2
        {
            get { return _MaintRepDesc2; }
            set { _MaintRepDesc2 = value; }
        }

        private double _MaintRepairs3 = 0;

        public double MaintRepairs3
        {
            get { return _MaintRepairs3; }
            set { _MaintRepairs3 = value; }
        }

        private string _MaintRepDesc3 = "";

        public string MaintRepDesc3
        {
            get { return _MaintRepDesc3; }
            set { _MaintRepDesc3 = value; }
        }

        private double _OperatingEquip = 0;

        public double OperatingEquip
        {
            get { return _OperatingEquip; }
            set { _OperatingEquip = value; }
        }

        private string _OperatingEquipDesc = "";

        public string OperatingEquipDesc
        {
            get { return _OperatingEquipDesc; }
            set { _OperatingEquipDesc = value; }
        }

        private double _MiscPurchases1 = 0;

        public double MiscPurchases1
        {
            get { return _MiscPurchases1; }
            set { _MiscPurchases1 = value; }
        }

        private string _MiscPurchDesc1 = "";

        public string MiscPurchDesc1
        {
            get { return _MiscPurchDesc1; }
            set { _MiscPurchDesc1 = value; }
        }

        private double _MiscPurchases2 = 0;

        public double MiscPurchases2
        {
            get { return _MiscPurchases2; }
            set { _MiscPurchases2 = value; }
        }

        private string _MiscPurchDesc2 = "";

        public string MiscPurchDesc2
        {
            get { return _MiscPurchDesc2; }
            set { _MiscPurchDesc2 = value; }
        }

        private double _MGMTUniforms = 0;

        public double MGMTUniforms
        {
            get { return _MGMTUniforms; }
            set { _MGMTUniforms = value; }
        }

        private string _MGMTUniDesc = "";

        public string MGMTUniDesc
        {
            get { return _MGMTUniDesc; }
            set { _MGMTUniDesc = value; }
        }

        private int _PaidTimeOffCount = 0;

        public int PaidTimeOffCount
        {
            get { return _PaidTimeOffCount; }
            set { _PaidTimeOffCount = value; }
        }

        private double _PaidTimeOff = 0;

        public double PaidTimeOff
        {
            get { return _PaidTimeOff; }
            set { _PaidTimeOff = value; }
        }

        private string _DepositBy1 = "";

        public string DepositBy1
        {
            get { return _DepositBy1; }
            set { _DepositBy1 = value; }
        }

        private string _DepositDate1 = "";

        public string DepositDate1
        {
            get { return _DepositDate1; }
            set { _DepositDate1 = value; }
        }

        private string _DepositValid1 = "";

        public string DepositValid1
        {
            get { return _DepositValid1; }
            set { _DepositValid1 = value; }
        }

        private string _DepositBy2 = "";

        public string DepositBy2
        {
            get { return _DepositBy2; }
            set { _DepositBy2 = value; }
        }

        private string _DepositDate2 = "";

        public string DepositDate2
        {
            get { return _DepositDate2; }
            set { _DepositDate2 = value; }
        }

        private string _DepositValid2 = "";

        public string DepositValid2
        {
            get { return _DepositValid2; }
            set { _DepositValid2 = value; }
        }

        private string _DepositBy3 = "";

        public string DepositBy3
        {
            get { return _DepositBy3; }
            set { _DepositBy3 = value; }
        }

        private string _DepositDate3 = "";

        public string DepositDate3
        {
            get { return _DepositDate3; }
            set { _DepositDate3 = value; }
        }

        private string _DepositValid3 = "";

        public string DepositValid3
        {
            get { return _DepositValid3; }
            set { _DepositValid3 = value; }
        }

        private string _CCBatchInside = "";

        public string CCBatchInside
        {
            get { return _CCBatchInside; }
            set { _CCBatchInside = value; }
        }

        private string _CCBatchDrive = "";

        public string CCBatchDrive
        {
            get { return _CCBatchDrive; }
            set { _CCBatchDrive = value; }
        }

        private string _CCBatchMobile = "";

        public string CCBatchMobile
        {
            get { return _CCBatchMobile; }
            set { _CCBatchMobile = value; }
        }

        private double _CCMobile = 0;

        public double CCMobile
        {
            get { return _CCMobile; }
            set { _CCMobile = value; }
        }
        private double _TotalTips = 0;

        public double TotalTips
        {
            get { return _TotalTips; }
            set { _TotalTips = value; }
        }
    }
}
