using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using EmotionPlatzi.Web.Models;
using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using Emotion = Microsoft.ProjectOxford.Common.Contract.Emotion;

namespace EmotionPlatzi.Web.Util
{
    public class EmotionHelper
    {
        public EmotionServiceClient emoClient;

        public EmotionHelper(string key)
        {
            this.emoClient = new EmotionServiceClient(key);
        }

        public async Task<EmoPicture> DetectAndExtractFacesAsync(Stream imageSream)
        {
           Emotion[] emotions = await  emoClient.RecognizeAsync(imageSream);

            var emoPicture = new EmoPicture();
            emoPicture.Faces = ExtractFaces(emotions, emoPicture);
            return emoPicture;
        }

        private ObservableCollection<EmoFace> ExtractFaces(Emotion[] emotions,EmoPicture emoPicture)
        {
            var listFaces = new ObservableCollection<EmoFace>();
            foreach (var emotion in emotions)
            {
                var emoFace = new EmoFace()
                {
                    X = emotion.FaceRectangle.Left,
                    Y = emotion.FaceRectangle.Top,
                    Width = emotion.FaceRectangle.Width,
                    Height = emotion.FaceRectangle.Height,
                    Picture = emoPicture,
                };

                emoFace.Emotions = ProcessEmotions(emotion.Scores,emoFace);
                listFaces.Add(emoFace);
            }
            return listFaces;
        }

        private ObservableCollection<EmoEmotion> ProcessEmotions(EmotionScores scores,EmoFace emoFace)
        {
           var emotionList = new ObservableCollection<EmoEmotion>();

            var properties = scores.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var filterProperties = properties.Where(p => p.PropertyType == typeof (float));

            var emoType = EmoEmotionEnum.Undetermined;

            foreach (var prop in filterProperties)
            {
                if(!Enum.TryParse<EmoEmotionEnum>(prop.Name, out emoType))
                {
                    emoType =  EmoEmotionEnum.Undetermined;
                }
                var emoEmotion = new EmoEmotion();
                emoEmotion.Score = (float) prop.GetValue(scores);
                emoEmotion.EmotionType = emoType;
                emoEmotion.Face = emoFace;
                emotionList.Add(emoEmotion);
            }

            return emotionList;
        }
    }
}