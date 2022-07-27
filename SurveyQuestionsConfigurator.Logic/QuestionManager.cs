using SurveyQuestionsConfigurator.CommonHelpers;
using SurveyQuestionsConfigurator.Entities;
using SurveyQuestionsConfigurator.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Configuration;
using static SurveyQuestionsConfigurator.Entities.Generic;
//using System.Web;
//using System.Web.Mvc;

namespace SurveyQuestionsConfigurator.QuestionLogic
{
    public interface IQuestionManager
    {
        void WatchForChanges();
        void InstantlyRefreshList();
        void StopWatching();
        int GetAutoRefreshTimerFromConfigFile();
        //static void ResetChachedQuestionsList(List<Question> pQuestionList) => throw new NotImplementedException();
        ErrorCode InsertSmileyQuestion(SmileyQuestion pSmileyQuestion);
        ErrorCode InsertSliderQuestion(SliderQuestion pSliderQuestion);
        ErrorCode InsertStarQuestion(StarQuestion pStarQuestion);

        ErrorCode UpdateSmileyQuestion(SmileyQuestion pSmileyQuestion);
        ErrorCode UpdateSliderQuestion(SliderQuestion pSliderQuestion);
        ErrorCode UpdateStarQuestion(StarQuestion pStarQuestion);

        ErrorCode GetAllQuestions(ref List<Question> questionsList);
        ErrorCode GetStarQuestionByID(ref StarQuestion pStarQuestion);
        ErrorCode GetSliderQuestionByID(ref SliderQuestion pSliderQuestion);
        ErrorCode GetSmileyQuestionByID(ref SmileyQuestion pSmileyQuestion);

        ErrorCode DeleteQuestionByID(int pQuestionId);

        ErrorCode CheckCommonQuestionInputFields(Question pQuestion);
        ErrorCode CheckSmileyQuestionValues(SmileyQuestion pSmileyQuestion);
        ErrorCode CheckSliderQuestionValues(SliderQuestion pSliderQuestion);
        ErrorCode CheckStarQuestionValues(StarQuestion pStarQuestion);

    }
    public class QuestionManager
    {
        #region Properties & Attributes

        private SmileyQuestionRepository mSmileyQuestionRepository;
        private SliderQuestionRepository mSliderQuestionRepository;
        private StarQuestionRepository mStarQuestionRepository;
        private GenericRepository mRepository;
        private static List<Question> mChachedQuestions; /// use to check if databsae data changed
        private static bool mKeepWatchingFlag; /// flag used in the thread that keep watching if data has changed

        #endregion

