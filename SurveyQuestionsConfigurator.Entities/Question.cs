using SurveyQuestionsConfigurator.CommonHelpers;
using SurveyQuestionsConfigurator.Entities.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SurveyQuestionsConfigurator.Entities.Generic;
using static SurveyQuestionsConfigurator.Entities.Resources.EnumResourceStrings;

namespace SurveyQuestionsConfigurator.Entities
{
    public class Question
    {
        public int ID { get; set; }
        [Display(Name = nameof(ResourceStrings.Order), ResourceType = typeof(LanguageStrings))]
        [Required(ErrorMessageResourceName = nameof(ResourceStrings.OrderRequired), ErrorMessageResourceType = typeof(LanguageStrings))]
        [Range(0, 9999999)]
        public int Order { get; set; }

        [Display(Name = nameof(ResourceStrings.Text), ResourceType = typeof(LanguageStrings))]
        [Required(ErrorMessageResourceName = nameof(ResourceStrings.TextRequired), ErrorMessageResourceType = typeof(LanguageStrings))]
        [MaxLength(3999)]
        public string Text { get; set; }

        [Display(Name = nameof(ResourceStrings.Type), ResourceType = typeof(LanguageStrings))]
        public QuestionType Type { get; set; }

        public Question(int pID)
        {
            try
            {
                ID = pID;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        public Question(int pID, QuestionType pType)
        {
            try
            {
                ID = pID;
                Type = pType;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        public Question(int pID, int pOrder, string pText, QuestionType pType)
        {
            try
            {
                ID = pID;
                Order = pOrder;
                Text = pText;
                Type = pType;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        public Question(Question pQuestion) :
            this(pQuestion.ID, pQuestion.Order, pQuestion.Text, pQuestion.Type)
        { }

        /// <summary>
        /// Compare 2 objects of the same type
        /// </summary>
        /// <param name="pObject"></param>
        /// <returns>
        /// True
        /// False
        /// </returns>
        public override bool Equals(object pObject)
        {
            try
            {
                if (
                    pObject == null ||
                    this.GetType() != pObject.GetType()
                    )
                {
                    return false;
                }

                Question q = (Question)pObject;
                if (
                    q.ID == this.ID &&
                    q.Order == this.Order &&
                    q.Text == this.Text
                    )
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return false;
            }
        }

        /// <summary>
        /// Must be overridden along with Equals
        /// </summary>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
