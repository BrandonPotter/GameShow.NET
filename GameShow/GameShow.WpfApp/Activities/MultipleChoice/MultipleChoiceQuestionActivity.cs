using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using GameShow.GameModel;

namespace GameShow.WpfApp.Activities.MultipleChoice
{
    [DisplayName("Multiple Choice Question")]
    public class MultipleChoiceQuestionActivity : ActivityBase
    {
        public override UserControl GetShowDisplayControl()
        {
            return new GenericText.GenericTextUserControl();
        }

        public override string GetActivityType()
        {
            return "Multiple Choice Question";
        }

        public override string GetTitle()
        {
            if (!string.IsNullOrEmpty(QuestionText))
            {
                return GetActivityType() + " - " + QuestionText;
            }

            return GetActivityType();
        }

        public string QuestionText { get; set; }
        private ObservableCollection<MultipleChoiceAnswer> _answers = new ObservableCollection<MultipleChoiceAnswer>();
        public ObservableCollection<MultipleChoiceAnswer> Answers => _answers;

        public string CorrectAnswer
        {
            get
            {
                string[] answerCodes = new string[] {"A", "B", "C", "D", "E", "F", "G"};
                var cAnswer = _answers.Where(a => a.IsCorrect).FirstOrDefault();
                if (cAnswer != null)
                {
                    return answerCodes[_answers.IndexOf(cAnswer)];
                }
                return string.Empty;
            }
            set
            {
                List<string> answerCodes = new string[] {"A", "B", "C", "D", "E", "F", "G"}.ToList();
                if (answerCodes.Contains(value))
                {
                    _answers.ToList().ForEach(a => a.IsCorrect = false);
                    var answerIndex = answerCodes.IndexOf(value);
                    if (answerIndex < _answers.Count)
                    {
                        _answers[answerIndex].IsCorrect = true;
                    }
                }
            }
        }

        public string AnswerA
        {
            get { return GetAnswerText(0); }
            set { SetAnswerText(0, value); }
        }

        public string AnswerB
        {
            get { return GetAnswerText(1); }
            set { SetAnswerText(1, value); }
        }

        public string AnswerC
        {
            get { return GetAnswerText(2); }
            set { SetAnswerText(2, value); }
        }

        public string AnswerD
        {
            get { return GetAnswerText(3); }
            set { SetAnswerText(3, value); }
        }

        private string GetAnswerText(int answerIndex)
        {
            if (answerIndex < _answers.Count)
            {
                return _answers[answerIndex].AnswerText;
            }
            return null;
        }

        private void SetAnswerText(int answerIndex, string value)
        {
            while (answerIndex >= _answers.Count)
            {
                _answers.Add(new MultipleChoiceAnswer());
            }

            var answer = _answers[answerIndex];
            answer.AnswerText = value;
        }

        public override void NotifyActive()
        {
            ShowContext.Current.SetQuestionPromptsAndAnswers(QuestionText,
                _answers.Where(a => (!string.IsNullOrEmpty(a.AnswerText))).Select(a =>
                    new PromptButton() {EventType = "Answer", EventValue = _answers.IndexOf(a).ToString(), Text = a.AnswerText}));
        }
    }
}
