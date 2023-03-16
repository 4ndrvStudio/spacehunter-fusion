using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.AzureFunction
{
    public class AzureFunctionAPIRequest
    {
        public string FunctionName;
        public AzureFunctionAPIRequest(string name)
        {
            FunctionName = name;
        }
    }


    public class AzureFunctionAPIRespone
    {
        public string Error;
    }
}


