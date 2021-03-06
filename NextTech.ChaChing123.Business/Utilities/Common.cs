﻿using System.Collections.Generic;
using System.Configuration;

using NextTech.ChaChing123.Common.Constants;
using NextTech.ChaChing123.Common.Models;
using Resource = NextTech.ChaChing123.Common.Resources.Resource;

namespace NextTech.ChaChing123.Business.Utilities
{
    using System;
    using NextTech.ChaChing123.Data.Extensions;

    /// <summary>
    /// Class Common.
    /// </summary>
    /// <seealso cref="BTMU.APF.Utilities.BaseUtilities" />
    public class Common
    {
        /// <summary>
        /// Checks the login.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="sessionKey">The session key.</param>
        /// <returns>System.Int32.</returns>
        public static ResultDTO CheckLogin(string sessionKey)
        {
            var result = new ResultDTO();
            if (string.IsNullOrEmpty(sessionKey))
            {
                result.StatusCode = ConvertErrorCodeToInt(RetCode.ECS0017);
                result.SetContentMsg();
            }
            try
            {
                CommonExtensions common = new CommonExtensions();
                return common.CheckLogin(sessionKey);
            }
            catch (Exception ex)
            {
                Logger.LogError("Call CheckLogin method error:" + ex.Message);
                result.StatusCode = ConvertErrorCodeToInt(RetCode.ECS9999);
                result.Details = "Call CheckLogin method error:" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// Converts the error code to int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.Int32.</returns>
        public static int ConvertErrorCodeToInt(RetCode value)
        {
            var retCode = -1;
            retCode = (int)Enum.Parse(typeof(RetCode), Enum.GetName(typeof(RetCode), value));
            return retCode;
        }
        
        /// <summary>
        /// Gets the configuration value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.String.</returns>
        public static string GetConfigValue(string key)
        {
            System.Configuration.AppSettingsReader settingsReader =
                new AppSettingsReader();
            //Get your key from config file to open the lock!
            string keyValue = (string)settingsReader.GetValue(key,
                typeof(string));

            return keyValue;
        }
        public static ResultDTO CheckLogin(RequestDTO obj)
        {
            var result = new ResultDTO();
            if (string.IsNullOrEmpty(obj.SessionKey) && string.IsNullOrEmpty(obj.UserName))
            {
                result.StatusCode = ConvertErrorCodeToInt(RetCode.ECS0017);
                result.SetContentMsg();
            }
            try
            {
                CommonExtensions common = new CommonExtensions();
                return common.CheckLogin(obj);
            }
            catch (Exception ex)
            {
                result.StatusCode = ConvertErrorCodeToInt(RetCode.ECS9999);
                result.Details = "Call CheckLogin method error:" + ex.Message;
            }
            return result;
        }
        public static int IsExitsUserByUserName(string userName)
        {
            try
            {
                CommonExtensions common = new CommonExtensions();
                return common.IsExitsUserByUserName(userName);
            }
            catch (Exception ex)
            {
                Logger.LogError("Method WriteLog:" + ex.Message);
            }

            return ConvertErrorCodeToInt(RetCode.ECS0019);
        }

        public static bool VerifykAccessWithRoot(string userId,string sessionKey)
        {
            if(CheckLogin(sessionKey).StatusCode != 0|| (GetConfigValue("RootId") != userId && GetConfigValue("IsCheckRoleRoot") == "1")) 
            {
                return false;
            }
            return true;
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                CommonExtensions common = new CommonExtensions();
                return common.IsValidEmail(email);
            }
            catch (Exception ex)
            {
                Logger.LogError("Method IsValidEmail WriteLog:" + ex.Message);
            }

            return false;
        }

        public static string GeStartListByYear(string strYear)
        {
            if (string.IsNullOrEmpty(strYear))
            {
                strYear = DateTime.Now.Year.ToString();
            }
            string monthList = "0101,0201,0301,0401,0501,0601,0701,0801,0901,1001,1101,1201";
            string rs = string.Empty;
            foreach(string item in monthList.Split(','))
            {
                rs += strYear+ item+",";
            }
            return rs.Remove(rs.Length-1,1);
        }

        public static string GeEndListByYear(string strYear)
        {
            string nextYear = string.Empty;
            if (string.IsNullOrEmpty(strYear))
            {
                strYear = DateTime.Now.Year.ToString();
            }
            nextYear = (int.Parse(strYear) + 1).ToString();
            string monthList = "0201,0301,0401,0501,0601,0701,0801,0901,1001,1101,1201";
            string rs = string.Empty;
            foreach (string item in monthList.Split(','))
            {
                rs += strYear+ item + ",";
            }
            rs = rs + nextYear + "0101";
            return rs;
        }

    }
}
