using System;
using Csla.Validation;

namespace Nuaguil.CommonModel
{
    public class ValidatorException : Exception
    {

        private BrokenRulesCollection _brokenRules = null;

        public BrokenRulesCollection BrokenRules
        {
            get
            {
                return _brokenRules;
            }
        }

        public ValidatorException(string message)
            : base(message)
        {
            
        }
        public ValidatorException(string message, BrokenRulesCollection brokenRules)
            : base(message)
        {
            _brokenRules = brokenRules;
        }
    }
}
