namespace Nuaguil.CommonModel.Interfaces
{
    public interface IEntity
    {
        void MarkNew();
        void MarkOld();
        bool IsNew { get; }
        void CheckRules();
        bool IsValid { get; }
        Csla.Validation.BrokenRulesCollection BrokenRulesCollection { get; }
        Csla.Validation.ValidationRules ValidationRules { get; }
    }
}
