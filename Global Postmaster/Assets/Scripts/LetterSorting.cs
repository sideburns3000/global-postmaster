using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterSorting : MonoBehaviour
{
    // fields for attaching the corresponding UI elements in the Unity Editor:
    public Text scoreText;
    public Text progressText;
    public Text personText;
    public Text streetText;
    public Text cityText;
    public Text resultingScoreText;

    public Button buttonAnswer1;
    public Button buttonAnswer2;
    public Button buttonAnswer3;
    public Button buttonAnswer4;

    public GameObject resultsPanel;

    public int score = 0;
    public int currentQuestion = 1;
    public int numberOfQuestions = 15;

    int correctAnswer = -1;

    struct Country
    {
        public int id;
        public string name;
        public string capital;
        public string person;
        public string street;
        public string postalCode;
        public string postbagImage;

        public Country(int id, string name, string capital, string person, string street, string postalCode, string postbagImage)
        {
            this.id = id;
            this.name = name;
            this.capital = capital;
            this.person = person;
            this.street = street;
            this.postalCode = postalCode;
            this.postbagImage = postbagImage;
        }
    }

    // we just hardcode a few example countries here for testing:
    Country germany = new Country(1, "Germany", "Berlin", "Herrn Paul Müller", "Goethestraße 27", "10115", "germany_frg_3_2005_208");
    Country france = new Country(2, "France", "Paris", "Mme Marianne Dubois", "12 Rue Rimbaud", "75001", "france_3.2000.3803_1");
    Country italy = new Country(3, "Italy", "Rome", "Sig. Alessandro Volta", "Via da Vinci 19", "00118", "italy_3.2000.3829_1");
    Country norway = new Country(4, "Norway", "Oslo", "Olaf Eriksson", "Example Street 12", "12345", "norway_3.2017.1511_1");
    Country vietnam = new Country(5, "Vietnam", "Hanoi", "Nguyen Thi Lam", "Example Street 12", "12345", "vietnam_3_2010_713");
    Country antilles = new Country(6, "Netherlands Antilles", "Willemstad", "Pieter de Boer", "Example Street 12", "12345", "netherlands_antilles_3.2000.3822_1");

    // TO DO: we need more example person names, streets, and correct postal codes
    Country china = new Country(7, "China", "Beijing", "Example Person", "Example Street 12", "12345", "china_3_2008_1964");
    Country denmark = new Country(8, "Denmark", "Copenhagen", "Example Person", "Example Street 12", "12345", "denmark_3.2017.1450_1");
    Country ecuador = new Country(9, "Ecuador", "Quito", "Example Person", "Example Street 12", "12345", "ecuador_3.2019.83_1");
    Country eritrea = new Country(10, "Eritrea", "Asmara", "Example Person", "Example Street 12", "12345", "eritrea_3.2017.1594_1");
    Country estonia = new Country(11, "Estonia", "Tallinn", "Example Person", "Example Street 12", "12345", "estonia_3.2019.94_1");
    Country fiji = new Country(12, "Fiji", "Suva", "Example Person", "Example Street 12", "12345", "fiji_3_2010_756");
    Country finland = new Country(13, "Finland", "Helsinki", "Example Person", "Example Street 12", "12345", "finland_4.0.2520_1");
    Country gabon = new Country(14, "Gabon", "Libreville", "Example Person", "Example Street 12", "12345", "gabon_3.2017.1684_1");
    Country haiti = new Country(15, "Haiti", "Port-au-Prince", "Example Person", "Example Street 12", "12345", "haiti_3.2017.1677_1");
    Country ivoryCoast = new Country(16, "Ivory Coast", "Yamoussoukro", "Example Person", "Example Street 12", "12345", "ivory_coast_3.2017.1593_1");
    Country japan = new Country(17, "Japan", "Tokyo", "Example Person", "Example Street 12", "12345", "japan_3.2019.102_1");
    Country lithuania = new Country(18, "Lithuania", "Vilnius", "Example Person", "Example Street 12", "12345", "lithuania_3_2010_660");
    Country moldova = new Country(19, "Moldova", "Chisinau", "Example Person", "Example Street 12", "12345", "moldova_3.2019.96_1");
    Country namibia = new Country(20, "Namibia", "Windhoek", "Example Person", "Example Street 12", "12345", "namibia_3_2010_707");
    Country netherlands = new Country(21, "Netherlands", "Amsterdam", "Example Person", "Example Street 12", "12345", "netherlands_3_2010_763");
    Country panama = new Country(22, "Panama", "Panama City", "Example Person", "Example Street 12", "12345", "panama_3.2017.1681_1");
    Country sweden = new Country(23, "Sweden", "Stockholm", "Example Person", "Example Street 12", "12345", "sweden_3.2017.1514_1");
    Country thailand = new Country(24, "Thailand", "Bangkok", "Example Person", "Example Street 12", "12345", "thailand_3.2019.119_1");

    // this will hold the questions for one round of the quiz:
    Dictionary<int, Country> questions = new Dictionary<int, Country>();

    // note: presumably, another Dictionary ID, Country will be useful
    // (at the moment, we don't use it)
    // Dictionary<int, Country> countries = new Dictionary<int, Country>();
    // (then, think about whether to throw away the id field in the struct)

    // we just hardcode a few example resources here for testing;
    // these are the names of images in the Assets/Resources folder
    List<string> postbagImages = new List<string> { "china_3_2008_1964", "denmark_3.2017.1450_1", "ecuador_3.2019.83_1", "eritrea_3.2017.1594_1", "estonia_3.2019.94_1", "fiji_3_2010_756", "finland_4.0.2520_1", "france_3.2000.3803_1", "gabon_3.2017.1684_1", "germany_frg_3_2005_208", "haiti_3.2017.1677_1", "italy_3.2000.3829_1", "ivory_coast_3.2017.1593_1", "japan_3.2019.102_1", "lithuania_3_2010_660", "moldova_3.2019.96_1", "namibia_3_2010_707", "netherlands_3_2010_763", "netherlands_antilles_3.2000.3822_1", "norway_3.2017.1511_1", "panama_3.2017.1681_1", "sweden_3.2017.1514_1", "thailand_3.2019.119_1", "vietnam_3_2010_713" };

    System.Random random = new System.Random();

    // this will be called when the player clicks on a postbag;
    // we check whether the answer is correct or not,
    // and then we proceed to the next question 
    // or display the results of this round if it's finished
    public void CheckAnswer(int answerNumber)
    {
        if (answerNumber == correctAnswer) 
        {
            // TO DO: give a visual cue that the answer was correct,
            // and play a nice sound 
            Debug.Log("User gave the correct answer: " + answerNumber);
            score++;
            scoreText.text = score.ToString();
        }
        else
        {
            // TO DO: give a visual cue that the answer was wrong,
            // and play a 'wrong' sound (but not too annoying) 
            Debug.Log("User gave the wrong answer: " + answerNumber);
        }
        
        currentQuestion++;
        progressText.text = currentQuestion.ToString() + "/" + numberOfQuestions.ToString();
        if (currentQuestion <= numberOfQuestions)
        {
            LoadQuestion(currentQuestion);
        }
        else
        {
            resultingScoreText.text = "You scored " + score.ToString() + " out of " + numberOfQuestions.ToString() + " possible points.";
            resultsPanel.SetActive(true);
        }
    }
    
    // this loads a question, which in this case means loading
    // an address into the letter's address field, and loading
    // images of postbags into the answer buttons, so that the 
    // player can select into which postbag the letter should go;
    // the parameter 'questionKey' is used as a key into the 
    // dictionary of questions
    void LoadQuestion(int questionKey)
    {
        personText.text = questions[questionKey].person;
        streetText.text = questions[questionKey].street;
        cityText.text = questions[questionKey].postalCode + " " + questions[questionKey].capital;
        Sprite correctPostbag = Resources.Load<Sprite>(questions[questionKey].postbagImage);
        List<string> otherImages = GetRandomPostbagImages(4, questions[questionKey].postbagImage);
        // choose the button (from 1 = leftmost up to 4 = rightmost) 
        // where the correct postbag image will be displayed:
        correctAnswer = random.Next(1, 5);
        switch (correctAnswer)
        {
            case 1:
                buttonAnswer1.image.sprite = correctPostbag; 
                buttonAnswer2.image.sprite = Resources.Load<Sprite>(otherImages[0]);
                buttonAnswer3.image.sprite = Resources.Load<Sprite>(otherImages[1]);
                buttonAnswer4.image.sprite = Resources.Load<Sprite>(otherImages[2]);
                break;
            case 2:
                buttonAnswer2.image.sprite = correctPostbag;
                buttonAnswer1.image.sprite = Resources.Load<Sprite>(otherImages[0]);
                buttonAnswer3.image.sprite = Resources.Load<Sprite>(otherImages[1]);
                buttonAnswer4.image.sprite = Resources.Load<Sprite>(otherImages[2]);
                break;
            case 3:
                buttonAnswer3.image.sprite = correctPostbag;
                buttonAnswer1.image.sprite = Resources.Load<Sprite>(otherImages[0]);
                buttonAnswer2.image.sprite = Resources.Load<Sprite>(otherImages[1]);
                buttonAnswer4.image.sprite = Resources.Load<Sprite>(otherImages[2]);
                break;
            case 4:
                buttonAnswer4.image.sprite = correctPostbag;
                buttonAnswer1.image.sprite = Resources.Load<Sprite>(otherImages[0]);
                buttonAnswer2.image.sprite = Resources.Load<Sprite>(otherImages[1]);
                buttonAnswer3.image.sprite = Resources.Load<Sprite>(otherImages[2]);
                break;
            default:
                break;
        }
    }

    // get random postbag images from our resources, which we will use to
    // provide images for the wrong answer buttons;
    // but don't get duplicates and don't get the actually correct image
    List<string> GetRandomPostbagImages(int count, string postbagToExclude)
    {
        HashSet<string> randomImages = new HashSet<string>();

        for (int i = 0; i < count; i++)
        {
            while (!randomImages.Add(postbagImages[random.Next(0, postbagImages.Count)]));
        }

        // we can call the function with a higher count than we actually need,
        // and then we can safely remove the postbag which shall be excluded
        // (if it is actually present in the set);
        // we will have enough elements remaining
        randomImages.Remove(postbagToExclude);

        // we convert the hashset result to a list because we want to easily assign
        // the elements to our separate buttons without enumerating the hashset:
        List<string> resultList = new List<string>(randomImages);
        return resultList;

    }

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        currentQuestion = 1;
        scoreText.text = score.ToString();
        progressText.text = currentQuestion.ToString() + "/" + numberOfQuestions.ToString();

        questions.Clear();

        // assemble the quiz questions for this round:
        // (at the moment, we just hardcode these for testing purposes; 
        // they will be randomized later)
        questions.Add(1, france);
        questions.Add(2, italy);
        questions.Add(3, germany);
        questions.Add(4, antilles);
        questions.Add(5, norway);
        questions.Add(6, vietnam);
        questions.Add(7, china);
        questions.Add(8, moldova);
        questions.Add(9, fiji);
        questions.Add(10, lithuania);
        questions.Add(11, ivoryCoast);
        questions.Add(12, eritrea);
        questions.Add(13, gabon);
        questions.Add(14, ecuador);
        questions.Add(15, denmark);

        LoadQuestion(1);
    }

    public void BackToMainMenu()
    {
        StartCoroutine(Manager.instance.LoadAsyncScene("MainMenu"));
    }

    public void PlayAgain()
    {
        Start();
        resultsPanel.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
