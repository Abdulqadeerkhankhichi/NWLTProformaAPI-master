using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Threading.Tasks;
using NWLTLambda.Models;
using System.IO;
using Amazon.SecretsManager;
using Amazon;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using Amazon.SecretsManager.Model;
using Microsoft.AspNetCore.Hosting;
using NWLTLambda.Helpers;

namespace NWLTLambda
{
    public class mySqlAccess
    {

        
       
      

        public async Task<UInt64> SaveSFProformaInput(InputModel mInputmodel)
        {
            string mConnection = GetSecret("properties");
            UInt64 mRet = 0;

            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"properties.sp_save_sfproforma", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("@mydata", JsonConvert.SerializeObject(mInputmodel));

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspStoreInputParams stored Proc. ");
                        Conn.Open();
                        // mRetNumber = await Cmd.ExecuteNonQueryAsync();
                        MySqlDataReader mSqlReader = Cmd.ExecuteReader();
                        while (mSqlReader.Read())
                        {
                            mRet = (UInt64)mSqlReader[0];
                        }
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }

            return mRet;
        }



        public async Task<int> UpdateSFProformaInput(InputModel mInputmodel)
        {
            string mConnection = GetSecret("properties");
            int mRet = 0;

            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"properties.sp_update_sfproforma", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("@mydata", JsonConvert.SerializeObject(mInputmodel));
                   // Cmd.Parameters.AddWithValue("@mydata", JsonConvert.SerializeObject(mInputmodel));
                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing sp_update_sfproforma stored Proc. ");
                        Conn.Open();
                        // mRetNumber = await Cmd.ExecuteNonQueryAsync();
                        MySqlDataReader mSqlReader = Cmd.ExecuteReader();
                        //while (mSqlReader.Read())
                        //{
                        //    mRet = (int)mSqlReader[0];
                        //}
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }

