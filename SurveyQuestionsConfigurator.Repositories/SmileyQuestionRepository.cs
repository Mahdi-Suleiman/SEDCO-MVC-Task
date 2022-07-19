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
    public class SmileyQuestionRepository : GenericRepository
    {
        /// <summary>
        /// Add the passed question object
        /// </summary>
        /// <returns>
        /// ErrorCode
        /// </returns>
        public ErrorCode Add(SmileyQuestion pSmileyQuestion)
        {
            try
            {
                return mDbConnect.InsertSmileyQuestion(pSmileyQuestion);
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
        public ErrorCode Get(ref SmileyQuestion pSmileyQuestion)
        {
            try
            {
                return mDbConnect.GetSmileyQuestionByID(ref pSmileyQuestion);
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
        public ErrorCode Update(SmileyQuestion pSmileyQuestion)
        {
            try
            {
                return mDbConnect.UpdateSmileyQuestion(pSmileyQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }
    }
}
