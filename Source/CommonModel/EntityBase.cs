using System;
using Csla.Validation;
using Nuaguil.CommonModel.Interfaces;

namespace Nuaguil.CommonModel
{
    public abstract class EntityBase<T> : IEntity
    {
        
        #region Fields
        private bool _isNew;
        [NonSerialized]
        private Csla.Validation.ValidationRules _validationRules;
        #endregion

        #region Business methods

        protected virtual void AddBusinessRules()
        {

        }

        protected virtual void AddInstanceBusinessRules()
        {

        }

        #endregion

        #region IEntity Members

        public virtual void CheckRules()
        {
            ValidationRules.CheckRules();    
        }

        public virtual Csla.Validation.BrokenRulesCollection BrokenRulesCollection
        {
            get { return ValidationRules.GetBrokenRules(); }
        }

        
        public virtual Csla.Validation.ValidationRules ValidationRules
        {
            get
            {
                if (_validationRules == null)
                    _validationRules = new Csla.Validation.ValidationRules(this);
                return _validationRules;
            }
        }

        
        public virtual bool IsValid
        {
            get { return ValidationRules.IsValid; }
        }

        [System.ComponentModel.DefaultValue(true)]
        public virtual bool IsNew
        {
            get
            {
                return _isNew;
            }
        }
        public virtual void MarkNew()
        {
            _isNew = true;
        }

        public virtual void MarkOld()
        {
            _isNew = false;
        }

        #endregion

        #region Constructors
        public EntityBase()
        {
            _isNew = true;
            AddInstanceBusinessRules();
            if (!SharedValidationRules.RulesExistFor(this.GetType()))
            {
                lock (this.GetType())
                {
                    if (!SharedValidationRules.RulesExistFor(this.GetType()))
                        AddBusinessRules();
                }
            }
        }
        #endregion

    }
}
