using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyQuestionsConfigurator.Entities
{
    public class ConnectionSetting
    {
        [Required]
        [MaxLength(128)]
        public string ServerName { get; set; }
        [Required]
        [MaxLength(128)]
        public string DatabaseName { get; set; }
        [Required]
        [MaxLength(128)]
        public string UserId { get; set; }
        [Required]
        [MaxLength(128)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public ConnectionSetting(SqlConnectionStringBuilder pBuilder)
        {
            ServerName = pBuilder.DataSource;
            DatabaseName = pBuilder.InitialCatalog;
            UserId = pBuilder.UserID;
            Password = pBuilder.Password;
        }

    }
}
