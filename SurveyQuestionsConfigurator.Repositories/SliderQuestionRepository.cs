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
    public class SliderQuestionRepository : GenericRepository
    {
        /// <summary>
        /// Add the passed question object to the database through Data Access Layer
        /// </summary>
        /// <returns>
        /// ErrorCode
        /// </returns>
        public ErrorCode Add(SliderQuestion pSliderQuestion)
        {
            try
            {
                return mDbConnect.InsertSliderQuestion(pSliderQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }

        /// <summary>
        /// Get a question based on its passed object's ID then overwrite it's fields
        /// </summary>
        /// <returns>
        /// ErrorCode
        /// </returns>
        public ErrorCode Get(ref SliderQuestion pSliderQuestion)
        {
            try
            {
                return mDbConnect.GetSliderQuestionByID(ref pSliderQuestion);
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
        public ErrorCode Update(SliderQuestion pSliderQuestion)
        {
            try
            {
                return mDbConnect.UpdateSliderQuestion(pSliderQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }
    }
}