            return 0;
        }

        public async Task<int> SFSaveFCalcValues(List<StructureTypesVM> mStructureTypes, int mformID)
        {
            string mConnection = GetSecret("properties");
            int mRetNumber = 0;

            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"properties.uspSFSaveCalcValues", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("@paramList", JsonConvert.SerializeObject(mStructureTypes));
                    Cmd.Parameters.AddWithValue("@formId", mformID);

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspSFSaveCalcValues stored Proc. ");
                        Conn.Open();
                        mRetNumber = await Cmd.ExecuteNonQueryAsync();
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }


            return mRetNumber;
        }

        public int ProformaUpdateImages(List<StructureImages> mStructureImages, int mformID)
        {
            string mConnection = GetSecret("properties");
            int mRetNumber = 0;

            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"properties.uspUpdateSfProformaImages", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("@mydata", JsonConvert.SerializeObject(mStructureImages));
                    Cmd.Parameters.AddWithValue("@formId", mformID);

                    // Execute SQL Command
                    try
                    {
                        
                        Conn.Open();
                        mRetNumber =  Cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }
            return mRetNumber;
        }
        public async Task<int> ProformaUpdateValues(List<StructureTypesVM> mStructureTypes, int mformID)
        {
            string mConnection = GetSecret("properties");
            int mRetNumber = 0;

            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"properties.uspUpdateSfProforma", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("@mydata", JsonConvert.SerializeObject(mStructureTypes));
                    Cmd.Parameters.AddWithValue("@formId", mformID);

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspUpdateSfProforma stored Proc. ");
                        Conn.Open();
                        mRetNumber = await Cmd.ExecuteNonQueryAsync();
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }
            return mRetNumber;
        }
        public List<CityModel> GetCities()
        {
            string mConnection = GetSecret("properties");
            List<CityModel> mParamList = new List<CityModel>();

            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"properties.uspGetCities", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspGetCites stored Proc. ");
                        Conn.Open();
                        MySqlDataReader mSqlReader = Cmd.ExecuteReader();
                        while (mSqlReader.Read())
                        {
                            CityModel mParam = new CityModel();
                            mParam.CityId = (int)mSqlReader[0];
                            mParam.CityName = mSqlReader[1].ToString();
                            mParam.StateAbbrev = mSqlReader[2].ToString();
                            mParam.Region = mSqlReader[3].ToString();
                            mParam.TimeZone = mSqlReader[4].ToString();
                            mParamList.Add(mParam);
                        }
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }


            return mParamList;
        }

        public List<FormModel> GetForms(CityInputModel mCityInputModel)
        {
            string mConnection = GetSecret("properties");
            List<FormModel> mParamList = new List<FormModel>();

            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"properties.uspGetFormList", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("cityId", mCityInputModel.CityId);

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspStoreInputParams stored Proc. ");
                        Conn.Open();
                        MySqlDataReader mSqlReader = Cmd.ExecuteReader();
                        while (mSqlReader.Read())
                        {
                            FormModel mParam = new FormModel();
                            mParam.city_id = (int)mSqlReader[0];
                            mParam.form_id = (int)mSqlReader[1];
                            mParam.form_name = mSqlReader[2].ToString();
                            mParam.form_creator = mSqlReader[3].ToString();
                            mParam.form_creation_date = mSqlReader[4].ToString();
                            mParamList.Add(mParam);
                        }
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }


            return mParamList;
        }
        public List<FormModel> SearchForms(FormSearchInputModel mFormSearchInputModel)
        {
            string mConnection = GetSecret("properties");
            List<FormModel> mParamList = new List<FormModel>();

            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"properties.uspSearchForms", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("cityId", mFormSearchInputModel.cityId);
                    Cmd.Parameters.AddWithValue("searchString", mFormSearchInputModel.SearchString);

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspStoreInputParams stored Proc. ");
                        Conn.Open();
                        MySqlDataReader mSqlReader = Cmd.ExecuteReader();
                        while (mSqlReader.Read())
                        {
                            FormModel mParam = new FormModel();
                            mParam.city_id = (int)mSqlReader[0];
                            mParam.form_id = (int)mSqlReader[1];
                            mParam.form_name = mSqlReader[2].ToString();
                            mParam.form_creator = mSqlReader[3].ToString();
                            mParam.form_creation_date = mSqlReader[4].ToString();
                            mParamList.Add(mParam);
                        }
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }


            return mParamList;
        }

        public List<FormValueModel> GetFormValues(FormValueInputModel mInputModel)
        {
            string mConnection = GetSecret("properties");
            List<FormValueModel> mParamList = new List<FormValueModel>();

            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"properties.uspGetFormValues", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("formID", mInputModel.FormId);

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspGetFormValues stored Proc. ");
                        Conn.Open();
                        MySqlDataReader mSqlReader = Cmd.ExecuteReader();
                        while (mSqlReader.Read())
                        {
                            FormValueModel mParam = new FormValueModel();
                            mParam.property_id = (int)mSqlReader[0];
                            mParam.form_name = mSqlReader[1].ToString();
                            mParam.city_name = mSqlReader[2].ToString();
                            mParam.form_creation_date = mSqlReader[3].ToString();
                            mParam.parameter_id = (int)mSqlReader[4];
                            mParam.parameter_name = mSqlReader[5].ToString();
                            mParam.value = mSqlReader[6].ToString();
                            mParamList.Add(mParam);
                        }
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }


            return mParamList;
        }

        public OutputModel GetFormValues2(FormValueInputModel mInputModel)
        {
            string mConnection = GetSecret("properties");
            int mFormID = 0;
            string mAddress = "";
            string mCityID = "";
            string mInputUser = "";
            string mStructureTypeCurrent = "";

            OutputModel mParamList = new OutputModel();
            List<ParameterInputModel> mParamInputList = new List<ParameterInputModel>();
            List<StructureTypesVM> mStructuresList = new List<StructureTypesVM>();
            List<CalcParameterOutputModel> mListOfCalcParam;
            List<StructureImages> mStructureImagesList = new List<StructureImages>();
            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"properties.uspGetAllFormStructure", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("formID", mInputModel.FormId);

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspGetFormValues stored Proc. ");
                        Conn.Open();
                        MySqlDataReader mSqlReader1 = Cmd.ExecuteReader();
                        while (mSqlReader1.Read())
                        {
                            using (var Conn1 = new MySqlConnection(mConnection))
                            {
                                using (var Cmd1 = new MySqlCommand($"properties.uspGetAllFormValues", Conn1))
                                {
                                    // Open SQL Connection
                                    Cmd1.CommandType = System.Data.CommandType.StoredProcedure;
                                    Cmd1.Parameters.AddWithValue("formID", mInputModel.FormId);
                                    Cmd1.Parameters.AddWithValue("StructureType", mSqlReader1[3].ToString());
                                    LambdaLogger.Log($"SQL INFO: executing uspGetFormValues stored Proc. ");
                                    Conn1.Open();
                                    MySqlDataReader mSqlReader = Cmd1.ExecuteReader();
                                    StructureTypesVM mCalcParam = new StructureTypesVM();
                                    mListOfCalcParam = new List<CalcParameterOutputModel>();
                                    while (mSqlReader.Read())
                                    {


                                        //  group all form input values together
                                        if (mSqlReader[4].ToString() == "N/A")
                                        {
                                            ParameterInputModel mParamInput = new ParameterInputModel();
                                            mParamInput.ParameterId = (int)mSqlReader[5];
                                            mParamInput.ParameterName = mSqlReader[6].ToString();
                                            if (mSqlReader[11].ToString() == "2")
                                            {
                                                mParamInput.value = mSqlReader[9].ToString().Replace('"', ' ').Trim();
                                            }
                                            else
                                            {
                                                mParamInput.value = mSqlReader[9].ToString();
                                            }

                                            mParamInput.Phase = mSqlReader[7].ToString();
                                            mParamInput.ParameterTypeId = mSqlReader[11].ToString();
                                            mParamInput.ParamOrder = mSqlReader[12].ToString();
                                            mParamInputList.Add(mParamInput);
                                        }
                                        else
                                        {
                                            //   Group all by structuretype
                                            //  StructureType, List<CalcParameterOutputModel>

                                            if (mStructureTypeCurrent != mSqlReader[4].ToString())
                                            {
                                                mStructureTypeCurrent = mSqlReader[4].ToString();
                                                mCalcParam.StructureType = mStructureTypeCurrent;
                                                if (mStructureTypeCurrent != "")
                                                {

                                                }


                                            }
                                            CalcParameterOutputModel mCParamOutput = new CalcParameterOutputModel();
                                            mCParamOutput.ParameterId = mSqlReader[5].ToString();
                                            mCParamOutput.ParameterName = mSqlReader[6].ToString();
                                            mCParamOutput.value = mSqlReader[9].ToString();
                                            mCParamOutput.Phase = mSqlReader[7].ToString();
                                            mCParamOutput.ParameterTypeId = "5";
                                            mCParamOutput.ParamOrder = mSqlReader[12].ToString();
                                            mListOfCalcParam.Add(mCParamOutput);


                                        }
                                    }
                                    if (mSqlReader1[3].ToString() != "N/A")
                                    {
                                        mCalcParam.Calculations = mListOfCalcParam;
                                        mStructuresList.Add(mCalcParam);
                                        //mListOfCalcParam.Clear();
                                    }
                                }
                            }
                        }
                            // save formid, address, cityid, inputuser  to Vars
                            mFormID = (int)mSqlReader1[0];
                            mAddress = mSqlReader1[1].ToString();
                            mCityID = mSqlReader1[2].ToString();
                            mInputUser = "";  //  TODO -- add inputuser to sP
                        }
                      
                    
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }
            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"properties.uspGetProformaImages", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("formID", mInputModel.FormId);

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspGetFormValues stored Proc. ");
                        Conn.Open();
                        MySqlDataReader mSqlReader = Cmd.ExecuteReader();
                        while (mSqlReader.Read())
                        {

                            StructureImages mParamInput = new StructureImages();
                            //mParamInput.form_id = (int)mSqlReader[0];
                            mParamInput.structure_type_id = mSqlReader[1].ToString();
                            mParamInput.image_type = mSqlReader[2].ToString();
                            mParamInput.image_url = mSqlReader[4].ToString();

                            mStructureImagesList.Add(mParamInput);



                        }
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }
            mParamList.FormId = mFormID;
            mParamList.CityId = Convert.ToInt32(mCityID);
            mParamList.FormValues = mParamInputList;
            mParamList.StructureTypes = mStructuresList;
            mParamList.StructureImages = mStructureImagesList;

            return mParamList;
        }
        public int DeleteFromValues(FormValueInputModel mInputModel)
        {
            string mConnection = GetSecret("properties");
            int mParamList = 0;
            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"properties.sp_Delete_sfproforma", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("formID", mInputModel.FormId);

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing sp_Delete_sfproforma stored Proc. ");
                        Conn.Open();
                        MySqlDataReader mSqlReader1 = Cmd.ExecuteReader();
                        while (mSqlReader1.Read())
                        {
                            mParamList = (int)mSqlReader1[0];
                        }
                       
                    }


                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }
           

            return mParamList;
        }
        public List<StructureImages> GetStructureImages(int FormId)
        {
            List<StructureImages> mStructureImagesList = new List<StructureImages>();
            string mConnection = GetSecret("properties");
            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"properties.uspGetProformaImages", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("formID",FormId);

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspGetFormValues stored Proc. ");
                        Conn.Open();
                        MySqlDataReader mSqlReader = Cmd.ExecuteReader();
                        while (mSqlReader.Read())
                        {

                            StructureImages mParamInput = new StructureImages();
                            //mParamInput.form_id = (int)mSqlReader[0];
                            mParamInput.structure_type_id = mSqlReader[1].ToString();
                            mParamInput.image_type = mSqlReader[2].ToString();
                            mParamInput.image_url = mSqlReader[4].ToString();

                            mStructureImagesList.Add(mParamInput);



                        }
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }
            return mStructureImagesList;
        }
        public OutputModel GetFormValues3(int FormId)
        {
            string mConnection = GetSecret("properties");
            int mFormID = 0;
            string mAddress = "";
            string mCityID = "";
            string mInputUser = "";
            string mStructureTypeCurrent = "";

            OutputModel mParamList = new OutputModel();
            List<ParameterInputModel> mParamInputList = new List<ParameterInputModel>();
            List<StructureImages> mStructureImagesList = new List<StructureImages>();
            List<StructureTypesVM> mStructuresList = new List<StructureTypesVM>();
            List<CalcParameterOutputModel> mListOfCalcParam;
           
            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"properties.uspGetAllFormStructure", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("formID", FormId);

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspGetFormValues stored Proc. ");
                        Conn.Open();
                        MySqlDataReader mSqlReader1 = Cmd.ExecuteReader();
                        while (mSqlReader1.Read())
                        {
                            using (var Conn1 = new MySqlConnection(mConnection))
                            {
                                using (var Cmd1 = new MySqlCommand($"properties.uspGetAllFormValues", Conn1))
                                {
                                    // Open SQL Connection
                                    Cmd1.CommandType = System.Data.CommandType.StoredProcedure;
                                    Cmd1.Parameters.AddWithValue("formID", FormId);
                                    Cmd1.Parameters.AddWithValue("StructureType", mSqlReader1[3].ToString());
                                    LambdaLogger.Log($"SQL INFO: executing uspGetFormValues stored Proc. ");
                                    Conn1.Open();
                                    MySqlDataReader mSqlReader = Cmd1.ExecuteReader();
                                    StructureTypesVM mCalcParam = new StructureTypesVM();
                                    mListOfCalcParam = new List<CalcParameterOutputModel>();
                                    while (mSqlReader.Read())
                                    {


                                        //  group all form input values together
                                        if (mSqlReader[4].ToString() == "N/A")
                                        {
                                            ParameterInputModel mParamInput = new ParameterInputModel();
                                            mParamInput.ParameterId = (int)mSqlReader[5];
                                            mParamInput.ParameterName = mSqlReader[6].ToString();
                                            if (mSqlReader[11].ToString() == "2")
                                            {
                                                mParamInput.value = mSqlReader[9].ToString().Replace('"', ' ').Trim();
                                            }
                                            else
                                            {
                                                mParamInput.value = mSqlReader[9].ToString();
                                            }

                                            mParamInput.Phase = mSqlReader[7].ToString();
                                            mParamInput.ParameterTypeId = mSqlReader[11].ToString();
                                            mParamInput.ParamOrder = mSqlReader[12].ToString();
                                            mParamInputList.Add(mParamInput);
                                        }
                                        else
                                        {
                                            //   Group all by structuretype
                                            //  StructureType, List<CalcParameterOutputModel>

                                            if (mStructureTypeCurrent != mSqlReader[4].ToString())
                                            {
                                                mStructureTypeCurrent = mSqlReader[4].ToString();
                                                mCalcParam.StructureType = mStructureTypeCurrent;
                                                if (mStructureTypeCurrent != "")
                                                {

                                                }


                                            }
                                            CalcParameterOutputModel mCParamOutput = new CalcParameterOutputModel();
                                            mCParamOutput.ParameterId = mSqlReader[5].ToString();
                                            mCParamOutput.ParameterName = mSqlReader[6].ToString();
                                            mCParamOutput.value = mSqlReader[9].ToString();
                                            mCParamOutput.Phase = mSqlReader[7].ToString();
                                            mCParamOutput.ParameterTypeId = "5";//mSqlReader[11].ToString();
                                            mCParamOutput.ParamOrder = mSqlReader[12].ToString();
                                            mListOfCalcParam.Add(mCParamOutput);


                                        }
                                    }
                                    if (mSqlReader1[3].ToString() != "N/A")
                                    {
                                        mCalcParam.Calculations = mListOfCalcParam;
                                        mStructuresList.Add(mCalcParam);
                                        //mListOfCalcParam.Clear();
                                    }
                                }
                            }
                        }
                        // save formid, address, cityid, inputuser  to Vars
                        mFormID = (int)mSqlReader1[0];
                        mAddress = mSqlReader1[1].ToString();
                        mCityID = mSqlReader1[2].ToString();
                        mInputUser = "";  //  TODO -- add inputuser to sP
                    }


                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }
            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"properties.uspGetProformaImages", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("formID", FormId);

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspGetFormValues stored Proc. ");
                        Conn.Open();
                        MySqlDataReader mSqlReader = Cmd.ExecuteReader();
                        while (mSqlReader.Read())
                        {
                            
                                StructureImages mParamInput = new StructureImages();
                               // mParamInput.form_id = (int)mSqlReader[0];
                                mParamInput.structure_type_id = mSqlReader[1].ToString();
                                mParamInput.image_type = mSqlReader[2].ToString();
                                mParamInput.image_url = mSqlReader[4].ToString();
                                
                                mStructureImagesList.Add(mParamInput);
                            


                        }
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }
            mParamList.FormId = mFormID;
            mParamList.CityId = Convert.ToInt32(mCityID);
            mParamList.FormValues = mParamInputList;
            mParamList.StructureTypes = mStructuresList;
            mParamList.StructureImages = mStructureImagesList;
            return mParamList;
        }
        public List<ParametersOutputModel> GetCityParams(CityInputModel mCityInputModel)
        {
            string mConnection = GetSecret("properties");
            List<ParametersOutputModel> mParamList = new List<ParametersOutputModel>();

            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"properties.uspGetParameters", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("cityId", mCityInputModel.CityId);

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspStoreInputParams stored Proc. ");
                        Conn.Open();
                        MySqlDataReader mSqlReader = Cmd.ExecuteReader();
                        while (mSqlReader.Read())
                        {
                            ParametersOutputModel mParam = new ParametersOutputModel();
                            mParam.ParameterId = (int)mSqlReader[0];
                            mParam.ParameterName = mSqlReader[1].ToString();
                            mParam.HtmlHeader = mSqlReader[2].ToString();
                            mParam.HtmlTag = mSqlReader[3].ToString();
                            mParam.Phase = mSqlReader[4].ToString();
                            mParam.Stage = mSqlReader[5].ToString();
                            mParam.ParameterTypeId = (int)mSqlReader[6];
                            mParam.ParamOrder = (int)mSqlReader[7];
                            mParamList.Add(mParam);
                        }
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }


            return mParamList;
        }

        public List<AllParametersOutputModel> GetAllParams(CityInputModel mCityInputModel)
        {
            string mConnection = GetSecret("properties");
            List<AllParametersOutputModel> mParamList = new List<AllParametersOutputModel>();

            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"properties.uspGetAllParameters", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("cityId", mCityInputModel.CityId);

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspGetAllParameters stored Proc. ");
                        Conn.Open();
                        MySqlDataReader mSqlReader = Cmd.ExecuteReader();
                        while (mSqlReader.Read())
                        {
                            AllParametersOutputModel mParam = new AllParametersOutputModel();
                            mParam.ParameterId = (int)mSqlReader[0];
                            mParam.ParameterName = mSqlReader[1].ToString();
                            mParam.HtmlHeader = mSqlReader[2].ToString();
                            mParam.HtmlTag = mSqlReader[3].ToString();
                            mParam.Phase = mSqlReader[4].ToString();
                            mParam.Stage = mSqlReader[5].ToString();
                            mParam.ParameterTypeId = (int)mSqlReader[5];
                            mParam.ParamOrder = (int)mSqlReader[6];
                            mParamList.Add(mParam);
                        }
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }


            return mParamList;
        }

        public List<ParametersOutputModel> GetCalcParams(CityInputModel mCityInputModel)
        {
            string mConnection = GetSecret("properties");
            List<ParametersOutputModel> mParamList = new List<ParametersOutputModel>();

            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"properties.uspGetCalcParameters", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("cityId", mCityInputModel.CityId);

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspGetCalcParameters stored Proc. ");
                        Conn.Open();
                        MySqlDataReader mSqlReader = Cmd.ExecuteReader();
                        while (mSqlReader.Read())
                        {
                            ParametersOutputModel mParam = new ParametersOutputModel();
                            mParam.ParameterId = (int)mSqlReader[0];
                            mParam.ParameterName = mSqlReader[1].ToString();
                            mParam.HtmlHeader = mSqlReader[2].ToString();
                            mParam.HtmlTag = mSqlReader[3].ToString();
                            mParam.Phase = mSqlReader[4].ToString();
                            mParam.ParameterTypeId = (int)mSqlReader[5];
                            mParam.ParamOrder = (int)mSqlReader[6];
                            mParamList.Add(mParam);
                        }
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }


            return mParamList;
        }

        public List<ParametersOutputModel2> GetFormParams(CityInputModel mCityInputModel)
        {
            string mConnection = GetSecret("parameters");
            List<ParametersOutputModel2> mParamList = new List<ParametersOutputModel2>();

            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"parameters.uspGetFormParameters", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("cityId", mCityInputModel.CityId);

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspStoreInputParams stored Proc. ");
                        Conn.Open();
                        MySqlDataReader mSqlReader = Cmd.ExecuteReader();
                        while (mSqlReader.Read())
                        {
                            ParametersOutputModel2 mParam = new ParametersOutputModel2();
                            mParam.ParameterId = (int)mSqlReader[0];
                            mParam.ParameterName = mSqlReader[1].ToString();
                            mParam.HtmlHeader = mSqlReader[2].ToString();
                            mParam.HtmlTag = mSqlReader[3].ToString();
                            mParam.Phase = mSqlReader[4].ToString();
                            mParam.Stage = mSqlReader[5].ToString();
                            mParam.ParameterTypeId = (int)mSqlReader[6];
                            mParam.ParamOrder = (int)mSqlReader[7];
                            mParamList.Add(mParam);
                        }
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }


            return mParamList;
        }

        public List<ParameterValuesOutputModel> GetParameterValues(ParameterValuesInputModel mParameterValuesInputModel)
        {

            string mConnection = GetSecret("parameters");

            List<ParameterValuesOutputModel> mListValues = new List<ParameterValuesOutputModel>();

            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"parameters.uspGetParameterValues", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("cityId", mParameterValuesInputModel.CityId);
                    Cmd.Parameters.AddWithValue("parameterID", mParameterValuesInputModel.ParameterId);

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspGetParameterValues stored Proc. ");
                        Conn.Open();
                        MySqlDataReader mSqlReader = Cmd.ExecuteReader();
                        while (mSqlReader.Read())
                        {
                            ParameterValuesOutputModel mParamValue = new ParameterValuesOutputModel();
                            mParamValue.ParameterId = (int)mSqlReader[0];
                            mParamValue.ParameterName = mSqlReader[1].ToString();
                            mParamValue.HtmlHeader = mSqlReader[2].ToString();
                            mParamValue.HtmlTag = mSqlReader[3].ToString();
                            mParamValue.Phase = mSqlReader[4].ToString();
                            mParamValue.Value = mSqlReader[5].ToString();
                            mListValues.Add(mParamValue);
                        }
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }


            return mListValues;
        }

        public decimal GetBaseCost(int mCityID, string isHighEnd, string mStructureType) 
        {
            string mConnection = GetSecret("lookups");
            decimal mReturn = 0;

            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"lookups.uspLookupBuildingBaseCost", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("cityId", mCityID);
                    Cmd.Parameters.AddWithValue("isHighEnd", isHighEnd);
                    Cmd.Parameters.AddWithValue("structureType", mStructureType);

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspStoreInputParams stored Proc. ");
                        Conn.Open();
                        MySqlDataReader mSqlReader = Cmd.ExecuteReader();
                        while (mSqlReader.Read())
                        {
                            mReturn =  (decimal)mSqlReader[0];
                        }
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }

            return mReturn;
        }

        public decimal GetDensityLimit(int mCityID, string mZoning , string mStructureType, string hasMhaSuffix, string isCorner)
        {
            string mConnection = GetSecret("lookups");
            decimal mReturn = 0;

            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"lookups.uspLookupDensityLimit", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("cityId", mCityID);
                    Cmd.Parameters.AddWithValue("zoningCode", mZoning);
                    Cmd.Parameters.AddWithValue("structureType", mStructureType);
                    Cmd.Parameters.AddWithValue("hasMhaSuffix", hasMhaSuffix);
                    Cmd.Parameters.AddWithValue("isCorner", isCorner);


                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspStoreInputParams stored Proc. ");
                        Conn.Open();
                        MySqlDataReader mSqlReader = Cmd.ExecuteReader();
                        while (mSqlReader.Read())
                        {
                            mReturn = (decimal)mSqlReader[0];
                        }
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }

            return mReturn;
        }

        public decimal GetFAR(int mCityID, string mZoning, string mStructureType, string hasMhaSuffix, string isGrowthArea)
        {
            string mConnection = GetSecret("lookups");
            decimal mReturn = 0;

            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"lookups.uspLookupFAR", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("cityId", mCityID);
                    Cmd.Parameters.AddWithValue("zoningCode", mZoning);
                    Cmd.Parameters.AddWithValue("structureType", mStructureType);
                    Cmd.Parameters.AddWithValue("hasMhaSuffix", hasMhaSuffix);
                    Cmd.Parameters.AddWithValue("isGrowthArea", isGrowthArea);

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspStoreInputParams stored Proc. ");
                        Conn.Open();
                        MySqlDataReader mSqlReader = Cmd.ExecuteReader();
                        while (mSqlReader.Read())
                        {
                            mReturn = (decimal)mSqlReader[0];
                        }
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }

            return mReturn;
        }

        public decimal GetMHAFee(int mCityID, string mhaAreaID, string mhaSuffixID)
        {
            string mConnection = GetSecret("lookups");
            decimal mReturn = 0;

            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"lookups.uspLookupMHAFee", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("cityId", mCityID);
                    Cmd.Parameters.AddWithValue("mhaArea", mhaAreaID);
                    Cmd.Parameters.AddWithValue("mhaSuffix", mhaSuffixID);

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspStoreInputParams stored Proc. ");
                        Conn.Open();
                        MySqlDataReader mSqlReader = Cmd.ExecuteReader();
                        while (mSqlReader.Read())
                        {
                            mReturn = (decimal)mSqlReader[0];
                        }
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }

            return mReturn;
        }

        public string GetSecret(string mSchema)
        {
            string secretName = "databaseconn";
            string region = "us-west-2";
            string secret = "";

            MemoryStream memoryStream = new MemoryStream();

            IAmazonSecretsManager client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));

            GetSecretValueRequest request = new GetSecretValueRequest();
            request.SecretId = secretName;
            request.VersionStage = "AWSCURRENT"; // VersionStage defaults to AWSCURRENT if unspecified.

            GetSecretValueResponse response = null;

            // In this sample we only handle the specific exceptions for the 'GetSecretValue' API.
            // See https://docs.aws.amazon.com/secretsmanager/latest/apireference/API_GetSecretValue.html
            // We rethrow the exception by default.

            try
            {
                response = client.GetSecretValueAsync(request).Result;
            }
            catch (DecryptionFailureException e)
            {
                // Secrets Manager can't decrypt the protected secret text using the provided KMS key.
                // Deal with the exception here, and/or rethrow at your discretion.
                string mMessage = e.Message;
                throw;
            }
            catch (InternalServiceErrorException e)
            {
                // An error occurred on the server side.
                // Deal with the exception here, and/or rethrow at your discretion.
                string mMessage = e.Message;
                throw;
            }
            catch (InvalidParameterException e)
            {
                // You provided an invalid value for a parameter.
                // Deal with the exception here, and/or rethrow at your discretion
                string mMessage = e.Message;
                throw;
            }
            catch (InvalidRequestException e)
            {
                // You provided a parameter value that is not valid for the current state of the resource.
                // Deal with the exception here, and/or rethrow at your discretion.
                string mMessage = e.Message;
                throw;
            }
            catch (ResourceNotFoundException e)
            {
                // We can't find the resource that you asked for.
                // Deal with the exception here, and/or rethrow at your discretion.
                string mMessage = e.Message;
                throw;
            }
            catch (System.AggregateException e)
            {
                // More than one of the above exceptions were triggered.
                // Deal with the exception here, and/or rethrow at your discretion.
                string mMessage = e.Message;
                throw;
            }

            // Decrypts secret using the associated KMS CMK.
            // Depending on whether the secret is a string or binary, one of these fields will be populated.
            if (response.SecretString != null)
            {
                secret = response.SecretString;
                SecretDBModel mDBModel = JsonConvert.DeserializeObject<SecretDBModel>(secret);
                MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
                builder.Server = mDBModel.host;
                builder.UserID = mDBModel.username;
                builder.Password = mDBModel.password;
                builder.Database = mSchema;
                builder.Port = mDBModel.port;
                secret = builder.ToString();
            }
            else
            {
                memoryStream = response.SecretBinary;
                StreamReader reader = new StreamReader(memoryStream);
                string decodedBinarySecret = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd()));
                secret = decodedBinarySecret;

                secret = "";
            }
            return secret;
            // Your code goes here.
        }



    }
}
