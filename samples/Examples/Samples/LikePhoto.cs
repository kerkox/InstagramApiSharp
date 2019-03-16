using System;
using System.IO;
using System.Threading.Tasks;
using Examples.Utils;
using InstagramApiSharp;
using InstagramApiSharp.API;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
/////////////////////////////////////////////////////////////////////
////////////////////// IMPORTANT NOTE ///////////////////////////////
// Please check wiki pages for more information:
// https://github.com/ramtinak/InstagramApiSharp/wiki
////////////////////// IMPORTANT NOTE ///////////////////////////////
/////////////////////////////////////////////////////////////////////
namespace Examples.Samples
{
    internal class LikePhoto : IDemoSample
    {
        private readonly IInstaApi InstaApi;
        private static readonly int _maxDescriptionLength = 20;

        public LikePhoto(IInstaApi instaApi)
        {
            InstaApi = instaApi;
        }

        public async Task DoShow()
        {
            var currentUser = await InstaApi.UserProcessor.GetUserAsync("jhontobar6666");


            var currentUserMedia = await InstaApi.UserProcessor.GetUserMediaAsync(currentUser.Value.UserName, PaginationParameters.MaxPagesToLoad(1));
            if (currentUserMedia.Succeeded)
            {
                Console.WriteLine($"Media count [{currentUser.Value.UserName}]: {currentUserMedia.Value.Count}");
                foreach (var media in currentUserMedia.Value)
                {
                    var likeResult = await InstaApi.MediaProcessor.LikeMediaAsync(media.InstaIdentifier);
                    var resultString = likeResult.Value ? "liked" : "not liked";
                    Console.WriteLine($"Media {media.Code} {resultString}");
                    ConsoleUtils.PrintMedia("Self media", media, _maxDescriptionLength);
                }
                    
            }
           
        }


        public async Task DoShow2()
        {
            var currentUser = await InstaApi.GetCurrentUserAsync();


            var currentUserMedia = await InstaApi.UserProcessor.GetUserMediaAsync(currentUser.Value.UserName, PaginationParameters.MaxPagesToLoad(5));
            if (currentUserMedia.Succeeded)
            {
                Console.WriteLine($"Media count [{currentUser.Value.UserName}]: {currentUserMedia.Value.Count}");
                foreach (var media in currentUserMedia.Value)
                {
                    var likeResult = await InstaApi.MediaProcessor.LikeMediaAsync(media.InstaIdentifier);
                    var resultString = likeResult.Value ? "liked" : "not liked";
                    Console.WriteLine($"Media {media.Code} {resultString}");
                    ConsoleUtils.PrintMedia("Self media", media, _maxDescriptionLength);
                }

            }

        }

        public async Task DoShowWithProgress()
        {
            var mediaImage = new InstaImageUpload
            {
                // leave zero, if you don't know how height and width is it.
                Height = 1080,
                Width = 1080,
                Uri = @"c:\someawesomepicture.jpg"
            };
            // Add user tag (tag people)
            mediaImage.UserTags.Add(new InstaUserTagUpload
            {
                Username = "rmt4006",
                X = 0.5,
                Y = 0.5
            });
            // Upload photo with progress
            var result = await InstaApi.MediaProcessor.UploadPhotoAsync(UploadProgress, mediaImage, "someawesomepicture");
            Console.WriteLine(result.Succeeded
                ? $"Media created: {result.Value.Pk}, {result.Value.Caption}"
                : $"Unable to upload photo: {result.Info.Message}");
        }
        void UploadProgress(InstaUploaderProgress progress)
        {
            if (progress == null)
                return;
            Console.WriteLine($"{progress.Name} {progress.UploadState}");
        }
    }
}