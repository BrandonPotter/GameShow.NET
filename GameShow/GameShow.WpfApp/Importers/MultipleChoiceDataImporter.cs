using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShow.WpfApp.Activities.MultipleChoice;

namespace GameShow.WpfApp.Importers
{
    public class MultipleChoiceDataImporter
    {
        public void ImportMultipleChoiceJson(string jsonText)
        {
            var qList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<dynamic>>(jsonText);
            foreach (var q in qList)
            {
                MultipleChoiceQuestionActivity mcq = new MultipleChoiceQuestionActivity();

                mcq.QuestionText = q.question;
                if (q.A != null)
                {
                    mcq.Answers.Add(new MultipleChoiceAnswer() {AnswerText = q.A, IsCorrect = q.answer == "A"});
                }

                if (q.B != null)
                {
                    mcq.Answers.Add(new MultipleChoiceAnswer() { AnswerText = q.B, IsCorrect = q.answer == "B" });
                }

                if (q.C != null)
                {
                    mcq.Answers.Add(new MultipleChoiceAnswer() { AnswerText = q.C, IsCorrect = q.answer == "C" });
                }

                if (q.D != null)
                {
                    mcq.Answers.Add(new MultipleChoiceAnswer() { AnswerText = q.D, IsCorrect = q.answer == "D" });
                }

                ShowContext.Current.Activities.Add(mcq);
            }
        }
    }
}
