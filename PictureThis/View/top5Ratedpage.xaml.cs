﻿//This page was primarily developed by Steven R
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using PictureThis.Model;
using Newtonsoft.Json;


namespace PictureThis.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class top5Ratedpage : ContentPage
    {

        int pictureIndex = 0;
        List<Picture> pictures;
        string json, imagesPath;
        Boolean fileFound = false;
        

        public top5Ratedpage()
        {
            InitializeComponent();
            imagesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "images.json"); //Get this later: Path that holds all of the embedded images

            //save the file to the device if it doesn't already exist
            if (!System.IO.File.Exists(imagesPath))
            {
                DisplayAlert("ALERT", "No Pictures were found. Please add pictures.", "OK");
            }
            else
            {
                fileFound = true;
                //Get the images.json
                string jsonString = System.IO.File.ReadAllText(imagesPath);

                //deserialize json into list of tags
                pictures = JsonConvert.DeserializeObject<List<Picture>>(jsonString);
                // swipedLabel.Text = "Name: " + pictures[pictureIndex].name + "\tRating: " + pictures[pictureIndex].getRating() + "\nTags: " + pictures[pictureIndex].getAllTags();
            }
            SetTopRated5();


        }

        private void SetTopRated5()
        {
            pictures = (from pic in pictures
                        orderby (pic.getRating()) descending
                        select pic).ToList();
            //display picture after query
            Box.Source = pictures.ElementAt(pictureIndex).path;
            swipedLabel.Text = "Name: " + pictures[pictureIndex].name + "\tRating: " + pictures[pictureIndex].getRating() + "\nTags: " + pictures[pictureIndex].getAllTags() + "\nThis picture is rated: " + pictureIndex;
        }
             

        void OnSwiped(object sender, SwipedEventArgs e)
        {
            if (fileFound)
            {
                //logic to update rating based on which direction the user swiped 
                //then get next picture.
                switch (e.Direction.ToString())
                {
                    case "Up":

                        break;
                    case "Right":
                        pictureIndex = (pictureIndex + 1) % pictures.Count;
                        pictureIndex = (pictureIndex ) % 5;



                        break;

                    //Remove the selected tag from the selected picture
                    case "Left":

                        if (pictureIndex == 0)
                        {
                            pictureIndex = pictures.Count() - 1;
                        }

                        pictureIndex = (pictureIndex - 1) % pictures.Count;
                        pictureIndex = (pictureIndex) % 5;


                        break;
                }


                Box.Source = pictures.ElementAt(pictureIndex).path;
                swipedLabel.Text = "Name: " + pictures[pictureIndex].name + "\tRating: " + pictures[pictureIndex].getRating() + "\nTags: " + pictures[pictureIndex].getAllTags();

                //rewrite the json file with updated rating


            }
        }//end OnSwiped

    }
}
