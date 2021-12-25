using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using NWLTLambda.Models;
using Newtonsoft.Json;
using System.IO;
using System.Web.Mvc;
using Microsoft.AspNetCore.Http;
using static Org.BouncyCastle.Bcpg.Attr.ImageAttrib;
using Microsoft.AspNetCore.Hosting;
using Amazon.S3;
using Amazon;
using Amazon.S3.Transfer;
using Amazon.S3.Model;
// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace NWLTLambda
{
    public class Functions:Controller
    {
        private IHostingEnvironment Environment;
        //public Functions(IHostingEnvironment hostEnvironment)
        //{
        //    Environment = hostEnvironment;
        //}

      
        public Functions()
        {
            
        }
        #region Resource (endpoints)
        public async Task<List<FormModel>> GetFormList(object request, ILambdaContext context) //task is the model if want to return
        {
            List<FormModel> mResponse = new List<FormModel>();
            mySqlAccess _sqlaccess = new mySqlAccess();
            ApiCityAuthorize mFullAuthRequest = new ApiCityAuthorize();
            CityInputModel mCityParams = new CityInputModel();
            FormModel mSingleResponse = new FormModel();
            // Verify Request
            if (request == null)
            {
                mSingleResponse.mResponseMessage = "Request cannot be empty";
                mResponse.Add(mSingleResponse);
                context.Logger.LogLine($"ERROR:" + "RequestID: " + context.AwsRequestId + " Request string is Empty");

            }
            else
            {
                context.Logger.LogLine($"Info: " + "RequestID: " + context.AwsRequestId + ", Deserializing request string");
                try
                {
                    mFullAuthRequest = JsonConvert.DeserializeObject<ApiCityAuthorize>(request.ToString());
                    mCityParams = mFullAuthRequest.Body;
                    context.Logger.LogLine($"Info: Request sent is: " + "RequestID: " + context.AwsRequestId + " " + request.ToString());
                }
                catch (Exception ex)
                {
                    mSingleResponse.mResponseMessage = "Request is Invalid: " + ex.Message;
                    mResponse.Add(mSingleResponse);
                    context.Logger.LogLine($"ERROR: " + "RequestID: " + context.AwsRequestId + " Invalid Request: " + ex.Message);
                    return mResponse;
                }
                context.Logger.LogLine($"Info: " + "RequestID: " + context.AwsRequestId + ", Deserializing request string");
                mResponse = _sqlaccess.GetForms(mCityParams);
            }
            return mResponse;
        }
        
        public async Task<List<FormModel>> SearchForms(object request, ILambdaContext context) //task is the model if want to return
        {
            List<FormModel> mResponse = new List<FormModel>();
            mySqlAccess _sqlaccess = new mySqlAccess();
            ApiFormSearchAuthorize mFullAuthRequest = new ApiFormSearchAuthorize();
            FormSearchInputModel mSearchParams = new FormSearchInputModel();
            FormModel mSingleResponse = new FormModel();
            // Verify Request
            if (request == null)
            {
                mSingleResponse.mResponseMessage = "Request cannot be empty";
                mResponse.Add(mSingleResponse);
                context.Logger.LogLine($"ERROR:" + "RequestID: " + context.AwsRequestId + " Request string is Empty");

            }
            else
            {
                context.Logger.LogLine($"Info: " + "RequestID: " + context.AwsRequestId + ", Deserializing request string");
                try
                {
                    mFullAuthRequest = JsonConvert.DeserializeObject<ApiFormSearchAuthorize>(request.ToString());
                    mSearchParams = mFullAuthRequest.Body;
                    context.Logger.LogLine($"Info: Request sent is: " + "RequestID: " + context.AwsRequestId + " " + request.ToString());
                }
                catch (Exception ex)
                {
                    mSingleResponse.mResponseMessage = "Request is Invalid: " + ex.Message;
                    mResponse.Add(mSingleResponse);
                    context.Logger.LogLine($"ERROR: " + "RequestID: " + context.AwsRequestId + " Invalid Request: " + ex.Message);
                    return mResponse;
                }
                context.Logger.LogLine($"Info: " + "RequestID: " + context.AwsRequestId + ", Deserializing request string");
                mResponse = _sqlaccess.SearchForms(mSearchParams);
            }
            return mResponse;
        }
        
        public async Task<List<FormValueModel>> GetFormValues (object request, ILambdaContext context) //task is the model if want to return
        {
            List<FormValueModel> mResponse = new List<FormValueModel>();
            mySqlAccess _sqlaccess = new mySqlAccess();
            ApiFormAuthorize mFullAuthRequest = new ApiFormAuthorize();
            FormValueInputModel mInputForm = new FormValueInputModel();
            FormValueModel mSingleResponse = new FormValueModel();
            // Verify Request
            if (request == null)
            {
                mSingleResponse.mResponseMessage = "Request cannot be empty";
                mResponse.Add(mSingleResponse);
                context.Logger.LogLine($"ERROR:" + "RequestID: " + context.AwsRequestId + " Request string is Empty");

            }
            else
            {
                context.Logger.LogLine($"Info: " + "RequestID: " + context.AwsRequestId + ", Deserializing request string");
                try
                {
                    mFullAuthRequest = JsonConvert.DeserializeObject<ApiFormAuthorize>(request.ToString());
                    mInputForm = mFullAuthRequest.Body;
                    context.Logger.LogLine($"Info: Request sent is: " + "RequestID: " + context.AwsRequestId + " " + request.ToString());
                }
                catch (Exception ex)
                {
                    mSingleResponse.mResponseMessage = "Request is Invalid: " + ex.Message;
                    mResponse.Add(mSingleResponse);
                    context.Logger.LogLine($"ERROR: " + "RequestID: " + context.AwsRequestId + " Invalid Request: " + ex.Message);
                    return mResponse;
                }
                context.Logger.LogLine($"Info: " + "RequestID: " + context.AwsRequestId + ", Deserializing request string");
                mResponse = _sqlaccess.GetFormValues(mInputForm);
            }
            return mResponse;
        }

        public async Task<List<CityModel>> GetCities(object request, ILambdaContext context) //task is the model if want to return
        {
            List<CityModel> mResponse = new List<CityModel>();
            mySqlAccess _sqlaccess = new mySqlAccess();
            CityModel mSingleResponse = new CityModel();
            // Verify Request

            context.Logger.LogLine($"Info: " + "RequestID: " + context.AwsRequestId + ", Deserializing request string");



            // Save Both Inputs to DB
            mResponse = _sqlaccess.GetCities();


            return mResponse;
        }

        public async Task<List<ParametersOutputModel>> GetParameters(object request, ILambdaContext context) //task is the model if want to return
        {
            List<ParametersOutputModel> mResponse = new List<ParametersOutputModel>();
            ApiCityAuthorize mFullAuthRequest = new ApiCityAuthorize();
            CityInputModel mCityParams = new CityInputModel();
            mySqlAccess _sqlaccess = new mySqlAccess();
            CalcEngine _calcEngine = new CalcEngine();
            ParametersOutputModel mSingleResponse = new ParametersOutputModel();
            // Verify Request
            if (request == null)
            {
                mSingleResponse.mResponseMessage = "Request cannot be empty";
                mResponse.Add(mSingleResponse);
                context.Logger.LogLine($"ERROR:" + "RequestID: " + context.AwsRequestId + " Request string is Empty");

            }
            else
            {
                context.Logger.LogLine($"Info: " + "RequestID: " + context.AwsRequestId + ", Deserializing request string");
                try
                {
                    mFullAuthRequest = JsonConvert.DeserializeObject<ApiCityAuthorize>(request.ToString());
                    mCityParams = mFullAuthRequest.Body;
                    context.Logger.LogLine($"Info: Request sent is: " + "RequestID: " + context.AwsRequestId + " " + request.ToString());
                }
                catch (Exception ex)
                {
                    mSingleResponse.mResponseMessage = "Request is Invalid: " + ex.Message;
                    mResponse.Add(mSingleResponse);
                    context.Logger.LogLine($"ERROR: " + "RequestID: " + context.AwsRequestId + " Invalid Request: " + ex.Message);
                    return mResponse;
                }



                // Save Both Inputs to DB
                mResponse = _sqlaccess.GetCityParams(mCityParams);
            }

            return mResponse;
        }

        public async Task<List<AllParametersOutputModel>> GetAllParameters(object request, ILambdaContext context) //task is the model if want to return
        {
            List<AllParametersOutputModel> mResponse = new List<AllParametersOutputModel>();
            ApiCityAuthorize mFullAuthRequest = new ApiCityAuthorize();
            CityInputModel mCityParams = new CityInputModel();
            mySqlAccess _sqlaccess = new mySqlAccess();
            CalcEngine _calcEngine = new CalcEngine();
            AllParametersOutputModel mSingleResponse = new AllParametersOutputModel();
            // Verify Request
            if (request == null)
            {
                mSingleResponse.mResponseMessage = "Request cannot be empty";
                mResponse.Add(mSingleResponse);
                context.Logger.LogLine($"ERROR:" + "RequestID: " + context.AwsRequestId + " Request string is Empty");

            }
            else
            {
                context.Logger.LogLine($"Info: " + "RequestID: " + context.AwsRequestId + ", Deserializing request string");
                try
                {
                    mFullAuthRequest = JsonConvert.DeserializeObject<ApiCityAuthorize>(request.ToString());
                    mCityParams = mFullAuthRequest.Body;
                    context.Logger.LogLine($"Info: Request sent is: " + "RequestID: " + context.AwsRequestId + " " + request.ToString());
                }
                catch (Exception ex)
                {
                    mSingleResponse.mResponseMessage = "Request is Invalid: " + ex.Message;
                    mResponse.Add(mSingleResponse);
                    context.Logger.LogLine($"ERROR: " + "RequestID: " + context.AwsRequestId + " Invalid Request: " + ex.Message);
                    return mResponse;
                }



                // Request from DB
                mResponse = _sqlaccess.GetAllParams(mCityParams);
            }

            return mResponse;
        }

        public async Task<List<ParametersOutputModel>> GetCalcParameters(object request, ILambdaContext context) //task is the model if want to return
        {
            List<ParametersOutputModel> mResponse = new List<ParametersOutputModel>();
            ApiCityAuthorize mFullAuthRequest = new ApiCityAuthorize();
            CityInputModel mCityParams = new CityInputModel();
            mySqlAccess _sqlaccess = new mySqlAccess();
            CalcEngine _calcEngine = new CalcEngine();
            ParametersOutputModel mSingleResponse = new ParametersOutputModel();
            // Verify Request
            if (request == null)
            {
                mSingleResponse.mResponseMessage = "Request cannot be empty";
                mResponse.Add(mSingleResponse);
                context.Logger.LogLine($"ERROR:" + "RequestID: " + context.AwsRequestId + " Request string is Empty");

            }
            else
            {
                context.Logger.LogLine($"Info: " + "RequestID: " + context.AwsRequestId + ", Deserializing request string");
                try
                {
                    mFullAuthRequest = JsonConvert.DeserializeObject<ApiCityAuthorize>(request.ToString());
                    mCityParams = mFullAuthRequest.Body;
                    context.Logger.LogLine($"Info: Request sent is: " + "RequestID: " + context.AwsRequestId + " " + request.ToString());
                }
                catch (Exception ex)
                {
                    mSingleResponse.mResponseMessage = "Request is Invalid: " + ex.Message;
                    mResponse.Add(mSingleResponse);
                    context.Logger.LogLine($"ERROR: " + "RequestID: " + context.AwsRequestId + " Invalid Request: " + ex.Message);
                    return mResponse;
                }



                // Request from DB
                mResponse = _sqlaccess.GetCalcParams(mCityParams);
            }

            return mResponse;
        }

        public async Task<List<ParametersOutputModel2>> GetFormParameters(object request, ILambdaContext context) //task is the model if want to return
        {
            List<ParametersOutputModel2> mResponse = new List<ParametersOutputModel2>();
            ApiCityAuthorize mFullAuthRequest = new ApiCityAuthorize();
            CityInputModel mCityParams = new CityInputModel();
            mySqlAccess _sqlaccess = new mySqlAccess();
            CalcEngine _calcEngine = new CalcEngine();
            ParametersOutputModel2 mSingleResponse = new ParametersOutputModel2();
            // Verify Request
            if (request == null)
            {
                mSingleResponse.mResponseMessage = "Request cannot be empty";
                mResponse.Add(mSingleResponse);
                context.Logger.LogLine($"ERROR:" + "RequestID: " + context.AwsRequestId + " Request string is Empty");

            }
            else
            {
                context.Logger.LogLine($"Info: " + "RequestID: " + context.AwsRequestId + ", Deserializing request string");
                try
                {
                    mFullAuthRequest = JsonConvert.DeserializeObject<ApiCityAuthorize>(request.ToString());
                    mCityParams = mFullAuthRequest.Body;
                    context.Logger.LogLine($"Info: Request sent is: " + "RequestID: " + context.AwsRequestId + " " + request.ToString());
                }
                catch (Exception ex)
                {
                    mSingleResponse.mResponseMessage = "Request is Invalid: " + ex.Message;
                    mResponse.Add(mSingleResponse);
                    context.Logger.LogLine($"ERROR: " + "RequestID: " + context.AwsRequestId + " Invalid Request: " + ex.Message);
                    return mResponse;
                }



                // Save Both Inputs to DB
                mResponse = _sqlaccess.GetFormParams(mCityParams);
            }

            return mResponse;
        }

        public async Task<List<ParameterValuesOutputModel>> GetParameterValues(object request, ILambdaContext context)
        {
            List<ParameterValuesOutputModel> mResponse = new List<ParameterValuesOutputModel>();
            ApiCityParamAuthorize mFullAuthRequest = new ApiCityParamAuthorize();
            ParameterValuesInputModel mParamValues = new ParameterValuesInputModel();
            mySqlAccess _sqlaccess = new mySqlAccess();
            ParameterValuesOutputModel mSingleResponse = new ParameterValuesOutputModel();
            // Verify Request
            if (request == null)
            {
                mSingleResponse.mResponseMessage = "Request cannot be empty";
                mResponse.Add(mSingleResponse);
                context.Logger.LogLine($"ERROR:" + "RequestID: " + context.AwsRequestId + " Request string is Empty");

            }
            else
            {
                context.Logger.LogLine($"Info: " + "RequestID: " + context.AwsRequestId + ", Deserializing request string");
                try
                {
                    mFullAuthRequest = JsonConvert.DeserializeObject<ApiCityParamAuthorize>(request.ToString());
                    mParamValues = mFullAuthRequest.Body;
                    context.Logger.LogLine($"Info: Request sent is: " + "RequestID: " + context.AwsRequestId + " " + request.ToString());
                }
                catch (Exception ex)
                {
                    mSingleResponse.mResponseMessage = "Request is Invalid: " + ex.Message;
                    mResponse.Add(mSingleResponse);
                    context.Logger.LogLine($"ERROR: " + "RequestID: " + context.AwsRequestId + " Invalid Request: " + ex.Message);
                    return mResponse;
                }


                // Save Both Inputs to DB
                mResponse = _sqlaccess.GetParameterValues(mParamValues);
            }

            return mResponse;
        }
        public class FileUploadResult
        {
            public long Length { get; set; }

            public string Name { get; set; }
        }

      
        //public string  SaveImage()
        //{
        //    //string imagepath = Path.GetFileNameWithoutExtension(formfile[0].FileName);
        //    //imagepath = imagepath + type + formid + Path.GetExtension(formfile[0].FileName);
        //    //var image = Path.Combine(Environment.ContentRootPath, "ProfileImg", imagepath);
        //    //string name = Path.GetFileName(formfile[0].FileName);
        //    string myBucketName = "nwlt-images"; //your s3 bucket name goes here  
        //    string s3DirectoryName = "";
        //    string s3FileName = "qadeer";
        //    bool a;
        //    using (var file = new FileStream("ProfileImg/qadeer.JPG", FileMode.Create))
        //    {
        //        sendMyFileToS3(file, myBucketName, s3DirectoryName, s3FileName);
        //       // formfile[0].CopyToAsync(file);
        //    }
          

        //    return "";
        //}
        public async Task<OutputModel> SFProforma(object request, ILambdaContext context)
        {
         
            OutputModel mResponse = new OutputModel();
            ApiAuthorize  mFullRequest = new ApiAuthorize();
            InputModel mInputParams = new InputModel();
            mySqlAccess _sqlaccess = new mySqlAccess();
            CalcEngine _calcEngine = new CalcEngine();
            ReCalcEngine _recalcEngine = new ReCalcEngine();
            ParametersList mParams = new ParametersList();
          

            List<StructureImages> Mimg = new List<StructureImages>();
            // Verify Request
            if (request == null)
            {
                mResponse.MessageText = "Request cannot be empty";
                context.Logger.LogLine($"ERROR:" + "RequestID: " + context.AwsRequestId + " Request string is Empty");

            }
            else
            {
                context.Logger.LogLine($"Info: " + "RequestID: " + context.AwsRequestId + ", Deserializing request string");
                try
                {
                    mFullRequest = JsonConvert.DeserializeObject<ApiAuthorize>(request.ToString());
                    mInputParams = mFullRequest.Body;
                    context.Logger.LogLine($"Info: Request sent is: " + "RequestID: " + context.AwsRequestId + " " + request.ToString());
                }
                catch (Exception ex)
                {
                    mResponse.MessageText = "Request is Invalid: " + ex.Message;
                    context.Logger.LogLine($"ERROR: " + "RequestID: " + context.AwsRequestId + " Invalid Request: " + ex.Message);
                    return mResponse;
                }

                if (mInputParams.parameters.Count > 0)
                {
                    foreach (ParameterInputModel mParam in mInputParams.parameters)
                    {
                        switch (mParam.ParameterId)
                        {   
                            case 4:
                                if(mParam.value=="No")
                                {
                                    mParam.value = "0";
                                }
                                mParams.YearBuilt = Convert.ToInt32(mParam.value);
                                break;
                            case 5:
                                if (mParam.value == "No")
                                {
                                    mParam.value = "0";
                                }
                                mParams.LotSize = Convert.ToDouble(mParam.value);
                                break;
                            case 6:
                                if (mParam.value == "No")
                                {
                                    mParam.value = "SF-5000";
                                }
                                mParams.Zoning = mParam.value.ToString();
                                break;
                            case 7:
                                mParams.MhaSuffix = mParam.value.ToString();
                                break;
                            case 8:
                                if (mParam.value == "No")
                                {
                                    mParam.value = "0";
                                }
                                mParams.AssessedValue = Convert.ToDouble(mParam.value);
                                break;
                            case 9:
                                if (mParam.value == "No")
                                {
                                    mParam.value = "0";
                                }
                                mParams.LotWidth = Convert.ToDouble(mParam.value);
                                break;
                            case 10:
                                if (mParam.value == "No")
                                {
                                    mParam.value = "0";
                                }
                                mParams.LotLength = Convert.ToDouble(mParam.value);
                                break;
                            case 11:
                                mParams.HighEndArea = mParam.value.ToString();
                                break;
                            case 12:
                                mParams.MajorArterial = mParam.value.ToString();
                                break;
                            case 13:
                                mParams.CornerLot = mParam.value.ToString();
                                break;
                            case 14:
                                mParams.Alley = mParam.value.ToString();
                                break;
                            case 15:
                                mParams.GrowthArea = mParam.value.ToString();
                                break;
                            case 16:
                                if (mParam.value == "No")
                                {
                                    mParam.value = "0";
                                }
                                mParams.ElevationChange = Convert.ToDouble(mParam.value);
                                break;
                            case 17:
                                mParams.SlopeDirection = mParam.value.ToString();
                                break;
                            case 18:
                                mParams.TreeCanopyCoverage = mParam.value.ToString();
                                break;
                            case 19:
                                mParams.Ecas = mParam.value.ToString().Replace("undefined,", "");
                                break;
                            case 20:
                                mParams.WaterMainExtension = mParam.value.ToString();
                                break;
                            case 21:
                                if (mParam.value == "No")
                                {
                                    mParam.value = "0";
                                }
                                mParams.WaterMainLength = Convert.ToDouble(mParam.value);
                                break;
                            case 22:
                                mParams.SewerMainExtension = mParam.value.ToString();
                                break;
                            case 23:
                                if (mParam.value == "No")
                                {
                                    mParam.value = "0";
                                }
                                mParams.SewerMainLength = Convert.ToDouble(mParam.value);
                                break;
                            case 24:
                                mParams.AlleyConstruction = mParam.value.ToString();
                                break;
                            case 25:
                                if (mParam.value == "No")
                                {
                                    mParam.value = "0";
                                }
                                mParams.AlleyConstructionLength = Convert.ToDouble(mParam.value);
                                break;
                            case 26:
                                mParams.UndergroundGarage = mParam.value.ToString();
                                break;
                            case 27:
                                if (mParam.value == "No")
                                {
                                    mParam.value = "0";
                                }
                                mParams.NumberOfSpots = Convert.ToInt32(mParam.value);
                                break;
                            //case 28:
                            //    mParams.StructureType = mParam.value.ToString();
                            //    break;
                            case 29:
                                mParams.Mhaarea = mParam.value.ToString();
                                break;
                            case 83:
                                mParams.DrainageMain = mParam.value.ToString();
                                break;
                            case 84:
                                if (mParam.value == "No")
                                {
                                    mParam.value = "0";
                                }
                                mParams.DrainageDistance = Convert.ToDouble(mParam.value);
                                break;
                            case 30:
                                if (mParam.value == "No")
                                {
                                    mParam.value = "0";
                                }
                                mParams.Number_of_homes = Convert.ToInt32(mParam.value);
                                break;
                            case 32:
                                if (mParam.value == "No")
                                {
                                    mParam.value = "0";
                                }
                                mParams.Aadu = Convert.ToDouble(mParam.value);
                                break;
                            case 33:
                                if (mParam.value == "No")
                                {
                                    mParam.value = "0";
                                }
                                mParams.Aadu_size = Convert.ToDouble(mParam.value);
                                break;
                            case 34:
                                if (mParam.value == "No")
                                {
                                    mParam.value = "0";
                                }
                                mParams.Dadu = Convert.ToDouble(mParam.value);
                                break;
                            case 35:
                                if (mParam.value == "No")
                                {
                                    mParam.value = "0";
                                }
                                mParams.Dadu_size = Convert.ToDouble(mParam.value);
                                break;
                            case 81:
                                if (mParam.value == "No")
                                {
                                    mParam.value = "0";
                                }
                                mParams.LoanFeesLandAcquisitionLoan = Convert.ToDouble(mParam.value);
                                break;
                            case 60:
                                if (mParam.value == "No")
                                {
                                    mParam.value = "0";
                                }
                                mParams.LoanFeesConstructionLoan = Convert.ToDouble(mParam.value);
                                break;
                            case 3:
                                mResponse.Address = mParam.value.ToString();
                                break;
                        }
                    }
                }


                
                
                mParams.CityId = Convert.ToInt32(mInputParams.CityId);
                int mRet2 = 0;
                // Step 1.    Save Inputs to DB -- this could eventually check for a valid returning formID and if not it cannot move to next steps
                if (Convert.ToInt32(mInputParams.formId) > 0)
                {
                    //if (mInputParams.Structureimages.Count > 0)
                    //{
                    //    //foreach (StructureImages mParam in mInputParams.Structureimages)
                    //    //{
                    //    //    StructureImages img = new StructureImages();
                    //    //    img.form_id = Convert.ToInt32(mInputParams.formId);
                    //    //    img.image_type = mParam.image_type;
                    //    //    img.image_url = mParam.image_url;
                    //    //    img.structure_type_id = mParam.structure_type_id;
                    //    //    Mimg.Add(img);
                    //    //}
                    //    //mResponse.Structureimages = Mimg;
                    //    mRet2 = await _sqlaccess.ProformaUpdateImages(mInputParams, Convert.ToInt32(mInputParams.formId));

                    //}
                    List<StructureTypesVM> structureTypes = new List<StructureTypesVM>();
                    StructureTypesVM structureTypesVMs;
                    if (mInputParams.StructureTypes.Count > 0)
                    {
                        foreach (StructureTypesVM item in mInputParams.StructureTypes)
                        {
                            //structureTypesVMs = new StructureTypesVM();
                            //structureTypesVMs.StructureType = item.StructureType;
                            foreach (CalcParameterOutputModel item1 in item.Calculations)
                            {
                                if (item1.value == "" || item1.value == "NaN" || item1.value == "∞" || item1.value == "null" || item1.value == " ")
                                {
                                    item1.value = "0";
                                }
                                switch (item1.ParameterId)
                                {

                                    case "116":
                                        mParams.WaterLineCostPerSqft = Convert.ToDouble(item1.value);
                                        break;
                                    case "93":
                                        mParams.ColorAreaBuildingCost = Convert.ToDouble(item1.value);
                                        break;
                                    case "92":
                                        mParams.AverageSqftPerHome = Convert.ToDouble(item1.value);
                                        break;
                                    case "91":
                                        mParams.SewerCost = Convert.ToDouble(item1.value);
                                        break;
                                    case "90":
                                        mParams.AlleyCost = Convert.ToDouble(item1.value);
                                        break;
                                    case "89":
                                        mParams.WaterLineCost = Convert.ToDouble(item1.value);
                                        break;
                                    case "88":
                                        mParams.ElevationGradeCost = Convert.ToDouble(item1.value);
                                        break;
                                    case "87":
                                        mParams.EcaCost = Convert.ToDouble(item1.value);
                                        break;
                                    case "114":
                                        mParams.CondoBaseCostPerSqft = Convert.ToDouble(item1.value);
                                        break;
                                    case "115":
                                        mParams.ADUBaseCostPerSqft = Convert.ToDouble(item1.value);
                                        break;
                                    case "112":
                                        mParams.RowhouseBaseCostPerSqft = Convert.ToDouble(item1.value);
                                        break;
                                    case "117":
                                        mParams.AlleyCostPerSqft = Convert.ToDouble(item1.value);
                                        break;
                                    case "118":
                                        mParams.SewerCostPerSqft = Convert.ToDouble(item1.value);
                                        break;
                                    case "119":
                                        mParams.StormDrainCostPerSqft = Convert.ToDouble(item1.value);
                                        break;
                                    case "120":
                                        mParams.UndergroundParkingCostPerStall = Convert.ToDouble(item1.value);
                                        break;
                                    case "121":
                                        mParams.MhaCost = Convert.ToDouble(item1.value);
                                        break;
                                    case "124":
                                        mParams.DaduBaseCost = Convert.ToDouble(item1.value);
                                        break;
                                    case "126":
                                        mParams.TotalBuildingCostPerUnit = Convert.ToDouble(item1.value);
                                        break;
                                    case "128":
                                        mParams.TotalBuildingCostPerHome = Convert.ToDouble(item1.value);
                                        break;
                                    case "95":
                                        mParams.CostPerGrade = Convert.ToDouble(item1.value);
                                        break;
                                    case "111":
                                        mParams.CottageBaseCostPerSqft = Convert.ToDouble(item1.value);
                                        break;
                                    case "110":
                                        mParams.TownhomeBaseCostPerSqft = Convert.ToDouble(item1.value);
                                        break;
                                    case "109":
                                        mParams.SfBaseCostPerSqft = Convert.ToDouble(item1.value);
                                        break;
                                    case "108":
                                        mParams.TotalProjectCost = Convert.ToDouble(item1.value);
                                        break;
                                    case "107":
                                        mParams.TotalBuildingCostPerLot = Convert.ToDouble(item1.value);
                                        break;
                                    case "106":
                                        mParams.UndergroundParkingBaseCost = Convert.ToDouble(item1.value);
                                        break;
                                    case "105":
                                        mParams.StormDrainCost = Convert.ToDouble(item1.value);
                                        break;
                                    case "104":
                                        mParams.AADUBaseCost = Convert.ToDouble(item1.value);
                                        break;
                                    case "102":
                                        mParams.ApartmentBaseCost = Convert.ToDouble(item1.value);
                                        break;
                                    case "103":
                                        mParams.CondoBaseCost = Convert.ToDouble(item1.value);
                                        break;
                                    case "101":
                                        mParams.CottageBaseCost = Convert.ToDouble(item1.value);
                                        break;
                                    case "100":
                                        mParams.RowhouseBaseCost = Convert.ToDouble(item1.value);
                                        break;
                                    case "99":
                                        mParams.TownhomeBaseCost = Convert.ToDouble(item1.value);
                                        break;
                                    case "97":
                                        mParams.SfBaseCost = Convert.ToDouble(item1.value);
                                        break;
                                    case "113":
                                        mParams.ApartmentBaseCostPerSqft = Convert.ToDouble(item1.value);
                                        break;
                                    case "94":
                                        mParams.Add4StoryCost = Convert.ToDouble(item1.value);
                                        break;
                                    case "53":
                                        mParams.SoftCost = Convert.ToDouble(item1.value);
                                        break;
                                    case "54":
                                        mParams.HardCost = Convert.ToDouble(item1.value);
                                        break;
                                    case "55":
                                        mParams.Equity = Convert.ToDouble(item1.value);
                                        break;
                                    case "57":
                                        mParams.InterestRateLandAcquisition = Convert.ToDouble(item1.value);
                                        break;
                                    case "58":
                                        mParams.TotalFinancingCostLandAcquisition = Convert.ToDouble(item1.value);
                                        break;
                                    case "59":
                                        mParams.InterestRateConstructionLoan = Convert.ToDouble(item1.value);
                                        break;
                                    case "60":
                                        mParams.LoanFeesConstructionLoan = Convert.ToDouble(item1.value);
                                        break;
                                    case "61":
                                        mParams.ConstructionLoanMonths = Convert.ToDouble(item1.value);
                                        break;
                                    case "62":
                                        mParams.TotalFinancingCostConstruction = Convert.ToDouble(item1.value);
                                        break;
                                    case "63":
                                        mParams.AssignmentFees = Convert.ToDouble(item1.value);
                                        break;
                                    case "64":
                                        mParams.MonthsToSellAfterCompletion = Convert.ToDouble(item1.value);
                                        break;
                                    case "65":
                                        mParams.ExtraFinancingAfterCompletion = Convert.ToDouble(item1.value);
                                        break;
                                    case "66":
                                        mParams.TotalFinancingFees = Convert.ToDouble(item1.value);
                                        break;
                                    case "56":
                                        mParams.AcquisitionCarryingMonths = Convert.ToDouble(item1.value);
                                        break;
                                    case "81":
                                        mParams.LoanFeesLandAcquisitionLoan = Convert.ToDouble(item1.value);
                                        break;
                                    case "52":
                                        mParams.TotalAcquisition = Convert.ToDouble(item1.value);
                                        break;
                                    case "179":
                                        mParams.TotalLandValue = Convert.ToDouble(item1.value);
                                        break;
                                    case "178":
                                        mParams.TotalProjectCost = Convert.ToDouble(item1.value);
                                        break;
                                    case "174":
                                        mParams.TotalSalesCostPerUnit = Convert.ToDouble(item1.value);
                                        break;
                                    case "172":
                                        mParams.TotalSalesCostPerLot = Convert.ToDouble(item1.value);
                                        break;
                                    case "171":
                                        mParams.TotalBuildingCostPerProject = Convert.ToDouble(item1.value);
                                        break;
                                    case "170":
                                        mParams.TotalBuildingCostPerLot = Convert.ToDouble(item1.value);
                                        break;
                                    case "169":
                                        mParams.TotalBuildingCostPerHome = Convert.ToDouble(item1.value);
                                        break;
                                    case "168":
                                        mParams.HomeEstimatedValue = Convert.ToDouble(item1.value);
                                        break;
                                    case "173":
                                        mParams.TotalSalesCostPerHome = Convert.ToDouble(item1.value);
                                        break;
                                    case "175":
                                        mParams.TotalSalesCostPerProject = Convert.ToDouble(item1.value);
                                        break;
                                    case "176":
                                        mParams.TotalBuildingCostPerUnit = Convert.ToDouble(item1.value);
                                        break;
                                    case "177":
                                        mParams.ApartmentEstimatedValue = Convert.ToDouble(item1.value);
                                        break;
                                    case "150":
                                        mParams.ValueOfLandPerUnit = Convert.ToDouble(item1.value);
                                        break;
                                    case "148":
                                        mParams.ValueOfLandPerLot = Convert.ToDouble(item1.value);
                                        break;
                                    case "147":
                                        mParams.LotEstimatedValue = Convert.ToDouble(item1.value);
                                        break;
                                    case "45":
                                        mParams.DesiredProfitBeforeFinancing = Convert.ToDouble(item1.value);
                                        break;
                                    case "46":
                                        mParams.ValueOfLandPerHome = Convert.ToDouble(item1.value);
                                        break;
                                    case "47":
                                        mParams.TotalLandValue = Convert.ToDouble(item1.value);
                                        break;
                                    case "180":
                                        mParams.BackendSalesPrice = Convert.ToDouble(item1.value);
                                        break;
                                    case "144":
                                        mParams.UnitEstimatedValue = Convert.ToDouble(item1.value);
                                        break;
                                    case "181":
                                        mParams.DesiredProfitPercent = Convert.ToDouble(item1.value);
                                        break;
                                    case "166":
                                        mParams.TotakBackendSalesPrice = Convert.ToDouble(item1.value);
                                        break;
                                    case "161":
                                        mParams.TotalBuildingCostPerLot = Convert.ToDouble(item1.value);
                                        break;
                                    case "167":
                                        mParams.HomeEstimatedValue = Convert.ToDouble(item1.value);
                                        break;
                                    case "182":
                                        mParams.TotalProjectCost = Convert.ToDouble(item1.value);
                                        break;
                                    case "155":
                                        mParams.ProfitForProjectBeforeFinancing = Convert.ToDouble(item1.value);
                                        break;
                                    case "141":
                                        mParams.TotalSalesCostPerProject = Convert.ToDouble(item1.value);
                                        break;
                                    case "143":
                                        mParams.UnitEstimatedValue = Convert.ToDouble(item1.value);
                                        break;
                                    case "183":
                                        mParams.TotalFinancingCostPerUnit = Convert.ToDouble(item1.value);
                                        break;
                                    case "146":
                                        mParams.LotEstimatedValue = Convert.ToDouble(item1.value);
                                        break;
                                    case "149":
                                        mParams.ValueOfLandPerLot = Convert.ToDouble(item1.value);
                                        break;
                                    case "151":
                                        mParams.ValueOfLandPerUnit = Convert.ToDouble(item1.value);
                                        break;
                                    case "153":
                                        mParams.ProfitPerLotBeforeFinancing = Convert.ToDouble(item1.value);
                                        break;
                                    case "139":
                                        mParams.TotalSalesCostPerUnit = Convert.ToDouble(item1.value);
                                        break;
                                    case "137":
                                        mParams.TotalSalesCostPerHome = Convert.ToDouble(item1.value);
                                        break;
                                    case "135":
                                        mParams.TotalSalesCostPerLot = Convert.ToDouble(item1.value);
                                        break;
                                    case "154":
                                        mParams.ProfitPerUnitBeforeFinancing = Convert.ToDouble(item1.value);
                                        break;
                                    case "127":
                                        mParams.TotalBuildingCostPerHome = Convert.ToDouble(item1.value);
                                        break;
                                    case "165":
                                        mParams.TotalFinacingCostPerUnit = Convert.ToDouble(item1.value);
                                        break;
                                    case "125":
                                        mParams.TotalBuildingCostPerUnit = Convert.ToDouble(item1.value);
                                        break;
                                    case "156":
                                        mParams.NetProfitPerLot = Convert.ToDouble(item1.value);
                                        break;
                                    case "123":
                                        mParams.TotalLandValue = Convert.ToDouble(item1.value);
                                        break;
                                    case "122":
                                        mParams.ValueofLandPerHome = Convert.ToDouble(item1.value);
                                        break;
                                    case "157":
                                        mParams.NetProfitPerHome = Convert.ToDouble(item1.value);
                                        break;
                                    case "158":
                                        mParams.NetProfitPerUnit = Convert.ToDouble(item1.value);
                                        break;
                                    case "159":
                                        mParams.NetProfitForProject = Convert.ToDouble(item1.value);
                                        break;
                                    case "160":
                                        mParams.ApartmentEstimatedValue = Convert.ToDouble(item1.value);
                                        break;
                                    case "162":
                                        mParams.TotalFinancingCost = Convert.ToDouble(item1.value);
                                        break;
                                    case "163":
                                        mParams.TotalFinancingCostPerLot = Convert.ToDouble(item1.value);
                                        break;
                                    case "164":
                                        mParams.TotalFinancingCostPerHome = Convert.ToDouble(item1.value);
                                        break;
                                    case "51":
                                        mParams.TotalNetProfit = Convert.ToDouble(item1.value);
                                        break;
                                    case "50":
                                        mParams.ProfitPerHomeBeforeFinancing = Convert.ToDouble(item1.value);
                                        break;
                                    case "145":
                                        mParams.LotEstimatedValue = Convert.ToDouble(item1.value);
                                        break;
                                    case "134":
                                        mParams.TotalSalesCostPerLot = Convert.ToDouble(item1.value);
                                        break;
                                    case "69":
                                        mParams.ApartmentEstimatedValue = Convert.ToDouble(item1.value);
                                        break;
                                    case "136":
                                        mParams.TotalSalesCostPerHome = Convert.ToDouble(item1.value);
                                        break;
                                    case "138":
                                        mParams.TotalSalesCostPerUnit = Convert.ToDouble(item1.value);
                                        break;
                                    case "140":
                                        mParams.TotalSalesCostPerProject = Convert.ToDouble(item1.value);
                                        break;
                                    case "142":
                                        mParams.UnitEstimatedValue = Convert.ToDouble(item1.value);
                                        break;
                                    case "36":
                                        mParams.HomeEstimatedValue = Convert.ToDouble(item1.value);
                                        break;
                                    case "133":
                                        mParams.TotalOperatingExpenses = Convert.ToDouble(item1.value);
                                        break;
                                    case "37":
                                        mParams.SfEstimatedValue = Convert.ToDouble(item1.value);
                                        break;
                                    case "44":
                                        mParams.MarketingAndStaging = Convert.ToDouble(item1.value);
                                        break;
                                    case "43":
                                        mParams.EscrowFees = Convert.ToDouble(item1.value);
                                        break;
                                    case "42":
                                        mParams.ExciseTax = Convert.ToDouble(item1.value);
                                        break;
                                    case "41":
                                        mParams.SalesCommission = Convert.ToDouble(item1.value);
                                        break;
                                    case "38":
                                        mParams.AADUEstimatedValue = Convert.ToDouble(item1.value);
                                        break;
                                    case "152":
                                        mParams.NetIncome = Convert.ToDouble(item1.value);
                                        break;
                                    case "39":
                                        mParams.DaduEstimatedValue = Convert.ToDouble(item1.value);
                                        break;
                                    case "132":
                                        mParams.ApartmentGrossIncome = Convert.ToDouble(item1.value);
                                        break;
                                    case "131":
                                        mParams.TotalVacancyAllowance = Convert.ToDouble(item1.value);
                                        break;
                                    case "130":
                                        mParams.OcipPolicy = Convert.ToDouble(item1.value);
                                        break;
                                    case "72":
                                        mParams.MonthlyRentPerUnit = Convert.ToDouble(item1.value);
                                        break;
                                    case "73":
                                        mParams.AnnualRent = Convert.ToDouble(item1.value);
                                        break;
                                    case "74":
                                        mParams.VacancyRate = Convert.ToDouble(item1.value);
                                        break;
                                    case "75":
                                        mParams.YearlyApartmentTaxes = Convert.ToDouble(item1.value);
                                        break;
                                    case "76":
                                        mParams.ApartmentCapRate = Convert.ToDouble(item1.value);
                                        break;
                                    case "77":
                                        mParams.OperatingExpensesPerFt = Convert.ToDouble(item1.value);
                                        break;
                                    case "78":
                                        mParams.OperatingExpenses = Convert.ToDouble(item1.value);
                                        break;
                                    case "48":
                                        mParams.TotalFinancingFees = Convert.ToDouble(item1.value);
                                        break;
                                    case "96":
                                        mParams.SellableSqftFactor = Convert.ToDouble(item1.value);
                                        break;
                                    case "86":
                                        mParams.FarMax = Convert.ToDouble(item1.value);
                                        break;
                                    case "85":
                                        mParams.FloorAreaRatio = Convert.ToDouble(item1.value);
                                        break;
                                    case "80":
                                        mParams.NumberOfUnits = Convert.ToDouble(item1.value);
                                        break;
                                    case "79":
                                        mParams.InefficientSpacePercentage = Convert.ToDouble(item1.value);
                                        break;
                                    case "71":
                                        mParams.BuildingSquareFootage = Convert.ToDouble(item1.value);
                                        break;
                                    case "70":
                                        mParams.BuildingSquareFootage = Convert.ToDouble(item1.value);
                                        break;
                                    case "129":
                                        mParams.MinimumLotSize = Convert.ToDouble(item1.value);
                                        break;
                                    case "68":
                                        mParams.MhaFeePerStructure = Convert.ToDouble(item1.value);
                                        break;
                                    case "30":
                                        mParams.Number_of_homes = Convert.ToDouble(item1.value);
                                        break;
                                    case "40":
                                        mParams.ConstructionCost = Convert.ToDouble(item1.value);
                                        break;
                                    case "49":
                                        mParams.FinancingFeesPerHome = Convert.ToDouble(item1.value);
                                        break;
                                    case "35":
                                        mParams.Dadu_size = Convert.ToDouble(item1.value);
                                        break;
                                    case "34":
                                        mParams.Dadu = Convert.ToDouble(item1.value);
                                        break;
                                    case "33":
                                        mParams.Aadu_size = Convert.ToDouble(item1.value);
                                        break;
                                    case "32":
                                        mParams.Aadu = Convert.ToDouble(item1.value);
                                        break;
                                    case "31":
                                        mParams.Listing_price = Convert.ToDouble(item1.value);
                                        break;
                                    case "67":
                                        mParams.SellableSqFootage = Convert.ToDouble(item1.value);
                                        break;

                                }
                            }

                            //structureTypesVMs.Calculations = item.Calculations;
                            structureTypes.Add(_recalcEngine.ReCalcProforma(mParams, item.StructureType));
                        }
                    }
                    else {

                        string[] mStructureType = { "Single Family", "Townhome", "Apartment", "Cottage", "Rowhouse", "Condo" };

                        foreach (string mStructType in mStructureType)
                        {
                            //structureTypesVMs = new StructureTypesVM();
                            //structureTypesVMs.StructureType = mStructType;
                            structureTypes.Add(_recalcEngine.ReCalcProforma(mParams, mStructType));
                        }

                    }
                    var UINT64FormID = await _sqlaccess.UpdateSFProformaInput(mInputParams);
                    mResponse.FormId = Convert.ToInt32(mInputParams.formId);
                    //mResponse.Address = mInputParams.address;
                    mResponse.CityId = Convert.ToInt32(mInputParams.CityId);
                    mResponse.InputUser = mInputParams.username;
                    mResponse.FormValues = mInputParams.parameters;
                    // Step 3.    Start the calculation
                    mResponse.StructureTypes = structureTypes;
                    mRet2 = await _sqlaccess.ProformaUpdateValues(mResponse.StructureTypes, mResponse.FormId);
                    mResponse = _sqlaccess.GetFormValues3(Convert.ToInt32(mInputParams.formId));
                    //OutputModel outputModel = new OutputModel();
                    //outputModel.MessageText = "Record has been successfully updated";
                    //return outputModel;
                }
                else
                {
                    var UINT64FormID = await _sqlaccess.SaveSFProformaInput(mInputParams);
                    mResponse.FormId = Convert.ToInt32(UINT64FormID);
                    // Step 2.    Assign other vital data to OUTPUT stream 
                   // mResponse.Address = mInputParams.address;
                    mResponse.CityId = Convert.ToInt32(mInputParams.CityId);
                    mResponse.InputUser = mInputParams.username;
                    mResponse.FormValues = mInputParams.parameters;
                    // Step 3.    Start the calculation
                    mResponse.StructureTypes = _calcEngine.CalcSFProforma(mParams);
                    mRet2 = await _sqlaccess.SFSaveFCalcValues(mResponse.StructureTypes, mResponse.FormId);
                    mResponse = _sqlaccess.GetFormValues3(Convert.ToInt32(UINT64FormID));
                }

                // Step 4.    Save Calculations to DB with same FormID
                //  TODO  ----  pass List<StructureTypesVM>  to a sql helper and save like you save InputParams
                
            }

            return mResponse;
        }

        public async Task<OutputModel> GetFormValues2(object request, ILambdaContext context) //task is the model if want to return
        {
            OutputModel mResponse = new OutputModel();
            mySqlAccess _sqlaccess = new mySqlAccess();
            ApiFormAuthorize mFullAuthRequest = new ApiFormAuthorize();
            FormValueInputModel mInputForm = new FormValueInputModel();
            FormValueModel mSingleResponse = new FormValueModel();
            // Verify Request
            if (request == null)
            {
                mSingleResponse.mResponseMessage = "Request cannot be empty";
                //mResponse.Add(mSingleResponse);
                context.Logger.LogLine($"ERROR:" + "RequestID: " + context.AwsRequestId + " Request string is Empty");

            }
            else
            {
                context.Logger.LogLine($"Info: " + "RequestID: " + context.AwsRequestId + ", Deserializing request string");
                try
                {
                    mFullAuthRequest = JsonConvert.DeserializeObject<ApiFormAuthorize>(request.ToString());
                    mInputForm = mFullAuthRequest.Body;
                    context.Logger.LogLine($"Info: Request sent is: " + "RequestID: " + context.AwsRequestId + " " + request.ToString());
                }
                catch (Exception ex)
                {
                    mSingleResponse.mResponseMessage = "Request is Invalid: " + ex.Message;
                    //mResponse.Add(mSingleResponse);
                    context.Logger.LogLine($"ERROR: " + "RequestID: " + context.AwsRequestId + " Invalid Request: " + ex.Message);
                    return mResponse;
                }
                context.Logger.LogLine($"Info: " + "RequestID: " + context.AwsRequestId + ", Deserializing request string");
                mResponse = _sqlaccess.GetFormValues2(mInputForm);
            }

            return mResponse;
        }

        public async Task<int> DeleteForm(object request, ILambdaContext context) //task is the model if want to return
        {
            int mResponse = 0;
            mySqlAccess _sqlaccess = new mySqlAccess();
            ApiFormAuthorize mFullAuthRequest = new ApiFormAuthorize();
            FormValueInputModel mInputForm = new FormValueInputModel();
            FormValueModel mSingleResponse = new FormValueModel();
            // Verify Request
            if (request == null)
            {
                mSingleResponse.mResponseMessage = "Request cannot be empty";
                //mResponse.Add(mSingleResponse);
                context.Logger.LogLine($"ERROR:" + "RequestID: " + context.AwsRequestId + " Request string is Empty");

            }
            else
            {
                context.Logger.LogLine($"Info: " + "RequestID: " + context.AwsRequestId + ", Deserializing request string");
                try
                {
                    mFullAuthRequest = JsonConvert.DeserializeObject<ApiFormAuthorize>(request.ToString());
                    mInputForm = mFullAuthRequest.Body;
                    context.Logger.LogLine($"Info: Request sent is: " + "RequestID: " + context.AwsRequestId + " " + request.ToString());
                }
                catch (Exception ex)
                {
                    mSingleResponse.mResponseMessage = "Request is Invalid: " + ex.Message;
                    //mResponse.Add(mSingleResponse);
                    context.Logger.LogLine($"ERROR: " + "RequestID: " + context.AwsRequestId + " Invalid Request: " + ex.Message);
                    return mResponse;
                }
                context.Logger.LogLine($"Info: " + "RequestID: " + context.AwsRequestId + ", Deserializing request string");
                mResponse = _sqlaccess.DeleteFromValues(mInputForm);
            }

            return mResponse;
        }

        public bool SavePerformaImages(List<StructureImages> structureImages,int formId)
        {    
            string myBucketName = "nwlt-images"; //your s3 bucket name goes here  
            string s3DirectoryName = "";
            mySqlAccess _sqlaccess = new mySqlAccess();
            List<StructureImages> Mimg = new List<StructureImages>();
            List<StructureImages> GetMimg = new List<StructureImages>();
            bool a;


            //GetMimg = _sqlaccess.GetStructureImages(formId);
            //for(int i=0;i<GetMimg.Count;i++)
            //{
            //    Deletefile(GetMimg[i].image_name);
            //}
            for (int i =0; i<= structureImages.Count;i++)
            {


                StructureImages img = new StructureImages();
                var ext = Path.GetExtension(structureImages[i].image_file.FileName);
                string name = Path.GetFileNameWithoutExtension(structureImages[i].image_file.FileName);
                string s3FileName = @name+ structureImages[i].image_type+ DateTime.Now.ToString("yyyy-mm-dd-HH:MM:ss") + ext;
                a = sendMyFileToS3(structureImages[i].image_file, myBucketName, s3DirectoryName, s3FileName);
                //GeneratePreSignedURL(name);
               
                img.form_id = Convert.ToInt32(structureImages[i].form_id);
                img.image_type = structureImages[i].image_type;
                img.image_url = GeneratePreSignedURL(name);
                img.image_name = name;
                img.structure_type_id = structureImages[i].structure_type_id;
                Mimg.Add(img);
            }
                 _sqlaccess.ProformaUpdateImages(Mimg,0);
            return true;
        }

        public string GeneratePreSignedURL(string bjectKey)
        {
            var credentials = new Amazon.Runtime.BasicAWSCredentials("AKIASXNDBBENTYNO2DVS", "0rkz12nqhP62xBLzqViAfEE4jDAdf54zyNgoA4/A");
            AmazonS3Client client = new AmazonS3Client(credentials, RegionEndpoint.USWest2);
            var request = new GetPreSignedUrlRequest
            {
                BucketName = "nwlt-images",
                Key = bjectKey,
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.AddYears(5)
            };

            string url = client.GetPreSignedURL(request);
            return url;
        }
        public bool sendMyFileToS3(IFormFile localFilePath, string bucketName, string subDirectoryInBucket, string fileNameInS3)
        {
            IAmazonS3 client = new AmazonS3Client(RegionEndpoint.USWest2);
            TransferUtility utility = new TransferUtility(client);
            TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();

            if (subDirectoryInBucket == "" || subDirectoryInBucket == null)
            {
                request.BucketName = bucketName; //no subdirectory just bucket name  
            }
            else
            {   // subdirectory and bucket name  
                request.BucketName = bucketName + @"/" + subDirectoryInBucket;
            }
            request.Key = fileNameInS3; //file name up in S3  
            request.InputStream = (Stream)localFilePath;
            utility.Upload(request); //commensing the transfer  

            return true; //indicate that the file was sent  
        }

        public string Deletefile(string bjectKey)
        {
            var credentials = new Amazon.Runtime.BasicAWSCredentials("AKIASXNDBBENTYNO2DVS", "0rkz12nqhP62xBLzqViAfEE4jDAdf54zyNgoA4/A");
            AmazonS3Client client = new AmazonS3Client(credentials, RegionEndpoint.USWest2);
            var fileTransferUtility = new TransferUtility(client);
            fileTransferUtility.S3Client.DeleteObjectAsync(new DeleteObjectRequest()
            {
                BucketName = "nwlt-images",
                Key = bjectKey
            });



            return "Deleted";
        }
        #endregion
    }
}
