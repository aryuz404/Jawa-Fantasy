using Firebase.Firestore;

[FirestoreData]
public struct QuestionData
{
    [FirestoreProperty]
    public string name { get; set; }

    [FirestoreProperty]
    public string question { get; set; }

    [FirestoreProperty]
    public string imageFile { get; set; }

    [FirestoreProperty]
    public string option_a { get; set; }

    [FirestoreProperty]
    public string option_b { get; set; }

    [FirestoreProperty]
    public string option_c { get; set; }

    [FirestoreProperty]
    public string option_d { get; set; }

    [FirestoreProperty]
    public string answer { get; set; }
}
