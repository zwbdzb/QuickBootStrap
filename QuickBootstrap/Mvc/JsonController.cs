using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuickBootstrap.Validations;

namespace QuickBootstrap.Mvc
{
    public class JsonController : Controller
    {
        private List<ResponseError> _validationErrors;
        private List<ResponseError> ValidationErrors
        {
            get { return _validationErrors ?? (_validationErrors = new List<ResponseError>()); }
        }

        protected bool HasValidationError
        {
            get { return ValidationErrors.Count > 0; }
        }

        private  ActionResult Json(object result, JsonSerializerSettings jsetting = null)
        {
            if (result == null)
            {
                return Content("", "text/plain");
            }
            if (jsetting == null)
            {
                jsetting = new JsonSerializerSettings();
            }
            if (result is ResponseError || result is ResponseError[])
            {
                Response.StatusCode = 403;
            }
            string json;
            try
            {
                json = JsonConvert.SerializeObject(result, jsetting);
            }
            catch (Exception ex)
            {
                return new JsonResponseResult(500, JsonConvert.SerializeObject(ex));
            }
            return new JsonResponseResult(200, json);
        }

        [NonAction]
        protected ActionResult ErrorJson()
        {
            Response.StatusCode = 403;
            return Json(ValidationErrors.ToArray());
        }

        [NonAction]
        protected ActionResult ErrorJson(string msg)
        {
            var json = JsonConvert.SerializeObject(new
            {
                msg
            });
            return new JsonResponseResult(403, json);
        }


        private string GetRequestBody()
        {
            using (var reader = new StreamReader(Request.GetBufferedInputStream()))
            {
                return reader.ReadToEnd();
            }
        }

      
        private JObject _requestBody;
        private NameValueCollection _collection;
        private int _bodyType = -1;

        private void EnsureBodyType()
        {
            if (_bodyType != -1)
            {
                return;
            }
            if (Request.Form.HasKeys())
            {
                _bodyType = 0;
                return;
            }
            var reqBody = GetRequestBody();
            try
            {
                _requestBody = JObject.Parse(reqBody);
                _bodyType = 1;
            }
            catch (Exception)
            {
                _collection = HttpUtility.ParseQueryString(reqBody);
                _bodyType = 2;
            }
        }

        private string GetFormRawValue(string fieldName)
        {
            EnsureBodyType();
            string rawValue = null;
            if (_bodyType == 0)
            {
                rawValue = Request.Form[fieldName];
            }
            else if (_bodyType == 1)
            {
                var jobj = _requestBody[fieldName];
                if (jobj != null)
                {
                    rawValue = jobj.ToString();
                }
            }
            else
            {
                rawValue = _collection[fieldName];
            }
            return rawValue;
        }

        [NonAction]
        protected string GetFormString(string fieldName, params IValidationRule[] validations)
        {
            var rawValue = GetFormRawValue(fieldName);
            if (validations != null && validations.Length > 0)
            {
                var result = ValidateValue(fieldName, rawValue, validations);
                if (result != null)
                {
                    ValidationErrors.Add(result);
                    return null;
                }
            }
            return rawValue;
        }

        [NonAction]
        protected bool GetFormBooleam(string fieldName)
        {
            var rawValue = GetFormRawValue(fieldName);
            if (string.IsNullOrEmpty(rawValue))
            {
                return false;
            }
            return rawValue.Trim().Equals("1")
                   || rawValue.Trim().Equals("true", StringComparison.InvariantCultureIgnoreCase);
        }

        [NonAction]
        protected int GetFormInt(string fieldName, params IValidationRule[] validations)
        {
            return GetFormValue<short>(fieldName, short.TryParse, validations);
        }

        [NonAction]
        protected short GetFormShort(string fieldName, params IValidationRule[] validations)
        {
            return GetFormValue<short>(fieldName, short.TryParse, validations);
        }

        [NonAction]
        protected long GetFormLong(string fieldName, params IValidationRule[] validations)
        {
            return GetFormValue<long>(fieldName, long.TryParse, validations);
        }

        [NonAction]
        protected Guid GetFormGuid(string fieldName, params IValidationRule[] validations)
        {
            return GetFormValue<Guid>(fieldName, Guid.TryParse, validations);
        }

        [NonAction]
        protected decimal GetFormDecimal(string fieldName, params IValidationRule[] validations)
        {
            return GetFormValue<decimal>(fieldName, decimal.TryParse, validations);
        }

        [NonAction]
        protected T GetFormValue<T>(string fieldName, Func<string, T, bool> tryParse, params IValidationRule[] validations)
        {
            var rawValue = GetFormRawValue(fieldName);
            var firstValidation = validations != null ? validations.FirstOrDefault() : null;
            if (firstValidation != null && firstValidation is RequiredValidation)
            {
                var res = ValidateValue(fieldName, rawValue, firstValidation);
                if (res != null)
                {
                    ValidationErrors.Add(res);
                    return default(T);
                }
            }
            var restValidations = firstValidation != null ? validations.Where(x => x != firstValidation).ToArray() : validations;
            T value;
            if (!tryParse(rawValue,out  value))
            {
                ValidationErrors.Add(new ResponseError { Field = fieldName, Msg = "cannot convert string \"" + rawValue + "\" to " + typeof(T) });
            }
            if (restValidations != null && restValidations.Length > 0)
            {
                var result = ValidateValue(fieldName, value, restValidations);
                if (result != null)
                {
                    ValidationErrors.Add(result);
                    return default(T);
                }
            }
            return value;
        }

