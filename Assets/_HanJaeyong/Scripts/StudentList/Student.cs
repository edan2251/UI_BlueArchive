public class Student
{
    public StudentData OriginData { get; private set; }
    public int CurrentLevel { get; set; }
    public bool IsFavorite { get; set; }

    public Student(StudentData data)
    {
        OriginData = data;
        CurrentLevel = data.level;
        IsFavorite = data.isFavorite;
    }

    public void ToggleFavorite()
    {
        IsFavorite = !IsFavorite;
    }
}