        #region Constructor
        public QuestionManager()
        {
            try
            {
                mSmileyQuestionRepository = new SmileyQuestionRepository();
                mSliderQuestionRepository = new SliderQuestionRepository();
                mStarQuestionRepository = new StarQuestionRepository();
                mRepository = new GenericRepository();
                mChachedQuestions = new List<Question>();
                mKeepWatchingFlag = true; /// keep checking for changes 
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        #endregion

        #region Delegates And Events

        public delegate void DataChanged(ErrorCode pErroCode, List<Question> pQuestionList);
        public static event DataChanged refreshDataEvent;

        /// <summary>
        /// Subscribe to a thread that continuously watch for DB changes every 30 seconds
        /// </summary>
        public void WatchForChanges()
        {
            try
            {
                int AutoRefreshTimer = GetAutoRefreshTimerFromConfigFile();
                bool firstTimeCheck = true;

                List<Question> tList = new List<Question>();
                ErrorCode tResult = ErrorCode.ERROR;


                Thread tAutoRefreshThread = new Thread(new ThreadStart(delegate
                {
                    while (mKeepWatchingFlag)
                    {
                        tList.Clear();
                        tResult = mRepository.GetAll(ref tList);

                        if (mChachedQuestions.SequenceEqual(tList) == false &&
                        !firstTimeCheck) /// if there is a difference
                        {
                            if (tResult != ErrorCode.ERROR)
                            {
                                ResetChachedQuestionsList(tList);
                            }
                            refreshDataEvent?.Invoke(tResult, tList);
                        }

                        if (firstTimeCheck) /// refresh data when DB is empty and both lists are empty and equal
                        {
                            firstTimeCheck = false;
                            refreshDataEvent?.Invoke(tResult, tList);
                        }

                        Thread.Sleep(AutoRefreshTimer);
                    }
                }));

                tAutoRefreshThread.IsBackground = true;
                tAutoRefreshThread.Start();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        /// <summary>
        /// Refresh list at anytime without any delay
        /// </summary>
        public void InstantlyRefreshList()
        {
            try
            {
                List<Question> tList = new List<Question>();
                ErrorCode tResult = ErrorCode.ERROR;

                tList.Clear();
                tResult = mRepository.GetAll(ref tList);

                refreshDataEvent.Invoke(tResult, tList);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        /// <summary>
        /// Breaks the infinite loop of "tAutoRefreshThread" 
        /// </summary>
        public void StopWatching()
        {
            try
            {
                mKeepWatchingFlag = false;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        /// <summary>
        /// Get auto refresh timer from config file
        /// </summary>
        /// <returns>
        /// int AutoRefreshTimer
        /// </returns>
        private int GetAutoRefreshTimerFromConfigFile()
        {
            try
            {
                string tSectionName = "AutoRefreshTimer";
                //var tConfigFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                Configuration tConfigFile = WebConfigurationManager.OpenWebConfiguration("~");
                var tSettings = tConfigFile.AppSettings.Settings;
                string tValue = tSettings[tSectionName].Value;
                return Int32.Parse(tValue);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                int tDefaultTimer = 30000;
                return tDefaultTimer;
            }
        }

        /// <summary>
        /// reset cached list with new passed list
        /// </summary>
        private static void ResetChachedQuestionsList(List<Question> pQuestionList)
        {
            try
            {
                lock (mChachedQuestions)
                {
                    mChachedQuestions.Clear();
                    mChachedQuestions.AddRange(pQuestionList);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        #endregion

        #region Add Question Functions
        /// <summary>
        /// Check values and pass it to repository layer
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.VALIDATION
        /// </returns>
        public ErrorCode InsertSmileyQuestion(SmileyQuestion pSmileyQuestion)
        {
            try
            {
                if (CheckSmileyQuestionValues(pSmileyQuestion) == ErrorCode.SUCCESS)
                {
                    return mSmileyQuestionRepository.Add(pSmileyQuestion);
                }
                else
                {
                    return Generic.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Generic.ErrorCode.ERROR;
            }
        }

        /// <summary>
        /// Check values and pass it to repository layer
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.VALIDATION
        /// </returns>
        public ErrorCode InsertSliderQuestion(SliderQuestion pSliderQuestion)
        {
            try
            {
                if (CheckSliderQuestionValues(pSliderQuestion) == ErrorCode.SUCCESS)
                {
                    return mSliderQuestionRepository.Add(pSliderQuestion);
                }
                else
                {
                    return Generic.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Generic.ErrorCode.ERROR;
            }
        }

        /// <summary>
        /// Check values and pass it to repository layer
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.VALIDATION
        /// </returns>
        public ErrorCode InsertStarQuestion(StarQuestion pStarQuestion)
        {
            try
            {
                if (CheckStarQuestionValues(pStarQuestion) == ErrorCode.SUCCESS)
                {
                    return mStarQuestionRepository.Add(pStarQuestion);
                }
                else
                {
                    return Generic.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Generic.ErrorCode.ERROR;
            }
        }

        #endregion

        #region Edit Question Functions

        /// <summary>
        /// Check values and pass it to repository layer
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.VALIDATION
        /// </returns>
        public ErrorCode UpdateSmileyQuestion(SmileyQuestion pSmileyQuestion)
        {
            try
            {
                if (CheckSmileyQuestionValues(pSmileyQuestion) == ErrorCode.SUCCESS)
                {
                    return mSmileyQuestionRepository.Update(pSmileyQuestion);
                }
                else
                {
                    return Generic.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Generic.ErrorCode.ERROR;
            }
        }

        /// <summary>
        /// Check values and pass it to repository layer
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.VALIDATION
        /// </returns>
        public ErrorCode UpdateSliderQuestion(SliderQuestion pSliderQuestion)
        {
            try
            {
                if (CheckSliderQuestionValues(pSliderQuestion) == ErrorCode.SUCCESS)
                {
                    return mSliderQuestionRepository.Update(pSliderQuestion);
                }
                else
                {
                    return Generic.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Generic.ErrorCode.ERROR;
            }
        }

        /// <summary>
        /// Check values and pass it to repository layer
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.VALIDATION
        /// </returns>
        public ErrorCode UpdateStarQuestion(StarQuestion pStarQuestion)
        {
            try
            {
                if (CheckStarQuestionValues(pStarQuestion) == ErrorCode.SUCCESS)
                {
                    return mStarQuestionRepository.Update(pStarQuestion);
                }
                else
                {
                    return Generic.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Generic.ErrorCode.ERROR;
            }
        }

        #endregion

        #region Get Question Functions

        /// <summary>
        /// Get a question from DB and check it values
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.VALIDATION
        /// </returns>
        public ErrorCode GetSmileyQuestionByID(ref SmileyQuestion pSmileyQuestion)
        {
            try
            {
                if (pSmileyQuestion.ID > 0)
                {
                    return mSmileyQuestionRepository.Get(ref pSmileyQuestion);
                }
                else
                {
                    return Generic.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Generic.ErrorCode.ERROR;
            }
        }

        /// <summary>
        /// Get a question from DB and check it values
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.VALIDATION
        /// </returns>
        public ErrorCode GetSliderQuestionByID(ref SliderQuestion pSliderQuestion)
        {
            try
            {
                if (pSliderQuestion.ID > 0)
                {
                    return mSliderQuestionRepository.Get(ref pSliderQuestion);
                }
                else
                {
                    return Generic.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Generic.ErrorCode.ERROR;
            }
        }

        /// <summary>
        /// Get a question from DB and check it values
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.VALIDATION
        /// </returns>
        public ErrorCode GetStarQuestionByID(ref StarQuestion pStarQuestion)
        {
            try
            {
                if (pStarQuestion.ID > 0)
                {
                    return mStarQuestionRepository.Get(ref pStarQuestion);
                }
                else
                {
                    return Generic.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Generic.ErrorCode.ERROR;
            }
        }

        /// <summary>
        /// Get all questions from DB and check it values
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.VALIDATION
        /// </returns>
        public ErrorCode GetAllQuestions(ref List<Question> questionsList)
        {
            try
            {
                var returnValue = mRepository.GetAll(ref questionsList);
                ResetChachedQuestionsList(questionsList);
                return returnValue;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Generic.ErrorCode.ERROR;
            }
        }

        #endregion

        #region Delete Question Functions

        /// <summary>
        /// Pass ID to the question of which to be deleted
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.VALIDATION
        /// </returns>
        public ErrorCode DeleteQuestionByID(int pQuestionId)
        {
            try
            {
                if (pQuestionId > 0)
                {
                    return mRepository.Delete(pQuestionId);
                }
                else
                {
                    return Generic.ErrorCode.ERROR;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Generic.ErrorCode.ERROR;
            }
        }

        #endregion

        #region Validation Functions

        /// <summary>
        /// Check coommon question values and validate them
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// </returns>
        private ErrorCode CheckCommonQuestionInputFields(Question pQuestion)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(pQuestion.Text) && pQuestion.Text.Length < 4000) //if Question text is not null or empty 
                    if (pQuestion.Order > 0)
                        return ErrorCode.SUCCESS;

                return ErrorCode.ERROR;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }

        /// <summary>
        /// Check smiley question values and validate them
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// </returns>
        public ErrorCode CheckSmileyQuestionValues(SmileyQuestion pSmileyQuestion)
        {
            try
            {
                if (CheckCommonQuestionInputFields(pSmileyQuestion) == ErrorCode.SUCCESS)
                    if (pSmileyQuestion.NumberOfSmileyFaces >= 2 && pSmileyQuestion.NumberOfSmileyFaces <= 5)
                        return ErrorCode.SUCCESS;

                return ErrorCode.ERROR;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        } /// Function end

        /// <summary>
        /// Check slider question values and validate them
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// </returns>
        public ErrorCode CheckSliderQuestionValues(SliderQuestion pSliderQuestion)
        {
            try
            {
                if (CheckCommonQuestionInputFields(pSliderQuestion) == ErrorCode.SUCCESS)
                    if (!String.IsNullOrWhiteSpace(pSliderQuestion.StartValueCaption) && pSliderQuestion.StartValueCaption.Length <= 100)
                        if (!String.IsNullOrWhiteSpace(pSliderQuestion.EndValueCaption) && pSliderQuestion.EndValueCaption.Length <= 100)
                            if (pSliderQuestion.StartValue < pSliderQuestion.EndValue)
                                return ErrorCode.SUCCESS;

                return ErrorCode.ERROR;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        } /// Function end

        /// <summary>
        /// Check star question values and validate them
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// </returns>
        public ErrorCode CheckStarQuestionValues(StarQuestion pStarQuestion)
        {
            try
            {
                if (CheckCommonQuestionInputFields(pStarQuestion) == ErrorCode.SUCCESS)
                    if (pStarQuestion.NumberOfStars >= 1 && pStarQuestion.NumberOfStars <= 10)
                        return ErrorCode.SUCCESS;

                return ErrorCode.ERROR;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        } /// Function end

        #endregion

    }

}
