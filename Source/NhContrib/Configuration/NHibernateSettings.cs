using System.Configuration;
using Nuaguil.Utils.DesignByContract;

namespace Nuaguil.NhContrib.Configuration
{
    /// <summary>
    /// Encapsulates a section of Web/App.config to declare which session factories are to be created.
    /// Huge kudos go out to http://msdn2.microsoft.com/en-us/library/system.configuration.configurationcollectionattribute.aspx
    /// for this technique - it was by far the best overview of the subject.
    /// </summary>
    public class NHibernateSettings : ConfigurationSection
    {

        private const string DefaultFactoryProperty = "defaultFactory";
        private const string UseSessionContextProperty = "useSessionContext";
        public const string SectionName = "nhibernateSettings";
        

        [ConfigurationProperty("sessionFactories", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(SessionFactoriesCollection), AddItemName = "sessionFactory",
            ClearItemsName = "clearFactories")]
        public SessionFactoriesCollection SessionFactories {
            get {
                SessionFactoriesCollection sessionFactoriesCollection =
                    (SessionFactoriesCollection)base["sessionFactories"];
                return sessionFactoriesCollection;
            }
        }

        [ConfigurationProperty(DefaultFactoryProperty, IsRequired = false)]
        public string DefaultFactory
        {
            get { return (string)this[DefaultFactoryProperty]; }
            set { this[DefaultFactoryProperty] = value; }
        }

        [ConfigurationProperty(UseSessionContextProperty, IsRequired = false,DefaultValue=true)]
        public bool UseSessionContext
        {
            get { return (bool)this[UseSessionContextProperty]; }
            set { this[UseSessionContextProperty] = value; }
        }


        public static NHibernateSettings GetNHibernateSettings()
        {
            NHibernateSettings settings = ConfigurationManager
                .GetSection(SectionName) as NHibernateSettings;
            
            Check.Ensure(settings != null,
                "The nhibernateSettings section was not found with ConfigurationManager.");
            return settings;
        }


    }
}
