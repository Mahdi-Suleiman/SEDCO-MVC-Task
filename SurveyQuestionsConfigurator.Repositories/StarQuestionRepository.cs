using SurveyQuestionsConfigurator.CommonHelpers;
using SurveyQuestionsConfigurator.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SurveyQuestionsConfigurator.Entities.Generic;

namespace SurveyQuestionsConfigurator.Repositories
{
    public class StarQuestionRepository : GenericRepository
    {
        /// <summary>
        /// Add the passed question object
        /// </summary>
        /// <returns>
        /// ErrorCode
        /// </returns>
        public ErrorCode Add(StarQuestion pStarQuestion)
        {
            try
            {
                return mDbConnect.InsertStarQuestion(pStarQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }

        /// <summary>
        /// Get a question based on it's passed object's ID then overwrite it's fields
        /// </summary>
        /// <returns>
        /// ErrorCode
        /// </returns>
        public ErrorCode Get(ref StarQuestion pStarQuestion)
        {
            try
            {
                return mDbConnect.GetStarQuestionByID(ref pStarQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }

        /// <summary>
        /// Update a question and set new values from the passed object's fields
        /// </summary>
        /// <returns>
        /// ErrorCode
        /// </returns>
        public ErrorCode Update(StarQuestion pStarQuestion)
        {
            try
            {
                return mDbConnect.UpdateStarQuestion(pStarQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }
    }
}
