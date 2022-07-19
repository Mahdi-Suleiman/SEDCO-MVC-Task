using SurveyQuestionsConfigurator.CommonHelpers;
using SurveyQuestionsConfigurator.DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SurveyQuestionsConfigurator.Entities.Generic;

namespace SurveyQuestionsConfigurator.QuestionLogic
{
    public class ConnectionSettingsManager
    {
        #region Properties & Attributes

        DbConnect mDbConnect; /// Data Access Layer

        #endregion

        #region Constructor

        public ConnectionSettingsManager()
        {
            try
            {
                mDbConnect = new DbConnect();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); /// write error to log file
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get connection string from Data Access Layer
        /// </summary>
        /// <returns>
        /// SqlConnectionStringBuilder
        /// </returns>
        public SqlConnectionStringBuilder GetConnectionString()
        {
            try
            {
                return mDbConnect.GetConnectionString();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); /// write error to log file
                return null;
            }
        }

        /// <summary>
        /// Check connectivity of the passed SqlConnectionStringBuilder connection string and return the corresponding ErrorCode
        /// </summary>
        /// <returns>
        /// ErrorCode
        /// </returns>
        public ErrorCode CheckConnectivity(SqlConnectionStringBuilder pBuilder)
        {
            try
            {
                return mDbConnect.CheckConnectivity(pBuilder);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); /// write error to log file
                return ErrorCode.ERROR;
            }
        }

        /// <summary>
        /// Save passed SqlConnectionStringBuilder connection string
        /// </summary>
        /// <returns>
        /// ErrorCode
        /// </returns>
        public ErrorCode SaveConnectionString(SqlConnectionStringBuilder pBuilder)
        {
            try
            {
                return mDbConnect.SaveConnectionString(pBuilder);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); /// write error to log file
                return ErrorCode.ERROR;
            }
        }

        #endregion

        #region Validation Methods

        /// <summary>
        /// Check connection string validation
        /// </summary>
        /// <returns>
        /// ErrorCode
        /// </returns>
        public ErrorCode CheckConnectionStringInputFields(SqlConnectionStringBuilder pBuilder)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(pBuilder.DataSource.ToString()) && pBuilder.DataSource.Length <= 128)
                    if (!String.IsNullOrWhiteSpace(pBuilder.InitialCatalog.ToString()) && pBuilder.InitialCatalog.Length <= 128)
                        if (!String.IsNullOrWhiteSpace(pBuilder.UserID.ToString()) && pBuilder.UserID.Length <= 128)
                            if (!String.IsNullOrWhiteSpace(pBuilder.Password.ToString()) && pBuilder.Password.Length <= 128)
                                return ErrorCode.SUCCESS;
                return ErrorCode.ERROR;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex); /// write error to log file
                return ErrorCode.ERROR;
            }
        }

        #endregion
    }
}
