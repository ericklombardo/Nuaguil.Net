using Csla.Validation;

namespace Nuaguil.Utils.Model
{
    
    public class EntityValidator<T> : ValidationRules
    {

        protected T Entity;
        
        #region Validation methods

        protected virtual void AddBusinessRules()
        {

        }

        protected virtual void AddInstanceBusinessRules()
        {

        }

        #endregion
        
        public void SetTarget(T target)
        {
            Entity = target;
            base.SetTarget(target);
        }
        public new void SetTarget(object target)
        {
            Entity = (T)target;
            base.SetTarget(target);
        }

        public EntityValidator(object target) : base(target)
        {
            Entity = (T)target;
            AddInstanceBusinessRules();
            if (!SharedValidationRules.RulesExistFor(typeof(T)))
            {
                lock (this.GetType())
                {
                    if (!SharedValidationRules.RulesExistFor(typeof(T)))
                        AddBusinessRules();
                }
            }            
        }        
        
    }
}
