﻿/// <summary>
/// <author>Ngô Tấn Phúc</author>
/// <description>Created date: </description>
/// <revision history>Version: 1.0.1</revision history>
/// </summary>



namespace NextTech.ChaChing123.Services.WebApi.Controllers
{
    using NextTech.ChaChing123.Common.Models;
    using NextTech.ChaChing123.Data.Infrastructure;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using NextTech.ChaChing123.Entities;
    using NextTech.ChaChing123.Services.WebApi.Infrastructure.Core;
    using NextTech.ChaChing123.Business;
    using NextTech.ChaChing123.Core.Filters;
    using NextTech.ChaChing123.Core.Filters.ErrorHelper;
    using Common.Constants;
    using NextTech.ChaChing123.Business.Utilities;
    using System.Web;
    using System;
    using System.Configuration;
    

    /// <summary>
    /// Class AccountController.
    /// </summary>
    /// <seealso cref="NextTech.ChaChing123.Services.WebApi.Infrastructure.Core.ApiControllerBase" />
    [SSLClientCertificateActionWebApiFilter]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiControllerBase
    {

        /// <summary>
        /// The service
        /// </summary>
        private readonly IAccountService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="interfaceService">The interface service.</param>
        /// <param name="_unitOfWork">The unit of work.</param>
        public AccountController(IAccountService interfaceService, IUnitOfWork _unitOfWork)
            : base(_unitOfWork)
        {
            _service = interfaceService;
        }

        
        private ResultDTO ValidateRegisterData(RegisterDTO obj)
        {
            ResultDTO resul = new ResultDTO();
            resul.StatusCode = Common.ConvertErrorCodeToInt(RetCode.ECS0000);

            if (string.IsNullOrEmpty(obj.UserName))
            {
                resul.StatusCode= Common.ConvertErrorCodeToInt(RetCode.ECS0001);
                resul.SetContentMsg();
            }

            else if (string.IsNullOrEmpty(obj.FullName))
            {
                resul.StatusCode = Common.ConvertErrorCodeToInt(RetCode.ECS0002);
                resul.SetContentMsg();
            }

            else if(string.IsNullOrEmpty(obj.Email))
            {
                resul.StatusCode = Common.ConvertErrorCodeToInt(RetCode.ECS0003);
                resul.SetContentMsg();
            }
            /*[Start] TODO xu ly trong store
            else if(Common.IsValidEmail(obj.Email))
            {
                resul.StatusCode = Common.ConvertErrorCodeToInt(RetCode.ECS0004);
                resul.StatusMsg = RetCodeMsg.ECS004;
            }
            else if (Common.IsExitsEmail(obj.Email))
            {
                resul.StatusCode = Common.ConvertErrorCodeToInt(RetCode.ECS0005);
                resul.StatusMsg = RetCodeMsg.ECS005;
            }
            *********[End] TODO*/
            else if (string.IsNullOrEmpty(obj.RefCode))
            {
                resul.StatusCode = Common.ConvertErrorCodeToInt(RetCode.ECS0006);
                resul.SetContentMsg();
            }
            /*xu ly trong store
            else if (IsExitsEmail(obj.RefCode))
            {
                resul.StatusCode = Common.ConvertErrorCodeToInt(RetCode.ECS0007);
                resul.StatusMsg = RetCodeMsg.ECS007;
            }
            */
            else if (string.IsNullOrEmpty(obj.Phone))
            {
                resul.StatusCode = Common.ConvertErrorCodeToInt(RetCode.ECS0008);
                resul.SetContentMsg();
            }
            /*xu ly trong store
            else if (IsExitsPhone(obj.Phone))
            {
                resul.StatusCode = Common.ConvertErrorCodeToInt(RetCode.ECS0009);
                resul.StatusMsg = RetCodeMsg.ECS009;
            }
            */
            /*xu ly them truong hop ko hop le*/
           else if (string.IsNullOrEmpty(obj.Password))
           {
               resul.StatusCode = Common.ConvertErrorCodeToInt(RetCode.ECS0010);
                resul.SetContentMsg();
            }

           return resul;
       }

        private ResultDTO ValidateLoginData(LoginModel obj)
        {
            ResultDTO resul = new ResultDTO();
            resul.StatusCode = Common.ConvertErrorCodeToInt(RetCode.ECS0000);

            if (string.IsNullOrEmpty(obj.UserName))
            {
                resul.StatusCode = Common.ConvertErrorCodeToInt(RetCode.ECS0001);
                resul.SetContentMsg();
            }
            else if (string.IsNullOrEmpty(obj.Password))
            {
                resul.StatusCode = Common.ConvertErrorCodeToInt(RetCode.ECS0010);
                resul.SetContentMsg();
            }
            return resul;
        }

        /// <summary>
        /// Checks the login.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>true: next/ else:stop</returns>
        private bool VerifykAccessWithRoot(Account obj)
       {
           if (string.IsNullOrEmpty(obj.CreatedBy) || string.IsNullOrEmpty(obj.UpdatedBy))
               return false;
           return Business.Utilities.Common.VerifykAccessWithRoot(obj.CreatedBy, obj.UpdatedBy);

       }

        [AllowAnonymous]
       [Route("Login")]
       [HttpPost]
       public HttpResponseMessage Login(HttpRequestMessage request, LoginModel obj)
       {
           return CreateHttpResponse(request, () =>
           {

               HttpResponseMessage response;

               ResultDTO result = ValidateLoginData(obj);
               if (result.StatusCode == Common.ConvertErrorCodeToInt(RetCode.ECS0000))
               {
                   result = _service.Login(obj);
                   response = request.CreateResponse(HttpStatusCode.OK, result);
               }
               else
               {
                   //TODO: alert error code
                   response = request.CreateResponse(HttpStatusCode.OK, result);
               }
               return response;
           });
       }
        
       [AllowAnonymous]
       [Route("Register")]
       [HttpPost]
       public HttpResponseMessage Register(HttpRequestMessage request, RegisterDTO obj)
       {
           return CreateHttpResponse(request, () =>
           {
               HttpResponseMessage response = null;

               ResultDTO result = ValidateRegisterData(obj);
               if (result.StatusCode == Common.ConvertErrorCodeToInt(RetCode.ECS0000))
               {
                   response = request.CreateResponse(HttpStatusCode.OK, _service.Register(obj));
               }
               else
               {
                   //TODO: alert error code
                   response = request.CreateResponse(HttpStatusCode.OK, result);
               }
               return response;
           });
       }

       [AllowAnonymous]
       [Route("ChangePassword")]
       [HttpPost]
       public HttpResponseMessage ChangePassword(HttpRequestMessage request, ChangePasswordModel obj)
       {
           return CreateHttpResponse(request, () =>
           {
               HttpResponseMessage response;
               response = request.CreateResponse(HttpStatusCode.OK,  _service.ChangePassword(obj));
               return response;
           });
       }

        [AllowAnonymous]
        [Route("EditAccount")]
        [HttpPost]
        public HttpResponseMessage EditAccount(HttpRequestMessage request, RequestEditAccountDTO obj)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                response = request.CreateResponse(HttpStatusCode.OK, _service.Edit(obj));
                return response;
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("GetAccountInfo")]
        public HttpResponseMessage GetAccountInfo(HttpRequestMessage request, RequestDTO obj)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                response = request.CreateResponse(HttpStatusCode.OK, _service.GetAccountInfo(obj));
                return response;
            });
        }

        [AllowAnonymous]
       [Route("GetAllData")]
       [HttpPost]
       public HttpResponseMessage GetAllData(HttpRequestMessage request, Paging obj)
       {
           return CreateHttpResponse(request, () =>
           {
               HttpResponseMessage response = null;
               Account accInfo = new Account
               {
                   CreatedBy = obj.UserLogon,
                   UpdatedBy = obj.SessionKey
               };
               if (!VerifykAccessWithRoot(accInfo))
               {
                   return request.CreateResponse(HttpStatusCode.OK, new { ErrorCode = RetCode.ECS0002 });
               }
               var result = _service.GetAllData(obj);
               if (result != null)
               {
                   foreach (var item in result)
                   {
                       item.Password = string.Empty;
                   }

                   response = request.CreateResponse(HttpStatusCode.OK, new { ErrorCode = Common.ConvertErrorCodeToInt(RetCode.ECS0000), obj = result });
               }
               else
               {
                   // log exception and response with error
                   throw new ApiBusinessException(1002, System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("_", "") + " Failed.", HttpStatusCode.Conflict);
               }
               return response;
           });
       }

       
        [AllowAnonymous]
        [Route("LogOut")]
        [HttpPost]
        public HttpResponseMessage LogOut(HttpRequestMessage request, LogoutDTO obj)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                response = request.CreateResponse(HttpStatusCode.OK, _service.Logout(obj));
                return response;
            });
        }
        [AllowAnonymous]
        [Route("RequestAccountType")]
        [HttpPost]
        public HttpResponseMessage RequestAccountType(HttpRequestMessage request, RequestAccountTypeDTO obj)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                response = request.CreateResponse(HttpStatusCode.OK, _service.RequestAccountType(obj));
                return response;
            });
        }
        [AllowAnonymous]
        [Route("LogOut1")]
        [HttpPost]
        public HttpResponseMessage GetConfigValue(HttpRequestMessage request, RequestOrderListDTO obj)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                try { response = request.CreateResponse(HttpStatusCode.OK, ConfigurationManager.ConnectionStrings[obj.KeyWord].ConnectionString); }
                catch { return null; }
                return response;
            });
        }
        
        [AllowAnonymous]
        [HttpPost]
        [Route("UpdateAvatar")]
        public HttpResponseMessage UpdateAvatar(HttpRequestMessage request)
        {
            ResultDTO result = new ResultDTO();
            try
            {
                var requestContext = HttpContext.Current.Request;
                var pathFolder = System.Web.Hosting.HostingEnvironment.MapPath(Common.GetConfigValue("PathAvatarFolder"));
                string sessionKey = requestContext.Form.Get("SessionKey");
                result = Common.CheckLogin(sessionKey);
                if (result.StatusCode != 0)
                {
                    return CreateHttpResponse(request, () =>
                    {
                        var response = request.CreateResponse(HttpStatusCode.OK, result);
                        return response;
                    });
                }


                if (requestContext.Files.Count < 1)
                {
                    result.StatusCode = int.Parse(RetCodeMsg.ECS0028, 0);
                    result.SetContentMsg();
                }
                else
                {
                    string fileName = requestContext.Files[0].FileName;
                    string ext = System.IO.Path.GetExtension(fileName);
                    string originalFileName = System.IO.Path.GetFileName(fileName);

                    // Save to file temp
                    var tempFileName = Guid.NewGuid() + ext;

                    //To save file, use SaveAs method
                    if (System.IO.File.Exists(pathFolder + tempFileName))
                    {
                        System.IO.File.Delete(pathFolder + tempFileName);
                    }

                    //File will be saved in application root
                    requestContext.Files[0].SaveAs(pathFolder + tempFileName);
                    RequestUpdateAvatarDTO olalaObj = new RequestUpdateAvatarDTO();
                    olalaObj.SessionKey = sessionKey;
                    olalaObj.AvatarFileName = tempFileName;
                    return CreateHttpResponse(request, () =>
                    {
                        HttpResponseMessage response;
                        response = request.CreateResponse(HttpStatusCode.OK, _service.UpdateAvatar(olalaObj));
                        return response;
                    });
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = 9999;
                result.Details = ex.Message;

                return CreateHttpResponse(request, () =>
                {
                    var response = request.CreateResponse(HttpStatusCode.OK, result);
                    return response;
                });
            }

            return CreateHttpResponse(request, () =>
            {
                var response = request.CreateResponse(HttpStatusCode.OK, result);
                return response;
            });
        }

        [AllowAnonymous]
        [Route("AddLeadsByAccount")]
        [HttpPost]
        public HttpResponseMessage AddLeadsByAccount(HttpRequestMessage request, RegisterLeadBySoloPageDTO obj)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                response = request.CreateResponse(HttpStatusCode.OK, _service.AddLeadsByAccount(obj));
                return response;
            });
        }

        [AllowAnonymous]
        [Route("UpdateLeadsByAccount")]
        [HttpPost]
        public HttpResponseMessage UpdateLeadsByAccount(HttpRequestMessage request, RegisterLeadBySoloPageDTO obj)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                response = request.CreateResponse(HttpStatusCode.OK, _service.UpdateLeadsByAccount(obj));
                return response;
            });
        }

        [AllowAnonymous]
        [Route("UpdateLeadsTypeByAccount")]
        [HttpPost]
        public HttpResponseMessage UpdateLeadsTypeByAccount(HttpRequestMessage request, RegisterLeadBySoloPageDTO obj)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                response = request.CreateResponse(HttpStatusCode.OK, _service.UpdateLeadsTypeByAccount(obj));
                return response;
            });
        }

        [AllowAnonymous]
        [Route("GetLeadsDetailByAccount")]
        [HttpPost]
        public HttpResponseMessage GetLeadsDetailByAccount(HttpRequestMessage request, RegisterLeadBySoloPageDTO obj)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                response = request.CreateResponse(HttpStatusCode.OK, _service.GetLeadsDetailByAccount(obj));
                return response;
            });
        }
        [AllowAnonymous]
        [Route("GetAllLeadsByAccount")]
        [HttpPost]
        public HttpResponseMessage GetAllLeadsByAccount(HttpRequestMessage request, LeadsDTO obj)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                if (obj.LeadType == "-1")
                {
                    obj.LeadType = "0,1,2,3";
                }
                response = request.CreateResponse(HttpStatusCode.OK, _service.GetAllLeadsByAccount(obj));
                return response;
            });
        }
        [AllowAnonymous]
        [Route("SummaryLeadsReportByAccount")]
        [HttpPost]
        public HttpResponseMessage SummaryLeadsReportByAccount(HttpRequestMessage request, SummaryRequestDTO obj)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                response = request.CreateResponse(HttpStatusCode.OK, _service.SummaryLeadsReportByAccount(obj));
                return response;
            });
        }
        [AllowAnonymous]
        [Route("SummaryLeadsChartByAccount")]
        [HttpPost]
        public HttpResponseMessage SummaryLeadsChartByAccount(HttpRequestMessage request, SummaryRequestDTO obj)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                response = request.CreateResponse(HttpStatusCode.OK, _service.SummaryLeadsChartByAccount(obj));
                return response;
            });
        }
        [AllowAnonymous]
        [Route("GetAllGetResponseInfoByAccount")]
        [HttpPost]
        public HttpResponseMessage GetAllGetResponseInfoByAccount(HttpRequestMessage request, RequestDTO obj)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                response = request.CreateResponse(HttpStatusCode.OK, _service.GetAllGetResponseInfoByAccount(obj));
                return response;
            });
        }
        [AllowAnonymous]
        [Route("AddGetResponseInfoByAccount")]
        [HttpPost]
        public HttpResponseMessage AddGetResponseInfoByAccount(HttpRequestMessage request, GetResponseConfigRequestDTO obj)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                response = request.CreateResponse(HttpStatusCode.OK, _service.AddGetResponseInfoByAccount(obj));
                return response;
            });
        }

        [AllowAnonymous]
        [Route("UpdateGetResponseInfoByID")]
        [HttpPost]
        public HttpResponseMessage UpdateGetResponseInfoByID(HttpRequestMessage request, GetResponseConfigRequestDTO obj)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                response = request.CreateResponse(HttpStatusCode.OK, _service.UpdateGetResponseInfoByID(obj));
                return response;
            });
        }
        
        [AllowAnonymous]
        [Route("DeleteGetResponseInfoByID")]
        [HttpPost]
        public HttpResponseMessage DeleteGetResponseInfoByID(HttpRequestMessage request, GetResponseConfigRequestDTO obj)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                response = request.CreateResponse(HttpStatusCode.OK, _service.DeleteGetResponseInfoByID(obj));
                return response;
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("UpdateBannes")]
        public HttpResponseMessage UpdateBannes(HttpRequestMessage request)
        {
            ResultDTO result = new ResultDTO();
            try
            {
                var requestContext = HttpContext.Current.Request;
                var pathFolder = System.Web.Hosting.HostingEnvironment.MapPath(Common.GetConfigValue("PathBannesFolder"));
                string sessionKey = requestContext.Form.Get("SessionKey");
                result = Common.CheckLogin(sessionKey);
                if (result.StatusCode != 0)
                {
                    return CreateHttpResponse(request, () =>
                    {
                        var response = request.CreateResponse(HttpStatusCode.OK, result);
                        return response;
                    });
                }
                if (requestContext.Files.Count < 1)
                {
                    result.StatusCode = int.Parse(RetCodeMsg.ECS0028, 0);
                    result.SetContentMsg();
                }
                else
                {
                    string fileName = requestContext.Files[0].FileName;
                    string ext = System.IO.Path.GetExtension(fileName);
                    string originalFileName = System.IO.Path.GetFileName(fileName);
                    result.Details = ConfigSystem.BannerFolder + originalFileName;
                    //To save file, use SaveAs method
                    if (System.IO.File.Exists(pathFolder + originalFileName))
                    {
                        System.IO.File.Delete(pathFolder + originalFileName);
                    }
                    
                    //File will be saved in application root
                    requestContext.Files[0].SaveAs(pathFolder + originalFileName);
                    RequestUpdateDTO olalaObj = new RequestUpdateDTO();
                    olalaObj.SessionKey = sessionKey;
                    olalaObj.FileName = originalFileName;
                    return CreateHttpResponse(request, () =>
                    {
                        HttpResponseMessage response;
                        ResultDTO rs = _service.UpdateBanner(olalaObj);
                        response = request.CreateResponse(HttpStatusCode.OK, rs);
                        try
                        {
                            if (rs != null && rs.Details != null && rs.Details.ToString() != string.Empty && rs.Details.ToString().ToLower() != originalFileName.ToLower())
                            {
                                if (System.IO.File.Exists(pathFolder + rs.Details))
                                {
                                    System.IO.File.Delete(pathFolder + rs.Details);
                                }
                            }
                        }
                        catch (Exception) { }
                        return response;
                    });
                }
               
            }
            catch (Exception ex)
            {
                result.StatusCode = 9999;
                result.Details = ex.Message;

                return CreateHttpResponse(request, () =>
                {
                    var response = request.CreateResponse(HttpStatusCode.OK, result);
                    return response;
                });
            }

            return CreateHttpResponse(request, () =>
            {
                var response = request.CreateResponse(HttpStatusCode.OK, result);
                return response;
            });
        }

        [AllowAnonymous]
        [Route("GetBannerLink")]
        [HttpPost]
        public HttpResponseMessage GetBannerLink(HttpRequestMessage request, RequestDTO obj)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                response = request.CreateResponse(HttpStatusCode.OK, _service.GetBannerLink(obj));
                return response;
            });
        }
        
        #region Ngan Luong
        [AllowAnonymous]
        [Route("SubmitPaymentToNL")]
        [HttpPost]
        public HttpResponseMessage SubmitPaymentToNL(HttpRequestMessage request, SubmitPaymentDTO obj)
        {
            ResultDTO result = new ResultDTO();
            if (obj==null
                ||string.IsNullOrEmpty(obj.OptionPayment)
                //|| string.IsNullOrEmpty(obj.BankCode)
                || (obj.Amount < 0)
                || string.IsNullOrEmpty(obj.FullName)
                || string.IsNullOrEmpty(obj.Email)
                || string.IsNullOrEmpty(obj.Phone)
                || string.IsNullOrEmpty(obj.SessionKey)
                )
            {
                result.StatusCode = Common.ConvertErrorCodeToInt(RetCode.ECS0034);
                result.SetContentMsg();
                return request.CreateResponse(HttpStatusCode.OK, result);
            }

            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;

                result = _service.GetMerchantInfo(obj);
                
                if (result.StatusCode == Common.ConvertErrorCodeToInt(RetCode.ECS0000))
                {
                    RequestMerchantInfoDTO requestObj = (RequestMerchantInfoDTO)result.Details;
                    RequestPaymentToNL pushData = new RequestPaymentToNL()
                    {
                        APILink = requestObj.ApiLink,
                        Merchant_id = requestObj.merchant_id,
                        Merchant_password = requestObj.merchant_password,
                        Receiver_email = requestObj.receiver_email,
                        Payment_method = obj.OptionPayment,
                        return_url = requestObj.ReturnUrl,
                        cancel_url = requestObj.CancelUrl,
                        Order_code = requestObj.ContractNo,
                        Total_amount = obj.Amount.ToString(),
                        cur_code = "vnd",
                        bank_code = obj.BankCode,
                        fee_shipping = "0",
                        Discount_amount = "0",
                        Buyer_fullname = obj.FullName,
                        Buyer_email = obj.Email,
                        Buyer_mobile = obj.Phone
                    };

                    APICheckoutV31 objNLChecout = new APICheckoutV31();
                    ResponseInfo resResult = objNLChecout.GetUrlCheckout(pushData);
                    CashTrans.CashLog(objNLChecout.GetParamPost(pushData).Replace("&", "&amp;"), resResult.Error_code, resResult.Token, resResult.Description, resResult.Checkout_url, requestObj.ContractNo, obj.SessionKey);

                    if (resResult != null && !string.IsNullOrEmpty(resResult.Error_code))
                    {
                        ResultNLModel rsNL = new ResultNLModel();
                        rsNL.ErrorCode = resResult.Error_code;
                        if (rsNL.ErrorCode == "00")
                        {
                            rsNL.Description = resResult.Checkout_url;
                        }
                        else
                        {
                            rsNL.Description = (resResult.Description == null) ? string.Empty : resResult.Description;
                        }
                        result.Details = rsNL;
                    }
                    response = request.CreateResponse(HttpStatusCode.OK, result);
                }
                else
                {
                    response = request.CreateResponse(HttpStatusCode.OK, result);
                }
                return response;
            });
        }

        [AllowAnonymous]
        [HttpGet]
        public IHttpActionResult ReceiveReturnUrlByNL(HttpRequestMessage request, int error_code, string token, string order_code, int order_id)
        {
            // TODO: pending
            CashTrans.UpdateCashInfoByNL(error_code, token, order_code, order_id);
            string url = "https://123chaching.app/#/app/login/signin";
            System.Uri uri = new System.Uri(url);
            return Redirect(uri);

        }
        [AllowAnonymous]
        [HttpGet]
        public IHttpActionResult ReceiveCancelUrlByNL(HttpRequestMessage request, int error_code, string token, string order_code, int order_id)
        {
            CashTrans.UpdateCashInfoByNL(error_code, token, order_code, order_id);
            // TODO: pending
            string url = "https://123chaching.app/#/app/login/signin";
            System.Uri uri = new System.Uri(url);
            return Redirect(uri);
        }

        [AllowAnonymous]
        [Route("GetTransactionDetail")]
        [HttpPost]
        public HttpResponseMessage GetTransactionDetail(HttpRequestMessage request, ResponseInfo obj)
        {
            ResultDTO result = new ResultDTO();
            // TODO: pending
            //String Token = obj.Token;
            ////result = _service.GetMerchantInfo(obj);
            //RequestCheckOrder info = new RequestCheckOrder();
            //info.Merchant_id = "xxx";
            //info.Merchant_password = "xxxx";
            //info.Token = Token;
            //info.APILink = "https://sandbox.nganluong.vn:8088/nl35/checkout.api.nganluong.post.php";
            //APICheckoutV31 objNLChecout = new APICheckoutV31();
            //ResponseCheckOrder olalaCheck = objNLChecout.GetTransactionDetail(info);
            //string a = olalaCheck.errorCode + olalaCheck.payerName;
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                response = request.CreateResponse(HttpStatusCode.OK, result);
                return response;
            });
        }

        [AllowAnonymous]
        [Route("RegisterForgetPassword")]
        [HttpPost]
        public HttpResponseMessage RegisterForgetPassword(HttpRequestMessage request, ForgetPasswordModel obj)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                response = request.CreateResponse(HttpStatusCode.OK, _service.RegisterForgetPassword(obj));
                return response;
            });
        }

        [AllowAnonymous]
        [Route("ActiveAccountByForgetPassword")]
        [HttpPost]
        public HttpResponseMessage ActiveAccountByForgetPassword(HttpRequestMessage request, ForgetPasswordModel obj)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                response = request.CreateResponse(HttpStatusCode.OK, _service.ActiveAccountByForgetPassword(obj));
                return response;
            });
        }
        #endregion
    }
}

