using SurveyQuestionsConfigurator.CommonHelpers;
using SurveyQuestionsConfigurator.Entities.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SurveyQuestionsConfigurator.Entities.Resources.EnumResourceConstants;

namespace SurveyQuestionsConfigurator.Entities
{
    public class ConnectionSetting
    {
        [Display(Name = nameof(ResourceConstants.ServerName), ResourceType = typeof(LanguageStrings))]
        [Required(ErrorMessageResourceName = nameof(ResourceConstants.ServerNameRequired), ErrorMessageResourceType = typeof(LanguageStrings))]
        [MaxLength(128)]
        public string ServerName { get; set; }

        [Display(Name = nameof(ResourceConstants.DatabaseName), ResourceType = typeof(LanguageStrings))]
        [Required(ErrorMessageResourceName = nameof(ResourceConstants.DatabaseNameRequired), ErrorMessageResourceType = typeof(LanguageStrings))]
        [MaxLength(128)]
        public string DatabaseName { get; set; }

        [Display(Name = nameof(ResourceConstants.UserId), ResourceType = typeof(LanguageStrings))]
        [Required(ErrorMessageResourceName = nameof(ResourceConstants.UserIdRequired), ErrorMessageResourceType = typeof(LanguageStrings))]
        [MaxLength(128)]
        public string UserId { get; set; }

        [Display(Name = nameof(ResourceConstants.Password), ResourceType = typeof(LanguageStrings))]
        [Required(ErrorMessageResourceName = nameof(ResourceConstants.PasswordRequired), ErrorMessageResourceType = typeof(LanguageStrings))]
        [MaxLength(128)]
        //[DataType(DataType.Password)]
        public string Password { get; set; }

        public ConnectionSetting(SqlConnectionStringBuilder pBuilder)
        {
            try
            {
                ServerName = pBuilder.DataSource;
                DatabaseName = pBuilder.InitialCatalog;
                UserId = pBuilder.UserID;
                Password = pBuilder.Password;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

    }
}