        [NonAction]
        protected DateTime GetFormDateTime(string fieldName, string formarString, params IValidationRule[] validations)
        {
            var rawValue = GetFormRawValue(fieldName);
            var firstValidation = validations != null ? validations.FirstOrDefault() : null;
            if (firstValidation != null && firstValidation is RequiredValidation)
            {
                var res = ValidateValue(fieldName, rawValue, firstValidation);
                if (res != null)
                {
                    ValidationErrors.Add(res);
                    return default(DateTime);
                }
            }
            var restValidations = firstValidation != null ? validations.Where(x => x != firstValidation).ToArray() : validations;
            DateTime value;
            if (!DateTime.TryParseExact(rawValue, formarString, CultureInfo.InvariantCulture, DateTimeStyles.None, out value))
            {
                ValidationErrors.Add(new ResponseError { Field = fieldName, Msg = "cannot convert string \"" + rawValue + "\" to datetime." });
                return default(DateTime);
            }
            if (restValidations != null && restValidations.Length > 0)
            {
                var result = ValidateValue(fieldName, value, restValidations);
                if (result != null)
                {
                    ValidationErrors.Add(result);
                    return default(DateTime);
                }
            }
            return value;
        }

        [NonAction]
        protected ResponseError ValidateValue<T>(string fieldName, T value, params IValidationRule[] validations)
        {
            return (from v in validations
                    where !v.Validate(value)
                    select new ResponseError
                    {
                        Field = fieldName,
                        Msg = fieldName + "_" + v.ErrorKey 
                    }).FirstOrDefault();
        }

        [NonAction]
        protected string GetQueryString(string fieldName, params IValidationRule[] validations)
        {
            var rawValue = Request.QueryString[fieldName];
            if (validations != null && validations.Length > 0)
            {
                var result = ValidateValue(fieldName, rawValue, validations);
                if (result != null)
                {
                    ValidationErrors.Add(result);
                    return null;
                }
            }
            return rawValue;
        }

        [NonAction]
        protected int GetQueryInt(string fieldName, params IValidationRule[] validations)
        {
            return GetQueryValue<int>(fieldName, int.TryParse, validations);
        }

        protected DateTime GetQueryDate(string fieldName, string format, params IValidationRule[] validations)
        {
            var rawValue = Request.QueryString[fieldName];
            var validation = validations != null ? validations.FirstOrDefault() : null;
            if (validation is RequiredValidation)
            {
                var res = ValidateValue(fieldName, rawValue, validation);
                if (res != null)
                {
                    ValidationErrors.Add(res);
                    return DateTime.MinValue;
                }
            }
            DateTime value;
            if (!DateTime.TryParseExact(rawValue, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out value))
            {
                ValidationErrors.Add(new ResponseError { Field = fieldName, Msg = "cannot convert string \"" + rawValue + "\" to " + typeof(DateTime) });
            }
            var restValidations = validation != null ? validations.Where(x => x != validation).ToArray() : validations;
            if (restValidations != null && restValidations.Length > 0)
            {
                var result = ValidateValue(fieldName, value, restValidations);
                if (result != null)
                {
                    ValidationErrors.Add(result);
                    return DateTime.MinValue;
                }
            }
            return value;
        }

        [NonAction]
        protected T GetQueryValue<T>(string fieldName, Func<string, T, bool> tryParse, params IValidationRule[] validations)
        {
            var rawValue = Request.QueryString[fieldName];
            var firstValidation = validations != null ? validations.FirstOrDefault() : null;
            if (firstValidation is RequiredValidation)
            {
                var res = ValidateValue(fieldName, rawValue, firstValidation);
                if (res != null)
                {
                    ValidationErrors.Add(res);
                    return default(T);
                }
            }
            var restValidations = firstValidation != null ? validations.Where(x => x != firstValidation).ToArray() : validations;
            T value;
            if (!tryParse(rawValue, out value))
            {
                ValidationErrors.Add(new ResponseError { Field = fieldName, Msg = "cannot convert string \"" + rawValue + "\" to " + typeof(T) });
            }
            if (restValidations != null && restValidations.Length > 0)
            {
                var result = ValidateValue(fieldName, value, restValidations);
                if (result != null)
                {
                    ValidationErrors.Add(result);
                    return default(T);
                }
            }
            return value;
        }

        [NonAction]
        protected T GetRequestModel<T>(string model = null)
        {
            var body = string.IsNullOrEmpty(model) ? Request.Form["model"] : model;

            if (string.IsNullOrWhiteSpace(body))
            {
                return default(T);
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(body);
            }
            catch
            {
                return default(T);
            }
        }
    }
}