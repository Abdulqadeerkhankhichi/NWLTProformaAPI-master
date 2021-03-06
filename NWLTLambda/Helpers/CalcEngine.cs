using NWLTLambda.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic;


namespace NWLTLambda
{
    public class CalcEngine
    {
        public double ExciseOutput(double SalesPrice)
        {
            double result = 0;
            if (SalesPrice <= 500000)
            {
                result = 0.011 * SalesPrice;
            }
            else if (SalesPrice > 500000 && SalesPrice <= 1500000)
            {
                result = (500000 * 0.011) + ((SalesPrice - 500000) * 0.0128);
            }
            else if (SalesPrice > 1500000 && SalesPrice <= 3000000)
            {
                result = (500000 * 0.011) + (1000000 * 0.0128) + ((SalesPrice - 1500000) * 0.0275);
            }
            else if (SalesPrice > 3000000)
            {
                result = (500000 * 0.011) + (1000000 * 0.0128) + (1500000 * 0.0275) + ((SalesPrice - 3000000) * 0.03);
            }
            return result;
        }
        public List<StructureTypesVM> CalcSFProforma(ParametersList mParams)
        {
            List<StructureTypesVM> mOutModel = new List<StructureTypesVM>();
            mySqlAccess _sqlaccess = new mySqlAccess();

            const double mFactor1 = 1.07;
            const double mFactor2 = 0.06;
            const double mFactor3 = 0.0175;
            const double mFactor4 = 0.0075;
            const double mFactor5 = 0.005;
            const double mFactor6 = 0.25;
            if (mParams.Aadu == 0)
            {
                mParams.Aadu = 1;
                mParams.Aadu_size = 1000;
            }                
            if (mParams.Aadu_size == 0)
            {
                mParams.Aadu_size = 1000;
            }
            if (mParams.Dadu == 0)
            {
                mParams.Dadu = 1;
                mParams.Dadu_size = 1000;
            }
            if (mParams.Dadu_size == 0)
            {
                mParams.Dadu_size = 1000;
            }

            // TODO ----   you have to get a LIST OR ARRAY of the StructureType,  There could be an ID associated with them, but you could just use the names LIKE...SFHome, RowHome, Apartment, TownHome
            string[] mStructureType = { "Single Family", "Townhome", "Apartment", "Cottage", "Rowhouse", "Condo" };

            // List of structure types
            foreach (string mStructType in mStructureType)
            {

                List<CalcParameterOutputModel> mCalcList = new List<CalcParameterOutputModel>();
                CalcParameterOutputModel mSingleCalculation = new CalcParameterOutputModel();
                StructureTypesVM mCalcOutput = new StructureTypesVM();
                mCalcOutput.StructureType = mStructType;
                
                //  TODO --- You will have to replace  mParams.StructureType with mStructureType  to run the next 3 calculations
                decimal BuildCost = _sqlaccess.GetBaseCost(mParams.CityId, mParams.HighEndArea, mStructType);
                decimal mFAR = _sqlaccess.GetFAR(mParams.CityId, mParams.Zoning, mStructType, mParams.MhaSuffix, mParams.GrowthArea);
                decimal mHaFee = _sqlaccess.GetMHAFee(mParams.CityId, mParams.Mhaarea, mParams.MhaSuffix);
                decimal mDig = _sqlaccess.GetDensityLimit(mParams.CityId, mParams.Zoning, mStructType, mParams.MhaSuffix, mParams.CornerLot);
                
                //decimal FarMax = Convert.ToDecimal(mParams.LotSize) * mFAR;
                // decimal SFHome = Convert.ToDouble(FarMax) / mParams.Number_of_homes * Convert.ToDecimal(mFactor1);

                //if (mFAR != 0)
                //{
                double CostPerGrade = (mParams.CostPerGrade == 0 ? 1 : mParams.CostPerGrade);
                    double WaterLineCostPerSqft = (mParams.WaterLineCostPerSqft == 0 ? 1000 : mParams.WaterLineCostPerSqft);
                    double SewerCostPerSqft = (mParams.SewerCostPerSqft == 0 ? 1000 : mParams.SewerCostPerSqft);
                    double StormDrainCostPerSqft = (mParams.StormDrainCostPerSqft == 0 ? 1000 : mParams.StormDrainCostPerSqft);
                    double AlleyCostPerSqft = (mParams.AlleyCostPerSqft == 0 ? 500 : mParams.AlleyCostPerSqft);
                    double UndergroundParkingCostPerStall = (mParams.UndergroundParkingCostPerStall == 0 ? 50000 : mParams.UndergroundParkingCostPerStall);
                    double mSellableSqftFactor = (mParams.SellableSqftFactor == 0 ? 1.07 : mParams.SellableSqftFactor);
                    double mTownhomeBaseCostPerSqft = (mParams.TownhomeBaseCostPerSqft == 0 ? 250 : mParams.TownhomeBaseCostPerSqft);
                    double mSfBaseCostPerSqft = (mParams.SfBaseCostPerSqft == 0 ? 240 : mParams.SfBaseCostPerSqft);
                    double mApartmentBaseCostPerSqft = (mParams.ApartmentBaseCostPerSqft == 0 ? 275 : mParams.ApartmentBaseCostPerSqft);
                    double mRowhouseBaseCostPerSqft = (mParams.RowhouseBaseCostPerSqft == 0 ? 250 : mParams.RowhouseBaseCostPerSqft);
                    double mCondoBaseCostPerSqft = (mParams.CondoBaseCostPerSqft == 0 ? 275 : mParams.CondoBaseCostPerSqft);
                    double mCottageBaseCostPerSqft = (mParams.CottageBaseCostPerSqft == 0 ? 250 : mParams.CottageBaseCostPerSqft);
                    double mSfEstimatedValue = (mParams.SfEstimatedValue == 0 ? 2000000 : mParams.SfEstimatedValue);
                    double mDesiredProfitPercent = (mParams.DesiredProfitPercent == 0 ? 0.2 : mParams.DesiredProfitPercent);
                    double mEquity = (mParams.Equity == 0 ? 200000 : mParams.Equity);
                    double mAcquisitionCarryingMonths = (mParams.AcquisitionCarryingMonths == 0 ? 12 : mParams.AcquisitionCarryingMonths);
                    double mInterestRateLandAcquisition = (mParams.InterestRateLandAcquisition == 0 ? 0.06 : mParams.InterestRateLandAcquisition);
                    double mLoanFeesLandAcquisitionLoan = (mParams.LoanFeesLandAcquisitionLoan == 0 ? 0.01 : mParams.LoanFeesLandAcquisitionLoan);
                    double mInterestRateConstructionLoan = (mParams.InterestRateConstructionLoan == 0 ? 0.06 : mParams.InterestRateConstructionLoan);
                    double mLoanFeesConstructionLoan = (mParams.LoanFeesConstructionLoan == 0 ? 0.01 : mParams.LoanFeesConstructionLoan);
                    double mMonthsToSellAfterCompletion = (mParams.MonthsToSellAfterCompletion == 0 ? 3 : mParams.MonthsToSellAfterCompletion);
                    double mConstructionLoanMonths = (mParams.ConstructionLoanMonths == 0 ? 12 : mParams.ConstructionLoanMonths);
                    double mHomeEstimatedValue = (mParams.HomeEstimatedValue == 0 ? 1000000 : mParams.HomeEstimatedValue);
                    double mUnitEstimatedValue = (mParams.UnitEstimatedValue == 0 ? 500000 : mParams.UnitEstimatedValue);
                    double mInefficientSpacePercentage = (mParams.InefficientSpacePercentage == 0 ? 0.15 : mParams.InefficientSpacePercentage);
                    double mApartmentCapRate = (mParams.ApartmentCapRate == 0 ? 0.03 : mParams.ApartmentCapRate);
                    double mMonthlyRentPerUnit = (mParams.MonthlyRentPerUnit == 0 ? 2000 : mParams.MonthlyRentPerUnit);
                    double mVacancyRate = (mParams.VacancyRate == 0 ? 0.05 : mParams.VacancyRate);
                    double mOperatingExpensesPerFoot = (mParams.OperatingExpensesPerFt == 0 ? 10 : mParams.OperatingExpensesPerFt);
                    double mYearlyApartmentTaxes = (mParams.YearlyApartmentTaxes == 0 ? 50000 : mParams.YearlyApartmentTaxes);


                    //-FAR(Get from Database) 85
                    CalcParameterOutputModel mSingleCalculation85 = new CalcParameterOutputModel();
                    mSingleCalculation85.ParameterId = "85";
                    mSingleCalculation85.ParameterName = "Floor Area Ratio";
                    double FloorToAreaRatio = Convert.ToDouble(mFAR);
                    mSingleCalculation85.value = FloorToAreaRatio.ToString();
                    mCalcList.Add(mSingleCalculation85);

                    //- Property’s MAX FAR(FAR X Lot Size) 86
                    CalcParameterOutputModel mSingleCalculation86 = new CalcParameterOutputModel();
                    mSingleCalculation86.ParameterId = "86";
                    mSingleCalculation86.ParameterName = "FAR Max";
                    double FARMax = Convert.ToDouble(mParams.LotSize) * FloorToAreaRatio;
                    mSingleCalculation86.value = FARMax.ToString();
                    mCalcList.Add(mSingleCalculation86);

                //- Number of single-family homes(Lot Size / Minimum Lot size) rounded down to the nearest whole number 30
                //CalcParameterOutputModel mSingleCalculation30 = new CalcParameterOutputModel();
                //mSingleCalculation30.ParameterId = "30";
                //mSingleCalculation30.ParameterName = "Number of Homes";
                    double NumberOfHomes = Math.Round(mParams.LotSize / Convert.ToDouble(mDig));
                    
                
                    //mSingleCalculation30.value = NumberOfHomes.ToString();
                   // mCalcList.Add(mSingleCalculation30);

                    //- ECA cost(Pulled form Database)
                    CalcParameterOutputModel mSingleCalculation87 = new CalcParameterOutputModel();
                    mSingleCalculation87.ParameterId = "87";
                    mSingleCalculation87.ParameterName = "ECA Cost";
                    double EcaCost = 0;
                    mSingleCalculation87.value = EcaCost.ToString();
                    mCalcList.Add(mSingleCalculation87);

                    // Elevation Grade Cost (Lot Size X Elevation Change /2 X Cost per sqft of dirt removed/Number of Homes) * Cost Per Grade
                    CalcParameterOutputModel mSingleCalculation88 = new CalcParameterOutputModel();
                    mSingleCalculation88.ParameterId = "88";
                    mSingleCalculation88.ParameterName = "Elevation Grade Cost";
                    double ElevationGradeCost = mParams.LotSize * mParams.ElevationChange / 2 * CostPerGrade / NumberOfHomes;
                    mSingleCalculation88.value = ElevationGradeCost.ToString();
                    mCalcList.Add(mSingleCalculation88);

                    //- Water Line Cost (Water Main Extension Length X Price of Watermain Extension per foot/Number of Homes)
                    CalcParameterOutputModel mSingleCalculation89 = new CalcParameterOutputModel();
                    mSingleCalculation89.ParameterId = "89";
                    mSingleCalculation89.ParameterName = "Water Line Cost";
                    double WaterLineCost = mParams.WaterMainLength * WaterLineCostPerSqft / NumberOfHomes;
                    mSingleCalculation89.value = WaterLineCost.ToString();
                    mCalcList.Add(mSingleCalculation89);

                    //- Sewer Cost (Sewer Main Extension Length X Price of Sewer Cost per Sqft / Number of Homes)
                    CalcParameterOutputModel mSingleCalculation91 = new CalcParameterOutputModel();
                    mSingleCalculation91.ParameterId = "91";
                    mSingleCalculation91.ParameterName = "Sewer Cost";
                    double SewerCost = mParams.SewerMainLength * SewerCostPerSqft / NumberOfHomes;
                    mSingleCalculation91.value = SewerCost.ToString();
                    mCalcList.Add(mSingleCalculation91);

                    //- Storm Drain Cost (Storm Main Extension Length X Price of Storm Drain Cost per sqft /Number of Homes)
                    CalcParameterOutputModel mSingleCalculation105 = new CalcParameterOutputModel();
                    mSingleCalculation105.ParameterId = "105";
                    mSingleCalculation105.ParameterName = "Storm Drain Cost";
                    double StormDrainCost = mParams.DrainageDistance * StormDrainCostPerSqft / NumberOfHomes;
                    mSingleCalculation105.value = StormDrainCost.ToString();
                    mCalcList.Add(mSingleCalculation105);

                    //- Alley Cost (Alley Reconstruction Length X Alley Cost per foot / Number of Homes)
                    CalcParameterOutputModel mSingleCalculation90 = new CalcParameterOutputModel();
                    mSingleCalculation90.ParameterId = "90";
                    mSingleCalculation90.ParameterName = "Alley Cost";
                    double AlleyCost = mParams.AlleyConstructionLength * AlleyCostPerSqft / NumberOfHomes;
                    mSingleCalculation90.value = AlleyCost.ToString();
                    mCalcList.Add(mSingleCalculation90);

                    //- Underground Parking Cost (Number of Underground Spots X Underground Parking Cost / Number of Homes)
                    CalcParameterOutputModel mSingleCalculation106 = new CalcParameterOutputModel();
                    mSingleCalculation106.ParameterId = "106";
                    mSingleCalculation106.ParameterName = "Underground Parking Cost";
                    double UndergroundParkingCost = UndergroundParkingCostPerStall * mParams.NumberOfSpots / NumberOfHomes;
                    mSingleCalculation106.value = UndergroundParkingCost.ToString();
                    mCalcList.Add(mSingleCalculation106);

                    if (mStructType == "Single Family")
                    {


                        CalcParameterOutputModel mSingleCalculation30 = new CalcParameterOutputModel();
                        mSingleCalculation30.ParameterId = "30";
                        mSingleCalculation30.ParameterName = "Number of Homes";
                        NumberOfHomes = Math.Round(mParams.LotSize / Convert.ToDouble(mDig));
                       
                        mSingleCalculation30.value = NumberOfHomes.ToString();
                        mCalcList.Add(mSingleCalculation30);

                    // Number of Dadu’s(Equaled to the number of single - family homes) rounded down to the nearest whole 32
                        CalcParameterOutputModel mSingleCalculation32 = new CalcParameterOutputModel();
                        mSingleCalculation32.ParameterId = "32";
                        mSingleCalculation32.ParameterName = "Dadu";
                        double DADUNumber = Math.Floor(mParams.LotSize / Convert.ToDouble(mDig));
                       
                        mSingleCalculation32.value = DADUNumber.ToString();
                        mCalcList.Add(mSingleCalculation32);

                        // Number of AADU’s(Equaled to the number of single - family homes) rounded down to the nearest whole 34
                        CalcParameterOutputModel mSingleCalculation34 = new CalcParameterOutputModel();
                        mSingleCalculation34.ParameterId = "34";
                        mSingleCalculation34.ParameterName = "Aadu";
                        double AADUNumber = Math.Floor(mParams.LotSize / Convert.ToDouble(mDig)); 
                        
                        mSingleCalculation34.value = AADUNumber.ToString();
                        mCalcList.Add(mSingleCalculation34);

                        // Sellable Sqft.Factor(1.07) Default 96
                        CalcParameterOutputModel mSingleCalculation96 = new CalcParameterOutputModel();
                        mSingleCalculation96.ParameterId = "96";
                        mSingleCalculation96.ParameterName = "Sellable Sqft Factor";
                        double SellableSqftFactor = mSellableSqftFactor;
                        mSingleCalculation96.value = SellableSqftFactor.ToString();
                        mCalcList.Add(mSingleCalculation96);

                        //- Average Single Family Sellable sqft. (MAX FAR X Sellable Sqft. Factor / Number of single-family homes) 67
                        CalcParameterOutputModel mSingleCalculation67 = new CalcParameterOutputModel();
                        mSingleCalculation67.ParameterId = "67";
                        mSingleCalculation67.ParameterName = "Sellable Square Footage";
                        double SellableSquareFootage = (Convert.ToDouble(FARMax) * SellableSqftFactor)/ NumberOfHomes;
                        mSingleCalculation67.value = SellableSquareFootage.ToString();
                        mCalcList.Add(mSingleCalculation67);

                        //-Average DADU Sellable sqft. (Default to 1, 100sqft)
                        CalcParameterOutputModel mSingleCalculation35 = new CalcParameterOutputModel();
                        mSingleCalculation35.ParameterId = "35";
                        mSingleCalculation35.ParameterName = "DADU Size";
                        double DaduSize = 1100;
                        mSingleCalculation35.value = DaduSize.ToString();
                        mCalcList.Add(mSingleCalculation35);

                        //- Average AADU Sellable sqft. (Default to 1, 100sqft)
                        CalcParameterOutputModel mSingleCalculation33 = new CalcParameterOutputModel();
                        mSingleCalculation33.ParameterId = "33";
                        mSingleCalculation33.ParameterName = "AADU Size";
                        double AaduSize = 1100;
                        mSingleCalculation33.value = AaduSize.ToString();
                        mCalcList.Add(mSingleCalculation33);

                        //2nd Step(Find out the construction Costs)
                        //-Single Family Base Cost Per Sqft. (Default to $240 per foot)
                        CalcParameterOutputModel mSingleCalculation109 = new CalcParameterOutputModel();
                        mSingleCalculation109.ParameterId = "109";
                        mSingleCalculation109.ParameterName = "SF Base Cost Per Sqft";
                        double SfBaseCostPerSqft = mSfBaseCostPerSqft;
                        mSingleCalculation109.value = SfBaseCostPerSqft.ToString();
                        mCalcList.Add(mSingleCalculation109);

                        //- ADU’s cost per sqft. (Default to $250)
                        CalcParameterOutputModel mSingleCalculation115 = new CalcParameterOutputModel();
                        mSingleCalculation115.ParameterId = "115";
                        mSingleCalculation115.ParameterName = "ADU Base Cost Per Sqft";
                        double AduCostPerSqft = 250;
                        mSingleCalculation115.value = AduCostPerSqft.ToString();
                        mCalcList.Add(mSingleCalculation115);

                        //SF Base Cost (SF Base Cost Per Sqft. X Sellable square Footage.)
                        CalcParameterOutputModel mSingleCalculation97 = new CalcParameterOutputModel();
                        mSingleCalculation97.ParameterId = "97";
                        mSingleCalculation97.ParameterName = "SF Base Cost";
                        double SfBaseCost = SfBaseCostPerSqft * SellableSquareFootage;
                        mSingleCalculation97.value = SfBaseCost.ToString();
                        mCalcList.Add(mSingleCalculation97);

                        //-AADU Base Cost (ADU’s cost per sqft. X AADU Size.)
                        CalcParameterOutputModel mSingleCalculation104 = new CalcParameterOutputModel();
                        mSingleCalculation104.ParameterId = "104";
                        mSingleCalculation104.ParameterName = "AADU Base Cost";
                        double AaduBaseCost = AduCostPerSqft * AaduSize;
                        mSingleCalculation104.value = AaduBaseCost.ToString();
                        mCalcList.Add(mSingleCalculation104);

                        //-DADU Base Cost (ADU’s cost per sqft. X DADU Size.)
                        CalcParameterOutputModel mSingleCalculation124 = new CalcParameterOutputModel();
                        mSingleCalculation124.ParameterId = "124";
                        mSingleCalculation124.ParameterName = "DADU Base Cost";
                        double DaduBaseCost = AduCostPerSqft * DaduSize;
                        mSingleCalculation124.value = DaduBaseCost.ToString();
                        mCalcList.Add(mSingleCalculation124);

                        //Total Building Cost Per Lot(Sum of ALL RED TEXT)
                        CalcParameterOutputModel mSingleCalculation107 = new CalcParameterOutputModel();
                        mSingleCalculation107.ParameterId = "107";
                        mSingleCalculation107.ParameterName = "Total Building Cost Per Lot";
                        double TotalBuildingCostPerLot = SfBaseCost + AaduBaseCost + DaduBaseCost + EcaCost + ElevationGradeCost + WaterLineCost + SewerCost + StormDrainCost + AlleyCost + UndergroundParkingCost;
                        mSingleCalculation107.value = TotalBuildingCostPerLot.ToString();
                        mCalcList.Add(mSingleCalculation107);

                        //Total Project Cost(Total Building Cost Per Lot X Number of Single Family homes)
                        CalcParameterOutputModel mSingleCalculation108 = new CalcParameterOutputModel();
                        mSingleCalculation108.ParameterId = "108";
                        mSingleCalculation108.ParameterName = "Total Project Cost";
                        double TotalProjectCost = TotalBuildingCostPerLot * NumberOfHomes;
                        mSingleCalculation108.value = TotalProjectCost.ToString();
                        mCalcList.Add(mSingleCalculation108);

                        //3rd Step(Sales Cost Breakdown)
                        //SF Estimated Value (Manually Imported)
                        CalcParameterOutputModel mSingleCalculation37 = new CalcParameterOutputModel();
                        mSingleCalculation37.ParameterId = "37";
                        mSingleCalculation37.ParameterName = "SF Estimated Value";
                        double SFEstimatedValue = mSfEstimatedValue;
                        mSingleCalculation37.value = SFEstimatedValue.ToString();
                        mCalcList.Add(mSingleCalculation37);

                        // AADU Estimated Value (Manually Imported)
                        CalcParameterOutputModel mSingleCalculation38 = new CalcParameterOutputModel();
                        mSingleCalculation38.ParameterId = "38";
                        mSingleCalculation38.ParameterName = "ADU Estimated Value";
                        double ADUEstimatedValue = mParams.Aadu * mParams.Aadu_size * 250;
                        mSingleCalculation38.value = ADUEstimatedValue.ToString();
                        mCalcList.Add(mSingleCalculation38);

                        // DADU Estimated Value (Manually Imported)
                        CalcParameterOutputModel mSingleCalculation39 = new CalcParameterOutputModel();
                        mSingleCalculation39.ParameterId = "39";
                        mSingleCalculation39.ParameterName = "DADU Estimated Value";
                        double DADUEstimatedValue = mParams.Dadu * mParams.Dadu_size * 250;
                        mSingleCalculation39.value = DADUEstimatedValue.ToString();
                        mCalcList.Add(mSingleCalculation39);

                        // Lot Estimated Value (Sum of Green above)
                        CalcParameterOutputModel mSingleCalculation145 = new CalcParameterOutputModel();
                        mSingleCalculation145.ParameterId = "145";
                        mSingleCalculation145.ParameterName = "Lot Estimated Value";
                        double LotEstimatedValue = SFEstimatedValue + ADUEstimatedValue + DADUEstimatedValue;
                        mSingleCalculation145.value = LotEstimatedValue.ToString();
                        mCalcList.Add(mSingleCalculation145);

                        // Sales Commission (Sales Commission Table X Lot Estimated Value)
                        // TODO: load sales commission table
                        CalcParameterOutputModel mSingleCalculation41 = new CalcParameterOutputModel();
                        mSingleCalculation41.ParameterId = "41";
                        mSingleCalculation41.ParameterName = "Sales Commission";
                        double SalesCommission = 0.06 * LotEstimatedValue;
                        mSingleCalculation41.value = SalesCommission.ToString();
                        mCalcList.Add(mSingleCalculation41);

                        // Excise Tax (Excise Tax Table X Lot Estimated Value)
                        CalcParameterOutputModel mSingleCalculation42 = new CalcParameterOutputModel();
                        mSingleCalculation42.ParameterId = "42";
                        mSingleCalculation42.ParameterName = "Excise Tax";
                        double ExciseTax = ExciseOutput(LotEstimatedValue);
                        mSingleCalculation42.value = ExciseTax.ToString();
                        mCalcList.Add(mSingleCalculation42);

                        // Escrow Fees / Insurance (Escrow and Title Fee table X Lot Estimated Value)
                        CalcParameterOutputModel mSingleCalculation43 = new CalcParameterOutputModel();
                        mSingleCalculation43.ParameterId = "43";
                        mSingleCalculation43.ParameterName = "Escrow Fees/Insurance";
                        double EscrowFees = (SFEstimatedValue + ADUEstimatedValue + DADUEstimatedValue) * mFactor5;
                        mSingleCalculation43.value = EscrowFees.ToString();
                        mCalcList.Add(mSingleCalculation43);

                        // Marketing / Staging (Marketing and Staging table X Lot Estimated Value)
                        CalcParameterOutputModel mSingleCalculation44 = new CalcParameterOutputModel();
                        mSingleCalculation44.ParameterId = "44";
                        mSingleCalculation44.ParameterName = "Marketing/Cleaners";
                        double Marketing = (SFEstimatedValue + ADUEstimatedValue + DADUEstimatedValue) * mFactor5;
                        mSingleCalculation44.value = Marketing.ToString();
                        mCalcList.Add(mSingleCalculation44);

                        //- Total Sales Cost per Single Per Lot(Sum of all blue)
                        CalcParameterOutputModel mSingleCalculation134 = new CalcParameterOutputModel();
                        mSingleCalculation134.ParameterId = "134";
                        mSingleCalculation134.ParameterName = "Total Sales Cost Per Lot";
                        double TotalSalesCostPerLot = SalesCommission + ExciseTax + EscrowFees + Marketing;
                        mSingleCalculation134.value = TotalSalesCostPerLot.ToString();
                        mCalcList.Add(mSingleCalculation134);

                        //-Total Sales Cost per Project(Total Sales Cost per Single Per Lot X Number of Single-Family Homes)
                        CalcParameterOutputModel mSingleCalculation140 = new CalcParameterOutputModel();
                        mSingleCalculation140.ParameterId = "140";
                        mSingleCalculation140.ParameterName = "Total Sales Cost Per Project";
                        double TotalSalesCostPerProject = TotalSalesCostPerLot * NumberOfHomes;
                        mSingleCalculation140.value = TotalSalesCostPerProject.ToString();
                        mCalcList.Add(mSingleCalculation140);

                        //4th Step(Land Analysis)
                        // -Lot Estimated Value
                        CalcParameterOutputModel mSingleCalculation147 = new CalcParameterOutputModel();
                        mSingleCalculation147.ParameterId = "147";
                        mSingleCalculation147.ParameterName = "Lot Estimated Value";
                        double LotEstimatedValueLA = LotEstimatedValue;
                        mSingleCalculation147.value = LotEstimatedValueLA.ToString();
                        mCalcList.Add(mSingleCalculation147);

                        // -Desired Profit Percent
                        CalcParameterOutputModel mSingleCalculation181 = new CalcParameterOutputModel();
                        mSingleCalculation181.ParameterId = "181";
                        mSingleCalculation181.ParameterName = "Desired Profit Percent";
                        double DesiredProfitPercentLA = mDesiredProfitPercent;
                        mSingleCalculation181.value = DesiredProfitPercentLA.ToString();
                        mCalcList.Add(mSingleCalculation181);

                        //-Total Building Cost Per Lot
                        CalcParameterOutputModel mSingleCalculation170 = new CalcParameterOutputModel();
                        mSingleCalculation170.ParameterId = "170";
                        mSingleCalculation170.ParameterName = "Total Building Cost Per Lot";
                        double TotalBuildingCostPerLotLA = TotalBuildingCostPerLot;
                        mSingleCalculation170.value = TotalBuildingCostPerLotLA.ToString();
                        mCalcList.Add(mSingleCalculation170);

                        // -Total Sales Cost Per Lot
                        CalcParameterOutputModel mSingleCalculation172 = new CalcParameterOutputModel();
                        mSingleCalculation172.ParameterId = "172";
                        mSingleCalculation172.ParameterName = "Total Sales Cost Per Lot";
                        double TotalSalesCostPerLotLA = TotalSalesCostPerLot;
                        mSingleCalculation172.value = TotalSalesCostPerLotLA.ToString();
                        mCalcList.Add(mSingleCalculation172);

                        // -Desired Profit before financing (Desired profit % X Backend Sales Price)
                        CalcParameterOutputModel mSingleCalculation45 = new CalcParameterOutputModel();
                        mSingleCalculation45.ParameterId = "45";
                        mSingleCalculation45.ParameterName = "Desired Profit Before Financing";
                        double DesiredProfitBeforeFinancingLA = DesiredProfitPercentLA * LotEstimatedValueLA;
                        mSingleCalculation45.value = DesiredProfitBeforeFinancingLA.ToString();
                        mCalcList.Add(mSingleCalculation45);

                        // -Value of Land Per Lot (Sum of Step 4 above this line (Red is Negative Green is Positive Number)
                        CalcParameterOutputModel mSingleCalculation148 = new CalcParameterOutputModel();
                        mSingleCalculation148.ParameterId = "148";
                        mSingleCalculation148.ParameterName = "Value of Land Per Lot";
                        double ValueofLandPerLotLA = LotEstimatedValue - (TotalBuildingCostPerLot + TotalSalesCostPerLot + DesiredProfitBeforeFinancingLA);
                        mSingleCalculation148.value = ValueofLandPerLotLA.ToString();
                        mCalcList.Add(mSingleCalculation148);

                        //-Total Land Value (Value of Land Per Lot X Number of Single Family Homes)
                        CalcParameterOutputModel mSingleCalculation47 = new CalcParameterOutputModel();
                        mSingleCalculation47.ParameterId = "47";
                        mSingleCalculation47.ParameterName = "Total Land Value";
                        double TotalLandValue = ValueofLandPerLotLA * NumberOfHomes;
                        mSingleCalculation47.value = TotalLandValue.ToString();
                        mCalcList.Add(mSingleCalculation47);

                        // Step 5 Proforma & Financing

                        // Total Project Cost* Step 2  
                        CalcParameterOutputModel mSingleCalculation178 = new CalcParameterOutputModel();
                        mSingleCalculation178.ParameterId = "178";
                        mSingleCalculation178.ParameterName = "Total Project Cost";
                        double TotalProjectCostFI = TotalProjectCost;
                        mSingleCalculation178.value = TotalProjectCostFI.ToString();
                        mCalcList.Add(mSingleCalculation178);

                        // Total Land Value* Step 4
                        CalcParameterOutputModel mSingleCalculation179 = new CalcParameterOutputModel();
                        mSingleCalculation179.ParameterId = "179";
                        mSingleCalculation179.ParameterName = "Total Land Value";
                        double TotalLandValueFI = TotalLandValue;
                        mSingleCalculation179.value = TotalLandValueFI.ToString();
                        mCalcList.Add(mSingleCalculation179);

                        // Equity(Manual Input)
                        CalcParameterOutputModel mSingleCalculation55 = new CalcParameterOutputModel();
                        mSingleCalculation55.ParameterId = "55";
                        mSingleCalculation55.ParameterName = "Equity";
                        double EquityFI = mEquity;
                        mSingleCalculation55.value = EquityFI.ToString();
                        mCalcList.Add(mSingleCalculation55);

                        // Acquisition Carrying Months(Manual Input)
                        CalcParameterOutputModel mSingleCalculation56 = new CalcParameterOutputModel();
                        mSingleCalculation56.ParameterId = "56";
                        mSingleCalculation56.ParameterName = "Acquisition Carrying Months";
                        double AcquisitionCarryingMonthsFI = mAcquisitionCarryingMonths;
                        mSingleCalculation56.value = AcquisitionCarryingMonthsFI.ToString();
                        mCalcList.Add(mSingleCalculation56);

                        // Interest Rate Land Acquisition(Manual Input)
                        CalcParameterOutputModel mSingleCalculation57 = new CalcParameterOutputModel();
                        mSingleCalculation57.ParameterId = "57";
                        mSingleCalculation57.ParameterName = "Interest Rate Land Acquisition";
                        double InterestRateLandAcquisitionFI = mInterestRateLandAcquisition;
                        mSingleCalculation57.value = InterestRateLandAcquisitionFI.ToString();
                        mCalcList.Add(mSingleCalculation57);

                        // Loan Fees Land Acquisition Loan(Manual Input)
                        CalcParameterOutputModel mSingleCalculation81 = new CalcParameterOutputModel();
                        mSingleCalculation81.ParameterId = "81";
                        mSingleCalculation81.ParameterName = "Loan Fees Land Acquisition Loan";
                        double LoanFeesLandAcquisitionLoanFI = mLoanFeesLandAcquisitionLoan;
                        mSingleCalculation81.value = LoanFeesLandAcquisitionLoanFI.ToString();
                        mCalcList.Add(mSingleCalculation81);

                        // Total Financing Cost Land Acquisition((Land Value – Equity) * (Interest Rate Land Acquisition * (Acquisition Carrying Months/ 12) +(Land Value – Equity)*(Loan Fees Land Acquisition Loan)
                        CalcParameterOutputModel mSingleCalculation58 = new CalcParameterOutputModel();
                        mSingleCalculation58.ParameterId = "58";
                        mSingleCalculation58.ParameterName = "Total Financing Cost Land Acquisition";
                        double TotalFinancingCostLandAcquisitionFI = ((TotalLandValueFI - EquityFI) * (InterestRateLandAcquisitionFI * (AcquisitionCarryingMonthsFI / 12)) + ((TotalLandValueFI - EquityFI) * (LoanFeesLandAcquisitionLoanFI)));
                        mSingleCalculation58.value = TotalFinancingCostLandAcquisitionFI.ToString();
                        mCalcList.Add(mSingleCalculation58);

                        // Interest Rate Construction Loan(Manual Default)
                        CalcParameterOutputModel mSingleCalculation59 = new CalcParameterOutputModel();
                        mSingleCalculation59.ParameterId = "59";
                        mSingleCalculation59.ParameterName = "Interest Rate Construction Loan";
                        double InterestRateConstructionLoan = mInterestRateConstructionLoan;
                        mSingleCalculation59.value = InterestRateConstructionLoan.ToString();
                        mCalcList.Add(mSingleCalculation59);

                        // Loan Fees Construction Loan(Manual Default)
                        CalcParameterOutputModel mSingleCalculation60 = new CalcParameterOutputModel();
                        mSingleCalculation60.ParameterId = "60";
                        mSingleCalculation60.ParameterName = "Loan Fees Construction Loan";
                        double LoanFeesConstructionLoanPI = mLoanFeesConstructionLoan;
                        mSingleCalculation60.value = LoanFeesConstructionLoanPI.ToString();
                        mCalcList.Add(mSingleCalculation60);

                        // Construction Loan Months(Manual Default)
                        CalcParameterOutputModel mSingleCalculation61 = new CalcParameterOutputModel();
                        mSingleCalculation61.ParameterId = "61";
                        mSingleCalculation61.ParameterName = "Construction Loan Months";
                        double ConstructionLoanMonthsFI = mConstructionLoanMonths;
                        mSingleCalculation61.value = ConstructionLoanMonthsFI.ToString();
                        mCalcList.Add(mSingleCalculation61);

                        // Total Financing Construction Cost(((Land Value -Equity) +(Total Project Cost/ 2) *((Interest Rate Construction Loan”) *(Construction Loan Months/ 12)) +((Land Value – Equity) *(Loan Fees Construction Loan”)))
                        CalcParameterOutputModel mSingleCalculation62 = new CalcParameterOutputModel();
                        mSingleCalculation62.ParameterId = "62";
                        mSingleCalculation62.ParameterName = "Total Financing Cost Construction";
                        double TotalFinancingCostConstructionFI = ((((TotalLandValueFI - EquityFI) + (TotalProjectCostFI / 2)) * ((InterestRateConstructionLoan) * (ConstructionLoanMonthsFI / 12))) + (((TotalProjectCostFI + TotalLandValueFI - EquityFI) * (LoanFeesConstructionLoanPI))));
                        mSingleCalculation62.value = TotalFinancingCostConstructionFI.ToString();
                        mCalcList.Add(mSingleCalculation62);

                        // Months to Sell after completion(Manual Default)
                        CalcParameterOutputModel mSingleCalculation64 = new CalcParameterOutputModel();
                        mSingleCalculation64.ParameterId = "64";
                        mSingleCalculation64.ParameterName = "Months to Sell After completion";
                        double MonthstoSellAfterCompletionFI = mMonthsToSellAfterCompletion;
                        mSingleCalculation64.value = MonthstoSellAfterCompletionFI.ToString();
                        mCalcList.Add(mSingleCalculation64);

                        // Extra Financing After Completion(Land Value +Total Project Cost – Contributed Equity) *(Interest Rate Construction Loan * (Months to Sell after completion Months / 12)  
                        CalcParameterOutputModel mSingleCalculation65 = new CalcParameterOutputModel();
                        mSingleCalculation65.ParameterId = "65";
                        mSingleCalculation65.ParameterName = "Extra Financing After Completion";
                        double ExtraFinancingAfterCompletionFI = (TotalLandValueFI + TotalProjectCostFI - EquityFI) * (InterestRateConstructionLoan * (MonthstoSellAfterCompletionFI / 12));
                        mSingleCalculation65.value = ExtraFinancingAfterCompletionFI.ToString();
                        mCalcList.Add(mSingleCalculation65);

                        // Total Financing Fees(Sum of Green Text)
                        CalcParameterOutputModel mSingleCalculation66 = new CalcParameterOutputModel();
                        mSingleCalculation66.ParameterId = "66";
                        mSingleCalculation66.ParameterName = "Total Financing Fees";
                        double TotalFinancingFeesFI = TotalFinancingCostLandAcquisitionFI + TotalFinancingCostConstructionFI + ExtraFinancingAfterCompletionFI;
                        mSingleCalculation66.value = TotalFinancingFeesFI.ToString();
                        mCalcList.Add(mSingleCalculation66);

                        CalcParameterOutputModel mSingleCalculation163 = new CalcParameterOutputModel();
                        mSingleCalculation163.ParameterId = "163";
                        mSingleCalculation163.ParameterName = "Total Financing Cost Per Lot";
                        double TotalFinancingCostPerHomePF = TotalFinancingFeesFI / NumberOfHomes;
                        mSingleCalculation163.value = TotalFinancingCostPerHomePF.ToString();
                        mCalcList.Add(mSingleCalculation163);

                    // Step 6 Proformas and Financing

                    // -Lot Estimated Value 
                    CalcParameterOutputModel mSingleCalculation146 = new CalcParameterOutputModel();
                        mSingleCalculation146.ParameterId = "146";
                        mSingleCalculation146.ParameterName = "Lot Estimated Value";
                        double LotEstimatedValuePB = LotEstimatedValue;
                        mSingleCalculation146.value = LotEstimatedValuePB.ToString();
                        mCalcList.Add(mSingleCalculation146);

                        // -Total Building Cost Per Lot
                        CalcParameterOutputModel mSingleCalculation161 = new CalcParameterOutputModel();
                        mSingleCalculation161.ParameterId = "161";
                        mSingleCalculation161.ParameterName = "Total Building Cost Per Lot";
                        double TotalBuildingCostPerLotPB = TotalBuildingCostPerLot;
                        mSingleCalculation161.value = TotalBuildingCostPerLotPB.ToString();
                        mCalcList.Add(mSingleCalculation161);

                        // -Total Sales Cost Per Lot 
                        CalcParameterOutputModel mSingleCalculation135 = new CalcParameterOutputModel();
                        mSingleCalculation135.ParameterId = "135";
                        mSingleCalculation135.ParameterName = "Total Sales Cost Per Lot";
                        double TotalSalesCostPerLotPB = TotalSalesCostPerLot;
                        mSingleCalculation135.value = TotalSalesCostPerLotPB.ToString();
                        mCalcList.Add(mSingleCalculation135);

                        // -Value of Land Per Lot 
                        CalcParameterOutputModel mSingleCalculation149 = new CalcParameterOutputModel();
                        mSingleCalculation149.ParameterId = "149";
                        mSingleCalculation149.ParameterName = "Value of Land Per Lot";
                        double ValueofLandPerLotPB = ValueofLandPerLotLA;
                        mSingleCalculation149.value = ValueofLandPerLotPB.ToString();
                        mCalcList.Add(mSingleCalculation149);

                        // -Profit per lot before Financing (Sum of Step 5 above this line (Red is Negative Green is Positive Number for per lot)
                        CalcParameterOutputModel mSingleCalculation153 = new CalcParameterOutputModel();
                        mSingleCalculation153.ParameterId = "153";
                        mSingleCalculation153.ParameterName = "Profit Per Home before Financing";
                        double ProfitPerLotBeforeFinancing = LotEstimatedValuePB - TotalBuildingCostPerLotPB - TotalSalesCostPerLotPB - ValueofLandPerLotPB;
                        mSingleCalculation153.value = ProfitPerLotBeforeFinancing.ToString();
                        mCalcList.Add(mSingleCalculation153);

                        // -Financing Costs(Total financing Cost / Number of Single Family Homes)
                        CalcParameterOutputModel mSingleCalculation162 = new CalcParameterOutputModel();
                        mSingleCalculation162.ParameterId = "162";
                        mSingleCalculation162.ParameterName = "Total Financing Cost";
                        double TotalFinancingCostPF = TotalFinancingFeesFI;
                        mSingleCalculation162.value = TotalFinancingCostPF.ToString();
                        mCalcList.Add(mSingleCalculation162);

                        // -Net Profit Per Lot (Profit per lot before Financing - Financing Costs)
                        CalcParameterOutputModel mSingleCalculation156 = new CalcParameterOutputModel();
                        mSingleCalculation156.ParameterId = "156";
                        mSingleCalculation156.ParameterName = "Net Profit Per Lot";
                        double NetProfitPerLotPF = ProfitPerLotBeforeFinancing - TotalFinancingCostPerHomePF;
                        mSingleCalculation156.value = NetProfitPerLotPF.ToString();
                        mCalcList.Add(mSingleCalculation156);

                        // Performa Per Project
                        // -Total Backend Sales Price(Backend Sales Price X Number of Single Family Homes)
                        CalcParameterOutputModel mSingleCalculation166 = new CalcParameterOutputModel();
                        mSingleCalculation166.ParameterId = "166";
                        mSingleCalculation166.ParameterName = "Total Backend Sales Price";
                        double TotalBackendSalesPricePF = LotEstimatedValueLA * NumberOfHomes;
                        mSingleCalculation166.value = TotalBackendSalesPricePF.ToString();
                        mCalcList.Add(mSingleCalculation166);

                        // - Total Project Cost *See step 2
                        CalcParameterOutputModel mSingleCalculation182 = new CalcParameterOutputModel();
                        mSingleCalculation182.ParameterId = "182";
                        mSingleCalculation182.ParameterName = "Total Project Cost";
                        double TotalProjectCostPF = TotalProjectCost;
                        mSingleCalculation182.value = TotalProjectCostPF.ToString();
                        mCalcList.Add(mSingleCalculation182);

                        // - Total Sales Cost Per Project *See step 3
                        CalcParameterOutputModel mSingleCalculation141 = new CalcParameterOutputModel();
                        mSingleCalculation141.ParameterId = "141";
                        mSingleCalculation141.ParameterName = "Total Sales Cost Per Project";
                        double TotalSalesCostPerProjectPF = TotalSalesCostPerProject;
                        mSingleCalculation141.value = TotalSalesCostPerProjectPF.ToString();
                        mCalcList.Add(mSingleCalculation141);

                        // - Total Land Value *See step 4
                        CalcParameterOutputModel mSingleCalculation123 = new CalcParameterOutputModel();
                        mSingleCalculation123.ParameterId = "123";
                        mSingleCalculation123.ParameterName = "Total Land Value";
                        double TotalLandValuePF = TotalLandValue;
                        mSingleCalculation123.value = TotalLandValuePF.ToString();
                        mCalcList.Add(mSingleCalculation123);

                        // - Profit For Project before financing(Sum of Step 5 above this line(Red is Negative Green is Positive Number for project)
                        CalcParameterOutputModel mSingleCalculation155 = new CalcParameterOutputModel();
                        mSingleCalculation155.ParameterId = "155";
                        mSingleCalculation155.ParameterName = "Profit For Project before Financing";
                        double ProfitForProjectbeforeFinancingPF = TotalBackendSalesPricePF - TotalProjectCostPF - TotalSalesCostPerProjectPF - TotalLandValuePF;
                        mSingleCalculation155.value = ProfitForProjectbeforeFinancingPF.ToString();
                        mCalcList.Add(mSingleCalculation155);

                        // -Total Financing Costs*See Step 6
                        // CalcParameterOutputModel mSingleCalculation162 = new CalcParameterOutputModel();
                        // mSingleCalculation162.ParameterId = "162";
                        // mSingleCalculation162.ParameterName = "Total Financing Cost";
                        // double TotalFinancingCostPF = TotalBackendSalesPricePF - (TotalProjectCostPF + TotalSalesCostPerProjectPF + TotalLandValuePF);
                        // mSingleCalculation162.value = ProfitForProjectbeforeFinancingPF.ToString();
                        // mCalcList.Add(mSingleCalculation162);


                        // - Net Profit For Project(Profit For Project before financing - Total Financing Costs)
                        CalcParameterOutputModel mSingleCalculation159 = new CalcParameterOutputModel();
                        mSingleCalculation159.ParameterId = "159";
                        mSingleCalculation159.ParameterName = "Net Profit For Project";
                        double NetProfitForProjectPF = ProfitForProjectbeforeFinancingPF - TotalFinancingCostPF;
                        mSingleCalculation159.value = NetProfitForProjectPF.ToString();
                        mCalcList.Add(mSingleCalculation159);

                    }

                    else if (mStructType == "Townhome" || mStructType == "Rowhouse" || mStructType == "Cottage")
                    {

                        CalcParameterOutputModel mSingleCalculation30 = new CalcParameterOutputModel();
                        mSingleCalculation30.ParameterId = "30";
                        mSingleCalculation30.ParameterName = "Number of Homes";
                        NumberOfHomes = Math.Round(mParams.LotSize / Convert.ToDouble(mDig));
                        
                        mSingleCalculation30.value = NumberOfHomes.ToString();
                        mCalcList.Add(mSingleCalculation30);

                    // Sellable Sqft.Factor(1.07) Default 96
                        CalcParameterOutputModel mSingleCalculation96 = new CalcParameterOutputModel();
                        mSingleCalculation96.ParameterId = "96";
                        mSingleCalculation96.ParameterName = "Sellable Sqft Factor";
                        double SellableSqftFactor = mSellableSqftFactor;
                        mSingleCalculation96.value = SellableSqftFactor.ToString();
                        mCalcList.Add(mSingleCalculation96);

                        //- Sellable Square Footage. (MAX FAR X Sellable Sqft. Factor / Number of Homes)
                        CalcParameterOutputModel mSingleCalculation67 = new CalcParameterOutputModel();
                        mSingleCalculation67.ParameterId = "67";
                        mSingleCalculation67.ParameterName = "Sellable Square Footage";
                        double SellableSquareFootage = Convert.ToDouble(FARMax) / NumberOfHomes * SellableSqftFactor;
                        mSingleCalculation67.value = SellableSquareFootage.ToString();
                        mCalcList.Add(mSingleCalculation67);

                        //2nd Step(Find out the Construction Costs Breakdown)
                        //-Townhome, Rowhouse or Cottage Base Cost Per Sqft. (Default)
                        double BaseCost = 0;
                        if (mStructType == "Townhome")
                        {
                            CalcParameterOutputModel mSingleCalculation110 = new CalcParameterOutputModel();
                            mSingleCalculation110.ParameterId = "110";
                            mSingleCalculation110.ParameterName = "Townhome Base Cost Per Sqft";
                            double TownhomeBaseCostPerSqft = mTownhomeBaseCostPerSqft;
                            mSingleCalculation110.value = TownhomeBaseCostPerSqft.ToString();
                            mCalcList.Add(mSingleCalculation110);

                            CalcParameterOutputModel mSingleCalculation99 = new CalcParameterOutputModel();
                            mSingleCalculation99.ParameterId = "99";
                            mSingleCalculation99.ParameterName = "Townhome Base Cost";
                            double TownhomeBaseCost = TownhomeBaseCostPerSqft * SellableSquareFootage;
                            mSingleCalculation99.value = TownhomeBaseCost.ToString();
                            mCalcList.Add(mSingleCalculation99);
                            BaseCost = TownhomeBaseCost;
                        }
                        else if (mStructType == "Rowhouse")
                        {
                            CalcParameterOutputModel mSingleCalculation112 = new CalcParameterOutputModel();
                            mSingleCalculation112.ParameterId = "112";
                            mSingleCalculation112.ParameterName = "Rowhouse Base Cost Per Sqft";
                            double RowhouseBaseCostPerSqft = mRowhouseBaseCostPerSqft;
                            mSingleCalculation112.value = RowhouseBaseCostPerSqft.ToString();
                            mCalcList.Add(mSingleCalculation112);

                            CalcParameterOutputModel mSingleCalculation100 = new CalcParameterOutputModel();
                            mSingleCalculation100.ParameterId = "100";
                            mSingleCalculation100.ParameterName = "Rowhouse Base Cost";
                            double RowhouseBaseCost = RowhouseBaseCostPerSqft * SellableSquareFootage;
                            mSingleCalculation100.value = RowhouseBaseCost.ToString();
                            mCalcList.Add(mSingleCalculation100);
                            BaseCost = RowhouseBaseCost;
                        }
                        else if (mStructType == "Cottage")
                        {
                            CalcParameterOutputModel mSingleCalculation111 = new CalcParameterOutputModel();
                            mSingleCalculation111.ParameterId = "111";
                            mSingleCalculation111.ParameterName = "Rowhouse Base Cost Per Sqft";
                            double CottageBaseCostPerSqft = mCottageBaseCostPerSqft;
                            mSingleCalculation111.value = CottageBaseCostPerSqft.ToString();
                            mCalcList.Add(mSingleCalculation111);

                            CalcParameterOutputModel mSingleCalculation101 = new CalcParameterOutputModel();
                            mSingleCalculation101.ParameterId = "101";
                            mSingleCalculation101.ParameterName = "Cottage Base Cost";
                            double CottageBaseCost = CottageBaseCostPerSqft * SellableSquareFootage;
                            mSingleCalculation101.value = CottageBaseCost.ToString();
                            mCalcList.Add(mSingleCalculation101);
                            BaseCost = CottageBaseCost;
                        }

                        // -MHA Cost (MHA Fee Table Price X MAX FAR/Number of Units)
                        CalcParameterOutputModel mSingleCalculation121 = new CalcParameterOutputModel();
                        mSingleCalculation121.ParameterId = "121";
                        mSingleCalculation121.ParameterName = "MHA Cost";
                        double MhaCost = Convert.ToDouble(mHaFee) * Convert.ToDouble(FARMax) / NumberOfHomes;
                        mSingleCalculation121.value = MhaCost.ToString();
                        mCalcList.Add(mSingleCalculation121);

                        //Total Building Cost Per Home(Sum of ALL RED TEXT)
                        CalcParameterOutputModel mSingleCalculation128 = new CalcParameterOutputModel();
                        mSingleCalculation128.ParameterId = "128";
                        mSingleCalculation128.ParameterName = "Total Building Cost Per Home";
                        double TotalBuildingCostPerHomeCB = BaseCost + EcaCost + ElevationGradeCost + WaterLineCost + SewerCost + StormDrainCost + AlleyCost + UndergroundParkingCost + MhaCost;
                        mSingleCalculation128.value = TotalBuildingCostPerHomeCB.ToString();
                        mCalcList.Add(mSingleCalculation128);

                        //Total Project Cost(Total Building Cost Per Lot X Number of Homes)
                        CalcParameterOutputModel mSingleCalculation108 = new CalcParameterOutputModel();
                        mSingleCalculation108.ParameterId = "108";
                        mSingleCalculation108.ParameterName = "Total Project Cost";
                        double TotalProjectCostCB = TotalBuildingCostPerHomeCB * NumberOfHomes;
                        mSingleCalculation108.value = TotalProjectCostCB.ToString();
                        mCalcList.Add(mSingleCalculation108);

                        //3rd Step(Sales Cost Breakdown)
                        //-Home Estimated Value
                        CalcParameterOutputModel mSingleCalculation36 = new CalcParameterOutputModel();
                        mSingleCalculation36.ParameterId = "36";
                        mSingleCalculation36.ParameterName = "Home Estimated Value";
                        double HomeEstimatedValueSCB = mHomeEstimatedValue;
                        mSingleCalculation36.value = HomeEstimatedValueSCB.ToString();
                        mCalcList.Add(mSingleCalculation36);

                        //-Sales Commission(Sales Commission Table X Home Estimated Value)
                        CalcParameterOutputModel mSingleCalculation41 = new CalcParameterOutputModel();
                        mSingleCalculation41.ParameterId = "41";
                        mSingleCalculation41.ParameterName = "Sales Commission";
                        double SalesCommissionSCB = mFactor2 * HomeEstimatedValueSCB;
                        mSingleCalculation41.value = SalesCommissionSCB.ToString();
                        mCalcList.Add(mSingleCalculation41);

                        //- Excise Tax(Excise Tax Table X Home Estimated Value)
                        CalcParameterOutputModel mSingleCalculation42 = new CalcParameterOutputModel();
                        mSingleCalculation42.ParameterId = "42";
                        mSingleCalculation42.ParameterName = "Excise Tax";
                        double ExciseTaxSCB = ExciseOutput(HomeEstimatedValueSCB);
                        mSingleCalculation42.value = ExciseTaxSCB.ToString();
                        mCalcList.Add(mSingleCalculation42);

                        //- Escrow Fees / Insurance(Escrow and Title Fee table X Home Estimated Value)
                        CalcParameterOutputModel mSingleCalculation43 = new CalcParameterOutputModel();
                        mSingleCalculation43.ParameterId = "43";
                        mSingleCalculation43.ParameterName = "Escrow Fees";
                        double EscrowFeesSCB = mFactor4 * HomeEstimatedValueSCB;
                        mSingleCalculation43.value = EscrowFeesSCB.ToString();
                        mCalcList.Add(mSingleCalculation43);

                        //- Marketing / Staging(Marketing and Staging table X Home Estimated Value)
                        CalcParameterOutputModel mSingleCalculation44 = new CalcParameterOutputModel();
                        mSingleCalculation44.ParameterId = "44";
                        mSingleCalculation44.ParameterName = "Marketing and Staging";
                        double MarketingandStagingSCB = mFactor5 * HomeEstimatedValueSCB;
                        mSingleCalculation44.value = MarketingandStagingSCB.ToString();
                        mCalcList.Add(mSingleCalculation44);

                        //- Total Sales Cost per Home(Sum of all blue)
                        CalcParameterOutputModel mSingleCalculation136 = new CalcParameterOutputModel();
                        mSingleCalculation136.ParameterId = "136";
                        mSingleCalculation136.ParameterName = "Total Sales Cost Per Home";
                        double TotalSalesCostPerHomeSCB = SalesCommissionSCB + ExciseTaxSCB + EscrowFeesSCB + MarketingandStagingSCB;
                        mSingleCalculation136.value = TotalSalesCostPerHomeSCB.ToString();
                        mCalcList.Add(mSingleCalculation136);

                        //-Total Sales Cost per Project(Total Sales Cost per Home X Number of Townhomes)
                        CalcParameterOutputModel mSingleCalculation140 = new CalcParameterOutputModel();
                        mSingleCalculation140.ParameterId = "140";
                        mSingleCalculation140.ParameterName = "Total Sales Cost Per Project";
                        double TotalSalesCostPerProjectSCB = TotalSalesCostPerHomeSCB * NumberOfHomes;
                        mSingleCalculation140.value = TotalSalesCostPerProjectSCB.ToString();
                        mCalcList.Add(mSingleCalculation140);

                        //4th Step(Land Analysis)
                        // Home Estimated Value
                        CalcParameterOutputModel mSingleCalculation168 = new CalcParameterOutputModel();
                        mSingleCalculation168.ParameterId = "168";
                        mSingleCalculation168.ParameterName = "Home Estimated Value";
                        double HomeEstimatedValueLA = HomeEstimatedValueSCB;
                        mSingleCalculation168.value = HomeEstimatedValueLA.ToString();
                        mCalcList.Add(mSingleCalculation168);

                        //-Total Building Cost Per Home
                        CalcParameterOutputModel mSingleCalculation169 = new CalcParameterOutputModel();
                        mSingleCalculation169.ParameterId = "169";
                        mSingleCalculation169.ParameterName = "Total Building Cost Per Home";
                        double TotalBuildingCostPerHomeLA = TotalBuildingCostPerHomeCB;
                        mSingleCalculation169.value = TotalBuildingCostPerHomeLA.ToString();
                        mCalcList.Add(mSingleCalculation169);

                        //-Total Sales Cost Per Home
                        CalcParameterOutputModel mSingleCalculation173 = new CalcParameterOutputModel();
                        mSingleCalculation173.ParameterId = "173";
                        mSingleCalculation173.ParameterName = "Total Sales Cost Per Home";
                        double TotalSalesCostPerHomeLA = TotalSalesCostPerHomeSCB;
                        mSingleCalculation173.value = TotalSalesCostPerHomeLA.ToString();
                        mCalcList.Add(mSingleCalculation173);

                        // -Desired Profit Percent
                        CalcParameterOutputModel mSingleCalculation181 = new CalcParameterOutputModel();
                        mSingleCalculation181.ParameterId = "181";
                        mSingleCalculation181.ParameterName = "Desired Profit Percent";
                        double DesiredProfitPercentLA = mDesiredProfitPercent;
                        mSingleCalculation181.value = DesiredProfitPercentLA.ToString();
                        mCalcList.Add(mSingleCalculation181);

                        //-Desired Profit before financing(Desired profit % X Lot Estimated Value)
                        CalcParameterOutputModel mSingleCalculation45 = new CalcParameterOutputModel();
                        mSingleCalculation45.ParameterId = "45";
                        mSingleCalculation45.ParameterName = "Desired Profit Before Financing";
                        double DesiredProfitBeforeFinancingLA = DesiredProfitPercentLA * HomeEstimatedValueLA;
                        mSingleCalculation45.value = DesiredProfitBeforeFinancingLA.ToString();
                        mCalcList.Add(mSingleCalculation45);

                        //- Value of Land Per Home(Sum of Step 4 above this line(Red is Negative Green is Positive Number)
                        CalcParameterOutputModel mSingleCalculation46 = new CalcParameterOutputModel();
                        mSingleCalculation46.ParameterId = "46";
                        mSingleCalculation46.ParameterName = "Value of Land Per Home";
                        double ValueofLandPerHomeLA = HomeEstimatedValueLA - TotalBuildingCostPerHomeLA - TotalSalesCostPerHomeLA - DesiredProfitBeforeFinancingLA;
                        mSingleCalculation46.value = ValueofLandPerHomeLA.ToString();
                        mCalcList.Add(mSingleCalculation46);

                        //- Total Land Value(Value of Land Per Lot X Number of Townhomes Homes)
                        CalcParameterOutputModel mSingleCalculation47 = new CalcParameterOutputModel();
                        mSingleCalculation47.ParameterId = "47";
                        mSingleCalculation47.ParameterName = "Total Land Value";
                        double TotalLandValueLA = ValueofLandPerHomeLA * NumberOfHomes;
                        mSingleCalculation47.value = TotalLandValueLA.ToString();
                        mCalcList.Add(mSingleCalculation47);

                        // Step 5 Financing
                        //Total Project Cost* Step 2
                        CalcParameterOutputModel mSingleCalculation178 = new CalcParameterOutputModel();
                        mSingleCalculation178.ParameterId = "178";
                        mSingleCalculation178.ParameterName = "Total Project Cost";
                        double TotalProjectCostFI = TotalProjectCostCB;
                        mSingleCalculation178.value = TotalProjectCostFI.ToString();
                        mCalcList.Add(mSingleCalculation178);

                        //Total Land Value* Step 4
                        CalcParameterOutputModel mSingleCalculation179 = new CalcParameterOutputModel();
                        mSingleCalculation179.ParameterId = "179";
                        mSingleCalculation179.ParameterName = "Total Land Value";
                        double TotalLandValueFI = TotalLandValueLA;
                        mSingleCalculation179.value = TotalLandValueFI.ToString();
                        mCalcList.Add(mSingleCalculation179);

                        //Equity(Manual Input)
                        CalcParameterOutputModel mSingleCalculation55 = new CalcParameterOutputModel();
                        mSingleCalculation55.ParameterId = "55";
                        mSingleCalculation55.ParameterName = "Equity";
                        double EquityFI = mEquity;
                        mSingleCalculation55.value = EquityFI.ToString();
                        mCalcList.Add(mSingleCalculation55);

                        //Acquisition Carrying Months(Manual Input)
                        CalcParameterOutputModel mSingleCalculation56 = new CalcParameterOutputModel();
                        mSingleCalculation56.ParameterId = "56";
                        mSingleCalculation56.ParameterName = "Acquisition Carrying Months";
                        double AcquisitionCarryingMonthsFI = mAcquisitionCarryingMonths;
                        mSingleCalculation56.value = AcquisitionCarryingMonthsFI.ToString();
                        mCalcList.Add(mSingleCalculation56);

                        //Interest Rate Land Acquisition(Manual Input)
                        CalcParameterOutputModel mSingleCalculation57 = new CalcParameterOutputModel();
                        mSingleCalculation57.ParameterId = "57";
                        mSingleCalculation57.ParameterName = "Interest Rate Land Acquisition";
                        double InterestRateLandAcquisitionFI = mInterestRateLandAcquisition;
                        mSingleCalculation57.value = InterestRateLandAcquisitionFI.ToString();
                        mCalcList.Add(mSingleCalculation57);

                        //Loan Fees Land Acquisition Loan(Manual Input)
                        CalcParameterOutputModel mSingleCalculation81 = new CalcParameterOutputModel();
                        mSingleCalculation81.ParameterId = "81";
                        mSingleCalculation81.ParameterName = "Loan Fees Land Acquisition Loan";
                        double LoanFeesLandAcquisitionLoanFI = mLoanFeesLandAcquisitionLoan;
                        mSingleCalculation81.value = LoanFeesLandAcquisitionLoanFI.ToString();
                        mCalcList.Add(mSingleCalculation81);

                        //Total Financing Cost Land Acquisition((Land Value – Equity) * (Interest Rate Land Acquisition * (Acquisition Carrying Months/ 12) +(Land Value – Equity)*(Loan Fees Land Acquisition Loan)
                        CalcParameterOutputModel mSingleCalculation58 = new CalcParameterOutputModel();
                        mSingleCalculation58.ParameterId = "58";
                        mSingleCalculation58.ParameterName = "Total Financing Cost Land Acquisition";
                        double TotalFinancingCostLandAcquisitionFI = ((TotalLandValueFI - EquityFI) * (InterestRateLandAcquisitionFI * (AcquisitionCarryingMonthsFI / 12)) + ((TotalLandValueFI - EquityFI) * (LoanFeesLandAcquisitionLoanFI)));
                        mSingleCalculation58.value = TotalFinancingCostLandAcquisitionFI.ToString();
                        mCalcList.Add(mSingleCalculation58);

                        //Interest Rate Construction Loan(Manual Default)
                        CalcParameterOutputModel mSingleCalculation59 = new CalcParameterOutputModel();
                        mSingleCalculation59.ParameterId = "59";
                        mSingleCalculation59.ParameterName = "Interest Rate Construction Loan";
                        double InterestRateConstructionLoanFI = mInterestRateConstructionLoan;
                        mSingleCalculation59.value = InterestRateConstructionLoanFI.ToString();
                        mCalcList.Add(mSingleCalculation59);

                        //Loan Fees Construction Loan(Manual Default)
                        CalcParameterOutputModel mSingleCalculation60 = new CalcParameterOutputModel();
                        mSingleCalculation60.ParameterId = "60";
                        mSingleCalculation60.ParameterName = "Loan Fees Construction Loan";
                        double LoanFeesConstructionLoanFI = mLoanFeesConstructionLoan;
                        mSingleCalculation60.value = LoanFeesConstructionLoanFI.ToString();
                        mCalcList.Add(mSingleCalculation60);

                        //Construction Loan Months(Manual Default)
                        CalcParameterOutputModel mSingleCalculation61 = new CalcParameterOutputModel();
                        mSingleCalculation61.ParameterId = "61";
                        mSingleCalculation61.ParameterName = "Construction Loan Months";
                        double ConstructionLoanMonthsFI = mConstructionLoanMonths;
                        mSingleCalculation61.value = ConstructionLoanMonthsFI.ToString();
                        mCalcList.Add(mSingleCalculation61);

                        //Total Financing Construction Cost(((Land Value -Equity) +(Total Project Cost/ 2) *((Interest Rate Construction Loan”) *(Construction Loan Months/ 12)) +((Land Value – Equity) *(Loan Fees Construction Loan”)))
                        CalcParameterOutputModel mSingleCalculation62 = new CalcParameterOutputModel();
                        mSingleCalculation62.ParameterId = "62";
                        mSingleCalculation62.ParameterName = "Total Financing Cost Construction";
                        double TotalFinancingCostConstructionFI = ((((TotalLandValueFI - EquityFI) + (TotalProjectCostFI / 2)) * ((InterestRateConstructionLoanFI) * (ConstructionLoanMonthsFI / 12))) + (((TotalProjectCostFI + TotalLandValueFI - EquityFI) * (LoanFeesConstructionLoanFI))));
                        mSingleCalculation62.value = TotalFinancingCostConstructionFI.ToString();
                        mCalcList.Add(mSingleCalculation62);

                        //Months to Sell after completion(Manual Default)
                        CalcParameterOutputModel mSingleCalculation64 = new CalcParameterOutputModel();
                        mSingleCalculation64.ParameterId = "64";
                        mSingleCalculation64.ParameterName = "Months to Sell After completion";
                        double MonthsToSellAftercompletionFI = mMonthsToSellAfterCompletion;
                        mSingleCalculation64.value = MonthsToSellAftercompletionFI.ToString();
                        mCalcList.Add(mSingleCalculation64);

                        //Extra Financing After Completion(Land Value +Total Project Cost – Contributed Equity) *(Interest Rate Construction Loan * (Months to Sell after completion Months / 12)  
                        CalcParameterOutputModel mSingleCalculation65 = new CalcParameterOutputModel();
                        mSingleCalculation65.ParameterId = "65";
                        mSingleCalculation65.ParameterName = "Extra Financing After Completion";
                        double ExtraFinancingAfterCompletionFI = (TotalLandValueFI + TotalProjectCostFI - EquityFI) * (InterestRateConstructionLoanFI * (MonthsToSellAftercompletionFI / 12));
                        mSingleCalculation65.value = ExtraFinancingAfterCompletionFI.ToString();
                        mCalcList.Add(mSingleCalculation65);

                        //Total Financing Fees(Sum of Green Text)
                        CalcParameterOutputModel mSingleCalculation66 = new CalcParameterOutputModel();
                        mSingleCalculation66.ParameterId = "66";
                        mSingleCalculation66.ParameterName = "Total Financing Fees";
                        double TotalFinancingFeesFI = TotalFinancingCostLandAcquisitionFI + TotalFinancingCostConstructionFI + ExtraFinancingAfterCompletionFI;
                        mSingleCalculation66.value = TotalFinancingFeesFI.ToString();
                        mCalcList.Add(mSingleCalculation66);

                        //Step 6(Proformas & Financing)
                        //PEFORMA PER HOME

                        //-Home Estimated Value
                        CalcParameterOutputModel mSingleCalculation167 = new CalcParameterOutputModel();
                        mSingleCalculation167.ParameterId = "167";
                        mSingleCalculation167.ParameterName = "Home Estimated Value";
                        double HomeEstimatedValuePF = HomeEstimatedValueSCB;
                        mSingleCalculation167.value = HomeEstimatedValuePF.ToString();
                        mCalcList.Add(mSingleCalculation167);

                        // -Total Building Cost Per Home
                        CalcParameterOutputModel mSingleCalculation127 = new CalcParameterOutputModel();
                        mSingleCalculation127.ParameterId = "127";
                        mSingleCalculation127.ParameterName = "Total Building Cost Per Home";
                        double TotalBuildingCostPerHomePF = TotalBuildingCostPerHomeCB;
                        mSingleCalculation127.value = TotalBuildingCostPerHomePF.ToString();
                        mCalcList.Add(mSingleCalculation127);

                        // -Total Sales Cost Per Home
                        CalcParameterOutputModel mSingleCalculation137 = new CalcParameterOutputModel();
                        mSingleCalculation137.ParameterId = "137";
                        mSingleCalculation137.ParameterName = "Total Sales Cost Per Home";
                        double TotalSalesCostPerHomePF = TotalSalesCostPerHomeSCB;
                        mSingleCalculation137.value = TotalSalesCostPerHomePF.ToString();
                        mCalcList.Add(mSingleCalculation137);

                        // -Value of Land Per Home
                        CalcParameterOutputModel mSingleCalculation122 = new CalcParameterOutputModel();
                        mSingleCalculation122.ParameterId = "122";
                        mSingleCalculation122.ParameterName = "Value of Land Per Home";
                        double ValueofLandPerHomePF = ValueofLandPerHomeLA;
                        mSingleCalculation122.value = ValueofLandPerHomePF.ToString();
                        mCalcList.Add(mSingleCalculation122);

                        // -Profit per home before Financing (Sum of Step 5 above this line (Red is Negative Green is Positive Number for per home)
                        CalcParameterOutputModel mSingleCalculation50 = new CalcParameterOutputModel();
                        mSingleCalculation50.ParameterId = "50";
                        mSingleCalculation50.ParameterName = "Profit per home before Financing";
                        double ProfitPerHomeBeforeFinancingPF = HomeEstimatedValuePF - TotalBuildingCostPerHomePF - TotalSalesCostPerHomePF - ValueofLandPerHomePF;
                        mSingleCalculation50.value = ProfitPerHomeBeforeFinancingPF.ToString();
                        mCalcList.Add(mSingleCalculation50);

                        // -Total Financing Costs Per Home (Total financing Cost/Number of Townhomes Homes)
                        CalcParameterOutputModel mSingleCalculation164 = new CalcParameterOutputModel();
                        mSingleCalculation164.ParameterId = "164";
                        mSingleCalculation164.ParameterName = "Total Financing Cost Per Home";
                        double TotalFinancingCostPerHomePF = TotalFinancingFeesFI/ NumberOfHomes;
                        mSingleCalculation164.value = TotalFinancingCostPerHomePF.ToString();
                        mCalcList.Add(mSingleCalculation164);

                        // -Net Profit Per Home (Profit per home before Financing - Financing Costs)
                        CalcParameterOutputModel mSingleCalculation157 = new CalcParameterOutputModel();
                        mSingleCalculation157.ParameterId = "157";
                        mSingleCalculation157.ParameterName = "Net Profit Per Home";
                        double NetProfitPerHomePF = ProfitPerHomeBeforeFinancingPF - TotalFinancingCostPerHomePF;
                        mSingleCalculation157.value = NetProfitPerHomePF.ToString();
                        mCalcList.Add(mSingleCalculation157);

                        // PROFORMA PER PROJECT

                        //- Backend Sales Price
                        CalcParameterOutputModel mSingleCalculation166 = new CalcParameterOutputModel();
                        mSingleCalculation166.ParameterId = "166";
                        mSingleCalculation166.ParameterName = "Total Backend Sales Price";
                        double TotalBackendSalesPricePF = HomeEstimatedValuePF * NumberOfHomes;
                        mSingleCalculation166.value = TotalBackendSalesPricePF.ToString();
                        mCalcList.Add(mSingleCalculation166);

                        // -Total Project Cost
                        CalcParameterOutputModel mSingleCalculation182 = new CalcParameterOutputModel();
                        mSingleCalculation182.ParameterId = "182";
                        mSingleCalculation182.ParameterName = "Total Project Cost";
                        double TotalProjectCostPF = TotalProjectCostFI;
                        mSingleCalculation182.value = TotalProjectCostPF.ToString();
                        mCalcList.Add(mSingleCalculation182);

                        // -Total Sales Cost Per Project
                        CalcParameterOutputModel mSingleCalculation141 = new CalcParameterOutputModel();
                        mSingleCalculation141.ParameterId = "141";
                        mSingleCalculation141.ParameterName = "Total Sales Cost Per Project";
                        double TotalSalesCostPerProjectPF = TotalSalesCostPerProjectSCB;
                        mSingleCalculation141.value = TotalSalesCostPerProjectPF.ToString();
                        mCalcList.Add(mSingleCalculation141);

                        // -Total Land Value *See step 4
                        CalcParameterOutputModel mSingleCalculation123 = new CalcParameterOutputModel();
                        mSingleCalculation123.ParameterId = "123";
                        mSingleCalculation123.ParameterName = "Total Land Value";
                        double TotalLandValuePF = TotalLandValueFI;
                        mSingleCalculation123.value = TotalLandValuePF.ToString();
                        mCalcList.Add(mSingleCalculation123);

                        //-Profit For Project before Financing(Sum of Step 5 above this line(Red is Negative Green is Positive Number for per home)
                        // TODO: is this correct?
                        CalcParameterOutputModel mSingleCalculation155 = new CalcParameterOutputModel();
                        mSingleCalculation155.ParameterId = "155";
                        mSingleCalculation155.ParameterName = "Profit For Project before Financing";
                        double ProfitForProjectBeforeFinancingPFP = TotalBackendSalesPricePF - (TotalProjectCostPF + TotalSalesCostPerProjectPF + TotalLandValuePF);
                        mSingleCalculation155.value = ProfitForProjectBeforeFinancingPFP.ToString();
                        mCalcList.Add(mSingleCalculation155);

                        // -Total Financing Costs* See Step 6
                        CalcParameterOutputModel mSingleCalculation162 = new CalcParameterOutputModel();
                        mSingleCalculation162.ParameterId = "162";
                        mSingleCalculation162.ParameterName = "Total Financing Cost";
                        double TotalFinancingCostPFP = TotalFinancingFeesFI;
                        mSingleCalculation162.value = TotalFinancingCostPFP.ToString();
                        mCalcList.Add(mSingleCalculation162);

                        // -Net Profit For Project (Profit For Project before financing - Total Financing Costs)
                        CalcParameterOutputModel mSingleCalculation159 = new CalcParameterOutputModel();
                        mSingleCalculation159.ParameterId = "159";
                        mSingleCalculation159.ParameterName = "Net Profit For Project";
                        double NetProfitForProjectPFP = ProfitForProjectBeforeFinancingPFP - TotalFinancingCostPFP;
                        mSingleCalculation159.value = NetProfitForProjectPFP.ToString();
                        mCalcList.Add(mSingleCalculation159);
                    }

                    else if (mStructType == "Condo")
                    {

                        CalcParameterOutputModel mSingleCalculation79 = new CalcParameterOutputModel();
                        mSingleCalculation79.ParameterId = "79";
                        mSingleCalculation79.ParameterName = "Inefficient Space Percentage";
                        mSingleCalculation79.value = mInefficientSpacePercentage.ToString();
                        mCalcList.Add(mSingleCalculation79);
                    // - Livable Sqft (MAX FAR) – ((Inefficiency Space Percentage X MAX FAR))
                        CalcParameterOutputModel mSingleCalculation71 = new CalcParameterOutputModel();
                        mSingleCalculation71.ParameterId = "71";
                        mSingleCalculation71.ParameterName = "Livable Sqft";
                        double LivableSqft = Convert.ToDouble(FARMax) - (mInefficientSpacePercentage * Convert.ToDouble(FARMax));
                        mSingleCalculation71.value = LivableSqft.ToString();
                        mCalcList.Add(mSingleCalculation71);

                        // -Number of Units (Lot Size/Minimum Lot size) rounded down to the nearest whole number
                        CalcParameterOutputModel mSingleCalculation80 = new CalcParameterOutputModel();
                        mSingleCalculation80.ParameterId = "80";
                        mSingleCalculation80.ParameterName = "Number Of Units";
                        double NumberofUnits = mParams.LotSize/ Convert.ToDouble(mDig);
                        
                        mSingleCalculation80.value = NumberofUnits.ToString();
                        mCalcList.Add(mSingleCalculation80);

                        // -Sellable square footage. (Livable Sqft. /Number of Units)
                        CalcParameterOutputModel mSingleCalculation67 = new CalcParameterOutputModel();
                        mSingleCalculation67.ParameterId = "67";
                        mSingleCalculation67.ParameterName = "Sellable Square Footage";
                        double SellableSquareFootage = LivableSqft / NumberofUnits;
                        mSingleCalculation67.value = SellableSquareFootage.ToString();
                        mCalcList.Add(mSingleCalculation67);


                        // Step 2 (Find out the Construction Costs Breakdown)

                        // - Condo Base Cost Per Sqft. (Default)
                        CalcParameterOutputModel mSingleCalculation114 = new CalcParameterOutputModel();
                        mSingleCalculation114.ParameterId = "114";
                        mSingleCalculation114.ParameterName = "Condo Base Cost Per Sqft";
                        double CondoBaseCostPerSqft = mCondoBaseCostPerSqft;
                        mSingleCalculation114.value = CondoBaseCostPerSqft.ToString();
                        mCalcList.Add(mSingleCalculation114);

                        // - Condo Base Cost 
                        CalcParameterOutputModel mSingleCalculation103 = new CalcParameterOutputModel();
                        mSingleCalculation103.ParameterId = "103";
                        mSingleCalculation103.ParameterName = "Condo Base Cost";
                        double CondoBaseCost = CondoBaseCostPerSqft ;
                        mSingleCalculation103.value = CondoBaseCost.ToString();
                        mCalcList.Add(mSingleCalculation103);

                        // -MHA Cost (MHA Fee Table Price X MAX FAR/ Number of Units)
                        CalcParameterOutputModel mSingleCalculation121 = new CalcParameterOutputModel();
                        mSingleCalculation121.ParameterId = "121";
                        mSingleCalculation121.ParameterName = "MHA Cost";
                        double MhaCost = Convert.ToDouble(mHaFee) * Convert.ToDouble(FARMax) / NumberofUnits;
                        mSingleCalculation121.value = MhaCost.ToString();
                        mCalcList.Add(mSingleCalculation121);

                        // Total Building Cost Per Unit(Sum of ALL RED TEXT)
                        CalcParameterOutputModel mSingleCalculation126 = new CalcParameterOutputModel();
                        mSingleCalculation126.ParameterId = "126";
                        mSingleCalculation126.ParameterName = "Total Building Cost Per Unit";
                        double TotalBuildingCostPerUnitCB = EcaCost + ElevationGradeCost + WaterLineCost + SewerCost + StormDrainCost + AlleyCost + UndergroundParkingCost + MhaCost;
                        mSingleCalculation126.value = TotalBuildingCostPerUnitCB.ToString();
                        mCalcList.Add(mSingleCalculation126);

                        // Total Project Cost(Total Building Cost Per Unit X Number of Units)
                        CalcParameterOutputModel mSingleCalculation108 = new CalcParameterOutputModel();
                        mSingleCalculation108.ParameterId = "108";
                        mSingleCalculation108.ParameterName = "Total Project Cost";
                        double TotalProjectCostCB = TotalBuildingCostPerUnitCB * NumberofUnits;
                        mSingleCalculation108.value = TotalProjectCostCB.ToString();
                        mCalcList.Add(mSingleCalculation108);

                        // Step 3 Sales Cost Breakdown

                        // Unit Estimated Value
                        CalcParameterOutputModel mSingleCalculation142 = new CalcParameterOutputModel();
                        mSingleCalculation142.ParameterId = "142";
                        mSingleCalculation142.ParameterName = "Unit Estimated Value";
                        double UnitEstimatedValueSCB = mParams.UnitEstimatedValue;
                        mSingleCalculation142.value = UnitEstimatedValueSCB.ToString();
                        mCalcList.Add(mSingleCalculation142);

                        //-Sales Commission(Sales Commission Table X Unit Estimated Value)
                        CalcParameterOutputModel mSingleCalculation41 = new CalcParameterOutputModel();
                        mSingleCalculation41.ParameterId = "41";
                        mSingleCalculation41.ParameterName = "Sales Commission";
                        double SalesCommissionSCB = mFactor2 * UnitEstimatedValueSCB;
                        mSingleCalculation41.value = SalesCommissionSCB.ToString();
                        mCalcList.Add(mSingleCalculation41);

                        //- Excise Tax(Excise Tax Table X Unit Estimated Value)
                        CalcParameterOutputModel mSingleCalculation42 = new CalcParameterOutputModel();
                        mSingleCalculation42.ParameterId = "42";
                        mSingleCalculation42.ParameterName = "Excise Tax";
                        double ExciseTaxSCB = ExciseOutput(UnitEstimatedValueSCB);
                        mSingleCalculation42.value = ExciseTaxSCB.ToString();
                        mCalcList.Add(mSingleCalculation42);

                        //- Escrow Fees / Insurance(Escrow and Title Fee table X Unit Estimated Value)
                        CalcParameterOutputModel mSingleCalculation43 = new CalcParameterOutputModel();
                        mSingleCalculation43.ParameterId = "43";
                        mSingleCalculation43.ParameterName = "Escrow Fees";
                        double EscrowFeesSCB = mFactor4 * UnitEstimatedValueSCB;
                        mSingleCalculation43.value = EscrowFeesSCB.ToString();
                        mCalcList.Add(mSingleCalculation43);

                        //- Marketing / Staging(Marketing and Staging table X Unit Estimated Value)
                        CalcParameterOutputModel mSingleCalculation44 = new CalcParameterOutputModel();
                        mSingleCalculation44.ParameterId = "44";
                        mSingleCalculation44.ParameterName = "Marketing and Staging";
                        double MarketingandStagingSCB = mFactor5 * UnitEstimatedValueSCB;
                        mSingleCalculation44.value = MarketingandStagingSCB.ToString();
                        mCalcList.Add(mSingleCalculation44);

                        //- OCIP Policy(3.5 % of Total Project Cost *)
                        CalcParameterOutputModel mSingleCalculation130 = new CalcParameterOutputModel();
                        mSingleCalculation130.ParameterId = "130";
                        mSingleCalculation130.ParameterName = "OCIP Policy";
                        double OCIPPolicySCB = 0.035 * TotalProjectCostCB;
                        mSingleCalculation130.value = OCIPPolicySCB.ToString();
                        mCalcList.Add(mSingleCalculation130);

                        //- Total Sales Cost per Unit(Sum of all blue)
                        CalcParameterOutputModel mSingleCalculation138 = new CalcParameterOutputModel();
                        mSingleCalculation138.ParameterId = "138";
                        mSingleCalculation138.ParameterName = "Total Sales Cost Per Unit";
                        double TotalSalesCostPerUnitSCB = SalesCommissionSCB + ExciseTaxSCB + EscrowFeesSCB + MarketingandStagingSCB + OCIPPolicySCB;
                        mSingleCalculation138.value = TotalSalesCostPerUnitSCB.ToString();
                        mCalcList.Add(mSingleCalculation138);

                        //-Total Sales Cost per Project(Total Sales Cost per Unit X Number of Units)
                        CalcParameterOutputModel mSingleCalculation140 = new CalcParameterOutputModel();
                        mSingleCalculation140.ParameterId = "140";
                        mSingleCalculation140.ParameterName = "Total Sales Cost Per Project";
                        double TotalSalesCostPerProjectSCB = TotalSalesCostPerUnitSCB * NumberofUnits;
                        mSingleCalculation140.value = TotalSalesCostPerUnitSCB.ToString();
                        mCalcList.Add(mSingleCalculation140);

                        // Land Analysis
                        //-Unit Estimated Value 144 "Unit Estimated Value"
                        CalcParameterOutputModel mSingleCalculation144 = new CalcParameterOutputModel();
                        mSingleCalculation144.ParameterId = "144";
                        mSingleCalculation144.ParameterName = "Unit Estimated Value";
                        double UnitEstimatedValueLA = mUnitEstimatedValue;
                        mSingleCalculation144.value = UnitEstimatedValueLA.ToString();
                        mCalcList.Add(mSingleCalculation144);

                        //-Total Building Cost Per Unit 176 "Total Building Cost Per Unit"
                        CalcParameterOutputModel mSingleCalculation176 = new CalcParameterOutputModel();
                        mSingleCalculation176.ParameterId = "176";
                        mSingleCalculation176.ParameterName = "Total Building Cost Per Unit";
                        double TotalBuildingCostPerUnitLA = TotalBuildingCostPerUnitCB;
                        mSingleCalculation176.value = TotalBuildingCostPerUnitLA.ToString();
                        mCalcList.Add(mSingleCalculation176);

                        //-Total Sales Cost Per Unit 174 "Total Sales Cost Per Unit"
                        CalcParameterOutputModel mSingleCalculation174 = new CalcParameterOutputModel();
                        mSingleCalculation174.ParameterId = "174";
                        mSingleCalculation174.ParameterName = "Total Sales Cost Per Unit";
                        double TotalSalesCostPerUnitLA = TotalBuildingCostPerUnitCB;
                        mSingleCalculation174.value = TotalSalesCostPerUnitLA.ToString();
                        mCalcList.Add(mSingleCalculation174);

                        // -Desired Profit Percent
                        CalcParameterOutputModel mSingleCalculation181 = new CalcParameterOutputModel();
                        mSingleCalculation181.ParameterId = "181";
                        mSingleCalculation181.ParameterName = "Desired Profit Percent";
                        double DesiredProfitPercentLA = mDesiredProfitPercent;
                        mSingleCalculation181.value = DesiredProfitPercentLA.ToString();
                        mCalcList.Add(mSingleCalculation181);

                        //-Desired Profit before financing(Desired profit % X Backend Sales Price) 45 "Desired Profit Before Financing"
                        CalcParameterOutputModel mSingleCalculation45 = new CalcParameterOutputModel();
                        mSingleCalculation45.ParameterId = "45";
                        mSingleCalculation45.ParameterName = "Desired Profit Before Financing";
                        double DesiredProfitBeforeFinancingLA = DesiredProfitPercentLA * UnitEstimatedValueLA;
                        mSingleCalculation45.value = DesiredProfitBeforeFinancingLA.ToString();
                        mCalcList.Add(mSingleCalculation45);

                        //- Value of Land Per Unit(Sum of Step 4 above this line(Red is Negative Green is Positive Number) 150 "Value of Land Per Unit"
                        CalcParameterOutputModel mSingleCalculation150 = new CalcParameterOutputModel();
                        mSingleCalculation150.ParameterId = "150";
                        mSingleCalculation150.ParameterName = "Value of Land Per Unit";
                        double ValueofLandPerUnitLA = UnitEstimatedValueLA - (TotalBuildingCostPerUnitLA + TotalSalesCostPerUnitLA + DesiredProfitBeforeFinancingLA);
                        mSingleCalculation150.value = ValueofLandPerUnitLA.ToString();
                        mCalcList.Add(mSingleCalculation150);

                        //- Total Land Value(Value of Land Per Unit X Number of Units) 47 "Total Land Value"
                        CalcParameterOutputModel mSingleCalculation47 = new CalcParameterOutputModel();
                        mSingleCalculation47.ParameterId = "47";
                        mSingleCalculation47.ParameterName = "Total Land Value";
                        double TotalLandValueLA = ValueofLandPerUnitLA * NumberofUnits;
                        mSingleCalculation47.value = TotalLandValueLA.ToString();
                        mCalcList.Add(mSingleCalculation47);

                        // Step 5 Financing
                        // PERFORMA PER PROJECT
                        // Total Project Cost* Step 2  
                        CalcParameterOutputModel mSingleCalculation178 = new CalcParameterOutputModel();
                        mSingleCalculation178.ParameterId = "178";
                        mSingleCalculation178.ParameterName = "Total Project Cost";
                        double TotalProjectCostFI = TotalProjectCostCB;
                        mSingleCalculation178.value = TotalProjectCostFI.ToString();
                        mCalcList.Add(mSingleCalculation178);

                        // Total Land Value* Step 4
                        CalcParameterOutputModel mSingleCalculation179 = new CalcParameterOutputModel();
                        mSingleCalculation179.ParameterId = "179";
                        mSingleCalculation179.ParameterName = "Total Land Value";
                        double TotalLandValuePF = TotalLandValueLA;
                        mSingleCalculation179.value = TotalLandValuePF.ToString();
                        mCalcList.Add(mSingleCalculation179);

                        // Equity(Manual Input)
                        CalcParameterOutputModel mSingleCalculation55 = new CalcParameterOutputModel();
                        mSingleCalculation55.ParameterId = "55";
                        mSingleCalculation55.ParameterName = "Equity";
                        double EquityPF = mEquity;
                        mSingleCalculation55.value = EquityPF.ToString();
                        mCalcList.Add(mSingleCalculation55);

                        // Acquisition Carrying Months(Manual Input)
                        CalcParameterOutputModel mSingleCalculation56 = new CalcParameterOutputModel();
                        mSingleCalculation56.ParameterId = "56";
                        mSingleCalculation56.ParameterName = "Acquisition Carrying Months";
                        double AcquisitionCarryingMonthsPF = mAcquisitionCarryingMonths;
                        mSingleCalculation56.value = AcquisitionCarryingMonthsPF.ToString();
                        mCalcList.Add(mSingleCalculation56);

                        // Interest Rate Land Acquisition(Manual Input)
                        CalcParameterOutputModel mSingleCalculation57 = new CalcParameterOutputModel();
                        mSingleCalculation57.ParameterId = "57";
                        mSingleCalculation57.ParameterName = "Interest Rate Land Acquisition";
                        double InterestRateLandAcquisitionPF = mInterestRateLandAcquisition;
                        mSingleCalculation57.value = InterestRateLandAcquisitionPF.ToString();
                        mCalcList.Add(mSingleCalculation57);

                        // Loan Fees Land Acquisition Loan(Manual Input)
                        CalcParameterOutputModel mSingleCalculation81 = new CalcParameterOutputModel();
                        mSingleCalculation81.ParameterId = "81";
                        mSingleCalculation81.ParameterName = "Loan Fees Land Acquisition Loan";
                        double LoanFeesLandAcquisitionLoanPF = mLoanFeesLandAcquisitionLoan;
                        mSingleCalculation81.value = LoanFeesLandAcquisitionLoanPF.ToString();
                        mCalcList.Add(mSingleCalculation81);

                        // Total Financing Cost Land Acquisition((Land Value – Equity) * (Interest Rate Land Acquisition * (Acquisition Carrying Months/ 12) +(Land Value – Equity)*(Loan Fees Land Acquisition Loan)
                        CalcParameterOutputModel mSingleCalculation58 = new CalcParameterOutputModel();
                        mSingleCalculation58.ParameterId = "58";
                        mSingleCalculation58.ParameterName = "Total Financing Cost Land Acquisition";
                        double TotalFinancingCostLandAcquisitionPF = ((TotalLandValuePF - EquityPF) * (InterestRateLandAcquisitionPF * (AcquisitionCarryingMonthsPF / 12)) + ((TotalLandValuePF - EquityPF) * (LoanFeesLandAcquisitionLoanPF)));
                        mSingleCalculation58.value = TotalFinancingCostLandAcquisitionPF.ToString();
                        mCalcList.Add(mSingleCalculation58);

                        // Interest Rate Construction Loan(Manual Default)
                        CalcParameterOutputModel mSingleCalculation59 = new CalcParameterOutputModel();
                        mSingleCalculation59.ParameterId = "59";
                        mSingleCalculation59.ParameterName = "Interest Rate Construction Loan";
                        double InterestRateConstructionLoan = mInterestRateConstructionLoan;
                        mSingleCalculation59.value = InterestRateConstructionLoan.ToString();
                        mCalcList.Add(mSingleCalculation59);

                        // Loan Fees Construction Loan(Manual Default)
                        CalcParameterOutputModel mSingleCalculation60 = new CalcParameterOutputModel();
                        mSingleCalculation60.ParameterId = "60";
                        mSingleCalculation60.ParameterName = "Loan Fees Construction Loan";
                        double LoanFeesConstructionLoanPF = mLoanFeesConstructionLoan;
                        mSingleCalculation60.value = LoanFeesConstructionLoanPF.ToString();
                        mCalcList.Add(mSingleCalculation60);

                        // Construction Loan Months(Manual Default)
                        CalcParameterOutputModel mSingleCalculation61 = new CalcParameterOutputModel();
                        mSingleCalculation61.ParameterId = "61";
                        mSingleCalculation61.ParameterName = "Construction Loan Months";
                        double ConstructionLoanMonthsFI = mConstructionLoanMonths;
                        mSingleCalculation61.value = ConstructionLoanMonthsFI.ToString();
                        mCalcList.Add(mSingleCalculation61);

                        // Total Financing Construction Cost(((Land Value -Equity) +(Total Project Cost/ 2) *((Interest Rate Construction Loan”) *(Construction Loan Months/ 12)) +((Land Value – Equity) *(Loan Fees Construction Loan”)))
                        CalcParameterOutputModel mSingleCalculation62 = new CalcParameterOutputModel();
                        mSingleCalculation62.ParameterId = "62";
                        mSingleCalculation62.ParameterName = "Total Financing Cost Construction";
                        double TotalFinancingCostConstructionFI = ((((TotalLandValuePF - EquityPF) + (TotalProjectCostFI / 2)) * ((InterestRateConstructionLoan) * (ConstructionLoanMonthsFI / 12))) + (((TotalProjectCostFI + TotalLandValuePF - EquityPF) * (LoanFeesConstructionLoanPF))));
                        mSingleCalculation62.value = TotalFinancingCostConstructionFI.ToString();
                        mCalcList.Add(mSingleCalculation62);

                        // Months to Sell after completion(Manual Default)
                        CalcParameterOutputModel mSingleCalculation64 = new CalcParameterOutputModel();
                        mSingleCalculation64.ParameterId = "64";
                        mSingleCalculation64.ParameterName = "Months to Sell After completion";
                        double MonthstoSellAfterCompletionPF = mMonthsToSellAfterCompletion;
                        mSingleCalculation64.value = MonthstoSellAfterCompletionPF.ToString();
                        mCalcList.Add(mSingleCalculation64);

                        // Extra Financing After Completion(Land Value +Total Project Cost – Contributed Equity) *(Interest Rate Construction Loan * (Months to Sell after completion Months / 12)  
                        CalcParameterOutputModel mSingleCalculation65 = new CalcParameterOutputModel();
                        mSingleCalculation65.ParameterId = "65";
                        mSingleCalculation65.ParameterName = "Extra Financing After Completion";
                        double ExtraFinancingAfterCompletionPF = (TotalLandValuePF + TotalProjectCostCB - EquityPF) * (InterestRateConstructionLoan * (MonthstoSellAfterCompletionPF / 12));
                        mSingleCalculation65.value = ExtraFinancingAfterCompletionPF.ToString();
                        mCalcList.Add(mSingleCalculation65);

                        // Total Financing Fees(Sum of Green Text)
                        CalcParameterOutputModel mSingleCalculation66 = new CalcParameterOutputModel();
                        mSingleCalculation66.ParameterId = "66";
                        mSingleCalculation66.ParameterName = "Total Financing Fees";
                        double TotalFinancingFeesFI = TotalFinancingCostLandAcquisitionPF + TotalFinancingCostConstructionFI + ExtraFinancingAfterCompletionPF;
                        mSingleCalculation66.value = TotalFinancingFeesFI.ToString();
                        mCalcList.Add(mSingleCalculation66);

                        //Step 6 (Proformas & Financing)
                        //PEFORMA PER CONDO
                        //- Estimated Unit Value
                        CalcParameterOutputModel mSingleCalculation143 = new CalcParameterOutputModel();
                        mSingleCalculation143.ParameterId = "143";
                        mSingleCalculation143.ParameterName = "Unit Estimated Value";
                        double UnitEstimatedValueFI = UnitEstimatedValueLA;
                        mSingleCalculation143.value = UnitEstimatedValueFI.ToString();
                        mCalcList.Add(mSingleCalculation143);

                        //-Total Building Cost Per Unit 125 "Total Building Cost Per Unit"
                        CalcParameterOutputModel mSingleCalculation125 = new CalcParameterOutputModel();
                        mSingleCalculation125.ParameterId = "125";
                        mSingleCalculation125.ParameterName = "Total Building Cost Per Unit";
                        double TotalBuildingCostPerUnitFI = TotalBuildingCostPerUnitLA;
                        mSingleCalculation125.value = TotalBuildingCostPerUnitFI.ToString();
                        mCalcList.Add(mSingleCalculation125);

                        //-Total Sales Cost Per Unit  139 "Total Sales Cost Per Unit"
                        CalcParameterOutputModel mSingleCalculation139 = new CalcParameterOutputModel();
                        mSingleCalculation139.ParameterId = "139";
                        mSingleCalculation139.ParameterName = "Total Sales Cost Per Unit";
                        double TotalSalesCostPerUnitPF = TotalSalesCostPerUnitLA;
                        mSingleCalculation139.value = TotalSalesCostPerUnitPF.ToString();
                        mCalcList.Add(mSingleCalculation139);

                        //-Value of Land Per Condo 151 "Value of Land Per Unit"
                        CalcParameterOutputModel mSingleCalculation151 = new CalcParameterOutputModel();
                        mSingleCalculation151.ParameterId = "151";
                        mSingleCalculation151.ParameterName = "Value of Land Per Unit";
                        double ValueofLandPerLotFI = ValueofLandPerUnitLA;
                        mSingleCalculation151.value = ValueofLandPerLotFI.ToString();
                        mCalcList.Add(mSingleCalculation151);

                        //-Profit per unit before Financing(Sum of Step 5 above this line(Red is Negative Green is Positive Number for per home) 154 "Profit Per Unit before Financing"
                        CalcParameterOutputModel mSingleCalculation154 = new CalcParameterOutputModel();
                        mSingleCalculation154.ParameterId = "154";
                        mSingleCalculation154.ParameterName = "Profit per unit before Financing";
                        double ProfitperunitbeforeFinancingPF = UnitEstimatedValueFI - (TotalBuildingCostPerUnitFI + TotalSalesCostPerUnitPF + ValueofLandPerLotFI);
                        mSingleCalculation154.value = ProfitperunitbeforeFinancingPF.ToString();
                        mCalcList.Add(mSingleCalculation154);

                        //-Financing Costs(Total financing Cost / Number of Condos Homes) 183 "Total Financing Cost Per Unit"
                        CalcParameterOutputModel mSingleCalculation183 = new CalcParameterOutputModel();
                        mSingleCalculation183.ParameterId = "183";
                        mSingleCalculation183.ParameterName = "Total Financing Cost Per Unit";
                        double TotalFinancingCostPerUnitPF = UnitEstimatedValueFI - (TotalBuildingCostPerUnitFI + TotalSalesCostPerUnitPF + ValueofLandPerLotFI);
                        mSingleCalculation183.value = ProfitperunitbeforeFinancingPF.ToString();
                        mCalcList.Add(mSingleCalculation183);

                        //- Net Profit Per Unit(Profit per unit before Financing - Financing Costs) 158 "Net Profit Per Unit"
                        CalcParameterOutputModel mSingleCalculation158 = new CalcParameterOutputModel();
                        mSingleCalculation158.ParameterId = "158";
                        mSingleCalculation158.ParameterName = "Net Profit Per Unit";
                        double NetProfitPerUnitPF = ProfitperunitbeforeFinancingPF - TotalFinancingCostPerUnitPF;
                        mSingleCalculation158.value = NetProfitPerUnitPF.ToString();
                        mCalcList.Add(mSingleCalculation158);

                        // PERFORMA PER PROJECT
                        //- Total Backend Sales Price(Backend Sales Price X Number of Condo) 166 "Total Backend Sales Price"
                        CalcParameterOutputModel mSingleCalculation166 = new CalcParameterOutputModel();
                        mSingleCalculation166.ParameterId = "166";
                        mSingleCalculation166.ParameterName = "Total Backend Sales Price";
                        double TotalBackendSalesPricePF = UnitEstimatedValueFI * NumberofUnits;
                        mSingleCalculation166.value = TotalBackendSalesPricePF.ToString();
                        mCalcList.Add(mSingleCalculation166);

                        //- Total Project Cost *See step 2 182 "Total Project Cost"
                        CalcParameterOutputModel mSingleCalculation182 = new CalcParameterOutputModel();
                        mSingleCalculation182.ParameterId = "182";
                        mSingleCalculation182.ParameterName = "Total Project Cost";
                        double TotalProjectCostPF = TotalProjectCostCB;
                        mSingleCalculation182.value = TotalProjectCostPF.ToString();
                        mCalcList.Add(mSingleCalculation182);

                        //- Total Sales Cost Per Project *See step 3 141 "Total Sales Cost Per Project"
                        CalcParameterOutputModel mSingleCalculation141 = new CalcParameterOutputModel();
                        mSingleCalculation141.ParameterId = "141";
                        mSingleCalculation141.ParameterName = "Total Sales Cost Per Project";
                        double TotalSalesCostPerProjectPF = TotalSalesCostPerProjectSCB;
                        mSingleCalculation141.value = TotalSalesCostPerProjectPF.ToString();
                        mCalcList.Add(mSingleCalculation141);

                        //- Total Land Value *See step 4 123 "Total Land Value"
                        CalcParameterOutputModel mSingleCalculation123 = new CalcParameterOutputModel();
                        mSingleCalculation123.ParameterId = "123";
                        mSingleCalculation123.ParameterName = "Total Land Value";
                        double TotalLandValuePFP = TotalLandValuePF;
                        mSingleCalculation123.value = TotalLandValuePFP.ToString();
                        mCalcList.Add(mSingleCalculation123);

                        //- Profit For Project before financing(Sum of Step 5 above this line(Red is Negative Green is Positive Number for project) 155 "Profit For Project before Financing"
                        CalcParameterOutputModel mSingleCalculation155 = new CalcParameterOutputModel();
                        mSingleCalculation155.ParameterId = "155";
                        mSingleCalculation155.ParameterName = "Profit For Project before Financing";
                        double ProfitForProjectbeforeFinancingPFP = TotalBackendSalesPricePF - (TotalProjectCostPF + TotalSalesCostPerProjectPF + TotalLandValuePFP);
                        mSingleCalculation155.value = ProfitForProjectbeforeFinancingPFP.ToString();
                        mCalcList.Add(mSingleCalculation155);

                        //-Total Financing Costs*See Step 6 162 "Total Financing Cost"
                        CalcParameterOutputModel mSingleCalculation162 = new CalcParameterOutputModel();
                        mSingleCalculation162.ParameterId = "162";
                        mSingleCalculation162.ParameterName = "Total Financing Cost";
                        double TotalFinancingCostPFP = TotalFinancingFeesFI;
                        mSingleCalculation162.value = TotalFinancingCostPFP.ToString();
                        mCalcList.Add(mSingleCalculation162);

                        //- Net Profit For Project(Profit For Project before financing - Total Financing Costs) 159 "Net Profit For Project" 
                        CalcParameterOutputModel mSingleCalculation159 = new CalcParameterOutputModel();
                        mSingleCalculation159.ParameterId = "159";
                        mSingleCalculation159.ParameterName = "Net Profit For Project";
                        double NetProfitForProjectPFP = ProfitForProjectbeforeFinancingPFP - TotalFinancingCostPFP;
                        mSingleCalculation159.value = NetProfitForProjectPFP.ToString();
                        mCalcList.Add(mSingleCalculation159);
                    }

                    else if (mStructType == "Apartment")
                    {
                        // Step 1
                        // - Livable Sqft (MAX FAR) – ((Inefficiency Space Percentage X MAX FAR))
                        CalcParameterOutputModel mSingleCalculation71 = new CalcParameterOutputModel();
                        mSingleCalculation71.ParameterId = "71";
                        mSingleCalculation71.ParameterName = "Livable Sqft";
                        double LivableSqft = Convert.ToDouble(FARMax) - (mInefficientSpacePercentage * Convert.ToDouble(FARMax));
                        mSingleCalculation71.value = LivableSqft.ToString();
                        mCalcList.Add(mSingleCalculation71);

                        CalcParameterOutputModel mSingleCalculation79 = new CalcParameterOutputModel();
                        mSingleCalculation79.ParameterId = "79";
                        mSingleCalculation79.ParameterName = "Inefficient Space Percentage";                      
                        mSingleCalculation79.value = mInefficientSpacePercentage.ToString();
                        mCalcList.Add(mSingleCalculation79);

                    // -Number of Units (Lot Size/Minimum Lot size) rounded down to the nearest whole number
                        CalcParameterOutputModel mSingleCalculation80 = new CalcParameterOutputModel();
                        mSingleCalculation80.ParameterId = "80";
                        mSingleCalculation80.ParameterName = "Number Of Units";
                        double NumberofUnits = mParams.LotSize / Convert.ToDouble(mDig);
                       
                        mSingleCalculation80.value = NumberofUnits.ToString();
                        mCalcList.Add(mSingleCalculation80);

                        // -Sellable square footage. (Livable Sqft. /Number of Units)
                        CalcParameterOutputModel mSingleCalculation67 = new CalcParameterOutputModel();
                        mSingleCalculation67.ParameterId = "67";
                        mSingleCalculation67.ParameterName = "Sellable Square Footage";
                        double SellableSquareFootage = LivableSqft / NumberofUnits;
                        mSingleCalculation67.value = SellableSquareFootage.ToString();
                        mCalcList.Add(mSingleCalculation67);


                        // Step 2 (Find out the Construction Costs Breakdown)

                        // - Apartment Base Cost Per Sqft
                        CalcParameterOutputModel mSingleCalculation113 = new CalcParameterOutputModel();
                        mSingleCalculation113.ParameterId = "113";
                        mSingleCalculation113.ParameterName = "Apartment Base Cost Per Sqft";
                        double ApartmentBaseCostPerSqft = mApartmentBaseCostPerSqft;
                        mSingleCalculation113.value = ApartmentBaseCostPerSqft.ToString();
                        mCalcList.Add(mSingleCalculation113);

                        // - Apartment Base Cost (Apartment Base Cost Per Sqft. X Sellable Square Footage)
                        CalcParameterOutputModel mSingleCalculation102 = new CalcParameterOutputModel();
                        mSingleCalculation102.ParameterId = "102";
                        mSingleCalculation102.ParameterName = "Apartment Base Cost";
                        double ApartmentBaseCost = ApartmentBaseCostPerSqft * SellableSquareFootage;
                        mSingleCalculation102.value = ApartmentBaseCost.ToString();
                        mCalcList.Add(mSingleCalculation102);

                        // -MHA Cost (MHA Fee Table Price X MAX FAR/ Number of Units)
                        CalcParameterOutputModel mSingleCalculation121 = new CalcParameterOutputModel();
                        mSingleCalculation121.ParameterId = "121";
                        mSingleCalculation121.ParameterName = "MHA Cost";
                        double MhaCost = Convert.ToDouble(mHaFee) * Convert.ToDouble(FARMax) / NumberofUnits;
                        mSingleCalculation121.value = MhaCost.ToString();
                        mCalcList.Add(mSingleCalculation121);

                        // Total Building Cost Per Unit(Sum of ALL RED TEXT)
                        CalcParameterOutputModel mSingleCalculation126 = new CalcParameterOutputModel();
                        mSingleCalculation126.ParameterId = "126";
                        mSingleCalculation126.ParameterName = "Total Building Cost Per Unit";
                        double TotalBuildingCostPerUnitCB = EcaCost + ElevationGradeCost + WaterLineCost + SewerCost + StormDrainCost + AlleyCost + UndergroundParkingCost + MhaCost;
                        mSingleCalculation126.value = TotalBuildingCostPerUnitCB.ToString();
                        mCalcList.Add(mSingleCalculation126);

                        // Total Project Cost(Total Building Cost Per Unit X Number of Units)
                        CalcParameterOutputModel mSingleCalculation108 = new CalcParameterOutputModel();
                        mSingleCalculation108.ParameterId = "108";
                        mSingleCalculation108.ParameterName = "Total Project Cost";
                        double TotalProjectCostCB = TotalBuildingCostPerUnitCB * NumberofUnits;
                        mSingleCalculation108.value = TotalProjectCostCB.ToString();
                        mCalcList.Add(mSingleCalculation108);

                        // Step 3 (Sales Cost Breakdown)
                        // -Apartment Cap Rate(Default 3 %)
                        CalcParameterOutputModel mSingleCalculation76 = new CalcParameterOutputModel();
                        mSingleCalculation76.ParameterId = "76";
                        mSingleCalculation76.ParameterName = "Apartment Cap Rate";
                        double ApartmentCapRateSCB = mApartmentCapRate;
                        mSingleCalculation76.value = ApartmentCapRateSCB.ToString();
                        mCalcList.Add(mSingleCalculation76);

                        // -Monthly Rent Per Unit(Manual Input) Monthly Rent Per Unit
                        CalcParameterOutputModel mSingleCalculation72 = new CalcParameterOutputModel();
                        mSingleCalculation72.ParameterId = "72";
                        mSingleCalculation72.ParameterName = "Monthly Rent Per Unit";
                        double MonthlyRentPerUnitSCB = mMonthlyRentPerUnit;
                        mSingleCalculation72.value = MonthlyRentPerUnitSCB.ToString();
                        mCalcList.Add(mSingleCalculation72);

                        // -Annual Rent(Average Rent Per Unit X 12)
                        CalcParameterOutputModel mSingleCalculation73 = new CalcParameterOutputModel();
                        mSingleCalculation73.ParameterId = "73";
                        mSingleCalculation73.ParameterName = "Annual Rent";
                        double AnnualRentSCB = mMonthlyRentPerUnit * 12;
                        mSingleCalculation73.value = AnnualRentSCB.ToString();
                        mCalcList.Add(mSingleCalculation73);

                        // -Vacancy Rate(Default to 5 %)
                        CalcParameterOutputModel mSingleCalculation74 = new CalcParameterOutputModel();
                        mSingleCalculation74.ParameterId = "74";
                        mSingleCalculation74.ParameterName = "Vacancy Rate";
                        double VacancyRate = mVacancyRate;
                        mSingleCalculation74.value = VacancyRate.ToString();
                        mCalcList.Add(mSingleCalculation74);

                        // -Total Vacancy Allowance(Vacancy Rate X Annual Rent)
                        CalcParameterOutputModel mSingleCalculation131 = new CalcParameterOutputModel();
                        mSingleCalculation131.ParameterId = "131";
                        mSingleCalculation131.ParameterName = "Total Vacancy Allowance";
                        double TotalVacancyAllowance = AnnualRentSCB * VacancyRate;
                        mSingleCalculation131.value = TotalVacancyAllowance.ToString();
                        mCalcList.Add(mSingleCalculation131);

                        // -Apartment Gross Income(Annual Rent – Total Vacancy Allowance)
                        CalcParameterOutputModel mSingleCalculation132 = new CalcParameterOutputModel();
                        mSingleCalculation132.ParameterId = "132";
                        mSingleCalculation132.ParameterName = "Apartment Gross Income";
                        double ApartmentGrossIncome = AnnualRentSCB - TotalVacancyAllowance;
                        mSingleCalculation132.value = ApartmentGrossIncome.ToString();
                        mCalcList.Add(mSingleCalculation132);

                        // -Operating Expense per sqft(Default $10)
                        CalcParameterOutputModel mSingleCalculation77 = new CalcParameterOutputModel();
                        mSingleCalculation77.ParameterId = "77";
                        mSingleCalculation77.ParameterName = "Operating Expenses Per Foot";
                        double OperatingExpensesPerFoot = mOperatingExpensesPerFoot;
                        mSingleCalculation77.value = OperatingExpensesPerFoot.ToString();
                        mCalcList.Add(mSingleCalculation77);

                        // -Total Operating Expense(Operating Expense per sqft. XMax FAR) *Step 1
                        CalcParameterOutputModel mSingleCalculation78 = new CalcParameterOutputModel();
                        mSingleCalculation78.ParameterId = "78";
                        mSingleCalculation78.ParameterName = "Operating Expenses";
                        double OperatingExpenses = mOperatingExpensesPerFoot;
                        mSingleCalculation78.value = OperatingExpenses.ToString();
                        mCalcList.Add(mSingleCalculation78);

                        // -Yearly Apartment Taxes(Default to $50,000)
                        CalcParameterOutputModel mSingleCalculation75 = new CalcParameterOutputModel();
                        mSingleCalculation75.ParameterId = "75";
                        mSingleCalculation75.ParameterName = "Yearly Apartment Taxes";
                        double YearlyApartmentTaxes = mYearlyApartmentTaxes;
                        mSingleCalculation75.value = YearlyApartmentTaxes.ToString();
                        mCalcList.Add(mSingleCalculation75);

                        // -Net Income(Step 3 Green text minus Red text)
                        CalcParameterOutputModel mSingleCalculation152 = new CalcParameterOutputModel();
                        mSingleCalculation152.ParameterId = "152";
                        mSingleCalculation152.ParameterName = "Net Income";
                        double NetIncome = ApartmentGrossIncome - (OperatingExpenses + YearlyApartmentTaxes);
                        mSingleCalculation152.value = NetIncome.ToString();
                        mCalcList.Add(mSingleCalculation152);

                        // -Apartment Estimated Value(Net Income/ Apartment Cap Rate)
                        CalcParameterOutputModel mSingleCalculation69 = new CalcParameterOutputModel();
                        mSingleCalculation69.ParameterId = "69";
                        mSingleCalculation69.ParameterName = "Apartment Estimated Value";
                        double ApartmentEstimatedValue = NetIncome / ApartmentCapRateSCB;
                        mSingleCalculation69.value = ApartmentEstimatedValue.ToString();
                        mCalcList.Add(mSingleCalculation69);

                        // -Unit Estimated Value(Apartment Estimated Value / Number of Units)
                        CalcParameterOutputModel mSingleCalculation142 = new CalcParameterOutputModel();
                        mSingleCalculation142.ParameterId = "142";
                        mSingleCalculation142.ParameterName = "Unit Estimated Value";
                        double UnitEstimatedValue = ApartmentEstimatedValue / NumberofUnits;
                        mSingleCalculation142.value = UnitEstimatedValue.ToString();
                        mCalcList.Add(mSingleCalculation142);

                        // -Sales Commission(Sales Commission Table * Apartment Estimated Value / Number of Units)
                        CalcParameterOutputModel mSingleCalculation41 = new CalcParameterOutputModel();
                        mSingleCalculation41.ParameterId = "41";
                        mSingleCalculation41.ParameterName = "Sales Commission";
                        double SalesCommission = 0.06 * ApartmentEstimatedValue / NumberofUnits;
                        mSingleCalculation41.value = SalesCommission.ToString();
                        mCalcList.Add(mSingleCalculation41);

                        // -Excise Tax (Excise Tax Table* Apartment Estimated Value / Number of Units )
                        CalcParameterOutputModel mSingleCalculation42 = new CalcParameterOutputModel();
                        mSingleCalculation42.ParameterId = "42";
                        mSingleCalculation42.ParameterName = "Excise Tax";
                        double ExciseTaxSCB = ExciseOutput(ApartmentEstimatedValue)/NumberofUnits;
                        mSingleCalculation42.value = ExciseTaxSCB.ToString();
                        mCalcList.Add(mSingleCalculation42);

                        //- Escrow Fees / Insurance(Escrow and Title Fee table X Unit Estimated Value)
                        CalcParameterOutputModel mSingleCalculation43 = new CalcParameterOutputModel();
                        mSingleCalculation43.ParameterId = "43";
                        mSingleCalculation43.ParameterName = "Escrow Fees";
                        double EscrowFeesSCB = mFactor4 * ApartmentEstimatedValue/NumberofUnits;
                        mSingleCalculation43.value = EscrowFeesSCB.ToString();
                        mCalcList.Add(mSingleCalculation43);

                        //- Marketing / Staging(Marketing and Staging table X Unit Estimated Value)
                        CalcParameterOutputModel mSingleCalculation44 = new CalcParameterOutputModel();
                        mSingleCalculation44.ParameterId = "44";
                        mSingleCalculation44.ParameterName = "Marketing and Staging";
                        double MarketingandStagingSCB = mFactor5 * ApartmentEstimatedValue/NumberofUnits;
                        mSingleCalculation44.value = MarketingandStagingSCB.ToString();
                        mCalcList.Add(mSingleCalculation44);

                        // -Total Sales Cost per Unit(Sum of all the Blue Above)
                        CalcParameterOutputModel mSingleCalculation138 = new CalcParameterOutputModel();
                        mSingleCalculation138.ParameterId = "138";
                        mSingleCalculation138.ParameterName = "Total Sales Cost Per Unit";
                        double TotalSalesCostPerUnitSCB = SalesCommission + ExciseTaxSCB + EscrowFeesSCB + MarketingandStagingSCB;
                        mSingleCalculation138.value = TotalSalesCostPerUnitSCB.ToString();
                        mCalcList.Add(mSingleCalculation138);

                        // -Total Sales Cost per Project(Total Sales Cost per Unit X Number of Units)
                        CalcParameterOutputModel mSingleCalculation140 = new CalcParameterOutputModel();
                        mSingleCalculation140.ParameterId = "140";
                        mSingleCalculation140.ParameterName = "Total Sales Cost Per Project";
                        double TotalSalesCostPerProjectSCB = TotalSalesCostPerUnitSCB * NumberofUnits;
                        mSingleCalculation140.value = TotalSalesCostPerProjectSCB.ToString();
                        mCalcList.Add(mSingleCalculation140);

                        //Step 4 Land Analysis
                        // -Unit Estimated Value
                        CalcParameterOutputModel mSingleCalculation144 = new CalcParameterOutputModel();
                        mSingleCalculation144.ParameterId = "144";
                        mSingleCalculation144.ParameterName = "Unit Estimated Value";
                        double UnitEstimatedValueLA = mUnitEstimatedValue;
                        mSingleCalculation144.value = UnitEstimatedValueLA.ToString();
                        mCalcList.Add(mSingleCalculation144);

                        //-Total Building Cost Per Unit 176 "Total Building Cost Per Unit"
                        CalcParameterOutputModel mSingleCalculation176 = new CalcParameterOutputModel();
                        mSingleCalculation176.ParameterId = "176";
                        mSingleCalculation176.ParameterName = "Total Building Cost Per Unit";
                        double TotalBuildingCostPerUnitLA = TotalBuildingCostPerUnitCB;
                        mSingleCalculation176.value = TotalBuildingCostPerUnitLA.ToString();
                        mCalcList.Add(mSingleCalculation176);

                        //-Total Sales Cost Per Unit 174 "Total Sales Cost Per Unit"
                        CalcParameterOutputModel mSingleCalculation174 = new CalcParameterOutputModel();
                        mSingleCalculation174.ParameterId = "174";
                        mSingleCalculation174.ParameterName = "Total Sales Cost Per Unit";
                        double TotalSalesCostPerUnitLA = TotalBuildingCostPerUnitCB;
                        mSingleCalculation174.value = TotalSalesCostPerUnitLA.ToString();
                        mCalcList.Add(mSingleCalculation174);

                        // -Desired Profit Percent
                        CalcParameterOutputModel mSingleCalculation181 = new CalcParameterOutputModel();
                        mSingleCalculation181.ParameterId = "181";
                        mSingleCalculation181.ParameterName = "Desired Profit Percent";
                        double DesiredProfitPercentLA = mDesiredProfitPercent;
                        mSingleCalculation181.value = DesiredProfitPercentLA.ToString();
                        mCalcList.Add(mSingleCalculation181);

                        //-Desired Profit before financing(Desired profit % X Backend Sales Price) 45 "Desired Profit Before Financing"
                        CalcParameterOutputModel mSingleCalculation45 = new CalcParameterOutputModel();
                        mSingleCalculation45.ParameterId = "45";
                        mSingleCalculation45.ParameterName = "Desired Profit Before Financing";
                        double DesiredProfitBeforeFinancingLA = DesiredProfitPercentLA * UnitEstimatedValueLA;
                        mSingleCalculation45.value = DesiredProfitBeforeFinancingLA.ToString();
                        mCalcList.Add(mSingleCalculation45);

                        //- Value of Land Per Unit(Sum of Step 4 above this line(Red is Negative Green is Positive Number) 150 "Value of Land Per Unit"
                        CalcParameterOutputModel mSingleCalculation150 = new CalcParameterOutputModel();
                        mSingleCalculation150.ParameterId = "150";
                        mSingleCalculation150.ParameterName = "Value of Land Per Unit";
                        double ValueofLandPerUnitLA = UnitEstimatedValueLA - (TotalBuildingCostPerUnitLA + TotalSalesCostPerUnitLA + DesiredProfitBeforeFinancingLA);
                        mSingleCalculation150.value = ValueofLandPerUnitLA.ToString();
                        mCalcList.Add(mSingleCalculation150);

                        //- Total Land Value(Value of Land Per Unit X Number of Units) 47 "Total Land Value"
                        CalcParameterOutputModel mSingleCalculation47 = new CalcParameterOutputModel();
                        mSingleCalculation47.ParameterId = "47";
                        mSingleCalculation47.ParameterName = "Total Land Value";
                        double TotalLandValueLA = ValueofLandPerUnitLA * NumberofUnits;
                        mSingleCalculation47.value = TotalLandValueLA.ToString();
                        mCalcList.Add(mSingleCalculation47);

                        // Step 5 Financing
                        // PERFORMA PER PROJECT
                        // Total Project Cost* Step 2  
                        CalcParameterOutputModel mSingleCalculation178 = new CalcParameterOutputModel();
                        mSingleCalculation178.ParameterId = "178";
                        mSingleCalculation178.ParameterName = "Total Project Cost";
                        double TotalProjectCostFI = TotalProjectCostCB;
                        mSingleCalculation178.value = TotalProjectCostFI.ToString();
                        mCalcList.Add(mSingleCalculation178);

                        // Total Land Value* Step 4
                        CalcParameterOutputModel mSingleCalculation179 = new CalcParameterOutputModel();
                        mSingleCalculation179.ParameterId = "179";
                        mSingleCalculation179.ParameterName = "Total Land Value";
                        double TotalLandValuePF = TotalLandValueLA;
                        mSingleCalculation179.value = TotalLandValuePF.ToString();
                        mCalcList.Add(mSingleCalculation179);

                        // Equity(Manual Input)
                        CalcParameterOutputModel mSingleCalculation55 = new CalcParameterOutputModel();
                        mSingleCalculation55.ParameterId = "55";
                        mSingleCalculation55.ParameterName = "Equity";
                        double EquityPF = mEquity;
                        mSingleCalculation55.value = EquityPF.ToString();
                        mCalcList.Add(mSingleCalculation55);

                        // Acquisition Carrying Months(Manual Input)
                        CalcParameterOutputModel mSingleCalculation56 = new CalcParameterOutputModel();
                        mSingleCalculation56.ParameterId = "56";
                        mSingleCalculation56.ParameterName = "Acquisition Carrying Months";
                        double AcquisitionCarryingMonthsPF = mAcquisitionCarryingMonths;
                        mSingleCalculation56.value = AcquisitionCarryingMonthsPF.ToString();
                        mCalcList.Add(mSingleCalculation56);

                        // Interest Rate Land Acquisition(Manual Input)
                        CalcParameterOutputModel mSingleCalculation57 = new CalcParameterOutputModel();
                        mSingleCalculation57.ParameterId = "57";
                        mSingleCalculation57.ParameterName = "Interest Rate Land Acquisition";
                        double InterestRateLandAcquisitionPF = mInterestRateLandAcquisition;
                        mSingleCalculation57.value = InterestRateLandAcquisitionPF.ToString();
                        mCalcList.Add(mSingleCalculation57);

                        // Loan Fees Land Acquisition Loan(Manual Input)
                        CalcParameterOutputModel mSingleCalculation81 = new CalcParameterOutputModel();
                        mSingleCalculation81.ParameterId = "81";
                        mSingleCalculation81.ParameterName = "Loan Fees Land Acquisition Loan";
                        double LoanFeesLandAcquisitionLoanPF = mLoanFeesLandAcquisitionLoan;
                        mSingleCalculation81.value = LoanFeesLandAcquisitionLoanPF.ToString();
                        mCalcList.Add(mSingleCalculation81);

                        // Total Financing Cost Land Acquisition((Land Value – Equity) * (Interest Rate Land Acquisition * (Acquisition Carrying Months/ 12) +(Land Value – Equity)*(Loan Fees Land Acquisition Loan)
                        CalcParameterOutputModel mSingleCalculation58 = new CalcParameterOutputModel();
                        mSingleCalculation58.ParameterId = "58";
                        mSingleCalculation58.ParameterName = "Total Financing Cost Land Acquisition";
                        double TotalFinancingCostLandAcquisitionPF = ((TotalLandValuePF - EquityPF) * (InterestRateLandAcquisitionPF * (AcquisitionCarryingMonthsPF / 12)) + ((TotalLandValuePF - EquityPF) * (LoanFeesLandAcquisitionLoanPF)));
                        mSingleCalculation58.value = TotalFinancingCostLandAcquisitionPF.ToString();
                        mCalcList.Add(mSingleCalculation58);

                        // Interest Rate Construction Loan(Manual Default)
                        CalcParameterOutputModel mSingleCalculation59 = new CalcParameterOutputModel();
                        mSingleCalculation59.ParameterId = "59";
                        mSingleCalculation59.ParameterName = "Interest Rate Construction Loan";
                        double InterestRateConstructionLoan = mInterestRateConstructionLoan;
                        mSingleCalculation59.value = InterestRateConstructionLoan.ToString();
                        mCalcList.Add(mSingleCalculation59);

                        // Loan Fees Construction Loan(Manual Default)
                        CalcParameterOutputModel mSingleCalculation60 = new CalcParameterOutputModel();
                        mSingleCalculation60.ParameterId = "60";
                        mSingleCalculation60.ParameterName = "Loan Fees Construction Loan";
                        double LoanFeesConstructionLoanPF = mLoanFeesConstructionLoan;
                        mSingleCalculation60.value = LoanFeesConstructionLoanPF.ToString();
                        mCalcList.Add(mSingleCalculation60);

                        // Construction Loan Months(Manual Default)
                        CalcParameterOutputModel mSingleCalculation61 = new CalcParameterOutputModel();
                        mSingleCalculation61.ParameterId = "61";
                        mSingleCalculation61.ParameterName = "Construction Loan Months";
                        double ConstructionLoanMonthsFI = mConstructionLoanMonths;
                        mSingleCalculation61.value = ConstructionLoanMonthsFI.ToString();
                        mCalcList.Add(mSingleCalculation61);

                        // Total Financing Construction Cost(((Land Value -Equity) +(Total Project Cost/ 2) *((Interest Rate Construction Loan”) *(Construction Loan Months/ 12)) +((Land Value – Equity) *(Loan Fees Construction Loan”)))
                        CalcParameterOutputModel mSingleCalculation62 = new CalcParameterOutputModel();
                        mSingleCalculation62.ParameterId = "62";
                        mSingleCalculation62.ParameterName = "Total Financing Cost Construction";
                        double TotalFinancingCostConstructionFI = ((((TotalLandValuePF - EquityPF) + (TotalProjectCostFI / 2)) * ((InterestRateConstructionLoan) * (ConstructionLoanMonthsFI / 12))) + (((TotalProjectCostFI + TotalLandValuePF - EquityPF) * (LoanFeesConstructionLoanPF))));
                        mSingleCalculation62.value = TotalFinancingCostConstructionFI.ToString();
                        mCalcList.Add(mSingleCalculation62);

                        // Months to Sell after completion(Manual Default)
                        CalcParameterOutputModel mSingleCalculation64 = new CalcParameterOutputModel();
                        mSingleCalculation64.ParameterId = "64";
                        mSingleCalculation64.ParameterName = "Months to Sell After completion";
                        double MonthstoSellAfterCompletionPF = mMonthsToSellAfterCompletion;
                        mSingleCalculation64.value = MonthstoSellAfterCompletionPF.ToString();
                        mCalcList.Add(mSingleCalculation64);

                        // Extra Financing After Completion(Land Value +Total Project Cost – Contributed Equity) *(Interest Rate Construction Loan * (Months to Sell after completion Months / 12)  
                        CalcParameterOutputModel mSingleCalculation65 = new CalcParameterOutputModel();
                        mSingleCalculation65.ParameterId = "65";
                        mSingleCalculation65.ParameterName = "Extra Financing After Completion";
                        double ExtraFinancingAfterCompletionPF = (TotalLandValuePF + TotalProjectCostCB - EquityPF) * (InterestRateConstructionLoan * (MonthstoSellAfterCompletionPF / 12));
                        mSingleCalculation65.value = ExtraFinancingAfterCompletionPF.ToString();
                        mCalcList.Add(mSingleCalculation65);

                        // Total Financing Fees(Sum of Green Text)
                        CalcParameterOutputModel mSingleCalculation66 = new CalcParameterOutputModel();
                        mSingleCalculation66.ParameterId = "66";
                        mSingleCalculation66.ParameterName = "Total Financing Fees";
                        double TotalFinancingFeesFI = TotalFinancingCostLandAcquisitionPF + TotalFinancingCostConstructionFI + ExtraFinancingAfterCompletionPF;
                        mSingleCalculation66.value = TotalFinancingFeesFI.ToString();
                        mCalcList.Add(mSingleCalculation66);

                        // Step 6
                        // PERFORMA PER PROJECT
                        // -Apartment Value 
                        CalcParameterOutputModel mSingleCalculation160 = new CalcParameterOutputModel();
                        mSingleCalculation160.ParameterId = "160";
                        mSingleCalculation160.ParameterName = "Apartment Estimated Value";
                        double ApartmentEstimatedValuePF = ApartmentEstimatedValue;
                        mSingleCalculation160.value = ApartmentEstimatedValuePF.ToString();
                        mCalcList.Add(mSingleCalculation160);

                        //- Total Project Cost *See step 2 182 "Total Project Cost"
                        CalcParameterOutputModel mSingleCalculation182 = new CalcParameterOutputModel();
                        mSingleCalculation182.ParameterId = "182";
                        mSingleCalculation182.ParameterName = "Total Project Cost";
                        double TotalProjectCostPF = TotalProjectCostCB;
                        mSingleCalculation182.value = TotalProjectCostPF.ToString();
                        mCalcList.Add(mSingleCalculation182);

                        //- Total Sales Cost Per Project *See step 3 141 "Total Sales Cost Per Project"
                        CalcParameterOutputModel mSingleCalculation141 = new CalcParameterOutputModel();
                        mSingleCalculation141.ParameterId = "141";
                        mSingleCalculation141.ParameterName = "Total Sales Cost Per Project";
                        double TotalSalesCostPerProjectPF = TotalSalesCostPerProjectSCB;
                        mSingleCalculation141.value = TotalSalesCostPerProjectPF.ToString();
                        mCalcList.Add(mSingleCalculation141);

                        //- Total Land Value *See step 4 123 "Total Land Value"
                        CalcParameterOutputModel mSingleCalculation123 = new CalcParameterOutputModel();
                        mSingleCalculation123.ParameterId = "123";
                        mSingleCalculation123.ParameterName = "Total Land Value";
                        double TotalLandValuePFP = TotalLandValuePF;
                        mSingleCalculation123.value = TotalLandValuePFP.ToString();
                        mCalcList.Add(mSingleCalculation123);

                        //- Profit For Project before financing(Sum of Step 5 above this line(Red is Negative Green is Positive Number for project) 155 "Profit For Project before Financing"
                        CalcParameterOutputModel mSingleCalculation155 = new CalcParameterOutputModel();
                        mSingleCalculation155.ParameterId = "155";
                        mSingleCalculation155.ParameterName = "Profit For Project before Financing";
                        double ProfitForProjectbeforeFinancingPFP = ApartmentEstimatedValuePF - (TotalProjectCostPF + TotalSalesCostPerProjectPF + TotalLandValuePFP);
                        mSingleCalculation155.value = ProfitForProjectbeforeFinancingPFP.ToString();
                        mCalcList.Add(mSingleCalculation155);

                        //-Total Financing Costs*See Step 6 162 "Total Financing Cost"
                        CalcParameterOutputModel mSingleCalculation162 = new CalcParameterOutputModel();
                        mSingleCalculation162.ParameterId = "162";
                        mSingleCalculation162.ParameterName = "Total Financing Cost";
                        double TotalFinancingCostPFP = TotalFinancingFeesFI;
                        mSingleCalculation162.value = TotalFinancingCostPFP.ToString();
                        mCalcList.Add(mSingleCalculation162);

                        //- Net Profit For Project(Profit For Project before financing - Total Financing Costs) 159 "Net Profit For Project" 
                        CalcParameterOutputModel mSingleCalculation159 = new CalcParameterOutputModel();
                        mSingleCalculation159.ParameterId = "159";
                        mSingleCalculation159.ParameterName = "Net Profit For Project";
                        double NetProfitForProjectPFP = ProfitForProjectbeforeFinancingPFP - TotalFinancingCostPFP;
                        mSingleCalculation159.value = NetProfitForProjectPFP.ToString();
                        mCalcList.Add(mSingleCalculation159);
                    }

                    // Append all Calculation to list of calculations
                    mCalcOutput.Calculations = mCalcList;
                    mOutModel.Add(mCalcOutput);
               // }

                    
            }

            return mOutModel;
        }
    }
}
